using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //VARIABLES
    Vector3 offset;
    Vector3 newpos;
    public GameObject player;

    //ATTACHES CAMERA TO PLAYER OBJECT
    void Start()
    {
        offset = player.transform.position - transform.position;
    }

    void Update()
    {
        transform.position = player.transform.position - offset;
    }
}
