using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossState : MonoBehaviour
{
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Public Fields                                                                                                                                                                                * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Debug Values ---------------------------------------------------------------------------------------------------------------

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Private/Protected Attributes                                                                                                                                                                 * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Object References ----------------------------------------------------------------------------------------------------------
    protected Animator animator; // will be set to whatever animator is being used for Boss Enemy

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Initialize Function                                                                                                                                                                          * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Initialize should be called whenever a state change occurs
    public void Initialize(Animator bossAnimator)
    {
        animator = bossAnimator;
    }

    // Enter is called when the state machine first transitions to this state
    public abstract void Enter();

    // Update is called once per frame
    public abstract void Update();

    // FixedUpdate is called at set intervals
    public abstract void FixedUpdate();

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Exit Function                                                                                                                                                                                * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Exit is called when the state machine transitions to another state
    public abstract void Exit();

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Boss States                                                                                                                                                                                  * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public class SleepingState:BossState
    {
        // Called when the state machine transitions to this state
        public override void Enter()
        {
            // Programming Logic

            // Animation Logic

        }

        // Called once per frame
        public override void Update()
        {
            // Programming Logic

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

    public class WakingUpState : BossState
    {
        // Called when the state machine transitions to this state
        public override void Enter()
        {
            // Programming Logic

            // Animation Logic

        }

        // Called once per frame
        public override void Update()
        {
            // Programming Logic

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

    public class SelfCheckState : BossState
    {
        // Called when the state machine transitions to this state
        public override void Enter()
        {
            // Programming Logic

            // Animation Logic

        }

        // Called once per frame
        public override void Update()
        {
            // Programming Logic

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

    public class AwakeState : BossState
    {
        // Called when the state machine transitions to this state
        public override void Enter()
        {
            // Programming Logic

            // Animation Logic

        }

        // Called once per frame
        public override void Update()
        {
            // Programming Logic

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

    public class LowEnergyState : BossState
    {
        // Called when the state machine transitions to this state
        public override void Enter()
        {
            // Programming Logic

            // Animation Logic

        }

        // Called once per frame
        public override void Update()
        {
            // Programming Logic

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

    public class DeathState : BossState
    {
        // Called when the state machine transitions to this state
        public override void Enter()
        {
            // Programming Logic

            // Animation Logic

        }

        // Called once per frame
        public override void Update()
        {
            // Programming Logic

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
    public class MeleeSwingState : BossState
    {
        // When to call this Attack Instruction State
        // -- insert here --
        
        // Called when the state machine transitions to this state
        public override void Enter()
        {
            // Programming Logic

            // Animation Logic

        }

        // Called once per frame
        public override void Update()
        {
            // Programming Logic

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
}