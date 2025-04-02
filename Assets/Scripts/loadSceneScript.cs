using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class loadSceneScript : MonoBehaviour
{
   
    //public string sceneName;
    private mainGameScript mainGameScript;
    public GameObject door;
    public GameObject proceedSign;
    public AudioSource proceedaudio;

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

        //fungus door has children
        if (door.transform.childCount > 1)
        {
            door.transform.GetChild(0).GetComponent<doorScript>().loadedNextStart = true;

            if (door.transform.GetChild(0).GetComponent<doorScript>().isFungus == false) //I DONT THINK THIS RUNS
            {
                door.transform.GetChild(0).GetComponent<doorScript>().openDoor();
            }
        }
        else //regular door
        {
            if (door.GetComponent<doorScript>().isFungus == false)
            {
                door.GetComponent<doorScript>().openDoor();
                door.GetComponent<doorScript>().loadedNextStart = true;
            }
        }

        //let combat scenes do they own thing
        if (!SceneManager.GetActiveScene().name.Contains("Combat"))
        {
            //turn on the proceed sign
            proceedSign.SetActive(true);
            proceedaudio.Play();
        }

        //remove to enusre it only runs once
        SceneManager.sceneLoaded -= OnSceneLoaded;

        //set newest scene to active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(mainGameScript.currentScene));
        // Debug.Log("set  " + mainGameScript.currentScene + "as active");

        //MOVE BATTLE TRACK NOW SO IT DOESNT CLIP WALLS LATER
        if (SceneManager.GetActiveScene().name.Contains("Combat"))
        {
            GameObject enemy = GameObject.FindGameObjectWithTag("Boss Enemy");

            ////move the track to teh enemy's position
            Vector3 bossPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 13, enemy.transform.position.z);
            GameObject.Find("battleCamTrack").transform.position = bossPos;
            GameObject.Find("battleCam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = 0;
        }


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
