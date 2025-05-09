using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.SceneManagement;

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
        
        //kill any vfx that might be happening
        fxBehave.VFX_stopPoles();

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
        m_audio.playEnemySFX(0);
        fxBehave.VFX_stopPoles();
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
        //Debug.Log("BossEnemy: Current Energy: " + bossEnemyComponent.returnCurrentEnergy());

        // Animation Logic
        animator.SetBool("inAttack", false);
        animator.SetBool("toIdle", true);
        animator.SetBool("downed", false);
        fxBehave.VFX_stopPoles();
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
        animator.SetBool("inAttack", false);
        animator.SetBool("toIdle", true);
        animator.SetBool("downed", false);
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
        animator.SetBool("exitDowned", false);

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
        animator.SetBool("downed", false);
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
        //animator.SetBool("inAttack", false);
        animator.SetBool("toIdle", true);
        animator.SetBool("exitDowned", true);
        fxBehave.eyesOnCoroutine = fxBehave.StartCoroutine(fxBehave.turnOnEyes());
        fxBehave.VFX_startPoles();
        m_audio.playEnemySFX(3);
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

        //kill any FX that slip through
        fxBehave.VFX_stopPoles();

        // FX Logic
        animator.SetBool("die", true);
        animator.SetBool("woken", false);
        animator.SetBool("toIdle", false);
        m_audio.playEnemySFX(2);
        GameObject groundFall = GameObject.Find("TermProject_Arena_floor_in");
        if (groundFall.GetComponent<AudioSource>().isPlaying == false)
        {
            groundFall.gameObject.GetComponent<AudioSource>().Play();
        }

        //turn off eyes on death
        fxBehave.eyesOffCoroutine = fxBehave.StartCoroutine(fxBehave.turnOffEyes());

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Animation Logic

        animator.SetBool("die", false);

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
    private bool attackSelected = false;

    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        //Debug.Log("BossEnemy: Entering State_Awake");
        ////Debug.Log("BossEnemy: Current Energy = " + bossEnemyComponent.returnCurrentEnergy());
        enterStateTimeStamp = Time.time;

        // INSERT: attack selection logic
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

        // Animation Logic
        animator.SetBool("inAttack", false);
        animator.SetBool("toIdle", true);
        animator.SetBool("downed", false);

        fxBehave.VFX_stopPoles();

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        // Animation Logic
        //note
        // Check for when it is one second before we enter State_Attack_Indicator
        if (attackSelected == true)
        {
            if (Time.time >= enterStateTimeStamp + bossEnemyComponent.returnStateAwakeDelay() - 1.0f)
            {
                if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_Melee01) == true)
                {
                    animator.SetBool("inAttack", false);
                    animator.SetBool("downed", false);
                    animator.SetBool("toIdle", false);
                }
                else
                {
                    animator.SetBool("downed", false);
                    animator.SetBool("toIdle", false);
                    animator.SetBool("inAttack", true);
                }
            }
        }

    }

    // Called once per frame
    public override void CheckTransition()
    {
        // Programming Logic
        if (bossEnemyComponent.HP_IsZero())                                  // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToState_Death();                     // if so, transition to Death State
        }
        else if (attackSelected == true && delayFinished == true && bossEnemyComponent.AreAllPoolsFinishedFilling() == true) // check if the delay has been completed and the projectile pools are filled
        {
            bossEnemyComponent.TransitionToState_Attack_Indicator();
        }
        else if (delayFinished == false) // check if the delay has been completed
        {
            if (Time.time - enterStateTimeStamp >= bossEnemyComponent.returnStateAwakeDelay()) // if not, check if the duration of the delay has been exceeded
            {
                delayFinished = true; // if so, set delay to have been completed
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
        //note
        //animator.SetBool("inAttack", false);
        //animator.SetBool("toIdle", true);
        //animator.SetBool("downed", false);

    }

    // ---------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Attack Selection Functions                                                                                                              * 
    // ---------------------------------------------------------------------------------------------------------------------------------------------------------
    // Define a delegate that matches the signature of a function to later call
    string Attack_BestName = null;
    float Attack_BestScore = 0.0f;
    BossEnemy.MyFunctionDelegate Attack_TransitionToExecute = null;

    // Determines which Attack_State to enter based on player information (attack_states must be manually added here)
    public void Attack_Selection_1()
    {
        // Determine Best Choice -----------------------------------------------------------------------------------*
        for (int i = 0; i < 5; i++)     // loop up to 5 times to find a suitable attack
        {
            //State_Attack_Bullet_SlowFiringShot_Easy---------------------- -
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
        //Attack_BestName = State_Attack_Bullet_JumpRope_Easy.Attack_Name;
        //Attack_BestScore = State_Attack_Bullet_JumpRope_Easy.CalculateScore(bossEnemyComponent);
        //Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_JumpRope_Easy;

        // Transition to Best Choice -------------------------------------------------------------------------------*

        if (Attack_TransitionToExecute == null) //fixes bug where every 6th attack becomes null??
        {
            Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_SlowFiringShot_Easy;
            //Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Melee01;
        }

        bossEnemyComponent.Set_Delegate(Attack_TransitionToExecute);
        attackSelected = true;
        //bossEnemyComponent.TransitionToState_Attack_Indicator();
    }

    public void Attack_Selection_2()
    {
        // Determine Best Choice -----------------------------------------------------------------------------------*
        for (int i = 0; i < 5; i++)     // loop up to 5 times to find a suitable attack
        {
            //State_Attack_Bullet_SlowFiringShot_Medium---------------------- -
            if (State_Attack_Bullet_SlowFiringShot_Medium.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = State_Attack_Bullet_SlowFiringShot_Medium.Attack_Name;
                Attack_BestScore = State_Attack_Bullet_SlowFiringShot_Medium.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_SlowFiringShot_Medium;
            }
            else if (State_Attack_Bullet_SlowFiringShot_Medium.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = State_Attack_Bullet_SlowFiringShot_Medium.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_SlowFiringShot_Medium.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_SlowFiringShot_Medium;
                }
            }

            // State_Attack_Bullet_RapidFireShot_Medium -----------------------
            if (State_Attack_Bullet_RapidFireShot_Medium.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = State_Attack_Bullet_RapidFireShot_Medium.Attack_Name;
                Attack_BestScore = State_Attack_Bullet_RapidFireShot_Medium.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RapidFireShot_Medium;
            }
            else if (State_Attack_Bullet_RapidFireShot_Medium.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = State_Attack_Bullet_RapidFireShot_Medium.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_RapidFireShot_Medium.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RapidFireShot_Medium;
                }
            }

            // State_Attack_Bullet_TrackingCone_Medium -----------------------
            if (State_Attack_Bullet_TrackingCone_Medium.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = State_Attack_Bullet_TrackingCone_Medium.Attack_Name;
                Attack_BestScore = State_Attack_Bullet_TrackingCone_Medium.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingCone_Medium;
            }
            else if (State_Attack_Bullet_TrackingCone_Medium.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = State_Attack_Bullet_TrackingCone_Medium.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_TrackingCone_Medium.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingCone_Medium;
                }
            }

            // State_Attack_Bullet_TrackingWall_Medium -----------------------
            if (State_Attack_Bullet_TrackingWall_Medium.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = State_Attack_Bullet_TrackingWall_Medium.Attack_Name;
                Attack_BestScore = State_Attack_Bullet_TrackingWall_Medium.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingWall_Medium;
            }
            else if (State_Attack_Bullet_TrackingWall_Medium.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = State_Attack_Bullet_TrackingWall_Medium.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_TrackingWall_Medium.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingWall_Medium;
                }
            }

            // State_Attack_Bullet_RotatingWall_Medium -----------------------
            if (State_Attack_Bullet_RotatingWall_Medium.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = State_Attack_Bullet_RotatingWall_Medium.Attack_Name;
                Attack_BestScore = State_Attack_Bullet_RotatingWall_Medium.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RotatingWall_Medium;
            }
            else if (State_Attack_Bullet_RotatingWall_Medium.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = State_Attack_Bullet_RotatingWall_Medium.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_RotatingWall_Medium.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_RotatingWall_Medium;
                }
            }

            // State_Attack_Bullet_JumpRope_Medium -----------------------
            if (State_Attack_Bullet_JumpRope_Medium.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = State_Attack_Bullet_JumpRope_Medium.Attack_Name;
                Attack_BestScore = State_Attack_Bullet_JumpRope_Medium.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_JumpRope_Medium;
            }
            else if (State_Attack_Bullet_JumpRope_Medium.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = State_Attack_Bullet_JumpRope_Medium.Attack_Name;
                    Attack_BestScore = State_Attack_Bullet_JumpRope_Medium.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_JumpRope_Medium;
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

            // State_Attack_ArenaHazard_Mine_Random -----------------------------------
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
        }

        // DEBUGGING (MUST BE REMOVED):
        //Attack_BestName = State_Attack_ArenaHazard_Mine_Random.Attack_Name;
        //Attack_BestScore = State_Attack_ArenaHazard_Mine_Random.CalculateScore(bossEnemyComponent);
        //Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_ArenaHazard_Mine_Random;

        // Transition to Best Choice -------------------------------------------------------------------------------*
        //Attack_TransitionToExecute();

        if (Attack_TransitionToExecute == null) //fixes bug where every 6th attack becomes null??
        {
            //Debug.Log("we hit null");
            Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_SlowFiringShot_Medium;
            //Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Melee01;
        }

        bossEnemyComponent.Set_Delegate(Attack_TransitionToExecute);
        attackSelected = true;
        //bossEnemyComponent.TransitionToState_Attack_Indicator();
    }

    public void Attack_Selection_3()
    {
        // Determine Best Choice -----------------------------------------------------------------------------------*
        for (int i = 0; i < 5; i++)     // loop up to 5 times to find a suitable attack
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

            // State_Attack_ArenaHazard_Mine_Random -----------------------------------
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
        }

        // DEBUGGING (MUST BE REMOVED):
        //Attack_BestName = State_Attack_Bullet_SlowFiringShot_Easy.Attack_Name;
        //Attack_BestScore = State_Attack_Bullet_SlowFiringShot_Easy.CalculateScore(bossEnemyComponent);
        //Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_SlowFiringShot_Easy;

        // Transition to Best Choice -------------------------------------------------------------------------------*
        //Attack_TransitionToExecute();

        if (Attack_TransitionToExecute == null) //fixes bug where every 6th attack becomes null??
        {
            //Debug.Log("we hit null");
            Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Bullet_SlowFiringShot_Hard;
            //Attack_TransitionToExecute = bossEnemyComponent.TransitionToState_Attack_Melee01;
        }

        bossEnemyComponent.Set_Delegate(Attack_TransitionToExecute);
        attackSelected = true;
        //bossEnemyComponent.TransitionToState_Attack_Indicator();
    }
}

