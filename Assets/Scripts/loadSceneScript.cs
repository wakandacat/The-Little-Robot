using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadSceneScript : MonoBehaviour
{
   
    public string sceneName;
    private mainGameScript mainGameScript;

    void Awake()
    {
        mainGameScript = GameObject.Find("WorldManager").GetComponent<mainGameScript>();
    }

    void Update()
    {

        //the current scene to load
        sceneName = mainGameScript.scenes[mainGameScript.currSceneName + 1];
        mainGameScript.currentScene = sceneName;
        //the current scene to unload
        mainGameScript.lastScene = mainGameScript.scenes[mainGameScript.currSceneName];

    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "tempPlayer" && !SceneManager.GetSceneByName(sceneName).isLoaded)
        {
   
            //load the new scene
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

            //callback once the scene is fully loaded
            SceneManager.sceneLoaded += OnSceneLoaded;

            mainGameScript.currentScene = sceneName;

        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        mainGameScript.switchCams();

        //remove to enusre it only runs once
        SceneManager.sceneLoaded -= OnSceneLoaded;
        
    }

}
