using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gasParticleScript : MonoBehaviour
{

    public GameObject particleSys;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
          //  Debug.Log("turning gas on");
            //particleSys.GetComponent<ParticleSystem>().Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
           // Debug.Log("turning gas off");
            //particleSys.GetComponent<ParticleSystem>().Stop();
        }
    }
}
