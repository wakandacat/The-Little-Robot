using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// *               Abstract Class BossState                                                                                                                                                                     * 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
public abstract class BossState
{
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Public Fields                                                                                                                                                                                * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Debug Values ---------------------------------------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Private/Protected Attributes                                                                                                                                                                 * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Object References ----------------------------------------------------------------------------------------------------------
    protected BossEnemy bossEnemyComponent; // the BossEnemy.cs script attached to the Boss Enemy
    protected Animator animator; // will be set to whatever animator is being used for Boss Enemy

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Initialize Function                                                                                                                                                                          * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Initialize should be called whenever a state change occurs
    public void Initialize(Animator bossAnimator, BossEnemy bossEnemyScriptComponent)
    {
        animator = bossAnimator;
        bossEnemyComponent = bossEnemyScriptComponent;
    }

    // Enter is called when the state machine first transitions to this state
    public abstract void Enter();

    // Update is called once per frame
    public abstract void Update();

    // CheckTransition is called once per frame and is used to determine if the criteria for changing states has been fulfilled for a given state
    public abstract void CheckTransition();

    // FixedUpdate is called at set intervals
    public abstract void FixedUpdate();

    // Exit is called when the state machine transitions to another state
    public abstract void Exit();

}

// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// *               Sleeping State                                                                                                                                                                               * 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
public class SleepingState : BossState
{
    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        Debug.Log("BossEnemy: Entering SleepingState");

        // Animation Logic

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic
        //Debug.Log("update logic :3");

        // Animation Logic

    }

    // Called once per frame
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.Player_ReturnPlayerTriggeredBossWakeup() == true)   // check if the player has triggered the Boss Wakeup Trigger
        {
            bossEnemyComponent.TransitionToWakingUpState();                 // if so, transition to waking up state
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called when the state machine transitions out of this state
    public override void Exit()
    {
        // Programming Logic

        // Animation Logic

    }
}

// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// *               Waking Up State                                                                                                                                                                              * 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
public class WakingUpState : BossState
{
    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        Debug.Log("BossEnemy: Entering WakingUpState");

        // Animation Logic

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called once per frame
    public override void CheckTransition()
    {
        // Programming Logic
        bossEnemyComponent.TransitionToSelfCheckState();    // This *should* only be done when the animations finish for WakingUpState but that logic hasn't been implemented yet

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called when the state machine transitions out of this state
    public override void Exit()
    {
        // Programming Logic

        // Animation Logic

    }
}

// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// *               Self Check State                                                                                                                                                                             * 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
public class SelfCheckState : BossState
{
    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        Debug.Log("BossEnemy: Entering SelfCheckState");

        // Animation Logic

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called once per frame
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.returnCurrentEnergy() <= 0)   // check if the current energy count of the Boss Enemy is below or equal to 0
        {
            bossEnemyComponent.TransitionToLowEnergyState(); // if so, transition to low energy state
        }
        else
        {
            bossEnemyComponent.TransitionToAwakeState();     // if not, transition to awake state
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called when the state machine transitions out of this state
    public override void Exit()
    {
        // Programming Logic

        // Animation Logic

    }
}

// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// *               Awake State                                                                                                                                                                                  * 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
public class AwakeState : BossState
{
    // State Specific Properties
    private bool delayFinished = false;
    private float enterStateTimeStamp = 0.0f;

    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        Debug.Log("BossEnemy: Entering AwakeState");
        enterStateTimeStamp = Time.time;

        // INSERT: attack selection logic

