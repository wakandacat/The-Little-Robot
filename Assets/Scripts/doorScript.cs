using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorScript : MonoBehaviour
{

    private float timer = 0;
    private float timeStep = 0.01f;
    private bool movingUp = false;
    private bool movingDown = false;
    private Vector3 startPos;
    public float delay = 1.0f;
    public float timeToOpen = 1.0f;

    public void Awake()
    {
        startPos = this.transform.position;
    }

    public void FixedUpdate()
    {
        //if the flag to open is set, then start the process
        if (timer < (timeToOpen + delay) && movingUp)
        {
            //increment the timer
            timer += timeStep;

            //if the timer is greater than the delay time and not yet at the full time, then lerp the door open
            if (timer >= delay && timer <= (timeToOpen + delay))
            {
                openDoor();
            }

            //if the timer is past the time + delay then reset the flag
            if (timer >= (timeToOpen + delay))
            {
                movingUp = false;
                startPos = this.transform.position;
            }
        }
        else if (timer < timeToOpen && movingDown)
        {
            timer += timeStep;
            closeDoor();

            if (timer >= timeToOpen)
            {
                movingDown = false;
                startPos = this.transform.position;
            }
        }
        else
        {
            timer = 0;
        }
    }

    public void openDoor()
    {
        movingUp = true;

        float newY = Mathf.Lerp(startPos.y, startPos.y + 5f, (timer - delay));
        this.transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    public void closeDoor()
    {
        movingDown = true;

        float newY = Mathf.Lerp(startPos.y, startPos.y - 5f, timer);
        this.transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
