using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    private void Update()
    {
        transform.Rotate(new Vector3(60, 30, 45) * Time.deltaTime);
    }
}
