using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setCheckpoint : MonoBehaviour
{

    private mainGameScript mainGameScript;

    void Awake()
    {
        mainGameScript = GameObject.Find("WorldManager").GetComponent<mainGameScript>();
    }


    public void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.name == "playerExport")
        {

            //set this as the new checkpoint
            mainGameScript.currCheckpoint = this.gameObject;

        }
    }

    //public void OnTriggerExit(Collider collision)
    //{
    //    if (collision.gameObject.name == "playerExport")
    //    {
    //        //get rid of the trigger area so it can't be triggered again
    //        this.gameObject.SetActive(false);

    //    }
    //}
}