// --------------------------------------------------------------------------------------------------------------------------------------------------
// *               Attack Indicator State                                                                                                           * 
// --------------------------------------------------------------------------------------------------------------------------------------------------
public class State_Attack_Indicator : BossState
{
    // Private Attributes
    private float Timer_StartTime;
    private float Timer_Duration = 2.25f;
    private string Attack_Name;

    private bool Spawner_FacePlayer = false;
    private float Spawner_RotateDegreesPerSecond = 0.0f;

    GameObject Indicator_Prefab;
    GameObject Indicator_GameObject;

    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        //Debug.Log("BossEnemy: Entering State_Attack_Indicator");
        bool isMelee = false;
        Timer_StartTime = Time.time;

        // State_Attack_Bullet_SlowFiringShot
        if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_Bullet_SlowFiringShot_Easy) == true)
        {
            Attack_Name = "SlowFiringShot";
            Spawner_FacePlayer = true;
            Spawner_RotateDegreesPerSecond = 0.0f;

        }
        else if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_Bullet_SlowFiringShot_Medium) == true)
        {
            Attack_Name = "SlowFiringShot";
            Spawner_FacePlayer = true;
            Spawner_RotateDegreesPerSecond = 0.0f;
        }
        else if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_Bullet_SlowFiringShot_Hard) == true)
        {
            Attack_Name = "SlowFiringShot";
            Spawner_FacePlayer = true;
            Spawner_RotateDegreesPerSecond = 0.0f;
        }

        // State_Attack_Bullet_RapidFireShot
        else if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_Bullet_RapidFireShot_Medium) == true)
        {
            Attack_Name = "RapidFireShot";
            Spawner_FacePlayer = true;
            Spawner_RotateDegreesPerSecond = 0.0f;
        }
        else if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_Bullet_RapidFireShot_Hard) == true)
        {
            Attack_Name = "RapidFireShot";
            Spawner_FacePlayer = true;
            Spawner_RotateDegreesPerSecond = 0.0f;
        }

        // State_Attack_Bullet_TrackingCone
        else if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingCone_Easy) == true)
        {
            Attack_Name = "TrackingCone";
            Spawner_FacePlayer = true;
            Spawner_RotateDegreesPerSecond = 0.0f;
        }
        else if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingCone_Medium) == true)
        {
            Attack_Name = "TrackingCone";
            Spawner_FacePlayer = true;
            Spawner_RotateDegreesPerSecond = 0.0f;
        }
        else if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingCone_Hard) == true)
        {
            Attack_Name = "TrackingCone";
            Spawner_FacePlayer = true;
            Spawner_RotateDegreesPerSecond = 0.0f;
        }

        // State_Attack_Bullet_TrackingWall
        else if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingWall_Medium) == true)
        {
            Attack_Name = "TrackingWall";
            Spawner_FacePlayer = false;
            Spawner_RotateDegreesPerSecond = 180f;
        }
        else if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_Bullet_TrackingWall_Hard) == true)
        {
            Attack_Name = "TrackingWall";
            Spawner_FacePlayer = false;
            Spawner_RotateDegreesPerSecond = 180f;
        }

        // State_Attack_Bullet_RotatingWall
        else if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_Bullet_RotatingWall_Easy) == true)
        {
            Attack_Name = "RotatingWall";
            Spawner_FacePlayer = false;
            Spawner_RotateDegreesPerSecond = 180f;
        }
        else if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_Bullet_RotatingWall_Medium) == true)
        {
            Attack_Name = "RotatingWall";
            Spawner_FacePlayer = false;
            Spawner_RotateDegreesPerSecond = 180f;
        }
        else if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_Bullet_RotatingWall_Hard) == true)
        {
            Attack_Name = "RotatingWall";
            Spawner_FacePlayer = false;
            Spawner_RotateDegreesPerSecond = 180f;
        }

        // State_Attack_Bullet_JumpRope
        else if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_Bullet_JumpRope_Easy) == true)
        {
            Attack_Name = "JumpRope";
            Spawner_FacePlayer = false;
            Spawner_RotateDegreesPerSecond = 90f;
        }
        else if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_Bullet_JumpRope_Medium) == true)
        {
            Attack_Name = "JumpRope";
            Spawner_FacePlayer = false;
            Spawner_RotateDegreesPerSecond = 90f;
        }
        else if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_Bullet_JumpRope_Hard) == true)
        {
            Attack_Name = "JumpRope";
            Spawner_FacePlayer = false;
            Spawner_RotateDegreesPerSecond = 90f;
        }

        // State_Attack_Melee
        else if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_Melee01) == true)
        {
            Attack_Name = "Melee";
            Timer_Duration = 0.0f;
            isMelee = true;
        }

        // State_Attack_ArenaHazard_Mine_Random
        else if (bossEnemyComponent.Compare_Delegate(bossEnemyComponent.TransitionToState_Attack_ArenaHazard_Mine_Random) == true)
        {
            Attack_Name = "Mine_Random";
            Timer_Duration = 0.0f;
        }

        bossEnemyComponent.Activate_AttackIndicatorByName(Attack_Name);

        //FX behavior
        if (isMelee)
        {
            animator.SetBool("melee", true);
        }
        else
        {
            //note
            //animator.SetBool("inAttack", true);
            fxBehave.VFX_startPoles();
            m_audio.playEnemySFX(3);
        }

        //fxBehave.VFX_startPoles();
        //animator.SetBool("toIdle", false);
    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic

        if (Spawner_FacePlayer == true)
        {
            bossEnemyComponent.Rotate_AttackIndicatorSpawner_FacePlayer();
        }

        if (Spawner_RotateDegreesPerSecond > 0.0f)
        {
            bossEnemyComponent.Rotate_AttackIndicatorSpawner_ByDegrees(Spawner_RotateDegreesPerSecond * Time.deltaTime);
        }

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
        else if (Time.time - Timer_StartTime >= Timer_Duration)
        {
            bossEnemyComponent.Execute_Delegate();
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
        bossEnemyComponent.Deactivate_AttackIndicatorByName(Attack_Name);

        // Animation Logic

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
        //animator.SetBool("inAttack", true);

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
        animator.SetBool("toIdle", true);
        fxBehave.VFX_stopPoles();

    }
}

