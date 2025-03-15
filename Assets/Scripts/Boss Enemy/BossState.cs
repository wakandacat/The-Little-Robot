using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

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
    protected BossEnemy bossEnemyComponent;                 // the BossEnemy.cs script attached to the Boss Enemy
    protected Animator animator;                            // will be set to whatever animator is being used for Boss Enemy
    protected boss_fx_behaviors fxBehave = GameObject.FindGameObjectWithTag("Boss Enemy").GetComponent<boss_fx_behaviors>();
    protected audioManager m_audio = GameObject.Find("AudioManager").GetComponent<audioManager>();

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

    // LateUpdate is called after all other update functions
    public abstract void LateUpdate();

    // Exit is called when the state machine transitions to another state
    public abstract void Exit();

}

// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// *               Sleeping State                                                                                                                                                                               * 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
public class State_Sleeping : BossState
{
    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        //Debug.Log("BossEnemy: Entering State_Sleeping");

        // Animation Logic

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic
        ////Debug.Log("update logic :3");

        // Animation Logic

    }

    // Called once per frame
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.Player_ReturnPlayerTriggeredBossWakeup() == true)   // check if the player has triggered the Boss Wakeup Trigger
        {
            bossEnemyComponent.TransitionToState_WakingUp();                   // if so, transition to waking up state
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
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
public class State_WakingUp : BossState
{
    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        //Debug.Log("BossEnemy: Entering State_WakingUp");

        // FX Logic
        //this results in coroutine error cannot continue
        //if (fxBehave.eyesOnCoroutine != null)
        //{
        //    fxBehave.StopCoroutine(fxBehave.eyesOnCoroutine);
        //    fxBehave.eyesOnCoroutine = null; // Clear reference after stopping
        //}
        m_audio.playEnemySFX(0);

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
        bossEnemyComponent.TransitionToState_SelfCheck();    // This *should* only be done when the animations finish for State_WakingUp but that logic hasn't been implemented yet

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
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
public class State_SelfCheck : BossState
{
    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        //Debug.Log("BossEnemy: Entering State_SelfCheck");

        // Animation Logic
        animator.SetBool("toIdle", true);

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

        if (bossEnemyComponent.HP_IsZero())                                  // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToState_Death();                     // if so, transition to Death State
        }
        else if (bossEnemyComponent.returnCurrentEnergy() <= 0)   // check if the current energy count of the Boss Enemy is below or equal to 0
        {
            bossEnemyComponent.TransitionToState_LowEnergy(); // if so, transition to low energy state
        }
        else
        {
            bossEnemyComponent.TransitionToState_Awake();     // if not, transition to awake state
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
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
public class State_LowEnergy : BossState
{
    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        //Debug.Log("BossEnemy: Entering State_LowEnergy");
        //Debug.Log("BossEnemy: Current HP = " + bossEnemyComponent.HP_ReturnCurrent());

        //bossEnemyComponent.HP_TurnInvulnerabilityOff();

        // FX Logic
        animator.SetBool("downed", true);
        animator.SetBool("inAttack", false);
        m_audio.playEnemySFX(1);
        for (int i = 0; i < fxBehave.eyes.Length; i++)
        {
            fxBehave.eyes[i].intensity = 0.01f;
        }

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic
        bossEnemyComponent.regainCurrentEnergyPerFrame();   // regain X% of Energy_RegainedPerSecond by multiplying the amount by the duration of time between frames (at 50fps, 1/50th)

        // Animation Logic

    }

    // Called once per frame
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.HP_IsZero())                                  // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToState_Death();                     // if so, transition to Death State
        }

        else if (bossEnemyComponent.Energy_IsFull())                                        // check if Energy_Current has exceeded Energy_Maximum
        {
            bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.Energy_ReturnMaximum());      // if so, set Energy_Current to Energy_Maximum
            //bossEnemyComponent.TransitionToState_Awake();                                    // and, transition to Awake State
            bossEnemyComponent.TransitionToState_Attack_StandUpMelee();
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called when the state machine transitions out of this state
    public override void Exit()
    {
        // Programming Logic
        //bossEnemyComponent.HP_TurnInvulnerabilityOn();
        //Debug.Log("BossEnemy: Current HP = " + bossEnemyComponent.HP_ReturnCurrent());

        // FX Logic
        animator.SetBool("downed", false);
        fxBehave.eyesOnCoroutine = fxBehave.StartCoroutine(fxBehave.turnOnEyes());
    }
}

// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// *               Death State                                                                                                                                                                                  * 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
public class State_Death : BossState
{
    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        //Debug.Log("BossEnemy: Entering State_Death");
        bossEnemyComponent.GetComponent<CapsuleCollider>().enabled = false;

        // Disbable Projectiles
        ProjectileSpawner SpawnerComponent_Bullet = bossEnemyComponent.ReturnComponent_Spawner_Bullet();
        SpawnerComponent_Bullet.ReturnAllProjectilesToPool();
        ProjectileSpawner SpawnerComponent_Mine = bossEnemyComponent.ReturnComponent_Spawner_Mine();
        SpawnerComponent_Mine.ReturnAllProjectilesToPool();

        // FX Logic
        animator.SetBool("die", true);
        animator.SetBool("woken", false);
        m_audio.playEnemySFX(2);

        //turn off eyes on death
        fxBehave.eyesOffCoroutine = fxBehave.StartCoroutine(fxBehave.turnOffEyes());
        //fxBehave.StartCoroutine(fxBehave.turnOffEyes());

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
        ////Debug.Log("debug text hehe :3");

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
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
public class State_Awake : BossState
{
    // Attack_State Selection Properties
    private bool delayFinished = false;
    private float enterStateTimeStamp = 0.0f;

    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        //Debug.Log("BossEnemy: Entering State_Awake");
        ////Debug.Log("BossEnemy: Current Energy = " + bossEnemyComponent.returnCurrentEnergy());
        enterStateTimeStamp = Time.time;

        // INSERT: attack selection logic

        // Animation Logic
        animator.SetBool("toIdle", true);


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
        if (bossEnemyComponent.HP_IsZero())                                  // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToState_Death();                     // if so, transition to Death State
        }
        else if (delayFinished == false) // check if the delay has been completed
        {
            if (Time.time - enterStateTimeStamp >= bossEnemyComponent.returnStateAwakeDelay()) // if not, check if the duration of the delay has been exceeded
            {
                delayFinished = true; // if so, set delay to have been completed
            }
        }
        else if (delayFinished == true && bossEnemyComponent.AreAllPoolsFinishedFilling() == true)  // check if the delay has been completed and the projectile pools are filled
        {
            if (bossEnemyComponent.returnBossEnemyEncounterIteration() == 1)
            {
                Attack_Selection_1();
            }
            else if (bossEnemyComponent.returnBossEnemyEncounterIteration() == 2)
            {
                Attack_Selection_2();
            }
            else if (bossEnemyComponent.returnBossEnemyEncounterIteration() == 3)
            {
                Attack_Selection_3();
            }
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
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
    // *               Attack Selection Functions                                                                                                              * 
    // ---------------------------------------------------------------------------------------------------------------------------------------------------------
    // Define a delegate that matches the signature of a function to later call
    public delegate void MyFunctionDelegate();
    string Attack_BestName = null;
    float Attack_BestScore = 0.0f;
    MyFunctionDelegate Attack_TransitionToExecute = null;

    // Determines which Attack_State to enter based on player information (attack_states must be manually added here)
    public void Attack_Selection_1()
    {
        // Determine Best Choice -----------------------------------------------------------------------------------*
        for (int i = 0; i < 5; i++)     // loop up to 5 times to find a suitable attack
        {
            // State_Attack_Bullet_SlowFiringShot_Easy -----------------------
            if (State_Attack_Bullet_SlowFiringShot_Easy.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = State_Attack_Bullet_SlowFiringShot_Easy.Attack_Name;
                Attack_BestScore = State_Attack_Bullet_SlowFiringShot_Easy.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_SlowFiringShot_Easy;
            }
            else if (State_Attack_Bullet_SlowFiringShot_Easy.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = State_Attack_Bullet_SlowFiringShot_Easy.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_SlowFiringShot_Easy.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_SlowFiringShot_Easy;
                }
            }

            // State_Attack_Bullet_RapidFireShot_Easy -----------------------
            if (State_Attack_Bullet_RapidFireShot_Easy.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = State_Attack_Bullet_RapidFireShot_Easy.Attack_Name;
                Attack_BestScore = State_Attack_Bullet_RapidFireShot_Easy.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RapidFireShot_Easy;
            }
            else if (State_Attack_Bullet_RapidFireShot_Easy.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = State_Attack_Bullet_RapidFireShot_Easy.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_RapidFireShot_Easy.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RapidFireShot_Easy;
                }
            }

            // State_Attack_Bullet_TrackingCone_Easy -----------------------
            if (State_Attack_Bullet_TrackingCone_Easy.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = State_Attack_Bullet_TrackingCone_Easy.Attack_Name;
                Attack_BestScore = State_Attack_Bullet_TrackingCone_Easy.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingCone_Easy;
            }
            else if (State_Attack_Bullet_TrackingCone_Easy.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = State_Attack_Bullet_TrackingCone_Easy.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_TrackingCone_Easy.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingCone_Easy;
                }
            }

