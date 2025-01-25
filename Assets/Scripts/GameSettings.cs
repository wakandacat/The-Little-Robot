using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    //add all the settings from main menu here that should be carried over to the BaseScene
    public float freelookSens = 0.5f;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

}
