using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class teleport : MonoBehaviour
{
    private GameObject worldManager;

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

            SceneManager.sceneLoaded += OnSceneLoaded;

            SceneManager.LoadSceneAsync("Combat1", LoadSceneMode.Additive);
        }
    }

    //wait till its loaded to set as active
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is the target scene
        if (scene.name == "Combat1")
        {

            // Set the active scene
            SceneManager.SetActiveScene(scene);

            // Move the player to the checkpoint
            GameObject player = GameObject.Find("playerExport");
            if (player != null)
            {
                player.GetComponent<checkPointScript>().MoveToCheckpoint();
            }

            //unregister the sceneLoaded event
            SceneManager.sceneLoaded -= OnSceneLoaded;


            //unload the previous scenes
            if (SceneManager.GetSceneByName("Tutorial").isLoaded)
            {
                SceneManager.UnloadSceneAsync("Tutorial");
            };
            if (SceneManager.GetSceneByName("Platform1").isLoaded)
            {
                SceneManager.UnloadSceneAsync("Platform1");
            };
        }
    }
}
