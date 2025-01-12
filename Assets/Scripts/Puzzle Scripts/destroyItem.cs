using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyItem : MonoBehaviour
{

    private void OnTriggerEnter(Collider collider)
    {
       // Debug.Log("destroyed");
        if(collider.gameObject.name == "Crate" || collider.gameObject.name == "Crate(Clone)")
        {
            Destroy(collider.gameObject);

        }
    }

}
