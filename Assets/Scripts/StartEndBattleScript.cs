using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartEndBattleScript : MonoBehaviour
{
    //get the bridges and the enemy
    public GameObject startBridge;
    public GameObject endBridge;
    public GameObject enemy;
    private mainGameScript mainGameScript;
    public GameObject loadObj;
    private bool runOnce = false;
    public CinemachineBlenderSettings enemyDeadBlend;
    public CinemachineBlenderSettings enemyAliveBlend;
    private CinemachineBrain camBrain;

    void Awake()
    {
        mainGameScript = GameObject.Find("WorldManager").GetComponent<mainGameScript>();

        camBrain = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineBrain>();
    }

    void Update()
    {
        //check enemy's state here for death
        if (enemy.GetComponent<BossEnemy>().HP_ReturnCurrent() <= 0 && runOnce == false)
        {
            if (SceneManager.GetActiveScene().name == "Combat1")
            {
                mainGameScript.firstBossDead = true;
            } 
            else if (SceneManager.GetActiveScene().name == "Combat2")
            {
                mainGameScript.secondBossDead = true;
            }
            else if (SceneManager.GetActiveScene().name == "Combat3")
            {
                mainGameScript.thirdBossDead = true;
            }

            camBrain.m_CustomBlends = enemyDeadBlend;

            //open end bridge
            endBridge.GetComponent<bridgeScript>().moveBridgeLeft();
            startBridge.GetComponent<bridgeScript>().moveBridgeRight();

            loadObj.SetActive(true);

            mainGameScript.currLevelCount++;

            //switch cameras
            mainGameScript.SwitchToPlatformCam(0.4f);

            runOnce = true;

            Invoke("SwitchBlend", 3.0f);
        }

    }

    public void SwitchBlend()
    {
        camBrain.m_CustomBlends = enemyAliveBlend;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "playerExport")
        {
            //Debug.Log("switch to boss cam");

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
