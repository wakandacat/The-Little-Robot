using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class gasCloudScript : MonoBehaviour
{
    //to use: inside gas cloud fungus prefab there is an empty gameobject with a box collider that can be resized using it's size attributes (don't touch the scale!)

    int tempHealth;

    //take damage over time
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "playerExport")
        {
            Debug.Log("Player Take Damage");
            //take damage over time
           // tempHealth = collision.gameObject.GetComponent<PlayerController>().playerHealth;
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "playerExport")
        {
            Debug.Log("Player Stop Taking Damage");
            //take damage over time
           // tempHealth = collision.gameObject.GetComponent<PlayerController>().playerHealth;
        }
    }
}
