using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// *               Abstract Class Projectile                                                                                                                                                                    * 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
public abstract class Projectile : MonoBehaviour
{
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Public Fields                                                                                                                          * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    [Tooltip("Period of time that the projectile can exist before it is deactivated.")]
    public float Projectile_Lifetime = 5f; 

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Private/Protected Attributes                                                                                                           * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    private ProjectilePool Projectile_HomePool;

    private Coroutine Coroutine_ReturnProjectileToPool;

    private Color Animation_OriginalMaterialColor;
    private Material Animation_OriginalMaterial;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Start Function                                                                                                                                                                               * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        // Set Object References
        Animation_OriginalMaterialColor = GetComponent<Renderer>().material.color;
        Animation_OriginalMaterial = GetComponent<Renderer>().material;
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Initialization                                                                                                                         * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // Initializes the Projectile Game Object
    protected void Initialize(ProjectilePool Projectile_Pool)
    {
        // Stop previous coroutine if active
        if (Coroutine_ReturnProjectileToPool != null)
        {
            StopCoroutine(Coroutine_ReturnProjectileToPool);
        }

        Coroutine_ReturnProjectileToPool = StartCoroutine(AutoReturnToPool());

        // Update Projectile_HomePool
        Projectile_HomePool = Projectile_Pool;
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Coroutine Functions                                                                                                                    * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    private IEnumerator AutoReturnToPool()
    {
        yield return new WaitForSeconds(Projectile_Lifetime);
        ReturnToPool();
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Projectile Value Update Functions                                                                                                      * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Set_ProjectileLifetime(float New_ProjectileLifetime)
    {
        Projectile_Lifetime = New_ProjectileLifetime;
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Update Functions                                                                                                                       * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // FixedUpdate is called at set intervals
    protected abstract void Update();

    // FixedUpdate is called at set intervals
    protected abstract void FixedUpdate();

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Destroy Functions                                                                                                                      * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    public void ReturnToPool()
    {
        Projectile_HomePool.ReturnProjectileToPool(gameObject);
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Animation Functions                                                                                                                    * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------
    protected void Animation_UpdateMaterialColor(Color newColor)
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material.color = newColor;
        }
    }

    protected void Animation_ResetMaterialColor()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material.color = Animation_OriginalMaterialColor;
        }
    }
    protected void Animation_UpdateMaterial(Material newMaterial)
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null && newMaterial != null)
        {
            renderer.material = newMaterial;
        }
    }

    protected void Animation_ResetMaterial()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material = Animation_OriginalMaterial;
        }
    }
}