using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bossUI : MonoBehaviour
{
    [SerializeField] World world;
    [SerializeField] GameObject textBox;

    void Update()
    {
        //Debug.Log(world.bossFight);

        textBox.GetComponent<Text>().text = "Boss HP: " + world.activeBossHP;

        if (!world.bossFight)
            {
            this.gameObject.SetActive(false);
            }
            else {
            this.gameObject.SetActive(true);
            }
    } 
}
