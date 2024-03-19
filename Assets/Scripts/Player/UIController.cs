using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class DistanceCounter : MonoBehaviour
{
    //DISTANCE VARIABLES
    public bool addingDis = false;
    public float moveSpeed = 5F;
    public GameObject distanceDisplay, itemDisplay, itemBox, scoreDisplay1, scoreDisplay2;
    public int distance = 0;

    private PlayerDodgeMovement pdm;

    void Start()
    {
        pdm = GetComponent<PlayerDodgeMovement>();
    }


    void Update()
    {
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

        distance += 1;
        distanceDisplay.GetComponent<Text>().text = "" + distance + "m";
        yield return new WaitForSeconds(0.25F);
        addingDis = false;
    }


}


