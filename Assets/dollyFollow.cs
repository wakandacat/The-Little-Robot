using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class dollyFollow : MonoBehaviour
{

    public CinemachineVirtualCamera cam;
    public float camPos;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        camPos = cam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition;
    }

    // Update is called once per frame
    void Update()
    {
        //we entered thru exitCollider and leaving thru enterCollider
        if (camPos >= 0.97 && cam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathOffset.z == -3)
        {
            //Debug.Log("hahahahaahhah");
            this.GetComponent<CinemachineDollyCart>().m_Position = 0.97f;
            cam.LookAt = null;
            cam.Follow = null;
        }
        //we entered thru enterCollider and leaving thru exitCollider
        else if (camPos <= 0.05 && cam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathOffset.z == 3)
        {
            //Debug.Log("ohohohoohoho");
            this.GetComponent<CinemachineDollyCart>().m_Position = 0.05f;
            cam.LookAt = null;
            cam.Follow = null;

        }
        //we are in the middle of the vent
        else
        {
            //Debug.Log("exploding");
            //update teh dolly position based on player position
            camPos = cam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition;
            this.GetComponent<CinemachineDollyCart>().m_Position = camPos;
            cam.LookAt = this.gameObject.transform;
            cam.Follow = player.gameObject.transform;
        }

    }
}