public class State_Attack_Bullet_SlowFiringShot_Medium : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_SlowFiringShot_Medium";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 50.0f;

    // Spawner Values
    private float Attack_FireRate = 1.25f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 10;
    private bool Attack_TrackHorizontal = false;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 0.0f;
    private float Attack_ProjectileSpeed = 45.0f;
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
        //Debug.Log("BossEnemy: Entering State_Attack_Bullet_SlowFiringShot_Medium");

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
        //animator.SetBool("inAttack", true);

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
        animator.SetBool("toIdle", true);
        fxBehave.VFX_stopPoles();
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
        //animator.SetBool("inAttack", true);

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
        animator.SetBool("toIdle", true);
        fxBehave.VFX_stopPoles();
    }
}

public class State_Attack_Bullet_RapidFireShot_Medium : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_RapidFireShot_Medium";
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
        //Debug.Log("BossEnemy: Entering State_Attack_Bullet_RapidFireShot_Medium");

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
        //animator.SetBool("inAttack", true);

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
                //public void InitializeCluster(string clusterName, Vector3 New_StartingPosition, Quaternion New_Rotation, Vector3 New_Direction, float New_Speed, float New_Lifetime)
                Vector3 direction = SpawnerComponent_Bullet.Return_FirePointRotation() * Vector3.forward;
                bossEnemyComponent.InitializeCluster("Projectile_Cluster_3x3", SpawnerComponent_Bullet.Return_FirePointPosition(), SpawnerComponent_Bullet.Return_FirePointRotation(), direction, Attack_ProjectileSpeed, Attack_ProjectileLifetime);

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
        animator.SetBool("toIdle", true);
        fxBehave.VFX_stopPoles();
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
        //animator.SetBool("inAttack", true);

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

                Vector3 direction = SpawnerComponent_Bullet.Return_FirePointRotation() * Vector3.forward;
                bossEnemyComponent.InitializeCluster("Projectile_Cluster_3x3", SpawnerComponent_Bullet.Return_FirePointPosition(), SpawnerComponent_Bullet.Return_FirePointRotation(), direction, Attack_ProjectileSpeed, Attack_ProjectileLifetime);

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
        animator.SetBool("toIdle", true);
        fxBehave.VFX_stopPoles();
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
    private int Attack_ProjectileCount = 8;
    private int Attack_ProjectileCount_ALT = 9;
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
        SpawnerComponent_Bullet.Update_FirePointRotation(0.0f, 0.0f, 0.0f);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        //animator.SetBool("inAttack", true);

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
                    float randomRotation = 0.0f;
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
        animator.SetBool("toIdle", true);
        fxBehave.VFX_stopPoles();
    }
}

