using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class deathPitScript : MonoBehaviour
{
    //to use this prefab: resize the deathpit using the boxColliders's size attributes (do not resize using the Transform's scale!) and place where needed

    //kill the player in death pits
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player Died");

            GameObject.FindWithTag("Player").GetComponent<PlayerController>().deathState = true;

        }
    }
}
