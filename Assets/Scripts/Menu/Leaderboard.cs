using System.Collections;
using System.Collections.Generic;
using Unity.Services.Leaderboards;
using Unity.Services.Authentication;
using UnityEngine;
using System;
using Unity.Services.Core;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;

//REF: https://docs.unity.com/ugs/en-us/manual/authentication/manual/get-started

public class Leaderboard : MonoBehaviour
{
    //VARIABLES
    private string currentName;
    private float currentScore;
    private float currentDistance;
    const string leaderboardId = "EndlessRunnerV2Leaderboard";

    private bool logInError = false;

    //UI VARIABLES
    private async void Start()
    {
        GameObject.FindGameObjectWithTag("connectionError").GetComponent<TMP_Text>().text = "";

        await UnityServices.InitializeAsync();
        //await AuthenticationService.Instance.SignInAnonymouslyAsync();
        try
        {
            AuthenticationService.Instance.ClearSessionToken();
            await SignInAnonymously();
        }
        catch (Exception ex)
        { 
            
        }
        GetScores();
    }

    //REF: https://docs.unity.com/ugs/en-us/manual/authentication/manual/use-anon-sign-in
    async Task SignInAnonymously()
    {
        try
        {

            AuthenticationService.Instance.SignedIn += () =>
            {
                logInError = false;
                //Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
            };
            AuthenticationService.Instance.SignInFailed += s =>
            {
                // Take some action here...
                Debug.Log("Sign in Failed");
                logInError = true;
            };
        } catch (Exception ex)
        {
            Debug.Log(ex);
        }

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void AddScore(string name, float score, float distance)
    {
        setPlayerName(name);

        var playerEntry = await LeaderboardsService.Instance
        .AddPlayerScoreAsync(leaderboardId, score);
        //Debug.Log(JsonConvert.SerializeObject(playerEntry));

        ResetToken();
        //new AddPlayerScoreOptions { Metadata = new Dictionary<string, string>() { { "team", "red" } } }
    }

    public async void setPlayerName(string playerName) {
        await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
    }

    public async void ResetToken() {
        try
        {
            AuthenticationService.Instance.SignOut(true);
        } catch (Exception ex)
        {
            Debug.Log(ex);
        }
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    //REF: https://forum.unity.com/threads/getting-values-from-a-leaderboard.1525033/
    public async void GetScores()
    {
        if (!logInError)
        {
            GameObject.FindGameObjectWithTag("connectionError").GetComponent<TMP_Text>().text = "";

            var scoresResponse = await LeaderboardsService.Instance
            .GetScoresAsync(leaderboardId);
            //Debug.Log(JsonConvert.SerializeObject(scoresResponse));

            GameObject.FindGameObjectWithTag("NameRecords").GetComponent<TMP_Text>().text = "";
            GameObject.FindGameObjectWithTag("ScoreRecords").GetComponent<TMP_Text>().text = "";

            foreach (var leaderboardEntry in scoresResponse.Results)
            {
                var name = leaderboardEntry.PlayerName.Split('#')[0];
                //Debug.Log(name);
                try
                {
                    GameObject.FindGameObjectWithTag("NameRecords").GetComponent<TMP_Text>().text += name + "\n";
                    GameObject.FindGameObjectWithTag("ScoreRecords").GetComponent<TMP_Text>().text += leaderboardEntry.Score.ToString() + "\n";
                }
                catch
                {

                    //Debug.Log("Text boxes not found");
                }
                //Debug.Log(leaderboardEntry.Score.ToString());
            }
        }
        else {
            GameObject.FindGameObjectWithTag("NameRecords").GetComponent<TMP_Text>().text = "";
            GameObject.FindGameObjectWithTag("ScoreRecords").GetComponent<TMP_Text>().text = "";
            GameObject.FindGameObjectWithTag("connectionError").GetComponent<TMP_Text>().text = "[Couldn't connect to database, please check your internet connection]";
        }
        
    }

    public void clearScoreText() {
        try
        {
            GameObject.FindGameObjectWithTag("NameRecords").GetComponent<TMP_Text>().text = "";
            GameObject.FindGameObjectWithTag("ScoreRecords").GetComponent<TMP_Text>().text = "";
        }
        catch
        {

            //Debug.Log("Text boxes not found");
        }
    }
}
