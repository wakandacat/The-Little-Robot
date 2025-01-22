using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class platformTriggerScript : MonoBehaviour
{
    private GameObject platform;
    void Awake()
    {
        //platform = transform.gameObject;
    }

    //trigger to determine if player is on platform or not
    private void OnTriggerEnter(Collider other)
    {
        //fi player collided
        //if (other.gameObject.name == "playerExport")
        //{
        //   // Debug.Log("on platform");
        //    other.transform.SetParent(platform.transform);
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        //if player leaves
        //if (other.gameObject.name == "playerExport")
        //{
        //    //Debug.Log("off platform");
        //    other.transform.SetParent(null);
        //}
    }
}
