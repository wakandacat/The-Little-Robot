using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class platformTriggerScript : MonoBehaviour
{
    private GameObject platform;
    private bool playerOn = false;
    private Vector3 lastPlatformPosition;
    private GameObject player;

    void Awake()
    {
        platform = transform.gameObject;
    }

    //get teh platform movement and send it to the player controller
    private void FixedUpdate()
    {
        Vector3 platformDelta = platform.transform.position - lastPlatformPosition;

        if (playerOn && player)
        {
            player.gameObject.GetComponent<PlayerController>().platformMovement = platformDelta;

        }

        lastPlatformPosition = platform.transform.position;
    }

    //trigger to determine if player is on platform
    private void OnTriggerEnter(Collider other)
    {
        //fi player collided
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("on platform");

            player = other.gameObject;
            playerOn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if player leaves
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("off platform");

            //immediatley set player's extra velocity to 0
            player.gameObject.GetComponent<PlayerController>().platformMovement = Vector3.zero;
            player = null;
            playerOn = false;
        }
    }
}
