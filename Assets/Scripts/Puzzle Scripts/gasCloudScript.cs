using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class gasCloudScript : MonoBehaviour
{
    //to use: inside gas cloud fungus prefab there is an empty gameobject with a box collider that can be resized using it's size attributes (don't touch the scale!)

    private PlayerController player;
    private GameObject playerCha;
    private float gasTimer = 0f;
    private float gasDamageTime = 1.5f;
    private bool cloudHit = false;
    public GameObject cloudfungushit;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        cloudfungushit = GameObject.Find("fungusCloudHitVFX");
        cloudfungushit.GetComponentInChildren<ParticleSystem>().Stop();
    }
    private void FixedUpdate()
    {
        PlayVFX();
    }

    public void PlayVFX()
    {
        if(cloudHit == true)
        {
            cloudfungushit.GetComponentInChildren<ParticleSystem>().Play();
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cloudHit = true;
            Debug.Log("enter" + gasTimer);
            gasTimer = 0f; //reset the timer

            //take one hit of damage immediately
            player.playerCurrenthealth--;
            player.canRegen = false;
            Debug.Log("player.canRegen" + player.canRegen);
            GameObject.Find("AudioManager").GetComponent<audioManager>().playPlayerSFX(11);
            

        }
    }

    //take damage over time
    public void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cloudHit = true;
            //start the timer
            gasTimer += Time.deltaTime;
            //Debug.Log("gas cloud" + gasTimer);

            player.canRegen = false;

            //after the delay, take damage
            if (gasTimer >= gasDamageTime)
            {
                player.playerCurrenthealth--;
                gasTimer = 0f; //reset the timer
                GameObject.Find("AudioManager").GetComponent<audioManager>().playPlayerSFX(11);
            }
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cloudHit = false;
            //Debug.Log("Player Stop Taking Damage");
            gasTimer = 0f; //reset the timer
            player.canRegen = true;

        }
    }
}
