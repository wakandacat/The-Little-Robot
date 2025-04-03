using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tendrilCollision : MonoBehaviour
{

    private PlayerController player;
    audioManager m_audio;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //get audio
        m_audio = GameObject.Find("AudioManager").GetComponent<audioManager>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && transform.root.gameObject.GetComponent<tendril_Behavior>().hasCollided == false)
        {
            transform.root.gameObject.GetComponent<tendril_Behavior>().hasCollided = true;
            m_audio.playPlayerSFX(10);
            player.collisionTendril = true;
            player.playerCurrenthealth -= 1;
            player.GetComponent<player_fx_behaviors>().Rumble(0.15f, 0.2f, 0.5f);

        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.collisionTendril = false;
        }
    }
}
