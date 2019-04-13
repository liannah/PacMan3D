using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public AudioClip pick;
    public AudioClip plus;
    public AudioClip loose;
    // public float move = 10;
    private Maze maze;
    private Rigidbody rb;
    private float speed = 5;
    private Color original;
    public Material material;
    // private Vector3 speed;
    public float health;
    GameManager gameManagerInstance;
    public Vector3 velocity;

    public int pickUp;
    private MeshRenderer mesh;
    private SphereCollider sphereColl;
    private Rigidbody rigid;
    private float startingHealth;

    //public event System.Action OnDeath;

    void Start()
    {
        startingHealth = health;
        mesh = this.GetComponent<MeshRenderer>();
        sphereColl = this.GetComponent<SphereCollider>();
        rigid = this.GetComponent<Rigidbody>();
        gameManagerInstance = GetComponent<GameManager>();
        original = material.color;
        rb = GetComponent<Rigidbody>();
        Camera.main.GetComponent<CameraControl>().setTarget(gameObject.transform);
    }

    private void Update()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * speed;
        Move(moveVelocity);
    }

    public void Move(Vector3 vel)
    {
        velocity = vel; 
    }

    public void onHittingPlayer()
    {
        health--;   
        if(health == 0)
        {
            AudioManager.instance.PlaySound(loose, transform.position);
            Die();
        } 
    }

    void Die()
    {
        EnableComponents(false);

        //GameObject.Destroy(gameObject);
        //gameManagerInstance.RestartGame();
    }

    public void Respawn()
    {
        EnableComponents(true);
        rigid.velocity = Vector3.zero;
        this.transform.position = new Vector3(0, .5f, 0);
        health = startingHealth;
    }

    private void EnableComponents(bool b)
    {
        mesh.enabled = b;
        sphereColl.enabled = b;
        rigid.isKinematic = !b;
    }

    void OnTriggerEnter(Collider other)
    {
        float spawntimer = 0;
        if (other.gameObject.CompareTag("PickUp"))
        {
            AudioManager.instance.PlaySound(pick, transform.position);

            other.gameObject.SetActive(false);
            material.color = Color.Lerp(original, Color.yellow, 1f);
            spawntimer += Time.deltaTime;
            pickUp--;
        }

        if(other.tag == "Plus")
        {
            AudioManager.instance.PlaySound(plus, transform.position);

            other.gameObject.SetActive(false);
            material.color = Color.Lerp(original, Color.yellow, 1f);
            spawntimer += Time.deltaTime;
            health++;
        }
        if (other.tag == "Block")
        {
            Die();
        }
    }


        void FixedUpdate()
    {
        material.color = original;
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");
        // Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        // rb.AddForce(movement*speed);
    }
}
