using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bridgeScript : MonoBehaviour
{
    public void moveBridgeLeft()
    {
       // Debug.Log("hiiiii");
        Vector3 currentPosition = this.transform.position;
        float newZ = Mathf.Lerp(currentPosition.z, currentPosition.z - 40f, 0.5f);
        this.transform.position = new Vector3(currentPosition.x, currentPosition.y, newZ);
    }

    public void moveBridgeRight()
    {
       // Debug.Log("byeeee");
        Vector3 currentPosition = this.transform.position;
        float newZ = Mathf.Lerp(currentPosition.z, currentPosition.z + 40f, 0.5f);
        this.transform.position = new Vector3(currentPosition.x, currentPosition.y, newZ);
    }
}
