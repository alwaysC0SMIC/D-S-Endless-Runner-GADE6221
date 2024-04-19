using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class DistanceCounter : MonoBehaviour
{
    //DISTANCE VARIABLES
    public bool addingDis = false;
    public float gameSpeed = 5F;    //DEFAULT IS 5
    public GameObject distanceDisplay, itemDisplay, itemBox, scoreDisplay1, scoreDisplay2;
    public float distance = 0;

    private PlayerDodgeMovement pdm;
    private World world;

    void Start()
    {
        pdm = GetComponent<PlayerDodgeMovement>();
        world = GameObject.Find("World").GetComponent<World>();
    }


    void Update()
    {
        if (world != null)
        {
            gameSpeed = world.gameSpeed;   
        }

        //SCORE
        scoreDisplay1.GetComponent<Text>().text = GetComponent<PlayerDodgeMovement>().playerScore.ToString();
        scoreDisplay2.GetComponent<Text>().text = "Score: " + GetComponent<PlayerDodgeMovement>().playerScore.ToString();

        //DISTANCE
        if (addingDis == false)
        {
            addingDis = true;
            StartCoroutine(AddingDistance());
        }
        if (pdm != null)
        {
            if (pdm.invincible)
            {
                itemDisplay.GetComponent<Text>().text = "Invincible";
                itemBox.GetComponent<RawImage>().color = new Color(0, 0, 0, 0.55F);
            }
            else
            {
                itemDisplay.GetComponent<Text>().text = "";
                itemBox.GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
            }
        }
    }

    IEnumerator AddingDistance()
    {
        distance += (1 * gameSpeed)/5;
        //Debug.Log(distance);
        distanceDisplay.GetComponent<Text>().text = "" + Mathf.RoundToInt(distance) + "m";
        yield return new WaitForSeconds(0.25F);
        addingDis = false;
    }


}


