using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ventFloor : MonoBehaviour
{
    private PlayerController player;
    public GameObject collider1;
    public GameObject collider2;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    //check if player is on the ground
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player.rollCounter == 1)
            {
                player.inVent = true;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player.rollCounter == 1)
            {
                Invoke("ActivateColliders", 0.1f);
                player.inVent = false;
            }
        }
    }

    void ActivateColliders()
    {
        //turn colliders back on
        collider1.SetActive(true);
        collider2.SetActive(true);
    }
}
