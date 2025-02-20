using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ventScript : MonoBehaviour
{
    private PlayerController player;
    public GameObject collider1;
    public GameObject collider2;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    //check if the player is rolled, if not do not allow them entry
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player.rollCounter == 1)
            {
                //turn colliders off
                collider1.SetActive(false);
                collider2.SetActive(false);
            }
        }
    }
}
