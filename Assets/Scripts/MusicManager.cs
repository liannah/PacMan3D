using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public AudioClip mainTheme;
    public AudioClip inter;
    PlayerController player;
    Rigidbody ribid;

    private void Start()
    {
     //   ribid = player.GetComponent<Rigidbody>();
    }


    private void Update()
    {
       /* if (ribid.isKinematic)
        {
            AudioManager.instance.PlayMusic(inter, 1);
        }
        */
    }
}
