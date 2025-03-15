using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BossWakeupTrigger : MonoBehaviour
{
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Public Fields                                                                                                                                                                                * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Debug Values ---------------------------------------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Private/Protected Attributes                                                                                                                                                                 * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Object References ----------------------------------------------------------------------------------------------------------
    GameObject playerGameObject;
    private GameObject enemy;
    BossEnemy bossEnemyScriptComponent;
    Animator m_animator;
    boss_fx_behaviors fxBehave;
    audioManager m_audio;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Start Function                                                                                                                                                                               * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        // Set Object References
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        bossEnemyScriptComponent = GameObject.FindGameObjectWithTag("Boss Enemy").GetComponent<BossEnemy>();
        m_animator = GameObject.FindGameObjectWithTag("Boss Enemy").GetComponent<Animator>();
        enemy = GameObject.FindGameObjectWithTag("Boss Enemy");
        fxBehave = enemy.GetComponent<boss_fx_behaviors>();
        m_audio = GameObject.Find("AudioManager").GetComponent<audioManager>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject ==  playerGameObject)
        {
            //Debug.Log("Player Entered Boss Wakeup Trigger");
            bossEnemyScriptComponent.Player_EnteredWakeupTrigger();
            m_animator.SetBool("woken", true);
            //turn on enemy eyes
            fxBehave.eyesOnCoroutine = StartCoroutine(fxBehave.turnOnEyes());
            m_audio.enemyWhirringSource.enabled = false; //turn off whirring when battle begins
        }
    }
}
