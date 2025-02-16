using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ventScript : MonoBehaviour
{

    private PlayerController player;
    private Collider[] childColliders;
    private int collisionCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        childColliders = GetComponentsInChildren<Collider>();
    }

    void FixedUpdate()
    {
        //Debug.Log("in vent" + player.inVent);
        //if you have collided with 1 collider then you are inside, if you have collided with 2 then you are out again
        if (collisionCount == 1)
        {
            //do not allow unroll
            player.inVent = true;
        } 
        else if (collisionCount >= 2) 
        {
            //allow for unroll again
            collisionCount = 0;
            player.inVent = false;
        }
    }

    //check if the player is rolled, if not do not allow them entry
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player.rollCounter == 1)
            {
                //chnage to triggers so player can go through
                foreach (Collider col in childColliders)
                {
                    col.isTrigger = true;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            collisionCount++;
            //Debug.Log("We are rolling " + collisionCount);

            //chnage back to colliders
            foreach (Collider col in childColliders)
            {
                col.isTrigger = false;
            }
        }
    }
}
