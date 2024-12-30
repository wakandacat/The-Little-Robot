using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempKillPlayerScript : MonoBehaviour
{
    //REMOVE THIS SCRIPT AND THE GAMEOBJECT IT IS ATTACHED TO ONCE THE PLAYER HAS ACTUAL DEATH STATES/FLAGS
    public void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.name == "playerExport")
        {
            Debug.Log("Player Died");
            GameObject.Find("playerExport").GetComponent<checkPointScript>().MoveToCheckpoint();
        }
    }
}
