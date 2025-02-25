using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

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
    public float introCamSpeed = 1f;
    private float camStillTimer = 0f;
    public CinemachineVirtualCamera securityCam;
    public GameObject securityCanvas;
    public GameObject playerUICanvas;
    public GameObject introCutCanvas;

    //cutscenes
    public bool cutScenePlaying = true; //toggle for when menus open or cutscenes

    //player eye lights
    public GameObject playerSpotLight; //full intensity = 4
    public GameObject playerPointLight; //full intensity = 0.1

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

    //audio vars
    public audioManager m_audio;

    void Awake()
    {

       // DisableMouse(); //UNCOMMENT THIS FOR THE BUILD

        //load the first scene in addition to the base scene
        SceneManager.LoadScene("Tutorial", LoadSceneMode.Additive);

        //ensure time is running and we are not still paused
        Time.timeScale = 1.0f;
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused = false;
        

        //get teh game settings if they exist --> if gameplay started from MainMenu scene then it will exist, otherwise it wont
        if (GameObject.Find("GameSettings"))
        {
            gameSettings = GameObject.Find("GameSettings");
        }

        setSettings();

        //get the first checkpoint from the checkpoint group
        currCheckpoint = checkpointGrp.transform.GetChild(0).gameObject;

        //set background music
        m_audio = GameObject.Find("AudioManager").GetComponent<audioManager>();

        //callback once the scene is fully loaded
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void Start()
    {
        if (EventSystem.current)
        {
            //Debug.Log("I exist");
            EventSystem.current.SetSelectedGameObject(null);
            //set new default selected
            EventSystem.current.SetSelectedGameObject(demoEndFirstButton);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Tutorial")
        {
            // Set the Tutorial scene as active
            SceneManager.SetActiveScene(scene);

            // Remove the event listener to ensure it only runs once
            SceneManager.sceneLoaded -= OnSceneLoaded;

            //call for music
            m_audio.playBackgroundMusic(scene.name);
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

    }

    public void DisableMouse()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
        Mouse.current.MakeCurrent(); 
        InputSystem.DisableDevice(Mouse.current); 
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
        //onyl runs after intro cam
        if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
        {
            platformCam.Priority = introCam.Priority + 1;
            introCam.Priority = 10;
            securityCam.Priority = 10;
        } 
        else //runs every other time
        {
            platformCam.Priority = bossCam.Priority + 1;
        }

        //maybe a slightly better transition idk
        if (SceneManager.GetActiveScene().name.Contains("Combat"))
        {
            platformCam.m_XAxis.Value = bossCam.transform.rotation.x;
        }
        //Debug.Log("platform cam");

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
                    Debug.Log("intro cinematic finished");
                    SwitchToPlatformCam(0.2f);
                    introPlayed = true;
                    cutScenePlaying = false;
                    camStillTimer = 0;
                    playerPointLight.GetComponent<Light>().intensity = 0.1f;
                    playerSpotLight.GetComponent<Light>().intensity = 4.0f;
                    playerUICanvas.SetActive(true);
                    securityCanvas.SetActive(false);
                    introCutCanvas.SetActive(false);
                }
                else
                {
                    camStillTimer = camStillTimer + Time.deltaTime; //increment the cutscene timer

                    //wait a few seconds at the beginning before starting the movement
                    if (camStillTimer >= 3.0f)
                    {
                        //switch to introcam
                        introCam.Priority = securityCam.Priority + 1;
                        securityCanvas.SetActive(false);
                        introCutCanvas.SetActive(true);

                        //turn off black canvas cut
                        if (camStillTimer >= 3.3f)
                        {
                            introCutCanvas.SetActive(false);
                        }

                        if (camStillTimer >= 5.0f)
                        {
                            //start adjusting the fov
                            if (introCam.m_Lens.FieldOfView > 70)
                            {
                                introCam.m_Lens.FieldOfView = Mathf.Lerp(100f, 70f, (camStillTimer - 5f) / 7.0f);
                            }

                            //move the camera along the dolly track
                            camPos += Mathf.Lerp(0, 1, introCamSpeed);
                            introCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = camPos;
                            if (camStillTimer >= 6.0f)
                            {
                                //turn on the robot's eye
                                if (playerPointLight.GetComponent<Light>().intensity < 0.1f)
                                {
                                    playerPointLight.GetComponent<Light>().intensity += (introCamSpeed / 10);
                                }

                                //turn on the robot's eye
                                if (playerSpotLight.GetComponent<Light>().intensity < 4.0f)
                                {
                                    playerSpotLight.GetComponent<Light>().intensity += (introCamSpeed * 10);
                                }
                            }

                        }                                
                      
                    } 
                    else 
                    {
                      //start at security cam
                    }
                }
            }
        }
        //user skipped cutscene with skip button
        else if (GameObject.FindWithTag("Player") && GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused == false || cutScenePlaying == false)
        {
            //Debug.Log("intro cinematic finished");
            SwitchToPlatformCam(0.2f);
            introPlayed = true;
            cutScenePlaying = false;
            playerPointLight.GetComponent<Light>().intensity = 0.1f;
            playerSpotLight.GetComponent<Light>().intensity = 4.0f;
            playerUICanvas.SetActive(true);
            securityCanvas.SetActive(false);
            introCutCanvas.SetActive(false);

        }
    }

}
