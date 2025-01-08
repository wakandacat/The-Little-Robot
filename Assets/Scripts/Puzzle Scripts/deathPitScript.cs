using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class deathPitScript : MonoBehaviour
{
    //to use this prefab: resize the deathpit using the boxColliders's size attributes (do not resize using the Transform's scale!) and place where needed

    //kill the player in death pits
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "playerExport")
        {
            Debug.Log("Player Died");
            //collision.gameObject.GetComponent<PlayerController>().deathState = true;
            // GameObject.Find("playerExport").GetComponent<checkPointScript>().MoveToCheckpoint();
        }
    }
}
