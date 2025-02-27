using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.SearchService;

//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEnemy : MonoBehaviour
{
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Public Fields                                                                                                                                                                                * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Debug Values ---------------------------------------------------------------------------------------------------------------
    [Tooltip("Maximum HP that the Boss Enemy can have (first combat iteration).")]
    public float HP_Maximum_1 = 50.0f;
    [Tooltip("Maximum Energy (cost of attacks) that the Boss Enemy can have (first combat iteration).")]
    public float Energy_Maximum_1 = 3.0f;
    [Tooltip("Amount of Energy the Boss Enemy regains over the course of a second while in 'State_LowEnergy' (first combat iteration).")]
    public float Energy_RegainedPerSecond_1 = 0.15f;
    [Tooltip("Amount of Energy the Boss Enemy when struck while in 'State_LowEnergy' (first combat iteration).")]
    public float Energy_RegainedOnStrike_1 = 0.4f;
    [Tooltip("Amount of time that must pass when entering the 'State_Awake' before the BossEnemy can execute the selected attack (first combat iteration).")]
    public float State_Awake_Delay_1 = 4.0f;

    [Tooltip("Maximum HP that the Boss Enemy can have (second combat iteration).")]
    public float HP_Maximum_2 = 50.0f;
    [Tooltip("Maximum Energy (cost of attacks) that the Boss Enemy can have (second combat iteration).")]
    public float Energy_Maximum_2 = 3.0f;
    [Tooltip("Amount of Energy the Boss Enemy regains over the course of a second while in 'State_LowEnergy' (second combat iteration).")]
    public float Energy_RegainedPerSecond_2 = 0.15f;
    [Tooltip("Amount of Energy the Boss Enemy when struck while in 'State_LowEnergy' (second combat iteration).")]
    public float Energy_RegainedOnStrike_2 = 0.4f;
    [Tooltip("Amount of time that must pass when entering the 'State_Awake' before the BossEnemy can execute the selected attack (second combat iteration).")]
    public float State_Awake_Delay_2 = 4.0f;

    [Tooltip("Maximum HP that the Boss Enemy can have (first combat iteration).")]
    public float HP_Maximum_3 = 50.0f;
    [Tooltip("Maximum Energy (cost of attacks) that the Boss Enemy can have (first combat iteration).")]
    public float Energy_Maximum_3 = 3.0f;
    [Tooltip("Amount of Energy the Boss Enemy regains over the course of a third while in 'State_LowEnergy' (first combat iteration).")]
    public float Energy_RegainedPerSecond_3 = 0.15f;
    [Tooltip("Amount of Energy the Boss Enemy when struck while in 'State_LowEnergy' (first combat iteration).")]
    public float Energy_RegainedOnStrike_3 = 0.4f;
    [Tooltip("Amount of time that must pass when entering the 'State_Awake' before the BossEnemy can execute the selected attack (first combat iteration).")]
    public float State_Awake_Delay_3 = 4.0f;

    [Tooltip("Amount of time that passed between storing player position (in seconds).")]
    public float Player_PositionTrackingTimeInterval = 0.02f;
    [Tooltip("Amount of time that must pass before an entry in the Player_PositionHistory list is deleted (in seconds).")]
    public float Player_PositionTrackingMaxTimeTracked = 3.0f;

    [Tooltip("The amount of damage that the boss enemy takes when a projectile is successfully deflected back into the boss enemy.")]
    public float Deflection_DamageOnSuccessfulDeflect = 5.0f;


    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Private/Protected Attributes                                                                                                                                                                 * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Boss Enemy Attributes ------------------------------------------------------------------------------------------------------
    private float HP_Current;
    private float Energy_Current;
    //private bool HP_BossInvulnerable = false; //changed from default true (isn't currently utilized, the functionality for if enemy can take damage is hardcoded into HP_TakeDamage()
    private float HP_Maximum;                    // Maximum HP that the Boss Enemy can have
    private float Energy_Maximum;                // Maximum Energy (cost of attacks) that the Boss Enemy can have
    private float Energy_RegainedPerSecond;      // Amount of Energy the Boss Enemy regains over the course of a second while in 'State_LowEnergy'
    private float Energy_RegainedOnStrike;       // Amount of Energy the Boss Enemy when struck while in 'State_LowEnergy'
    private float State_Awake_Delay;             // Amount of time that must pass when entering the 'State_Awake' before the BossEnemy can execute the selected attack

    // Object References ----------------------------------------------------------------------------------------------------------
    private Animator bossAnimator;
    GameObject playerGameObject;

    // Spawners -------------------------------------------------------------------------------------------------------------------
    private ProjectileSpawner[] ProjectileSpawnersList;

    // State Machine Attributes ---------------------------------------------------------------------------------------------------
    private BossStateMachine stateMachine;

    // Player Actions/Location ----------------------------------------------------------------------------------------------------
    private bool playerTriggeredBossWakeup = false;
    private float Player_TimePassedSinceLastPositionTrack = 0f;
    private List<Tuple<Vector3, float>> Player_PositionHistory = new List<Tuple<Vector3, float>>(); // stores past positions and timestamps of past positions

    private class Tuple<T1, T2> // tuple for storing both position and timestamp
    {
        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }

        public Tuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }

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

        // Set Spawner References
        ProjectileSpawnersList = GetComponents<ProjectileSpawner>();

        // Initialize Attributes
        stateMachine = new BossStateMachine();

        // Initialize Boss Enemy Attributes and Set To State_Sleeping
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
        //Debug.Log(HP_Current);
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Fixed Update Function                                                                                                                                                                        * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // FixedUpdate is called at set intervals
    void FixedUpdate()
    {
        // Update Player_PositionHistory
        Player_TimePassedSinceLastPositionTrack += Time.fixedDeltaTime; // track the time passed since the last stored position
        if (Player_TimePassedSinceLastPositionTrack >= Player_PositionTrackingTimeInterval) // store position at the specified interval
        {
            Player_PositionHistory.Add(new Tuple<Vector3, float>(Player_ReturnPlayerPosition(), Time.time));
            Player_TimePassedSinceLastPositionTrack = 0f;
        }

        // Remove entries from Player_PositionHistory older than maxStoredTime
        Player_PositionHistoryRemoveOldEntries();

        // Execute FixedUpdate instructions for StateMachine
        stateMachine.FixedUpdate();
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Late Update Function                                                                                                                                                                         * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Called after all other update functions
    public void LateUpdate()
    {
        stateMachine.LateUpdate();
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Player Functions                                                                                                                                                                             * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public float Player_ReturnDistance()
    {
        return Vector3.Distance(transform.position, playerGameObject.transform.position);
    }

    public Vector3 Player_ReturnPlayerPosition()
    {
        return playerGameObject.transform.position;
    }

    public void Player_EnteredWakeupTrigger()
    {
        playerTriggeredBossWakeup = true;
    }
    public bool Player_ReturnPlayerTriggeredBossWakeup()
    {
        return playerTriggeredBossWakeup;
    }

    public void Player_PositionHistoryRemoveOldEntries()
    {
        float currentTime = Time.time;
        Player_PositionHistory.RemoveAll(entry => currentTime - entry.Item2 > Player_PositionTrackingMaxTimeTracked);
    }

    public Vector3 Player_ReturnPositionFromXSecondsAgo(float secondsAgo)
    {
        int index = Mathf.FloorToInt(secondsAgo / Player_PositionTrackingTimeInterval);

        // Ensure the index is within bounds
        if (index >= 0 && index < Player_PositionHistory.Count)
        {
            return Player_PositionHistory[Player_PositionHistory.Count - 1 - index].Item1;
        }
        else
        {
           // Debug.LogWarning("Requested time is out of range. Returning the oldest stored position.");
            return Player_PositionHistory.Count > 0 ? Player_PositionHistory[Player_PositionHistory.Count - 1].Item1 : transform.position; // if requested timestamp is out of range, the oldest stored position is instead used
        }
    }

    // Returns a quaternion pointing towards the position of the player from positionToCheckFrom
    public Quaternion Player_ReturnDirectionOfPlayer(Vector3 positionToCheckFrom)
    {
        Vector3 playerPosition = Player_ReturnPlayerPosition();

        Vector3 directionToPlayer = playerPosition - positionToCheckFrom;   // calculate the direction from the given position to the player

        if (directionToPlayer != Vector3.zero)                              // check if the direction is non-zero to avoid errors
        {
            return Quaternion.LookRotation(directionToPlayer);              // return a Quaternion representing the direction
        }
        else
        {
            return Quaternion.identity;                                     // return an identity quaternion if there's no direction
        }
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Spawner Functions                                                                                                                                                                            * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public ProjectileSpawner ReturnComponent_Spawner_Bullet()
    {
        foreach (var Spawner in ProjectileSpawnersList)
        {
            if (Spawner.Spawner_ID == "Bullet")
            {
                return Spawner;
            }
        }
        return null;
    }

    public ProjectileSpawner ReturnComponent_Spawner_Mine()
    {
        foreach (var Spawner in ProjectileSpawnersList)
        {
            if (Spawner.Spawner_ID == "Mine")
            {
                return Spawner;
            }
        }
        return null;
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               State Transition Functions                                                                                                                                                                   * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void TransitionToState_Sleeping()
    {
        State_Sleeping sleepingState = new State_Sleeping();
        sleepingState.Initialize(bossAnimator, this);
        stateMachine.SetState(sleepingState);
    }
    public void TransitionToState_WakingUp()
    {
        State_WakingUp wakingUpState = new State_WakingUp();
        wakingUpState.Initialize(bossAnimator, this);
        stateMachine.SetState(wakingUpState);
    }
    public void TransitionToState_SelfCheck()
    {
        State_SelfCheck selfCheckState = new State_SelfCheck();
        selfCheckState.Initialize(bossAnimator, this);
        stateMachine.SetState(selfCheckState);
    }
    public void TransitionToState_Awake()
    {
        State_Awake awakeState = new State_Awake();
        awakeState.Initialize(bossAnimator, this);
        stateMachine.SetState(awakeState);
    }
    public void TransitionToState_LowEnergy()
    {
        State_LowEnergy lowEnergyState = new State_LowEnergy();
        lowEnergyState.Initialize(bossAnimator, this);
        stateMachine.SetState(lowEnergyState);
    }
    public void TransitionToState_Death()
    {
        State_Death deathState = new State_Death();
        deathState.Initialize(bossAnimator, this);
        stateMachine.SetState(deathState);
    }
    public void TransitionToState_Attack_Bullet_SlowFiringShot_Easy()
    {
        State_Attack_Bullet_SlowFiringShot_Easy attackBulletSlowFiringShot = new State_Attack_Bullet_SlowFiringShot_Easy();
        attackBulletSlowFiringShot.Initialize(bossAnimator, this);
        stateMachine.SetState(attackBulletSlowFiringShot);
    }
    public void TransitionToState_Attack_Bullet_SlowFiringShot_Hard()
    {
        State_Attack_Bullet_SlowFiringShot_Hard attackBulletSlowFiringShot = new State_Attack_Bullet_SlowFiringShot_Hard();
        attackBulletSlowFiringShot.Initialize(bossAnimator, this);
        stateMachine.SetState(attackBulletSlowFiringShot);
    }
    public void TransitionToState_Attack_Bullet_TrackingCone_Easy()
    {
        State_Attack_Bullet_TrackingCone_Easy attackBulletTrackingCone = new State_Attack_Bullet_TrackingCone_Easy();
        attackBulletTrackingCone.Initialize(bossAnimator, this);
        stateMachine.SetState(attackBulletTrackingCone);
    }
    public void TransitionToState_Attack_Bullet_TrackingCone_Hard()
    {
        State_Attack_Bullet_TrackingCone_Hard attackBulletTrackingCone = new State_Attack_Bullet_TrackingCone_Hard();
        attackBulletTrackingCone.Initialize(bossAnimator, this);
        stateMachine.SetState(attackBulletTrackingCone);
    }
    public void TransitionToState_Attack_Bullet_TrackingWall_Easy()
    {
        State_Attack_Bullet_TrackingWall_Easy attackBulletTrackingWall = new State_Attack_Bullet_TrackingWall_Easy();
        attackBulletTrackingWall.Initialize(bossAnimator, this);
        stateMachine.SetState(attackBulletTrackingWall);
    }
    public void TransitionToState_Attack_Bullet_TrackingWall_Hard()
    {
        State_Attack_Bullet_TrackingWall_Hard attackBulletTrackingWall = new State_Attack_Bullet_TrackingWall_Hard();
        attackBulletTrackingWall.Initialize(bossAnimator, this);
        stateMachine.SetState(attackBulletTrackingWall);
    }
    public void TransitionToState_Attack_Bullet_RotatingWall_Easy()
    {
        State_Attack_Bullet_RotatingWall_Easy attackBulletRotatingWall = new State_Attack_Bullet_RotatingWall_Easy();
        attackBulletRotatingWall.Initialize(bossAnimator, this);
        stateMachine.SetState(attackBulletRotatingWall);
    }
    public void TransitionToState_Attack_Bullet_RotatingWall_Hard()
    {
        State_Attack_Bullet_RotatingWall_Hard attackBulletRotatingWall = new State_Attack_Bullet_RotatingWall_Hard();
        attackBulletRotatingWall.Initialize(bossAnimator, this);
        stateMachine.SetState(attackBulletRotatingWall);
    }
    public void TransitionToState_Attack_ArenaHazard_Mine_Random()
    {
        State_Attack_ArenaHazard_Mine_Random attackArenaHazardMineRandom = new State_Attack_ArenaHazard_Mine_Random();
        attackArenaHazardMineRandom.Initialize(bossAnimator, this);
        stateMachine.SetState(attackArenaHazardMineRandom);
    }
    public void TransitionToState_Attack_StandUpMelee()
    {
        State_Attack_StandUpMelee attackStandUpMelee = new State_Attack_StandUpMelee();
        attackStandUpMelee.Initialize(bossAnimator, this);
        stateMachine.SetState(attackStandUpMelee);
    }
    public void TransitionToState_Attack_Melee01()
    {
        State_Attack_Melee01 attackMelee01State = new State_Attack_Melee01();
        attackMelee01State.Initialize(bossAnimator, this);
        stateMachine.SetState(attackMelee01State);
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               HP Functions                                                                                                                                                                                 * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public float HP_ReturnCurrent()
    {
        return HP_Current;
    }

    public float HP_ReturnMax()
    {
        return HP_Maximum;
    }

    public float HP_ReturnCurrentAsPercentage()
    {
        return (HP_ReturnCurrent() / HP_ReturnMax()) * 100f;
    }

    public bool HP_IsZero()
    {
        if (HP_Current <= 0.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void HP_UpdateCurrent(float newHPCurrent)
    {
        HP_Current = newHPCurrent;
    }

    public void HP_TakeDamage(float damageAmount)
    {
        //Debug.Log("Boss Enemy: HP_TakeDamage() Performed");
        //if (!HP_ReturnInvulnerabilityStatus())
        //{
        //    HP_Current -= damageAmount;
        //    Energy_Current += Energy_RegainedOnStrike;
        //    //Debug.Log("BossEnemy: " + damageAmount + " Damage Taken | HP = " + HP_Current);
        //}
        HP_Current -= damageAmount;
        VFX_DamageTaken();
        // if state
        if (stateMachine.returnCurrentState() is State_LowEnergy)
        {
            Energy_Current += Energy_RegainedOnStrike;
        }
    }

    //public void HP_TurnInvulnerabilityOn()
    //{
    //    HP_BossInvulnerable = true;
    //}

    //public void HP_TurnInvulnerabilityOff()
    //{
    //    HP_BossInvulnerable = false;
    //}

    //public bool HP_ReturnInvulnerabilityStatus()
    //{
    //    return HP_BossInvulnerable;
    //}

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               VFX Functions                                                                                                                                                                                * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // run when taking damage
    public void VFX_DamageTaken()
    {
        //Debug.Log("Boss Enemy: VFX_DamageTaken() Performed");
        if (HP_ReturnCurrentAsPercentage() > 75)
        {
            VFX_DamageSparks_PlayRandom(1);
        }
        else if (HP_ReturnCurrentAsPercentage() > 50)
        {
            VFX_DamageSparks_PlayRandom(2);
        }
        else if (HP_ReturnCurrentAsPercentage() > 25)
        {
            VFX_DamageSparks_PlayRandom(3);
        }
        else if (HP_ReturnCurrentAsPercentage() > 0)
        {
            VFX_DamageSparks_PlayRandom(4);
        }
    }

    public void VFX_DamageSparks_PlayRandom(int numSparks)
    {
        //Debug.Log("Boss Enemy: VFX_DamageSparks_PlayRandom() Performed");
        GameObject Sparks_Parent = transform.Find("root/jnt_rotBase/OnHit_VFX")?.gameObject;
        int Sparks_NumberOfNodes = Sparks_Parent.transform.childCount;
        for (int i = numSparks; i > 0; i--)
        {
            int Sparks_SelectedNumber = UnityEngine.Random.Range(1, Sparks_NumberOfNodes + 1);
            GameObject Sparks_SelectedGameObject = Sparks_Parent.transform.Find("Spark_Node_" + Sparks_SelectedNumber)?.gameObject;
            ParticleSystem Sparks_SelectedParticleSystem = Sparks_SelectedGameObject.GetComponent<ParticleSystem>();
            Sparks_SelectedParticleSystem.Play();
        }
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Energy Functions                                                                                                                                                                             * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public float returnCurrentEnergy()
    {
        return Energy_Current;
    }

    public bool Energy_IsFull()
    {
        if (Energy_Current >= Energy_Maximum)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float Energy_ReturnMaximum()
    {
        return Energy_Maximum;
    }

    public void updateCurrentEnergy(float newEnergyCurrent)
    {
        Energy_Current = newEnergyCurrent;
    }

    public void regainCurrentEnergyPerFrame()
    {
        Energy_Current += Energy_RegainedPerSecond * Time.deltaTime;   // regain X% of Energy_RegainedPerSecond by multiplying the amount by the duration of time between frames (at 50fps, 1/50th)
        //Debug.Log("BossEnemy: Current Energy = " + Energy_Current);
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Get/Set Functions                                                                                                                                                                            * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public Vector3 returnBossEnemyPosition()
    {
        return transform.position;
    }

    public float returnStateAwakeDelay()
    {
        return State_Awake_Delay;
    }

    public int returnBossEnemyEncounterIteration()
    {
        int sceneCount = SceneManager.sceneCount;
        for (int i = 0; i < sceneCount; i++)
        {
            UnityEngine.SceneManagement.Scene scene = SceneManager.GetSceneAt(i);
            //Debug.Log("Loaded scene: " + scene.name);
            if (scene.name == "Combat1")
            {
                return 1;
            }
            if (scene.name == "Combat2")
            {
                return 2;
            }
            if (scene.name == "Combat3")
            {
                return 3;
            }
        }
        Debug.Log("BossEnemy: NO COMBAT SCENE LOADED");
        return 0;
    }

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
    // *               Deflected Projectiles Functions                                                                                                                                                              * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Deflecttion_SuccessfullyStrikedBoss(GameObject projectile)
    {
        projectile.GetComponent<Projectile_Bullet>().ReturnToPool();
    }
    
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile" && collision.gameObject.GetComponent<Projectile_Bullet>() != null)
        {
            if(collision.gameObject.GetComponent<Projectile_Bullet>().Deflection_HasBeenDeflected() == true)
            {
                Deflecttion_SuccessfullyStrikedBoss(collision.gameObject);
            }
        }
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Misc. Functions                                                                                                                                                                              * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void resetBossEnemy()
    {
        if (returnBossEnemyEncounterIteration() == 1)
        {
            HP_Maximum = HP_Maximum_1;
            Energy_Maximum = Energy_Maximum_1;
            Energy_RegainedPerSecond = Energy_RegainedPerSecond_1;
            Energy_RegainedOnStrike = Energy_RegainedOnStrike_1;
            State_Awake_Delay = State_Awake_Delay_1;
        }
        if (returnBossEnemyEncounterIteration() == 2)
        {
            HP_Maximum = HP_Maximum_2;
            Energy_Maximum = Energy_Maximum_2;
            Energy_RegainedPerSecond = Energy_RegainedPerSecond_2;
            Energy_RegainedOnStrike = Energy_RegainedOnStrike_2;
            State_Awake_Delay = State_Awake_Delay_2;
        }
        if (returnBossEnemyEncounterIteration() == 3)
        {
            HP_Maximum = HP_Maximum_3;
            Energy_Maximum = Energy_Maximum_3;
            Energy_RegainedPerSecond = Energy_RegainedPerSecond_3;
            Energy_RegainedOnStrike = Energy_RegainedOnStrike_3;
            State_Awake_Delay = State_Awake_Delay_3;
        }

        // Initialize Boss Enemy Attributes
        HP_Current = HP_Maximum;
        Energy_Current = Energy_Maximum;

        // Set To State_Sleeping
        TransitionToState_Sleeping();
    }
}