            // State_Attack_Bullet_TrackingWall_Easy -----------------------
            if (State_Attack_Bullet_TrackingWall_Easy.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = State_Attack_Bullet_TrackingWall_Easy.Attack_Name;
                Attack_BestScore = State_Attack_Bullet_TrackingWall_Easy.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingWall_Easy;
            }
            else if (State_Attack_Bullet_TrackingWall_Easy.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = State_Attack_Bullet_TrackingWall_Easy.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_TrackingWall_Easy.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingWall_Easy;
                }
            }

            // State_Attack_Bullet_RotatingWall_Easy -----------------------
            if (State_Attack_Bullet_RotatingWall_Easy.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = State_Attack_Bullet_RotatingWall_Easy.Attack_Name;
                Attack_BestScore = State_Attack_Bullet_RotatingWall_Easy.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RotatingWall_Easy;
            }
            else if (State_Attack_Bullet_RotatingWall_Easy.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = State_Attack_Bullet_RotatingWall_Easy.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_RotatingWall_Easy.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RotatingWall_Easy;
                }
            }

            // State_Attack_Bullet_JumpRope_Easy -----------------------
            if (State_Attack_Bullet_JumpRope_Easy.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = State_Attack_Bullet_JumpRope_Easy.Attack_Name;
                Attack_BestScore = State_Attack_Bullet_JumpRope_Easy.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_JumpRope_Easy;
            }
            else if (State_Attack_Bullet_JumpRope_Easy.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = State_Attack_Bullet_JumpRope_Easy.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_JumpRope_Easy.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_JumpRope_Easy;
                }
            }

            // State_Attack_Melee01 -----------------------------------
            if (State_Attack_Melee01.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = State_Attack_Melee01.Attack_Name;
                Attack_BestScore = State_Attack_Melee01.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Melee01;
            }
            else if (State_Attack_Melee01.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = State_Attack_Melee01.Attack_Name;
                    Attack_BestScore = State_Attack_Melee01.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Melee01;
                }
            }
        }

        // DEBUGGING (MUST BE REMOVED):
        //Attack_BestName = State_Attack_ArenaHazard_Mine_Random.Attack_Name;
        //Attack_BestScore = State_Attack_ArenaHazard_Mine_Random.CalculateScore(bossEnemyComponent);
        //Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_ArenaHazard_Mine_Random;

        // Transition to Best Choice -------------------------------------------------------------------------------*
        Attack_TransitionToExecute();
    }

    public void Attack_Selection_2()
    {
        // Determine Best Choice -----------------------------------------------------------------------------------*
        for (int i = 0; i < 5; i++)     // loop up to 5 times to find a suitable attack
        {
            // 1 / 3 chance for hard attack
            if (Random.Range(0, 3) == 0) {
                // State_Attack_Bullet_SlowFiringShot_Hard -----------------------
                if (State_Attack_Bullet_SlowFiringShot_Hard.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_SlowFiringShot_Hard.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_SlowFiringShot_Hard.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_SlowFiringShot_Hard;
                }
                else if (State_Attack_Bullet_SlowFiringShot_Hard.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_SlowFiringShot_Hard.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_SlowFiringShot_Hard.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_SlowFiringShot_Hard;
                    }
                }

                // State_Attack_Bullet_RapidFireShot_Hard -----------------------
                if (State_Attack_Bullet_RapidFireShot_Hard.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_RapidFireShot_Hard.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_RapidFireShot_Hard.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RapidFireShot_Hard;
                }
                else if (State_Attack_Bullet_RapidFireShot_Hard.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_RapidFireShot_Hard.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_RapidFireShot_Hard.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RapidFireShot_Hard;
                    }
                }

                // State_Attack_Bullet_TrackingCone_Hard -----------------------
                if (State_Attack_Bullet_TrackingCone_Hard.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_TrackingCone_Hard.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_TrackingCone_Hard.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingCone_Hard;
                }
                else if (State_Attack_Bullet_TrackingCone_Hard.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_TrackingCone_Hard.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_TrackingCone_Hard.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingCone_Hard;
                    }
                }

                // State_Attack_Bullet_TrackingWall_Hard -----------------------
                if (State_Attack_Bullet_TrackingWall_Hard.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_TrackingWall_Hard.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_TrackingWall_Hard.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingWall_Hard;
                }
                else if (State_Attack_Bullet_TrackingWall_Hard.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_TrackingWall_Hard.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_TrackingWall_Hard.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingWall_Hard;
                    }
                }

                // State_Attack_Bullet_RotatingWall_Hard -----------------------
                if (State_Attack_Bullet_RotatingWall_Hard.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_RotatingWall_Hard.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_RotatingWall_Hard.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RotatingWall_Hard;
                }
                else if (State_Attack_Bullet_RotatingWall_Hard.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_RotatingWall_Hard.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_RotatingWall_Hard.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RotatingWall_Hard;
                    }
                }

                // State_Attack_Bullet_JumpRope_Hard -----------------------
                if (State_Attack_Bullet_JumpRope_Hard.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_JumpRope_Hard.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_JumpRope_Hard.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_JumpRope_Hard;
                }
                else if (State_Attack_Bullet_JumpRope_Hard.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_JumpRope_Hard.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_JumpRope_Hard.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_JumpRope_Hard;
                    }
                }
            }

            // otherwise use easy attack
            else
            {
                // State_Attack_Bullet_RapidFireShot_Easy -----------------------
                if (State_Attack_Bullet_RapidFireShot_Easy.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_RapidFireShot_Easy.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_RapidFireShot_Easy.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RapidFireShot_Easy;
                }
                else if (State_Attack_Bullet_RapidFireShot_Easy.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_RapidFireShot_Easy.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_RapidFireShot_Easy.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RapidFireShot_Easy;
                    }
                }

                // State_Attack_Bullet_TrackingCone_Easy -----------------------
                if (State_Attack_Bullet_TrackingCone_Easy.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_TrackingCone_Easy.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_TrackingCone_Easy.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingCone_Easy;
                }
                else if (State_Attack_Bullet_TrackingCone_Easy.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_TrackingCone_Easy.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_TrackingCone_Easy.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingCone_Easy;
                    }
                }

                // State_Attack_Bullet_TrackingWall_Easy -----------------------
                if (State_Attack_Bullet_TrackingWall_Easy.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_TrackingWall_Easy.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_TrackingWall_Easy.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingWall_Easy;
                }
                else if (State_Attack_Bullet_TrackingWall_Easy.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_TrackingWall_Easy.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_TrackingWall_Easy.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingWall_Easy;
                    }
                }

                // State_Attack_Bullet_RotatingWall_Easy -----------------------
                if (State_Attack_Bullet_RotatingWall_Easy.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_RotatingWall_Easy.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_RotatingWall_Easy.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RotatingWall_Easy;
                }
                else if (State_Attack_Bullet_RotatingWall_Easy.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_RotatingWall_Easy.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_RotatingWall_Easy.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RotatingWall_Easy;
                    }
                }

                // State_Attack_Bullet_JumpRope_Easy -----------------------
                if (State_Attack_Bullet_JumpRope_Easy.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_JumpRope_Easy.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_JumpRope_Easy.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_JumpRope_Easy;
                }
                else if (State_Attack_Bullet_JumpRope_Easy.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_JumpRope_Easy.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_JumpRope_Easy.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_JumpRope_Easy;
                    }
                }
            }

            // always check mine or melee
            // TransitionToState_Attack_ArenaHazard_Mine_Random -------
            if (State_Attack_ArenaHazard_Mine_Random.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = State_Attack_ArenaHazard_Mine_Random.Attack_Name;
                Attack_BestScore = State_Attack_ArenaHazard_Mine_Random.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_ArenaHazard_Mine_Random;
            }
            else if (State_Attack_ArenaHazard_Mine_Random.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = State_Attack_ArenaHazard_Mine_Random.Attack_Name;
                    Attack_BestScore = State_Attack_ArenaHazard_Mine_Random.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_ArenaHazard_Mine_Random;
                }
            }

            // State_Attack_Melee01 -----------------------------------
            if (State_Attack_Melee01.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = State_Attack_Melee01.Attack_Name;
                Attack_BestScore = State_Attack_Melee01.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Melee01;
            }
            else if (State_Attack_Melee01.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = State_Attack_Melee01.Attack_Name;
                    Attack_BestScore = State_Attack_Melee01.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Melee01;
                }
            }
        }

        // DEBUGGING (MUST BE REMOVED):
        //Attack_BestName = State_Attack_Bullet_SlowFiringShot_Easy.Attack_Name;
        //Attack_BestScore = State_Attack_Bullet_SlowFiringShot_Easy.CalculateScore(bossEnemyComponent);
        //Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_SlowFiringShot_Easy;

        // Transition to Best Choice -------------------------------------------------------------------------------*
        Attack_TransitionToExecute();
    }

    public void Attack_Selection_3()
    {
        // Determine Best Choice -----------------------------------------------------------------------------------*
        for (int i = 0; i < 5; i++)     // loop up to 5 times to find a suitable attack
        {
            // 70% chance for hard attack
            if (Random.Range(0, 10) <= 6)
            {
                // State_Attack_Bullet_SlowFiringShot_Hard -----------------------
                if (State_Attack_Bullet_SlowFiringShot_Hard.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_SlowFiringShot_Hard.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_SlowFiringShot_Hard.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_SlowFiringShot_Hard;
                }
                else if (State_Attack_Bullet_SlowFiringShot_Hard.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_SlowFiringShot_Hard.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_SlowFiringShot_Hard.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_SlowFiringShot_Hard;
                    }
                }

                // State_Attack_Bullet_RapidFireShot_Hard -----------------------
                if (State_Attack_Bullet_RapidFireShot_Hard.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_RapidFireShot_Hard.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_RapidFireShot_Hard.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RapidFireShot_Hard;
                }
                else if (State_Attack_Bullet_RapidFireShot_Hard.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_RapidFireShot_Hard.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_RapidFireShot_Hard.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RapidFireShot_Hard;
                    }
                }

                // State_Attack_Bullet_TrackingCone_Hard -----------------------
                if (State_Attack_Bullet_TrackingCone_Hard.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_TrackingCone_Hard.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_TrackingCone_Hard.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingCone_Hard;
                }
                else if (State_Attack_Bullet_TrackingCone_Hard.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_TrackingCone_Hard.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_TrackingCone_Hard.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingCone_Hard;
                    }
                }

                // State_Attack_Bullet_TrackingWall_Hard -----------------------
                if (State_Attack_Bullet_TrackingWall_Hard.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_TrackingWall_Hard.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_TrackingWall_Hard.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingWall_Hard;
                }
                else if (State_Attack_Bullet_TrackingWall_Hard.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_TrackingWall_Hard.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_TrackingWall_Hard.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingWall_Hard;
                    }
                }

                // State_Attack_Bullet_RotatingWall_Hard -----------------------
                if (State_Attack_Bullet_RotatingWall_Hard.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_RotatingWall_Hard.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_RotatingWall_Hard.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RotatingWall_Hard;
                }
                else if (State_Attack_Bullet_RotatingWall_Hard.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_RotatingWall_Hard.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_RotatingWall_Hard.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RotatingWall_Hard;
                    }
                }

                // State_Attack_Bullet_JumpRope_Hard -----------------------
                if (State_Attack_Bullet_JumpRope_Hard.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_JumpRope_Hard.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_JumpRope_Hard.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_JumpRope_Hard;
                }
                else if (State_Attack_Bullet_JumpRope_Hard.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_JumpRope_Hard.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_JumpRope_Hard.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_JumpRope_Hard;
                    }
                }
            }

            // otherwise use easy attack
            else
            {
                // State_Attack_Bullet_RapidFireShot_Easy -----------------------
                if (State_Attack_Bullet_RapidFireShot_Easy.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_RapidFireShot_Easy.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_RapidFireShot_Easy.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RapidFireShot_Easy;
                }
                else if (State_Attack_Bullet_RapidFireShot_Easy.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_RapidFireShot_Easy.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_RapidFireShot_Easy.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RapidFireShot_Easy;
                    }
                }

                // State_Attack_Bullet_TrackingCone_Easy -----------------------
                if (State_Attack_Bullet_TrackingCone_Easy.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_TrackingCone_Easy.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_TrackingCone_Easy.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingCone_Easy;
                }
                else if (State_Attack_Bullet_TrackingCone_Easy.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_TrackingCone_Easy.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_TrackingCone_Easy.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingCone_Easy;
                    }
                }

                // State_Attack_Bullet_TrackingWall_Easy -----------------------
                if (State_Attack_Bullet_TrackingWall_Easy.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_TrackingWall_Easy.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_TrackingWall_Easy.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingWall_Easy;
                }
                else if (State_Attack_Bullet_TrackingWall_Easy.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_TrackingWall_Easy.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_TrackingWall_Easy.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingWall_Easy;
                    }
                }

                // State_Attack_Bullet_RotatingWall_Easy -----------------------
                if (State_Attack_Bullet_RotatingWall_Easy.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_RotatingWall_Easy.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_RotatingWall_Easy.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RotatingWall_Easy;
                }
                else if (State_Attack_Bullet_RotatingWall_Easy.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_RotatingWall_Easy.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_RotatingWall_Easy.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RotatingWall_Easy;
                    }
                }

                // State_Attack_Bullet_JumpRope_Easy -----------------------
                if (State_Attack_Bullet_JumpRope_Easy.CalculateScore(bossEnemyComponent) > Attack_BestScore)
                {
                    Attack_BestName = State_Attack_Bullet_JumpRope_Easy.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_JumpRope_Easy.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_JumpRope_Easy;
                }
                else if (State_Attack_Bullet_JumpRope_Easy.CalculateScore(bossEnemyComponent) == Attack_BestScore)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        Attack_BestName = State_Attack_Bullet_JumpRope_Easy.Attack_Name;
                        Attack_BestScore = State_Attack_Bullet_JumpRope_Easy.CalculateScore(bossEnemyComponent);
                        Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_JumpRope_Easy;
                    }
                }
            }

            // always check mine or melee
            // TransitionToState_Attack_ArenaHazard_Mine_Random -------
            if (State_Attack_ArenaHazard_Mine_Random.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = State_Attack_ArenaHazard_Mine_Random.Attack_Name;
                Attack_BestScore = State_Attack_ArenaHazard_Mine_Random.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_ArenaHazard_Mine_Random;
            }
            else if (State_Attack_ArenaHazard_Mine_Random.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = State_Attack_ArenaHazard_Mine_Random.Attack_Name;
                    Attack_BestScore = State_Attack_ArenaHazard_Mine_Random.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_ArenaHazard_Mine_Random;
                }
            }

            // State_Attack_Melee01 -----------------------------------
            if (State_Attack_Melee01.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = State_Attack_Melee01.Attack_Name;
                Attack_BestScore = State_Attack_Melee01.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Melee01;
            }
            else if (State_Attack_Melee01.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = State_Attack_Melee01.Attack_Name;
                    Attack_BestScore = State_Attack_Melee01.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Melee01;
                }
            }
        }

        // DEBUGGING (MUST BE REMOVED):
        //Attack_BestName = State_Attack_Bullet_SlowFiringShot_Easy.Attack_Name;
        //Attack_BestScore = State_Attack_Bullet_SlowFiringShot_Easy.CalculateScore(bossEnemyComponent);
        //Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_SlowFiringShot_Easy;

        // Transition to Best Choice -------------------------------------------------------------------------------*
        Attack_TransitionToExecute();
    }
}

// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// *               Attack Instruction States                                                                                                                                                                    * 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// When an attack is added, it must also have Transition() methods created in BossEnemy.cs and Attack Selection logic added in the State_Awake Inherited Class
// This state is used for testing attack selection 
// --------------------------------------------------------------------------------------------------------------------------------------------------
// *               Bullet-Bassed Projectile Attacks                                                                                                 * 
// --------------------------------------------------------------------------------------------------------------------------------------------------
public class State_Attack_Bullet_SlowFiringShot_Easy : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_SlowFiringShot_Easy";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 50.0f;

    // Spawner Values
    private float Attack_FireRate = 0.5f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 6;
    private bool Attack_TrackHorizontal = false;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 0.0f;
    private float Attack_ProjectileSpeed = 30.0f;
    private float Attack_ProjectileLifetime = 10.0f;

    // Attack Spawner
    private ProjectileSpawner SpawnerComponent_Bullet;


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
        // Debugging
        //Debug.Log("BossEnemy: Entering State_Attack_Bullet_SlowFiringShot_Easy");

        // Boss Enemy Logic
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost); // energy cost of attack applied

        // Spawner Logic
        SpawnerComponent_Bullet = bossEnemyComponent.ReturnComponent_Spawner_Bullet();
        SpawnerComponent_Bullet.ReturnAllProjectilesToPool();
        SpawnerComponent_Bullet.UpdateSpawner_AllValues(Attack_FireRate, Attack_Count, Attack_TrackHorizontal, Attack_TrackVertical, Attack_TrackSpeed);
        SpawnerComponent_Bullet.Set_All_ProjectileLifetime(Attack_ProjectileLifetime);
        SpawnerComponent_Bullet.Set_Bullet_ProjectileSpeed(Attack_ProjectileSpeed);

        SpawnerComponent_Bullet.Reset_FirePointPositionToGameObject();
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Spawner Logic
        // Attack is still occuring
        if (SpawnerComponent_Bullet.ReturnSpawnerActive() == true)
        {
            //Debug.Log("BossEnemy: Spawner Is Active");
            if (SpawnerComponent_Bullet.IsSpawnerReadyToFire() == true)
            {
                ////Debug.Log("BossEnemy: Spawner Ready To Fire");
                SpawnerComponent_Bullet.PreAttackLogic();

                Vector3 playerPos = bossEnemyComponent.Player_EstimateFuturePosition(0.5f);
                SpawnerComponent_Bullet.Update_FirePointRotation_FaceTarget(playerPos, 0.0f, 0.0f, true, true);
                SpawnerComponent_Bullet.Spawner_Bullet_SingleShot(true);

                SpawnerComponent_Bullet.PostAttackLogic();
            }
        }
        // Attack has finished
        else
        {
            //Debug.Log("BossEnemy: Attack Completed");
            Attack_Completed = true;
        }

        // Animation Logic

    }

    // Called once per frame - after update
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.HP_IsZero())                                  // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToState_Death();                     // if so, transition to Death State
        }
        else if (Attack_Completed == true)
        {
            bossEnemyComponent.TransitionToState_SelfCheck();
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called when the state machine transitions out of this state
    public override void Exit()
    {
        // Programming Logic
        bossEnemyComponent.appendToAttackHistory(Attack_Name);

        SpawnerComponent_Bullet.Reset_FirePointRotationToGameObject();

        // Animation Logic
        animator.SetBool("inAttack", false);

    }
}

public class State_Attack_Bullet_SlowFiringShot_Hard : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_SlowFiringShot_Hard";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 50.0f;

    // Spawner Values
    private float Attack_Starting_FireRate = 0.5f;
    private float Attack_Final_FireRate = 2.5f;
    private float Attack_Current_FireRate;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 12;
    private bool Attack_TrackHorizontal = false;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 0.0f;
    private float Attack_Starting_ProjectileSpeed = 20.0f;
    private float Attack_Final_ProjectileSpeed = 60.0f;
    private float Attack_Current_ProjectileSpeed;
    private float Attack_ProjectileLifetime = 10.0f;

    // Attack Spawner
    private ProjectileSpawner SpawnerComponent_Bullet;


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
        // Debugging
        //Debug.Log("BossEnemy: Entering State_Attack_Bullet_SlowFiringShot_Hard");

        // Boss Enemy Logic
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost); // energy cost of attack applied

        // Spawner Logic
        SpawnerComponent_Bullet = bossEnemyComponent.ReturnComponent_Spawner_Bullet();
        SpawnerComponent_Bullet.ReturnAllProjectilesToPool();
        SpawnerComponent_Bullet.UpdateSpawner_AllValues(Attack_Starting_FireRate, Attack_Count, Attack_TrackHorizontal, Attack_TrackVertical, Attack_TrackSpeed);
        SpawnerComponent_Bullet.Set_All_ProjectileLifetime(Attack_ProjectileLifetime);
        SpawnerComponent_Bullet.Set_Bullet_ProjectileSpeed(Attack_Starting_ProjectileSpeed);

        SpawnerComponent_Bullet.Reset_FirePointPositionToGameObject();
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Spawner Logic
        // Attack is still occuring
        if (SpawnerComponent_Bullet.ReturnSpawnerActive() == true)
        {
            ////Debug.Log("BossEnemy: Spawner Is Active");
            // Spawner is ready for next projectile fire
            if (SpawnerComponent_Bullet.IsSpawnerReadyToFire() == true)
            {
                ////Debug.Log("BossEnemy: Spawner Ready To Fire");
                SpawnerComponent_Bullet.PreAttackLogic();

                // increment fire rate and projectile speed
                float fireRateStep = (Attack_Final_FireRate - Attack_Starting_FireRate) / (Attack_Count - 1);
                Attack_Current_FireRate += fireRateStep;
                Attack_Current_FireRate = Mathf.Clamp(Attack_Current_FireRate, Attack_Starting_FireRate, Attack_Final_FireRate);
                SpawnerComponent_Bullet.UpdateSpawner_FireRate(Attack_Current_FireRate);
                float projectileSpeedStep = (Attack_Final_ProjectileSpeed - Attack_Starting_ProjectileSpeed) / (Attack_Count - 1);
                Attack_Current_ProjectileSpeed += projectileSpeedStep;
                Attack_Current_ProjectileSpeed = Mathf.Clamp(Attack_Current_ProjectileSpeed, Attack_Starting_ProjectileSpeed, Attack_Final_ProjectileSpeed);
                SpawnerComponent_Bullet.Set_Bullet_ProjectileSpeed(Attack_Current_ProjectileSpeed);

                Vector3 playerPos = bossEnemyComponent.Player_EstimateFuturePosition(0.5f);
                SpawnerComponent_Bullet.Update_FirePointRotation_FaceTarget(playerPos, 0.0f, 0.0f, true, true);
                SpawnerComponent_Bullet.Spawner_Bullet_SingleShot(true);

                SpawnerComponent_Bullet.PostAttackLogic();
            }
        }
        // Attack has finished
        else
        {
            //Debug.Log("BossEnemy: Attack Completed");
            Attack_Completed = true;
        }

        // Animation Logic

    }

    // Called once per frame - after update
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.HP_IsZero())                                  // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToState_Death();                     // if so, transition to Death State
        }
        else if (Attack_Completed == true)
        {
            bossEnemyComponent.TransitionToState_SelfCheck();
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called when the state machine transitions out of this state
    public override void Exit()
    {
        // Programming Logic
        bossEnemyComponent.appendToAttackHistory(Attack_Name);

        SpawnerComponent_Bullet.Reset_FirePointRotationToGameObject();

        // Animation Logic
        animator.SetBool("inAttack", false);

    }
}

