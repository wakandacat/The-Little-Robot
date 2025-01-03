using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Public Fields                                                                                                                                                                                * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Debug Values ---------------------------------------------------------------------------------------------------------------
    [Tooltip("Maximum HP that the Boss Enemy can have.")]
    public float HP_Maximum = 30.0f;
    [Tooltip("Maximum Energy (cost of attacks) that the Boss Enemy can have.")]
    public float Energy_Maximum = 3.0f;
    [Tooltip("Amount of Energy the Boss Enemy regains over the course of a second while in 'LowEnergyState'.")]
    public float Energy_RegainedPerSecond = 0.5f;
    [Tooltip("Amount of Energy the Boss Enemy when struck while in 'LowEnergyState'.")]
    public float Energy_RegainedOnStrike = 1.0f;

    [Tooltip("Amount of time that must pass when entering the 'AwakeState' before the BossEnemy can execute the selected attack.")]
    public float AwakeState_Delay = 2.0f;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Private/Protected Attributes                                                                                                                                                                 * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Boss Enemy Attributes ------------------------------------------------------------------------------------------------------
    private float HP_Current;
    private float Energy_Current;

    // Object References ----------------------------------------------------------------------------------------------------------
    private Animator bossAnimator;
    GameObject playerGameObject;

    // State Machine Attributes ---------------------------------------------------------------------------------------------------
    private BossStateMachine stateMachine;

    // Player Actions/Location ----------------------------------------------------------------------------------------------------
    private bool playerTriggeredBossWakeup = false;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Start Function                                                                                                                                                                               * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        // Initialize Boss Enemy Attributes
        HP_Current = HP_Maximum;
        Energy_Current = Energy_Maximum;

        // Set Object References
        bossAnimator = GetComponent<Animator>(); // assign the Animator component of the BossEnemy to bossAnimator
        playerGameObject = GameObject.FindGameObjectWithTag("Player");

        // Initialize Attributes
        stateMachine = new BossStateMachine();

        // Set To SleepingState
        TransitionToSleepingState();
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Update Function                                                                                                                                                                              * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        // Execute Update instructions for StateMachine
        stateMachine.Update();
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Fixed Update Function                                                                                                                                                                        * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // FixedUpdate is called at set intervals
    void FixedUpdate()
    {
        // Execute FixedUpdate instructions for StateMachine
        stateMachine.FixedUpdate();
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Player Action/Location Functions                                                                                                                                                             * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public float returnDistanceOfPlayer()
    {
        return Vector3.Distance(transform.position, playerGameObject.transform.position);
    }

    public void playerEnteredWakeupTrigger()
    {
        playerTriggeredBossWakeup = true;
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               State Transition Functions                                                                                                                                                                   * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void TransitionToSleepingState()
    {
        SleepingState sleepingState = new SleepingState();
        sleepingState.Initialize(bossAnimator, this);
        stateMachine.SetState(sleepingState);
    }
    public void TransitionToWakingUpState()
    {
        WakingUpState wakingUpState = new WakingUpState();
        wakingUpState.Initialize(bossAnimator, this);
        stateMachine.SetState(wakingUpState);
    }
    public void TransitionToSelfCheckState()
    {
        SelfCheckState selfCheckState = new SelfCheckState();
        selfCheckState.Initialize(bossAnimator, this);
        stateMachine.SetState(selfCheckState);
    }
    public void TransitionToAwakeState()
    {
        AwakeState awakeState = new AwakeState();
        awakeState.Initialize(bossAnimator, this);
        stateMachine.SetState(awakeState);
    }
    public void TransitionToLowEnergyState()
    {
        LowEnergyState lowEnergyState = new LowEnergyState();
        lowEnergyState.Initialize(bossAnimator, this);
        stateMachine.SetState(lowEnergyState);
    }
    public void TransitionToDeathState()
    {
        DeathState deathState = new DeathState();
        deathState.Initialize(bossAnimator, this);
        stateMachine.SetState(deathState);
    }
    public void TransitionToAttackTestingState()
    {
        AttackTestingState attackTestingState = new AttackTestingState();
        attackTestingState.Initialize(bossAnimator, this);
        stateMachine.SetState(attackTestingState);
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               HP Functions                                                                                                                                                                                 * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public float returnCurrentHP()
    {
        return HP_Current;
    }

    public void updateCurrentHP(float newHPCurrent)
    {
        HP_Current = newHPCurrent;
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Energy Functions                                                                                                                                                                             * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public float returnCurrentEnergy()
    {
        return Energy_Current;
    }

    public void updateCurrentEnergy(float newEnergyCurrent)
    {
        Energy_Current = newEnergyCurrent;
    }

    public void regainCurrentEnergyPerFrame()
    {
        Energy_Current += Energy_RegainedPerSecond * Time.fixedDeltaTime;   // regain X% of Energy_RegainedPerSecond by multiplying the amount by the duration of time between frames (at 50fps, 1/50th)
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Get/Set Functions                                                                                                                                                                            * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public bool returnPlayerTriggeredBossWakeup()
    {
        return playerTriggeredBossWakeup;
    }
}
