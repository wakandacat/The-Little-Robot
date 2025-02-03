using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               INSTRUCTIONS FOR USE                                                                                                                                                                         * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // You must start by running UpdateSpawner_AllValues()
    // This will set how many times the spawner should attack and how often per second
    // You should run UpdateAllProjectileValues(float New_ProjectileSpeed, float New_ProjectileLifetime) to ensure the projectiles are how you want them
    // When the attack is ready to begin you can run StartAttack()
    // Then check on each frame ReturnSpawnerActive() to determine if the attack has been completed
    // If it is true then the attack is still occuring and if it is false then the attack is over
    // The attack can be ended early by running EndAttack()
    // While the attack is occuring, check if it is time to execute the next projectile spawn by running IsSpawnerReadyToFire()
    // If it returns true then you must run PreAttackLogic() and then you can execute the next spawn of projectiles
    // Finally run PostAttackLogic() to ensure that the attack ends when there are no more projectiles left to shoot and the Spawner_RemainingAttackCount is decrementing
    // OPTIONALLY: also run CheckIfCheckIfAllProjectilesHaveBeenReturnedToQueue() to ensure all projectiles have met their lifetime and been returned to their pool 
    // Misc. Functions:
    // ReturnAllProjectilesToPool() can be run before an attack begins to ensure the list of projectiles is refreshed

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Public Fields                                                                                                                                                                                * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    [Tooltip("Unique name for spawner to use.")]
    public string Spawner_ID;
    [Tooltip("Pool from which projectiles are taken.")]
    public ProjectilePool Spawner_ProjectilePool;
    [Tooltip("Location projectile is fired from.")]
    public GameObject Spawner_FirePoint_GameObject;
    [Tooltip("Numer of projectile being fired a second.")]
    public float Spawner_FireRate = 1f;
    [Tooltip("Numer of projectiles/attack sequences to be fired.")]
    public int Spawner_RemainingAttackCount = 1;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Private/Protected Attributes                                                                                                                                                                 * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private Transform Spawner_FirePoint;            // transform for the spawner fire point game object
    
    private float Spawner_NextFireTime;             // when the next projectile should be fired

    private bool Spawner_Active = false;            // whether the spawner should be spawning new projectiles or not

    private bool Spawner_TrackingHorizontal = false;    // whether the spawner should be rotating to track the player along the Y-AXIS
    private bool Spawner_TrackingVertical = false;      // whether the spawner should be rotating to track the player along the X-AXIS
    private float Spawner_TrackingSpeed = 0.0f;         // speed of rotation towards the players position when tracking (degrees per second)

    // Game Object References
    GameObject Spawner_Player_GameObject;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Start Function                                                                                                                                                                               * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        // Setup Private Attributes
        Spawner_FirePoint = Spawner_FirePoint_GameObject.transform;

        // Set Object References
        Spawner_Player_GameObject = GameObject.FindGameObjectWithTag("Player");
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Update Function                                                                                                                                                                              * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Fixed Update Function                                                                                                                                                                        * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // FixedUpdate is called at set intervals
    void FixedUpdate()
    {
        // Spawner Tracking for Player
        Tracking_Horizontal(Spawner_Player_GameObject);
        Tracking_Vertical(Spawner_Player_GameObject);
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Spawner Update Functions                                                                                                                                                                     * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void UpdateSpawner_AllValues(float New_SpawnerFireRate, int New_SpawnerRemainingAttackCount, bool New_SpawnerTrackingHorizontal, bool New_SpawnerTrackingVertical, float New_TrackingSpeed)
    {
        Spawner_FireRate = New_SpawnerFireRate;
        Spawner_RemainingAttackCount = New_SpawnerRemainingAttackCount;
        UpdateSpawner_Tracking(New_SpawnerTrackingHorizontal, New_SpawnerTrackingVertical, New_TrackingSpeed);
    }

    // Updates Spawner_NextFireTime to equal the current time plus (1.0f / Spawner_FireRate)
    public void UpdateSpawner_NextFireTime()
    {
        Spawner_NextFireTime = Time.time + 1f / Spawner_FireRate;
    }

    public void UpdateSpawner_Tracking(bool New_SpawnerTrackingHorizontal, bool New_SpawnerTrackingVertical, float New_TrackingSpeed)
    {
        Spawner_TrackingHorizontal = New_SpawnerTrackingHorizontal;
        Spawner_TrackingVertical = New_SpawnerTrackingVertical;
        Spawner_TrackingSpeed = New_TrackingSpeed;
    }

    public void Update_FirePointPosition(Vector3 NewPosition)
    {
        Spawner_FirePoint.transform.position = NewPosition;
    }

    // update position for the fire point with optional positional values (pass 'null' to not change a value when calling)
    public void Update_FirePointPosition(float? x = null, float? y = null, float? z = null)
    {
        // get the current position
        Vector3 currentPosition = Spawner_FirePoint.transform.position;

        // update the position only if the value is provided
        float newX = x.HasValue ? x.Value : currentPosition.x;
        float newY = y.HasValue ? y.Value : currentPosition.y;
        float newZ = z.HasValue ? z.Value : currentPosition.z;

        // apply the updated position
        Spawner_FirePoint.transform.position = new Vector3(newX, newY, newZ);
    }

    public void Reset_FirePointPositionToGameObject()
    {
        Update_FirePointPosition(Spawner_FirePoint_GameObject.transform.position);
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Projectile Update Functions                                                                                                                                                                  * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void UpdateAllProjectileValues(float New_ProjectileSpeed, float New_ProjectileLifetime)
    {
        Spawner_ProjectilePool.UpdateProjectileSpeed(New_ProjectileSpeed);
        Spawner_ProjectilePool.UpdateProjectileLifetime(New_ProjectileLifetime);
    }

    public void UpdateProjectileSpeed(float New_ProjectileSpeed)
    {
        Spawner_ProjectilePool.UpdateProjectileSpeed(New_ProjectileSpeed);
    }

    public void UpdateProjectileLifetime(float New_ProjectileLifetime)
    {
        Spawner_ProjectilePool.UpdateProjectileLifetime(New_ProjectileLifetime);
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Spawner Movement Functions                                                                                                                                                                   * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Tracking_ResetRotationToZero()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void Tracking_Horizontal(GameObject ObjectToFollow)
    {
        if (Spawner_TrackingHorizontal && ObjectToFollow != null)
        {
            // get the direction to the target, ignoring vertical difference (Y-AXIS rotation only)
            Vector3 directionToTarget = ObjectToFollow.transform.position - Spawner_FirePoint.position; // Change 'transform.position' to 'Spawner_FirePoint.position'
            directionToTarget.y = 0; // lock Y-AXIS rotation for horizontal tracking

            if (directionToTarget != Vector3.zero) // not already looking at target
            {
                // get the desired rotation towards the target
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

                // turn towards player position at a speed of Spawner_TrackingSpeed
                Spawner_FirePoint.rotation = Quaternion.RotateTowards(Spawner_FirePoint.rotation, targetRotation, Spawner_TrackingSpeed * Time.fixedDeltaTime); // Update Spawner_FirePoint.rotation
            }
        }
    }

    public void Tracking_Vertical(GameObject ObjectToFollow)
    {
        if (Spawner_TrackingVertical && ObjectToFollow != null)
        {
            // get the direction to the target, ignoring horizontal difference (X-AXIS rotation only)
            Vector3 directionToTarget = ObjectToFollow.transform.position - Spawner_FirePoint.position; // Change 'transform.position' to 'Spawner_FirePoint.position'
            directionToTarget.x = 0; // lock X-AXIS rotation for vertical tracking

            if (directionToTarget != Vector3.zero) // not already looking at target
            {
                // get the desired rotation towards the target
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

                // turn towards player position at a speed of Spawner_TrackingSpeed
                Spawner_FirePoint.rotation = Quaternion.RotateTowards(Spawner_FirePoint.rotation, targetRotation, Spawner_TrackingSpeed * Time.fixedDeltaTime); // Update Spawner_FirePoint.rotation
            }
        }
    }

    public Transform ReturnSpawnerTransform()
    {
        return Spawner_FirePoint;
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Spawner Logic Functions                                                                                                                                                                      * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void StartAttack()
    {
        UpdateSpawner_NextFireTime();
        Spawner_Active = true;
    }

    public void EndAttack()
    {
        Spawner_Active = false;
    }

    public bool ReturnSpawnerActive()
    {
        return Spawner_Active;
    }

    // Is used to determine if the spawner is ready to fire the next attack
    public bool IsSpawnerReadyToFire()
    {
        if (Spawner_Active)
        {
            if (Spawner_RemainingAttackCount > 0)
            {
                if (Time.time + 0.01f >= Spawner_NextFireTime)
                {
                    return true;
                }
            }
        }
        return false;
    }

    // This should be run before an attack occurs to ensure that the next fire time is being set properly
    public void PreAttackLogic()
    {
        
    }

    // This should be run after an attack occurs to ensure that the spawner is correctly deactivated and that the Spawner_RemainingAttackCount is decrementing
    public void PostAttackLogic()
    {
        Spawner_RemainingAttackCount--;
        UpdateSpawner_NextFireTime();
        if (Spawner_Active)
        {
            if (Spawner_RemainingAttackCount <= 0)
            {
                EndAttack();
            }
        }
    }

    public bool CheckIfCheckIfAllProjectilesHaveBeenReturnedToQueue()
    {
        return Spawner_ProjectilePool.CheckIfAllProjectilesHaveBeenReturnedToQueue();
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Projectile Pool Functions                                                                                                                                                                    * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void ReturnAllProjectilesToPool()
    {
        Spawner_ProjectilePool.ReturnAllProjectilesToPool();
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Spawning Projectiles Functions                                                                                                                                                               * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Fires a single projectile towards where the spawner is pointing
    public void SpawnProjectile()
    {
        GameObject NewProjectile = Spawner_ProjectilePool.GetNextProjectile();
        NewProjectile.transform.position = Spawner_FirePoint.position;
        NewProjectile.transform.rotation = Spawner_FirePoint.rotation;
        NewProjectile.GetComponent<Projectile>().Initialize(Spawner_ProjectilePool, Spawner_FirePoint.forward);
    }

    // Fires Projectiles_Count projectiles spread over AngleOfSpread
    public void SpawnSpreadAttack(int Projectile_Count, float AngleOfSpread)
    {
        float startAngle = -AngleOfSpread / 2;
        float angleStep = AngleOfSpread / (Projectile_Count - 1);

        for (int i = 0; i < Projectile_Count; i++)
        {
            float angle = startAngle + angleStep * i;
            Quaternion New_Rotation = Quaternion.Euler(0, angle, 0) * Spawner_FirePoint.rotation;
            GameObject NewProjectile = Spawner_ProjectilePool.GetNextProjectile();
            NewProjectile.transform.position = Spawner_FirePoint.position;
            NewProjectile.transform.rotation = New_Rotation;
            NewProjectile.GetComponent<Projectile>().Initialize(Spawner_ProjectilePool, New_Rotation * Vector3.forward);
        }
    }

    // Fires Projectiles_Count projectiles spread over AngleOfSpread, stacked vertically Projectile_VerticalCount times spread across a distance of Spawner_MinHeight to Spawner_MaxHeight
    public void SpawnStackedSpreadAttack(int Projectile_Count, float AngleOfSpread, int Projectile_VerticalCount, float Spawner_MinHeight, float Spawner_MaxHeight)
    {
        float startAngle = -AngleOfSpread / 2;
        float angleStep = (Projectile_Count > 1) ? AngleOfSpread / (Projectile_Count - 1) : 0;
        float heightStep = (Projectile_VerticalCount > 1) ? (Spawner_MaxHeight - Spawner_MinHeight) / (Projectile_VerticalCount - 1) : 0;

        for (int v = 0; v < Projectile_VerticalCount; v++)
        {
            float heightOffset = Spawner_MinHeight + heightStep * v;

            for (int i = 0; i < Projectile_Count; i++)
            {
                float angle = startAngle + angleStep * i;
                Quaternion New_Rotation = Quaternion.Euler(0, angle, 0) * Spawner_FirePoint.rotation;
                GameObject NewProjectile = Spawner_ProjectilePool.GetNextProjectile();

                Vector3 spawnPosition = Spawner_FirePoint.position + new Vector3(0, heightOffset, 0);
                NewProjectile.transform.position = spawnPosition;
                NewProjectile.transform.rotation = New_Rotation;

                Vector3 direction = New_Rotation * Vector3.forward;

                NewProjectile.GetComponent<Projectile>().Initialize(Spawner_ProjectilePool, direction);
            }
        }
    }
}
