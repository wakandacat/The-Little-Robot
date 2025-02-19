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

    //animation variables
    private Animator m_animator;

    //vfx variables

    //sfx variables

    // Start is called before the first frame update
    void Start()
    {
        //get player controller script, needed for accessing joystick inputs
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        m_animator = this.GetComponent<Animator>();



        //StartCoroutine(turnOffAnim());
    }

    // Update is called once per frame
    void Update()
    {
        animationCalls();
    }

    void FixedUpdate()
    {
        animationCalls();
    }

    void LateUpdate()
    {
        animationCalls();
    }

    public void animationCalls()
    {
        //animation for walking
        if (m_animator != null)
        {
            //set playback speed for animation
            m_animator.SetFloat("walkSpeed", playerScript.leftStick.magnitude);
            m_animator.SetFloat("rollSpeed", playerScript.leftStick.magnitude);

            //if the player is moving then trigger the walk animation
            //update this code to keep player in ball if rolling
            //if (playerScript.leftStick.magnitude > 0.1f)
            //{
            //    /* if (playerScript.rollCounter == 1)
            //     {
            //         m_animator.SetBool("isRolling", true);
            //     }
            //     else
            //     {
            //     }*/
            //    m_animator.SetBool("isWalking", true);

            //}
            //else
            //{
            //    //end walking or rolling animations 
            //    m_animator.SetBool("isRolling", false);
            //    m_animator.SetBool("isWalking", false);
            //    //m_animator.SetFloat("walkSpeed", 1.25f);
            //}

            //if (playerScript.rollCounter == 1)
            //{
            //    m_animator.SetBool("isRolling", true);
            //}
            //else if (playerScript.rollCounter == 0)
            //{
            //    m_animator.SetBool("isRolling", false);

            //}

            //if player is moving
            if (playerScript.leftStick.magnitude > 0.1f)
            {

                //check if in roll
                if (playerScript.rollCounter == 1)
                {
                    //roll
                    m_animator.SetTrigger("roll");
                    m_animator.SetBool("isRolling", true);
                }
                else if(playerScript.rollCounter == 0)
                {
                    m_animator.SetBool("isRolling", false);
                    m_animator.SetTrigger("walk");
                }
                else
                {
                    //walking
                    m_animator.SetTrigger("walk");
                }

            }
            else if(playerScript.rollCounter == 1)//no longer moving
            {
                m_animator.SetTrigger("roll");
                m_animator.SetBool("isRolling", true);

                ////check if in roll -> yes just pause
                //if(playerScript.rollCounter == 1)
                //{
                //    //roll
                //}
                //else //otherwise stop all movement
                //{
                //    //go to idle
                //    m_animator.SetTrigger("idle");
                //}

            }
            else
            {
                m_animator.SetFloat("walkSpeed", 1);
                m_animator.SetFloat("rollSpeed", 1);
                m_animator.SetBool("isRolling", false);
                m_animator.SetTrigger("idle");
                
            }
        }

    }

}
