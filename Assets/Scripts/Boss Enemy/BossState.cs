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

    // LateUpdate is called after all other update functions
    public abstract void LateUpdate();

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
            //animator.SetBool("woken", true);
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
public class WakingUpState : BossState
{
    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        Debug.Log("BossEnemy: Entering WakingUpState");

        // Animation Logic
        animator.SetBool("woken", true);

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
public class SelfCheckState : BossState
{
    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        Debug.Log("BossEnemy: Entering SelfCheckState");

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

        if (bossEnemyComponent.HP_ReturnCurrent() <= 0) // check if HP is less than or equal to 0
        {
            bossEnemyComponent.TransitionToDeathState();     // if so, transition to death state
        }
        else if (bossEnemyComponent.returnCurrentEnergy() <= 0)   // check if the current energy count of the Boss Enemy is below or equal to 0
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
public class AwakeState : BossState
{
    // Attack_State Selection Properties
    private bool delayFinished = false;
    private float enterStateTimeStamp = 0.0f;

    // Called when the state machine transitions to this state
    public override void Enter()
    {
        // Programming Logic
        Debug.Log("BossEnemy: Entering AwakeState");
        Debug.Log("BossEnemy: Current Energy = " + bossEnemyComponent.returnCurrentEnergy());
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
        if (delayFinished == false) // check if the delay has been completed
        {
            if (Time.time - enterStateTimeStamp >= bossEnemyComponent.AwakeState_Delay) // if not, check if the duration of the delay has been exceeded
            {
                delayFinished = true; // if so, set delay to have been completed
            }
        }

        if (bossEnemyComponent.HP_ReturnCurrent() <= 0) // check if HP is less than or equal to 0
        {
            bossEnemyComponent.TransitionToDeathState();     // if so, transition to death state
        }
        else if (delayFinished == true)  // check if the delay has been completed and the attack has been chosen
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
            //if (Attack_TestingState.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            //{
            //    Attack_BestName = Attack_TestingState.Attack_Name;
            //    Attack_BestScore = Attack_TestingState.CalculateScore(bossEnemyComponent);
            //    Attack_TransitionToExecute = bossEnemyComponent.TransitionToAttack_TestingState;
            //}
            //else if (Attack_TestingState.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            //{
            //    if (Random.Range(0, 2) == 0)
            //    {
            //        Attack_BestName = Attack_TestingState.Attack_Name;
            //        Attack_BestScore = Attack_TestingState.CalculateScore(bossEnemyComponent);
            //        Attack_TransitionToExecute = bossEnemyComponent.TransitionToAttack_TestingState;
            //    }
            //}
            // Attack_SeekingProjectile01State -----------
            if (Attack_SeekingProjectile01State.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = Attack_SeekingProjectile01State.Attack_Name;
                Attack_BestScore = Attack_SeekingProjectile01State.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToAttack_SeekingProjectile01State;
            }
            else if (Attack_SeekingProjectile01State.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = Attack_SeekingProjectile01State.Attack_Name;
                    Attack_BestScore = Attack_SeekingProjectile01State.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToAttack_SeekingProjectile01State;
                }
            }
            // Attack_SeekingProjectile02State -----------
            if (Attack_SeekingProjectile02State.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = Attack_SeekingProjectile02State.Attack_Name;
                Attack_BestScore = Attack_SeekingProjectile02State.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToAttack_SeekingProjectile02State;
            }
            else if (Attack_SeekingProjectile02State.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 2) == 0)
                {
                    Attack_BestName = Attack_SeekingProjectile02State.Attack_Name;
                    Attack_BestScore = Attack_SeekingProjectile02State.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToAttack_SeekingProjectile02State;
                }
            }
            // Attack_SeekingProjectile03State -----------
            if (Attack_SeekingProjectile03State.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            {
                Attack_BestName = Attack_SeekingProjectile03State.Attack_Name;
                Attack_BestScore = Attack_SeekingProjectile03State.CalculateScore(bossEnemyComponent);
                Attack_TransitionToExecute = bossEnemyComponent.TransitionToAttack_SeekingProjectile03State;
            }
            else if (Attack_SeekingProjectile03State.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            {
                if (Random.Range(0, 3) == 0)
                {
                    Attack_BestName = Attack_SeekingProjectile03State.Attack_Name;
                    Attack_BestScore = Attack_SeekingProjectile03State.CalculateScore(bossEnemyComponent);
                    Attack_TransitionToExecute = bossEnemyComponent.TransitionToAttack_SeekingProjectile03State;
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
            // Attack_ArenaHazard01State -----------------
            //if (Attack_ArenaHazard01State.CalculateScore(bossEnemyComponent) > Attack_BestScore)
            //{
            //    Attack_BestName = Attack_ArenaHazard01State.Attack_Name;
            //    Attack_BestScore = Attack_ArenaHazard01State.CalculateScore(bossEnemyComponent);
            //    Attack_TransitionToExecute = bossEnemyComponent.TransitionToAttack_ArenaHazard01State;
            //}
            //else if (Attack_ArenaHazard01State.CalculateScore(bossEnemyComponent) == Attack_BestScore)
            //{
            //    if (Random.Range(0, 2) == 0)
            //    {
            //        Attack_BestName = Attack_ArenaHazard01State.Attack_Name;
            //        Attack_BestScore = Attack_ArenaHazard01State.CalculateScore(bossEnemyComponent);
            //        Attack_TransitionToExecute = bossEnemyComponent.TransitionToAttack_ArenaHazard01State;
            //    }
            //}
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
            Attack_BestName = Attack_SeekingProjectile01State.Attack_Name;
            Attack_BestScore = Attack_SeekingProjectile01State.CalculateScore(bossEnemyComponent);
            Attack_TransitionToExecute = bossEnemyComponent.TransitionToAttack_SeekingProjectile01State;
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
        Debug.Log("BossEnemy: Current HP = " + bossEnemyComponent.HP_ReturnCurrent());

        //bossEnemyComponent.HP_TurnInvulnerabilityOff();

        // Animation Logic
        animator.SetBool("downed", true);
        animator.SetBool("inAttack", false);

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
        if (bossEnemyComponent.HP_ReturnCurrent() <= 0.0f)                                  // check if HP_Current has fallen below 0
        {
            bossEnemyComponent.TransitionToDeathState();                                    // if so, transition to Death State
        }

        else if (bossEnemyComponent.returnCurrentEnergy() >= bossEnemyComponent.Energy_Maximum)  // check if Energy_Current has exceeded Energy_Maximum
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
        Debug.Log("BossEnemy: Current HP = " + bossEnemyComponent.HP_ReturnCurrent());

        // Animation Logic
        animator.SetBool("downed", false);
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
        animator.SetBool("die", true);

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
// *               Attack Instruction States                                                                                                                                                                    * 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// When an attack is added, it must also have Transition() methods created in BossEnemy.cs and Attack Selection logic added in the AwakeState Inherited Class
// This state is used for testing attack selection 
public class Attack_TestingState : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
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
        //Debug.Log("BossEnemy: Current Energy = " + bossEnemyComponent.returnCurrentEnergy());

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic
        // TEMP DEBUGGING CODE:
        Attack_Completed = true;

        // Animation Logic

    }

    // Called once per frame
    public override void CheckTransition()
    {
        // Programming Logic
        if (Attack_Completed == true)
        {
            bossEnemyComponent.TransitionToSelfCheckState();
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
// *               Seeking Projectile Attacks                                                                                                       * 
// --------------------------------------------------------------------------------------------------------------------------------------------------
public class Attack_SeekingProjectile01State : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;
    private GameObject Attack_GameObjectParent;
    private GameObject Attack_ProjectileOriginObject;
    private int Attack_NumberOfProjectiles_ToFire = 10;
    private int Attack_NumberOfProjectiles_BeenFired = 0;
    private float Attack_ProjectileSpeed = 30.0f;
    private Vector3 Attack_ProjectileScale = new Vector3(1.75f, 1.75f, 1.75f);
    private Vector3 Attack_ProjectileSpawnOffset = new Vector3(0, 10, 0);
    private float Attack_ProjectileInterval = 0.8f;
    private float Attack_Delay = 1.0f;
    private float Attack_StartTimeStamp = 0.0f;
    private float Attack_ProjectileLastFiredTimeStamp = 0.0f;
    private float Attack_CompletionDelay = 1.0f;

    // Attack_State Selection Properties
    public static string Attack_Name = "Attack_SeekingProjectile01State";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 40.0f;

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
        Debug.Log("BossEnemy: Entering Attack_SeekingProjectile01State");
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost);
        //Debug.Log("BossEnemy: Current Energy = " + bossEnemyComponent.returnCurrentEnergy());

        // Timer
        Attack_StartTimeStamp = Time.time;

        // Attack Setup Logic
        Attack_GameObjectParent = new GameObject("Attack_GameObjectParent");

        // Projectile Origin Object
        Attack_ProjectileOriginObject = new GameObject("Attack_ProjectileOriginObject");
        MeshFilter Attack_ProjectileOriginObject_meshfilter = Attack_ProjectileOriginObject.AddComponent<MeshFilter>();
        Attack_ProjectileOriginObject_meshfilter.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
        MeshRenderer Attack_ProjectileOriginObject_meshRenderer = Attack_ProjectileOriginObject.AddComponent<MeshRenderer>();
        Attack_ProjectileOriginObject.transform.position = bossEnemyComponent.returnBossEnemyPosition() + Attack_ProjectileSpawnOffset;
        Attack_ProjectileOriginObject.transform.SetParent(Attack_GameObjectParent.transform);

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic
        if (Attack_NumberOfProjectiles_BeenFired == Attack_NumberOfProjectiles_ToFire)              // check if all projectiles have been fired
        {
            Attack_Completed = true;                                                                // if so, end attack
        }
        else
        {
            if (Time.time - Attack_StartTimeStamp >= Attack_Delay)                                  // check if attack delay has been exceeded
            {
                if (Time.time - Attack_ProjectileLastFiredTimeStamp >= Attack_ProjectileInterval)   // if so, check if interval between projectiles has been exceeded
                {
                    Attack_NumberOfProjectiles_BeenFired += 1;                                      // if so, increment Attack_NumberOfProjectiles_BeenFired and fire projectile
                    Attack_ProjectileLastFiredTimeStamp = Time.time;                                // update timestamp for most recent projectile being fired

                    // Spawn Projectile
                    GameObject Attack_NewProjectile = Object.Instantiate(bossEnemyComponent.Attack_BasicProjectile01, Attack_ProjectileSpawnOffset, bossEnemyComponent.Player_ReturnDirectionOfPlayer(bossEnemyComponent.returnBossEnemyPosition()));
                    Attack_NewProjectile.transform.localScale = Attack_ProjectileScale;
                    Attack_NewProjectile.name = "Attack_Projectile_" + Attack_NumberOfProjectiles_BeenFired;
                    Attack_NewProjectile.tag = "Damage Source";
                    Attack_NewProjectile.transform.SetParent(Attack_ProjectileOriginObject.transform);
                    Attack_NewProjectile.transform.localPosition = Vector3.zero;                    // reset local position

                    // Move Projectile
                    Attack_NewProjectile.GetComponent<BasicProjectile>().FireProjectile(Attack_ProjectileSpeed, bossEnemyComponent.Player_ReturnPlayerPosition());
                }
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
            if (Time.time - Attack_ProjectileLastFiredTimeStamp >= Attack_CompletionDelay)
            {
                bossEnemyComponent.TransitionToSelfCheckState();
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
        bossEnemyComponent.appendToAttackHistory(Attack_Name);

        // Attack Logic
        GameObject.Destroy(Attack_GameObjectParent);

        // Animation Logic
        animator.SetBool("inAttack", false);

    }
}

public class Attack_SeekingProjectile02State : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;
    private GameObject Attack_GameObjectParent;
    private GameObject Attack_ProjectileOriginObject;
    private int Attack_NumberOfProjectiles_ToFire = 50;
    private int Attack_NumberOfProjectiles_BeenFired = 0;
    private float Attack_ProjectileSpeed = 25.0f;
    private Vector3 Attack_ProjectileSpawnOffset = new Vector3(0, 10, 0);
    private float Attack_ProjectileInterval = 0.125f;
    private float Attack_Delay = 1.0f;
    private float Attack_StartTimeStamp = 0.0f;
    private float Attack_ProjectileLastFiredTimeStamp = 0.0f;
    private float Attack_CompletionDelay = 1.0f;

    // Attack_State Selection Properties
    public static string Attack_Name = "Attack_SeekingProjectile02State";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 40.0f;

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
        Debug.Log("BossEnemy: Entering Attack_SeekingProjectile02State");
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost);
        //Debug.Log("BossEnemy: Current Energy = " + bossEnemyComponent.returnCurrentEnergy());

        // Timer
        Attack_StartTimeStamp = Time.time;

        // Attack Setup Logic
        Attack_GameObjectParent = new GameObject("Attack_GameObjectParent");

        // Projectile Origin Object
        Attack_ProjectileOriginObject = new GameObject("Attack_ProjectileOriginObject");
        MeshFilter Attack_ProjectileOriginObject_meshfilter = Attack_ProjectileOriginObject.AddComponent<MeshFilter>();
        Attack_ProjectileOriginObject_meshfilter.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
        MeshRenderer Attack_ProjectileOriginObject_meshRenderer = Attack_ProjectileOriginObject.AddComponent<MeshRenderer>();
        Attack_ProjectileOriginObject.transform.position = bossEnemyComponent.returnBossEnemyPosition() + Attack_ProjectileSpawnOffset;
        Attack_ProjectileOriginObject.transform.SetParent(Attack_GameObjectParent.transform);

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic
        if (Attack_NumberOfProjectiles_BeenFired == Attack_NumberOfProjectiles_ToFire)              // check if all projectiles have been fired
        {
            Attack_Completed = true;                                                                // if so, end attack
        }
        else
        {
            if (Time.time - Attack_StartTimeStamp >= Attack_Delay)                                  // check if attack delay has been exceeded
            {
                if (Time.time - Attack_ProjectileLastFiredTimeStamp >= Attack_ProjectileInterval)   // if so, check if interval between projectiles has been exceeded
                {
                    Attack_NumberOfProjectiles_BeenFired += 1;                                      // if so, increment Attack_NumberOfProjectiles_BeenFired and fire projectile
                    Attack_ProjectileLastFiredTimeStamp = Time.time;                                // update timestamp for most recent projectile being fired

                    // Spawn Projectile
                    GameObject Attack_NewProjectile = Object.Instantiate(bossEnemyComponent.Attack_BasicProjectile01, Attack_ProjectileSpawnOffset, bossEnemyComponent.Player_ReturnDirectionOfPlayer(bossEnemyComponent.returnBossEnemyPosition()));
                    Attack_NewProjectile.name = "Attack_Projectile_" + Attack_NumberOfProjectiles_BeenFired;
                    Attack_NewProjectile.tag = "Damage Source";
                    Attack_NewProjectile.transform.SetParent(Attack_ProjectileOriginObject.transform);
                    Attack_NewProjectile.transform.localPosition = Vector3.zero;                    // reset local position

                    // Move Projectile
                    Attack_NewProjectile.GetComponent<BasicProjectile>().FireProjectile(Attack_ProjectileSpeed, bossEnemyComponent.Player_ReturnPlayerPosition());
                }
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
            if (Time.time - Attack_ProjectileLastFiredTimeStamp >= Attack_CompletionDelay)
            {
                bossEnemyComponent.TransitionToSelfCheckState();
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
        bossEnemyComponent.appendToAttackHistory(Attack_Name);

        // Attack Logic
        GameObject.Destroy(Attack_GameObjectParent);

        // Animation Logic
        animator.SetBool("inAttack", false);

    }
}

public class Attack_SeekingProjectile03State : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;
    private GameObject Attack_GameObjectParent;
    private GameObject Attack_ProjectileOriginObject;
    private int Attack_NumberOfProjectiles_ToFire = 10;
    private int Attack_NumberOfProjectiles_BeenFired = 0;
    private float Attack_ProjectileSpeed = 25.0f;
    private Vector3 Attack_ProjectileScale = new Vector3(3, 3, 3);
    private Vector3 Attack_ProjectileSpawnOffset = new Vector3(0, 10, 0);
    private float Attack_ProjectileInterval = 0.75f;
    private float Attack_Delay = 1.0f;
    private float Attack_StartTimeStamp = 0.0f;
    private float Attack_ProjectileLastFiredTimeStamp = 0.0f;
    private float Attack_CompletionDelay = 2.0f;

    // Attack_State Selection Properties
    public static string Attack_Name = "Attack_SeekingProjectile03State";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 10.0f;
    public static float Player_MaxDistance = 40.0f;

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
        Debug.Log("BossEnemy: Entering Attack_SeekingProjectile03State");
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost);
        //Debug.Log("BossEnemy: Current Energy = " + bossEnemyComponent.returnCurrentEnergy());

        // Timer
        Attack_StartTimeStamp = Time.time;

        // Attack Setup Logic
        Attack_GameObjectParent = new GameObject("Attack_GameObjectParent");

        // Projectile Origin Object
        Attack_ProjectileOriginObject = new GameObject("Attack_ProjectileOriginObject");
        MeshFilter Attack_ProjectileOriginObject_meshfilter = Attack_ProjectileOriginObject.AddComponent<MeshFilter>();
        Attack_ProjectileOriginObject_meshfilter.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
        MeshRenderer Attack_ProjectileOriginObject_meshRenderer = Attack_ProjectileOriginObject.AddComponent<MeshRenderer>();
        Attack_ProjectileOriginObject.transform.position = bossEnemyComponent.returnBossEnemyPosition() + Attack_ProjectileSpawnOffset;
        Attack_ProjectileOriginObject.transform.SetParent(Attack_GameObjectParent.transform);

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic
        if (Attack_NumberOfProjectiles_BeenFired == Attack_NumberOfProjectiles_ToFire)              // check if all projectiles have been fired
        {
            Attack_Completed = true;                                                                // if so, end attack
        }
        else
        {
            if (Time.time - Attack_StartTimeStamp >= Attack_Delay)                                  // check if attack delay has been exceeded
            {
                if (Time.time - Attack_ProjectileLastFiredTimeStamp >= Attack_ProjectileInterval)   // if so, check if interval between projectiles has been exceeded
                {
                    Attack_NumberOfProjectiles_BeenFired += 1;                                      // if so, increment Attack_NumberOfProjectiles_BeenFired and fire projectile
                    Attack_ProjectileLastFiredTimeStamp = Time.time;                                // update timestamp for most recent projectile being fired

                    // Spawn Projectile
                    GameObject Attack_NewProjectile = Object.Instantiate(bossEnemyComponent.Attack_BasicProjectile01, Attack_ProjectileSpawnOffset, bossEnemyComponent.Player_ReturnDirectionOfPlayer(bossEnemyComponent.returnBossEnemyPosition()));
                    Attack_NewProjectile.transform.localScale = Attack_ProjectileScale;
                    Attack_NewProjectile.name = "Attack_Projectile_" + Attack_NumberOfProjectiles_BeenFired;
                    Attack_NewProjectile.tag = "Damage Source";
                    Attack_NewProjectile.transform.SetParent(Attack_ProjectileOriginObject.transform);
                    Attack_NewProjectile.transform.localPosition = Vector3.zero;                    // reset local position

                    // Move Projectile
                    Attack_NewProjectile.GetComponent<BasicProjectile>().FireProjectile(Attack_ProjectileSpeed, bossEnemyComponent.Player_ReturnPlayerPosition());
                }
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
            if (Time.time - Attack_ProjectileLastFiredTimeStamp >= Attack_CompletionDelay)
            {
                bossEnemyComponent.TransitionToSelfCheckState();
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
        bossEnemyComponent.appendToAttackHistory(Attack_Name);

        // Attack Logic
        GameObject.Destroy(Attack_GameObjectParent);

        // Animation Logic
        animator.SetBool("inAttack", false);

    }
}

// --------------------------------------------------------------------------------------------------------------------------------------------------
// *               Laser Attacks                                                                                                                    * 
// --------------------------------------------------------------------------------------------------------------------------------------------------
public class Attack_Laser01State : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;
    private GameObject Attack_GameObjectParent;
    private GameObject Attack_LaserOriginObject;
    private Vector3 LaserSourceOffset = new Vector3(0, 5, 0);
    private GameObject Attack_LaserObject;
    private GameObject Attack_LaserContactObject;
    private float Attack_Duration = 10.0f;
    private float Attack_LaserDelay = 0.8f;
    private float Attack_StartTimeStamp = 0.0f;
    private float Attack_PlayerPositionDelay = 0.6f;

    // Attack_State Selection Properties
    public static string Attack_Name = "Attack_Laser01State";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 5.0f;
    public static float Player_MaxDistance = 40.0f;

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
        // Energy Update and Debugging
        Debug.Log("BossEnemy: Entering Attack_Laser01State");
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost);
        //Debug.Log("BossEnemy: Current Energy = " + bossEnemyComponent.returnCurrentEnergy());

        // Timer
        Attack_StartTimeStamp = Time.time;

        // Attack Setup Logic
        Attack_GameObjectParent = new GameObject("Attack_GameObjectParent");

        // Laser Source
        Attack_LaserOriginObject = new GameObject("Attack_LaserOriginObject");
        MeshFilter Attack_LaserOriginObject_meshfilter = Attack_LaserOriginObject.AddComponent<MeshFilter>();
        Attack_LaserOriginObject_meshfilter.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
        MeshRenderer Attack_LaserOriginObject_meshRenderer = Attack_LaserOriginObject.AddComponent<MeshRenderer>();
        Attack_LaserOriginObject.transform.position = bossEnemyComponent.returnBossEnemyPosition() + LaserSourceOffset;
        Attack_LaserOriginObject.transform.SetParent(Attack_GameObjectParent.transform);

        // Laser
        Attack_LaserObject = new GameObject("Attack_LaserObject");
        Attack_LaserObject.SetActive(false);
        LineRenderer lineRenderer = Attack_LaserObject.AddComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, Attack_LaserOriginObject.transform.position);
        lineRenderer.SetPosition(1, bossEnemyComponent.Player_ReturnPositionFromXSecondsAgo(Attack_PlayerPositionDelay));
        Attack_LaserObject.transform.SetParent(Attack_GameObjectParent.transform);

        // Laser Contact
        Attack_LaserContactObject = new GameObject("Attack_LaserContactObject");
        Attack_LaserContactObject.SetActive(false);
        MeshFilter Attack_LaserContactObject_meshfilter = Attack_LaserContactObject.AddComponent<MeshFilter>();
        Attack_LaserContactObject_meshfilter.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
        MeshRenderer Attack_LaserContactObject_meshRenderer = Attack_LaserContactObject.AddComponent<MeshRenderer>();
        SphereCollider Attack_LaserContactObject_collider = Attack_LaserContactObject.AddComponent<SphereCollider>();
        Attack_LaserContactObject_collider.isTrigger = true; 
        Attack_LaserContactObject.tag = "Damage Source";
        Attack_LaserContactObject.transform.position = bossEnemyComponent.Player_ReturnPositionFromXSecondsAgo(Attack_PlayerPositionDelay);
        Attack_LaserContactObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Attack_LaserContactObject.transform.SetParent(Attack_GameObjectParent.transform);

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
        if (Attack_LaserObject.activeSelf == false)                     // check if laser object is not active
        {
            if (Time.time - Attack_StartTimeStamp >= Attack_LaserDelay) // if so, check if the duration of the attack has been exceeded Attack_LaserDelay
            {
                Attack_LaserObject.SetActive(true);                     // if so, set laser object to active
                Attack_LaserContactObject.SetActive(true);              // set laser contact object to active
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
            bossEnemyComponent.TransitionToSelfCheckState();
        }

        // Animation Logic

    }

    // Called at fixed intervals (used for physics updates)
    public override void FixedUpdate()
    {
        // Programming Logic
        // Laser
        Attack_LaserObject.GetComponent<LineRenderer>().SetPosition(1, bossEnemyComponent.Player_ReturnPositionFromXSecondsAgo(Attack_PlayerPositionDelay));

        // Laser Contact
        Attack_LaserContactObject.transform.position = bossEnemyComponent.Player_ReturnPositionFromXSecondsAgo(Attack_PlayerPositionDelay);

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

// --------------------------------------------------------------------------------------------------------------------------------------------------
// *               Arena Hazard Attacks                                                                                                             * 
// --------------------------------------------------------------------------------------------------------------------------------------------------
public class Attack_ArenaHazard01State : BossState
{
    // Private Attributes
    private bool Attack_Completed = false;

    // Attack_State Selection Properties
    public static string Attack_Name = "Attack_ArenaHazard01State";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 15.0f;
    public static float Player_MaxDistance = 40.0f;

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
        //Debug.Log("BossEnemy: Entering Attack_ArenaHazard01State");
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost);
        //Debug.Log("BossEnemy: Current Energy = " + bossEnemyComponent.returnCurrentEnergy());

        // Animation Logic
        animator.SetBool("inAttack", true);

    }

    // Called once per frame
    public override void Update()
    {
        // Programming Logic
        // TEMP DEBUGGING CODE:
        Attack_Completed = true;

        // Animation Logic

    }

    // Called once per frame
    public override void CheckTransition()
    {
        // Programming Logic
        if (Attack_Completed == true)
        {
            bossEnemyComponent.TransitionToSelfCheckState();
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
public class Attack_Melee01State : BossState
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
    public static string Attack_Name = "Attack_Melee01State";
    public static float Energy_Cost = 1.0f;
    public static float Player_MinDistance = 0.0f;
    public static float Player_MaxDistance = 5.0f;

    public static float CalculateScore(BossEnemy bossEnemyComponent)
    {
        float score = 0.0f;

        // Check distance ----------------------------*
        if (bossEnemyComponent.Player_ReturnDistance() >= Player_MinDistance && bossEnemyComponent.Player_ReturnDistance() <= Player_MaxDistance)
        {
            score += 2.0f;
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
        Debug.Log("BossEnemy: Entering Attack_Melee01State");
        bossEnemyComponent.updateCurrentEnergy(bossEnemyComponent.returnCurrentEnergy() - Energy_Cost);
        //Debug.Log("BossEnemy: Current Energy = " + bossEnemyComponent.returnCurrentEnergy());

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
        SphereCollider Attack_LaserContactObject_collider = Attack_ColliderSphere.AddComponent<SphereCollider>();
        Attack_LaserContactObject_collider.isTrigger = true;
        Attack_ColliderSphere.tag = "Damage Source";
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
                Attack_IsColliderSphereScaleOut = true;                                         // if so, update Attack_IsColliderSphereScaleOut to true
                Attack_ColliderSphere.transform.localScale = Attack_ColliderSphereScale_Out;    // set collider sphere object to use Attack_ColliderSphereScale_Out scaling
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
            bossEnemyComponent.TransitionToSelfCheckState();
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