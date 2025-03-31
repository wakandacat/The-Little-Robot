using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class movingPlatformScript : MonoBehaviour
{
    //to use: ensure the platformGeo has the "ground" tag on it so the player can jump off
    //ensure the platformGeo is of scale (1,1,1) so the player does not distort when on top
    //the platformPath holds a bunch of empty gameobjects that you can select and manually move to where you want the platform to go
    //you should be able to add as many platformPath objects as you want
    //KEEP THE ORDER OF ELEMENTS IN THE PREFAB ALWAYS IN THE SAME ORDER: GEO ABOVE PATHS
    //you can modify the speed variable in the outliner


    //referenced this tutorial: https://www.youtube.com/watch?v=ly9mK0TGJJo
    //get the elements within this gameobject 
    //first child is the platform itself
    private GameObject platform;

    //second object is the waypoint object with waypoint location children
    private Transform waypointParent;

    //platform vars
    public float speed = 5;
    private int targetWaypointIndex;
    private float timeToPoint;
    private float elapsedTime;

    //get two waypoints to move smoothly between them
    private Transform targetWaypoint;
    private Transform prevWaypoint;

    //fungus bool
    public bool isFungus = false;

    //jitter
    private Vector3 startPos;
    public Coroutine jitterPlat;
    private float minJitter = 0.05f;
    private float maxJitter = 0.1f;

    void Awake()
    {
        platform = transform.GetChild(0).gameObject;
        waypointParent = transform.GetChild(1);
        targetWaypointIndex = 0;

        startPos = this.transform.position; // for jitter
        jitterPlat = StartCoroutine(JitterPlatform());
    }

    void Start()
    {
        UpdateWaypoints();
    }

    //jitter teh door while it has fungus on it
    public IEnumerator JitterPlatform()
    {
        //if we have fungus and we have the next scene loaded
        while (true)
        {
            if (isFungus == true)
            {
                //move plat left
                this.transform.GetChild(0).transform.position = new Vector3(startPos.x + Random.Range(minJitter, maxJitter), startPos.y, startPos.z);
                yield return new WaitForSeconds(Random.Range(minJitter, maxJitter));
                startPos = this.transform.GetChild(0).transform.position;

                //move plat right
                this.transform.GetChild(0).transform.position = new Vector3(startPos.x - Random.Range(minJitter, maxJitter), startPos.y, startPos.z);
                yield return new WaitForSeconds(Random.Range(minJitter, maxJitter));
                startPos = this.transform.GetChild(0).transform.position;
            }
            else
            {
                yield return null; //wait
            }
        }
    }


    void FixedUpdate()
    {

        if (isFungus == false)
        {
            elapsedTime += Time.deltaTime;
            float elapsedPercent = elapsedTime / timeToPoint;

            //optional smoothing when closer to a waypoint
            elapsedPercent = Mathf.SmoothStep(0, 1, elapsedPercent);

            //lerp the platform
            platform.transform.position = Vector3.Lerp(prevWaypoint.position, targetWaypoint.position, elapsedPercent);

            //move onto the next waypoint once the platform is at destination
            if (elapsedPercent >= 1)
            {
                UpdateWaypoints();
            }
        }

    }

    //get the next waypoint child INDEX and loop if we've hit the max
    public int GetNextWaypoint(int currIndex)
    {
        int nextWayPoint = currIndex + 1;

        //loop the indeces
        if (nextWayPoint == waypointParent.childCount)
        {
            nextWayPoint = 0;
        }

        return nextWayPoint;
    }

    public void UpdateWaypoints()
    {
        //get current waypoint and set it to previous
        prevWaypoint = waypointParent.GetChild(targetWaypointIndex);

        //get the next waypoint
        targetWaypointIndex = GetNextWaypoint(targetWaypointIndex);
        targetWaypoint = waypointParent.GetChild(targetWaypointIndex);
        elapsedTime = 0;

        //distance between the two waypoints
        float distanceBetweenPoints = Vector3.Distance(prevWaypoint.position, targetWaypoint.position);
        timeToPoint = distanceBetweenPoints / speed;
    }
}
