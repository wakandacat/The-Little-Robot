using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    private float jumpForce = 10f;
    private float speed = 7f;
    private float fallMultiplier = 400f;
    groundCheck ground;
    private bool isJumping = false;
    private bool isQuickDropping = false;
    private int jumpCounter = 0;
    float rotationSpeed = 720f;
    public Rigidbody rb;

    void Start()
    {
        rb = player.gameObject.GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        player.GetComponent<Rigidbody>().velocity = new Vector3(player.GetComponent<Rigidbody>().velocity.x, 0f, player.GetComponent<Rigidbody>().velocity.z);
        player.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
        }
    }
}
