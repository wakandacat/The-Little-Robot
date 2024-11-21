using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class mainGameScript : MonoBehaviour
{

    //global vars
    public bool playerCanMove = false; //toggle for when menus open or cutscenes
    public int currLevelCount = 1;
    public int maxLevelCount = 3;
    public bool isPlatforming = false;
    public bool isCombat = false;
    public bool isTutorial = true;
    public string currentScene = "Tutorial";
    public string lastScene = "Tutorial";

    //get the 2 virtual cameras
    public CinemachineVirtualCamera platformCam;
    public CinemachineVirtualCamera bossCam;

    private GameObject enemy;


    public void switchCams()
    {

        //switch cameras for combat sections
        if (currentScene.StartsWith("Combat"))
        {

            enemy = GameObject.Find("enemy" + currLevelCount);

            bossCam.LookAt = enemy.transform;
            bossCam.Priority = platformCam.Priority + 1;
        }
        else //switch back for platforming sections
        {
            platformCam.Priority = bossCam.Priority + 1;
        }
    }

}
