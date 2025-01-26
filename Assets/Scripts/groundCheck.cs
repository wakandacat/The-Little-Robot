using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{
    public GameObject player;
    public bool onGround = false;
    public bool jumpState = false;
    public bool doublejumpState = false;
    PlayerController jumping;
    // Start is called before the first frame update
    void Start()
    {
        jumping = player.GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Ground = " + onGround);

    }

    public void OnCollisionStay(Collision collision)
    {
      
        if (collision.gameObject.tag == "ground")
        {
            onGround = true;
            jumpState = false;
            doublejumpState = false;
        }
       
    }
    public void OnCollisionEnter(Collision collision)
    {
        jumping.handleJump();
        jumping.handleQuickDrop();
    }

    public void OnCollisionExit(Collision collision)
    {
        onGround = false;
        
    }


}
