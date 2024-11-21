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

        //if you are currently platforming, next you will be combat
        if (mainGameScript.isCombat == true)
        {
            sceneName = "Platform" + mainGameScript.currLevelCount;
            mainGameScript.isPlatforming = true;
            mainGameScript.isCombat = false;
        } 
        else if (mainGameScript.isPlatforming == true) //currently combat
        {
            sceneName = "Combat" + mainGameScript.currLevelCount;
            mainGameScript.isPlatforming = false;
            mainGameScript.isCombat = true;
        }
        else //currently in tutorial
        {
            sceneName = "Platform" + mainGameScript.currLevelCount;
            mainGameScript.isTutorial = false;
            mainGameScript.isPlatforming = true;
        }
        mainGameScript.lastScene = mainGameScript.currentScene;

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
