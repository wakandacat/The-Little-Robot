using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//ToDo
//Fix the floatiness of the jump look at these resources --> https://www.youtube.com/watch?v=hG9SzQxaCm8, https://www.youtube.com/watch?app=desktop&v=h2r3_KjChf4&t=233s
//Implement Deflect
//Implement Roll
//Implement Attack
//Implement Attack Combo
//Might fall after dashing not sure why falls because the forces applied on the player are not balanced well enough so to fix freeze rotations lol
//add freecam
public class PlayerController : MonoBehaviour
{
    //player controller reference
    PlayerControls pc;

    //Script declarations
    groundCheck ground;
    EnemyCollision enemyCollision;
    cameraRotation rotationCam;

    //player variables
    public int playerHealth = 3;
    private int playerDamage = 10;
    public float playerCurrenthealth;
    private int healthRegenDelay = 10;
    public GameObject player;
    public Rigidbody rb;

    //jump variables
    private float jumpForce = 10f;
    private float speed = 30f;
    private float fallMultiplier = 800f;
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
    public Transform orientation;
    public float dashUpwardForce = 10f;

    //Attack vars
    private bool isAttacking = false;
    public bool attackState = false;
    public int attackCounter = 0;
    public float comboTimer = 0f;
    public float comboMaxTime = 5f;

    //Roll vars
    private bool Rolling = false;

    //Deflect vars
    private bool Deflecting = false;

    //pause vars
    public bool isPaused = false;
    public GameObject pauseMenu;

    //Death handlers
    UnityEngine.SceneManagement.Scene currentScene;
    private bool deathState = false;
    private int fadeDelay = 10;
    public GameObject fadeOutPanel;

    //Enemy collision temp handlers
    public bool collision = false;
    private int damageTakenDelay = 10;
    private bool invulnerable = false;

    void Start()
    {
        pc = new PlayerControls();     
        pc.Gameplay.Enable();

        pc.Gameplay.Jump.performed += OnJump;
        pc.Gameplay.QuickDrop.performed += OnQuickDrop;
        pc.Gameplay.Dash.performed += OnDash;
        pc.Gameplay.Attack.performed += OnAttack;
        pc.Gameplay.Roll.performed += OnRoll;
        pc.Gameplay.Deflect.performed += OnDeflect;
        pc.Gameplay.Pause.performed += onPause;

        rb = player.gameObject.GetComponent<Rigidbody>();
        ground = player.GetComponent<groundCheck>();
        enemyCollision = player.GetComponent<EnemyCollision>();
        rotationCam = player.GetComponent<cameraRotation>();

        playerCurrenthealth = playerHealth;
        currentScene = SceneManager.GetActiveScene();
        fadeOutPanel.SetActive(false);

    }

    //open pause menu
    public void onPause(InputAction.CallbackContext context)
    {
        pauseMenu.GetComponent<PauseMenuScript>().PauseGame();

    }