public class State_Attack_Bullet_TrackingCone_Medium : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_TrackingCone_Medium";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 50.0f;

    // Spawner Values
    private float Attack_FireRate = 0.75f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 8;
    private bool Attack_TrackHorizontal = true;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 120.0f;
    private float Attack_ProjectileSpeed = 15.0f;
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
        //Debug.Log("BossEnemy: Entering State_Attack_Bullet_TrackingCone_Medium");

        // Boss Enemy Logic
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost); // energy cost of attack applied

        // Spawner Logic
        SpawnerComponent_Bullet = bossEnemyComponent.ReturnComponent_Spawner_Bullet();
        SpawnerComponent_Bullet.ReturnAllProjectilesToPool();
        SpawnerComponent_Bullet.UpdateSpawner_AllValues(Attack_FireRate, Attack_Count, Attack_TrackHorizontal, Attack_TrackVertical, Attack_TrackSpeed);
        SpawnerComponent_Bullet.Set_All_ProjectileLifetime(Attack_ProjectileLifetime);
        SpawnerComponent_Bullet.Set_Bullet_ProjectileSpeed(Attack_ProjectileSpeed);

        SpawnerComponent_Bullet.Update_FirePointPosition(null, 0.5f, null);
        SpawnerComponent_Bullet.Update_FirePointRotation(0.0f, 0.0f, 0.0f);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        //animator.SetBool("inAttack", true);

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
                    float randomRotation = 0.0f;
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
        animator.SetBool("toIdle", true);
        fxBehave.VFX_stopPoles();
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
    private int Attack_Count = 10;
    private bool Attack_TrackHorizontal = true;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 160.0f;
    private float Attack_ProjectileSpeed = 15.0f;
    private float Attack_ProjectileLifetime = 10.0f;

    // Attack Spawner
    private ProjectileSpawner SpawnerComponent_Bullet;

    // Attack Values
    // Spawner_Bullet_StackedConeShot(int Projectile_Count, float AngleOfSpread, int Projectile_VerticalCount, float Spawner_MinHeight, float Spawner_MaxHeight)
    private int Attack_ProjectileCount = 10;
    private int Attack_ProjectileCount_ALT = 11;
    private float Attack_AngleOfSpread = 65.0f;
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
        SpawnerComponent_Bullet.Update_FirePointRotation(0.0f, 0.0f, 0.0f);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        //animator.SetBool("inAttack", true);

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
                    float randomRotation = 0.0f;
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
        animator.SetBool("toIdle", true);
        fxBehave.VFX_stopPoles();
    }
}

