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
        sceneToUnload = mainGameScript.lastScene;
    }

    // Update is called once per frame
    public void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.name == "tempPlayer" && SceneManager.GetSceneByName(mainGameScript.currentScene).isLoaded)
        {
            mainGameScript.currSceneName++;

            SceneManager.UnloadSceneAsync(sceneToUnload);
        }
    }
}
