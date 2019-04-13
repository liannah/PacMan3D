using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject optionMenu;
    int playerhealth;
    float timeleft;
    AudioSource source;
    private GameManager game;

   /* private void Start()
    {
        game = GetComponent<GameManager>();
    }
    */

     private void Start()
    {
        source = this.GetComponent<AudioSource>();
    }

    public void Play()
    {
        SceneManager.LoadScene("Lab");
    }

    public void Options()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void Back()
    {
        optionMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Advance()
    {
        playerhealth = 20;
        timeleft = 40f;
        PlayerPrefs.SetInt("Player", playerhealth);
        PlayerPrefs.SetFloat("Time", timeleft);
        PlayerPrefs.Save();
    }

    public void Inter()
    {
        playerhealth = 30;
        timeleft = 50f;
        PlayerPrefs.SetInt("Player", playerhealth);
        PlayerPrefs.SetFloat("Time", timeleft);
        PlayerPrefs.Save();
    }

    public void Easy()
    {
        playerhealth = 50;
        timeleft = 50f;
        PlayerPrefs.SetInt("Player", playerhealth);
        PlayerPrefs.SetFloat("Time", timeleft);
        PlayerPrefs.Save();
    }
}
