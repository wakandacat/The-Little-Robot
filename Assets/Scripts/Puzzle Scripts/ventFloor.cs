using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ventFloor : MonoBehaviour
{
    private PlayerController player;
    public GameObject collider1;
    public GameObject collider2;

    private CinemachineVirtualCamera ventCam;

    private CinemachineFreeLook platformCam;

    public int ventNum;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        platformCam = GameObject.Find("freeLookCam").GetComponent<CinemachineFreeLook>();

        ventCam = GameObject.Find("VentCams").GetComponentsInChildren<CinemachineVirtualCamera>()[ventNum - 1];
    }

    //check if player is on the ground
    void OnCollisionEnter(Collision collision)
    {

        foreach (ContactPoint contact in collision.contacts)
        {
           // Debug.Log(contact.thisCollider.tag);
            if (contact.thisCollider.CompareTag("vent_floor"))
            {
                if (collision.gameObject.CompareTag("Player") && player.rollCounter == 1)
                {
                   // Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaa");
                    player.inVent = true;
                    //turn colliders off
                    collider1.SetActive(false);
                    collider2.SetActive(false);

                    //reset cam values
                    ventCam.LookAt = GameObject.Find("VentCams").GetComponentsInChildren<CinemachineDollyCart>()[ventNum - 1].gameObject.transform;
                    ventCam.Follow = player.gameObject.transform;

                    //switch to the vent cam
                    ventCam.Priority = platformCam.Priority + 1;
                }
                break; // Exit loop once we find a match
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Debug.Log("Player exited collision with: " + gameObject.tag);

            if (gameObject.CompareTag("vent_floor") && player.rollCounter == 1)
            {
                //Debug.Log("skejfkljdrgjkljhjjk");
                ventCam.LookAt = null;
                ventCam.Follow = null;
                player.inVent = false;
                Invoke("ActivateColliders", 0.1f);
            }
        }
    }

    void ActivateColliders()
    {
        //switch back to the freelook cam
        platformCam.Priority = ventCam.Priority + 1;
        ventCam.Priority = 10;
        //turn colliders back on
        collider1.SetActive(true);
        collider2.SetActive(true);
    }
}
