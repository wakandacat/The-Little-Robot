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
    public float delay = 0.7f;
    public float timeToOpen = 1.0f;
    public bool doorOpen = false;

    public bool isFungus = false;

    public void Awake()
    {
        startPos = this.transform.position;
        timer = 0;
    }

    public void FixedUpdate()
    {
        //if the flag to open is set, then start the process
        if (timer < (timeToOpen + delay) && movingUp && isFungus == false && movingDown == false)
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
                doorOpen = true;
                timer = 0;
            }
        }
        if (timer < timeToOpen && movingDown && movingUp == false)
        {
            timer += timeStep;
            closeDoor();

            if (timer >= timeToOpen)
            {
                movingDown = false;
                startPos = this.transform.position;
                doorOpen = false;
                timer = 0;
            }
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

    //for doors with fungus
    public void fungusOpen()
    {
        //hide the fungus
        this.transform.parent.GetChild(1).gameObject.SetActive(false);
        this.transform.parent.GetChild(2).gameObject.SetActive(true);
        isFungus = false;

        //play fungus dead sound
        if (this.transform.parent.GetComponent<AudioSource>().isPlaying == false)
        {
            this.transform.parent.GetComponent<AudioSource>().Play();
        }

        //door sound
        if (this.transform.parent.GetChild(0).GetComponent<AudioSource>().isPlaying == false)
        {
            this.transform.parent.GetChild(0).GetComponent<AudioSource>().Play();
        }

        //do some vfx explosion thing here to mask it???

        //move the door like normal
        movingUp = true;
    }

}
