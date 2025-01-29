using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Public Fields                                                                                                                                                                                * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    [Tooltip("Pool from which projectiles are taken.")]
    public ProjectilePool Spawner_ProjectilePool;
    [Tooltip("Location projectile is fired from.")]
    public Transform Spawner_FirePoint;
    [Tooltip("Numer of projectile being fired a second.")]
    public float Spawner_FireRate = 1f;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Private/Protected Attributes                                                                                                                                                                 * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private float Spawner_NextFireTime;     // when the next projectile should be fired

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Start Function                                                                                                                                                                               * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Update Function                                                                                                                                                                              * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {
        HandleProjectileSpawning();
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Fixed Update Function                                                                                                                                                                        * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // FixedUpdate is called at set intervals
    void FixedUpdate()
    {
        
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Spawning Logic Function                                                                                                                                                                      * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void HandleProjectileSpawning()
    {
        if (Time.time >= Spawner_NextFireTime)
        {
            SpawnProjectile();
            Spawner_NextFireTime = Time.time + 1f / Spawner_FireRate;
        }
    }

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
}
