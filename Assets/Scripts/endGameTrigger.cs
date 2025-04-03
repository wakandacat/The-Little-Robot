using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class endGameTrigger : MonoBehaviour
{

    private audioManager m_audio;
    player_fx_behaviors fxBehave;

    private mainGameScript mainGameScript;

    //end game cutscene
    public float endTimer = 0f;
    private float outroSpeed = 0.005f;
    private float camPos = 0;
    private GameObject endCutsceneGrp;
    private GameObject cutCanvas;

    private CinemachineFreeLook freeCam;
    private CinemachineVirtualCamera securityCam;
    private CinemachineVirtualCamera playerCam;
    private VideoPlayer staticVid;
    private GameObject staticCanvas;
    private AudioSource staticSound;
    private GameObject playerViewCanvas;
    private AudioSource doorAudio;
    private CinemachineVirtualCamera walkingCam;
    public GameObject camLookingUp;


    private GameObject player;
    private GameObject playerUI;
    public GameObject cutScenePlayer;
    public GameObject scientist;

    //credits
    public GameObject creditsCanvas;
    public GameObject scrollObj;
    public GameObject creditsBlack;
    private CinemachineVirtualCamera creditsCam;
    private bool fadeDone = false;
    public bool endCutscene = false;

    //Animation call booleans
    public bool startEndIdle = false;
    public bool playerLookingUp = false;
    public bool sci_startAnim = false;
    private bool grab_player = false;

    //dude vars
    public GameObject finalPos;

    Animator m_animator;
    Animator guy_animator;
    private void Start()
    {
        m_audio = GameObject.Find("AudioManager").GetComponent<audioManager>();
        fxBehave = GameObject.FindWithTag("Player").GetComponent<player_fx_behaviors>();
        mainGameScript = GameObject.Find("WorldManager").GetComponent<mainGameScript>();
        player = GameObject.Find("playerExport");
        cutScenePlayer = GameObject.Find("cutscenePlayer");
        playerUI = GameObject.Find("Player_UI");
        endCutsceneGrp = GameObject.Find("outroCutscene");

        freeCam = GameObject.Find("freeLookCam").GetComponent<CinemachineFreeLook>();
        creditsCam = GameObject.Find("creditsCam").gameObject.GetComponent<CinemachineVirtualCamera>();

        //DO NOT CHNAGE ORDER OF CHILDREN OH MAH GOD
        cutCanvas = endCutsceneGrp.transform.GetChild(0).gameObject;
        playerCam = endCutsceneGrp.transform.GetChild(1).gameObject.GetComponent<CinemachineVirtualCamera>();
        securityCam = endCutsceneGrp.transform.GetChild(2).gameObject.GetComponent<CinemachineVirtualCamera>();
        staticVid = endCutsceneGrp.transform.GetChild(3).gameObject.GetComponent<VideoPlayer>();
        staticCanvas = endCutsceneGrp.transform.GetChild(4).gameObject;
        staticSound = endCutsceneGrp.transform.GetChild(5).gameObject.GetComponent<AudioSource>();
        playerViewCanvas = endCutsceneGrp.transform.GetChild(7).gameObject;
        doorAudio = endCutsceneGrp.transform.GetChild(8).gameObject.GetComponent<AudioSource>();
        walkingCam = endCutsceneGrp.transform.GetChild(9).gameObject.GetComponent<CinemachineVirtualCamera>();

        if(cutScenePlayer != null)
        {
            m_animator = cutScenePlayer.gameObject.GetComponent<Animator>();

        }
        guy_animator = scientist.gameObject.GetComponent<Animator>();
        cutScenePlayer.SetActive(false);
        //scientist.SetActive(false);


    }
    public void hidePlayer()
    {
        player.transform.position = new Vector3(0.0f, 500.0f, 0.0f);
        if (cutScenePlayer != null)
        {
            cutScenePlayer.SetActive(true);

        }

    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.transform.position = new Vector3(0.0f, 500.0f, 0.0f);
            //Debug.Log("Player position " + player.transform.position);
            m_audio.walkSource.loop = false; //turn off looping before killing coroutine
            if (fxBehave.walkCoroutine != null)
            {
                //fxBehave.StopCoroutine(fxBehave.walkCoroutine);
                //fxBehave.walkCoroutine = null; // Clear reference after stopping
            }
            hidePlayer();
            mainGameScript.cutScenePlaying = true;
            //turn off gameplay values
            playerUI.SetActive(false);

            cutCanvas.SetActive(true); //bring up black canvas

            //stop animations???
            //stopping animation in player fx script this is just a boolean that calls that condition
            endCutscene = true;

            //player end animations
            //m_animator.SetBool("goToS1", true);

            //ensure cam is at end of path
            walkingCam.Follow = null;
            walkingCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_AutoDolly.m_Enabled = false;
            walkingCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = 1;

            //collision.gameObject.GetComponent<Animator>().enabled = false;

            playerCam.m_LookAt = camLookingUp.transform;

            //prep the player's stats
            playerViewCanvas.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "RESETS: " + mainGameScript.playerDeaths;
            playerViewCanvas.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "TIME: " + Mathf.FloorToInt(mainGameScript.playerTime / 60f) + ":" + Mathf.FloorToInt(mainGameScript.playerTime % 60f);
        }
    }


    void FixedUpdate()
    {

        //--------------INTRO CUTSCENE---------------
        if (mainGameScript.outroPlayed == false && mainGameScript.creditsPlaying == false)
        {
            OutroCam();
        } 
        else if (mainGameScript.outroPlayed == true && mainGameScript.creditsPlaying == true)
        {
            CreditsRoll();
        }

        if(sci_startAnim == true)
        {
            makeDudeMove();
        }

    }

    public void makeDudeMove()
    {
        float speed = 1.0f;
       // Debug.Log("Moving towawards player");
        //scientist.transform.position = Vector3.MoveTowards(scientist.transform.position, finalPos.transform.position, speed*Time.deltaTime);
        float dudeNewZ = Mathf.Lerp(scientist.transform.position.z, finalPos.transform.position.z, 0.01f);
        scientist.transform.position = new Vector3(scientist.transform.position.x, scientist.transform.position.y, dudeNewZ);
        //guy_animator.SetBool("guy_S1", true);
        //if (grab_player == true)
        //{

        //    guy_animator.SetBool("guy_S2", true);

        //}

    }
    public void OutroCam()
    {
        if (mainGameScript.cutScenePlaying) //left button on controller to skip cutscenes
        {
            if (GameObject.FindWithTag("Player") && GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused == false) //continue the cutscene if the game is not paused
            {
                //the cutscene is finished
                if (mainGameScript.outroPlayed != true)
                {
                    endTimer = endTimer + Time.deltaTime; //increment the cutscene timer                  
                    mainGameScript.outroPlaying = true;
                    m_animator.SetBool("goToS1", true);

                    //hide intro load
                    if (endTimer <= 0.5f)
                    {
                       // cutCanvas.SetActive(true);
                        //face player forward if not already
/*                        player.transform.position = this.transform.position; //center player
                        player.transform.rotation = this.transform.rotation;*/
                        mainGameScript.CheckPointResetPlatformCam(this.transform.eulerAngles.y); //freelook face forward --> not sure if needed

                        //Call player idle animation here
                        startEndIdle = true;

                        //switch to playercam first to show door
                        playerCam.Priority = walkingCam.Priority + 1;
                    }
                    else
                    {
                        cutCanvas.SetActive(false);

                        //after seeing door, cut to black again
                        if (endTimer >= 3f && endTimer < 3.1f)
                        {
                            cutCanvas.SetActive(true);

                            //switch to security cam
                            //https://discussions.unity.com/t/how-to-change-look-at-target-for-cinemachine/707973 cause Lina did not know this
                            securityCam.m_LookAt = cutScenePlayer.transform;
                            securityCam.Priority = playerCam.Priority + 1;

                            //bring up security overlay
                            staticCanvas.SetActive(true);

                            if (staticVid.isPlaying == false)
                            {
                                staticVid.Play(); //ensure the static video is playing
                            }

                            if (staticSound.isPlaying == false)
                            {
                                staticSound.Play(); //ensure the static sound is playing
                            }
                            

                        }

                        if (endTimer >= 3.1f && endTimer < 3.5f)
                        {
                            cutCanvas.SetActive(false); //hide black cut again
                            startEndIdle = false;
                            m_animator.SetBool("goToS2", true);

                        }
                        playerLookingUp = true;
                       // Debug.Log("player looking up " + playerLookingUp);

                        if (endTimer >= 3.5f && endTimer < 10f)
                        {
                            //open door sound
                            if (endTimer >= 3.9f && endTimer < 4.0f)
                            {
                                if (doorAudio.isPlaying == false)
                                {
                                    Debug.Log("door open");
                                    doorAudio.Play();
                                    scientist.SetActive(true); //ginette
                                    
                                }
                            } 
                            else if (endTimer >= 5.4f && endTimer < 5.5f)
                            {
                                doorAudio.Stop();
                            }  

                            //close door sound
                            if (endTimer >= 5.6f && endTimer < 5.7f)
                            {
                                if (doorAudio.isPlaying == false)
                                {
                                    Debug.Log("door close");
                                    doorAudio.Play();
                                }
                            }

                        }
                        if(endTimer >= 6f && endTimer < 6.1f)
                        {
                            
                            m_animator.SetBool("goToS3", true);
                           // Debug.Log("play 3rd shot");
                        }
                        
                        if (endTimer >= 8f && endTimer < 8.1f)
                        {
                            cutCanvas.SetActive(true);
                            sci_startAnim = true;
                            guy_animator.SetBool("guy_S1", true);
                            //switch to player cam
                            playerCam.Priority = securityCam.Priority + 1;

                            //hide security overlay
                            staticCanvas.SetActive(false);
                            if (staticVid.isPlaying)
                            {
                                staticVid.Stop(); //ensure the static video stops
                            }

                            if (staticSound.isPlaying)
                            {
                                staticSound.Stop(); //ensure the static sound stops
                            }

                        }

                        if (endTimer >= 8.1f && endTimer < 8.2f)
                        {
                            cutCanvas.SetActive(false); //hide black cut again
                            //Scientist start animation here
                            //sci_startAnim = true;

                        }

                        //move camera along track
                        if (endTimer >= 8.2f && endTimer < 18f)
                        {
                            camPos += Mathf.Lerp(0, 1, outroSpeed);
                            playerCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = camPos;

                            //once we hit the end of the track
                            if (camPos >= 0.87f && playerViewCanvas.activeSelf == false)
                            {
                                playerCam.LookAt = camLookingUp.transform;
                                playerViewCanvas.SetActive(true); //look through the player's eyes
                                playerCam.m_Lens.FieldOfView = 70;
                                //Scientist grab
                                //grab_player = true;
                                guy_animator.SetBool("guy_S2", true);
                                //static again
                                if (staticVid.isPlaying == false)
                                {
                                    staticCanvas.GetComponent<RawImage>().color = new Color32(142, 142, 142, 255);
                                    staticCanvas.SetActive(true);
                                    staticCanvas.transform.GetChild(0).gameObject.SetActive(false);
                                    staticVid.Play(); //ensure the static video is playing
                                }
                            }

                            //obscure transition with big static
                            if (endTimer >= 15f && endTimer < 15.25f) //this is the dude moving lol
                            {
                                staticCanvas.GetComponent<RawImage>().color = new Color32(142, 142, 142, 63);
                            }
                        }

                        if (endTimer >= 17f && endTimer < 21f)
                        {
                            //researcher grabs, big static
                            staticCanvas.GetComponent<RawImage>().color = new Color32(142, 142, 142, 255);
                            if (staticSound.isPlaying == false)
                            {
                                staticSound.Play(); //ensure the static sound is playing
                            }
                        }

                        if (endTimer >= 21f && endTimer < 23f)
                        {
                            //cut to black
                            cutCanvas.SetActive(true);

                            staticCanvas.SetActive(false);
                            if (staticVid.isPlaying)
                            {
                                staticVid.Stop(); //ensure the static video stops
                            }

                            if (staticSound.isPlaying)
                            {
                                staticSound.Stop(); //ensure the static sound stops
                            }

                            playerViewCanvas.SetActive(false); 
                        }

                        if (endTimer >= 23) //hang on black screen for a second before ending
                        {
                            playerCam.LookAt = null;
                            endTimer = 0; //reset teh timer
                            cutCanvas.SetActive(true);
                            mainGameScript.outroPlayed = true; //finish the cutscene
                            mainGameScript.OutroDone();
                        }               
                    }
                }                 
            }
        }
    }

    public void callEnd()
    {
        mainGameScript.EndGame();
    }

    public void CreditsRoll()
    {
        if (mainGameScript.cutScenePlaying) //left button on controller to skip cutscenes
        {
            if (GameObject.FindWithTag("Player") && GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused == false) //continue the cutscene if the game is not paused
            {
                //the cutscene is finished
                if (mainGameScript.creditsPlaying == true)
                {

                    if (creditsCanvas.activeSelf == false)
                    {

                        //ensure the outro canvases are not active
                        staticCanvas.SetActive(false);
                        if (staticVid.isPlaying)
                        {
                            staticVid.Stop(); //ensure the static video stops
                        }

                        if (staticSound.isPlaying)
                        {
                            staticSound.Stop(); //ensure the static sound stops
                        }

                        cutCanvas.SetActive(false);
                        creditsBlack.SetActive(true); //start on black screen
                        creditsCanvas.SetActive(true);

                        //switch to the end credits camera
                        creditsCam.Priority = playerCam.Priority + 1;
                    }

                    endTimer = endTimer + Time.deltaTime; //increment the cutscene timer      

                    //hang on title for a few seconds before moving
                    if (endTimer >= 3f && scrollObj.transform.localPosition.y < 8500) //if we have not yet hit the end logo, scroll
                    {
                        scrollObj.transform.localPosition += new Vector3(0, 100 * Time.deltaTime, 0);

                        //fade out the alpah on black background
                        if (endTimer >= 10f && fadeDone == false)
                        {
                            Color currentColor = creditsBlack.GetComponent<Image>().color;
                            currentColor.a -= Time.deltaTime * 0.3f;

                            if (currentColor.a <= 0)
                            {
                                currentColor.a = 0;
                                fadeDone = true;
                            }
                            creditsBlack.GetComponent<Image>().color = currentColor;
                        }
                        //fade background back in
                        else if (fadeDone == true && endTimer >= 55f)
                        {

                            Color currentColor = creditsBlack.GetComponent<Image>().color;
                            currentColor.a += Time.deltaTime * 0.3f;
                            if (currentColor.a >= 255)
                            {
                                currentColor.a = 255;
                            }
                            creditsBlack.GetComponent<Image>().color = currentColor;
                        }

                    } 
                    else if(scrollObj.transform.localPosition.y >= 8500)
                    {
                        Invoke("callEnd", 3f);
                    }
                }
            }
        }
    }

}
