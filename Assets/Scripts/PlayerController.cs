using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;

    PlayerControls pc;
    public float jumpForce = 10;
    public float speed;
    groundCheck ground;
    void Start()
    {
        pc = new PlayerControls();     
        pc.Gameplay.Enable();

        ground = player.GetComponent<groundCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Gameplay Action Map Enabled: " + pc.Gameplay.enabled);
        //https://www.youtube.com/watch?app=desktop&v=f473C43s8nE
        //https://gamedevbeginner.com/how-to-jump-in-unity-with-or-without-physics/
        //https://docs.unity3d.com/Manual/scripting-vectors.html
        //walk
        //Less floaty
        Vector2 leftStick = pc.Gameplay.Walk.ReadValue<Vector2>();
        Debug.Log(leftStick);
        Vector3 translation = new Vector3(leftStick.x, 0f, leftStick.y);
        player.transform.Translate(speed * translation);
        //Move jump to its own class file
        //jump
        //Add ground or else it will fly somewhere
        float jumpButton = pc.Gameplay.Jump.ReadValue<float>();
        if (jumpButton == 1 && ground.onGround == true)
        {
            player.GetComponent<Rigidbody>().AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
        //double jump

        //Move dashes to their own class files
        //dash
        //short dash
        //roll

    }
}
