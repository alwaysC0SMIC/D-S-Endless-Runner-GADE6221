using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeathParticles : MonoBehaviour
{
    private bool startDestroy = false;

    private void Update()
    {
        if (!startDestroy)
        {
            Destroy(this.gameObject, 5F);
            startDestroy = true;
        }
    }
}
