using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SceneManagement;

public class checkPointScript : MonoBehaviour
{
    //CALL THIS SCRIPT WHEREVER DEATH FLAG IS SET

    private string currScene;
    private GameObject currCheckpoint;
    private mainGameScript mainGameScript;

    //move the player back to the correct checkpoint
    public void MoveToCheckpoint()
    {
        mainGameScript = GameObject.Find("WorldManager").GetComponent<mainGameScript>();

        //get the current scene
        currScene = mainGameScript.currentScene;

        //reload the currently active scene
       // Debug.Log("current scene:" + currScene);
        SceneManager.UnloadSceneAsync(currScene);
        SceneManager.LoadScene(currScene, LoadSceneMode.Additive);

        //callback once the scene is fully loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("current scene loaded");
        //remove to enusre it only runs once
        SceneManager.sceneLoaded -= OnSceneLoaded;

        //set newest scene to active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currScene));

        //find the object with the tag "Checkpoint" in that scene
        currCheckpoint = GameObject.FindWithTag("Checkpoint");

        //move the player there
        this.transform.position = currCheckpoint.transform.position;

        //switch cameras if needed
        mainGameScript.SwitchToPlatformCam();

    }
}
