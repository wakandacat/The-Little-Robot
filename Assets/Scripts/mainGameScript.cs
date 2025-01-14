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
    public GameObject battleTrack;
    private bool usingBossCam = false;

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

        if (GameObject.Find("enemy" + currLevelCount) && usingBossCam == true && GameObject.FindWithTag("Player"))
        {

            //calculate the direction from the track center to the player    
            Vector3 direction = GameObject.FindWithTag("Player").transform.position - battleTrack.transform.position;

            //calculate the angle in degrees (0 to 360) around the track center
            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

            //ensure its positive
            if (angle < 0)
            {
                angle = angle + 360f;
            }
            angle = angle - 270f;  //camera is offset by 270?

            //normalize again
            if (angle < 0)
            {
                angle = angle + 360f;
            }

            //map the angle to a normalized path position (0 to 1) -> we are using normalized on battleCam
            float targetPathPosition = angle / 360f;

            //get path placement
            float currentPathPosition = bossCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition;
            float shortestRoute = targetPathPosition - currentPathPosition;

            //shortest route to avoid track look bugging at beginning of loop
            if (shortestRoute > 0.5f)
            {
                shortestRoute -= 1f;
            }
            else if (shortestRoute < -0.5f)
            {
                shortestRoute += 1f;
            }

            bossCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition += shortestRoute * Time.deltaTime * 10f;
            bossCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition %= 1f;
            if (bossCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition < 0)
            {
                bossCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition += 1f;
            }
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

        //move the track to teh enemy's position
        Vector3 bossPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 6, enemy.transform.position.z);
        battleTrack.transform.position = bossPos;
        bossCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = 0;
       // Debug.Log(enemy.transform.position.x + " " + enemy.transform.position.y + " " + enemy.transform.position.z);

        bossCam.LookAt = enemy.transform;
        //bossCam.Follow = enemy.transform;
        bossCam.Priority = platformCam.Priority + 1;

        usingBossCam = true;
    }

    public void SwitchToPlatformCam()
    {

        //Debug.Log("platform cam");
        platformCam.Priority = bossCam.Priority + 1;

        usingBossCam = false;
    }

}
