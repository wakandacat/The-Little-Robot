using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class corpseFlowerScript : MonoBehaviour
{

    private float delayTime = 2f;
    private bool isRotating = false;
    private bool goingDown = true;
    private float rotateSpeed = 0.005f;
    private float rotAngle = 40f;

    //jitter
    private bool isRumbling = false;
    private Vector3 startPos;
    public Coroutine rumblePlat;
    private float minJitter = 0.05f;
    private float maxJitter = 0.3f;

    private void Start()
    {
        this.transform.rotation = Quaternion.Euler(0, this.transform.eulerAngles.y, this.transform.eulerAngles.z); //make sure its right

        startPos = this.transform.position; // for jitter
        rumblePlat = StartCoroutine(LeafRumble());
    }

    public IEnumerator LeafRumble()
    {
        //rumble after triggering
        while (true)
        {
            if (isRumbling == true)
            {
                //move door up
                this.transform.position = new Vector3(startPos.x, startPos.y + Random.Range(minJitter, maxJitter), startPos.z);
                yield return new WaitForSeconds(Random.Range(minJitter, maxJitter));
                startPos = this.transform.position;

                //move door down
                this.transform.position = new Vector3(startPos.x, startPos.y - Random.Range(minJitter, maxJitter), startPos.z);
                yield return new WaitForSeconds(Random.Range(minJitter, maxJitter));
                startPos = this.transform.position;
            }
            else
            {
                yield return null; //wait
            }
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        //we are falling down
        if (isRotating && goingDown)
        {
            if (this.transform.eulerAngles.x <= rotAngle)
            {
                //lerp the rotation down
                this.transform.rotation = Quaternion.Euler(this.transform.eulerAngles.x + Mathf.Lerp(0, rotAngle, rotateSpeed), this.transform.eulerAngles.y, this.transform.eulerAngles.z);
            } 
            else
            {
                isRotating = false;
                goingDown = false;

                this.transform.rotation = Quaternion.Euler(rotAngle, this.transform.eulerAngles.y, this.transform.eulerAngles.z); //make sure its right

                endRotate(); //reset flags
            }       

        } 
        else if (isRotating && goingDown == false)
        {
            //Debug.Log(this.transform.eulerAngles.x);
            if (this.transform.eulerAngles.x >= -0.1f && this.transform.eulerAngles.x <= rotAngle + 1) //eulerangles alwasys positive
            {
                //lerp the rotation back
                this.transform.rotation = Quaternion.Euler(this.transform.eulerAngles.x + Mathf.Lerp(0, -rotAngle, rotateSpeed), this.transform.eulerAngles.y, this.transform.eulerAngles.z);
            }
            else
            {
                this.transform.rotation = Quaternion.Euler(0, this.transform.eulerAngles.y, this.transform.eulerAngles.z); //make sure its right

                isRotating = false;
                goingDown = true;
            }
        }
    }

    //when player jumps onto leaf
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isRotating == false && goingDown == true)
        {
            Invoke("startRumble", delayTime);
            if (this.gameObject.GetComponent<AudioSource>().isPlaying == false)
            {
                this.gameObject.GetComponent<AudioSource>().Play();
            }
        }
    }

    void startRumble()
    {
        isRumbling = true;
        Invoke("startRotate", delayTime);
    }

    void startRotate()
    {
        isRotating = true;
        isRumbling = false;
    }

    void endRotate()
    {
        Invoke("startRotate", delayTime); //get ready to put the leaf back to where it was
    }
}
