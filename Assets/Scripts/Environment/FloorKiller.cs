using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorKiller : MonoBehaviour
{

    //DESTROYS OLD LEVEL SEGMENTS 
    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);   
    }
}
