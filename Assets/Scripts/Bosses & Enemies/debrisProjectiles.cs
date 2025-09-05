using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class debrisProjectiles : MonoBehaviour
{
    //VARIABLES
    Vector3 target;
    private float fallSpeed = 20F;
    private float despawnTime = 10F;

    private void Awake()
    {
        Invoke("Despawn", despawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        target = new Vector3(transform.position.x, 1.5F, transform.position.z);

        if (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, fallSpeed * Time.deltaTime);
        }
    }

    private void Despawn() { 
        Destroy(gameObject);
    }
}
