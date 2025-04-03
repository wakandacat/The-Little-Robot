using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class player_fx_behaviors : MonoBehaviour
{
    //This script is for animation, sound effects, and visual effects
    /** NOTE: attack sfx, vfx, and animation calls 
       * are in the player controller script
       * due to callback functionality
    **/
    //related to the player character
    public GameObject player;
    public Rigidbody rb;
    private PlayerController playerScript;
    groundCheck ground;
    public GameObject cloudFungus;
    private mainGameScript mainScript;
    private GameObject bridge;
    private bool left;
    private bool right;
    private GameObject endGameTrigger;
    private bool camShake;
    private GameObject unloadTrigger;
    private GameObject door;

    //animation variables
    private Animator m_animator;
    private string state = "Idle";

    //vfx variables
    private bool runOnce = false;
    public ParticleSystem landVfx;
    public ParticleSystem doubleJumpVfx;
    public ParticleSystem attack_1;
    public ParticleSystem attack_2;
    public ParticleSystem attack_3;
    public ParticleSystem takeDamage;
    public GameObject invulnerability;
    public ParticleSystem fungusHit;
    public ParticleSystem cloudfungushit;
    public ParticleSystem rollVfx;
    public ParticleSystem healthRegen;
    public int attackCounter = 0;

    //sfx variables
    public audioManager m_audio;
    public Coroutine walkCoroutine;
    public Coroutine dashVfx;

    //haptics variables
    Gamepad pad;
    private Coroutine stopRumbleCoroutine;
    private endGameTrigger endScene;
    private bool foundTrigger = false;


    public void Awake()
    {
        //set gamepad
        pad = Gamepad.current;
        stopRumbling();
    }

    void Start()
    {
        //get player controller script, needed for accessing joystick inputs
        rb = player.GetComponent<Rigidbody>();
        ground = player.GetComponent<groundCheck>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_animator = this.GetComponent<Animator>();
        m_audio = GameObject.Find("AudioManager").GetComponent<audioManager>();
        mainScript = GameObject.Find("WorldManager").GetComponent<mainGameScript>();
        door = GameObject.Find("DoorGroup");
        if(unloadTrigger != null)
        {
            unloadTrigger = GameObject.Find("unloadSceneObject");
        }
        
        //animation
        state = "Idle";
        landVfx.Stop();
        doubleJumpVfx.Stop();
        attack_1.Stop();
        attack_2.Stop();
        attack_3.Stop();
        takeDamage.Stop();
        invulnerability.SetActive(false);
        fungusHit.Stop();
        cloudfungushit.Stop();
        rollVfx.Stop();
        healthRegen.Stop();
        if (m_animator == null)
        {
            Debug.Log("this is null");
        }

        //sfx
        walkCoroutine = StartCoroutine(walkSFX());
    }
    public void findComabtSceneElements()
    {
        if (playerScript.combatState == true)
        {
            bridge = GameObject.Find("TermProject_bridge_end");
            left = bridge.gameObject.GetComponent<bridgeScript>().movingLeft;
            right = bridge.gameObject.GetComponent<bridgeScript>().movingRight;
            endGameTrigger = GameObject.Find("startBattleTrigger");
            camShake = endGameTrigger.gameObject.GetComponent<StartEndBattleScript>().camShake;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        findComabtSceneElements();
        vfx_triggers();
        RumbleConditions();
        var currentState = getPlayerState();
        if(state == "Falling" && ground.onGround == true)
        {
            m_audio.playPlayerSFX(9);
        }
        else if(currentState.Equals(state))
        {
            return;
        }
        if(currentState == "roll")
        {
            currentState = "roll";
        }
        state = currentState;
        m_animator.CrossFade(state, 0.1f, 0);

    }

    //https://www.youtube.com/watch?v=SmmBC-yCJ28
    //https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Gamepad.html
    public void Rumble(float low, float high, float duration)
    {
        if (pad != null)
        {
            pad.SetMotorSpeeds(low, high);
        }
        stopRumbleCoroutine = StartCoroutine(stopRumble(duration)); //maybe remove, we shouldn't need to start/stop it
        //Invoke("stopRumbling", 0.3f); //temp unless lina can't get the rumble to stop
        
    }
    public IEnumerator stopRumble(float duration)
    {
        float elapsedTime = 0.0f;
        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        pad.SetMotorSpeeds(0f, 0f);
    }
    //temp unless lina can't figure out stopping
    public void stopRumbling()
    {
        pad.SetMotorSpeeds(0.0f,0.0f);
    }
    public void RumbleConditions()
    {
        //you hit the enemy
        if (playerScript.attackCounter == 1 && playerScript.runAttack1Once == true && playerScript.enemyPlayerCollision == true)
        {
            Rumble(0.08f, 0.3f, 0.2f);
        }
        //you hit enemy 2
        if (playerScript.attackCounter == 2 && playerScript.runAttack2Once == true && playerScript.enemyPlayerCollision == true)
        {
            Rumble(0.1f, 0.3f, 0.2f);
        }
        //you hit enemy 3
        if (playerScript.attackCounter == 3 && playerScript.runAttack3Once == true && playerScript.enemyPlayerCollision == true)
        {
            Rumble(0.13f, 0.4f, 0.3f);
        }
        //if projectile hits you
        if (playerScript.collision == true)
        {
            Rumble(0.15f, 0.3f, 0.5f);
        }
        //you died
        if (playerScript.deathState == true)
        {
            Rumble(0.0f, 0.0f, 0.0f);
        }
        //bridge haptics
        if (left == true || right == true)
        {
            //Debug.Log("Here bridge haptics");
            Rumble(0.008f, 0.2f, 0.2f);
        }
        //camshake
        //if (camShake == true) //if this is used for boss death it needs to start and end slightly earlier 
        //{
        //    Rumble(0.2f, 0.7f, 0.5f);
        //}
        //door closing
        if (door.GetComponentInChildren<doorScript>().movingUp == true)
        {
            Debug.Log("doors are opening");
            Rumble(0.005f, 0.15f, 0.10f);
        }
        if (door.GetComponentInChildren<doorScript>().movingDown == true)
        {
            Debug.Log("doors are closing");
            Rumble(0.08f, 0.12f, 0.1f);
        }
        
    }
    public void OnApplicationQuit()
    {
        stopRumbling();
    }
    public IEnumerator playDashVfx()
    {
        rollVfx.Play();
        yield return new WaitForSeconds(0.2f);
        rollVfx.Stop();
    }

    //https://discussions.unity.com/t/playing-a-particle-system-through-script-c/610122
    public void vfx_triggers()
    {
        if (playerScript.jumpCounter == 1 && runOnce == false)
        {
            ground.runOnce = true;
            doubleJumpVfx.Play();
        }
        if (ground.onGround == true && ground.runOnce == false)
        {
            runOnce = false;
            ground.runOnce = true;
            landVfx.Play();
            //m_audio.playPlayerSFX(9); //land sfx
        }
        if (playerScript.attackCounter == 1 && playerScript.runAttack == false)
        {
            playerScript.runAttack = true;
            attack_1.Play();
        }
        if (playerScript.attackCounter == 2 && playerScript.runAttack == false)
        {
            playerScript.runAttack = true;
            attack_2.Play();
        }
        if (playerScript.attackCounter == 3 && playerScript.runAttack == false)
        {
            playerScript.runAttack = true;
            attack_3.Play();
        }
        if ((playerScript.collision == true || playerScript.projectilefungus == true) && playerScript.runTakeDamageOnce == false)
        {
            playerScript.runTakeDamageOnce = true;
            takeDamage.Play();
        }
        if(playerScript.immunity_on == true)
        {
            invulnerability.SetActive(true);
        }
        if(playerScript.immunity_on == false)
        {
            invulnerability.SetActive(false);
        }
        if(playerScript.collisionTendril == true)
        {
            fungusHit.Play();
        }
        else
        {
            fungusHit.Stop();
        }
        if(playerScript.rollCounter == 1 && playerScript.leftStick.magnitude >= 0.1f)
        {
            rollVfx.Play();
        }
        else if(playerScript.rollCounter == 1 && playerScript.leftStick.magnitude == 0.0f)
        {
            rollVfx.Stop();
        }
        else if(playerScript.rollCounter == 0)
        {
            rollVfx.Stop();
        }
        if(playerScript.isDashing == true)
        {
            dashVfx = StartCoroutine(playDashVfx());
        }
        if(playerScript.playerCurrenthealth < playerScript.playerHealth)
        {
            Debug.Log(playerScript.playerCurrenthealth);
            Debug.Log("we are regaining health");
            healthRegen.Play();
        }
        else
        {
            healthRegen.Stop();
        }
    }

    //https://www.youtube.com/watch?v=ToGq1LCTqMw
    public string getPlayerState()
    {
        attackCounter = playerScript.attackCounter;
        if(mainScript.ballform == true)
        {
            return "ball_in";
        }
        if (mainScript.wakeupAnim == true)
        {
            return "wakeup";
        }
        if (SceneManager.GetActiveScene().name == "EndScene")
        {
            if (playerScript.foundScene == true && playerScript.endScene.GetComponent<endGameTrigger>().startEndIdle == true)
            {
                return "Idle";
            }

            if (playerScript.foundScene == true && playerScript.endScene.GetComponent<endGameTrigger>().playerLookingUp == true)
            {
                return "LookingAround";
            }
        }

        //Attack State
        if (attackCounter == 1 && playerScript.runAttack1Once == true)
        {
            return "Attack_1";
        }
        if (attackCounter == 2 && playerScript.runAttack2Once == true)
        {
            return "Attack_2";
        }
        if (attackCounter == 3 && playerScript.runAttack3Once == true)
        {
            return "Attack_3";
        }
        //Roll State
        if (playerScript.leftStick.magnitude >= 0.1f && playerScript.rollCounter == 1 || (playerScript.leftStick.magnitude == 0.0f && playerScript.rollCounter == 1))
        {
            m_animator.SetFloat("rollSpeed", playerScript.leftStick.magnitude);
            return "roll";
        }
        //Dash State
        if ((playerScript.leftStick.magnitude > 0.1f && playerScript.isDashing == true) || playerScript.isDashing == true)
        {
            return "ball_in";
        }
        //Walk State
        if (playerScript.leftStick.magnitude > 0.1f && ground.onGround == true)
        {
            runOnce = false;
            m_animator.SetFloat("walkSpeed", playerScript.leftStick.magnitude);
            return "walk";
        }
        //Jump State
        if (playerScript.isJumping == true)
        {
            return "jump";
        }
        if (ground.onGround == false)
        {
            return "Falling";
        }
        if (ground.onGround == true && runOnce == false)
        {
            runOnce = true;
            return "Land";
        }
        //unroll when done with roll
        if (playerScript.rollCounter == 0)
        {
            m_animator.SetFloat("rollSpeed", 1);
            return "Idle";
        }
        return "Idle";
    }

    public IEnumerator walkSFX()
    {
        //for as long as player is using joystick
        while (true)
        {
            float timeToWait = 0.2f; //default time to wait
            //clip full length is 0.6f
            //if(playerScript.Rolling != true)
            if (playerScript.leftStick.magnitude >= 0.7f && ground.onGround == true && playerScript.Rolling != true)
            {
                timeToWait = 0.25f;
                m_audio.walkSource.Play();
            }
            else if(playerScript.leftStick.magnitude >= 0.4f && ground.onGround == true && playerScript.Rolling != true)
            {
                timeToWait = 0.35f;
                m_audio.walkSource.Play();
            }
            else if (playerScript.leftStick.magnitude >= 0.25f && ground.onGround == true && playerScript.Rolling != true)
            {
                timeToWait = 0.42f;
                m_audio.walkSource.Play();
            }
            else if(playerScript.leftStick.magnitude >= 0.1f && ground.onGround == true && playerScript.Rolling != true)
            {
                timeToWait = 0.5f;
                m_audio.walkSource.Play();
            }
            else
            {
                timeToWait = 0.1f;
                m_audio.walkSource.Stop();
            }

            yield return new WaitForSeconds(timeToWait);
        }
        
    }
}
