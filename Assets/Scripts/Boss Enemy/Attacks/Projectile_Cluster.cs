using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Cluster : MonoBehaviour
{
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Public Attributes                                                                                                                      * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    [Tooltip("Period of time that the projectile can exist before it is deactivated.")]
    public float Projectile_Lifetime = 5f;
    [Tooltip("Speed at which the projectile moves in Projectile_Direction.")]
    public float Projectile_Speed = 0f;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Private Attributes                                                                                                                     * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    private Vector3 Projectile_Direction;
    private float Projectile_FireTime;

    private Coroutine Coroutine_DestroyProjectile;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Initialization                                                                                                                         * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // Initializes the Projectile Game Object
    public void Initialize(Vector3 New_Direction, float New_Speed, float New_Lifetime)
    {
        // Set the target direction of the projectile
        Projectile_Direction = New_Direction.normalized;

        // Set the speed of the projectile
        Projectile_Speed = New_Speed;

        // Set the lifetime of the projectile
        Projectile_Lifetime = New_Lifetime;

        this.gameObject.SetActive(true);

        // Stop previous coroutine if active
        if (Coroutine_DestroyProjectile != null)
        {
            StopCoroutine(Coroutine_DestroyProjectile);
        }

        Projectile_FireTime = Time.time; // get the current time when the projectile is initialized (fired)

        Coroutine_DestroyProjectile = StartCoroutine(AutoDestroy());
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Coroutine Functions                                                                                                                    * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator AutoDestroy()
    {
        yield return new WaitForSeconds(Projectile_Lifetime);
        Destroy(gameObject);
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Projectile Value Get/Set Functions                                                                                                     * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Set_ProjectileLifetime(float New_ProjectileLifetime)
    {
        Projectile_Lifetime = New_ProjectileLifetime;
    }

    // Function to get the percentage of lifetime completed (0.0f - 1.0f)
    public float Get_ProjectileLifetime_AsPercentage()
    {
        if (Projectile_Lifetime <= 0) return 0f; // prevent division by zero
        return Mathf.Clamp01((Time.time - Projectile_FireTime) / Projectile_Lifetime);
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Projectile Value Update Functions                                                                                                      * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Set_ProjectileSpeed(float New_ProjectileSpeed)
    {
        Projectile_Speed = New_ProjectileSpeed;
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Update Functions                                                                                                                       * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // FixedUpdate is called at set intervals
    void Update()
    {
        // do nuffin'
    }

    void FixedUpdate()
    {
        MoveProjectile();
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Movement Functions                                                                                                                     * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    private void MoveProjectile()
    {
        transform.position += Projectile_Direction * Projectile_Speed * Time.fixedDeltaTime;    // move towards Projectile_Direction at Projectile_Speed per second
    }
}
