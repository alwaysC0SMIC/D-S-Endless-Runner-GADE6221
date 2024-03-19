using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentTrigger : MonoBehaviour
{
    //LEVEL VARIABLES
    private int levelIndex = 0;
    public GameObject[] levelSegments;

    //OBSTACLES VARIABLES
    public GameObject[] dodgeObstacle;

    private bool[] rowFilled = new bool[34];
    private float[] zPosition = new float[34];

    //PICKUP VARIABLES
    public GameObject pickupItem;

    //DISTANCE VARIABLES
    [SerializeField] DistanceCounter distCounter;
    public int totalDistance;

    //GETS POSITIONS OF WHERE NEXT LEVEL SEGMENT WILL SPAWN IN ORDER TO CREATE OBSTACLES IN RIGHT POSITION
    void Start()
    {
        int i = 0;
        float startingVal = 46.34225F;
        float rowDistance = 2F;

        while (i < 34)
        {   
            zPosition[i] = startingVal;
            startingVal += rowDistance;
            i++;
        }
        //Array.ForEach(zPosition, i => Debug.Log(i));
    }

    void Update()
    {
        if (distCounter != null) {
            totalDistance = distCounter.distance;
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

            //LEVEL GENERATION
            Instantiate(GetLevelSegment(levelIndex), new Vector3(1.66893e-06F, -8.076429e-06F, 68.58755F), Quaternion.Euler(0, 180, 0));
            
            //OBSTACLE GENERATION
            generateRows(rowFilled);
            fillRows(dodgeObstacle, zPosition);
            addPickUps(pickupItem);
            resetRows();
            //Array.ForEach(rowFilled, i => Debug.Log(i));
            //Debug.Log(rowFilled);
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

    //INSTANTIATES OBSTACLES IN CHOSEN ROWS
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

    public void addPickUps(GameObject pickUp) {

        int i = 1;
        int minDistance = 5;
        int maxDistance = 10;

        int distanceBetween = UnityEngine.Random.Range(minDistance, maxDistance);
        int position = UnityEngine.Random.Range(-1, 2) * 2;

        while (i < 33)
        {
            if (!rowFilled[i] && !rowFilled[i+1] && !rowFilled[i - 1]) {
                Instantiate(pickUp, new Vector3(position, 1.5F, zPosition[i]), Quaternion.Euler(90, 0, 0));  
            }
            rowFilled[i] = true;
            i += distanceBetween;
            distanceBetween = UnityEngine.Random.Range(minDistance, maxDistance);

            position = UnityEngine.Random.Range(-1, 2) * 2;
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

    //METHOD TO GET LEVEL SEGMENT FROM ARRAY
    public GameObject GetLevelSegment(int levelIndex)
    {
        return levelSegments[levelIndex];
    }
}