public class State_Attack_Bullet_RapidFireShot_Easy : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_RapidFireShot_Easy";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 50.0f;

    // Spawner Values
    private float Attack_FireRate = 2.0f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 24;
    private bool Attack_TrackHorizontal = false;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 0.0f;
    private float Attack_ProjectileSpeed = 30.0f;
    private float Attack_ProjectileLifetime = 10.0f;

    // Attack Spawner
    private ProjectileSpawner SpawnerComponent_Bullet;

    // Attack Values
    // Spawner_Bullet_StackedConeShot(int Projectile_Count, float AngleOfSpread, int Projectile_VerticalCount, float Spawner_MinHeight, float Spawner_MaxHeight)
    private int Attack_ProjectileCount = 3;
    private float Attack_AngleOfSpread = 10.0f;
    private int Attack_ProjectileVerticalCount = 3;
    private float Attack_MinHeight = 0.0f;
    private float Attack_MaxHeight = 3.0f;

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
        // Debugging
        //Debug.Log("BossEnemy: Entering State_Attack_Bullet_RapidFireShot_Easy");

        // Boss Enemy Logic
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost); // energy cost of attack applied

        // Spawner Logic
        SpawnerComponent_Bullet = bossEnemyComponent.ReturnComponent_Spawner_Bullet();
        SpawnerComponent_Bullet.ReturnAllProjectilesToPool();
        SpawnerComponent_Bullet.UpdateSpawner_AllValues(Attack_FireRate, Attack_Count, Attack_TrackHorizontal, Attack_TrackVertical, Attack_TrackSpeed);
        SpawnerComponent_Bullet.Set_All_ProjectileLifetime(Attack_ProjectileLifetime);
        SpawnerComponent_Bullet.Set_Bullet_ProjectileSpeed(Attack_ProjectileSpeed);

        SpawnerComponent_Bullet.Reset_FirePointPositionToGameObject();
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Spawner Logic
        // Attack is still occuring
        if (SpawnerComponent_Bullet.ReturnSpawnerActive() == true)
        {
            //Debug.Log("BossEnemy: Spawner Is Active");
            if (SpawnerComponent_Bullet.IsSpawnerReadyToFire() == true)
            {
                ////Debug.Log("BossEnemy: Spawner Ready To Fire");
                SpawnerComponent_Bullet.PreAttackLogic();

                Vector3 playerPos;
                if (bossEnemyComponent.Player_GetDistanceFromXSecondsAgo(1.0f) < 5.0f)
                {
                    playerPos = bossEnemyComponent.Player_ReturnPlayerPosition();
                    SpawnerComponent_Bullet.Update_FirePointRotation_FaceTarget(playerPos, 0.0f, 0.0f, true, true);
                }
                else
                {
                    playerPos = bossEnemyComponent.Player_EstimateFuturePosition(1.0f);
                    SpawnerComponent_Bullet.Update_FirePointRotation_FaceTarget(playerPos, 0.0f, 0.0f, true, true);
                }

                SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_ProjectileCount, Attack_AngleOfSpread, Attack_ProjectileVerticalCount, Attack_MinHeight, Attack_MaxHeight);

                SpawnerComponent_Bullet.PostAttackLogic();
            }
        }
        // Attack has finished
        else
        {
            //Debug.Log("BossEnemy: Attack Completed");
            Attack_Completed = true;
        }

        // Animation Logic

    }

    // Called once per frame - after update
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.HP_IsZero())                                  // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToState_Death();                     // if so, transition to Death State
        }
        else if (Attack_Completed == true)
        {
            bossEnemyComponent.TransitionToState_SelfCheck();
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
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
        animator.SetBool("inAttack", false);

    }
}

public class State_Attack_Bullet_RapidFireShot_Hard : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_RapidFireShot_Easy";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 50.0f;

    // Spawner Values
    private float Attack_FireRate = 4.0f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 48;
    private bool Attack_TrackHorizontal = false;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 0.0f;
    private float Attack_ProjectileSpeed = 30.0f;
    private float Attack_ProjectileLifetime = 10.0f;

    // Attack Spawner
    private ProjectileSpawner SpawnerComponent_Bullet;

    // Attack Values
    // Spawner_Bullet_StackedConeShot(int Projectile_Count, float AngleOfSpread, int Projectile_VerticalCount, float Spawner_MinHeight, float Spawner_MaxHeight)
    private int Attack_ProjectileCount = 3;
    private float Attack_AngleOfSpread = 10.0f;
    private int Attack_ProjectileVerticalCount = 3;
    private float Attack_MinHeight = 0.0f;
    private float Attack_MaxHeight = 3.0f;

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
        // Debugging
        //Debug.Log("BossEnemy: Entering State_Attack_Bullet_RapidFireShot_Easy");

        // Boss Enemy Logic
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost); // energy cost of attack applied

        // Spawner Logic
        SpawnerComponent_Bullet = bossEnemyComponent.ReturnComponent_Spawner_Bullet();
        SpawnerComponent_Bullet.ReturnAllProjectilesToPool();
        SpawnerComponent_Bullet.UpdateSpawner_AllValues(Attack_FireRate, Attack_Count, Attack_TrackHorizontal, Attack_TrackVertical, Attack_TrackSpeed);
        SpawnerComponent_Bullet.Set_All_ProjectileLifetime(Attack_ProjectileLifetime);
        SpawnerComponent_Bullet.Set_Bullet_ProjectileSpeed(Attack_ProjectileSpeed);

        SpawnerComponent_Bullet.Reset_FirePointPositionToGameObject();
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Spawner Logic
        // Attack is still occuring
        if (SpawnerComponent_Bullet.ReturnSpawnerActive() == true)
        {
            //Debug.Log("BossEnemy: Spawner Is Active");
            if (SpawnerComponent_Bullet.IsSpawnerReadyToFire() == true)
            {
                ////Debug.Log("BossEnemy: Spawner Ready To Fire");
                SpawnerComponent_Bullet.PreAttackLogic();

                Vector3 playerPos;
                if (bossEnemyComponent.Player_GetDistanceFromXSecondsAgo(1.0f) < 5.0f)
                {
                    playerPos = bossEnemyComponent.Player_ReturnPlayerPosition();
                    SpawnerComponent_Bullet.Update_FirePointRotation_FaceTarget(playerPos, 0.0f, 0.0f, true, true);
                }
                else
                {
                    playerPos = bossEnemyComponent.Player_EstimateFuturePosition(0.75f);
                    SpawnerComponent_Bullet.Update_FirePointRotation_FaceTarget(playerPos, 0.0f, 0.0f, true, true);
                }

                SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_ProjectileCount, Attack_AngleOfSpread, Attack_ProjectileVerticalCount, Attack_MinHeight, Attack_MaxHeight);

                SpawnerComponent_Bullet.PostAttackLogic();
            }
        }
        // Attack has finished
        else
        {
            //Debug.Log("BossEnemy: Attack Completed");
            Attack_Completed = true;
        }

        // Animation Logic

    }

    // Called once per frame - after update
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.HP_IsZero())                                  // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToState_Death();                     // if so, transition to Death State
        }
        else if (Attack_Completed == true)
        {
            bossEnemyComponent.TransitionToState_SelfCheck();
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
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
        animator.SetBool("inAttack", false);

    }
}

public class State_Attack_Bullet_TrackingCone_Easy : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_TrackingCone_Easy";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 50.0f;

    // Spawner Values
    private float Attack_FireRate = 0.5f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 7;
    private bool Attack_TrackHorizontal = true;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 80.0f;
    private float Attack_ProjectileSpeed = 12.0f;
    private float Attack_ProjectileLifetime = 10.0f;

    // Attack Spawner
    private ProjectileSpawner SpawnerComponent_Bullet;

    // Attack Values
    // Spawner_Bullet_StackedConeShot(int Projectile_Count, float AngleOfSpread, int Projectile_VerticalCount, float Spawner_MinHeight, float Spawner_MaxHeight)
    private int Attack_ProjectileCount = 10;
    private int Attack_ProjectileCount_ALT = 11;
    private float Attack_AngleOfSpread = 60.0f;
    private int Attack_ProjectileVerticalCount = 3;
    private float Attack_MinHeight = 0.0f;
    private float Attack_MaxHeight = 3.5f;
    private bool Attack_Alt = false;

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
        // Debugging
        //Debug.Log("BossEnemy: Entering State_Attack_Bullet_TrackingCone_Easy");

        // Boss Enemy Logic
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost); // energy cost of attack applied

        // Spawner Logic
        SpawnerComponent_Bullet = bossEnemyComponent.ReturnComponent_Spawner_Bullet();
        SpawnerComponent_Bullet.ReturnAllProjectilesToPool();
        SpawnerComponent_Bullet.UpdateSpawner_AllValues(Attack_FireRate, Attack_Count, Attack_TrackHorizontal, Attack_TrackVertical, Attack_TrackSpeed);
        SpawnerComponent_Bullet.Set_All_ProjectileLifetime(Attack_ProjectileLifetime);
        SpawnerComponent_Bullet.Set_Bullet_ProjectileSpeed(Attack_ProjectileSpeed);

        SpawnerComponent_Bullet.Update_FirePointPosition(null, 0.5f, null);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Spawner Logic
        // Attack is still occuring
        if (SpawnerComponent_Bullet.ReturnSpawnerActive() == true)
        {
            ////Debug.Log("BossEnemy: Spawner Is Active");
            // Spawner is ready for next projectile fire
            if (SpawnerComponent_Bullet.IsSpawnerReadyToFire() == true)
            {
                ////Debug.Log("BossEnemy: Spawner Ready To Fire");
                SpawnerComponent_Bullet.PreAttackLogic();

                // On first shot, miss player
                if (SpawnerComponent_Bullet.IsSpawnerRemainingAttackCountEqualToValue(Attack_Count) == true)
                {
                    SpawnerComponent_Bullet.UpdateSpawner_Tracking(false, false, 0);

                    // Rotate spawner to face away from player by random ammount between 90 - 270 degrees
                    float randomRotation = Random.Range(90.0f, 270.0001f);
                    Vector3 playerPos = bossEnemyComponent.Player_ReturnPlayerPosition();
                    SpawnerComponent_Bullet.Update_FirePointRotation_FaceTarget(playerPos, randomRotation, 0.0f, true, false);

                    SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_ProjectileCount, Attack_AngleOfSpread, Attack_ProjectileVerticalCount, Attack_MinHeight, Attack_MaxHeight);
                    SpawnerComponent_Bullet.UpdateSpawner_Tracking(Attack_TrackHorizontal, Attack_TrackVertical, Attack_TrackSpeed);
                }

                // Otherwise track and shoot player
                else
                {
                    if (Attack_Alt == false)
                    {
                        Attack_Alt = true;
                        SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_ProjectileCount, Attack_AngleOfSpread, Attack_ProjectileVerticalCount, Attack_MinHeight, Attack_MaxHeight);
                    }
                    else
                    {
                        Attack_Alt = false;
                        SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_ProjectileCount_ALT, Attack_AngleOfSpread, Attack_ProjectileVerticalCount, Attack_MinHeight, Attack_MaxHeight);
                    }
                }
                
                SpawnerComponent_Bullet.PostAttackLogic();
            }
        }
        // Attack has finished
        else
        {
            //Debug.Log("BossEnemy: Attack Completed");
            Attack_Completed = true;
        }

        // Animation Logic

    }

    // Called once per frame - after update
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.HP_IsZero())                                  // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToState_Death();                     // if so, transition to Death State
        }
        else if (Attack_Completed == true)
        {
            bossEnemyComponent.TransitionToState_SelfCheck();
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
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
        animator.SetBool("inAttack", false);

    }
}

