using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               INSTRUCTIONS FOR USE                                                                                                                                                                         * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // You must start by running UpdateAllSpawnerValues(float New_SpawnerFireRate, int New_SpawnerRemainingAttackCount)
    // This will set how many times the spawner should attack and how often per second
    // You should run UpdateAllProjectileValues(float New_ProjectileSpeed, float New_ProjectileLifetime) to ensure the projectiles are how you want them
    // When the attack is ready to begin you can run StartAttack()
    // Then check on each frame ReturnSpawnerActive to determine if the attack has been completed
    // This can be done with ReturnSpawnerActive()
    // If it is true then the attack is still occuring and if it is false then the attack is over
    // The attack can be ended early by running EndAttack()
    // While the attack is occuring, check if it is time to execute the next projectile spawn by running IsSpawnerReadyToFire()
    // If it returns true then you can execute the next spawn of projectiles
    // Finally run UpdateAttackStatus() to ensure that the attack ends when there are no more projectiles left to shoot
    // OPTIONALLY: also run CheckIfCheckIfAllProjectilesHaveBeenReturnedToQueue() to ensure all projectiles have met their lifetime and been returned to their pool 

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Public Fields                                                                                                                                                                                * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
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

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Start Function                                                                                                                                                                               * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        // Setup Private Attributes
        Spawner_FirePoint = Spawner_FirePoint_GameObject.transform;
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
        
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Spawner Update Functions                                                                                                                                                                     * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void UpdateAllSpawnerValues(float New_SpawnerFireRate, int New_SpawnerRemainingAttackCount)
    {
        Spawner_FireRate = New_SpawnerFireRate;
        Spawner_RemainingAttackCount = New_SpawnerRemainingAttackCount;
    }

    public void UpdateSpawnerNextFireTime()
    {
        Spawner_NextFireTime = Time.time + 1f / Spawner_FireRate;
    }

    public void StartAttack()
    {
        UpdateSpawnerNextFireTime();
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
    // *               Spawner Logic Functions                                                                                                                                                                      * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public bool IsSpawnerReadyToFire()
    {
        if (Spawner_Active)
        {
            if (Spawner_RemainingAttackCount > 0)
            {
                if (Time.time >= Spawner_NextFireTime)
                {
                    UpdateSpawnerNextFireTime();
                    return true;
                }
            }
        }
        return false;
    }

    public void UpdateAttackStatus()
    {
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
    // *               Spawning Projectiles Functions                                                                                                                                                               * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Fires a single projectile towards where the spawner is pointing
    void SpawnProjectile()
    {
        GameObject NewProjectile = Spawner_ProjectilePool.GetNextProjectile();
        NewProjectile.transform.position = Spawner_FirePoint.position;
        NewProjectile.transform.rotation = Spawner_FirePoint.rotation;
        NewProjectile.GetComponent<Projectile>().Initialize(Spawner_ProjectilePool, Spawner_FirePoint.forward);
    }

    // Fires Projectiles_Count projectiles spread over AngleOfSpread
    void SpawnSpreadAttack(int Projectile_Count, float AngleOfSpread)
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
    void SpawnStackedSpreadAttack(int Projectile_Count, float AngleOfSpread, int Projectile_VerticalCount, float Spawner_MinHeight, float Spawner_MaxHeight)
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
                NewProjectile.GetComponent<Projectile>().Initialize(Spawner_ProjectilePool, New_Rotation * Vector3.forward);
            }
        }
    }
}
