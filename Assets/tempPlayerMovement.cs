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
        
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        transform.Translate(xInput * 0.01f, 0, yInput * 0.01f);

    }
}
