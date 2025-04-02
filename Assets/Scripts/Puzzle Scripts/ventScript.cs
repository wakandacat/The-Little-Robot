using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ventScript : MonoBehaviour
{
    private PlayerController player;
    public GameObject collider1;
    public GameObject collider2;

    private CinemachineVirtualCamera ventCam;

    private CinemachineFreeLook platformCam;

    public int ventNum;

    public int entranceDirection = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        platformCam = GameObject.Find("freeLookCam").GetComponent<CinemachineFreeLook>();

        ventCam = GameObject.Find("VentCams").GetComponentsInChildren<CinemachineVirtualCamera>()[ventNum-1];
    }

    //check if the player is rolled, if not do not allow them entry
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player.rollCounter == 1) //we are rolling
            {

                //turn colliders off
                collider1.SetActive(false);
                collider2.SetActive(false);

                //switch the camera offset direction depending on which side is entered first
                if (collision.contacts[0].thisCollider.name == "exitCollider")
                {
                    //Debug.Log(collision.contacts[0].thisCollider.name + "-1");
                    entranceDirection = -1;
                } 
                else if (collision.contacts[0].thisCollider.name == "enterCollider")
                {
                    //Debug.Log(collision.contacts[0].thisCollider.name + "1");
                    entranceDirection = 1;
                }

                GameObject.Find("VentCams").GetComponentsInChildren<CinemachineTrackedDolly>()[ventNum - 1].m_PathOffset.z = 3 * entranceDirection;
                GameObject.Find("VentCams").GetComponentsInChildren<CinemachineComposer>()[ventNum - 1].m_TrackedObjectOffset.z = 3 * (-1) * entranceDirection;

            }
        }
    }
}
