using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Bullet : Projectile
{
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Public Attributes                                                                                                                      * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    [Tooltip("Speed at which the projectile moves in Projectile_Direction.")]
    public float Projectile_Speed = 10f;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Private Attributes                                                                                                                     * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    private Vector3 Projectile_Direction;
    private bool Projectile_IsDeflectable = false;
    private bool Projectile_HasBeenDeflected = false;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Initialization                                                                                                                         * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Initialize_Bullet(ProjectilePool Projectile_Pool, Vector3 New_Direction)
    {
        // Run base Initialize() function
        Initialize(Projectile_Pool);

        // Set the target direction of the projectile
        Projectile_Direction = New_Direction.normalized;

        // Reset Projectile_IsDeflectable to false
        Projectile_IsDeflectable = false;
        Projectile_HasBeenDeflected = false;
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
    protected override void Update()
    {
        // do nuffin'
    }

    protected override void FixedUpdate()
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

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Deflection Functions                                                                                                                   * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    public bool Deflection_IsDeflectable()
    {
        return Projectile_IsDeflectable;
    }

    public bool Deflection_HasBeenDeflected()
    {
        return Projectile_HasBeenDeflected;
    }

    public void Deflection_Perform()
    {
        Projectile_Direction = -Projectile_Direction;
        Projectile_HasBeenDeflected = true;
    }
}
