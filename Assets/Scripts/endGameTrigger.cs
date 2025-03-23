using Cinemachine;
using System.Collections;
using System.Collections.Generic;
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
    private float outroSpeed = 0.003f;
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

    private GameObject player;
    private GameObject playerUI;

    //credits
    public GameObject creditsCanvas;
    public GameObject scrollObj;
    public GameObject creditsBlack;
    private CinemachineVirtualCamera creditsCam;
    private bool fadeDone = false;

    private void Start()
    {
        m_audio = GameObject.Find("AudioManager").GetComponent<audioManager>();
        fxBehave = GameObject.FindWithTag("Player").GetComponent<player_fx_behaviors>();
        mainGameScript = GameObject.Find("WorldManager").GetComponent<mainGameScript>();
        player = GameObject.FindGameObjectWithTag("Player");
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
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_audio.walkSource.loop = false; //turn off looping before killing coroutine
            if (fxBehave.walkCoroutine != null)
            {
                fxBehave.StopCoroutine(fxBehave.walkCoroutine);
                fxBehave.walkCoroutine = null; // Clear reference after stopping
            }

            mainGameScript.cutScenePlaying = true;

            //turn off gameplay values
            playerUI.SetActive(false);
            //stop animations???
            //collision.gameObject.GetComponent<Animator>().enabled = false;

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

                    //hide intro load
                    if (endTimer <= 0.5f)
                    {
                        cutCanvas.SetActive(true);
                        //face player forward if not already
                        player.transform.position = this.transform.position; //center player
                        player.transform.rotation = this.transform.rotation;
                        mainGameScript.CheckPointResetPlatformCam(this.transform.eulerAngles.y); //freelook face forward --> not sure if needed

                        //switch to playercam first to show door
                        playerCam.Priority = freeCam.Priority + 1;
                    }
                    else
                    {
                        cutCanvas.SetActive(false);

                        //after seeing door, cut to black again
                        if (endTimer >= 3f && endTimer < 3.1f)
                        {
                            cutCanvas.SetActive(true);

                            //switch to security cam
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
                        }

                        if (endTimer >= 3.5f && endTimer < 10f)
                        {
                            //open door sound
                            if (endTimer >= 4.5f && endTimer < 4.6f)
                            {
                                if (doorAudio.isPlaying == false)
                                {
                                    Debug.Log("door open");
                                    doorAudio.Play();
                                }
                            }

                            //close door sound
                            if (endTimer >= 5.5f && endTimer < 5.6f)
                            {
                                if (doorAudio.isPlaying == false)
                                {
                                    Debug.Log("door close");
                                    doorAudio.Play();
                                }
                            }

                        }

                        if (endTimer >= 8f && endTimer < 8.1f)
                        {
                            cutCanvas.SetActive(true);

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
                        }

                        //move camera along track
                        if (endTimer >= 8.2f && endTimer < 18f)
                        {
                            camPos += Mathf.Lerp(0, 1, outroSpeed);
                            playerCam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = camPos;

                            //once we hit the end of the track
                            if (camPos >= 0.87f && playerViewCanvas.activeSelf == false)
                            {
                                playerViewCanvas.SetActive(true); //look through the player's eyes

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
                            if (endTimer >= 15f && endTimer < 16f)
                            {
                                staticCanvas.GetComponent<RawImage>().color = new Color32(142, 142, 142, 63);
                            }
                        }

                        if (endTimer >= 18f && endTimer < 21f)
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
                    if (endTimer >= 3f && scrollObj.transform.localPosition.y < 6500) //if we have not yet hit the end logo, scroll
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
                        else if (fadeDone == true && endTimer >= 45f)
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
                    else if(scrollObj.transform.localPosition.y >= 6125)
                    {
                        Invoke("callEnd", 3f);
                    }
                }
            }
        }
    }

}
