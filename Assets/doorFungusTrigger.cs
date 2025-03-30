using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorFungusTrigger : MonoBehaviour
{

    private bool runOnce = false;

    public void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(collision.gameObject.GetComponent<PlayerController>().attackCounter == 3 && runOnce == false)
            {
                Debug.Log("inside trigger");
                runOnce = true;

                //open door
                this.transform.parent.GetChild(0).GetComponent<doorScript>().fungusOpen();
            }
        }
    }
}
