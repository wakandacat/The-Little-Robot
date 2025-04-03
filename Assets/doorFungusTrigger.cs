using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorFungusTrigger : MonoBehaviour
{

    private bool runOnce = false;
    private player_fx_behaviors player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<player_fx_behaviors>();
    }
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
                player.Rumble(0.13f, 0.4f, 0.3f);
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
