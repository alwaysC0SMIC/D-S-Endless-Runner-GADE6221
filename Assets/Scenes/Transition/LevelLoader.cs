using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    //VARIABLES
    private Animator top;
    private Animator bottom;

    //private static float animationTime = 1.167F;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //TRANSITION ANIMATORS
        top = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        bottom = transform.GetChild(0).GetChild(1).GetComponent<Animator>();
        //PlayTransition();
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
