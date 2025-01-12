using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadSceneScript : MonoBehaviour
{
   
    //public string sceneName;
    private mainGameScript mainGameScript;

    void Awake()
    {
        mainGameScript = GameObject.Find("WorldManager").GetComponent<mainGameScript>();
    }


    public void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.name == "playerExport")
        {
            //increment the scene to load
            mainGameScript.currSceneName++;
            mainGameScript.currentScene = mainGameScript.scenes[mainGameScript.currSceneName];

            //load the new scene
            SceneManager.LoadScene(mainGameScript.currentScene, LoadSceneMode.Additive);
            //Debug.Log("loading: " + mainGameScript.currentScene);

            //callback once the scene is fully loaded
            SceneManager.sceneLoaded += OnSceneLoaded;

        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        //remove to enusre it only runs once
        SceneManager.sceneLoaded -= OnSceneLoaded;

        //set newest scene to active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainGameScript.currentScene));
       // Debug.Log("set  " + mainGameScript.currentScene + "as active");

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
