using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDodgeMovement : MonoBehaviour
{
    //UI OBJECTS
    [SerializeField] private GameObject deathScreenUI;
    [SerializeField] private GameObject pauseScreenUI;

    //PLAYER SPEEDS
    public float moveSpeed = 30;
    public int horizontalMoveSpeed = 100;

    //PLAYER SCORE
    public float playerScore = 0F;
    //private float scoreMultiplier = 0F;

    //RIGIDBODY
    private Rigidbody rigid;

    //LANE VARIABLES
    public bool lane1 = false;
    public static float lane1co = -2F;
    public bool lane2 = true;
    public static float lane2co = 0F;
    public bool lane3 = false;
    public static float lane3co = 2F;
    public int laneNum = 2;

    //ACTIVE MOVEMENT BOOLS
    bool movingL1 = false;
    bool movingL2 = false;
    bool movingL3 = false;

    //PICKUP BOOLS
    public bool invincible = false;
    private float invincibleTimePeriod = 5F;

    //MESH VARIABLES
    [SerializeField] GameObject playerModel;
    public Material playerMaterial;
    public Material invMaterial;

    //BOSS RELATED VARIABLES
    public bool attackingBoss = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        playerScore = 0;
    }

    void Update()
    {
        //Debug.Log(playerScore);
        //MOVING FORWARD
        Vector3 stay = new Vector3(0, 0, 0);
        transform.Translate(stay * Time.deltaTime * moveSpeed, Space.World);

        //UPDATING LANE AND HEIGHT
        Vector3 lane1Pos = new Vector3(lane1co, transform.position.y, transform.position.z);
        Vector3 lane2Pos = new Vector3(lane2co, transform.position.y, transform.position.z);
        Vector3 lane3Pos = new Vector3(lane3co, transform.position.y, transform.position.z);

        //===KEY INPUTS:===
        //===LEFT SIDE===
        //MIDDLE TO LEFT
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && lane2 && lane1 == false)
        {
            movingL1 = true;
            lane2 = false;
            lane1 = true;
            lane3 = false;
            laneNum = 1;
        }
        //RIGHT TO MIDDLE
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && lane3 && lane2 == false)
        {
            movingL2 = true;
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
            lane1 = false;
            lane2 = false;
            lane3 = true;
            laneNum = 3;
        }
        //LEFT TO MIDDLE
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && lane1 && lane2 == false)
        {
            movingL2 = true;
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

        //FOR PAUSING GAME
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GamePause();
        }
    }


    //FOR WHEN HITTING HAZARD OR PICKUP
    void OnTriggerEnter(Collider other)
    {
        //PLAYER DIES WHEN HITTING OBSTACLE
        if ((other.gameObject.CompareTag("ObstacleTrigger") || other.gameObject.CompareTag("ObstacleTrigger1")) && invincible == false)
        {
            PlayerDeath();  
        }

        //INVINCIBILITY PICKUP
        if (other.gameObject.CompareTag("InvincibilityTrigger"))
        {
            addPickUpScore();
            StopCoroutine("InvincibilityTimer");
            StartCoroutine("InvincibilityTimer");
        }

        //BOSS PROJECTILE
        //if (other.gameObject.CompareTag("BossInjureTrigger"))
        //{ 
        //    other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.left*20F;
        //    if (other.gameObject.GetComponent<Rigidbody>().position.x == -10F) {
        //        attackingBoss = true;
        //    }
        //}
    }

    public void addObstacleScore() {
        playerScore += 5;
        //Debug.Log("Added obstacle score");
    }

    public void addPickUpScore() {
        playerScore += 10;
        //Debug.Log("Added pickup score");
    }

    //STOPS TIME + MOVEMENT AND DISPLAYS DEATH SCREEN
    public void PlayerDeath() {
        horizontalMoveSpeed = 0;
        Time.timeScale = 0;
        deathScreenUI.SetActive(true);

        //DEACCELERATION CODE
        //float deacceleration = 1F;
        //if (environment.gameSpeed > 0)
        //{
        //    environment.gameSpeed -= deacceleration * Time.deltaTime;
        //    //Debug.Log(moveSpeed);
        //}
    }

    public void GamePause() {
        Time.timeScale = 0;
        pauseScreenUI.SetActive(true);
    }

    public void GameResume() {
        Time.timeScale = 1;
        pauseScreenUI.SetActive(false);
    }

    public void RestartRun() {
        deathScreenUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public IEnumerator InvincibilityTimer()
    {
        invincible = true;
        playerModel.GetComponent<MeshRenderer>().material = invMaterial;

        yield return new WaitForSeconds(invincibleTimePeriod);

        invincible = false;
        playerModel.GetComponent<MeshRenderer>().material = playerMaterial;
    }
}


