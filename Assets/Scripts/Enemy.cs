using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public AudioClip death;

    private float attackDistanceThreshold = .5f;
    private float timeBetweenAttacks = 1;
    private float damage = 1;
    private float nextAttackTime;
    private float myCollisionRadius;
    private float targetCollisionRadius;
    private bool hasTarget;
    private Rigidbody rig;
    private PlayerController targetInstance;
    private NavMeshAgent pathfinder;
    private Transform target;
    private Color original;
    public Material material;

    private void Start()
    {
        original = material.color;
        pathfinder = GetComponent<NavMeshAgent>();
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            hasTarget = true;
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetInstance = target.GetComponent<PlayerController>();
            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<SphereCollider>().radius;
            StartCoroutine(UpdatePath());
            rig = targetInstance.GetComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        if (hasTarget && targetInstance.health > 0 && targetInstance.pickUp != 0)
        {
            if (Time.time > nextAttackTime && target != null)
            {

                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                {
                    AudioManager.instance.PlaySound(death, transform.position);

                    nextAttackTime = Time.time + timeBetweenAttacks;

                    StartCoroutine(Attack());
                }
            }
        }
    }

    /* void OnTriggerEnter(Collider other)
     {
         if (other.tag == "Player")
         {
             Destroy(other.gameObject);
         }
     }*/

    private IEnumerator Attack()
    {
        pathfinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

        float attackSpeed = 3;
        float percent = 0;

        material.color = Color.red;
        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetInstance.onHittingPlayer();
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
            pathfinder.enabled = true;

            yield return null;
        }

        material.color = original;
    }

    public IEnumerator UpdatePath()
    {
        if(!pathfinder.enabled) { yield break; }
        float refreshRate = 0.25f;
        while (target != null && targetInstance.health > 0)
        {
            Vector3 targetposition = new Vector3(target.position.x, -1, target.position.z);
            pathfinder.SetDestination(targetposition);
            yield return new WaitForSeconds(refreshRate);
        }
        hasTarget = false;

    }
}