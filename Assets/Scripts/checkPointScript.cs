using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SceneManagement;

public class checkPointScript : MonoBehaviour
{
    //CALL THIS SCRIPT WHEREVER DEATH FLAG IS SET

    private Scene currScene;
    private GameObject currCheckpoint;

    //move the player back to the correct checkpoint
    public void MoveToCheckpoint()
    {

        //get the current scene
        currScene = SceneManager.GetActiveScene();

        //find the object with the tag "Checkpoint" in that scene
        currCheckpoint = GameObject.FindWithTag("Checkpoint");

        //move the player there
        this.transform.position = currCheckpoint.transform.position;

        //do any resetting of the scene
        //if currScene is a combat scene, then reset enemy stuff
        //if currScene is a platforming scene, then reset platforming stuff
    }
}
