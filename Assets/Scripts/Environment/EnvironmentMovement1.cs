using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnvironmentMovement : MonoBehaviour
{
    //GAME SPEED 
    public float gameSpeed = 5F;

    void Update()
    {
        //MOVING 
        Vector3 dimension = new Vector3(0, 0, -1);
        transform.Translate(dimension * Time.deltaTime * gameSpeed, Space.World);
    }

    //DESTROYS OLD LEVEL SEGMENTS 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DestroyerTrigger"))
        {
            Destroy(this.gameObject);
        }
    }
}
