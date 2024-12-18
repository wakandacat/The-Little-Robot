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
        
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ground")
        {
            onGround = true;
            Debug.Log("Ground = " + onGround);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        onGround = false;
    }
}
