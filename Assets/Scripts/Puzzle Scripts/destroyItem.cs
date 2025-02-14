using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyItem : MonoBehaviour
{

    private void OnTriggerEnter(Collider collider)
    {
       // Debug.Log("destroyed");
        if(collider.gameObject.tag != "Player" && collider.gameObject.tag != "ground")
        {
            Destroy(collider.gameObject);

        }
    }

}