        // Animation Logic

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called once per frame
    public override void CheckTransition()
    {
        // Programming Logic
        if (delayFinished == false) // check if the delay has been completed
        {
            if (Time.time - enterStateTimeStamp >= bossEnemyComponent.AwakeState_Delay) // if not, check if the duration of the delay has been exceeded
            {
                delayFinished = true; // if so, set delay to have been completed
            }
        }

        if (delayFinished == true)  // check if the delay has been completed and the attack has been chosen
        {
            Attack_Selection();
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called when the state machine transitions out of this state
    public override void Exit()
    {
        // Programming Logic

        // Animation Logic

    }

    // ---------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Attack Selection                                                                                                                        * 
    // ---------------------------------------------------------------------------------------------------------------------------------------------------------
    // Define a delegate that matches the signature of a function to later call
    public delegate void MyFunctionDelegate();

    // Determines which Attack_State to enter based on player information (attack_states must be manually added here)
    public void Attack_Selection()
    {
        string Attack_BestName = null;
        float Attack_BestScore = 0.0f;
        MyFunctionDelegate Attack_TransitionToExecute = null;

        // Determine Best Choice -----------------------------------------------------------------------------------*
        for (int i = 0; i < 5; i++)     // loop up to 5 times to find a suitable attack
        {
            // Attack_TestingState -----------------------
            if (Attack_TestingState.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = Attack_TestingState.Attack_Name;
                Attack_BestScore = Attack_TestingState.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToAttack_TestingState;
            }
            else if (Attack_TestingState.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = Attack_TestingState.Attack_Name;
                    Attack_BestScore = Attack_TestingState.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToAttack_TestingState;
                }
            }
            // Attack_Laser01State -----------------------
            if (Attack_Laser01State.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = Attack_Laser01State.Attack_Name;
                Attack_BestScore = Attack_Laser01State.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToAttack_Laser01State;
            }
            else if (Attack_Laser01State.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = Attack_Laser01State.Attack_Name;
                    Attack_BestScore = Attack_Laser01State.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToAttack_Laser01State;
                }
            }
            // Attack_Melee01State -----------------------
            if (Attack_Melee01State.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = Attack_Melee01State.Attack_Name;
                Attack_BestScore = Attack_Melee01State.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToAttack_Melee01State;
            }
            else if (Attack_Melee01State.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = Attack_Melee01State.Attack_Name;
                    Attack_BestScore = Attack_Melee01State.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToAttack_Melee01State;
                }
            }
        }
        // If No Attack Selected, Choose Default -------------------------------------------------------------------*
        if (Attack_BestName == null)
        {
            Attack_BestName = Attack_TestingState.Attack_Name;
            Attack_BestScore = Attack_TestingState.CalculateScore(bossEnemyComponent);
            Attack_TransitionToExecute = bossEnemyComponent.TransitionToAttack_TestingState;
        }
        // Transition to Best Choice -------------------------------------------------------------------------------*
        Attack_TransitionToExecute();
    }
}

// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// *               Low Energy State                                                                                                                                                                             * 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
public class LowEnergyState : BossState
{
    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        Debug.Log("BossEnemy: Entering LowEnergyState");

        // TEMP DEBUGGING LOGIC
        bossEnemyComponent.updateCurrentHP(bossEnemyComponent.returnCurrentHP() - 10.0f);
        Debug.Log("BossEnemy: Current HP = " + bossEnemyComponent.returnCurrentHP());

        // Animation Logic

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic
        bossEnemyComponent.regainCurrentEnergyPerFrame();   // regain X% of Energy_RegainedPerSecond by multiplying the amount by the duration of time between frames (at 50fps, 1/50th)

        // INSERT: check for strikes against enemy to regain energy from
        // INSERT: check for strikes against enemy to lose HP from

        // Animation Logic

    }

    // Called once per frame
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.returnCurrentHP() <= 0.0f)                                   // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToDeathState();                                    // if so, transition to Death State
        }

        if (bossEnemyComponent.returnCurrentEnergy() >= bossEnemyComponent.Energy_Maximum)  // check if Energy_Current has exceeded Energy_Maximum
        {
            bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.Energy_Maximum);      // if so, set Energy_Current to Energy_Maximum
            bossEnemyComponent.TransitionToAwakeState();                                    // and, transition to Awake State
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called when the state machine transitions out of this state
    public override void Exit()
    {
        // Programming Logic

        // Animation Logic

    }
}

// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// *               Death State                                                                                                                                                                                  * 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
public class DeathState : BossState
{
    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        Debug.Log("BossEnemy: Entering DeathState");

