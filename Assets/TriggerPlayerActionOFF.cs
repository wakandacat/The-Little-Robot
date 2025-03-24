using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlayerActionOFF : MonoBehaviour
{
    public bool stopAction = false;
    public PlayerControls player;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            stopAction = true;
            Debug.Log("stopAction" + stopAction);
        }
    }
}
