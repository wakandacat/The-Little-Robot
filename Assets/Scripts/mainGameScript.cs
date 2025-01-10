using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class mainGameScript : MonoBehaviour
{

    //global vars
    public bool playerCanMove = false; //toggle for when menus open or cutscenes
    public int currLevelCount = 1;
    public int maxLevelCount = 3;
    public string nextScene = "Platform1";
    public string currentScene = "Tutorial";

    //get the 2 virtual cameras
    public CinemachineFreeLook platformCam;
    public CinemachineVirtualCamera bossCam;

    private GameObject enemy;
    public string[] scenes = new[] { "Tutorial", "Platform1", "Combat1", "Platform2", "Combat2", "Platform3", "Combat3", "EndScene" };
    public int currSceneName = 0;
    
    //menu vars
    public GameObject demoEndScreen;
    public GameObject mainMenu;
    public GameObject controlMenu;
    public GameObject demoEndFirstButton;
    bool gameEnded = false;

    void Awake()
    {
        //load the first scene in addition to the base scene
        SceneManager.LoadScene("Tutorial", LoadSceneMode.Additive);

        //ensure time is running and we are not still paused
        Time.timeScale = 1.0f;
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused = false;

        //clear event selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new default selected
        EventSystem.current.SetSelectedGameObject(demoEndFirstButton);
    }
    void Update()
    {
        //the current scene
        currentScene = scenes[currSceneName];

        if (currentScene != "EndScene")
        {
            //the next scene
            nextScene = scenes[currSceneName + 1];
        }
        //-------------------TEMPORARY FOR MILESTONE 3 DEMO---------------------------------------
        //if (currentScene == "Platform2" && gameEnded == false)
        //{
        //    EndGame();
        //    gameEnded = true;
        //}
        //--------------------------------------------------------------------------------------------
    }

    public void EndGame()
    {
        Debug.Log("Demo Over");
        demoEndScreen.SetActive(true);
        Time.timeScale = 0.0f;
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused = true;
        mainMenu.SetActive(false);
        controlMenu.SetActive(false);

        //clear event selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new default selected
        EventSystem.current.SetSelectedGameObject(demoEndFirstButton);
    }

    public void SwitchToBossCam()
    {
       // Debug.Log("boss cam");

        enemy = GameObject.Find("enemy" + currLevelCount);

        bossCam.LookAt = enemy.transform;
        bossCam.Priority = platformCam.Priority + 1;
    }

    public void SwitchToPlatformCam()
    {
        //Debug.Log("platform cam");
        platformCam.Priority = bossCam.Priority + 1;
    }

}
