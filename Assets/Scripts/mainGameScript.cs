using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class mainGameScript : MonoBehaviour
{
    //main game settings from main menu
    private GameObject gameSettings;
    public GameObject settingsMenu;

    //global scene vars
    public int currLevelCount = 1;
    public int maxLevelCount = 3;
    public string nextScene = "Platform1";
    public string currentScene = "Tutorial";
    public string[] scenes = new[] { "Tutorial", "Platform1", "Combat1", "Platform2", "Combat2", "Platform3", "Combat3", "EndScene" };
    public int currSceneName = 0;

    //get the 2 virtual cameras
    public CinemachineFreeLook platformCam;
    public CinemachineVirtualCamera bossCam;
    public GameObject battleTrack;
    private bool usingBossCam = false;

    //intro cinematic camera and path
    public CinemachineVirtualCamera introCam;
    bool introPlayed = false;
    private float camPos = 0;
    public float introCamSpeed = 0.003f;
    private float camStillTimer = 0f;

    //cutscenes
    public bool cutScenePlaying = true; //toggle for when menus open or cutscenes

    //enemy
    private GameObject enemy;
    public bool firstBossDead = false;
    public bool secondBossDead = false;
    public bool thirdBossDead = false;

    //menu vars
    public GameObject demoEndScreen;
    public GameObject controlMenu;
    public GameObject demoEndFirstButton;
    bool gameEnded = false;

    //doors between scenes
    public int doorNum = 0;

    //checkpoint management
    public GameObject currCheckpoint;
    public GameObject checkpointGrp;

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

        //get teh game settings if they exist --> if gameplay started from MainMenu scene then it will exist, otherwise it wont
        if (GameObject.Find("GameSettings"))
        {
            gameSettings = GameObject.Find("GameSettings");
        }

        setSettings();

        //get the first checkpoint from the checkpoint group
        currCheckpoint = checkpointGrp.transform.GetChild(0).gameObject;

        //callback once the scene is fully loaded
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Tutorial")
        {
            // Set the Tutorial scene as active
            SceneManager.SetActiveScene(scene);

            // Remove the event listener to ensure it only runs once
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    //method to set all the values from the main game settings that carried over
    void setSettings()
    {
        if (gameSettings)
        {
            //set the initial freelook sensitivity from main menu
            platformCam.m_XAxis.m_MaxSpeed = 300 * gameSettings.GetComponent<GameSettings>().freelookSens; //middle is 150
            platformCam.m_YAxis.m_MaxSpeed = 4 * gameSettings.GetComponent<GameSettings>().freelookSens; //middle is 2
            settingsMenu.GetComponentInChildren<Slider>().value = gameSettings.GetComponent<GameSettings>().freelookSens;
        } 
        else
        {
            //set the defaul freelook sensitivity if gamesettings not chosen
            platformCam.m_XAxis.m_MaxSpeed = 300 * 0.5f; //middle is 150
            platformCam.m_YAxis.m_MaxSpeed = 4 * 0.5f; //middle is 2
            settingsMenu.GetComponentInChildren<Slider>().value = 0.5f;
        }
    }

    void FixedUpdate()
    {

        //--------------INTRO CUTSCENE---------------
        if (introPlayed == false)
        {
            IntroCam();
        }

        //---------------BOSS CAM-------------------
        //if (GameObject.Find("enemy" + currLevelCount) && usingBossCam == true && GameObject.FindWithTag("Player"))
        //{
        //    //calculate the direction from the track center to the player    
        //    Vector3 direction = GameObject.FindWithTag("Player").transform.position - battleTrack.transform.position;

        //    //calculate the angle in degrees (0 to 360) around the track center
        //    float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        //    //ensure its positive
        //    if (angle < 0)
        //    {
        //        angle = angle + 360f;
        //    }
        //    angle = angle - 270f;  //camera is offset by 270?

        //    //normalize again
        //    if (angle < 0)
        //    {
        //        angle = angle + 360f;
        //    }

        //    //map the angle to a normalized path position (0 to 1) -> we are using normalized on battleCam
        //    float targetPathPosition = angle / 360f;

        //    //get path placement
        //    float currentPathPosition = bossCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition;
        //    float shortestRoute = targetPathPosition - currentPathPosition;

        //    //shortest route to avoid track look bugging at beginning of loop
        //    if (shortestRoute > 0.5f)
        //    {
        //        shortestRoute -= 1f;
        //    }
        //    else if (shortestRoute < -0.5f)
        //    {
        //        shortestRoute += 1f;
        //    }

        //    //take into account the player's distance from the center enemy (closer should move the camera less quickly)
        //    // float distanceToCenter = Vector3.Distance(GameObject.FindWithTag("Player").transform.position, GameObject.Find("enemy" + currLevelCount).transform.position);
        //    //damping factor
        //    // float dampingFactor = Mathf.Clamp01(distanceToCenter / 10f);
        //    // bossCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition += shortestRoute * Time.deltaTime * 10f * dampingFactor;
        //    // Debug.Log(shortestRoute * Time.deltaTime * 10f * dampingFactor);

        //    bossCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition += shortestRoute * Time.deltaTime * 10f;
        //    bossCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition %= 1f;
        //    if (bossCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition < 0)
        //    {
        //        bossCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition += 1f;
        //    }
        //}
    }

    public void SkipIntro()
    {
        cutScenePlaying = false;

    }

    public void EndGame()
    {
       // Debug.Log("Thanks for playing!");
        demoEndScreen.SetActive(true);
        Time.timeScale = 0.0f;
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused = true;
       // mainMenu.SetActive(false);
        controlMenu.SetActive(false);

        //clear event selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set new default selected
        EventSystem.current.SetSelectedGameObject(demoEndFirstButton);
    }

    public void SwitchToBossCam()
    {
        //Debug.Log("battle cam");
        // enemy = GameObject.Find("enemy" + currLevelCount);
        enemy = GameObject.FindGameObjectWithTag("Boss Enemy");

        //move the track to teh enemy's position
        Vector3 bossPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 10, enemy.transform.position.z);
        battleTrack.transform.position = bossPos;
        bossCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = 0;

        bossCam.LookAt = enemy.transform;
        bossCam.Priority = platformCam.Priority + 1;

        usingBossCam = true;
    }

    public void SwitchToPlatformCam(float yaxis)
    {
        //maybe a slightly better transition idk
        if (SceneManager.GetActiveScene().name.Contains("Combat"))
        {
            platformCam.m_XAxis.Value = bossCam.transform.rotation.x;
        }
        //Debug.Log("platform cam");
        platformCam.Priority = bossCam.Priority + 1;
        platformCam.m_YAxis.Value = yaxis; //position up teh spine axis

        usingBossCam = false;
    }

    public void CheckPointResetPlatformCam(float rotation)
    {
        platformCam.m_XAxis.Value = rotation;
        platformCam.ForceCameraPosition(transform.position, Quaternion.Euler(0, rotation, 0)); //force freelook to have player's rotation
        platformCam.m_YAxis.Value = 0.4f; //position up teh spine axis
    }

    public void IntroCam()
    {
        if (cutScenePlaying) //left button on controller to skip cutscenes
        {
            if (GameObject.FindWithTag("Player") && GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused == false) //continue the cutscene if the game is not paused
            {
                if (introCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition >= 1 && introPlayed == false)
                {
                  //  Debug.Log("intro cinematic finished");
                    SwitchToPlatformCam(0.2f);
                    introPlayed = true;
                    cutScenePlaying = false;
                    camStillTimer = 0;
                }
                else
                {
                    camStillTimer = camStillTimer + Time.deltaTime;

                    //wait a few seconds at the beginning before starting the movement
                    if (camStillTimer >= 2.0f)
                    {
                        camPos += Mathf.Lerp(0, 1, introCamSpeed);
                        introCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = camPos;
                    } 
                    else //otherwise adjust the fov
                    {
                        if (introCam.m_Lens.FieldOfView > 70) 
                        {
                            introCam.m_Lens.FieldOfView = Mathf.Lerp(100f, 70f, camStillTimer * 0.5f); 
                        }
                    }
                }
            }
        }
        else if (GameObject.FindWithTag("Player") && GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused == false || cutScenePlaying == false)
        {
            //Debug.Log("intro cinematic finished");
            SwitchToPlatformCam(0.2f);
            introPlayed = true;
            cutScenePlaying = false;
        }
    }

}