public class State_Attack_Bullet_TrackingWall_Medium : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_TrackingWall_Medium";
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
    private float Attack_ProjectileLifetime = 7.0f;

    // Attack Spawner
    private ProjectileSpawner SpawnerComponent_Bullet;

    // Attack Values
    // Spawner_Bullet_StackedConeShot(int Projectile_Count, float AngleOfSpread, int Projectile_VerticalCount, float Spawner_MinHeight, float Spawner_MaxHeight)
    private float Attack_JumpRope_HeightOffset = 1.25f;
    private float Spawner_JumpRope_Rotation_Speed = 120.0f;
    private Quaternion Spawner_JumpRope_RotationQuat;

    //private int Attack_Wall_ProjectileCount = 1;
    //private float Attack_Wall_AngleOfSpread = 1.0f;
    //private int Attack_Wall_ProjectileVerticalCount = 8;
    //private float Attack_Wall_MinHeight = 0.0f;
    //private float Attack_Wall_MaxHeight = 7.5f;

    //private GameObject Wall_Prefab;
    //private GameObject Wall_GameObject;
    private Vector3 Wall_Position;
    private Quaternion Wall_Rotation;
    private float Wall_RotationSpeed = 20.0f;
    private bool Wall_RotatesRight = true;

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
        //Debug.Log("BossEnemy: Entering State_Attack_Bullet_TrackingWall_Medium");

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
        SpawnerComponent_Bullet.Update_FirePointRotation(0.0f, 0.0f, 0.0f);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Wall Setup
        //Wall_Prefab = bossEnemyComponent.FindClusterProjectileByName("Projectile_Wall");

        // Get the fire point position
        Wall_Position = SpawnerComponent_Bullet.Return_FirePointPosition();

        Vector3 playerPos = bossEnemyComponent.Player_ReturnPlayerPosition();
        SpawnerComponent_Bullet.Update_FirePointRotation_FaceTarget(playerPos, 0.0f, 0.0f, true, false);
        float Wall_Rotation_Offset = 0.0f;
        bool randomLean = Random.Range(0, 2) == 0;
        if (randomLean)
        {
            Wall_Rotation_Offset = 30.0f;
            Wall_RotatesRight = false;
        }
        else
        {
            Wall_Rotation_Offset = -30.0f;
            Wall_RotatesRight = true;
        }

        // Get the fire point rotation
        Quaternion baseRotation = SpawnerComponent_Bullet.Return_FirePointRotation();

        // Apply the Y-axis offset
        Wall_Rotation = baseRotation * Quaternion.Euler(0, Wall_Rotation_Offset, 0);


        //Wall_GameObject = bossEnemyComponent.InitializeGameObject(Wall_Prefab, Wall_Position, Wall_Rotation);

        // Animation Logic
        //animator.SetBool("inAttack", true);

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
                //public void InitializeCluster(string clusterName, Vector3 New_StartingPosition, Quaternion New_Rotation, Vector3 New_Direction, float New_Speed, float New_Lifetime)
                Vector3 direction = SpawnerComponent_Bullet.Return_FirePointRotation() * Vector3.forward;
                Vector3 firingPosition = SpawnerComponent_Bullet.Return_FirePointPosition();
                firingPosition.y += Attack_JumpRope_HeightOffset;
                bossEnemyComponent.InitializeCluster("Projectile_Cluster_3x3", firingPosition, SpawnerComponent_Bullet.Return_FirePointRotation(), direction, Attack_ProjectileSpeed, Attack_ProjectileLifetime);

                // Tracking Wall OLD
                //Spawner_JumpRope_RotationQuat = SpawnerComponent_Bullet.ReturnSpawnerTransform().rotation;
                //Vector3 playerPos = bossEnemyComponent.Player_ReturnPlayerPosition();
                //SpawnerComponent_Bullet.Update_FirePointRotation_FaceTarget(playerPos, 0.0f, 0.0f, true, false);
                //SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_Wall_ProjectileCount, Attack_Wall_AngleOfSpread, Attack_Wall_ProjectileVerticalCount, Attack_Wall_MinHeight, Attack_Wall_MaxHeight);
                //SpawnerComponent_Bullet.Update_FirePointRotation(Spawner_JumpRope_RotationQuat);

                SpawnerComponent_Bullet.PostAttackLogic();
            }

            // Tracking Wall NEW
            // Get the current rotation of the wall
            //Quaternion currentRotation = Wall_GameObject.transform.rotation;

            // Get the rotation direction based on Wall_RotatesRight
            float rotationDirection = Wall_RotatesRight ? 1.0f : -1.0f;

            // Calculate the target rotation (the current rotation + rotation per second)
            //float targetYRotation = currentRotation.eulerAngles.y + (rotationDirection * Wall_RotationSpeed * Time.deltaTime);

            // Ensure the Y rotation stays within the range of 0 to 360 degrees
            //targetYRotation = targetYRotation % 360f; // Keeps the targetYRotation between 0 and 360

            // Create a target Quaternion with the updated Y rotation
            //Quaternion targetRotation = Quaternion.Euler(0f, targetYRotation, 0f);

            // Smoothly rotate towards the target rotation
            //Wall_GameObject.transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, Wall_RotationSpeed * Time.deltaTime);
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
        //bossEnemyComponent.DeleteGameObject(Wall_GameObject);

        // Animation Logic
        animator.SetBool("inAttack", false);
        animator.SetBool("toIdle", true);
        fxBehave.VFX_stopPoles();
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
    private float Attack_ProjectileLifetime = 6.0f;

    // Attack Spawner
    private ProjectileSpawner SpawnerComponent_Bullet;

    // Attack Values
    // Spawner_Bullet_StackedConeShot(int Projectile_Count, float AngleOfSpread, int Projectile_VerticalCount, float Spawner_MinHeight, float Spawner_MaxHeight)
    private float Attack_JumpRope_HeightOffset = 1.25f;
    private float Spawner_JumpRope_Rotation_Speed = 210.0f;
    private Quaternion Spawner_JumpRope_RotationQuat;

    //private int Attack_Wall_ProjectileCount = 1;
    //private float Attack_Wall_AngleOfSpread = 1.0f;
    //private int Attack_Wall_ProjectileVerticalCount = 8;
    //private float Attack_Wall_MinHeight = 0.0f;
    //private float Attack_Wall_MaxHeight = 7.5f;
    //private bool Attack_Wall_Fire = false;

    //private GameObject Wall_Prefab;
    //private GameObject Wall_GameObject;
    private Vector3 Wall_Position;
    private Quaternion Wall_Rotation;
    private float Wall_RotationSpeed = 20.0f;
    private bool Wall_RotatesRight = true;

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
        SpawnerComponent_Bullet.Update_FirePointRotation(0.0f, 0.0f, 0.0f);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Wall Setup
        //Wall_Prefab = bossEnemyComponent.FindClusterProjectileByName("Projectile_Wall");

        // Get the fire point position
        Wall_Position = SpawnerComponent_Bullet.Return_FirePointPosition();

        Vector3 playerPos = bossEnemyComponent.Player_ReturnPlayerPosition();
        SpawnerComponent_Bullet.Update_FirePointRotation_FaceTarget(playerPos, 0.0f, 0.0f, true, false);
        float Wall_Rotation_Offset = 0.0f;
        bool randomLean = Random.Range(0, 2) == 0;
        if (randomLean)
        {
            Wall_Rotation_Offset = 30.0f;
            Wall_RotatesRight = false;
        }
        else
        {
            Wall_Rotation_Offset = -30.0f;
            Wall_RotatesRight = true;
        }

        // Get the fire point rotation
        Quaternion baseRotation = SpawnerComponent_Bullet.Return_FirePointRotation();

        // Apply the Y-axis offset
        Wall_Rotation = baseRotation * Quaternion.Euler(0, Wall_Rotation_Offset, 0);


       // Wall_GameObject = bossEnemyComponent.InitializeGameObject(Wall_Prefab, Wall_Position, Wall_Rotation);

        // Animation Logic
        //animator.SetBool("inAttack", true);

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
                //public void InitializeCluster(string clusterName, Vector3 New_StartingPosition, Quaternion New_Rotation, Vector3 New_Direction, float New_Speed, float New_Lifetime)
                Vector3 direction = SpawnerComponent_Bullet.Return_FirePointRotation() * Vector3.forward;
                Vector3 firingPosition = SpawnerComponent_Bullet.Return_FirePointPosition();
                firingPosition.y += Attack_JumpRope_HeightOffset;
                bossEnemyComponent.InitializeCluster("Projectile_Cluster_3x3", firingPosition, SpawnerComponent_Bullet.Return_FirePointRotation(), direction, Attack_ProjectileSpeed, Attack_ProjectileLifetime);

                // Jump Rope ALT
                float randomDegree = Random.Range(0.0f, 360.0f);
                SpawnerComponent_Bullet.Update_FirePointRotation(SpawnerComponent_Bullet.ReturnSpawnerTransform().rotation *= Quaternion.Euler(0, randomDegree, 0));
                //public void InitializeCluster(string clusterName, Vector3 New_StartingPosition, Quaternion New_Rotation, Vector3 New_Direction, float New_Speed, float New_Lifetime)
                Vector3 directionALT = SpawnerComponent_Bullet.Return_FirePointRotation() * Vector3.forward;
                Vector3 firingPositionALT = SpawnerComponent_Bullet.Return_FirePointPosition();
                firingPositionALT.y += Attack_JumpRope_HeightOffset;
                bossEnemyComponent.InitializeCluster("Projectile_Cluster_3x3", firingPositionALT, SpawnerComponent_Bullet.Return_FirePointRotation(), directionALT, Attack_ProjectileSpeed, Attack_ProjectileLifetime);
                SpawnerComponent_Bullet.Update_FirePointRotation(SpawnerComponent_Bullet.ReturnSpawnerTransform().rotation *= Quaternion.Euler(0, -randomDegree, 0));

                // Tracking Wall OLD
                //if (Attack_Wall_Fire == true)
                //{
                //    Spawner_JumpRope_RotationQuat = SpawnerComponent_Bullet.ReturnSpawnerTransform().rotation;
                //    Vector3 playerPos = bossEnemyComponent.Player_ReturnPlayerPosition();
                //    SpawnerComponent_Bullet.Update_FirePointRotation_FaceTarget(playerPos, 0.0f, 0.0f, true, false);
                //    SpawnerComponent_Bullet.Spawner_Bullet_StackedConeShot(Attack_Wall_ProjectileCount, Attack_Wall_AngleOfSpread, Attack_Wall_ProjectileVerticalCount, Attack_Wall_MinHeight, Attack_Wall_MaxHeight);
                //    SpawnerComponent_Bullet.Update_FirePointRotation(Spawner_JumpRope_RotationQuat);
                //    Attack_Wall_Fire = false;
                //}
                //else
                //{
                //    Attack_Wall_Fire = true;
                //}

                SpawnerComponent_Bullet.PostAttackLogic();
            }

            // Tracking Wall NEW
            // Get the current rotation of the wall
            //Quaternion currentRotation = Wall_GameObject.transform.rotation;

            // Get the rotation direction based on Wall_RotatesRight
            float rotationDirection = Wall_RotatesRight ? 1.0f : -1.0f;

            // Calculate the target rotation (the current rotation + rotation per second)
            //float targetYRotation = currentRotation.eulerAngles.y + (rotationDirection * Wall_RotationSpeed * Time.deltaTime);

            // Ensure the Y rotation stays within the range of 0 to 360 degrees
            //targetYRotation = targetYRotation % 360f; // Keeps the targetYRotation between 0 and 360

            // Create a target Quaternion with the updated Y rotation
            //Quaternion targetRotation = Quaternion.Euler(0f, targetYRotation, 0f);

            // Smoothly rotate towards the target rotation
            //Wall_GameObject.transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, Wall_RotationSpeed * Time.deltaTime);
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
        //bossEnemyComponent.DeleteGameObject(Wall_GameObject);

        // Animation Logic
        animator.SetBool("inAttack", false);
        animator.SetBool("toIdle", true);
        fxBehave.VFX_stopPoles();
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
    private float Attack_FireRate = 6.0f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 80;
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
        SpawnerComponent_Bullet.Update_FirePointRotation(0.0f, randomRotation, 0.0f);
        SpawnerComponent_Bullet.Update_FirePointPosition(null, 0.5f, null);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        //animator.SetBool("inAttack", true);

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
        animator.SetBool("toIdle", true);
        fxBehave.VFX_stopPoles();
    }
}

