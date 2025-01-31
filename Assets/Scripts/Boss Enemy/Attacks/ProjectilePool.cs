using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Public Fields                                                                                                                                                                                * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    [Tooltip("Projectile prefab.")]
    public GameObject Projectile_Prefab;
    [Tooltip("Number of projectiles available in pool.")]
    public int Pool_Size = 20;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Private/Protected Attributes                                                                                                                                                                 * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private Queue<GameObject> Pool_Projectiles = new Queue<GameObject>();   // this is the pool of projectiles that the spawner can use

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Start Function                                                                                                                                                                               * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        // Instatiate the Pool_Projectiles queue
        for (int i = 0; i < Pool_Size; i++)
        {
            GameObject NewProjectile = Instantiate(Projectile_Prefab);  // create new projectile game object
            NewProjectile.SetActive(false);                             // set it to false
            Pool_Projectiles.Enqueue(NewProjectile);                    // add it to the Pool_Projectiles queue
        }
    }
    
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Pool_Projectiles Queue Functions                                                                                                                                                             * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Returns (and takes out) the next Projectile in the Pool_Projectiles queue
    public GameObject GetNextProjectile()
    {
        GameObject NextProjectile = Pool_Projectiles.Dequeue();
        NextProjectile.SetActive(true);
        return NextProjectile;
    }

    // Puts a projectile back into the Pool_Projectiles queue
    public void ReturnProjectileToPool(GameObject ReturningProjectile)
    {
        ReturningProjectile.SetActive(false);
        Pool_Projectiles.Enqueue(ReturningProjectile);
    }

    public bool CheckIfAllProjectilesHaveBeenReturnedToQueue()
    {
        if (Pool_Projectiles.Count == Pool_Size)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Update Projectiles in Pool_Projectiles Queue Functions                                                                                                                                       * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void UpdateAllProjectileValues(float New_ProjectileSpeed, float New_ProjectileLifetime)
    {
        UpdateProjectileSpeed(New_ProjectileSpeed);
        UpdateProjectileLifetime(New_ProjectileLifetime);
    }

    public void UpdateProjectileSpeed(float New_ProjectileSpeed)
    {
        foreach (GameObject projectile in Pool_Projectiles)
        {
            projectile.GetComponent<Projectile>().UpdateProjectileSpeed(New_ProjectileSpeed);
        }
    }

    public void UpdateProjectileLifetime(float New_ProjectileLifetime)
    {
        foreach (GameObject projectile in Pool_Projectiles)
        {
            projectile.GetComponent<Projectile>().UpdateProjectileLifetime(New_ProjectileLifetime);
        }
    }
}
