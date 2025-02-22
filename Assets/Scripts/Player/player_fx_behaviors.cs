using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    public int attackCounter = 0;

    //sfx variables
    public audioManager m_audio;

    // Start is called before the first frame update
    void Start()
    {
        //get player controller script, needed for accessing joystick inputs
        rb = player.GetComponent<Rigidbody>();
        ground = player.GetComponent<groundCheck>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_animator = this.GetComponent<Animator>();
        m_audio = GameObject.Find("AudioManager").GetComponent<audioManager>();

        //animation
        state = "Idle";
        landVfx.Stop();
        doubleJumpVfx.Stop();
        attack_1.Stop();
        attack_2.Stop();
        attack_3.Stop();
        takeDamage.Stop();
        if (m_animator == null)
        {
            Debug.Log("this is null");
        }

        //sfx
        StartCoroutine(walkSFX());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vfx_triggers();
        var currentState = getPlayerState();
        if (currentState.Equals(state))
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
            m_audio.playPlayerSFX(9);
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
        if (playerScript.collision == true && playerScript.runTakeDamageOnce == false)
        {
            playerScript.runTakeDamageOnce = true;
            takeDamage.Play();
        }
    }

    //https://www.youtube.com/watch?v=ToGq1LCTqMw
    public string getPlayerState()
    {
        attackCounter = playerScript.attackCounter;
        //Attack State
        if (attackCounter == 1)
        {
            return "Attack_1";
        }
        if (attackCounter == 2)
        {
            return "Attack_2";
        }
        if (attackCounter == 3)
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
            //if(playerScript.Rolling != true)
            if(playerScript.leftStick.magnitude >= 0.1 && ground.onGround == true && playerScript.Rolling != true)
            {
                //m_audio.walkSource.PlayOneShot(m_audio.walkSource.clip);
                m_audio.walkSource.Play();
            }
            else
            {
                m_audio.walkSource.Stop();
            }

            yield return new WaitForSeconds(0.2f);
        }
        
    }
}
