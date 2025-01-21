using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    //create instance of AudioManager for easier access
    public static audioManager instance;

    //audio clip array
    public AudioClip[] playerSFXClips;
    public AudioClip[] enemySFXClips;

    //audio sources
    public AudioSource menuSource;
    public AudioSource backgroundSource;
    public AudioSource playerSource;
    public AudioSource enemySource;

    private void Awake()
    {
        //if instance doesn't exist, fill with this one else destroy the existing version
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            //nothing
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
