using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodRaysFX : MonoBehaviour
{
    private Vector3 target;
    private ParticleSystem particles;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z <= -30F) {
            ResetPosition();
        }    
    }

    private void ResetPosition() {
        float zOffset = Random.Range(15, 51);
        transform.position = new Vector3(transform.position.x, transform.position.y, zOffset);
    }
}
