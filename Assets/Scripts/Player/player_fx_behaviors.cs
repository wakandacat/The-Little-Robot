using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_fx_behaviors : MonoBehaviour
{
    //This script is for animation, sound effects, and visual effects
    //related to the player character

    public GameObject player;

    //animation variables
    private Animator m_animator;

    //vfx variables

    //sfx variables

    // Start is called before the first frame update
    void Start()
    {
        //player = this.GetComponent<GameObject>();
        m_animator = this.GetComponent<Animator>();

        if(player != null)
        {
            Debug.Log("got the " + player.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //m_animator.CrossFadeInFixedTime("Strike", 0.2f, 0, 0.2f);

    }

    //public void animationCalls()
    //{
    //    //animation for walking
    //    if (m_animator != null)
    //    {
    //        //set playback speed for animation
    //        m_animator.SetFloat("walkSpeed", player.leftStick.magnitude);

    //        //if the player is moving then trigger the walk animation
    //        if (player.leftStick.magnitude > 0.1f)
    //        {
    //            if (player.Rolling == true)
    //            {
    //                m_animator.SetBool("isRolling", true);
    //            }
    //            else
    //            {
    //                m_animator.SetBool("isRolling", false);
    //                m_animator.SetBool("isWalking", true);
    //            }

    //        }
    //        else
    //        {
    //            //end walking or rolling animations 
    //            m_animator.SetBool("isRolling", false);
    //            m_animator.SetBool("isWalking", false);
    //            m_animator.SetFloat("walkSpeed", 1.25f);
    //        }
    //    }

    //}

}