public class State_Attack_Bullet_RotatingWall_Medium : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_RotatingWall_Medium";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 50.0f;

    // Spawner Values
    private float Attack_FireRate = 10.0f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 160;
    private bool Attack_TrackHorizontal = false;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 0.0f;
    private float Attack_ProjectileSpeed = 22.5f;
    private float Attack_ProjectileLifetime = 10.0f;
    private float Spawner_Rotation_Speed = 80.0f;

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
        //Debug.Log("BossEnemy: Entering State_Attack_Bullet_RotatingWall_Medium");

        // Boss Enemy Logic
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost); // energy cost of attack applied

        // Spawner Logic
        SpawnerComponent_Bullet = bossEnemyComponent.ReturnComponent_Spawner_Bullet();
        SpawnerComponent_Bullet.ReturnAllProjectilesToPool();
        SpawnerComponent_Bullet.UpdateSpawner_AllValues(Attack_FireRate, Attack_Count, Attack_TrackHorizontal, Attack_TrackVertical, Attack_TrackSpeed);
        SpawnerComponent_Bullet.Set_All_ProjectileLifetime(Attack_ProjectileLifetime);
        SpawnerComponent_Bullet.Set_Bullet_ProjectileSpeed(Attack_ProjectileSpeed);

        float randomRotation = Random.Range(0.0f, 360.0f);
        SpawnerComponent_Bullet.Update_FirePointRotation(0.0f, randomRotation, 0.0f);
        SpawnerComponent_Bullet.Update_FirePointPosition(null, 0.5f, null);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        //animator.SetBool("inAttack", true);

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
        animator.SetBool("toIdle", true);
        fxBehave.VFX_stopPoles();
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
    private float Attack_FireRate = 14.0f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 220;
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
        SpawnerComponent_Bullet.Update_FirePointRotation(0.0f, randomRotation, 0.0f);
        SpawnerComponent_Bullet.Update_FirePointPosition(null, 0.5f, null);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        //animator.SetBool("inAttack", true);

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
        animator.SetBool("toIdle", true);
        fxBehave.VFX_stopPoles();
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
    private int Attack_ProjectileCount = 50;
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

        SpawnerComponent_Bullet.Update_FirePointPosition(null, 0.3f, null);
        SpawnerComponent_Bullet.Update_FirePointRotation(0.0f, 0.0f, 0.0f);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        //animator.SetBool("inAttack", true);

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
        animator.SetBool("toIdle", true);
        fxBehave.VFX_stopPoles();
    }
}

