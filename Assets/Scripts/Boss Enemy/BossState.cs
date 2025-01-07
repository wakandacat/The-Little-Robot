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
        if (bossEnemyComponent.returnPlayerTriggeredBossWakeup() == true)   // check if the player has triggered the Boss Wakeup Trigger
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
    private bool attackChosen = false;
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
        attackChosen = true;

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

        if (attackChosen == true && delayFinished == true)  // check if the delay has been completed and the attack has been chosen
        {
            // INSERT: attack selection procedure for state transitions
            bossEnemyComponent.TransitionToAttackTestingState();
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
public class AttackTestingState : BossState
{
    // When to call this Attack Instruction State
    // -- insert here --

    // State Specific Properties
    private float Energy_Cost = 1.0f;

    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        Debug.Log("BossEnemy: Entering AttackTestingState");
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

        // Animation Logic

    }
}

public class MeleeSwingState : BossState
{
    // When to call this Attack Instruction State
    // -- insert here --
    // only do when player is close

    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        Debug.Log("BossEnemy: Entering MeleeSwingState");

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