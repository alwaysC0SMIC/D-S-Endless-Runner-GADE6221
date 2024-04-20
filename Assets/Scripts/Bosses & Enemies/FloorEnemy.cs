using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorEnemy : MonoBehaviour
{
    private bool enemyAttack = false;
    private bool isAlive = true;

    public float movementSpeed = 1;
    private bool movingLeft = true;
    private bool movingRight = false;

    PlayerDodgeMovement pdm;
    private ParticleSystem bloodParticles;

    //CAM SHAKE AND TIME RAMP VARIABLES
    private float timeRampCounter;
    private float timeRamp = 0.5F;
    private CameraShake shake;
    private bool endAttack = false;

    private void Awake()
    {
        pdm = GameObject.Find("PLAYER").GetComponent<PlayerDodgeMovement>();
        bloodParticles = GetComponentInChildren<ParticleSystem>();

        shake = GameObject.Find("ISO CAMERA").GetComponent<CameraShake>();

        int moveSpeed = UnityEngine.Random.Range(1, 5);
        movementSpeed = moveSpeed;
    }

    void Update()
    {
        //Debug.Log(Time.timeScale);

        if (isAlive)
        {
            enemyAttack = this.transform.GetChild(1).gameObject.GetComponent<EnemyAttack>().attackPlayer;

            Vector3 leftPos = new Vector3(-2, transform.position.y, transform.position.z);
            Vector3 rightPos = new Vector3(2, transform.position.y, transform.position.z);

            if (transform.position == rightPos)
            {
                movingLeft = true;
                movingRight = false;
            }

            if (transform.position == leftPos)
            {
                movingLeft = false;
                movingRight = true;
            }

            if (movingRight)
            {
                transform.position = Vector3.MoveTowards(transform.position, rightPos, movementSpeed * Time.deltaTime);
            }

            if (movingLeft)
            {
                transform.position = Vector3.MoveTowards(transform.position, leftPos, movementSpeed * Time.deltaTime);
            }
        }
        else {
            //TIME RAMP
            if (!pdm.isPaused && !endAttack) {
               
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !enemyAttack)
        {
            //Debug.Log("Enemy stamped");

            if (pdm != null)
            {
                pdm.addScore(2);
            }
            isAlive = false;
            shake.Shake();
            //DISABLES MESH AND COLLIDER AND STARTS TIMER FOR DESPAWN WHILE PLAYING BLOOD SPLATTER
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.transform.GetChild(1).gameObject.GetComponent<Collider>().enabled = false;

            //CHANGES PARTICLE SPRAY DIRECTION
            if (other.gameObject.transform.position.x > transform.position.x)
            {
                bloodParticles.Play();
            }
            else {
                //CHANGE DIRECTION OF BLOOD SPRAY
                gameObject.transform.rotation = new Quaternion(gameObject.transform.rotation.x, gameObject.transform.rotation.y+180, gameObject.transform.rotation.z, gameObject.transform.rotation.w);
                bloodParticles.Play();
            }

            Invoke("Despawn", 5F);
        }
    }

    private void Despawn() {
        Destroy(this.gameObject);
    }

}
