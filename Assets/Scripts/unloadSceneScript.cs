using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class unloadSceneScript : MonoBehaviour
{

    //public string sceneToUnload;
    private mainGameScript mainGameScript;

    void Awake()
    {

        mainGameScript = GameObject.Find("WorldManager").GetComponent<mainGameScript>();
        //Debug.Log("current" + mainGameScript.currentScene);

        //if the previous scene is already unloaded (you've died and respawned here), then move this trigger out of the way
        if (mainGameScript.currentScene != "Tutorial" && !SceneManager.GetSceneByName(mainGameScript.scenes[mainGameScript.currSceneName - 1]).isLoaded)
        {
            //get rid of the trigger area so it can't be triggered again
            this.gameObject.SetActive(false);
           // Debug.Log("move out of way, previous scene already unloaded");
        } 
    }

    //unload the previous scene
    public void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.name == "playerExport" && SceneManager.GetSceneByName(mainGameScript.currentScene).isLoaded)
        {
            SceneManager.UnloadSceneAsync(mainGameScript.scenes[mainGameScript.currSceneName - 1]);
            //Debug.Log("get rid of previous scene: " + mainGameScript.scenes[mainGameScript.currSceneName - 1]);
        }
    }


    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "playerExport")
        {
            //get rid of the trigger area so it can't be triggered again
            this.gameObject.SetActive(false);

        }
    }
}
