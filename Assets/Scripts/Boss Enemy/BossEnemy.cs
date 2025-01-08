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

    // Attack_State History -------------------------------------------------------------------------------------------------------
    private List<string> Attack_HistoryList = new List<string>();
    private int Attack_HistoryLength = 3;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Start Function                                                                                                                                                                               * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        // Set Object References
        bossAnimator = GetComponent<Animator>(); // assign the Animator component of the BossEnemy to bossAnimator
        playerGameObject = GameObject.FindGameObjectWithTag("Player");

        // Initialize Attributes
        stateMachine = new BossStateMachine();

        // Initialize Boss Enemy Attributes and Set To SleepingState
        resetBossEnemy();
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
    // *               Player Functions                                                                                                                                                                             * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public float Player_ReturnDistance()
    {
        return Vector3.Distance(transform.position, playerGameObject.transform.position);
    }

    public void Player_EnteredWakeupTrigger()
    {
        playerTriggeredBossWakeup = true;
    }
    public bool Player_ReturnPlayerTriggeredBossWakeup()
    {
        return playerTriggeredBossWakeup;
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
    public void TransitionToAttack_TestingState()
    {
        Attack_TestingState attackTestingState = new Attack_TestingState();
        attackTestingState.Initialize(bossAnimator, this);
        stateMachine.SetState(attackTestingState);
    }
    public void TransitionToAttack_Laser01State()
    {
        Attack_Laser01State attackLaser01State = new Attack_Laser01State();
        attackLaser01State.Initialize(bossAnimator, this);
        stateMachine.SetState(attackLaser01State);
    }
    public void TransitionToAttack_Melee01State()
    {
        Attack_Melee01State attackMelee01State = new Attack_Melee01State();
        attackMelee01State.Initialize(bossAnimator, this);
        stateMachine.SetState(attackMelee01State);
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

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Attack History Functions                                                                                                                                                                     * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void appendToAttackHistory(string attackName)
    {
        // Add new attackName to Attack_HistoryList
        Attack_HistoryList.Insert(0, attackName);

        // Remove oldest attack if more than Attack_HistoryLength attacks are in the Attack_HistoryList
        if (Attack_HistoryList.Count > Attack_HistoryLength )
        {
            Attack_HistoryList.RemoveAt(Attack_HistoryList.Count - 1);
        }
    }

    // Checks if an attack was recently used, if so, the more recent the attack the lower the returned negative score, if not, a positive score is returned
    public float returnAttackHistoryScore(string attackName)
    {
        float totalScore = 0.0f;

        // Iterate through every entry in the Attack_HistoryList
        for (int i = 0; i < Attack_HistoryList.Count; i++)
        {
            // Add the score based on the index (-1.0f for the last, 0.0f for second last, etc.)
            if (Attack_HistoryList[i] == attackName)
            {
                // Score decreases by 1.0f the more recent the entry was
                totalScore += -1.0f * (Attack_HistoryList.Count - i);
            }
        }

        // If no match was found, return 1.0f
        if (totalScore == 0.0f)
        {
            return 1.0f;
        }

        // Return the accumulated total score
        return totalScore;
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Misc. Functions                                                                                                                                                                              * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void resetBossEnemy()
    {
        // Initialize Boss Enemy Attributes
        HP_Current = HP_Maximum;
        Energy_Current = Energy_Maximum;

        // Set To SleepingState
        TransitionToSleepingState();
    }
}
