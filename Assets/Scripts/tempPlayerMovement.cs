using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempPlayerMovement : MonoBehaviour
{
    //cinemachine reference
    //https://www.youtube.com/watch?v=P_ibDJhFVMU
    float xInput;
    float yInput;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //raw inputs
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        //camera orientation
        //https://discussions.unity.com/t/moving-character-relative-to-camera/614923
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate the movement direction relative to the camera
        Vector3 movementDirection = cameraRight * xInput + cameraForward * yInput;

        // Move the player
        transform.Translate(movementDirection * 0.05f, Space.World);

    }
}
