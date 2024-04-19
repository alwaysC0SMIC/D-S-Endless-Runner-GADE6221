using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDodgeMovement : MonoBehaviour
{
    //WORLD VARIABLE
    //World world;

    public bool isPaused = false;
    private bool hasStopped = false;
    private CameraShake shake;

    //UI OBJECTS
    [SerializeField] private GameObject deathScreenUI;
    [SerializeField] private GameObject pauseScreenUI;

    //PLAYER SPEEDS
    private float moveSpeed = 30;
    private float horizontalMoveSpeed = 20;
    private float deathSlowDown = 1F;

    //PLAYER SCORE
    public float playerScore = 0F;
    //private float scoreMultiplier = 0F; [FOR FUTURE]

    //RIGIDBODY
    private Rigidbody rigid;

    //PLAYER LOCK
    private bool playerUnlock = false;

    //LANE VARIABLES
    private bool lane1 = false;
    private static float lane1co = -2F;
    private bool lane2 = true;
    private static float lane2co = 0F;
    private bool lane3 = false;
    private static float lane3co = 2F;
    public int laneNum = 2;

    //ACTIVE MOVEMENT BOOLS
    private bool movingL1 = false;
    private bool movingL2 = false;
    private bool movingL3 = false;

    //PLAYER HP
    private int playerHP = 2;
    private int maxPlayerHP = 2;
    private float healthRegenerationTimePeriod = 20F;
    private bool damage = false;
    private bool dmgInv = false;

    //PICKUPS
    public bool invincible = false;
    private float invincibleTimePeriod = 5F;

    //MESH + MATERIAL VARIABLES
    [SerializeField] GameObject playerModel;
    public Material playerMaterial;
    public Material invMaterial;
    public Material damageMaterial;
    private Material currentMaterial;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        playerScore = 0;
        currentMaterial = playerMaterial;
        //UNLOCKS PLAYER MOVEMENT AFTER EXITING HOUSE
        Invoke("unlockPlayerMovement", 2);
        shake = GameObject.Find("ISO CAMERA").GetComponent<CameraShake>();
        //world = GameObject.Find("World").GetComponent<World>();
    }

    void Update()
    {
        //Debug.Log(playerHP);

        //UPDATES CHARACTER'S MATERIAL
        playerModel.GetComponent<MeshRenderer>().material = currentMaterial;

        if (invincible)
        {
            currentMaterial = invMaterial;
        }
        else if (damage)
        {
            currentMaterial = damageMaterial;
        }
        else if (!damage && !invincible)
        {
            currentMaterial = playerMaterial;
        }

        //MOVING FORWARD
        Vector3 stay = new Vector3(0, 0, 0);
        transform.Translate(stay * Time.deltaTime * moveSpeed, Space.World);

        //UPDATING LANE AND HEIGHT
        Vector3 lane1Pos = new Vector3(lane1co, transform.position.y, transform.position.z);
        Vector3 lane2Pos = new Vector3(lane2co, transform.position.y, transform.position.z);
        Vector3 lane3Pos = new Vector3(lane3co, transform.position.y, transform.position.z);

        if (playerUnlock)
        {
        //===KEY INPUTS:===
        //===LEFT SIDE===
        //MIDDLE TO LEFT
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && lane2 && lane1 == false)
        {
            movingL1 = true;
            
            movingL2 = false;
            movingL3 = false;

            lane2 = false;
            lane1 = true;
            lane3 = false;
            laneNum = 1;
        }
        //RIGHT TO MIDDLE
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && lane3 && lane2 == false)
        {
            movingL2 = true;

            movingL1 = false;
            movingL3 = false;

            lane1 = false;
            lane2 = true;
            lane3 = false;
            laneNum = 2;
            }

        //===RIGHT SIDE===
        //MIDDLE TO RIGHT
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && lane2 && lane3 == false)
        {
            movingL3 = true;

            movingL2 = false;
            movingL1 = false;

            lane1 = false;
            lane2 = false;
            lane3 = true;
            laneNum = 3;
            }
        //LEFT TO MIDDLE
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && lane1 && lane2 == false)
        {
            movingL2 = true;

            movingL1 = false;
            movingL3 = false;

            lane2 = true;
            lane1 = false;
            lane3 = false;
            laneNum = 2;
            }

        //===ACTIONS===
        if (movingL1)
        {
            transform.position = Vector3.MoveTowards(transform.position, lane1Pos, horizontalMoveSpeed * Time.deltaTime);
            if (transform.position == lane1Pos)
            {
                movingL1 = false;
            }
        }
        else if (movingL2)
        {
            transform.position = Vector3.MoveTowards(transform.position, lane2Pos, horizontalMoveSpeed * Time.deltaTime);
            if (transform.position == lane2Pos)
            {
                movingL2 = false;
            }
        }
        else if (movingL3)
        {
            transform.position = Vector3.MoveTowards(transform.position, lane3Pos, horizontalMoveSpeed * Time.deltaTime);
            if (transform.position == lane3Pos)
            {
                movingL3 = false;
            }
        }
        }
        //FOR PAUSING GAME
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GamePause();
        }

        //PLAYER HP CHECK
        if (playerHP <= 0) {
            PlayerDeath();

            if (!hasStopped)
            {
                deathSlowDown -= Time.deltaTime * 2;
                Time.timeScale = deathSlowDown;

                if (deathSlowDown <= 0.005F)
                {
                    Time.timeScale = 0;
                    hasStopped = true;

                }
            }
        }

    }

    //FOR INTERACTING WITH COLLIDERS IN-GAME
    void OnTriggerEnter(Collider other)
    {
        //PLAYER TAKES DAMAGE WHEN HITTING OBSTACLE
        if ((other.gameObject.CompareTag("ObstacleTrigger") || other.gameObject.CompareTag("ObstacleTrigger1")) && !invincible && !dmgInv)
        {
            //DAMAGES PLAYER + STARTS HP REGENERATION
            damagePlayer();
            Invoke("healDamage", healthRegenerationTimePeriod);

            //TEMPORARY INVINCIBILITY AFTER TAKING DAMAGE
            dmgInv = true;
            Invoke("endDamageInvincibility", 1);
        }

        //INVINCIBILITY PICKUP
        if (other.gameObject.CompareTag("InvincibilityTrigger"))
        {
            addPickUpScore();
                activateInvincibility();
                Invoke("deactivateInvincibility", invincibleTimePeriod);
            
            //StopCoroutine("InvincibilityTimer");
            //StartCoroutine("InvincibilityTimer");
        }
        //STACKS INVINCIBILITY TIME SHOULD ANOTHER ONE BE ACTIVATED
        if (other.gameObject.CompareTag("InvincibilityTrigger") && invincible)
        {
            addPickUpScore();
            CancelInvoke("deactivateInvincibility");
            activateInvincibility();
            Invoke("deactivateInvincibility", invincibleTimePeriod);
        }

        //ACTIVATES WHEN ACTIVATING HEALTH PICKUP
        if (other.gameObject.CompareTag("HealthTrigger"))
        {
            addPickUpScore();
            if (playerHP < maxPlayerHP)
            {
                healDamage();
                CancelInvoke("healDamage");
            }
        }
    }

    private void unlockPlayerMovement() {
        playerUnlock = true;
    }

    public void addObstacleScore() {
        playerScore += 5;
    }

    public void addPickUpScore() {
        playerScore += 10;
    }

    public void addEnemyScore() {
        playerScore += 15;
    }

    public void addBossScore() {
        playerScore += 100;
    }

    public void damagePlayer() {
        playerHP--;
        damage = true; 
        shake.Shake();
    }

    //REGENERATES HP
    public void healDamage() {
        damage = false;
        playerHP++;
    }

    public void endDamageInvincibility()
    {
        dmgInv = false;
    }

    //INVINCIBILITY ITEM
    public void activateInvincibility() {
        invincible = true;
    }

    public void deactivateInvincibility() {
        invincible = false;
    }

    //STOPS TIME + MOVEMENT AND DISPLAYS DEATH SCREEN
    public void PlayerDeath() {
        isPaused = true;
        Time.timeScale = 0;
        deathScreenUI.SetActive(true);
    }

    //PAUSING/RESTARTING GAME:
    public void GamePause() {
        isPaused = true;
        CancelInvoke("Shake");
        Time.timeScale = 0;
        pauseScreenUI.SetActive(true);
    }

    public void GameResume() {
        isPaused = false;
        Time.timeScale = 1;
        pauseScreenUI.SetActive(false);
    }

    public void RestartRun() {
        CancelInvoke("Shake");
        deathScreenUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isPaused = false;
        Time.timeScale = 1;
        playerHP = 2;
    }

    public void ExitToMainMenu() {
        SceneManager.LoadSceneAsync(sceneName: "MainMenu");
        Time.timeScale = 1;
    }

    //COROUTINES [NO LONGER NEEDED]:
    //public IEnumerator InvincibilityTimer()
    //{
    //    invincible = true;
    //    playerModel.GetComponent<MeshRenderer>().material = invMaterial;

    //    yield return new WaitForSeconds(invincibleTimePeriod);

    //    invincible = false;
    //    playerModel.GetComponent<MeshRenderer>().material = playerMaterial;
    //}

    //public IEnumerator HealthRegenerationTimer()
    //{
    //    invincible = true;
    //    playerModel.GetComponent<MeshRenderer>().material = damageMaterial;

    //    yield return new WaitForSeconds(healthRegenerationTimePeriod);

    //    playerHP++;
    //    playerModel.GetComponent<MeshRenderer>().material = playerMaterial;
    //}
}


