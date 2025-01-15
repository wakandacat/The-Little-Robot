using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class gasCloudScript : MonoBehaviour
{
    //to use: inside gas cloud fungus prefab there is an empty gameobject with a box collider that can be resized using it's size attributes (don't touch the scale!)

    int tempHealth;
    public PlayerController player;

    private void Start()
    {
        player = player.GetComponent<PlayerController>();
    }
    //take damage over time
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "playerExport")
        {
            Debug.Log("Player Take Damage");
            player.combatState = true;
            //take damage over time
           // tempHealth = collision.gameObject.GetComponent<PlayerController>().playerHealth;
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "playerExport")
        {
            Debug.Log("Player Stop Taking Damage");
            player.combatState = false;

            //take damage over time
            // tempHealth = collision.gameObject.GetComponent<PlayerController>().playerHealth;
        }
    }
}
