using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SegmentTrigger : MonoBehaviour
{
    //WORLD VARIABLES
    World world;

    //LEVEL VARIABLES
    private Vector3 levelSpawnPos = new Vector3(0, 0, 67.5F);   //private Vector3 levelSpawnPos = new Vector3(1.66893e-06F, -8.076429e-06F, 68.50555F);
    private int levelIndex = 0;
    [SerializeField] GameObject[] levelSegments;
    [SerializeField] GameObject[] bossLevelSegments;
    [SerializeField] GameObject[] levelGates;
    private GameObject currentLevel;

    //OBSTACLES VARIABLES
    private GameObject[] currentObs;
    public GameObject[] level0DodgeObs;
    private bool[] rowFilled = new bool[34];
    private float[] zPosition = new float[34];

    //PICKUP VARIABLES
    public GameObject[] pickupItem;

    //DISTANCE VARIABLES
    [SerializeField] DistanceCounter distCounter;
    public int totalDistance;

    //ENEMY VARIABLES
    [SerializeField] GameObject[] enemies;

    //BOSS VARIABLES
    public bool bossMode = false;
    public bool bossSpawn = false;
    private GameObject currentBoss;
    [SerializeField] GameObject[] bosses;
    [SerializeField] GameObject bossSpawner;
    [SerializeField] GameObject bossUI;

    //GETS POSITIONS OF WHERE NEXT LEVEL SEGMENT WILL SPAWN IN ORDER TO CREATE OBSTACLES IN RIGHT POSITION
    void Start()
    {
        int i = 0;
        float startingVal = 50F;  //46.34225F;
        float rowDistance = 2F;

        while (i < 34)
        {   
            zPosition[i] = startingVal;
            startingVal += rowDistance;
            i++;
        }
        //Array.ForEach(zPosition, i => Debug.Log(i));

        //Current level and object setters, need to change it based off of conditions later
        currentObs = level0DodgeObs;    //Objects stay as array
        currentLevel = levelSegments[levelIndex];   //Levels will stay as single objects (unless you want variation)

        world = GameObject.Find("World").GetComponent<World>();
    }

    void Update()
    {
        if (world != null)
        {
            bossMode = world.bossFight;
            levelIndex = world.currentLevel;
            //Debug.Log(bossMode);
        }

        //RESETS LEVEL ARRAY IF YOU REACH THE END OF IT
        if (world.currentLevel == levelSegments.Length)
        {
            Debug.Log("You reached the end of levels, restarting cycle");
            world.currentLevel = 0;
        }
        

        if (distCounter != null) {
            totalDistance = Mathf.RoundToInt(distCounter.distance);
        }
        //Debug.Log(distCounter.distance);
    }

    //INSTANTIATES NEW LEVEL SEGMENTS
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BuilderTrigger"))
        {
            //Use this to figure out spacing between levels
            //Time.timeScale = 0;

            //LEVEL + OBSTACLE GENERATION
            if (!bossMode)
            {
                //NORMAL LEVEL GENERATION
                Instantiate(GetLevelSegment(levelIndex), levelSpawnPos, Quaternion.Euler(0, 180, 0));

                //NORMAL OBSTACLE GENERATION
                generateRows(rowFilled);
                fillRows(getObstaclesForLevel(levelIndex), zPosition);
                addPickUps(pickupItem, false);
                addEnemies(enemies, zPosition);
                levelGateSpawn();

                resetRows();
                //Array.ForEach(rowFilled, i => Debug.Log(i));
                //Debug.Log(rowFilled);
            }
            else 
            {
                //LEVEL/OBSTACLE GENERATION IF THERE'S A BOSS FIGHT
                Instantiate(GetBossLevelSegment(levelIndex), levelSpawnPos, Quaternion.Euler(0, 180, 0));
                addPickUps(pickupItem, true);
                resetRows();

                //SPAWNS IN BOSS
                if (!world.bossSpawn)
                {
                    Instantiate(bossSpawner, new Vector3(1.66893e-06F, -8.076429e-06F, 52F), Quaternion.Euler(0, 0, 0));
                    world.bossSpawn = true;
                }    
            }
        }
        //TRIGGERS BOSS TO APPEAR AND DESTROYS TRIGGER
        if (other.gameObject.CompareTag("BossTrigger")) 
        {
            Instantiate(GetBoss(levelIndex), new Vector3(-10, 0, 10), Quaternion.Euler(0, 90, 0));
            bossUI.SetActive(true);
            Destroy(other.gameObject);
        }
    }

    //FILLS THE ROW BOOLEAN WITH CHOSEN OBSTACLE ROWS
    public void generateRows(bool[] rowFilled) {
        //OBSTACLE GENERATION
        int i = 2;

        while (i < 34)
        {
            rowFilled[i] = true;
            int randomNumber = UnityEngine.Random.Range(4, 8);

            i += randomNumber;
            //Debug.Log(i);  
            //Debug.Log(randomNumber + " -> " + i);
        }
    }

    //INSTANTIATES OBSTACLES IN CHOSEN ROWSs
    public void fillRows(GameObject[] obstacle, float[] zPosition) {
        int i = 0;
        int randomNumber = UnityEngine.Random.Range(0, obstacle.Length);
        while (i < 34)
        {
            if (rowFilled[i])
            {
                Instantiate(obstacle[randomNumber], new Vector3(0F, 1.5F, zPosition[i]), Quaternion.Euler(90, 0, 0));
            }
            i++;
            randomNumber = UnityEngine.Random.Range(0, obstacle.Length);
        }
    }

    public void addPickUps(GameObject[] pickUp, bool boss) {
        int i = 1;
        int minDistance = 5;
        int maxDistance = 10;

        if (boss)
        {
            minDistance = 15;
            maxDistance = 30;
        }

        int distanceBetween = UnityEngine.Random.Range(minDistance, maxDistance);
        int position = UnityEngine.Random.Range(-1, 2) * 2;

        while (i < 33)
        {
            if (!rowFilled[i] && !rowFilled[i+1] && !rowFilled[i - 1]) {
                int itemIndex = UnityEngine.Random.Range(0, pickupItem.Length);
                Instantiate(pickUp[itemIndex], new Vector3(position, 1.5F, zPosition[i]), Quaternion.Euler(90, 0, 0));
                //rowFilled[i] = true;
            }
            
            i += distanceBetween;
            distanceBetween = UnityEngine.Random.Range(minDistance, maxDistance);

            position = UnityEngine.Random.Range(-1, 2) * 2;
        }
    }

    public void addEnemies(GameObject[] enemies, float[] zPosition)
    {
        int i = 1;
        while (i < 32)
        {
            if (!rowFilled[i] && !rowFilled[i + 1] && !rowFilled[i - 1] && !rowFilled[i + 2] && !rowFilled[i - 2])
            {
                Instantiate(enemies[0], new Vector3(0, 0F, zPosition[i]), Quaternion.Euler(0, 0, 0));
                rowFilled[i] = true;
            }
            i++;
        }
    }

    public void levelGateSpawn() {
        if (!world.levelGateSpawned) {
            //GENERATE LEVEL GATE PREFAB
            Instantiate(levelGates[0], new Vector3(0F, 0F, 54F), Quaternion.Euler(0, 0, 0));
            world.levelGateSpawned = true;
        }
    }

    //RESETS THE ROWS FOR THE NEXT INSTANTIATION
    public void resetRows() {
        int i = 0;
        while (i < 34)
        {
            rowFilled[i] = false;
            i++;
        }
    }

    //METHODS TO GET LEVEL INDEX FROM ARRAYS
    public GameObject GetLevelSegment(int levelIndex)
    {
        GameObject selectedLevel = null;
        switch (levelIndex)
        {
            case 0:
                selectedLevel = levelSegments[0];   //LEVEL 1
                break;
                case 1:
                selectedLevel = levelSegments[1]; //LEVEL 2
                break;
            default:
                selectedLevel = levelSegments[0];
                break;
        }
        return selectedLevel;
    }

    public GameObject[] getObstaclesForLevel(int levelIndex) {
        switch (levelIndex) {

            case 0:
                currentObs = level0DodgeObs;    //SET TO LEVEL 1
                break;
            default:
                currentObs = level0DodgeObs;    //LEVEL 0 BY DEFAULT
                break;
        }
        return currentObs;
    }

    public GameObject GetBossLevelSegment(int levelIndex)
    {
        GameObject selectedBossLevel;
        switch (levelIndex)
        {
            case 0:
                selectedBossLevel = bossLevelSegments[0];
                break;
            case 1:
                selectedBossLevel = bossLevelSegments[1];
                break;
            default:
                selectedBossLevel = bossLevelSegments[0];
                break;
        }
        return selectedBossLevel;
    }

    public GameObject GetBoss(int levelIndex)
    {
        GameObject boss = null;
        switch (levelIndex)
        {
            case 0:
                boss = bosses[0];   //BOSS 1
                break;
            default:
                boss = bosses[0];   //DEMON BOY BY DEFAULT
                break;
        }
        return boss;
    }
}
