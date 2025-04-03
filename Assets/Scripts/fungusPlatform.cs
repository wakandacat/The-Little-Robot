using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fungusPlatform : MonoBehaviour
{
    public bool postuleCollision = false;
    public GameObject postule;
    public GameObject deadPostule;
    public bool runOnce = false;
   
    
    //always check for quick drop collision
    void FixedUpdate()
    {
        breakPlatform();
    }

    //break the fungus on the platform
    public void breakPlatform()
    {
        if (postuleCollision == true && GameObject.FindWithTag("Player").GetComponent<PlayerController>().quickDropState == true && runOnce == false)
        {
            //Debug.Log("Hello");
            postule.SetActive(false);
            deadPostule.SetActive(true);
            GameObject.FindWithTag("Player").GetComponent<player_fx_behaviors>().Rumble(0.25f, 0.25f, 0.1f);
            this.transform.parent.GetComponent<movingPlatformScript>().isFungus = false; //allow the platform to move again

            //stop the random jitter
            StopCoroutine(this.transform.parent.GetComponent<movingPlatformScript>().JitterPlatform());
            this.transform.parent.GetComponent<movingPlatformScript>().jitterPlat = null;
            

            //play fungus dead sound
            if (this.transform.parent.GetComponent<AudioSource>().isPlaying == false)
            {
                this.transform.parent.GetComponent<AudioSource>().Play();
            }

            runOnce = true;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            postuleCollision = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        postuleCollision = false;
    }
}
