using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crateDelete : MonoBehaviour
{

    void Awake()
    {
        Invoke("Delete", 30f);
    }


    void Delete()
    {
        Destroy(this.gameObject);
    }
}
