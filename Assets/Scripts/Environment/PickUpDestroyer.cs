using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpDestroyer : MonoBehaviour
{
    //DESTROYS ITEMS IF INTERACTED WITH
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }   
    }
}
