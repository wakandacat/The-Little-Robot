using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartEndBattleScript : MonoBehaviour
{
    //camera shake ref: https://www.youtube.com/watch?v=ACf1I27I6Tk


    //get the bridges and the enemy
    public GameObject startBridge;
    public GameObject endBridge;
    public GameObject enemy;
    private mainGameScript mainGameScript;
    public GameObject loadObj;
    public CinemachineBlenderSettings regularBlend;
    public CinemachineBlenderSettings enemyDeadBlend;
    private CinemachineBrain camBrain;
    public GameObject enemyUI;
    public GameObject proceedLight;
    public AudioSource proceedSound;

    private float timer = 0;
    private float timeToFall = 2.0f;
    public float delay = 2.0f;
    private bool falling = false;
    public GameObject middlePlatform;
    private float timeStep = 0.01f;
    private Vector3 startPos;

    public GameObject battleCam;
    public GameObject lookAtWhenDead;
    public SpriteRenderer scientists;
    private Color m_color;

    public bool camShake = false;
    public player_fx_behaviors fxBehave;

    public Coroutine deathExplosions;
    void Awake()
    {
        mainGameScript = GameObject.Find("WorldManager").GetComponent<mainGameScript>();

        camBrain = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CinemachineBrain>();

        startPos = middlePlatform.transform.position;

        battleCam = GameObject.Find("battleCam");

    }

    private void Start()
    {
        fxBehave = GameObject.FindWithTag("Player").GetComponent<player_fx_behaviors>();
        if(fxBehave == null)
        {
            Debug.Log("scream");
        }
    }

    void FixedUpdate()
    {

        //if player dies during enemy death cutscene, stop all of it
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().deathState == true)
        {
            CancelInvoke("enemyDeadCutsceneStart");
            CancelInvoke("enemyDeadCutsceneEnd");
            CancelInvoke("SwitchBlend");
            CancelInvoke("startCamShake");
            return;
        }

        //check enemy's state here for death
        if (enemy.GetComponent<BossEnemy>().HP_ReturnCurrent() <= 0)
        {

            if (SceneManager.GetActiveScene().name == "Combat1" && mainGameScript.firstBossDead == false)
            {
                mainGameScript.firstBossDead = true;
                mainGameScript.m_audio.playBackgroundMusic("platform");

                enemyUI.SetActive(false);              

                //run explosion vfx for enemy death
                deathExplosions = StartCoroutine(enemy.GetComponent<BossEnemy>().VFX_DeathExplosions());

                mainGameScript.currLevelCount++;

                //cutscene chunks
                Invoke("startCamShake", 0.8f);
                Invoke("enemyDeadCutsceneStart", 2.7f);

                Invoke("enemyDeadCutsceneEnd", 6.0f);

                Invoke("SwitchBlend", 7.0f); //switch cameras after a delay

            }
            else if (SceneManager.GetActiveScene().name == "Combat2" && mainGameScript.secondBossDead == false)
            {
                mainGameScript.secondBossDead = true;
                mainGameScript.m_audio.playBackgroundMusic("platform");

                enemyUI.SetActive(false);

                //run explosion vfx for enemy death
                deathExplosions = StartCoroutine(enemy.GetComponent<BossEnemy>().VFX_DeathExplosions());

                mainGameScript.currLevelCount++;

                //cutscene chunks
                Invoke("startCamShake", 0.8f);
                Invoke("enemyDeadCutsceneStart", 2.7f);

                Invoke("enemyDeadCutsceneEnd", 6.0f);

                Invoke("SwitchBlend", 7.0f); //switch cameras after a delay
            }
            else if (SceneManager.GetActiveScene().name == "Combat3" && mainGameScript.thirdBossDead == false)
            {
                mainGameScript.thirdBossDead = true;
                mainGameScript.m_audio.playBackgroundMusic("platform");

                enemyUI.SetActive(false);

                //run explosion vfx for enemy death
                deathExplosions = StartCoroutine(enemy.GetComponent<BossEnemy>().VFX_DeathExplosions());

                mainGameScript.currLevelCount++;

                //cutscene chunks
                Invoke("startCamShake", 0.8f);
                Invoke("enemyDeadCutsceneStart", 2.7f);

                Invoke("enemyDeadCutsceneEnd", 6.0f);

                Invoke("SwitchBlend", 7.0f); //switch cameras after a delay
            }

            //enemy.GetComponent<boss_fx_behaviors>().StopCoroutine(enemy.GetComponent<boss_fx_behaviors>().turnOffEyes()); //ginette

            //middle platform fall thru
            if (timer < (timeToFall + delay) && falling)
            {
                timer += timeStep;
                //if the timer is greater than the delay time and not yet at the full time, then lerp the platform away
                if (timer >= delay && timer <= (timeToFall + delay))
                {
                    platformFall();
                }

                //if the timer is past the time + delay then reset the flag
                if (timer >= (timeToFall + delay))
                {
                    falling = false;
                   
                }
            }

        }

    }

    public void enemyDeadCutsceneStart()
    {
        //Debug.Log("enemy dead blend because boss dead");
        camBrain.m_CustomBlends = enemyDeadBlend; //switch blend

        battleCam.GetComponent<CinemachineVirtualCamera>().LookAt = lookAtWhenDead.transform; // no longer look at the enemy, he boutta plummet

        //tilt platform a little under enemy's weight
        middlePlatform.transform.position = new Vector3(startPos.x, startPos.y - 1.5f, startPos.z);
        middlePlatform.transform.Rotate(0f, 0f, 5f);

        //push the robot away --> explosion or something
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().combatPush(battleCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition, enemy.transform.position);

        platformFall();

    }

    public void enemyDeadCutsceneEnd()
    {
        stopShakeCam();
        enemy.SetActive(false);
        middlePlatform.SetActive(false);

        //open end bridge
        endBridge.GetComponent<bridgeScript>().moveBridgeLeft();
        startBridge.GetComponent<bridgeScript>().moveBridgeRight();

        if (startBridge.GetComponent<AudioSource>().isPlaying == false)
        {
            startBridge.gameObject.GetComponent<AudioSource>().Play();
        }

        if (endBridge.GetComponent<AudioSource>().isPlaying == false)
        {
            endBridge.gameObject.GetComponent<AudioSource>().Play();
        }
        //turn on exit light
        proceedLight.SetActive(true);
        m_color = new Color(255.0f, 255.0f, 255.0f, 0.25f);
        scientists.color = m_color;
        proceedSound.Play();
    }

    public void platformFall()
    {
        falling = true;

        float newY = Mathf.Lerp(startPos.y, startPos.y - 70f, (timer - delay));
        middlePlatform.transform.position = new Vector3(startPos.x, newY, startPos.z);
        if (middlePlatform.transform.position.y < startPos.y - 8f)
        {
            //float eneY = Mathf.Lerp(startPos.y, startPos.y - 49f, 0.5f);
            //enemy.transform.position = new Vector3(startPos.x, eneY, startPos.z);
            float enemyFallStart = Mathf.Max(0, (timer - 0.5f) * 0.5f);
            float eneY = Mathf.Lerp(startPos.y, startPos.y - 190f, enemyFallStart);
            enemy.transform.position = new Vector3(startPos.x, eneY, startPos.z);
        }
        else
        {
            enemy.transform.position = new Vector3(startPos.x, startPos.y+1, startPos.z);
        }
        //enemy.transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    public void shakeCam(float intensity)
    {
        battleCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
    }

    public void startCamShake() 
    {
        camShake = true;
        shakeCam(2f);
        fxBehave.Rumble(0.2f, 0.7f, 2.0f);

    }

    public void stopShakeCam()
    {
        camShake = false;
        battleCam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }

    //if boss is dead but you've respawned in combat area
    public void bossDefeated()
    {
        camBrain.m_CustomBlends = regularBlend; //switch blend
        enemy.SetActive(false);
        loadObj.SetActive(true);
        proceedLight.SetActive(true);
        m_color = new Color(255.0f, 255.0f, 255.0f, 0.25f);
        scientists.color = m_color;
        this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 100f, this.gameObject.transform.position.z);
        middlePlatform.SetActive(false);
        if(deathExplosions != null)
        {
            StopCoroutine(deathExplosions);
            deathExplosions = null;
        }
        
    }

    public void SwitchBlend()
    {
        mainGameScript.CheckPointResetPlatformCam(battleCam.transform.eulerAngles.y); //force freelook to bosscam rotation

        //switch cameras
        mainGameScript.SwitchToPlatformCam(0.4f);

        loadObj.SetActive(true); //activate trigger for next area at this point and not any earlier

        //camBrain.m_CustomBlends = regularBlend;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("switch to boss cam");

            //ensure cam shake is off
            stopShakeCam();

            //hide the bridges
            startBridge.GetComponent<bridgeScript>().moveBridgeLeft();
            endBridge.GetComponent<bridgeScript>().moveBridgeRight();

            if (startBridge.GetComponent<AudioSource>().isPlaying == false)
            {
                startBridge.gameObject.GetComponent<AudioSource>().Play();
            }

            if (endBridge.GetComponent<AudioSource>().isPlaying == false)
            {
                endBridge.gameObject.GetComponent<AudioSource>().Play();
            }

            //switch cameras
            mainGameScript.SwitchToBossCam();

            //play battle music
            mainGameScript.m_audio.playBackgroundMusic(SceneManager.GetActiveScene().name);

            //Bool to show the enemy UI

            enemyUI.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //move itself so it can't be triggered again
            Vector3 currentPosition = this.transform.position;
            float newY = currentPosition.y + 100f;
            this.transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);
        }
    }
}
