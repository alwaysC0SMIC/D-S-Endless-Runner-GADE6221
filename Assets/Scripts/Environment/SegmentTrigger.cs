using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SegmentTrigger : MonoBehaviour
{
    //WORLD VARIABLES
    World world;

    //LEVEL VARIABLES
    private Transform endOfCurrentLevel;

    private int levelIndex = 0;
    [SerializeField] GameObject[] levelSegments;
    [SerializeField] GameObject[] bossLevelSegments;
    [SerializeField] GameObject[] levelGates;
    private GameObject currentLevel;
    private bool levelHasSpawned = false;

    //OBSTACLES VARIABLES
    private GameObject[] currentObs;
    public GameObject[] level0DodgeObs;
    public GameObject[] grassDodgeObs;

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
        float startingVal = 30F;  //46.34225F;  was 50
        float rowDistance = 2F;

        while (i < 30)
        {   
            zPosition[i] = startingVal;
            startingVal += rowDistance;
            i++;
        }
        //Array.ForEach(zPosition, i => Debug.Log(i));

        //Current level and object setters, need to change it based off of conditions later
        currentObs = level0DodgeObs;    //Objects stay as array
        currentLevel = levelSegments[levelIndex];   //Levels will stay as single objects (unless you want variation)

        world = GameObject.FindGameObjectWithTag("World").GetComponent<World>();
    }

    void Update()
    {
        if (world != null)
        {
            bossMode = world.bossFight;
            levelIndex = world.currentLevel;
            //Debug.Log(bossMode);
        }

        //GETS LEVEL TAIL TO SPAWN IN NEXT LEVEL
        endOfCurrentLevel = GameObject.FindGameObjectWithTag("LevelTail").transform;
        //Debug.Log(endOfCurrentLevel.position.z);

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
        if (other.gameObject.CompareTag("BuilderTrigger") && !levelHasSpawned)
        {
            //Use this to figure out spacing between levels
            //Time.timeScale = 0;

            //Debug.Log("Spawning in level");

            //LEVEL + OBSTACLE GENERATION
            if (!bossMode)
            {
                //NORMAL LEVEL GENERATION
                Instantiate(GetLevelSegment(levelIndex), new Vector3(endOfCurrentLevel.position.x, endOfCurrentLevel.position.y, endOfCurrentLevel.position.z+30), Quaternion.Euler(0, 0, 0));
                Destroy(endOfCurrentLevel.gameObject);

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
                Instantiate(GetBossLevelSegment(levelIndex), new Vector3(endOfCurrentLevel.position.x, endOfCurrentLevel.position.y, endOfCurrentLevel.position.z+30), Quaternion.Euler(0, 0, 0));
                Destroy(endOfCurrentLevel.gameObject);

                addPickUps(pickupItem, true);
                resetRows();

                //SPAWNS IN BOSS
                if (!world.bossSpawn)
                {
                    Instantiate(bossSpawner, new Vector3(0F, 0F, 32F), Quaternion.Euler(0, 0, 0));
                    world.bossSpawn = true;
                }    
            }
            
            levelHasSpawned = true;
            Invoke("resetSpawnBuffer", 0.1F);
        }
        //TRIGGERS BOSS TO APPEAR AND DESTROYS TRIGGER
        if (other.gameObject.CompareTag("BossTrigger")) 
        {
            Instantiate(GetBoss(levelIndex), new Vector3(-10, 4, 10), Quaternion.Euler(0, 90+45, 0));
            bossUI.SetActive(true);
            Destroy(other.gameObject);
        }
    }

    //FILLS THE ROW BOOLEAN WITH CHOSEN OBSTACLE ROWS
    public void generateRows(bool[] rowFilled) {
        //OBSTACLE GENERATION
        int i = 2;  

        while (i < 30)
        {
            rowFilled[i] = true;
            int randomNumber = UnityEngine.Random.Range(5, 8);  //ORIGINAL RANGE: 4, 8

            i += randomNumber;
            //Debug.Log(i);  
            //Debug.Log(randomNumber + " -> " + i);
        }
    }

    //INSTANTIATES OBSTACLES IN CHOSEN ROWSs
    public void fillRows(GameObject[] obstacle, float[] zPosition) {
        int i = 2; //(Initial offset)
        int randomNumber = UnityEngine.Random.Range(0, obstacle.Length);
        while (i < 30)
        {
            if (rowFilled[i])
            {
                Instantiate(obstacle[randomNumber], new Vector3(0F, 0.5F, zPosition[i]), Quaternion.Euler(0, 0, 0));
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

        while (i < 29)
        {
            if (!rowFilled[i] && !rowFilled[i+1] && !rowFilled[i - 1]) {
                int itemIndex = UnityEngine.Random.Range(0, pickupItem.Length);
                Instantiate(pickUp[itemIndex], new Vector3(position, 1F, zPosition[i]), Quaternion.Euler(0, 0, 0));
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
        while (i < 30)
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
            Instantiate(levelGates[0], new Vector3(0F, 0F, 34F), Quaternion.Euler(0, 0, 0));
            world.levelGateSpawned = true;
        }
    }

    //RESETS THE ROWS FOR THE NEXT INSTANTIATION
    public void resetRows() {
        int i = 0;
        while (i < 30)
        {
            rowFilled[i] = false;
            i++;
        }
    }

    private void resetSpawnBuffer() {
        levelHasSpawned = false;
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
                currentObs = grassDodgeObs;    //SET TO 1 (GRASS LEVEL)
                break;
            default:
                currentObs = grassDodgeObs;    //LEVEL 1 OBSTACLES BY DEFAULT
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
