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
    public int currentLevelCounter = 0;

    //BOSS FIGHT VARIABLES
    public float activeBossHP = 0;
    public int numOfLevelsForBoss = 2;
    public bool bossFight = false;
    public bool bossSpawn = false;

    private void Start()
    {
        gameSpeed = 1F;
    }

    void Update()
    {
        //Debug.Log(bossFight);
        //Debug.Log(gameSpeed);
        //Debug.Log(currentLevelCounter);
        //Debug.Log(activeBossHP);
        introAcceleration();

        //SPEEDS UP THE GAME SLIGHTLY EVERY FEW SECONDS
        if (!gameSpeedTimeractive && gameSpeed < maxGameSpeed)
        {
            gameSpeedTimeractive = true;
            Invoke("IncreaseSpeed", speedChangeTime);
        }


        if (currentLevelCounter >= numOfLevelsForBoss && !bossFight) {
            bossFight = true;
        }
    }

    public void introAcceleration()
    {
        if (gameSpeed < 5)
        {
            gameSpeed += 2F * Time.deltaTime;
        }

    }

    public void IncreaseSpeed()
    {
        gameSpeed += 0.05F;
        //gameSpeed += 0.1F;
        gameSpeedTimeractive = false;
    }
}
