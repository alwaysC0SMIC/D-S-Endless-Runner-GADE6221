using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePoints : MonoBehaviour
{
    [SerializeField] GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ObstacleTrigger1"))
        {
            player.GetComponent<PlayerDodgeMovement>().addObstacleScore();
            //Debug.Log("Adding obstacle points - collider");
        }
    }
}

