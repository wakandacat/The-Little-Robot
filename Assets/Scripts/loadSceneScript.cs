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


    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "tempPlayer" && !SceneManager.GetSceneByName(sceneName).isLoaded)
        {
          //  mainGameScript.currSceneName++;
            //load the new scene
            SceneManager.LoadScene(mainGameScript.nextScene, LoadSceneMode.Additive);

            //callback once the scene is fully loaded
            SceneManager.sceneLoaded += OnSceneLoaded;

        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mainGameScript.switchCams();

        //remove to enusre it only runs once
        SceneManager.sceneLoaded -= OnSceneLoaded;

        //set newest scene to active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainGameScript.nextScene));

    }

}
