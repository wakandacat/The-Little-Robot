using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Public Fields                                                                                                                                                                                * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    [Tooltip("Projectile prefab for this pool.")]
    public GameObject Projectile_Prefab;
    [Tooltip("The total number of projectiles available in pool.")]
    public int Total_Pool_Size = 20;
    [Tooltip("The number of projectiles immediately available in pool.")]
    public int Starting_Pool_Size = 10;
    [Tooltip("The number of projectiles instantiated into the pool every second.")]
    public int Pool_Instantiation_PerSecond = 1;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Private/Protected Attributes                                                                                                                                                                 * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    private Queue<GameObject> Pool_Projectiles = new Queue<GameObject>();       // this is the pool of projectiles that the spawner can use
    private List<GameObject> Pool_ActiveProjectiles = new List<GameObject>();   // this list contains all projectiles that have been fired and are no longer in the pool ("active")

    private string Pool_ID;

    private ProjectileSpawner Spawner;

    private bool Pool_IsReady = false;

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Start Function                                                                                                                                                                               * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        // Instatiate the initial Pool_Projectiles queue
        for (int i = 0; i < Starting_Pool_Size; i++)
        {
            InstantiateProjectile();
        }

        // Start coroutine to gradually fill the pool
        yield return StartCoroutine(GraduallyFillPool());   // fill the pool completely before using it
        Pool_IsReady = true;                                // set a flag to indicate that the pool is ready for use
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Projectile Instantiation Functions                                                                                                                                                           * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Instantiates a projectile and adds it to the pool
    private void InstantiateProjectile()
    {
        if (Pool_Projectiles.Count + Pool_ActiveProjectiles.Count >= Total_Pool_Size)
        {
            return; // stop if we've reached the limit
        }

        GameObject NewProjectile = Instantiate(Projectile_Prefab, transform);   // create new projectile game object, make the gameobject this script is attached to the parent
        NewProjectile.SetActive(false);                                         // set it to false
        Pool_Projectiles.Enqueue(NewProjectile);                                // add it to the Pool_Projectiles queue

        //Debug
        //Debug.Log("BossEnemy: Instantiated " + (Pool_Projectiles.Count + Pool_ActiveProjectiles.Count) + " out of " + Total_Pool_Size + " projectiles.");
    }

    // Slowly instantiates projectiles into the pool over a rate of Pool_Instantiation_PerSecond
    private IEnumerator GraduallyFillPool()
    {
        while (Pool_Projectiles.Count + Pool_ActiveProjectiles.Count < Total_Pool_Size)
        {
            yield return new WaitForSeconds(1f / Pool_Instantiation_PerSecond); // wait based on the instantiation rate
            InstantiateProjectile();
        }
    }


    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Pool_Projectiles Queue Functions                                                                                                                                                             * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Returns (and takes out) the next Projectile in the Pool_Projectiles queue
    public GameObject GetNextProjectile()
    {
        //Debug
        //Debug.Log("BossEnemy: There are " + Pool_ActiveProjectiles.Count + " active projectiles and " + Pool_Projectiles.Count + " projectiles in the pool");

        // check if queue is empty, if so return check oldest projectile lifetime completion
        if (Pool_Projectiles.Count == 0)
        {
            ReturnOldestActiveProjectileToPool();
        }

        // retrieve next available projectile from pool
        GameObject NextProjectile = Pool_Projectiles.Dequeue();     // remove new projectile from pool
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

    //public bool CheckIfAllProjectilesHaveBeenReturnedToQueue()
    //{
    //    if (Pool_Projectiles.Count == Pool_Size)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    public void ReturnOldestActiveProjectileToPool()
    {
        if (Pool_ActiveProjectiles.Count > 0)
        {
            //Debug.Log("ProjectilePool: Oldest Active Projectile Returned To Pool");
            GameObject oldestProjectile = Pool_ActiveProjectiles[0];    // get the first (oldest) projectile in the list
            ReturnProjectileToPool(oldestProjectile);                   // return it to the pool
        }
    }

    public float Return_OldestActiveProjectile_Lifetime_AsPercentage()
    {
        if (Pool_ActiveProjectiles.Count > 0)
        {
            //Debug.Log("ProjectilePool: Oldest Active Projectile Returned To Pool");
            GameObject oldestProjectileGameObject = Pool_ActiveProjectiles[0];    // get the first (oldest) projectile in the list
            Projectile oldestProjectile = oldestProjectileGameObject.GetComponent<Projectile>();
            return oldestProjectile.Get_ProjectileLifetime_AsPercentage();
        }
        return 0.0f;
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Update Projectiles in Pool_Projectiles Queue Functions                                                                                                                                       * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Set_All_ProjectileLifetime(float New_ProjectileLifetime)
    {
        foreach (GameObject projectile in Pool_Projectiles)
        {
            projectile.GetComponent<Projectile>().Set_ProjectileLifetime(New_ProjectileLifetime);
        }
    }

    public void Set_Bullet_ProjectileSpeed(float New_ProjectileSpeed)
    {
        foreach (GameObject projectile in Pool_Projectiles)
        {
            projectile.GetComponent<Projectile_Bullet>().Set_ProjectileSpeed(New_ProjectileSpeed);
        }
    }

    public void Set_Bullet_ProjectileDeflectable(bool New_DeflectableStaus)
    {
        foreach (GameObject projectile in Pool_Projectiles)
        {
            projectile.GetComponent<Projectile_Bullet>().Set_ProjectileDeflectable(New_DeflectableStaus);
        }
    }

    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // *               Misc. Functions                                                                                                                                                                              * 
    // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void Set_Pool_ID(string newID)
    {
        Pool_ID = newID;
    }

    public void Set_Spawner(ProjectileSpawner newSpawner)
    {
        Spawner = newSpawner;
    }

    public bool IsPoolFinishedFilling()
    {
        return Pool_IsReady;
    }
}
