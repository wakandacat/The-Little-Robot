using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crateDelete : MonoBehaviour
{
    public float timeToKill;
    void Awake()
    {
        Invoke("Delete", timeToKill);
    }


    void Delete()
    {
        Destroy(this.gameObject);
    }
}
