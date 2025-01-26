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

    public void Awake()
    {
        startPos = this.transform.position;
    }

    public void FixedUpdate()
    {
        if (timer < 1 && movingUp)
        {
            timer += timeStep;
            openDoor();

            if (timer >= 1)
            {
                movingUp = false;
                startPos = this.transform.position;
            }
        }
        else if (timer < 1 && movingDown)
        {
            timer += timeStep;
            closeDoor();

            if (timer >= 1)
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

        float newY = Mathf.Lerp(startPos.y, startPos.y + 5f, timer);
        this.transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    public void closeDoor()
    {
        movingDown = true;

        float newY = Mathf.Lerp(startPos.y, startPos.y - 5f, timer);
        this.transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
