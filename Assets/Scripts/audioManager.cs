using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class audioManager : MonoBehaviour
{
    //create instance of AudioManager for easier access
    public static audioManager instance;
    //UnityEngine.SceneManagement.Scene currentScene;

    //audio clip array
    public AudioClip[] backgroundClips;
    public AudioClip[] playerSFXClips; //spin1, spin2, spin3, hit enemey, took damage from enemy, jump,dbl jump, dash, error
    public AudioClip[] enemySFXClips; //downed, dead

    //audio sources
    public AudioSource backgroundSource; //used for environment music
    public AudioSource playerSource;     //used for player sfx
    public AudioSource enemySource;      //used for enemy sfx

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

    //loads audio source for background music based on the index and plays
    public void playBackgroundMusic(string sceneName)
    {
        int index = 0;
        backgroundSource.Stop();
        if (sceneName.Contains("Combat"))
        {

            index = 1;
        }
        else
        {
            index = 0;
        }

        //play clip
        backgroundSource.clip = backgroundClips[index];          //load sfx clip based on array index
        backgroundSource.PlayOneShot(backgroundClips[index]);
    }

    //loads audio source for player based on the index and plays
    public void playPlayerSFX(int i)
    {
        //set sfx
        playerSource.clip = playerSFXClips[i];          //load sfx clip based on array index
        playerSource.PlayOneShot(playerSFXClips[i]);    //play clip
    }

    //loads audio source for enemy based on the index and plays
    public void playEnemySFX(int i)
    {
        //set sfx
        enemySource.clip = enemySFXClips[i];          //load sfx clip based on array index
        enemySource.PlayOneShot(enemySFXClips[i]);    //play clip
    }
}
