using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endGameTrigger : MonoBehaviour
{

    private audioManager m_audio;

    private void Start()
    {
        m_audio = GameObject.Find("AudioManager").GetComponent<audioManager>();
    }

    //unload the previous scene
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_audio.GetComponent<audioManager>().walkSource.enabled = false; // kill the player sounds
            GameObject.Find("WorldManager").GetComponent<mainGameScript>().EndGame();
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //get rid of the trigger area so it can't be triggered again
            this.gameObject.SetActive(false);

        }
    }
}
