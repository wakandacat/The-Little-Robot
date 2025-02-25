using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float fadeDelay = 0.5f;
    public bool fadingIn = false;
    public bool fadingOut = false;

    public CanvasGroup fadeOutPanel;

    public GameObject player;
    private PlayerController playerController;

    void Awake()
    {
        playerController = player.GetComponent<PlayerController>(); 
    }

    void Update()
    {
        // Debug.Log("fadeout panel alpha: " + fadeOutPanel.alpha);
        //fading canvas ref: https://www.youtube.com/watch?v=Ox0JCbVIMCQ&ab_channel=ClipCollectionVault

        if (playerController.isFading) //fading state from playercontroller
        {
            if (fadingIn)
            {
                fadeOutPanel.alpha += fadeDelay * Time.deltaTime;
                if (fadeOutPanel.alpha >= 1) // fully faded in
                {
                    fadeOutPanel.alpha = 1; // stops at 1
                    fadingIn = false;

                    //move player
                    playerController.playerCurrenthealth = playerController.playerHealth;
                    player.GetComponent<checkPointScript>().MoveToCheckpoint();

                    fadingOut = true; //wait a frame
                }
            }
            else if (fadingOut)
            {
                fadeOutPanel.alpha -= fadeDelay * Time.deltaTime;
                if (fadeOutPanel.alpha <= 0) //fully faded out
                {
                    fadeOutPanel.alpha = 0; //stop at 0
                    fadingOut = false;
                    playerController.isFading = false; // reset fade state
                }
            }
        }
    }

    public void ResetCanvas()
    {
        //Debug.Log("resetting canvas");
        fadeOutPanel.alpha = 0;
        fadingIn = false;
        fadingOut = false;
        playerController.isFading = false;
    }

}
