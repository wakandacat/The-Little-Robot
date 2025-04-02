using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{
    public GameObject player;
    public bool onGround = false;
    public bool jumpState = false;
    public bool doublejumpState = false;
    public bool runOnce = false;
    PlayerController jumping;
    public bool sfxRunOnce = false;
    public bool runRumbleOnce = false;
    // Start is called before the first frame update
    void Start()
    {
        jumping = player.GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Ground = " + onGround);
        if (onGround == true)
        {
            //jumpState = false;
            doublejumpState = false;
        }

    }

    public void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "crate")
        {
            onGround = true;
            jumping.quickDropStatetimer();
        }

    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground" || collision.gameObject.tag == "crate")
        {
            jumping.falling = false;
            runOnce = false;
            runRumbleOnce = false;
            jumping.handleJump();
            jumping.handleQuickDrop();
        }
/*        if(collision.gameObject.tag == "ground")
        {
            //this.GetComponent<player_fx_behaviors>().Rumble(0.25f, 0.25f, 0.1f);
        }*/

    }

    public void OnCollisionExit(Collision collision) { 

        onGround = false;
    }


}
