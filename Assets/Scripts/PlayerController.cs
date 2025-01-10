using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//ToDo
//Fix the floatiness of the jump look at these resources --> https://www.youtube.com/watch?v=hG9SzQxaCm8, https://www.youtube.com/watch?app=desktop&v=h2r3_KjChf4&t=233s
//Implement Deflect
//Might need to split up some code to different scripts 
public class PlayerController : MonoBehaviour
{
    //player controller reference
    PlayerControls pc;

    //External Script declarations
    groundCheck ground;
    EnemyCollision enemyCollision;
    cameraRotation rotationCam;
    BossEnemy enemy;

    //player variables
    public int playerHealth = 3;
    private int playerDamage = 10;
    public float playerCurrenthealth;
    private int healthRegenDelay = 10;
    public bool combatState = false;
    private float speed = 30f;
    public GameObject player;
    public Rigidbody rb;

    //jump + quick drop vars
    private float jumpForce = 10f;
    private float fallMultiplier = 800f;
    private bool isJumping = false;
    private bool isQuickDropping = false;
    private int jumpCounter = 0;
    float rotationSpeed = 1.0f;

    //Dash vars
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
    public  bool Rolling = false;
    public  bool rollState = false;
    public  int rollCounter = 0;

    //Deflect vars
    private bool Deflecting = false;

    //pause vars
    public bool isPaused = false;
    public GameObject pauseMenu;

    //Death vars
    UnityEngine.SceneManagement.Scene currentScene;
    private bool deathState = false;
    private int fadeDelay = 10;
    public GameObject fadeOutPanel;

    //Player taken damage vars
    public bool collision = false;
    private int damageTakenDelay = 10;
    private bool invulnerable = false;

    //animator
    private Animator playerAnimator;

    void Start()
    {
        pc = new PlayerControls();     
        pc.Gameplay.Enable();

        currentScene = SceneManager.GetActiveScene();

        rb = player.gameObject.GetComponent<Rigidbody>();
        ground = player.GetComponent<groundCheck>();
        enemyCollision = player.GetComponent<EnemyCollision>();
        rotationCam = player.GetComponent<cameraRotation>();
        //enemy = enemy.GetComponent<BossEnemy>();

        playerCurrenthealth = playerHealth;
        fadeOutPanel.SetActive(false);

        //get animator
        playerAnimator = player.GetComponent<Animator>();

        pc.Gameplay.Jump.performed += OnJump;
        pc.Gameplay.QuickDrop.performed += OnQuickDrop;
        pc.Gameplay.Dash.performed += OnDash;
        pc.Gameplay.Attack.performed += OnAttack;
        pc.Gameplay.Roll.performed += OnRoll;
        pc.Gameplay.Deflect.performed += OnDeflect;
        pc.Gameplay.Pause.performed += onPause;
    }

    //open pause menu
    public void onPause(InputAction.CallbackContext context)
    {
        pauseMenu.GetComponent<PauseMenuScript>().PauseGame();

    }


