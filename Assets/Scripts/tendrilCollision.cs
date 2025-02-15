using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tendrilCollision : MonoBehaviour
{

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && transform.root.gameObject.GetComponent<tendril_Behavior>().hasCollided == false)
        {
            transform.root.gameObject.GetComponent<tendril_Behavior>().hasCollided = true;
            Debug.Log("hit by tendril");
            player.collision = true;
            player.takeDamage();

        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log("no longer hit by tendril");
            player.collision = false;

        }
    }
}
