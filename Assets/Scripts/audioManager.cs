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
    public AudioSource walkSource;       //used exclusively for player walking
    public AudioSource enemySource;      //used for enemy sfx
    public AudioSource enemyWhirringSource;      //used for enemy pre-battle sfx
    public AudioSource ventSource;         //used for rolling inside of vents

    //get enemy
    BossEnemy enemy;

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
            //on detection of combat scene and the enemy exists but hasn't been woken yet, make whirring sound
            //if(GameObject.FindGameObjectWithTag("Boss Enemy"))
            //{
            //    enemy = GameObject.FindGameObjectWithTag("Boss Enemy").GetComponent<BossEnemy>();
            //    Debug.Log(enemy.Player_ReturnPlayerTriggeredBossWakeup());
            //    if (enemy.Player_ReturnPlayerTriggeredBossWakeup() == false)
            //    {
            //        enemyWhirringSource.enabled = true;
            //    }
            //}
            
        }
        else
        {
            index = 0;
            //on detection of enemy existing but hasn't been woken yet, make whirring sound
            if (GameObject.FindGameObjectWithTag("Boss Enemy"))
            {
                enemy = GameObject.FindGameObjectWithTag("Boss Enemy").GetComponent<BossEnemy>();
                //Debug.Log(enemy.Player_ReturnPlayerTriggeredBossWakeup());
                if (enemy.Player_ReturnPlayerTriggeredBossWakeup() == false)
                {
                    enemyWhirringSource.enabled = true;
                }
            }
        }

        //play clip
        backgroundSource.clip = backgroundClips[index];          //load sfx clip based on array index
        backgroundSource.Play();
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
        //if in combat
        if (SceneManager.GetActiveScene().name.Contains("Combat"))
        {
            //Debug.Log("in combat scene");
            //find + assign enemy
            if (GameObject.FindGameObjectWithTag("Boss Enemy"))
            {
                enemy = GameObject.FindGameObjectWithTag("Boss Enemy").GetComponent<BossEnemy>();

                //if enemy is awake + alive
                if (enemy.Player_ReturnPlayerTriggeredBossWakeup() == true && enemy.HP_ReturnCurrent() > 0)
                {
                    //Debug.Log("boss was woken");
                    //special case for downed + they're alive
                    if (enemy.GetComponent<BossEnemy>().returnCurrentEnergy() <= 0 && enemy.HP_ReturnCurrent() > 0)
                    {
                        //Debug.Log("in downed");
                        //set sfx
                        enemySource.clip = enemySFXClips[i];
                        enemySource.PlayDelayed(0.4f);
                    }
                    else
                    {
                        //Debug.Log("everything else");
                        //set sfx
                        enemySource.clip = enemySFXClips[i];          //load sfx clip based on array index
                        enemySource.PlayOneShot(enemySFXClips[i]);    //play clip
                    }
                }
                else
                {
                    //set sfx
                    enemySource.clip = enemySFXClips[i];          //load sfx clip based on array index
                    enemySource.PlayOneShot(enemySFXClips[i]);    //play clip
                }
                //else if (enemy.Player_ReturnPlayerTriggeredBossWakeup() == true && enemy.HP_ReturnCurrent() <= 0) //was woken but is now dead
                //{
                //    enemySource.clip = enemySFXClips[3];          //load sfx clip based on array index
                //    enemySource.PlayOneShot(enemySFXClips[3]);    //play clip
                //}
                //else //combat hasn't started
                //{
                //    //Debug.Log("play whirring");
                //    //set sfx
                //    enemyWhirringSource.clip = enemySFXClips[i];          //load sfx clip based on array index
                //    enemySource.Play();
                //}
            }         
           
        }

    }
}
