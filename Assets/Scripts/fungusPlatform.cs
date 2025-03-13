using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fungusPlatform : MonoBehaviour
{
    public bool postuleCollision = false;
    public GameObject postule;
    public GameObject deadPostule;
    // Start is called before the first frame update
    void FixedUpdate()
    {
        breakPlatform();
    }
    public void breakPlatform()
    {
        if (postuleCollision == true && GameObject.FindWithTag("Player").GetComponent<PlayerController>().quickDropState == true)
        {
            //Debug.Log("Hello");
            postule.SetActive(false);
            deadPostule.SetActive(true);

        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            postuleCollision = true;
            //Debug.Log("collsiuon " + postuleCollision);
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        postuleCollision = false;
    }
}