public class State_Attack_Bullet_TrackingCone_Hard : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_TrackingCone_Hard";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 50.0f;

    // Spawner Values
    private float Attack_FireRate = 1.0f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 15;
    private bool Attack_TrackHorizontal = true;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 200.0f;
    private float Attack_ProjectileSpeed = 15.0f;
    private float Attack_ProjectileLifetime = 10.0f;

    // Attack Spawner
    private ProjectileSpawner SpawnerComponent_Bullet;

    // Attack Values
    // Spawner_Bullet_StackedConeShot(int Projectile_Count, float AngleOfSpread, int Projectile_VerticalCount, float Spawner_MinHeight, float Spawner_MaxHeight)
    private int Attack_ProjectileCount = 20;
    private int Attack_ProjectileCount_ALT = 21;
    private float Attack_AngleOfSpread = 90.0f;
    private int Attack_ProjectileVerticalCount = 3;
    private float Attack_MinHeight = 0.0f;
    private float Attack_MaxHeight = 3.5f;
    private bool Attack_Alt = false;


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
        // Debugging
        //Debug.Log("BossEnemy: Entering State_Attack_Bullet_TrackingCone_Hard");

        // Boss Enemy Logic
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost); // energy cost of attack applied

        // Spawner Logic
        SpawnerComponent_Bullet = bossEnemyComponent.ReturnComponent_Spawner_Bullet();
        SpawnerComponent_Bullet.ReturnAllProjectilesToPool();
        SpawnerComponent_Bullet.UpdateSpawner_AllValues(Attack_FireRate, Attack_Count, Attack_TrackHorizontal, Attack_TrackVertical, Attack_TrackSpeed);
        SpawnerComponent_Bullet.Set_All_ProjectileLifetime(Attack_ProjectileLifetime);
        SpawnerComponent_Bullet.Set_Bullet_ProjectileSpeed(Attack_ProjectileSpeed);

        SpawnerComponent_Bullet.Update_FirePointPosition(null, 0.5f, null);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Spawner Logic
        // Attack is still occuring
        if (SpawnerComponent_Bullet.ReturnSpawnerActive() == true)
        {
            ////Debug.Log("BossEnemy: Spawner Is Active");
            // Spawner is ready for next projectile fire
            if (SpawnerComponent_Bullet.IsSpawnerReadyToFire() == true)
            {
                ////Debug.Log("BossEnemy: Spawner Ready To Fire");
                SpawnerComponent_Bullet.PreAttackLogic();

                // On first shot, miss player
                if (SpawnerComponent_Bullet.IsSpawnerRemainingAttackCountEqualToValue(Attack_Count) == true)
                {
                    SpawnerComponent_Bullet.UpdateSpawner_Tracking(false, false, 0);

                    // Rotate spawner to face away from player by random ammount between 90 - 270 degrees
                    float randomRotation = Random.Range(90.0f, 270.0001f);
                    Vector3 playerPos = bossEnemyComponent.Player_ReturnPlayerPosition();
                    SpawnerComponent_Bullet.Update_FirePointRotation_FaceTarget(playerPos, randomRotation, 0.0f, true, false);

                    SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_ProjectileCount, Attack_AngleOfSpread, Attack_ProjectileVerticalCount, Attack_MinHeight, Attack_MaxHeight);
                    SpawnerComponent_Bullet.UpdateSpawner_Tracking(Attack_TrackHorizontal, Attack_TrackVertical, Attack_TrackSpeed);
                }

                // Otherwise track and shoot player
                else
                {
                    if (Attack_Alt == false)
                    {
                        Attack_Alt = true;
                        SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_ProjectileCount, Attack_AngleOfSpread, Attack_ProjectileVerticalCount, Attack_MinHeight, Attack_MaxHeight);
                    }
                    else
                    {
                        Attack_Alt = false;
                        SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_ProjectileCount_ALT, Attack_AngleOfSpread, Attack_ProjectileVerticalCount, Attack_MinHeight, Attack_MaxHeight);
                    }
                }

                SpawnerComponent_Bullet.PostAttackLogic();
            }
        }
        // Attack has finished
        else
        {
            //Debug.Log("BossEnemy: Attack Completed");
            Attack_Completed = true;
        }

        // Animation Logic

    }

    // Called once per frame - after update
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.HP_IsZero())                                  // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToState_Death();                     // if so, transition to Death State
        }
        else if (Attack_Completed == true)
        {
            bossEnemyComponent.TransitionToState_SelfCheck();
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
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
        animator.SetBool("inAttack", false);

    }
}

public class State_Attack_Bullet_TrackingWall_Easy : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_TrackingWall_Easy";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 50.0f;

    // Spawner Values
    private float Attack_FireRate = 8.0f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 100;
    private bool Attack_TrackHorizontal = false;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 0.0f;
    private float Attack_ProjectileSpeed = 20.0f;
    private float Attack_ProjectileLifetime = 10.0f;

    // Attack Spawner
    private ProjectileSpawner SpawnerComponent_Bullet;

    // Attack Values
    // Spawner_Bullet_StackedConeShot(int Projectile_Count, float AngleOfSpread, int Projectile_VerticalCount, float Spawner_MinHeight, float Spawner_MaxHeight)
    private int Attack_JumpRope_ProjectileCount = 3;
    private float Attack_JumpRope_AngleOfSpread = 3.0f;
    private int Attack_JumpRope_ProjectileVerticalCount = 3;
    private float Attack_JumpRope_MinHeight = 0.0f;
    private float Attack_JumpRope_MaxHeight = 1.0f;
    private float Spawner_JumpRope_Rotation_Speed = 120.0f;
    private Quaternion Spawner_JumpRope_RotationQuat;

    private int Attack_Wall_ProjectileCount = 1;
    private float Attack_Wall_AngleOfSpread = 1.0f;
    private int Attack_Wall_ProjectileVerticalCount = 8;
    private float Attack_Wall_MinHeight = 0.0f;
    private float Attack_Wall_MaxHeight = 7.5f;

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
        // Debugging
        //Debug.Log("BossEnemy: Entering State_Attack_Bullet_TrackingWall_Easy");

        // Boss Enemy Logic
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost); // energy cost of attack applied

        if (Random.Range(0, 2) == 0)
        {
            Spawner_JumpRope_Rotation_Speed = Spawner_JumpRope_Rotation_Speed * -1;
        }

        // Spawner Logic
        SpawnerComponent_Bullet = bossEnemyComponent.ReturnComponent_Spawner_Bullet();
        SpawnerComponent_Bullet.ReturnAllProjectilesToPool();
        SpawnerComponent_Bullet.UpdateSpawner_AllValues(Attack_FireRate, Attack_Count, Attack_TrackHorizontal, Attack_TrackVertical, Attack_TrackSpeed);
        SpawnerComponent_Bullet.Set_All_ProjectileLifetime(Attack_ProjectileLifetime);
        SpawnerComponent_Bullet.Set_Bullet_ProjectileSpeed(Attack_ProjectileSpeed);

        SpawnerComponent_Bullet.Update_FirePointPosition(null, 0.5f, null);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Spawner Logic
        // Attack is still occuring
        if (SpawnerComponent_Bullet.ReturnSpawnerActive() == true)
        {
            //increment rotation
            float Spawner_Rotation_PerFrame = Spawner_JumpRope_Rotation_Speed * Time.deltaTime;
            SpawnerComponent_Bullet.Update_FirePointRotation(SpawnerComponent_Bullet.ReturnSpawnerTransform().rotation *= Quaternion.Euler(0, Spawner_Rotation_PerFrame, 0));

            // Spawner is ready for next projectile fire
            if (SpawnerComponent_Bullet.IsSpawnerReadyToFire() == true)
            {
                SpawnerComponent_Bullet.PreAttackLogic();
                
                // Jump Rope
                SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_JumpRope_ProjectileCount, Attack_JumpRope_AngleOfSpread, Attack_JumpRope_ProjectileVerticalCount, Attack_JumpRope_MinHeight, Attack_JumpRope_MaxHeight);

                // Tracking Wall
                Spawner_JumpRope_RotationQuat = SpawnerComponent_Bullet.ReturnSpawnerTransform().rotation;
                Vector3 playerPos = bossEnemyComponent.Player_ReturnPlayerPosition();
                SpawnerComponent_Bullet.Update_FirePointRotation_FaceTarget(playerPos, 0.0f, 0.0f, true, false);
                SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_Wall_ProjectileCount, Attack_Wall_AngleOfSpread, Attack_Wall_ProjectileVerticalCount, Attack_Wall_MinHeight, Attack_Wall_MaxHeight);
                SpawnerComponent_Bullet.Update_FirePointRotation(Spawner_JumpRope_RotationQuat);

                SpawnerComponent_Bullet.PostAttackLogic();
            }
        }
        // Attack has finished
        else
        {
            //Debug.Log("BossEnemy: Attack Completed");
            Attack_Completed = true;
        }

        // Animation Logic

    }

    // Called once per frame - after update
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.HP_IsZero())                                  // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToState_Death();                     // if so, transition to Death State
        }
        else if (Attack_Completed == true)
        {
            bossEnemyComponent.TransitionToState_SelfCheck();
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
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
        animator.SetBool("inAttack", false);

    }
}

