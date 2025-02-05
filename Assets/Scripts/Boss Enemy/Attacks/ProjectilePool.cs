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
    private Queue<GameObject> Pool_Projectiles = new Queue<GameObject>();       // this is the pool of projectiles that the spawner can use
    private List<GameObject> Pool_ActiveProjectiles = new List<GameObject>();   // this list contains all projectiles that have been fired and are no longer in the pool ("active")

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
        // check if queue is empty, if so return oldest projectile
        if (Pool_Projectiles.Count == 0)
        {
            ReturnOldestActiveProjectileToPool();
        }

        GameObject NextProjectile = Pool_Projectiles.Dequeue();     // remove new projectile from pool
        NextProjectile.SetActive(true);
        Pool_ActiveProjectiles.Add(NextProjectile);                 // add new projectile to active list
        return NextProjectile;
    }

    // Puts a projectile back into the Pool_Projectiles queue
    public void ReturnProjectileToPool(GameObject ReturningProjectile)
    {
        ReturningProjectile.SetActive(false);
        Pool_Projectiles.Enqueue(ReturningProjectile);          // add returning projectile to pool
        Pool_ActiveProjectiles.Remove(ReturningProjectile);     // remove returning projectile from active list
    }

    //
    public void ReturnAllProjectilesToPool()
    {
        foreach (GameObject Projectile in new List<GameObject>(Pool_ActiveProjectiles))
        {
            ReturnProjectileToPool(Projectile);
        }
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

    public void ReturnOldestActiveProjectileToPool()
    {
        if (Pool_ActiveProjectiles.Count > 0)
        {
            Debug.Log("ProjectilePool: Oldest Active Projectile Returned To Pool");
            GameObject oldestProjectile = Pool_ActiveProjectiles[0];    // get the first (oldest) projectile in the list
            ReturnProjectileToPool(oldestProjectile);                   // Return it to the pool
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
