using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    //GAME SPEED VARIABLES
    public float gameSpeed = 5F;
    private float speedChangeTime = 5F;
    private float maxGameSpeed = 13F;
    private bool gameSpeedTimeractive = false;

    //GAME STATE + LEVEL VARIABLES
    public int currentLevel = 0;
    public int levelCounter = 0;

    //BOSS FIGHT VARIABLES
    public float activeBossHP = 0;
    public int numOfLevelsForBoss = 2;
    public bool bossFight = false;
    public bool bossSpawn = false;

    public bool playerIsDead = false;

    public bool levelGateSpawned = false;

    private void Start()
    {
        gameSpeed = 1F;
        playerIsDead = false;
    }

    void Update()
    {
        //Debug.Log(bossFight);
        //Debug.Log(gameSpeed + " " + Time.timeScale);
        //Debug.Log(currentLevelCounter);
        //Debug.Log(activeBossHP);

            introAcceleration();

            //SPEEDS UP THE GAME SLIGHTLY EVERY FEW SECONDS
            if (!gameSpeedTimeractive && gameSpeed < maxGameSpeed)
            {
                gameSpeedTimeractive = true;
                Invoke("IncreaseSpeed", speedChangeTime);
            }

            if (levelCounter >= numOfLevelsForBoss && !bossFight)
            {
                bossFight = true;
            }

            //DEACCELERATION FOR WHEN PLAYER DIES
            if (playerIsDead && gameSpeed > 0) {
                gameSpeed -= Time.deltaTime * 3;
            }
            if (playerIsDead && gameSpeed < 0)
            {
                gameSpeed = 0;
            }
    }

    public void introAcceleration()
    {
        if (gameSpeed < 5 && !playerIsDead)
        {
            gameSpeed += 2F * Time.deltaTime;
        }

    }

    public void IncreaseSpeed()
    {
        if (!playerIsDead)
        {
            gameSpeed += 0.075F;
            //gameSpeed += 0.1F;
            gameSpeedTimeractive = false;
        }
    }
}
