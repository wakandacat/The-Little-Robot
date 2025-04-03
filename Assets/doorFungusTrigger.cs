using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorFungusTrigger : MonoBehaviour
{

    private bool runOnce = false;
    private int previousCounter = 0;

    public void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(collision.gameObject.GetComponent<PlayerController>().attackCounter == 3 && runOnce == false)
            {
                runOnce = true;

                //open door
                this.transform.parent.GetChild(0).GetComponent<doorScript>().fungusOpen();
            }
            else if(collision.gameObject.GetComponent<PlayerController>().attackCounter == 1 || collision.gameObject.GetComponent<PlayerController>().attackCounter == 2)
            {
                if(collision.gameObject.GetComponent<PlayerController>().attackCounter != previousCounter)
                {
                    this.GetComponent<AudioSource>().PlayOneShot(this.GetComponent<AudioSource>().clip);
                }
            }

            previousCounter = collision.gameObject.GetComponent<PlayerController>().attackCounter;
        }
    }
}
