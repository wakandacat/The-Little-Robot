using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//ToDo
//Fix the floatiness of the jump look at these resources --> https://www.youtube.com/watch?v=hG9SzQxaCm8, https://www.youtube.com/watch?app=desktop&v=h2r3_KjChf4&t=233s
//Implement Deflect
//Implement Roll
//Implement Attack
//Implement Attack Combo
//Might fall after dashing not sure why
//add freecam
public class PlayerController : MonoBehaviour
{
    //player variables
    public GameObject player;
    public Rigidbody rb;

    //player controller reference
    PlayerControls pc;

    //jump variables
    private float jumpForce = 10f;
    private float speed = 7f;
    private float fallMultiplier = 800f;
    groundCheck ground;
    private bool isJumping = false;
    private bool isQuickDropping = false;
    private int jumpCounter = 0;
    float rotationSpeed = 1.0f;

    //Dash Booleans
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingpower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    private bool Dashing = false;
    public float gravityScale = 1.0f;
    public static float globalGravity = -9.81f;


    //pause vars
    public bool isPaused = false;
    public GameObject pauseMenu;

    void Start()
    {
        pc = new PlayerControls();     
        pc.Gameplay.Enable();

        pc.Gameplay.Jump.performed += OnJump;
        pc.Gameplay.QuickDrop.performed += OnQuickDrop;
        pc.Gameplay.Dash.performed += OnDash;
        pc.Gameplay.Pause.performed += onPause;
        rb = player.gameObject.GetComponent<Rigidbody>();
        ground = player.GetComponent<groundCheck>();

    }

    //open pause menu
    public void onPause(InputAction.CallbackContext context)
    {
        pauseMenu.GetComponent<PauseMenuScript>().PauseGame();

    }


    // Update is called once per frame
    void Update()
    {
        Vector3 forwardVelocity = new Vector3(player.GetComponent<Rigidbody>().velocity.x, 0f, player.GetComponent<Rigidbody>().velocity.z);
        //Move might have to be on performed cause u can move while jumping and dashing and i do not think that is the intended effect

        moveCharacter();

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
        Debug.DrawRay(transform.position, forward, Color.green);
        Debug.Log(player.GetComponent<Rigidbody>().velocity.y);

        if (isJumping == true && ground.onGround == true)
        {
            Debug.Log("Jump button pressed");
            player.GetComponent<Rigidbody>().velocity = new Vector3(player.GetComponent<Rigidbody>().velocity.x, 0f, player.GetComponent<Rigidbody>().velocity.z);
            player.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (rb.velocity.y < 0)
            { 
                rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
            }
            jumpCounter++;
            ground.jumpState = true;
        }
        if (ground.onGround == false && jumpCounter == 1 && isJumping)
        {
            Debug.Log("Jump button pressed");
            player.GetComponent<Rigidbody>().velocity = new Vector3(player.GetComponent<Rigidbody>().velocity.x, 0f, player.GetComponent<Rigidbody>().velocity.z);
            player.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
            }
            jumpCounter = 0;
            ground.jumpState = true;
        }
    }
    //-----------------------------------------------Move-----------------------------------------------//
    public void moveCharacter()
    {
        //https://www.youtube.com/watch?v=BJzYGsMcy8Q
        //https://www.youtube.com/watch?app=desktop&v=KjaRQr74jV0&t=210s
        //This will change to follow convention of moving the rigidbody and not the gameObject
        Vector2 leftStick = pc.Gameplay.Walk.ReadValue<Vector2>();

        Vector3 camForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1)).normalized;

        Vector3 movementDirection = ((camForward * leftStick.y) + (camRight * leftStick.x)) * 1.0f;

        //player.transform.Translate(speed * movementDirection * Time.deltaTime, Space.World);
        Vector3 translation = new Vector3(leftStick.x, 0f, leftStick.y);
        rb.MovePosition(transform.position + translation * Time.deltaTime * speed);

        //Vector3 positionToLookAt;

        //positionToLookAt.x = leftStick.x;
        //positionToLookAt.y = 0.0f;
        //positionToLookAt.z = leftStick.y;


        Quaternion currentRotation = transform.rotation;

        if(movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed*Time.deltaTime);
        }
    }

    //-----------------------------------------------Jump-----------------------------------------------//
    public void Jump()
    {
        player.GetComponent<Rigidbody>().velocity = new Vector3(player.GetComponent<Rigidbody>().velocity.x, 0f, player.GetComponent<Rigidbody>().velocity.z);
        player.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
        }



        //so the issue is that the jump is too floaty
        //fixes is to make the gravity higher once it reaches the peak of the arc?


        /*  player.GetComponent<Rigidbody>().velocity = new Vector3(player.GetComponent<Rigidbody>().velocity.x, intialVelocity, player.GetComponent<Rigidbody>().velocity.z);
          player.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpGravity, ForceMode.Impulse);*/

    }

    public void doubleJump()
    {
        Jump();
        
    }
    public void handleJump()
    {
        Debug.Log("we are handling jump");
       
            isJumping = false;
            jumpCounter = 0;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("OnJump triggered");

        isJumping = context.ReadValueAsButton();
/*        if (ground.onGround == true && jumpCounter == 0 && isJumping)
        {
            Jump();
            player.GetComponent<Rigidbody>().freezeRotation = true;
            jumpCounter++;
            ground.jumpState = true;
        }
        if (ground.onGround == false && jumpCounter == 1 && isJumping)
        {
            doubleJump();
            player.GetComponent<Rigidbody>().freezeRotation = true;
            jumpCounter = 0;
            ground.jumpState = true;
        }

        Debug.Log("help me");*/
    }

    //-----------------------------------------------Quick Drop-----------------------------------------------//
    //The idea is that when quickdropping increase downwards velocity or increase gravity?
    //https://www.youtube.com/watch?v=7KiK0Aqtmzc&t=474s
    public void quickDrop()
    {
        Debug.Log("1 = " + player.GetComponent<Rigidbody>().velocity);

        if (ground.onGround == false)
        {
            player.GetComponent<Rigidbody>().velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
            Debug.Log("hELLO QUICK DROP");
            Debug.Log("2 = " + player.GetComponent<Rigidbody>().velocity);
        }
    }
    //breaks singular jump when trying to quick drop from it

    public void handleQuickDrop()
    {
        if (ground.onGround == true)
        {
            isQuickDropping = false;
        }
    }
    public void OnQuickDrop(InputAction.CallbackContext context)
    {
        Debug.Log("On quickdrop triggered");

        isQuickDropping = context.ReadValueAsButton();
        if(isQuickDropping == true){
            quickDrop();
        }
        handleQuickDrop();

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

        //get the walk direction input needs to be refined
        Vector2 leftStick = pc.Gameplay.Walk.ReadValue<Vector2>();
        Vector3 translation = new Vector3(leftStick.x, 0f, leftStick.y);
        //translation.Normalize();
        player.transform.Translate(translation * Time.deltaTime);
        
        rb.velocity = new Vector3(translation.x * dashingPower, 0f,0f);
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
        pc.Gameplay.Pause.performed -= onPause;
    }
}