public class State_Attack_Bullet_TrackingWall_Hard : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_TrackingWall_Hard";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 50.0f;

    // Spawner Values
    private float Attack_FireRate = 10.0f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 125;
    private bool Attack_TrackHorizontal = false;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 0.0f;
    private float Attack_ProjectileSpeed = 25.0f;
    private float Attack_ProjectileLifetime = 10.0f;

    // Attack Spawner
    private ProjectileSpawner SpawnerComponent_Bullet;

    // Attack Values
    // Spawner_Bullet_StackedConeShot(int Projectile_Count, float AngleOfSpread, int Projectile_VerticalCount, float Spawner_MinHeight, float Spawner_MaxHeight)
    private int Attack_JumpRope_ProjectileCount = 3;
    private float Attack_JumpRope_AngleOfSpread = 3.0f;
    private int Attack_JumpRope_ProjectileVerticalCount = 3;
    private float Attack_JumpRope_MinHeight = 0.0f;
    private float Attack_JumpRope_MaxHeight = 2.0f;
    private float Spawner_JumpRope_Rotation_Speed = 210.0f;
    private Quaternion Spawner_JumpRope_RotationQuat;

    private int Attack_Wall_ProjectileCount = 1;
    private float Attack_Wall_AngleOfSpread = 1.0f;
    private int Attack_Wall_ProjectileVerticalCount = 8;
    private float Attack_Wall_MinHeight = 0.0f;
    private float Attack_Wall_MaxHeight = 7.5f;
    private bool Attack_Wall_Fire = false;

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
        // Debugging
        //Debug.Log("BossEnemy: Entering State_Attack_Bullet_TrackingWall_Hard");

        // Boss Enemy Logic
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost); // energy cost of attack applied

        if (Random.Range(0, 2) == 0)
        {
            Spawner_JumpRope_Rotation_Speed = Spawner_JumpRope_Rotation_Speed * -1;
        }

        // Spawner Logic
        SpawnerComponent_Bullet = bossEnemyComponent.ReturnComponent_Spawner_Bullet();
        SpawnerComponent_Bullet.ReturnAllProjectilesToPool();
        SpawnerComponent_Bullet.UpdateSpawner_AllValues(Attack_FireRate, Attack_Count, Attack_TrackHorizontal, Attack_TrackVertical, Attack_TrackSpeed);
        SpawnerComponent_Bullet.Set_All_ProjectileLifetime(Attack_ProjectileLifetime);
        SpawnerComponent_Bullet.Set_Bullet_ProjectileSpeed(Attack_ProjectileSpeed);

        SpawnerComponent_Bullet.Update_FirePointPosition(null, 0.5f, null);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Spawner Logic
        // Attack is still occuring
        if (SpawnerComponent_Bullet.ReturnSpawnerActive() == true)
        {
            //increment rotation
            float Spawner_Rotation_PerFrame = Spawner_JumpRope_Rotation_Speed * Time.deltaTime;
            SpawnerComponent_Bullet.Update_FirePointRotation(SpawnerComponent_Bullet.ReturnSpawnerTransform().rotation *= Quaternion.Euler(0, Spawner_Rotation_PerFrame, 0));

            // Spawner is ready for next projectile fire
            if (SpawnerComponent_Bullet.IsSpawnerReadyToFire() == true)
            {
                SpawnerComponent_Bullet.PreAttackLogic();

                // Jump Rope
                SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_JumpRope_ProjectileCount, Attack_JumpRope_AngleOfSpread, Attack_JumpRope_ProjectileVerticalCount, Attack_JumpRope_MinHeight, Attack_JumpRope_MaxHeight);

                // Jump Rope ALT
                float randomDegree = Random.Range(0.0f, 360.0f);
                SpawnerComponent_Bullet.Update_FirePointRotation(SpawnerComponent_Bullet.ReturnSpawnerTransform().rotation *= Quaternion.Euler(0, randomDegree, 0));
                SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_JumpRope_ProjectileCount, Attack_JumpRope_AngleOfSpread, Attack_JumpRope_ProjectileVerticalCount, Attack_JumpRope_MinHeight, Attack_JumpRope_MaxHeight);
                SpawnerComponent_Bullet.Update_FirePointRotation(SpawnerComponent_Bullet.ReturnSpawnerTransform().rotation *= Quaternion.Euler(0, -randomDegree, 0));

                // Tracking Wall
                if (Attack_Wall_Fire == true)
                {
                    Spawner_JumpRope_RotationQuat = SpawnerComponent_Bullet.ReturnSpawnerTransform().rotation;
                    Vector3 playerPos = bossEnemyComponent.Player_ReturnPlayerPosition();
                    SpawnerComponent_Bullet.Update_FirePointRotation_FaceTarget(playerPos, 0.0f, 0.0f, true, false);
                    SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_Wall_ProjectileCount, Attack_Wall_AngleOfSpread, Attack_Wall_ProjectileVerticalCount, Attack_Wall_MinHeight, Attack_Wall_MaxHeight);
                    SpawnerComponent_Bullet.Update_FirePointRotation(Spawner_JumpRope_RotationQuat);
                    Attack_Wall_Fire = false;
                }
                else
                {
                    Attack_Wall_Fire = true;
                }

                SpawnerComponent_Bullet.PostAttackLogic();
            }
        }
        // Attack has finished
        else
        {
            //Debug.Log("BossEnemy: Attack Completed");
            Attack_Completed = true;
        }

        // Animation Logic

    }

    // Called once per frame - after update
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.HP_IsZero())                                  // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToState_Death();                     // if so, transition to Death State
        }
        else if (Attack_Completed == true)
        {
            bossEnemyComponent.TransitionToState_SelfCheck();
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
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
        animator.SetBool("inAttack", false);

    }
}

public class State_Attack_Bullet_RotatingWall_Easy : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_RotatingWall_Easy";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 50.0f;

    // Spawner Values
    private float Attack_FireRate = 8.0f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 160;
    private bool Attack_TrackHorizontal = false;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 0.0f;
    private float Attack_ProjectileSpeed = 15.0f;
    private float Attack_ProjectileLifetime = 10.0f;
    private float Spawner_Rotation_Speed = 40.0f;

    // Attack Spawner
    private ProjectileSpawner SpawnerComponent_Bullet;

    // Attack Values
    // Spawner_Bullet_StackedConeShot(int Projectile_Count, float AngleOfSpread, int Projectile_VerticalCount, float Spawner_MinHeight, float Spawner_MaxHeight)
    private int Attack_ProjectileCount = 1;
    private float Attack_AngleOfSpread = 1.0f;
    private int Attack_ProjectileVerticalCount = 4;
    private float Attack_TallWall_MinHeight = 4.0f;
    private float Attack_TallWall_MaxHeight = 7.0f;
    private float Attack_LowWall_MinHeight = 0.0f;
    private float Attack_LowWall_MaxHeight = 3.0f;

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
        // Debugging
        //Debug.Log("BossEnemy: Entering State_Attack_Bullet_RotatingWall_Easy");

        // Boss Enemy Logic
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost); // energy cost of attack applied

        // Spawner Logic
        SpawnerComponent_Bullet = bossEnemyComponent.ReturnComponent_Spawner_Bullet();
        SpawnerComponent_Bullet.ReturnAllProjectilesToPool();
        SpawnerComponent_Bullet.UpdateSpawner_AllValues(Attack_FireRate, Attack_Count, Attack_TrackHorizontal, Attack_TrackVertical, Attack_TrackSpeed);
        SpawnerComponent_Bullet.Set_All_ProjectileLifetime(Attack_ProjectileLifetime);
        SpawnerComponent_Bullet.Set_Bullet_ProjectileSpeed(Attack_ProjectileSpeed);

        float randomRotation = Random.Range(0.0f, 360.0f);
        SpawnerComponent_Bullet.Update_FirePointRotation(null, randomRotation, null);
        SpawnerComponent_Bullet.Update_FirePointPosition(null, 0.5f, null);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Spawner Logic
        // Attack is still occuring
        if (SpawnerComponent_Bullet.ReturnSpawnerActive() == true)
        {
            ////Debug.Log("BossEnemy: Spawner Is Active");

            //increment rotation
            float Spawner_Rotation_PerFrame = Spawner_Rotation_Speed * Time.deltaTime;
            SpawnerComponent_Bullet.Update_FirePointRotation(SpawnerComponent_Bullet.ReturnSpawnerTransform().rotation *= Quaternion.Euler(0, Spawner_Rotation_PerFrame, 0));

            // Spawner is ready for next projectile fire
            if (SpawnerComponent_Bullet.IsSpawnerReadyToFire() == true)
            {
                ////Debug.Log("BossEnemy: Spawner Ready To Fire");
                SpawnerComponent_Bullet.PreAttackLogic();

                for (int i = 1; i <= 4; i++)
                {
                    if (i % 2 == 0)
                    {
                        // shoot low
                        SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_ProjectileCount, Attack_AngleOfSpread, Attack_ProjectileVerticalCount, Attack_LowWall_MinHeight, Attack_LowWall_MaxHeight);
                    }
                    else
                    {
                        // shoot high
                        SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_ProjectileCount, Attack_AngleOfSpread, Attack_ProjectileVerticalCount, Attack_TallWall_MinHeight, Attack_TallWall_MaxHeight);
                    }
                    // rotate 90 degrees
                    SpawnerComponent_Bullet.Update_FirePointRotation(SpawnerComponent_Bullet.ReturnSpawnerTransform().rotation *= Quaternion.Euler(0, 90.0f, 0));
                }

                SpawnerComponent_Bullet.PostAttackLogic();
            }
        }
        // Attack has finished
        else
        {
            //Debug.Log("BossEnemy: Attack Completed");
            Attack_Completed = true;
        }

        // Animation Logic

    }

    // Called once per frame - after update
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.HP_IsZero())                                  // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToState_Death();                     // if so, transition to Death State
        }
        else if (Attack_Completed == true)
        {
            bossEnemyComponent.TransitionToState_SelfCheck();
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
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
        animator.SetBool("inAttack", false);

    }
}

