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
    public GameObject enemyUI;
    public Light proceedLight;

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
                mainGameScript.m_audio.playBackgroundMusic("platform");
            } 
            else if (SceneManager.GetActiveScene().name == "Combat2")
            {
                mainGameScript.secondBossDead = true;
                mainGameScript.m_audio.playBackgroundMusic("platform");
            }
            else if (SceneManager.GetActiveScene().name == "Combat3")
            {
                mainGameScript.thirdBossDead = true;
                mainGameScript.m_audio.playBackgroundMusic("platform");
            }

            camBrain.m_CustomBlends = enemyDeadBlend;

            //open end bridge
            endBridge.GetComponent<bridgeScript>().moveBridgeLeft();
            startBridge.GetComponent<bridgeScript>().moveBridgeRight();

            loadObj.SetActive(true);

            mainGameScript.currLevelCount++;

            //turn on exit light
            proceedLight.intensity = 3;

            //switch cameras
            mainGameScript.SwitchToPlatformCam(0.4f);
            //enemy.GetComponent<boss_fx_behaviors>().StopCoroutine(enemy.GetComponent<boss_fx_behaviors>().turnOffEyes()); //ginette

            runOnce = true;

            Invoke("SwitchBlend", 3.0f);

            enemyUI.SetActive(false);
        }

    }

    public void SwitchBlend()
    {
        camBrain.m_CustomBlends = enemyAliveBlend;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("switch to boss cam");

            //hide the bridges
            startBridge.GetComponent<bridgeScript>().moveBridgeLeft();
            endBridge.GetComponent<bridgeScript>().moveBridgeRight();

            //switch cameras
            mainGameScript.SwitchToBossCam();

            //play battle music
            mainGameScript.m_audio.playBackgroundMusic(SceneManager.GetActiveScene().name);

            //Bool to show the enemy UI

            enemyUI.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //move itself so it can't be triggered again
            Vector3 currentPosition = this.transform.position;
            float newY = currentPosition.y + 100f;
            this.transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);
        }
    }
}
