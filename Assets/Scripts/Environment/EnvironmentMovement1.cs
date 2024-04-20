using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class EnvironmentMovement : MonoBehaviour
{
    //GAME SPEED 
    public float gameSpeed = 5F;
    private World world;

    void Awake()
    {
        world = GameObject.Find("World").GetComponent<World>();
    }

    void Update()
    {
        if (world != null)
        {
            gameSpeed = world.gameSpeed;
        }

        //MOVING 
        transform.Translate(Vector3.back * Time.deltaTime * gameSpeed, Space.World);
    }

    //DESTROYS OLD LEVEL SEGMENTS 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DestroyerTrigger"))
        {
            if (this.gameObject.CompareTag("LevelSegment"))
            {
                world.levelCounter++;
            }
            Destroy(this.gameObject);
        }
    }
}