public class State_Attack_Bullet_RotatingWall_Hard : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_RotatingWall_Hard";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 50.0f;

    // Spawner Values
    private float Attack_FireRate = 16.0f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 320;
    private bool Attack_TrackHorizontal = false;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 0.0f;
    private float Attack_ProjectileSpeed = 30.0f;
    private float Attack_ProjectileLifetime = 10.0f;
    private float Spawner_Rotation_Speed = 120.0f;

    // Attack Spawner
    private ProjectileSpawner SpawnerComponent_Bullet;

    // Attack Values
    // Spawner_Bullet_StackedConeShot(int Projectile_Count, float AngleOfSpread, int Projectile_VerticalCount, float Spawner_MinHeight, float Spawner_MaxHeight)
    private int Attack_ProjectileCount = 1;
    private float Attack_AngleOfSpread = 1.0f;
    private int Attack_ProjectileVerticalCount = 4;
    private float Attack_TallWall_MinHeight = 4.0f;
    private float Attack_TallWall_MaxHeight = 7.0f;
    private float Attack_LowWall_MinHeight = 0.0f;
    private float Attack_LowWall_MaxHeight = 3.0f;

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
        // Debugging
        //Debug.Log("BossEnemy: Entering State_Attack_Bullet_RotatingWall_Hard");

        // Boss Enemy Logic
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost); // energy cost of attack applied

        // Spawner Logic
        SpawnerComponent_Bullet = bossEnemyComponent.ReturnComponent_Spawner_Bullet();
        SpawnerComponent_Bullet.ReturnAllProjectilesToPool();
        SpawnerComponent_Bullet.UpdateSpawner_AllValues(Attack_FireRate, Attack_Count, Attack_TrackHorizontal, Attack_TrackVertical, Attack_TrackSpeed);
        SpawnerComponent_Bullet.Set_All_ProjectileLifetime(Attack_ProjectileLifetime);
        SpawnerComponent_Bullet.Set_Bullet_ProjectileSpeed(Attack_ProjectileSpeed);

        float randomRotation = Random.Range(0.0f, 360.0f);
        SpawnerComponent_Bullet.Update_FirePointRotation(null, randomRotation, null);
        SpawnerComponent_Bullet.Update_FirePointPosition(null, 0.5f, null);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Spawner Logic
        // Attack is still occuring
        if (SpawnerComponent_Bullet.ReturnSpawnerActive() == true)
        {
            ////Debug.Log("BossEnemy: Spawner Is Active");

            //increment rotation
            float Spawner_Rotation_PerFrame = Spawner_Rotation_Speed * Time.deltaTime;
            SpawnerComponent_Bullet.Update_FirePointRotation(SpawnerComponent_Bullet.ReturnSpawnerTransform().rotation *= Quaternion.Euler(0, Spawner_Rotation_PerFrame, 0));

            // Spawner is ready for next projectile fire
            if (SpawnerComponent_Bullet.IsSpawnerReadyToFire() == true)
            {
                ////Debug.Log("BossEnemy: Spawner Ready To Fire");
                SpawnerComponent_Bullet.PreAttackLogic();

                for (int i = 1; i <= 4; i++)
                {
                    if (i % 2 == 0)
                    {
                        // shoot low
                        SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_ProjectileCount, Attack_AngleOfSpread, Attack_ProjectileVerticalCount, Attack_LowWall_MinHeight, Attack_LowWall_MaxHeight);
                    }
                    else
                    {
                        // shoot high
                        SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_ProjectileCount, Attack_AngleOfSpread, Attack_ProjectileVerticalCount, Attack_TallWall_MinHeight, Attack_TallWall_MaxHeight);
                    }
                    // rotate 90 degrees
                    SpawnerComponent_Bullet.Update_FirePointRotation(SpawnerComponent_Bullet.ReturnSpawnerTransform().rotation *= Quaternion.Euler(0, 90.0f, 0));
                }

                SpawnerComponent_Bullet.PostAttackLogic();
            }
        }
        // Attack has finished
        else
        {
            //Debug.Log("BossEnemy: Attack Completed");
            Attack_Completed = true;
        }

        // Animation Logic

    }

    // Called once per frame - after update
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.HP_IsZero())                                  // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToState_Death();                     // if so, transition to Death State
        }
        else if (Attack_Completed == true)
        {
            bossEnemyComponent.TransitionToState_SelfCheck();
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
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
        animator.SetBool("inAttack", false);

    }
}

public class State_Attack_Bullet_JumpRope_Easy : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_JumpRope_Easy";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 50.0f;

    // Spawner Values
    private float Attack_FireRate = 0.5f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 7;
    private bool Attack_TrackHorizontal = false;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 0.0f;
    private float Attack_ProjectileSpeed = 12.0f;
    private float Attack_ProjectileLifetime = 10.0f;

    // Attack Spawner
    private ProjectileSpawner SpawnerComponent_Bullet;

    // Attack Values
    // Spawner_Bullet_StackedConeShot(int Projectile_Count, float AngleOfSpread, int Projectile_VerticalCount, float Spawner_MinHeight, float Spawner_MaxHeight)
    private int Attack_ProjectileCount = 100;
    private float Attack_AngleOfSpread = 360.0f;
    private int Attack_ProjectileVerticalCount = 1;
    private float Attack_MinHeight = 0.5f;
    private float Attack_MaxHeight = 0.5f;


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
        // Debugging
        //Debug.Log("BossEnemy: Entering State_Attack_Bullet_JumpRope_Easy");

        // Boss Enemy Logic
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost); // energy cost of attack applied

        // Spawner Logic
        SpawnerComponent_Bullet = bossEnemyComponent.ReturnComponent_Spawner_Bullet();
        SpawnerComponent_Bullet.ReturnAllProjectilesToPool();
        SpawnerComponent_Bullet.UpdateSpawner_AllValues(Attack_FireRate, Attack_Count, Attack_TrackHorizontal, Attack_TrackVertical, Attack_TrackSpeed);
        SpawnerComponent_Bullet.Set_All_ProjectileLifetime(Attack_ProjectileLifetime);
        SpawnerComponent_Bullet.Set_Bullet_ProjectileSpeed(Attack_ProjectileSpeed);

        SpawnerComponent_Bullet.Update_FirePointPosition(null, 0.5f, null);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Spawner Logic
        // Attack is still occuring
        if (SpawnerComponent_Bullet.ReturnSpawnerActive() == true)
        {
            ////Debug.Log("BossEnemy: Spawner Is Active");
            // Spawner is ready for next projectile fire
            if (SpawnerComponent_Bullet.IsSpawnerReadyToFire() == true)
            {
                ////Debug.Log("BossEnemy: Spawner Ready To Fire");
                SpawnerComponent_Bullet.PreAttackLogic();

                float randomDegree = Random.Range(0.0f, 360.0f);
                SpawnerComponent_Bullet.Update_FirePointRotation(SpawnerComponent_Bullet.ReturnSpawnerTransform().rotation *= Quaternion.Euler(0, randomDegree, 0));
                SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_ProjectileCount, Attack_AngleOfSpread, Attack_ProjectileVerticalCount, Attack_MinHeight, Attack_MaxHeight);

                SpawnerComponent_Bullet.PostAttackLogic();
            }
        }
        // Attack has finished
        else
        {
            //Debug.Log("BossEnemy: Attack Completed");
            Attack_Completed = true;
        }

        // Animation Logic

    }

    // Called once per frame - after update
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.HP_IsZero())                                  // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToState_Death();                     // if so, transition to Death State
        }
        else if (Attack_Completed == true)
        {
            bossEnemyComponent.TransitionToState_SelfCheck();
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
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
        animator.SetBool("inAttack", false);

    }
}

public class State_Attack_Bullet_JumpRope_Hard : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_JumpRope_Easy";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 50.0f;

    // Spawner Values
    private float Attack_FireRate = 1.75f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 24;
    private bool Attack_TrackHorizontal = false;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 0.0f;
    private float Attack_ProjectileSpeed = 20.0f;
    private float Attack_ProjectileLifetime = 10.0f;

    // Attack Spawner
    private ProjectileSpawner SpawnerComponent_Bullet;

    // Attack Values
    // Spawner_Bullet_StackedConeShot(int Projectile_Count, float AngleOfSpread, int Projectile_VerticalCount, float Spawner_MinHeight, float Spawner_MaxHeight)
    private int Attack_ProjectileCount = 100;
    private float Attack_AngleOfSpread = 360.0f;
    private int Attack_ProjectileVerticalCount = 1;
    private float Attack_MinHeight = 0.5f;
    private float Attack_MaxHeight = 0.5f;


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
        // Debugging
        //Debug.Log("BossEnemy: Entering State_Attack_Bullet_JumpRope_Easy");

        // Boss Enemy Logic
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost); // energy cost of attack applied

        // Spawner Logic
        SpawnerComponent_Bullet = bossEnemyComponent.ReturnComponent_Spawner_Bullet();
        SpawnerComponent_Bullet.ReturnAllProjectilesToPool();
        SpawnerComponent_Bullet.UpdateSpawner_AllValues(Attack_FireRate, Attack_Count, Attack_TrackHorizontal, Attack_TrackVertical, Attack_TrackSpeed);
        SpawnerComponent_Bullet.Set_All_ProjectileLifetime(Attack_ProjectileLifetime);
        SpawnerComponent_Bullet.Set_Bullet_ProjectileSpeed(Attack_ProjectileSpeed);

        SpawnerComponent_Bullet.Update_FirePointPosition(null, 0.5f, null);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Spawner Logic
        // Attack is still occuring
        if (SpawnerComponent_Bullet.ReturnSpawnerActive() == true)
        {
            ////Debug.Log("BossEnemy: Spawner Is Active");
            // Spawner is ready for next projectile fire
            if (SpawnerComponent_Bullet.IsSpawnerReadyToFire() == true)
            {
                ////Debug.Log("BossEnemy: Spawner Ready To Fire");
                SpawnerComponent_Bullet.PreAttackLogic();

                float randomDegree = Random.Range(0.0f, 360.0f);
                SpawnerComponent_Bullet.Update_FirePointRotation(SpawnerComponent_Bullet.ReturnSpawnerTransform().rotation *= Quaternion.Euler(0, randomDegree, 0));
                SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_ProjectileCount, Attack_AngleOfSpread, Attack_ProjectileVerticalCount, Attack_MinHeight, Attack_MaxHeight);

                SpawnerComponent_Bullet.PostAttackLogic();
            }
        }
        // Attack has finished
        else
        {
            //Debug.Log("BossEnemy: Attack Completed");
            Attack_Completed = true;
        }

        // Animation Logic

    }

    // Called once per frame - after update
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.HP_IsZero())                                  // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToState_Death();                     // if so, transition to Death State
        }
        else if (Attack_Completed == true)
        {
            bossEnemyComponent.TransitionToState_SelfCheck();
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
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
        animator.SetBool("inAttack", false);

    }
}

