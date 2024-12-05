using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTemp : MonoBehaviour
{
    private mainGameScript mainGameScript;
    public GameObject loadObj;

    void Awake()
    {
        mainGameScript = GameObject.Find("WorldManager").GetComponent<mainGameScript>();
    }

    public void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.name == "tempPlayer")
        {
            Debug.Log("PLAYER KILLED ENEMY");

            mainGameScript.currLevelCount++;

            loadObj.SetActive(true);

            //kill the enemy
            this.gameObject.tag = "Dead";
            this.GetComponent<BoxCollider>().enabled = false;

            mainGameScript.switchCams();

        }
    }
}
