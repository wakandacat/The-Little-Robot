using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class unloadSceneScript : MonoBehaviour
{

    public string sceneToUnload;
    private mainGameScript mainGameScript;

    void Awake()
    {

        mainGameScript = GameObject.Find("WorldManager").GetComponent<mainGameScript>();
        //Debug.Log("current" + mainGameScript.currentScene);
    }

    // Update is called once per frame
    public void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.name == "playerExport" && SceneManager.GetSceneByName(mainGameScript.currentScene).isLoaded)
        {

            SceneManager.UnloadSceneAsync(mainGameScript.currentScene);
            mainGameScript.currSceneName++;
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
