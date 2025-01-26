using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class teleport : MonoBehaviour
{
    private GameObject worldManager;
    private GameObject door;

    void Awake()
    {
        worldManager = GameObject.Find("WorldManager");
    }

    //to teleport directly to the combat area
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "playerExport")
        {

            //set the current scene to be combat then move the player to that checkpoint
            worldManager.GetComponent<mainGameScript>().currSceneName = 2;
            worldManager.GetComponent<mainGameScript>().currentScene = "Combat1";

            //unload the previous scenes
            if (SceneManager.GetSceneByName("Tutorial").isLoaded)
            {
                SceneManager.UnloadSceneAsync("Tutorial");
            };
            if (SceneManager.GetSceneByName("Platform1").isLoaded)
            {
                SceneManager.UnloadSceneAsync("Platform1");
            };


            SceneManager.LoadSceneAsync("Combat1", LoadSceneMode.Additive);

            SceneManager.sceneLoaded += OnSceneLoaded;

        }
    }

    //wait till its loaded to set as active
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //unregister the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Combat1"));

        // Move the player to the checkpoint
        GameObject player = GameObject.Find("playerExport");

        if (player != null)
        {
            player.GetComponent<checkPointScript>().MoveToCheckpoint();
        }

        //update the door number
        door = GameObject.Find("DoorGroup").transform.GetChild(worldManager.GetComponent<mainGameScript>().doorNum).gameObject;
        worldManager.GetComponent<mainGameScript>().doorNum = 2;

    }
}
