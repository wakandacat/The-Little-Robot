using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadSceneScript : MonoBehaviour
{
   
    //public string sceneName;
    private mainGameScript mainGameScript;
    private GameObject door;
    public GameObject proceedSign;

    void Awake()
    {
        mainGameScript = GameObject.Find("WorldManager").GetComponent<mainGameScript>();
    }


    public void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            //increment the scene to load
            mainGameScript.currSceneName++;
            mainGameScript.currentScene = mainGameScript.scenes[mainGameScript.currSceneName];

            //load the new scene
            SceneManager.LoadSceneAsync(mainGameScript.currentScene, LoadSceneMode.Additive);
           // Debug.Log("loading: " + mainGameScript.currentScene);

            //callback once the scene is fully loaded
            SceneManager.sceneLoaded += OnSceneLoaded;

        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        //find the door and open it 
        door = GameObject.Find("DoorGroup").transform.GetChild(mainGameScript.doorNum).gameObject;

        if (door.transform.childCount > 1)
        {
            if (door.transform.GetChild(0).GetComponent<doorScript>().isFungus == false)
            {
                door.transform.GetChild(0).GetComponent<doorScript>().openDoor();
            }
        }
        else
        {
            if (door.GetComponent<doorScript>().isFungus == false)
            {
                door.GetComponent<doorScript>().openDoor();

                //door sound
                if (door.GetComponent<AudioSource>().isPlaying == false)
                {
                    door.GetComponent<AudioSource>().Play();
                }
            }
        }

        //let combat scenes do they own thing
        if (!SceneManager.GetActiveScene().name.Contains("Combat"))
        {
            //turn on the proceed sign
            proceedSign.transform.GetChild(0).gameObject.SetActive(true);
            proceedSign.transform.GetChild(1).gameObject.GetComponent<AudioSource>().Play();
        }

        //remove to enusre it only runs once
        SceneManager.sceneLoaded -= OnSceneLoaded;

        //set newest scene to active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainGameScript.currentScene));
        // Debug.Log("set  " + mainGameScript.currentScene + "as active");

    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //get rid of the trigger area so it can't be triggered again
            this.gameObject.SetActive(false);

        }
    }

}
