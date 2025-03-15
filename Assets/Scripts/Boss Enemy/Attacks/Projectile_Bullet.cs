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
    public float Projectile_Deflected_SpeedMultiplier = 1.5f;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Private Attributes                                                                                                                     * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    private Vector3 Projectile_Direction;
    private bool Projectile_IsDeflectable = false;
    private bool Projectile_HasBeenDeflected = false;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Initialization                                                                                                                         * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Initialize_Bullet(ProjectilePool Projectile_Pool, Vector3 New_Direction, bool New_DeflectableStaus)
    {
        // Set the target direction of the projectile
        Projectile_Direction = New_Direction.normalized;

        // Reset Projectile_IsDeflectable to false
        Projectile_IsDeflectable = New_DeflectableStaus;
        if (Projectile_IsDeflectable)
        {
            //Animation_UpdateMaterialColor(Color.yellow);
        }
        else
        {
            //Animation_ResetMaterialColor();
        }
        Projectile_HasBeenDeflected = false;

        // Run base Initialize() function
        Initialize(Projectile_Pool);
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Projectile Value Update Functions                                                                                                      * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Set_ProjectileSpeed(float New_ProjectileSpeed)
    {
        Projectile_Speed = New_ProjectileSpeed;
    }

    public void Set_ProjectileDeflectable(bool New_DeflectableStaus)
    {
        Projectile_IsDeflectable = New_DeflectableStaus;
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
        if (Deflection_HasBeenDeflected() == true)
        {
            transform.position += Projectile_Direction * Projectile_Speed * Time.fixedDeltaTime * Projectile_Deflected_SpeedMultiplier;    // move towards Projectile_Direction at Projectile_Speed * Projectile_Deflected_SpeedMultiplier per second
        }
        else
        {
            transform.position += Projectile_Direction * Projectile_Speed * Time.fixedDeltaTime;    // move towards Projectile_Direction at Projectile_Speed per second
        }
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
        Debug.Log("Projectile_Bullet: Deflection Has Occured!");
    }
}
