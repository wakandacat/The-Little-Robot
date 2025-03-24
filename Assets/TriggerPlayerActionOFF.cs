using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TriggerPlayerActionOFF : MonoBehaviour
{
    public bool stopAction = false;
    public PlayerControls player;
    public float rotation;
    private CinemachineVirtualCamera walkingCam;
    private CinemachineFreeLook freeLook;

    private void Awake()
    {
        walkingCam = GameObject.Find("walkingCam").GetComponent<CinemachineVirtualCamera>();
        freeLook = GameObject.Find("freeLookCam").GetComponent<CinemachineFreeLook>();
    }


    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            stopAction = true;
            Debug.Log("stopAction" + stopAction);
            rotation = this.transform.eulerAngles.y;
            walkingCam.Priority = freeLook.Priority + 1; //switch to the track cam
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.gameObject.GetComponent<Collider>().isTrigger = false; //set this to be a collider so you cant go back
        }
    }
}
