using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public int fadeDelay = 1;
    public bool fadein = false;
    public bool fadeout = false;

    public CanvasGroup fadeOutPanel;

    public GameObject player;
    private PlayerController playerController;

    void Awake()
    {
        playerController = player.GetComponent<PlayerController>(); 
    }

    void Update()
    {

        //fading canvas ref: https://www.youtube.com/watch?v=Ox0JCbVIMCQ&ab_channel=ClipCollectionVault

        //fading in the canvas to black
        if (playerController.fadingIn == true)
        {
            if (fadeOutPanel.alpha < 1)
            {
                fadeOutPanel.alpha += fadeDelay * Time.deltaTime;
                if (fadeOutPanel.alpha >= 1)
                {
                    playerController.fadingIn = false;
                    playerController.fadeOut();
                }
            }
        }

        //fading out the black back to invisible
        if (playerController.fadingOut == true)
        {
            if (fadeOutPanel.alpha >= 0)
            {
                fadeOutPanel.alpha -= fadeDelay * Time.deltaTime;
                if (fadeOutPanel.alpha == 0)
                {
                    playerController.fadingOut = false;
                }
            }
        }
    }
}
