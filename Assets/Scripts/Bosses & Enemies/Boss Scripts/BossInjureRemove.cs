using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInjureRemove : MonoBehaviour
{
    [SerializeField] DemonBoss db;
    float bossHp;
    void Start()
    {
        if (db != null) { 
            bossHp = db.bossHP;
        }
    }

    void Update()
    {
            if (transform.position.x <= -10F)
            {
                db.bossHP = db.bossHP - 10;
                Debug.Log(db.bossHP);
                Destroy(this.gameObject);
            } 
    }
}
