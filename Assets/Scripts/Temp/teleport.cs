using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class teleport : MonoBehaviour
{

    private int door;
    public GameObject doorGrp;
    private string checkpointName;
    private string sceneName;

    private audioManager AudioManager;
    private GameObject player;

    private void Awake()
    {
        Debug.Log("USE KEYPAD FOR DEV TELEPORT: 1 = Platform1, 2 = Combat1, 3 = Platform2, 4 = Combat2, 5 = Platform3, 6 = Combat3, 7 = EndScene");

        AudioManager = GameObject.Find("AudioManager").GetComponent<audioManager>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    //wait till its loaded to set as active
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //unregister the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));


        //move the player to the checkpoint

        if (player != null)
        {
            GameObject checkpt = GameObject.Find(checkpointName);
            this.GetComponent<mainGameScript>().currCheckpoint = checkpt;

            player.GetComponent<checkPointScript>().MoveToCheckpoint();
        }

        //endscene does not have a next door to open
        if (sceneName != "EndScene")
        {
            //update the door number
            this.GetComponent<mainGameScript>().doorNum = door;
        }

        //MOVE BATTLE TRACK NOW SO IT DOESNT CLIP WALLS LATER
        if (sceneName.Contains("Combat"))
        {
            GameObject enemy = GameObject.FindGameObjectWithTag("Boss Enemy");

            ////move the track to teh enemy's position
            Vector3 bossPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 13, enemy.transform.position.z);
            GameObject.Find("battleCamTrack").transform.position = bossPos;
            GameObject.Find("battleCam").GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = 0;
        }

    }

    //dev tool teleport to any scene
    public void Update()
    {
        if (SceneManager.GetActiveScene().name != "EndScene") //don't allow teleporting out of endscene bc it breaks everything lol
        {
            //platform 1
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
            {
                //Debug.Log("1 pressed");

                this.GetComponent<mainGameScript>().isTeleporting = true;

                if (this.GetComponent<mainGameScript>().cutScenePlaying == true)
                {
                    //this.GetComponent<mainGameScript>().IntroDoneResets();
                    this.GetComponent<mainGameScript>().SkipCutScene();
                }

                //set the current scene to be combat then move the player to that checkpoint
                this.GetComponent<mainGameScript>().currSceneName = 1;
                this.GetComponent<mainGameScript>().currentScene = "Platform1";

                //set the variables to be used in the sceneLoaded callback
                door = 1;
                checkpointName = "checkPointP1";
                sceneName = "Platform1";

                //unload all scenes
                UnloadEverything();

                SceneManager.LoadSceneAsync("Platform1", LoadSceneMode.Additive);

                SceneManager.sceneLoaded += OnSceneLoaded;

            }
            //combat 1
            else if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
            {
                // Debug.Log("2 pressed");

                this.GetComponent<mainGameScript>().isTeleporting = true;

                if (this.GetComponent<mainGameScript>().cutScenePlaying == true)
                {
                    this.GetComponent<mainGameScript>().SkipCutScene();
                }

                //set the current scene to be combat then move the player to that checkpoint
                this.GetComponent<mainGameScript>().currSceneName = 2;
                this.GetComponent<mainGameScript>().currentScene = "Combat1";

                //set the variables to be used in the sceneLoaded callback
                door = 2;
                checkpointName = "checkPointC1";
                sceneName = "Combat1";

                //unload all scenes
                UnloadEverything();

                SceneManager.LoadSceneAsync("Combat1", LoadSceneMode.Additive);

                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            //platform 2
            else if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
            {
                //Debug.Log("3 pressed");

                this.GetComponent<mainGameScript>().isTeleporting = true;

                if (this.GetComponent<mainGameScript>().cutScenePlaying == true)
                {
                    this.GetComponent<mainGameScript>().SkipCutScene();
                }

                //set the current scene to be combat then move the player to that checkpoint
                this.GetComponent<mainGameScript>().currSceneName = 3;
                this.GetComponent<mainGameScript>().currentScene = "Platform2";

                //set the variables to be used in the sceneLoaded callback
                door = 3;
                checkpointName = "checkPointP2";
                sceneName = "Platform2";

                //unload all scenes
                UnloadEverything();

                SceneManager.LoadSceneAsync("Platform2", LoadSceneMode.Additive);

                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            //combat 2
            else if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
            {
                //Debug.Log("4 pressed");

                this.GetComponent<mainGameScript>().isTeleporting = true;

                if (this.GetComponent<mainGameScript>().cutScenePlaying == true)
                {
                    this.GetComponent<mainGameScript>().SkipCutScene();
                }

                //set the current scene to be combat then move the player to that checkpoint
                this.GetComponent<mainGameScript>().currSceneName = 4;
                this.GetComponent<mainGameScript>().currentScene = "Combat2";

                //set the variables to be used in the sceneLoaded callback
                door = 4;
                checkpointName = "checkPointC2";
                sceneName = "Combat2";

                //unload all scenes
                UnloadEverything();

                SceneManager.LoadSceneAsync("Combat2", LoadSceneMode.Additive);

                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            //platform 3
            else if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
            {
                //Debug.Log("5 pressed");

                this.GetComponent<mainGameScript>().isTeleporting = true;

                if (this.GetComponent<mainGameScript>().cutScenePlaying == true)
                {
                    this.GetComponent<mainGameScript>().SkipCutScene();
                }

                //set the current scene to be combat then move the player to that checkpoint
                this.GetComponent<mainGameScript>().currSceneName = 5;
                this.GetComponent<mainGameScript>().currentScene = "Platform3";

                //set the variables to be used in the sceneLoaded callback
                door = 5;
                checkpointName = "checkPointP3";
                sceneName = "Platform3";

                //unload all scenes
                UnloadEverything();

                SceneManager.LoadSceneAsync("Platform3", LoadSceneMode.Additive);

                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            //combat 3
            else if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6))
            {
                //Debug.Log("6 pressed");

                this.GetComponent<mainGameScript>().isTeleporting = true;

                if (this.GetComponent<mainGameScript>().cutScenePlaying == true)
                {
                    this.GetComponent<mainGameScript>().SkipCutScene();
                }

                //set the current scene to be combat then move the player to that checkpoint
                this.GetComponent<mainGameScript>().currSceneName = 6;
                this.GetComponent<mainGameScript>().currentScene = "Combat3";

                //set the variables to be used in the sceneLoaded callback
                door = 6;
                checkpointName = "checkPointC3";
                sceneName = "Combat3";

                //unload all scenes
                UnloadEverything();

                SceneManager.LoadSceneAsync("Combat3", LoadSceneMode.Additive);

                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            //endscene
            else if (Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Alpha7))
            {
                //Debug.Log("7 pressed");

                this.GetComponent<mainGameScript>().isTeleporting = true;

                if (this.GetComponent<mainGameScript>().cutScenePlaying == true)
                {
                    this.GetComponent<mainGameScript>().SkipCutScene();
                }

                //set the current scene to be combat then move the player to that checkpoint
                this.GetComponent<mainGameScript>().currSceneName = 7;
                this.GetComponent<mainGameScript>().currentScene = "EndScene";

                //set the variables to be used in the sceneLoaded callback
                door = 7;
                checkpointName = "checkPointEndScene";
                sceneName = "EndScene";

                //unload all scenes
                UnloadEverything();

                SceneManager.LoadSceneAsync("EndScene", LoadSceneMode.Additive);

                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                //do nothing
            }
        }
        

    }

    public void UnloadEverything()
    {
        //turn off sounds here
        AudioManager.enemyWhirringSource.enabled = false;
       // if (SceneManager.GetActiveScene().name != "EndScene")
        //{
        player.GetComponent<player_fx_behaviors>().StopCoroutine(player.GetComponent<player_fx_behaviors>().walkCoroutine);
        player.GetComponent<PlayerController>().platformMovement = Vector3.zero; //zero out moving platform velocity
        //}       

        //if we are in the final cutscene, revert it all
        if (player.GetComponent<PlayerController>().triggerFound == true)
        {
            player.GetComponent<PlayerController>().triggerFound = false;
        }

        if (player.GetComponent<PlayerController>().inVent == true)
        {
            player.GetComponent<PlayerController>().inVent = false;
        }

        //if there are any vents, reset their colliders
        if (GameObject.FindGameObjectsWithTag("vent_floor").Length > 0)
        {
            for (int i = 0; i < GameObject.FindGameObjectsWithTag("vent_floor").Length; i++)
            {
                //Debug.Log("resetting vents");
                GameObject.FindGameObjectsWithTag("vent_floor")[i].GetComponent<ventFloor>().ActivateColliders();
            }
        }

        //close all the doors
        for (int i = 0; i < doorGrp.transform.childCount; i++)
        {
            GameObject child = doorGrp.transform.GetChild(i).gameObject;

            //if the child has fungus, ensure it is reloaded
            if (child.transform.childCount > 1)
            {
                if (child.transform.GetChild(0).GetComponent<doorScript>().doorOpen == true)
                {
                    child.transform.GetChild(0).GetComponent<doorScript>().closeDoor();
                }
                child.transform.GetChild(1).gameObject.SetActive(true);
                child.transform.GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                if (child.GetComponent<doorScript>().doorOpen == true)
                {
                    child.GetComponent<doorScript>().closeDoor();
                }
            }
            
        }

        //unload the previous scenes
        if (SceneManager.GetSceneByName("Tutorial").isLoaded)
        {
            SceneManager.UnloadSceneAsync("Tutorial");
        };
        if (SceneManager.GetSceneByName("Platform1").isLoaded)
        {
            SceneManager.UnloadSceneAsync("Platform1");
        };
        if (SceneManager.GetSceneByName("Combat1").isLoaded)
        {
            SceneManager.UnloadSceneAsync("Combat1");
        };
        if (SceneManager.GetSceneByName("Platform2").isLoaded)
        {
            SceneManager.UnloadSceneAsync("Platform2");
        };
        if (SceneManager.GetSceneByName("Combat2").isLoaded)
        {
            SceneManager.UnloadSceneAsync("Combat2");
        };
        if (SceneManager.GetSceneByName("Platform3").isLoaded)
        {
            SceneManager.UnloadSceneAsync("Platform3");
        };
        if (SceneManager.GetSceneByName("Combat3").isLoaded)
        {
            SceneManager.UnloadSceneAsync("Combat3");
        };
        if (SceneManager.GetSceneByName("EndScene").isLoaded)
        {
            SceneManager.UnloadSceneAsync("EndScene");
        };
    }
}
