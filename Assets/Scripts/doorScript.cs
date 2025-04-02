using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class doorScript : MonoBehaviour
{

    private float timer = 0;
    private float timeStep = 0.01f;
    public bool movingUp = false;
    public bool movingDown = false;
    private Vector3 startPos;
    public float delay = 0.7f;
    public float timeToOpen = 1.0f;
    public bool doorOpen = false;

    public bool isFungus = false;

    private Coroutine jittering;
    private float minJitter = 0.02f;
    private float maxJitter = 0.04f;
    public bool loadedNextStart = false;
    private bool runOnce = false;

    private GameObject player;

    public void Awake()
    {
        startPos = this.transform.position;
        timer = 0;

        jittering = StartCoroutine(JitterDoor());
    }
    public void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    //jitter teh door while it has fungus on it
    public IEnumerator JitterDoor()
    {
        //if we have fungus and we have the next scene loaded
        while (true)
        {
            if (isFungus == true && loadedNextStart == true)
            {
                //move door up
                this.transform.position = new Vector3(startPos.x, startPos.y + Random.Range(minJitter, maxJitter), startPos.z);
                yield return new WaitForSeconds(0.08f);
                startPos = this.transform.position;

                //move door down
                this.transform.position = new Vector3(startPos.x, startPos.y - Random.Range(minJitter, maxJitter), startPos.z);
                yield return new WaitForSeconds(0.08f);
                startPos = this.transform.position;
            }
            else
            {
                yield return null; //wait
            }   
        }
    }

    public void FixedUpdate()
    {
        //Debug.Log("checking: " + isFungus + " " + loadedNextStart);
        //if the flag to open is set, then start the process
        if (timer < (timeToOpen + delay) && movingUp && isFungus == false && movingDown == false)
        {
            //increment the timer
            timer += timeStep;

            //if the timer is greater than the delay time and not yet at the full time, then lerp the door open
            if (timer >= delay && timer <= (timeToOpen + delay))
            {
                //door sound
                if (this.transform.parent.GetChild(0).GetComponent<AudioSource>().isPlaying == false && runOnce == false)
                {
                    this.transform.parent.GetChild(0).GetComponent<AudioSource>().Play();
                    runOnce = true;
                }

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

        player.gameObject.GetComponent<player_fx_behaviors>().Rumble(0.25f, 0.25f, 1.0f);

        float newY = Mathf.Lerp(startPos.y, startPos.y + 5f, (timer - delay));
        this.transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    public void closeDoor()
    {
        movingDown = true;

        player.gameObject.GetComponent<player_fx_behaviors>().Rumble(0.25f, 0.25f, 1.0f);

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

        //stop the random jitter for teh door
        StopCoroutine(JitterDoor());
        jittering = null;

        //play fungus dead sound
        if (this.transform.parent.GetComponent<AudioSource>().isPlaying == false)
        {
            this.transform.parent.GetComponent<AudioSource>().Play();
        }

        //do some vfx explosion thing here to mask it???

        //move the door like normal
        movingUp = true;
    }

}