        // Animation Logic

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called once per frame
    public override void CheckTransition()
    {
        // Programming Logic
        //Debug.Log("debug text hehe :3");

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called when the state machine transitions out of this state
    public override void Exit()
    {
        // Programming Logic

        // Animation Logic

    }
}

// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// *               Attack Instruction States                                                                                                                                                                    * 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// This state is used for testing attack selection 
public class Attack_TestingState : BossState
{
    // State Specific Properties
    public static string Attack_Name = "Attack_TestingState";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 20.0f;
    public static float Player_MaxDistance = 30.0f;

    public static float CalculateScore(BossEnemy bossEnemyComponent)
    {
        float score = 0.0f;

        // Check distance ----------------------------*
        if (bossEnemyComponent.Player_ReturnDistance() >= Player_MinDistance && bossEnemyComponent.Player_ReturnDistance() <= Player_MaxDistance)
        {
            score += 1.0f;
        }
        else
        {
            score -= 1.0f;
        }

        // Check Attack_HistoryList ------------------*
        score += bossEnemyComponent.returnAttackHistoryScore(Attack_Name);

        return score;
    }

    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        Debug.Log("BossEnemy: Entering Attack_TestingState");
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost);
        Debug.Log("BossEnemy: Current Energy = " + bossEnemyComponent.returnCurrentEnergy());

        // Animation Logic

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called once per frame
    public override void CheckTransition()
    {
        // Programming Logic
        bossEnemyComponent.TransitionToSelfCheckState();

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called when the state machine transitions out of this state
    public override void Exit()
    {
        // Programming Logic
        bossEnemyComponent.appendToAttackHistory(Attack_Name);

        // Animation Logic

    }
}

public class Attack_Laser01State : BossState
{
    // State Specific Properties
    public static string Attack_Name = "Attack_Laser01State";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 20.0f;

    public static float CalculateScore(BossEnemy bossEnemyComponent)
    {
        float score = 0.0f;

        // Check distance ----------------------------*
        if (bossEnemyComponent.Player_ReturnDistance() >= Player_MinDistance && bossEnemyComponent.Player_ReturnDistance() <= Player_MaxDistance)
        {
            score += 1.0f;
        }
        else
        {
            score -= 1.0f;
        }

        // Check Attack_HistoryList ------------------*
        score += bossEnemyComponent.returnAttackHistoryScore(Attack_Name);

        return score;
    }

    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        Debug.Log("BossEnemy: Entering public class Attack_Laser01State : BossState");
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost);
        Debug.Log("BossEnemy: Current Energy = " + bossEnemyComponent.returnCurrentEnergy());

        // Animation Logic

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called once per frame
    public override void CheckTransition()
    {
        // Programming Logic
        bossEnemyComponent.TransitionToSelfCheckState();

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called when the state machine transitions out of this state
    public override void Exit()
    {
        // Programming Logic
        bossEnemyComponent.appendToAttackHistory(Attack_Name);

        // Animation Logic

    }
}

public class Attack_Melee01State : BossState
{
    // State Specific Properties
    public static string Attack_Name = "Attack_Melee01State";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 0.0f;
    public static float Player_MaxDistance = 10.0f;

    public static float CalculateScore(BossEnemy bossEnemyComponent)
    {
        float score = 0.0f;

        // Check distance ----------------------------*
        if (bossEnemyComponent.Player_ReturnDistance() >= Player_MinDistance && bossEnemyComponent.Player_ReturnDistance() <= Player_MaxDistance)
        {
            score += 1.0f;
        }
        else
        {
            score -= 1.0f;
        }

        // Check Attack_HistoryList ------------------*
        score += bossEnemyComponent.returnAttackHistoryScore(Attack_Name);

        return score;
    }

    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        Debug.Log("BossEnemy: Entering public class Attack_Melee01State : BossState");
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost);
        Debug.Log("BossEnemy: Current Energy = " + bossEnemyComponent.returnCurrentEnergy());

        // Animation Logic

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called once per frame
    public override void CheckTransition()
    {
        // Programming Logic
        bossEnemyComponent.TransitionToSelfCheckState();

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called when the state machine transitions out of this state
    public override void Exit()
    {
        // Programming Logic
        bossEnemyComponent.appendToAttackHistory(Attack_Name);

        // Animation Logic

    }
}