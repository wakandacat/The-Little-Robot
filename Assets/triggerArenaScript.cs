using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerArenaScript : MonoBehaviour
{

    //https://medium.com/design-bootcamp/tip-of-the-day-cinemachine-quick-camera-blending-unity-9532c038b9e3
    //https://www.youtube.com/watch?v=asruvbmUyw8

    //get the 2 virtual cameras
    public CinemachineVirtualCamera platformCam;
    public CinemachineVirtualCamera bossCam;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("entered arena");
        bossCam.Priority = platformCam.Priority + 1;

    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("exited arena");
        platformCam.Priority = bossCam.Priority + 1;

    }
}