    // Update is called once per frame
    void Update()
    {
        //Check if the player is dead or alive
        if (deathState == false)
        {
            manageHealth();
            moveCharacter();

            //Raycast for debugging purposes
            Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
            Debug.DrawRay(transform.position, forward, Color.green);

            //if player in attack State start attack combo timer
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

        //Move player using the rigid body
        rb.MovePosition(transform.position + cameraRelativeMovement * Time.deltaTime * speed);

        //player rotation
        Quaternion currentRotation = transform.rotation;

        if (cameraRelativeMovement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraRelativeMovement * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        //animation for walking
        if (playerAnimator != null)
        {
            //set playback speed for animation
            playerAnimator.SetFloat("walkSpeed", leftStick.magnitude);

            //if the player is moving then trigger the walk animation
            if (leftStick.magnitude > 0.1f)
            {
                if(Rolling == true)
                {
                    playerAnimator.SetBool("isRolling", true);
                }
                else
                {
                    playerAnimator.SetBool("isRolling", false);
                    playerAnimator.SetBool("isWalking", true);

                }
                
            }
            else
            {
                //end walk cycle and set directions back to false
                playerAnimator.SetBool("isRolling", false);
                playerAnimator.SetBool("isWalking", false);

            }


        }
    }

    //-----------------------------------------------Jump-----------------------------------------------//

    //Basic Jump Script using rigidody forces
    public void Jump()
    {
        player.GetComponent<Rigidbody>().velocity = new Vector3(player.GetComponent<Rigidbody>().velocity.x, 0f, player.GetComponent<Rigidbody>().velocity.z);
        player.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        //this does not work as intended will need to be fixed
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
        }
    }

    //Jump flags handler
    public void handleJump()
    {       
        isJumping = false;
        jumpCounter = 0;
    }
    public void OnJump(InputAction.CallbackContext context)
    {   
        isJumping = context.ReadValueAsButton();
        if (ground.onGround == true && jumpCounter == 0 && isJumping)
        {
            Jump();
            player.GetComponent<Rigidbody>().freezeRotation = true;
            jumpCounter++;
            ground.jumpState = true;
        }
        //Double Jump call
        if (ground.onGround == false && jumpCounter == 1 && isJumping)
        {
            Jump();
            player.GetComponent<Rigidbody>().freezeRotation = true;
            jumpCounter = 0;
            ground.jumpState = true;
        }

    }

    //-----------------------------------------------Quick Drop-----------------------------------------------//
    //https://www.youtube.com/watch?v=7KiK0Aqtmzc&t=474s
    //Quick drop logic
    public void quickDrop()
    {
        if (ground.onGround == false)
        {
            player.GetComponent<Rigidbody>().velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
        }
    }
    //quick drop flag handler
    public void handleQuickDrop()
    {
        if (ground.onGround == true)
        {
            isQuickDropping = false;
        }
    }
    //on button press call quick drop and handler
    public void OnQuickDrop(InputAction.CallbackContext context)
    {
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
    //Dash coroutine
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
    //coroutinr call on button press
    public void OnDash(InputAction.CallbackContext context)
    {
        Dashing = context.ReadValueAsButton();
        if (Dashing == true)
        {
            StartCoroutine(Dash());

        }
    }
    //-----------------------------------------------Roll-----------------------------------------------//
    //Roll flags handler
    public void handleRoll()
    {
        Rolling = false;
        rollState = false;
        rollCounter = 0;
    }
    //player presses button we play the animation and the animation plays of the player just rolling in place except he is moving but he is rolling in place
    //Execute roll on button press
    public void OnRoll(InputAction.CallbackContext context)
    {
        Rolling = context.ReadValueAsButton();
        if(Rolling == true)
        {
            rollCounter++;
            //animation here
            if(rollCounter == 1)
            {
                moveCharacter();
            }
            else if (rollCounter == 2){
                handleRoll();
            }
        }
    }
    //-----------------------------------------------Deflect-----------------------------------------------//
    public void OnDeflect(InputAction.CallbackContext context)
    {
        Deflecting = context.ReadValueAsButton();
    }
    //-----------------------------------------------Attack-----------------------------------------------//
    //Check which combo state we are in and returns the animation, enemy damage
    public void attackCombo(int counter)
    {
        if (counter == 1)
        {
            //animation here
            if (enemyCollision.enemyCollision == true)
            {
                //enemy.HP_TakeDamage(playerDamage);
            }
        }
        else if(counter == 2)
        {
            //animation here
            if (enemyCollision.enemyCollision == true)
            {
                //enemy.HP_TakeDamage(playerDamage*5);
            }
        }
        else if(counter == 3)
        {
            //animation here
            if (enemyCollision.enemyCollision == true)
            {
                //enemy.HP_TakeDamage(playerDamage*10);
            }
        }
    }
    //Handles the combo attack flags
    public void handleAttack()
    {
        isAttacking = false;
        attackState = false;
        attackCounter = 0;
        comboMaxTime = 5f;
    }
    //Starts the timer and checks whether it is done or not
    //https://discussions.unity.com/t/start-countdown-timer-with-condition/203968
    public void timer()
    {
        comboMaxTime -= Time.deltaTime;
       if (comboMaxTime < 0)
        {
            comboMaxTime = 0;
            handleAttack();
        }

    }
    //Call the relevant methods on button press or presses
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
        if (combatState == true)
        {
            takeDamage();
        }

        if (playerCurrenthealth == 0)
        {
            deathState = true;
        }
        else if (playerCurrenthealth < playerHealth)
        {
            if ((combatState == false || collision == false) && (deathState == false))
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
