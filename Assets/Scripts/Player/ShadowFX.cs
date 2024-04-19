using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFX : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.Find("PLAYER");   
        
    }

    void Update()
    {
        transform.localScale = new Vector3(Vector3.Distance(gameObject.transform.GetChild(0).position, player.transform.position)/3, Vector3.Distance(gameObject.transform.GetChild(0).position, player.transform.position) / 3, 1F);
        gameObject.transform.GetChild(0).position = new Vector3(player.transform.position.x, gameObject.transform.GetChild(0).position.y, gameObject.transform.GetChild(0).position.z);
    }


}
