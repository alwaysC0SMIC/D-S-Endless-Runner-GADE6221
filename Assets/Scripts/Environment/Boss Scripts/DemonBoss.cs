using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBoss : MonoBehaviour
{
    public float bossHP = 50;

    private void Start()
    {
        bossHP = 50F;
    }

    void Update()
    {
        //STARTING MOVEMENT
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0F, 9F), 5F * Time.deltaTime);
        
    }
}
