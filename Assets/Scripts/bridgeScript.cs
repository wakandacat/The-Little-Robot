using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bridgeScript : MonoBehaviour
{

    private float timer = 0;
    private float timeStep = 0.01f;
    private bool movingLeft = false;
    private bool movingRight = false;
    private Vector3 startPos;


    public void Awake()
    {
        startPos = this.transform.position;
    }

    public void FixedUpdate()
    {
        if (timer < 1 && movingLeft) {
            timer += timeStep;
            //Debug.Log("moving bridge left");
            moveBridgeLeft();

            if (timer >= 1)
            {
                movingLeft = false;
                startPos = this.transform.position;
            }
        } 
        else if (timer < 1 && movingRight)
        {
            timer += timeStep;
            //Debug.Log("moving bridge right");
            moveBridgeRight();

            if (timer >= 1)
            {
                movingRight = false;
                startPos = this.transform.position;
            }
        } 
        else
        {
            timer = 0;
        }
    }

    public void moveBridgeLeft()
    {
        movingLeft = true;
        // Debug.Log("hiiiii");
        float newZ = Mathf.Lerp(startPos.z, startPos.z - 20f, timer);
        this.transform.position = new Vector3(startPos.x, startPos.y, newZ);
    }

    public void moveBridgeRight()
    {
        movingRight = true;
        // Debug.Log("byeeee");
        float newZ = Mathf.Lerp(startPos.z, startPos.z + 20f, timer);
        this.transform.position = new Vector3(startPos.x, startPos.y, newZ);
    }
}