public class State_Attack_Bullet_JumpRope_Medium : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_JumpRope_Medium";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 50.0f;

    // Spawner Values
    private float Attack_FireRate = 1.0f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 14;
    private bool Attack_TrackHorizontal = false;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 0.0f;
    private float Attack_ProjectileSpeed = 16.0f;
    private float Attack_ProjectileLifetime = 10.0f;

    // Attack Spawner
    private ProjectileSpawner SpawnerComponent_Bullet;

    // Attack Values
    // Spawner_Bullet_StackedConeShot(int Projectile_Count, float AngleOfSpread, int Projectile_VerticalCount, float Spawner_MinHeight, float Spawner_MaxHeight)
    private int Attack_ProjectileCount = 70;
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
        //Debug.Log("BossEnemy: Entering State_Attack_Bullet_JumpRope_Medium");

        // Boss Enemy Logic
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost); // energy cost of attack applied

        // Spawner Logic
        SpawnerComponent_Bullet = bossEnemyComponent.ReturnComponent_Spawner_Bullet();
        SpawnerComponent_Bullet.ReturnAllProjectilesToPool();
        SpawnerComponent_Bullet.UpdateSpawner_AllValues(Attack_FireRate, Attack_Count, Attack_TrackHorizontal, Attack_TrackVertical, Attack_TrackSpeed);
        SpawnerComponent_Bullet.Set_All_ProjectileLifetime(Attack_ProjectileLifetime);
        SpawnerComponent_Bullet.Set_Bullet_ProjectileSpeed(Attack_ProjectileSpeed);

        SpawnerComponent_Bullet.Update_FirePointPosition(null, 0.3f, null);
        SpawnerComponent_Bullet.Update_FirePointRotation(0.0f, 0.0f, 0.0f);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        //animator.SetBool("inAttack", true);

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
        animator.SetBool("toIdle", true);
        fxBehave.VFX_stopPoles();
    }
}

public class State_Attack_Bullet_JumpRope_Hard : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Bullet_JumpRope_Hard";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 50.0f;

    // Spawner Values
    private float Attack_FireRate = 1.75f;
    private float Attack_FireRateDelay = 1f;
    private int Attack_Count = 22;
    private bool Attack_TrackHorizontal = false;
    private bool Attack_TrackVertical = false;
    private float Attack_TrackSpeed = 0.0f;
    private float Attack_ProjectileSpeed = 24.0f;
    private float Attack_ProjectileLifetime = 22.0f;

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

        SpawnerComponent_Bullet.Update_FirePointPosition(null, 0.3f, null);
        SpawnerComponent_Bullet.Update_FirePointRotation(0.0f, 0.0f, 0.0f);
        SpawnerComponent_Bullet.StartAttack(Attack_FireRateDelay);

        // Animation Logic
        //animator.SetBool("inAttack", true);

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
        animator.SetBool("toIdle", true);
        fxBehave.VFX_stopPoles();
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
        animator.SetBool("toIdle", true);
        fxBehave.VFX_stopPoles();
    }
}

// --------------------------------------------------------------------------------------------------------------------------------------------------
// *               Melee Attacks                                                                                                                    * 
// --------------------------------------------------------------------------------------------------------------------------------------------------
public class State_Attack_StandUpMelee : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;
    //private GameObject Attack_GameObjectParent; //scrapped
    private GameObject Attack_ColliderSphere; //use find
    //private Vector3 Attack_ColliderSphereScale_In = new Vector3(1.5f, 3.0f, 1.5f); //scrapped
    //private Vector3 Attack_ColliderSphereScale_Out = new Vector3(6.0f, 4.0f, 6.0f); //scrapped
    //private bool Attack_IsColliderSphereScaleOut = false; //scrapped
    private float Attack_Duration = 3.0f;
    private float Attack_Delay = 1.0f;
    private float Attack_StartTimeStamp = 0.0f;
    private ParticleSystem VFX_standUp;
    private GameObject player;
    private Vector3 colliderTargetSize = new Vector3(15.0f, 15.0f, 15.0f);
    private Vector3 originalSize;
    private void findPlayer()
    {
        player = GameObject.FindWithTag("Player");
    }
    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Collider Sphere
        Attack_ColliderSphere = GameObject.Find("attackCollider_sphere");
        originalSize = Attack_ColliderSphere.transform.localScale;
        Debug.Log(originalSize);

        Attack_StartTimeStamp = Time.time;
        findPlayer();

        // FX logic
        VFX_standUp = GameObject.Find("VFX_standUp").GetComponent<ParticleSystem>();
        VFX_standUp.Stop();

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic
        if (Time.time - Attack_StartTimeStamp >= Attack_Duration) // check if the duration of the attack has been exceeded Attack_Duration
        {
            Attack_Completed = true;
            //reset collider size
            Attack_ColliderSphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            //stop vfx
            VFX_standUp.Stop();
        }

        
        if (Attack_Completed == false)
        {
 
            if (Time.time - Attack_StartTimeStamp >= Attack_Delay) // if so, check if the duration of the attack has been exceeded Attack_Delay
            {
                //update timer
                float elapsedTime = (Time.time - Attack_StartTimeStamp - Attack_Delay) / 0.5f;
                elapsedTime = Mathf.Clamp01(elapsedTime);

                //lerp collider up until it hits 15 in scale
                Vector3 newScale = Vector3.Lerp(originalSize, colliderTargetSize, elapsedTime);
                Attack_ColliderSphere.transform.localScale = newScale;
                Debug.Log(newScale);
                //play vfx
                VFX_standUp.Play();
                //haptics
                if (player != null && player.GetComponent<PlayerController>().collision == true)
                {
                    player.GetComponent<player_fx_behaviors>().Rumble(0.5f, 0.5f, 0.3f);
                    //ginette
                }
            }
        }

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
        else if (Attack_Completed == true)
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
        //GameObject.Destroy(Attack_GameObjectParent);

        // Animation Logic
        //animator.SetBool("inAttack", false);
        animator.SetBool("toIdle", true);
        fxBehave.VFX_stopPoles();

    }
}

