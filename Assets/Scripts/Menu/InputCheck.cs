using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputCheck : MonoBehaviour
{
    //VARIABLES
    private GameObject textField;
    private GameObject leaderboard;

    //RECORD VARIABLES
    private string playerName;
    private float score;
    private float distance;

    private void Update()
    {
        leaderboard = GameObject.FindGameObjectWithTag("Leaderboard");
    }

    //CHECKS AND RETURNS VARIABLES RELATED TO RECORD FOR LEADERBOARD SUBMISSION
    public void checkCurrentRecord() {

        //GETS NAME INPUT
        textField = GameObject.FindGameObjectWithTag("nameInput");
        if (textField != null && textField.GetComponent<TMP_InputField>().text.Length > 0)
        {
            string name = textField.GetComponent<TMP_InputField>().text;
            //Debug.Log("Text Check: " + name);            
                Debug.Log("Text Check: " + name);

                //GETS SCORE
                score = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerDodgeMovement>().playerScore;
                distance = GameObject.FindGameObjectWithTag("Player").GetComponent<DistanceCounter>().distance;
                distance = Mathf.RoundToInt(distance);

                Debug.Log(score + " " + distance);
                playerName = name;

                //CONFIRMS SUBMISSION
                textField.GetComponent<TMP_InputField>().DeactivateInputField();
                textField.GetComponent<TMP_InputField>().text = "Record Saved";
                textField.GetComponent<TMP_InputField>().interactable = false;
                this.gameObject.SetActive(false);

                leaderboard.GetComponent<Leaderboard>().AddScore(name, score, distance);
            }
            else
            {
                Debug.Log("Invalid name");
                //textField.GetComponent<TMP_InputField>().placeholder.GetComponent<Text>().text = "Invalid Name";
            }  
    }

    //public void getRecentRecord(out string name, out float outScore, out float outDistance) {
    //    name = playerName;
    //    outScore = score;
    //    outDistance = distance;
    //}
}
