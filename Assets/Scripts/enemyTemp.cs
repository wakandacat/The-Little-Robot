using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyTemp : MonoBehaviour
{
    //GET RID OF THIS WHEN REAL ENEMY IN AND FUNCTIONAL
    private mainGameScript mainGameScript;

    void Awake()
    {
        mainGameScript = GameObject.Find("WorldManager").GetComponent<mainGameScript>();
    }

    public void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.name == "playerExport")
        {
            Debug.Log("PLAYER KILLED ENEMY");

            //kill the enemy
            this.gameObject.tag = "Dead";

        }
    }
}
