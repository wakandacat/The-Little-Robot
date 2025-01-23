using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEndBattleScript : MonoBehaviour
{
    //get the bridges and the enemy
    public GameObject startBridge;
    public GameObject endBridge;
    public GameObject enemy;
    private mainGameScript mainGameScript;
    public GameObject loadObj;
    private bool runOnce = false;

    void Awake()
    {
        mainGameScript = GameObject.Find("WorldManager").GetComponent<mainGameScript>();
    }

    void Update()
    {
        //check enemy's state here for death
        if (enemy.GetComponent<BossEnemy>().HP_ReturnCurrent() <= 0 && runOnce == false)
        //if (enemy.tag == "Dead" && runOnce == false) //TEMPPPP
        {
            //Debug.Log("dead enemy");
            //open end bridge
            endBridge.GetComponent<bridgeScript>().moveBridgeLeft();
            startBridge.GetComponent<bridgeScript>().moveBridgeRight();

            loadObj.SetActive(true);

            mainGameScript.currLevelCount++;

            //switch cameras
            mainGameScript.SwitchToPlatformCam();

            runOnce = true;
        }

    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "playerExport")
        {
            Debug.Log("switch to boss cam");

            //hide the bridges
            startBridge.GetComponent<bridgeScript>().moveBridgeLeft();
            endBridge.GetComponent<bridgeScript>().moveBridgeRight();

            //switch cameras
            mainGameScript.SwitchToBossCam();
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "playerExport")
        {
            //move itself so it can't be triggered again
            Vector3 currentPosition = this.transform.position;
            float newY = currentPosition.y + 100f;
            this.transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);
        }
    }
}
