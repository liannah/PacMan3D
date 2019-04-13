using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public Wave[] waves;
   // public int size;
    public Enemy enemyPrefab;
    private Enemy enemyInstance;
    private List<Enemy> enemies = new List<Enemy>(); 
    private Maze maze;
    private Transform player;
    public Material material; 
    private Color original;
    private Wave currentWave;

    private int maximumEnemies = 2;
    private int currentWaveNumber = 0;
    public float timeBetweenSpawns = 1f;

    private float spawnTimer = 0f;

    private bool isDead;

    private void Start()
    {
        //waves = new Wave[size];
        player = FindObjectOfType<PlayerController>().transform;
        maze = FindObjectOfType<Maze>();
        NextWave();
    }

    private void Update()
    {
        if (player != null)
        {
            spawnTimer += Time.deltaTime;
            if (enemies.Count < maximumEnemies  && spawnTimer > timeBetweenSpawns)
            {
                spawnTimer = 0;
                StartCoroutine("SpawnEnemy");
            }
            // if(enemyInstance.transform.position.magnitude < (maze.MazefromPosition(Vector3.zero).position + (Vector3.up) * 3).magnitude)
            //{
            //}
        }
    }

    public void Restart()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i].gameObject);
        }
        enemies.Clear();
        spawnTimer = 0;
    }


    private IEnumerator SpawnEnemy()
    {
        float spawnDelay = 1;
        float tileFlashSpeed = 4;
        Transform randomPos = maze.GetRandomOpenMazeCoord();
        // randomPos = maze.MazefromPosition(player.position);

        Material mazeMat = randomPos.GetComponent<Renderer>().material;
        Color initialColor = mazeMat.color;
        Color flashColor = Color.red;
        float spawntimer = 0;
        while (spawntimer < spawnDelay)
        {
            mazeMat.color = Color.Lerp(initialColor, flashColor, Mathf.PingPong(spawntimer * tileFlashSpeed, 1));

            spawntimer += Time.deltaTime;

            yield return null;
        }

        enemyInstance = Instantiate(enemyPrefab, randomPos.position + Vector3.up, Quaternion.identity) as Enemy;
        enemies.Add(enemyInstance);
    }

    private void PlayerDeath()
    {
        isDead = true;
    }

    private void ResetPlayerPosition()
    {
        player.position = maze.MazefromPosition(Vector3.zero).position + (Vector3.up) * 3;
    }

    private void NextWave()
    {
        currentWaveNumber++;
        //is not entering here
        if (currentWaveNumber < waves.Length - 1)
        {
            currentWave = waves[currentWaveNumber - 1];
            maximumEnemies = currentWave.enemyCount;
            if (!isDead)
            {
                ResetPlayerPosition();
            }
        }
    }

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;

        public float timebetweenspawns;
    }
}