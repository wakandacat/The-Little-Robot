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

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject ==  playerGameObject)
        {
            //Debug.Log("Player Entered Boss Wakeup Trigger");
            bossEnemyScriptComponent.Player_EnteredWakeupTrigger();
            m_animator.SetBool("woken", true);
            //Debug.Log("start ON");
            //turn on light/trigger intensity change
            enemy.GetComponent<boss_fx_behaviors>().StartCoroutine(enemy.GetComponent<boss_fx_behaviors>().turnOnEyes());

        }
    }
}
