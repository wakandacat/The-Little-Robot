using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Mine : Projectile
{
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Public Attributes                                                                                                                      * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    [Tooltip("The amount of time in seconds it takes for the mine to arm once it has landed on the ground.")]
    public float Mine_TimeToArm = 0.75f;
    [Tooltip("The amount of time in seconds it takes for the mine to explode once it has been triggered.")]
    public float Mine_TimeToExplode = 0.15f;
    [Tooltip("The amount of time in seconds that the explosion lasts.")]
    public float Mine_DurationOfExplosion = 0.75f;
    [Tooltip("The collider used for range detection on the mine.")]
    public Collider Mine_DetectionCollider;
    [Tooltip("The GameObject used for the explosion.")]
    public GameObject Mine_ExplosionGameObject;
    [Tooltip("The Material applied when the bomb is armed.")]
    public Material Mine_ArmedMaterial;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Private Attributes                                                                                                                     * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    private float Arc_Height = 2.0f;        // the max height the mine reaches during travel when thrown (relative to spawn height)
    private float Arc_Duration = 1.0f;      // the amount of time in seconds it takes for the mine to reach its target
    private Vector3 Arc_StartPosition;
    private Vector3 Arc_TargetPosition;
    private float Arc_ElapsedTime = 0.0f;

    private enum MineState
    {
        ArcMovement,
        Arming,
        Armed,
        Triggered,
        Exploding,
        Disabled
    }

    private MineState Mine_CurrentState = MineState.Disabled;

    private float Mine_TimeToArm_ElapsedTime = 0.0f;
    private float Mine_TimeToExplode_ElapsedTime = 0.0f;
    private float Mine_DurationOfExplosion_ElapsedTime = 0.0f;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Initialization                                                                                                                         * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Initialize_Mine(ProjectilePool Projectile_Pool, Vector3 Arc_NewTarget, float Arc_NewHeight, float Arc_NewDuration)
    {
        InitializeArcMovement(Arc_NewTarget, Arc_NewHeight, Arc_NewDuration);

        // 
        ExplosionActive(false);

        // Animation Stuff
        Animation_Reset();

        // Run base Initialize() function
        Initialize(Projectile_Pool);
    }

    public void InitializeArcMovement(Vector3 Arc_NewTarget, float Arc_NewHeight, float Arc_NewDuration)
    {
        Arc_Height = Arc_NewHeight;
        Arc_Duration = Arc_NewDuration;
        Arc_StartPosition = transform.position;
        Arc_TargetPosition = Arc_NewTarget;
        Arc_ElapsedTime = 0.0f;
        Mine_CurrentState = MineState.ArcMovement;
        Mine_TimeToArm_ElapsedTime = 0.0f;
        Mine_TimeToExplode_ElapsedTime = 0.0f;
        Mine_DurationOfExplosion_ElapsedTime = 0.0f;
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Update Functions                                                                                                                       * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // FixedUpdate is called at set intervals
    protected override void Update()
    {
        if (Mine_CurrentState == MineState.Arming)
        {
            Arming();
        }
        else if (Mine_CurrentState == MineState.Triggered)
        {
            Triggered();
        }
        else if (Mine_CurrentState == MineState.Exploding) 
        {
            Exploding();
        }
    }

    protected override void FixedUpdate()
    {
        if (Mine_CurrentState == MineState.ArcMovement)
        {
            MoveProjectileInArc();
        }
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Mine Behaviour Functions                                                                                                               * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Exploding()
    {
        Mine_DurationOfExplosion_ElapsedTime += Time.deltaTime;

        if (Mine_DurationOfExplosion_ElapsedTime >= Mine_DurationOfExplosion)
        {
            ExplosionActive(false);
            Mine_CurrentState = MineState.Disabled;
            ReturnToPool();
        }
    }

    private void Arming()
    {
        Mine_TimeToArm_ElapsedTime += Time.deltaTime;

        if (Mine_TimeToArm_ElapsedTime >= Mine_TimeToArm)
        {
            Mine_CurrentState = MineState.Armed;
            Animation_Armed();
        }
    }

    private void Triggered()
    {
        Mine_TimeToExplode_ElapsedTime += Time.deltaTime;

        if (Mine_TimeToExplode_ElapsedTime >= Mine_TimeToExplode)
        {
            Mine_CurrentState = MineState.Exploding;
            ExplosionActive(true);
        }
    }

    private void ExplosionActive(bool active)
    {
        Mine_ExplosionGameObject.SetActive(active);
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Mine Trigger Functions                                                                                                                 * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (Mine_CurrentState != MineState.Armed) return;

        if (other.CompareTag("Player"))
        {
            Mine_CurrentState = MineState.Triggered;
        }
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Movement Functions                                                                                                                     * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    private void MoveProjectileInArc()
    {
        Arc_ElapsedTime += Time.fixedDeltaTime;

        float t = Mathf.Clamp01(Arc_ElapsedTime / Arc_Duration);    // normalize between 0 and 1

        // Interpolate XZ position (horizontal movement)
        Vector3 NewPosition = Vector3.Lerp(Arc_StartPosition, Arc_TargetPosition, t);

        // Compute height using a simple parabola formula
        float PeakHeight = Arc_Height * (1 - 4 * (t - 0.5f) * (t - 0.5f));
        NewPosition.y = Mathf.Lerp(Arc_StartPosition.y, Arc_TargetPosition.y, t) + PeakHeight;

        // Move Mine
        transform.position = NewPosition;

        if (t >= 1.0f)
        {
            Mine_CurrentState = MineState.Arming;
        }
    }
    
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Animation Functions                                                                                                                    * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    private void Animation_Reset()
    {
        Animation_ResetMaterial();
    }

    private void Animation_Armed()
    {
        Animation_UpdateMaterial(Mine_ArmedMaterial);
    }
}
