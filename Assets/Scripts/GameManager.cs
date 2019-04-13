using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // public AudioClip mainTheme;
    //  public AudioClip menuTheme;
    public AudioClip loose;
    private AudioSource audio;
    public AudioManager manager;

    public Maze mazePrefab;
    private Maze mazeInstance;
    public PlayerController playerPrefab;
    private PlayerController playerInstance;
    private Rigidbody rigid;

    public Image fade;
    public GameObject gameOverUI;
    public GameObject clockUI;
    public GameObject winUI;
    public Text clock;
    public Text health;
    public Text particles;

    private int pickUP;
    public static float timeLeft;
    public float startingTime;
    public static int playerhealth;

    //public Enemy enemyPrefab;
    //private Enemy enemyInstance;
    public Spawn spawnEnemyPrefab;

    //contaier to delete spawn something
    private Spawn spawnEnemyInstance;

    //public Transform obstPrefab;
    // private Transform obstInstance;

    private void Start()
    {
        audio = manager.GetComponent<AudioSource>();
        //  AudioManager.instance.PlayMusic(mainTheme, 3);
        BeginGame();
        rigid = playerInstance.GetComponent<Rigidbody>();
        playerInstance.pickUp = mazeInstance.countPick;
        playerInstance.health = playerhealth;
        startingTime = timeLeft;
        if (!audio.mute)
        {
            audio.mute = false;
        }
    }

    private void Awake()
    {
        timeLeft = PlayerPrefs.GetFloat("Time");
        playerhealth = PlayerPrefs.GetInt("Player");
    }

    /*   private void Awake()
       {
           if (instance == null)
           {
               instance = this;
           }
           else if (instance != this)
           {
               Destroy(gameObject);
           }

           DontDestroyOnLoad(gameObject);
       }
       */

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        clock.text = " " + Mathf.Round(timeLeft);
        health.text = " " + playerInstance.health;
        particles.text = " " + playerInstance.pickUp;
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnMain();
        }
        if (rigid.isKinematic && winUI.activeInHierarchy == false)
        {
            StartCoroutine(Fade(Color.clear, Color.black, 0.001f));
            gameOverUI.SetActive(true);
            clockUI.SetActive(false);
            audio.mute = true;
            //AudioManager.instance.PlayMusic(loose);

            // RestartGame();
        }

        if (timeLeft < 0 && winUI.activeInHierarchy == false)
        {
            //AudioManager.instance.PlaySound(loose, transform.position);

            StartCoroutine(Fade(Color.clear, Color.black, 0.001f));
            gameOverUI.SetActive(true);
            audio.mute = true;
            clockUI.SetActive(false);
        }

        if (playerInstance.pickUp == 0 && timeLeft > 0 && !rigid.isKinematic)
        {
            // AudioManager.instance.StopAllCoroutines();
            StartCoroutine(Fade(Color.clear, Color.black, 0.001f));
            winUI.SetActive(true);
            clockUI.SetActive(false);
            audio.mute = true;
            // timeLeft = startingTime;
            //rigid.isKinematic = true;
        }
    }

    private void BeginGame()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze;
        mazeInstance.Generate();
        playerInstance = Instantiate(playerPrefab) as PlayerController;
        spawnEnemyInstance = Instantiate(spawnEnemyPrefab) as Spawn;
        audio.mute = false;
        //Transform randomPos = mazeInstance.GetRandomOpenMazeCoord();

        // enemyInstance = Instantiate(enemyPrefab, randomPos.position + Vector3.up, Quaternion.identity) as Enemy;
        //  StartCoroutine(enemyInstance.UpdatePath());
    }

    public void RestartGame()
    {
        // AudioManager.instance.StopAllCoroutines();

        Debug.Log("New game");
        StopAllCoroutines();
        fade.enabled = false;
        gameOverUI.SetActive(false);
        winUI.SetActive(false);
        mazeInstance.Regenerate();
        playerInstance.Respawn();
        spawnEnemyInstance.Restart();
        clockUI.SetActive(true);
        timeLeft = startingTime;
        playerInstance.pickUp = mazeInstance.countPick;
        audio.mute = false;
    }

    private IEnumerator Fade(Color from, Color to, float time)
    {
        fade.enabled = true;
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent = Time.deltaTime * speed;
            fade.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }

    public void ReturnMain()
    {
        SceneManager.LoadScene("Menu");
    }
}

/*  private void OnCollisionStay(Collision other)
   {
      if(other.gameObject.tag == "Block")
       {
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
       }
   }

*/