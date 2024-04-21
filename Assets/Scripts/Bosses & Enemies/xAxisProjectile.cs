using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xAxisProjectile : MonoBehaviour
{
    //ACTIVE BOSS VARIABLE
    GameObject[] boss;

    //PROJECTILE VARIABLES
    private static float projectileSpeed = 5F;
    private static float deflectSpeed = 20F;
    private bool deflected = false;

    //CAM SHAKE AND TIME RAMP VARIABLES
    private float timeRampCounter;
    private float timeRamp = 0.5F;
    private CameraShake shake;
    private bool endAttack = false;

    void Awake()
    {
        boss = GameObject.FindGameObjectsWithTag("Boss");
        shake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
    }

    void Update()
    {
        if (!deflected)
        {
            Vector3 endDestination = new Vector3(15F, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, endDestination, projectileSpeed * Time.deltaTime);
            //DESTROYS OBJECT WHEN REACHING END DESTINATION
            if (transform.position == endDestination)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            Vector3 endDestination = new Vector3(-7.5F, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, endDestination, deflectSpeed * Time.deltaTime);

            if (transform.position == endDestination)
            {
                shake.Shake();
                boss[0].GetComponent<DemonBoss>().takeDamage();
                //Debug.Log("Boss got smacked + HP: " + boss[0].GetComponent<DemonBoss>().bossHP);
                Destroy(this.gameObject);
            }
        }

        if (deflected && !endAttack)
        {

            timeRamp -= Time.deltaTime;

            Time.timeScale = timeRamp;
            if (timeRamp <= 0.3F)
            {
                timeRamp = 1F;
                Time.timeScale = 1F;
                endAttack = true;
            }

        }
    }

    private void OnDestroy()
    {
        Time.timeScale = 1F;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && this.gameObject.CompareTag("Parry"))
        {
            deflected = true;
        }
    }
}
