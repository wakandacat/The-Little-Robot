using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class securityCamRotate : MonoBehaviour
{

    private Transform player;
    private mainGameScript mainScript;
    public bool inRadius = false;


    void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        mainScript = GameObject.Find("WorldManager").GetComponent<mainGameScript>();
    }

    void FixedUpdate()
    {
        if (player && mainScript.cutScenePlaying == false && inRadius == true)
        {
            updateRotation();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //fi player collided
        if (other.gameObject.tag == "Player")
        {
            inRadius = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //fi player collided
        if (other.gameObject.tag == "Player")
        {
            inRadius = false;
        }
    }

    public void updateRotation()
    {
        this.transform.LookAt(player); //make it look at the player       
    }
}