    // Update is called once per frame
    void Update()
    {
        //Debug.Log(currentScene.name);
        if (deathState == false)
        {
            manageHealth();
            Vector3 forwardVelocity = new Vector3(player.GetComponent<Rigidbody>().velocity.x, 0f, player.GetComponent<Rigidbody>().velocity.z);
            //Move might have to be on performed cause u can move while jumping and dashing and i do not think that is the intended effect

            moveCharacter();

            Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
            Debug.DrawRay(transform.position, forward, Color.green);

            Debug.Log("Jump Counter: " + jumpCounter);

            if(attackState == true)
            {
                timer();
            }

        }
        else if(deathState == true)
        {
            ManagedeathState();
        }
    }
    //-----------------------------------------------Move-----------------------------------------------//
    public void moveCharacter()
    {
        //https://www.youtube.com/watch?v=BJzYGsMcy8Q
        //https://www.youtube.com/watch?app=desktop&v=KjaRQr74jV0&t=210s

        Vector2 leftStick = pc.Gameplay.Walk.ReadValue<Vector2>();
        Vector3 movementInput = new Vector3(leftStick.x, 0f, leftStick.y);

        Vector3 cameraRelativeMovement = rotationCam.convertToCamSpace(movementInput);

        rb.MovePosition(transform.position + cameraRelativeMovement * Time.deltaTime * speed);

        Quaternion currentRotation = transform.rotation;

        if (cameraRelativeMovement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraRelativeMovement * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
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
        if (ground.onGround == true && jumpCounter == 0 && isJumping)
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

        Debug.Log("help me");
    
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
    //https://discussions.unity.com/t/rigidbody-falls-over-when-addforce-addrelativeforce-is-used/421107/4
    //https://www.youtube.com/watch?v=QRYGrCWumFw
    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = gravityScale;
        gravityScale = 0f;
        Vector3 forceToApply = orientation.forward * dashingPower;
        rb.freezeRotation = true;
        rb.AddForce(forceToApply, ForceMode.Impulse);
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
    public void OnRoll(InputAction.CallbackContext context)
    {
        Rolling = context.ReadValueAsButton();
        //player presses button we play the animation and the animation plays of the player just rolling in place except he is moving but he is rolling in place
    }
    //-----------------------------------------------Deflect-----------------------------------------------//
    public void OnDeflect(InputAction.CallbackContext context)
    {
        Deflecting = context.ReadValueAsButton();
    }
    //-----------------------------------------------Attack-----------------------------------------------//
    public void attackCombo(int counter)
    {
        if (counter == 1)
        {
            //animation here
            //enemy healtj decrease here
            Debug.Log("Attack 1");
        }
        else if(counter == 2)
        {
            //animation here
            //enemy healtj decrease here
            Debug.Log("Attack 2");
        }
        else if(counter == 3)
        {
            //animation here
            //enemy healtj decrease here
            Debug.Log("Attack 3");
        }
    }

    public void handleAttack()
    {
        isAttacking = false;
        attackState = false;
        attackCounter = 0;
        comboMaxTime = 5f;
    }
    public void timer()
    {
        comboMaxTime -= Time.deltaTime;
       if (comboMaxTime < 0)
        {
            comboMaxTime = 0;
            handleAttack();
        }

    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        isAttacking = context.ReadValueAsButton();
        if (isAttacking)
        {
            attackState = true;
            attackCounter++;
            if (comboTimer < comboMaxTime && attackState == true)
            {
                attackCombo(attackCounter);
                if(attackCounter == 4)
                {
                    handleAttack();
                }
            }
        }

    }
    //-----------------------------------------------Take Damage-----------------------------------------------//
    public void TakeDamage()
    {
        takeDamage();
    }

    public void takeDamage()
    {
        if(collision == true)
        {
            playerCurrenthealth--;
            invulnerable = true;
            StartCoroutine(Immunity());
            if(playerCurrenthealth < 0)
            {
                playerCurrenthealth = 0;
            }
        }
    }

    IEnumerator Immunity()
    {
        if(invulnerable == true)
        {
            collision = false;
            yield return new WaitForSeconds(damageTakenDelay);
            invulnerable = false;
        }

    }
    //-----------------------------------------------Health Regen-----------------------------------------------//
    //https://www.youtube.com/watch?v=uGDOiq1c7Yc
    public void manageHealth()
    {
        takeDamage();

        if (playerCurrenthealth == 0)
        {
            deathState = true;
        }
        else if (playerCurrenthealth < playerHealth)
        {
            if ( (attackState == false || collision == false) && (deathState == false))
            {

                StartCoroutine(healthRegen());
            }
        }
    }
    //Make sure to add a check if player in combat or not
    IEnumerator healthRegen()
    {
        if(playerCurrenthealth == 1)
        {
            yield return new WaitForSeconds(healthRegenDelay);
            playerCurrenthealth = 2;
        }
        else if(playerCurrenthealth == 2)
        {
            yield return new WaitForSeconds(healthRegenDelay);
            playerCurrenthealth = 3;
        }
        else if(playerCurrenthealth == 3)
        {
            playerCurrenthealth = playerHealth;
        }
    }

    //-----------------------------------------------Death State-----------------------------------------------//
    public void ManagedeathState()
    {
        fadeIn();
        Invoke("fadeOut", fadeDelay);
    }

    //Needs to change to use canvas opacity
    public void fadeIn()
    {

        fadeOutPanel.SetActive(true);
        gameObject.SetActive(false);
    }
    public void fadeOut()
    {
        SceneManager.LoadScene(currentScene.name);
        playerCurrenthealth = playerHealth;
        fadeOutPanel.SetActive(false);
        gameObject.SetActive(true);
    }

    //Destroy inputs if not used
    private void OnDestroy()
    {
        pc.Gameplay.Jump.performed -= OnJump;
        pc.Gameplay.QuickDrop.performed -= OnQuickDrop;
        pc.Gameplay.Dash.performed -= OnDash;
        pc.Gameplay.Attack.performed -= OnAttack;
        pc.Gameplay.Roll.performed -= OnRoll;
        pc.Gameplay.Deflect.performed -= OnDeflect;
        pc.Gameplay.Pause.performed -= onPause;
    }
}
