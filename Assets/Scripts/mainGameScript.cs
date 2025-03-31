using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.Video;

public class mainGameScript : MonoBehaviour
{
    public int playerDeaths = 0; //stats
    public float playerTime = 0; //https://discussions.unity.com/t/how-to-make-a-timer-that-counts-up-in-seconds-as-an-int/147546
    //timer += Time.deltaTime; int seconds = timer % 60;

    //main game settings from main menu
    private GameObject gameSettings;
    public GameObject settingsMenu;

    //input system
    public InputActionAsset inputActions;

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
    private float camPos = 0;
    private float introCamSpeed = 0.003f;
    private float camStillTimer = 0f;
    public CinemachineVirtualCamera securityCam;
    public GameObject securityCanvas;
    public GameObject playerUICanvas;
    public GameObject introCutCanvas;

    //cutscenes
    public bool cutScenePlaying = true; //toggle for cutscenes to prevent player controller

    //intro
    bool introPlayed = false;
    public GameObject introStatic;
    public GameObject mainAudioMan;
    public GameObject introSplashScreen;
    public GameObject introStaticVideo;
    public GameObject splashScreenCanvas;
    public bool wakeupAnim = false;
    public bool ballform = false;

    //ending
    public bool outroPlayed = false;
    public bool outroPlaying = false;
    public bool creditsPlaying = false;
    public CinemachineVirtualCamera securCamEnd;
    public CinemachineVirtualCamera walkingCamEnd;
    public CinemachineVirtualCamera playerViewCamEnd;
    public GameObject endSecurityCanvas;
    public GameObject endPlayerCanvas;
    public GameObject endDoorAudio;
    public GameObject endStaticAudio;

    //vent cams
    public CinemachineVirtualCamera ventCam1;
    public CinemachineVirtualCamera ventCam2;
    public CinemachineVirtualCamera ventCam3;

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

        DisableMouse(); 

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

        //reset stats
        playerDeaths = 0;
        playerTime = 0;

