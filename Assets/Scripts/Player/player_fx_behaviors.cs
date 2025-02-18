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
    private PlayerController playerScript;
    groundCheck ground;

    //animation variables
    private Animator m_animator;
    private string state = "Idle";
    private bool Ball_in = false;


    //vfx variables
    private bool runOnce = false;
    public ParticleSystem landVfx;
    public ParticleSystem doubleJumpVfx;
    public ParticleSystem attack_1;
    public ParticleSystem attack_2;
    public ParticleSystem attack_3;

    //sfx variables

    // Start is called before the first frame update
    void Start()
    {
        //get player controller script, needed for accessing joystick inputs
        ground = player.GetComponent<groundCheck>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_animator = this.GetComponent<Animator>();
        state = "Idle";
        landVfx.Stop();
        doubleJumpVfx.Stop();
        attack_1.Stop();
        attack_2.Stop();
        attack_3.Stop();
        if (m_animator == null)
        {
            Debug.Log("this is null");
        }
        //StartCoroutine(turnOffAnim());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //animationCalls();
        vfx_triggers();
        var currentState = getPlayerState();
        if (currentState.Equals(state))
        {
            return;
        }
        state = currentState;
        m_animator.CrossFade(state, 0.1f, 0);
    }



    /*public void animationCalls()
    {
        //animation for walking
        if (m_animator != null)
        {
            //set playback speed for animation
            m_animator.SetFloat("walkSpeed", playerScript.leftStick.magnitude);
            m_animator.SetFloat("rollSpeed", playerScript.leftStick.magnitude);

            //if the player is moving then trigger the walk animation
            //update this code to keep player in ball if rolling
            if (playerScript.leftStick.magnitude > 0.1f)
            {
                m_animator.SetBool("isWalking", true);  
            }
            else
            {
                //end walking or rolling animations 
                m_animator.SetBool("isRolling", false);
                m_animator.SetBool("isWalking", false);
                m_animator.SetFloat("walkSpeed", 1.25f);
            }
         
            if (playerScript.rollCounter == 1)
            {
                m_animator.SetBool("isRolling", true);
            }
            else if (playerScript.rollCounter == 0)
            {
                m_animator.SetBool("isRolling", false);

            }
        }

    }*/
    public void animationCalls()
    {
        var currentState = getPlayerState();
        if (currentState.Equals(state))
        {
            return;
        }
        m_animator.CrossFade(this.state, 0.2f, 0);
    }
    /*    public void setBall_in()
        {
            if(playerScript.rollState == true)
            {
                Ball_in = true;
            }
        }*/
    public void vfx_triggers()
    {
        if (playerScript.jumpCounter == 1 && runOnce == false)
        {
            ground.runOnce = true;
            doubleJumpVfx.Play();
        }
        if (ground.onGround == true && ground.runOnce == false )
        {
            runOnce = false;
            ground.runOnce = true;
            landVfx.Play();
        }
        if (playerScript.attackCounter == 1)
        {
            attack_1.Play();
        }
        if (playerScript.attackCounter == 2)
        {
            attack_2.Play();

        }
        if (playerScript.attackCounter == 3)
        {
            attack_3.Play();

        }
    }
    //https://www.youtube.com/watch?v=ToGq1LCTqMw
    public string getPlayerState()
    {
        /*        if (Ball_in == true)
                {
                    Debug.Log("we are here ballin");
                    return state = "ball_in";
                }
                if (playerScript.rollState == true && playerScript.leftStick.magnitude > 0.1f)
                {
                    //Debug.Log("we are here roll");
                    return state = "roll";
                }
                *//*       
                        if (playerScript.rollCounter == 1 && playerScript.leftStick.magnitude > 0.1f)
                        {
                            Debug.Log("we are here roll");
                            return state = "roll";
                        }
                        if (playerScript.rollCounter == 0)
                        {
                            Debug.Log("we are here ball out");

                            return state = "ballOut";
                        }*/

        if (playerScript.leftStick.magnitude > 0.1f)
        {
            return "walk";
        }
        if (playerScript.isJumping == true)
        {
            return "jump";
        }
        if (ground.onGround == false && playerScript.falling == true)
        {
            return "Falling";
        }
        if (playerScript.attackCounter == 0)
        {
            return "Idle";
        }
        if (playerScript.attackCounter == 1)
        {
            return "Attack_1";
        }
        if (playerScript.attackCounter == 2)
        {
            return "Attack_2";
        }
        if (playerScript.attackCounter == 3)
        {
            return "Attack_3";
        }
/*        if (playerScript.canDash == true)
        {
            return "ball_in";
        }*/

        return "Idle";
    }

}
