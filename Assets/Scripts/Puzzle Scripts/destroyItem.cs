using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyItem : MonoBehaviour
{

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("destroyed");
        if(collider.gameObject.name == "box" || collider.gameObject.name == "box(Clone)")
        {
            Destroy(collider.gameObject);

        }
    }

}