        if (EventSystem.current)
        {
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
            settingsMenu.GetComponentsInChildren<Slider>()[0].value = gameSettings.GetComponent<GameSettings>().freelookSens;

            AudioListener.volume = gameSettings.GetComponent<GameSettings>().gameVolume * 2;
            settingsMenu.GetComponentsInChildren<Slider>()[1].value = gameSettings.GetComponent<GameSettings>().gameVolume;
        } 
        else
        {
            //set the defaul freelook sensitivity if gamesettings not chosen
            platformCam.m_XAxis.m_MaxSpeed = 300 * 0.5f; //middle is 150
            platformCam.m_YAxis.m_MaxSpeed = 4 * 0.5f; //middle is 2
            settingsMenu.GetComponentsInChildren<Slider>()[0].value = 0.5f;

            AudioListener.volume = 1f;
            settingsMenu.GetComponentsInChildren<Slider>()[1].value = 0.5f;
        }
    }

    void FixedUpdate()
    {
        playerTime += Time.deltaTime; //record the player's time

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

    public void SkipCutScene()
    {
        if (cutScenePlaying == true)
        {
            cutScenePlaying = false;
            wakeupAnim = false;
            ballform = false;

            if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
            {
                Debug.Log("intro skipped");
                introPlayed = true;
                IntroDoneResets();
            }

            if (SceneManager.GetActiveScene().name.Contains("EndScene") && outroPlaying == true)
            {
                Debug.Log("outro skipped");
                outroPlayed = true;
                OutroDone();
            } 
            else if (SceneManager.GetActiveScene().name.Contains("EndScene") && outroPlaying == false) //MIGHT NOT WORK ON REPLAY?????
            {
                Debug.Log("credits skipped");
                creditsPlaying = false;
                EndGame();
            }
        }
        
    }

    public void IntroDoneResets()
    {
        Debug.Log("intro cinematic finished");
        SwitchToPlatformCam(0.2f);
        introPlayed = true;
        cutScenePlaying = false;
        wakeupAnim = false;
        ballform = false;
        playerPointLight.GetComponent<Light>().intensity = 0.1f;
        playerSpotLight.GetComponent<Light>().intensity = 4.0f;
        playerUICanvas.SetActive(true);
        securityCanvas.SetActive(false);
        introCutCanvas.SetActive(false);
        splashScreenCanvas.SetActive(false);
        introStatic.SetActive(false);
        mainAudioMan.GetComponent<audioManager>().playerSource.enabled = true; //once cutscene is done, turn on the player sound effects
    }

    public void OutroDone()
    {
        Debug.Log("outro cinematic finished");
        outroPlayed = true;
        outroPlaying = false;
        playerPointLight.GetComponent<Light>().intensity = 0.0f;
        playerSpotLight.GetComponent<Light>().intensity = 0.0f;    
        mainAudioMan.GetComponent<audioManager>().playerSource.enabled = false; //turn off the player sound effects

        //ensure none of this is playing
        endSecurityCanvas.SetActive(false);
        endPlayerCanvas.SetActive(false);
        endDoorAudio.GetComponent<AudioSource>().Stop();
        endStaticAudio.GetComponent<AudioSource>().Stop();

        //start the end credits
        creditsPlaying = true;
        cutScenePlaying = true;
        if (GameObject.Find("endSceneStartObj"))
        {
            GameObject.Find("endSceneStartObj").GetComponent<endGameTrigger>().endTimer = 0; //reset teh timer
        }
    }

    //go back to main menu
    public void EndGame()
    {
        cutScenePlaying = false;
        creditsPlaying = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void SwitchToBossCam()
    {
        //Debug.Log("battle cam");
        enemy = GameObject.FindGameObjectWithTag("Boss Enemy");

        //move the track to teh enemy's position
        Vector3 bossPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 13, enemy.transform.position.z);
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
            //ensure outro cams are not active
            securCamEnd.Priority = 5;
            playerViewCamEnd.Priority = 6;
            walkingCamEnd.Priority = 7;

            //ensure vent cams are not active
            ventCam1.Priority = 10;
            ventCam2.Priority = 10;
            ventCam3.Priority = 10;

            platformCam.Priority = bossCam.Priority + 1; 
        }

        platformCam.m_YAxis.Value = yaxis; //position up teh spine axis

        usingBossCam = false;
    }

    public void CheckPointResetPlatformCam(float rotation)
    {
        platformCam.m_XAxis.Value = rotation;
        platformCam.ForceCameraPosition(transform.position, Quaternion.Euler(0, rotation, 0)); //force freelook to have player's rotation
        platformCam.m_YAxis.Value = 0.4f; //position up teh spine axis
    }

    //intro cutscene
    public void IntroCam()
    {
        if (cutScenePlaying) //left button on controller to skip cutscenes
        {
            if (GameObject.FindWithTag("Player") && GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused == false) //continue the cutscene if the game is not paused
            {
               
               
                if (introCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition >= 1 && introPlayed == false)
                {
                   // Debug.Log("whatttt");
                    IntroDoneResets(); //end of cutscene
                }
                else
                {
                    camStillTimer = camStillTimer + Time.deltaTime; //increment the cutscene timer                  

                    //hide intro load
                    if (camStillTimer <= 0.2f)
                    {
                        introCutCanvas.SetActive(true);
                    }
                    else
                    {
                        introCutCanvas.SetActive(false);
                    }

                    //SPLASHSCREEN INTRO PLAYS HERE FIRST --> ~10seconds

                    //once our splashscreen intro is finished
                    if (introSplashScreen.GetComponent<VideoPlayer>().isPlaying == false)
                    {
                        ballform = true;
                        introSplashScreen.SetActive(false); //turn it off
                        splashScreenCanvas.SetActive(false);

                        //hide intro load
                        if (camStillTimer <= 11.7f)
                        {
                            introCutCanvas.SetActive(true); //cut to black
                        }
                        else
                        {
                            introCutCanvas.SetActive(false);

                            //STATIC INTRO PLAYS HERE NEXT --> ~5seconds
                            securityCanvas.SetActive(true);
                            introStaticVideo.GetComponent<VideoPlayer>().Play();

                            if (introStatic.GetComponent<AudioSource>().isPlaying == false)
                            {
                                introStatic.GetComponent<AudioSource>().Play(); //ensure the static sound is playing
                            }

                        }          
                    }

                    //wait a few seconds at the beginning before starting the movement
                    if (camStillTimer >= 16.5f)
                    {
                        //switch to introcam
                        introCam.Priority = securityCam.Priority + 1;
                        securityCanvas.SetActive(false);
                        introCutCanvas.SetActive(true);

                        //stop the static noise
                        introStatic.SetActive(false);
                        introStaticVideo.SetActive(false); //hide static video

                        //turn off black canvas cut
                        if (camStillTimer >= 16.8f)
                        {
                            introCutCanvas.SetActive(false);
                        }

                        if (camStillTimer >= 18f)
                        {
                            //stop ball form and start wake up
                            ballform = false;
                            //Play wake up animation
                            wakeupAnim = true;
                            //move the camera along the dolly track
                            camPos += Mathf.Lerp(0, 1, introCamSpeed);
                            introCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = camPos;
                            if (camStillTimer >= 20f)
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
                            if (camStillTimer >= 24f)
                            {
                                wakeupAnim = false;
                            }

                        }


                    } 
                    //else 
                    //{
                    //  //start at security cam
                    //}
                }
            } 
        }
        ////user skipped cutscene with skip button
        //else 
        //{
        //    Debug.Log("do do do");
        //    IntroDoneResets();

        //}
    }

}