public class State_Attack_Melee01 : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;
    //private GameObject Attack_GameObjectParent;
    private GameObject Attack_ColliderCube;
    private Vector3 originalSize;
    private Vector3 originalPosition;
    private Vector3 colliderTargetSize = new Vector3(18.0f, 0, 18.0f);
    private float Attack_Duration = 3.0f;
    private float Attack_Delay01 = 1.6f; //originally 2.0 --> it seems first melee is faster than all other melees???
    private float Attack_Delay02 = 1.8f; //this one's ok actually, was 1.8
    private float Attack_Delay03 = 1.8f; //this one is impossible to sync idk,, when in doubt reuse medium lol
    private float Attack_StartTimeStamp = 0.0f;
    //private float Attack_Delay = 1.6f;
    // Attack_State Selection Properties
    public static string Attack_Name = "State_Attack_Melee01";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 0.0f;
    public static float Player_MaxDistance = 5.0f;

    private GameObject player;
    
    //Particle system
    public ParticleSystem blast_outer;
    public ParticleSystem blast_inner;
    public ParticleSystem debris;
    public ParticleSystem hand_impact;

    private bool sfxPlayed = false;

    public void playMeleeVFX()
    {
        //Slam_rings.Play();
        blast_outer.Play();
        blast_inner.Play();
        debris.Play();
        hand_impact.Play();
    } 
    
    public void stopMeleeVFX()
    {
        //Slam_rings.Stop();
        blast_outer.Stop();
        blast_inner.Stop();
        debris.Stop();
        hand_impact.Stop();
    }

    private void findPlayer()
    {
        player = GameObject.FindWithTag("Player");
    }
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
        //get collider object and save it's scale and position values
        Attack_ColliderCube = GameObject.Find("attackCollider_cube");
        originalSize = Attack_ColliderCube.transform.localScale;
        originalPosition = Attack_ColliderCube.transform.localPosition;

        // Misc.
        Attack_StartTimeStamp = Time.time;

        // Animation Logic
        animator.SetBool("melee", true);
        
        //particle system
        //Slam_rings = GameObject.Find("double_ring").GetComponent<ParticleSystem>();
        blast_outer = GameObject.Find("blast_outer").GetComponent<ParticleSystem>();
        blast_inner = GameObject.Find("blast_inner").GetComponent<ParticleSystem>();
        debris = GameObject.Find("debris").GetComponent<ParticleSystem>();
        hand_impact = GameObject.Find("slamHandImpact").GetComponent<ParticleSystem>();

        findPlayer();
        stopMeleeVFX();
        fxBehave.VFX_stopPoles(); //safe guard

    }

    // Called once per frame
    public override void Update()
    {
        
        // Programming Logic
        if (Time.time - Attack_StartTimeStamp >= Attack_Duration) // check if the duration of the attack has been exceeded Attack_Duration
        {
            Attack_Completed = true; // if so, set Attack_Completed to true
            stopMeleeVFX(); //stop melee vfx

            //reset collider values
            Attack_ColliderCube.transform.localScale = originalSize;
            Attack_ColliderCube.transform.localPosition = originalPosition;            
        }

        if (Attack_Completed == false)
        {
            //play melee vfx,, each iteration needs unique delay value bc each iteration changes
            if (SceneManager.GetActiveScene().name == "Combat1" && Time.time - Attack_StartTimeStamp >= Attack_Delay01)
            {
                //move collider where it can be reached
                Attack_ColliderCube.transform.localPosition = new Vector3(originalPosition.x, 1.0f, originalPosition.z);

                //update timer
                float elapsedTime = (Time.time - Attack_StartTimeStamp - Attack_Delay01) / 0.8f;
                elapsedTime = Mathf.Clamp01(elapsedTime);

                //lerp collider up until it hits the target scale
                Vector3 newScale = Vector3.Lerp(originalSize, colliderTargetSize, elapsedTime);
                Attack_ColliderCube.transform.localScale = newScale;
                Debug.Log(newScale);

                playMeleeVFX();
                if(sfxPlayed == false)
                {
                    m_audio.playEnemySFX(5);
                    sfxPlayed = true;
                }
                

                if (player != null && player.GetComponent<PlayerController>().collision == true)
                {
                    player.GetComponent<player_fx_behaviors>().Rumble(0.5f, 0.5f, 0.2f);
                    //ginette
                }
            }
            else if (SceneManager.GetActiveScene().name == "Combat2" && Time.time - Attack_StartTimeStamp >= Attack_Delay01)
            {
                //move collider where it can be reached
                Attack_ColliderCube.transform.localPosition = new Vector3(originalPosition.x, 1.0f, originalPosition.z);

                //update timer
                float elapsedTime = (Time.time - Attack_StartTimeStamp - Attack_Delay01) / 0.8f;
                elapsedTime = Mathf.Clamp01(elapsedTime);

                //lerp collider up until it hits the target scale
                Vector3 newScale = Vector3.Lerp(originalSize, colliderTargetSize, elapsedTime);
                Attack_ColliderCube.transform.localScale = newScale;
                Debug.Log(newScale);

                playMeleeVFX();
                if (sfxPlayed == false)
                {
                    m_audio.playEnemySFX(5);
                    sfxPlayed = true;
                }

                if (player != null && player.GetComponent<PlayerController>().collision == true)
                {
                    player.GetComponent<player_fx_behaviors>().Rumble(0.5f, 0.5f, 0.2f);
                    //ginette
                }
            }
            else if (SceneManager.GetActiveScene().name == "Combat3" && Time.time - Attack_StartTimeStamp >= Attack_Delay02)
            {
                //move collider where it can be reached
                Attack_ColliderCube.transform.localPosition = new Vector3(originalPosition.x, 1.0f, originalPosition.z);

                //update timer
                float elapsedTime = (Time.time - Attack_StartTimeStamp - Attack_Delay01) / 0.8f;
                elapsedTime = Mathf.Clamp01(elapsedTime);

                //lerp collider up until it hits the target scale
                Vector3 newScale = Vector3.Lerp(originalSize, colliderTargetSize, elapsedTime);
                Attack_ColliderCube.transform.localScale = newScale;
                Debug.Log(newScale);

                playMeleeVFX();
                if (sfxPlayed == false)
                {
                    m_audio.playEnemySFX(5);
                    sfxPlayed = true;
                }

                if (player != null && player.GetComponent<PlayerController>().collision == true)
                {
                    player.GetComponent<player_fx_behaviors>().Rumble(0.5f, 0.5f, 0.2f);
                    //ginette
                }
            }

            //if (player != null && player.GetComponent<PlayerController>().collision == true)
            //{
            //    player.GetComponent<player_fx_behaviors>().Rumble(0.5f, 0.5f, 0.3f);
            //    //ginette
            //}

        }

    }

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
        animator.SetBool("melee", false);

    }
}