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

    private void FixedUpdate()
    {
        if (collider1.activeSelf == false && collider2.activeSelf == false && player.inVent == false)
        {
           // Debug.Log("resetting vents");
            Invoke("ActivateColliders", 3f);
        }
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

                    //Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaa");
                    player.inVent = true;
                    //turn colliders off
                    collider1.SetActive(false);
                    collider2.SetActive(false);

                    //reset cam values
                    ventCam.LookAt = GameObject.Find("VentCams").GetComponentsInChildren<CinemachineDollyCart>()[ventNum - 1].gameObject.transform;
                    ventCam.Follow = player.gameObject.transform;

                    //switch to the vent cam
                    ventCam.Priority = platformCam.Priority + 1;
                    //Debug.Log("increasing ventcam prioirty");
                }
                else if (collision.gameObject.CompareTag("Player") && player.rollCounter != 1)
                {
                    //on vent floor but not rolling? --> sometimes the colliders just dont come back ggs
                    //Debug.Log("not rollingggggggggggggggggg");
                    //well too bad you're stuck in freecam then
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
                Invoke("ActivateColliders", 0.3f);
            }
        }
    }

    public void ActivateColliders()
    {
        if (player.inVent == false)
        {
            //turn colliders back on
            collider1.SetActive(true);
            collider2.SetActive(true);
        }
    }
}
