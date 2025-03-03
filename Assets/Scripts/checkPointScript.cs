using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class checkPointScript : MonoBehaviour
{
    //CALL THIS SCRIPT WHEREVER DEATH FLAG IS SET

    private string currScene;
    private GameObject currCheckpoint;
    private mainGameScript mainGameScript;
    player_fx_behaviors fxBehave;

    //move the player back to the correct checkpoint
    public void MoveToCheckpoint()
    {
        mainGameScript = GameObject.Find("WorldManager").GetComponent<mainGameScript>();

        //get the current scene
        currScene = mainGameScript.currentScene;

        //stop music
        mainGameScript.m_audio.backgroundSource.Stop();
        SceneManager.UnloadSceneAsync(currScene);
        SceneManager.LoadScene(currScene, LoadSceneMode.Additive);

        //callback once the scene is fully loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //remove to enusre it only runs once
        SceneManager.sceneLoaded -= OnSceneLoaded;

        //set newest scene to active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currScene));
        mainGameScript.m_audio.playBackgroundMusic("platform");

        //don't respawn the enemy or its triggers if the player dies after the enemy is dead, but keep the light on
        if (SceneManager.GetActiveScene().name == "Combat1" && mainGameScript.firstBossDead == true)
        {
            GameObject.FindGameObjectWithTag("Boss Enemy").SetActive(false);
            GameObject.Find("startBattleTrigger").SetActive(false);
            GameObject.Find("proceedLight").GetComponent<Light>().intensity = 3;
            mainGameScript.m_audio.enemyWhirringSource.enabled = false; //don't play whirring if enemy is dead
        }
        else if (SceneManager.GetActiveScene().name == "Combat2" && mainGameScript.secondBossDead == true)
        {
            GameObject.FindGameObjectWithTag("Boss Enemy").SetActive(false);
            GameObject.Find("startBattleTrigger").SetActive(false);
            GameObject.Find("proceedLight").GetComponent<Light>().intensity = 3;
            mainGameScript.m_audio.enemyWhirringSource.enabled = false; //don't play whirring if enemy is dead
        }
        else if (SceneManager.GetActiveScene().name == "Combat3" && mainGameScript.thirdBossDead == true)
        {
            GameObject.FindGameObjectWithTag("Boss Enemy").SetActive(false);
            GameObject.Find("startBattleTrigger").SetActive(false);
            GameObject.Find("proceedLight").GetComponent<Light>().intensity = 3;
            mainGameScript.m_audio.enemyWhirringSource.enabled = false; //don't play whirring if enemy is dead
        }

        //if (SceneManager.GetActiveScene().name.Contains("Combat"))
        //{
        //    if(mainGameScript.firstBossDead == false || mainGameScript.secondBossDead == false || mainGameScript.thirdBossDead == false)
        //    {
        //        mainGameScript.m_audio.enemyWhirringSource.enabled = true; //play whirring if enemy is dead
        //    }
        //}

        //switch cameras if needed
        mainGameScript.SwitchToPlatformCam(0.4f);

        //get the current checkpoint from the main script
        currCheckpoint = mainGameScript.currCheckpoint;
        //Debug.Log("running onscene loaded in checkpoint method");

        //move the player there
        this.transform.position = currCheckpoint.transform.position;

        //load fx script
        fxBehave = this.GetComponent<player_fx_behaviors>();
        //restart sound coroutine
        fxBehave.walkCoroutine = fxBehave.StartCoroutine(fxBehave.walkSFX());

        //restart pre battle enemy sfx if respawned
        //mainGameScript.m_audio.playEnemySFX(0); //start whirring if in a combat scene
        //mainGameScript.m_audio.enemyWhirringSource.enabled = true;

        //rotate them to face forward
        this.transform.rotation = currCheckpoint.transform.rotation;

        //MAKE THE FREELOOK CAMERA FACE FORWARD AS WELL
        mainGameScript.CheckPointResetPlatformCam(this.transform.eulerAngles.y);

    }
}
