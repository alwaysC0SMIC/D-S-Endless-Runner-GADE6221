using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    //VARIABLES
    Animator top;
    Animator bottom;

    //private static float animationTime = 1.167F;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        top = transform.GetChild(0).GetComponent<Animator>();
        bottom = transform.GetChild(1).GetComponent<Animator>();
    }

    void Update()
    {

    }

    public void PlayTransition() {
        top.SetTrigger("Top");
        bottom.SetTrigger("Bottom");
    }

    //IEnumerator LoadSceneWithTransition(int levelIndex)
    //{
    //    top.Play("TopTransitionAnim");
    //    bottom.Play("BottomTransitionAnim");

    //    yield return new WaitForSeconds(animationTime);

    //    SceneManager.LoadScene(levelIndex);
    //}
}
