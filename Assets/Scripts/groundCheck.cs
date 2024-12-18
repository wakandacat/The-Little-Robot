using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundCheck : MonoBehaviour
{
    public GameObject player;
    public bool onGround = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Ground = " + onGround);

    }

    public void OnCollisionStay(Collision collision)
    {
      
        if (collision.gameObject.tag == "ground")
        {
            onGround = true;
            Debug.Log("Ground = " + onGround);
        }
       
    }

    public void OnCollisionExit(Collision collision)
    {
        onGround = false;
        Debug.Log("Ground = " + onGround);
    }
}
