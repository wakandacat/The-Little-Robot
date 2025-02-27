using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endGameTrigger : MonoBehaviour
{

    private player_fx_behaviors m_audio;

    private void Start()
    {
        m_audio = GameObject.FindGameObjectWithTag("Player").GetComponent<player_fx_behaviors>();
    }

    //unload the previous scene
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_audio.StopCoroutine(m_audio.walkSFX()); // kill the player sounds
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