// --------------------------------------------------------------------------------------------------------------------------------------------------
// *               Arena Hazard Attacks                                                                                                             * 
// --------------------------------------------------------------------------------------------------------------------------------------------------
public class State_Attack_ArenaHazard_Mine_Random : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_ArenaHazard_Mine_Random";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 30.0f;

    // Spawner Values
    private float Attack_FireRate = 3.0f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 12;
    private bool Attack_TrackHorizontal = false;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 0.0f;
    private float Attack_ProjectileLifetime = 30.0f;
    private float Spawner_Rotation_Y = 0.0f;

    // Attack Spawner
    private ProjectileSpawner SpawnerComponent_Mine;

    // Attack Values
    // public void Spawner_Mine_TossSingle(float Toss_Distance, float Arc_Height, float Arc_Duration)
    private float Mine_MinTossDistance = 5.0f;
    private float Mine_MaxTossDistance = 23.0f;
    private float Mine_TossDistance = 5.0f;
    private float Mine_ArcHeight = 2.0f;
    private float Mine_ArcDuration = 1.0f;


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
        score += (bossEnemyComponent.returnAttackHistoryScore(Attack_Name) * 2);
        return score;
    }

    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Debugging
        //Debug.Log("BossEnemy: Entering State_Attack_ArenaHazard_Mine_Random");

        // Boss Enemy Logic
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost); // energy cost of attack applied

        // Spawner Logic
        SpawnerComponent_Mine = bossEnemyComponent.ReturnComponent_Spawner_Mine();
        SpawnerComponent_Mine.UpdateSpawner_AllValues(Attack_FireRate, Attack_Count, Attack_TrackHorizontal, Attack_TrackVertical, Attack_TrackSpeed);
        SpawnerComponent_Mine.Set_All_ProjectileLifetime(Attack_ProjectileLifetime);

        SpawnerComponent_Mine.Reset_FirePointPositionToGameObject();
        //SpawnerComponent_Mine.ReturnAllProjectilesToPool();
        SpawnerComponent_Mine.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Spawner Logic
        // Attack is still occuring
        if (SpawnerComponent_Mine.ReturnSpawnerActive() == true)
        {
            ////Debug.Log("BossEnemy: Spawner Is Active");
            // Spawner is ready for next projectile fire
            if (SpawnerComponent_Mine.IsSpawnerReadyToFire() == true)
            {
                ////Debug.Log("BossEnemy: Spawner Ready To Fire");
                SpawnerComponent_Mine.PreAttackLogic();
                Spawner_Rotation_Y = Random.Range(0f, 360f);
                SpawnerComponent_Mine.Update_FirePointRotation(null, Spawner_Rotation_Y, null);
                Mine_TossDistance = Random.Range(Mine_MinTossDistance, Mine_MaxTossDistance);
                SpawnerComponent_Mine.Spawner_Mine_TossSingle(Mine_TossDistance, Mine_ArcHeight, Mine_ArcDuration);
                SpawnerComponent_Mine.PostAttackLogic();
            }
        }
        // Attack has finished
        else
        {
            //Debug.Log("BossEnemy: Attack Completed");
            Attack_Completed = true;
        }

        // Animation Logic

    }

    // Called once per frame - after update
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.HP_IsZero())                                  // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToState_Death();                     // if so, transition to Death State
        }
        else if (Attack_Completed == true)
        {
            bossEnemyComponent.TransitionToState_SelfCheck();
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
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
        animator.SetBool("inAttack", false);

    }
}

// --------------------------------------------------------------------------------------------------------------------------------------------------
// *               Melee Attacks                                                                                                                    * 
// --------------------------------------------------------------------------------------------------------------------------------------------------
public class State_Attack_StandUpMelee : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;
    private GameObject Attack_GameObjectParent;
    private GameObject Attack_ColliderSphere;
    private Vector3 Attack_ColliderSphereScale_In = new Vector3(1.5f, 4.0f, 1.5f);
    private Vector3 Attack_ColliderSphereScale_Out = new Vector3(8.0f, 4.0f, 8.0f);
    private bool Attack_IsColliderSphereScaleOut = false;
    private float Attack_Duration = 3.0f;
    private float Attack_Delay = 1.0f;
    private float Attack_StartTimeStamp = 0.0f;
    
    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        //Debug.Log("BossEnemy: Entering State_Attack_StandUpMelee");
        // Attack Setup Logic
        Attack_GameObjectParent = new GameObject("Attack_GameObjectParent");

        // Collider Sphere
        Attack_ColliderSphere = new GameObject("Attack_ColliderSphere");
        MeshFilter Attack_ColliderSphere_meshfilter = Attack_ColliderSphere.AddComponent<MeshFilter>();
        Attack_ColliderSphere_meshfilter.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
        MeshRenderer Attack_ColliderSphere_meshRenderer = Attack_ColliderSphere.AddComponent<MeshRenderer>();
        Rigidbody rigidBody = Attack_ColliderSphere.AddComponent<Rigidbody>();
        rigidBody.useGravity = false;
        rigidBody.isKinematic = true;
        Attack_ColliderSphere.transform.position = bossEnemyComponent.returnBossEnemyPosition();
        Attack_ColliderSphere.transform.localScale = Attack_ColliderSphereScale_In;
        Attack_ColliderSphere.transform.SetParent(Attack_GameObjectParent.transform);

        // Misc.
        Attack_StartTimeStamp = Time.time;

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic
        if (Time.time - Attack_StartTimeStamp >= Attack_Duration) // check if the duration of the attack has been exceeded Attack_Duration
        {
            Attack_Completed = true; // if so, set Attack_Completed to true
        }

        // laser and laser contact
        if (Attack_IsColliderSphereScaleOut == false)              // check if collider sphere object scale has been set to use Attack_ColliderSphereScale_Out scaling
        {
            if (Time.time - Attack_StartTimeStamp >= Attack_Delay) // if so, check if the duration of the attack has been exceeded Attack_Delay
            {
                Attack_IsColliderSphereScaleOut = true;                                                                     // if so, update Attack_IsColliderSphereScaleOut to true
                SphereCollider Attack_LaserContactObject_collider = Attack_ColliderSphere.AddComponent<SphereCollider>();   // add a collider
                Attack_LaserContactObject_collider.isTrigger = true;                                                        // set collider trigger to true
                Attack_ColliderSphere.tag = "Damage Source";
                Attack_ColliderSphere.transform.localScale = Attack_ColliderSphereScale_Out;                                // set collider sphere object to use Attack_ColliderSphereScale_Out scaling
            }
        }

        // Animation Logic

    }

    // Called once per frame
    public override void CheckTransition()
    {
        // Programming Logic
        if (Attack_Completed == true)
        {
            bossEnemyComponent.TransitionToState_Awake();
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called when the state machine transitions out of this state
    public override void Exit()
    {
        // Attack Logic
        GameObject.Destroy(Attack_GameObjectParent);

        // Animation Logic
        animator.SetBool("inAttack", false);

    }
}

public class State_Attack_Melee01 : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;
    private GameObject Attack_GameObjectParent;
    private GameObject Attack_ColliderSphere;
    private Vector3 Attack_ColliderSphereScale_In = new Vector3(1.5f, 4.0f, 1.5f);
    private Vector3 Attack_ColliderSphereScale_Out = new Vector3(8.0f, 4.0f, 8.0f);
    private bool Attack_IsColliderSphereScaleOut = false;
    private float Attack_Duration = 3.0f;
    private float Attack_Delay = 1.0f;
    private float Attack_StartTimeStamp = 0.0f;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Melee01";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 0.0f;
    public static float Player_MaxDistance = 5.0f;

    public static float CalculateScore(BossEnemy bossEnemyComponent)
    {
        float score = 0.0f;

        // Check distance ----------------------------*
        if (bossEnemyComponent.Player_ReturnDistance() >= Player_MinDistance && bossEnemyComponent.Player_ReturnDistance() <= Player_MaxDistance)
        {
            score += 1.5f;
        }
        else
        {
            score -= 10.0f;
        }

        // Check Attack_HistoryList ------------------*
        score += bossEnemyComponent.returnAttackHistoryScore(Attack_Name);

        return score;
    }

    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        //Debug.Log("BossEnemy: Entering State_Attack_StandUpMelee");
        // Attack Setup Logic
        Attack_GameObjectParent = new GameObject("Attack_GameObjectParent");

        // Collider Sphere
        Attack_ColliderSphere = new GameObject("Attack_ColliderSphere");
        MeshFilter Attack_ColliderSphere_meshfilter = Attack_ColliderSphere.AddComponent<MeshFilter>();
        Attack_ColliderSphere_meshfilter.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
        MeshRenderer Attack_ColliderSphere_meshRenderer = Attack_ColliderSphere.AddComponent<MeshRenderer>();
        Rigidbody rigidBody = Attack_ColliderSphere.AddComponent<Rigidbody>();
        rigidBody.useGravity = false;
        rigidBody.isKinematic = true;
        Attack_ColliderSphere.transform.position = bossEnemyComponent.returnBossEnemyPosition();
        Attack_ColliderSphere.transform.localScale = Attack_ColliderSphereScale_In;
        Attack_ColliderSphere.transform.SetParent(Attack_GameObjectParent.transform);

        // Misc.
        Attack_StartTimeStamp = Time.time;

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic
        if (Time.time - Attack_StartTimeStamp >= Attack_Duration) // check if the duration of the attack has been exceeded Attack_Duration
        {
            Attack_Completed = true; // if so, set Attack_Completed to true
        }

        // laser and laser contact
        if (Attack_IsColliderSphereScaleOut == false)              // check if collider sphere object scale has been set to use Attack_ColliderSphereScale_Out scaling
        {
            if (Time.time - Attack_StartTimeStamp >= Attack_Delay) // if so, check if the duration of the attack has been exceeded Attack_Delay
            {
                Attack_IsColliderSphereScaleOut = true;                                                                     // if so, update Attack_IsColliderSphereScaleOut to true
                SphereCollider Attack_LaserContactObject_collider = Attack_ColliderSphere.AddComponent<SphereCollider>();   // add a collider
                Attack_LaserContactObject_collider.isTrigger = true;                                                        // set collider trigger to true
                Attack_ColliderSphere.tag = "Damage Source";
                Attack_ColliderSphere.transform.localScale = Attack_ColliderSphereScale_Out;                                // set collider sphere object to use Attack_ColliderSphereScale_Out scaling
            }
        }

        // Animation Logic

    }

    // Called once per frame
    public override void CheckTransition()
    {
        // Programming Logic
        if (Attack_Completed == true)
        {
            bossEnemyComponent.TransitionToState_SelfCheck();
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called after all other update functions
    public override void LateUpdate()
    {
        // Programming Logic

        // Animation Logic

    }

    // Called when the state machine transitions out of this state
    public override void Exit()
    {
        // Programming Logic
        bossEnemyComponent.appendToAttackHistory(Attack_Name);

        // Attack Logic
        GameObject.Destroy(Attack_GameObjectParent);

        // Animation Logic
        animator.SetBool("inAttack", false);

    }
}