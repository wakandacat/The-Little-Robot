using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//ToDo
//Fix the floatiness of the jump look at these resources --> https://www.youtube.com/watch?v=hG9SzQxaCm8, https://www.youtube.com/watch?app=desktop&v=h2r3_KjChf4&t=233s
//Fix the walk so you cannot walk while in any other state
//Fix the quick drop so you can actually quick drop
//Fix the quick drop state breaking the singular jump state
//Implement Deflect
//Implement Roll
//Implement Attack
//Implement Attack Combo
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;

    PlayerControls pc;
    public float jumpForce = 3f;
    public float speed = 5f;
    public float fallMultiplier = 2.5f;
    groundCheck ground;
    public bool isJumping = false;
    public bool isQuickDropping = false;
    private int jumpCounter = 0;
    public bool jumpState = false;

    //Dash Booleans
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingpower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    public Rigidbody rb;
    private bool Dashing = false;
    public float gravityScale = 1.0f;
    public static float globalGravity = -9.81f;


    void Start()
    {
        pc = new PlayerControls();     
        pc.Gameplay.Enable();

        pc.Gameplay.Jump.performed += OnJump;
        pc.Gameplay.QuickDrop.performed += OnQuickDrop;
        pc.Gameplay.Dash.performed += OnDash;
        rb = player.gameObject.GetComponent<Rigidbody>();
        ground = player.GetComponent<groundCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        //Move might have to be on performed cause u can move while jumping and dashing and i do not think that is the intended effect

        Vector2 leftStick = pc.Gameplay.Walk.ReadValue<Vector2>();
       
        Vector3 translation = new Vector3(leftStick.x, 0f, leftStick.y);
        player.transform.Translate(speed * translation *Time.deltaTime);
    }
    //-----------------------------------------------Jump-----------------------------------------------//
    //https://www.youtube.com/watch?v=7KiK0Aqtmzc&t=474s
    public void Jump()
    {
        player.GetComponent<Rigidbody>().velocity = new Vector3(player.GetComponent<Rigidbody>().velocity.x, 0f, player.GetComponent<Rigidbody>().velocity.z);
        player.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        //this didn't fix the slow drop
        if (player.GetComponent<Rigidbody>().velocity.y < 0)
        {
            player.GetComponent<Rigidbody>().velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

    }

    public void doubleJump()
    {
        player.GetComponent<Rigidbody>().velocity = new Vector3(player.GetComponent<Rigidbody>().velocity.x, 0f, player.GetComponent<Rigidbody>().velocity.z);
        player.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        //this didn't fix the slow drop
        if (player.GetComponent<Rigidbody>().velocity.y < 0)
        {
            player.GetComponent<Rigidbody>().velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

    }
    public void handleJump()
    {
        if (ground.onGround == true && jumpState == true)
        {
            jumpState = false;
            isJumping = false;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("OnJump triggered");

        isJumping = context.ReadValueAsButton();
        if (ground.onGround == true && jumpCounter == 0 && isJumping)
        {
            Jump();
            jumpCounter++;
            jumpState = true;
        }
        if (ground.onGround == false && jumpCounter == 1 && isJumping)
        {
            doubleJump();
            jumpCounter = 0;
            jumpState = true;
        }

        handleJump();
        Debug.Log("help me");
    }

    //-----------------------------------------------Quick Drop-----------------------------------------------//
    //The idea is that when quickdropping increase downwards velocity or increase gravity?
    //https://www.youtube.com/watch?v=7KiK0Aqtmzc&t=474s
    public void quickDrop()
    {
        if(ground.onGround == false && jumpState == true)
        {
            player.GetComponent<Rigidbody>().velocity += Vector3.up * Physics.gravity.y * (10f - 1) * Time.deltaTime;
        }
        jumpState = false;
    }
    //breaks singular jump when trying to quick drop from it

    public void handleQuickDrop()
    {
        if (ground.onGround == false)
        {
            isQuickDropping = false;
            jumpState = false;
        }
    }
    public void OnQuickDrop(InputAction.CallbackContext context)
    {
        Debug.Log("On quickdrop triggered");

        isQuickDropping = context.ReadValueAsButton();
        if(isQuickDropping == true){
            quickDrop();
            handleQuickDrop();
        }

    }

    //-----------------------------------------------Dash-----------------------------------------------//
    //https://www.youtube.com/watch?v=vTNWUbGkZ58
    //https://discussions.unity.com/t/why-does-rigidbody-3d-not-have-a-gravity-scale/645511/2
    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = gravityScale;
        gravityScale = 0f;
        rb.velocity = new Vector3(transform.localScale.x * dashingPower, 0f, transform.localScale.z);
        yield return new WaitForSeconds(dashingTime);
        gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        Debug.Log("OnDash triggered");

        Dashing = context.ReadValueAsButton();
        if (Dashing == true)
        {
            StartCoroutine(Dash());

        }
    }
    //-----------------------------------------------Roll-----------------------------------------------//
    //-----------------------------------------------Deflect-----------------------------------------------//
    //-----------------------------------------------Attack-----------------------------------------------//



    private void OnDestroy()
    {
        pc.Gameplay.Jump.performed -= OnJump;
        pc.Gameplay.QuickDrop.performed -= OnQuickDrop;
        pc.Gameplay.Dash.performed -= OnDash;

    }
}
