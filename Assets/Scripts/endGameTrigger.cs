using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endGameTrigger : MonoBehaviour
{

    private audioManager m_audio;
    player_fx_behaviors fxBehave;

    private void Start()
    {
        m_audio = GameObject.Find("AudioManager").GetComponent<audioManager>();
        fxBehave = GameObject.FindWithTag("Player").GetComponent<player_fx_behaviors>();
    }

    //unload the previous scene
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_audio.walkSource.loop = false; //turn off looping before killing coroutine
            if (fxBehave.walkCoroutine != null)
            {
                fxBehave.StopCoroutine(fxBehave.walkCoroutine);
                fxBehave.walkCoroutine = null; // Clear reference after stopping
            }
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
