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
    //particle system
    public ParticleSystem attack_1;
    public ParticleSystem attack_2;
    public ParticleSystem attack_3;
    //player controller reference
    PlayerControls pc;

    //External Script declarations
    groundCheck ground;
    EnemyCollision enemyCollision;
    cameraRotation rotationCam;
    checkPointScript checkPoint;
    audioManager m_audio;

    //player variables
    public int playerHealth = 5;
    private float playerDamage = 1.0f;
    public float playerCurrenthealth;
    private int healthRegenDelay = 10;
    public bool combatState = false;
    private float speed = 8.0f;
    public GameObject player;
    public Rigidbody rb;
    public Vector2 leftStick;

    //jump + quick drop vars
    public float jumpForce = 20.0f;
    private float JfallMultiplier = 8.0f;
    private float quickDropMultiplier = 20.0f;
    public bool isJumping = false;
    private bool isQuickDropping = false;
    public int jumpCounter = 0;
    private float rotationSpeed = 1.0f;
    public bool jumpState = false;
    public bool falling = false;

    //Dash vars
    public bool canDash = true;
    public bool isDashing = false;
    private float dashingPower = 40.0f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 2.0f;
    private bool Dashing = false;
    private float gravityScale = 1.0f;
    private static float globalGravity = -9.81f;
    public Transform orientation;
    private float dashUpwardForce = 10.0f;

    //Attack vars
    public bool isAttacking = false;
    private bool attackState = false;
    public int attackCounter = 0;
    private float comboMaxTime = 5.0f;
    private float attackCD = 2.0f;

    //Roll vars
    public bool Rolling = false;
    public int rollCounter = 0;
    private float rollSpeed = 10.0f;
    private float rollTime = 3.0f;
    private float maxRollSpeed = 16.0f;
    public bool rollState = false;
    public bool inVent = false;

    //Deflect vars
    private bool Deflecting = false;

    //pause vars
    public bool isPaused = false;
    public GameObject pauseMenu;

    //cutscene vars
    public GameObject worldManager;
    private mainGameScript mainScript;

    //Death vars
    UnityEngine.SceneManagement.Scene currentScene;
    public bool deathState = false;

    //canvas fade bool
    public bool fadingIn = false;
    public bool fadingOut = false;

    //Player taken damage vars
    public bool collision = false;
    private int damageTakenDelay = 10;
    private bool invulnerable = false;

    //animator
    private Animator playerAnimator;

    //Game Vars
    public string[] Combatscenes = new[] { "Combat1", "Combat2", "Combat3" };
    private GameObject enemy;

    //platforming vars
    public Vector3 platformMovement;

    //health regen
    public bool canRegen = true;

    void Start()
    {
        //https://discussions.unity.com/t/playing-a-particle-system-through-script-c/610122
        //Prep vfx
        attack_1.Stop();
        attack_2.Stop();
        attack_3.Stop();

        pc = new PlayerControls();
        pc.Gameplay.Enable();

        currentScene = SceneManager.GetActiveScene();

        rb = player.gameObject.GetComponent<Rigidbody>();
        ground = player.GetComponent<groundCheck>();
        enemyCollision = player.GetComponent<EnemyCollision>();
        rotationCam = player.GetComponent<cameraRotation>();
        checkPoint = player.GetComponent<checkPointScript>();

        playerCurrenthealth = playerHealth;
        //fadeOutPanel.SetActive(false);

        //get animator
        //playerAnimator = player.GetComponent<Animator>();

        //get audio
        m_audio = GameObject.Find("AudioManager").GetComponent<audioManager>();

        pc.Gameplay.Jump.performed += OnJump;
        pc.Gameplay.QuickDrop.performed += OnQuickDrop;
        pc.Gameplay.Dash.performed += OnDash;
        pc.Gameplay.Attack.performed += OnAttack;
        pc.Gameplay.Roll.performed += OnRoll;
        pc.Gameplay.Deflect.performed += OnDeflect;
        pc.Gameplay.Pause.performed += onPause;
        pc.Gameplay.Skip.performed += onSkip;
    }

    //open pause menu
    public void onPause(InputAction.CallbackContext context)
    {
        pauseMenu.GetComponent<PauseMenuScript>().PauseGame();
    }

    //skip cinematic
    public void onSkip(InputAction.CallbackContext context)
    {
        mainScript.GetComponent<mainGameScript>().SkipIntro();
    }

    public void findEnemy()
    {
        //update current scene reference
        currentScene = SceneManager.GetActiveScene();


        if (currentScene.name == "Combat1" || currentScene.name == "Combat2" || currentScene.name == "Combat3")
        {
            //assign current enemy
            enemy = GameObject.FindGameObjectWithTag("Boss Enemy");

            //set battle state to true
            combatState = true;
        }
        else
        {
            combatState = false;
        }
    }

    void Awake()
    {
        mainScript = worldManager.GetComponent<mainGameScript>();

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (mainScript.cutScenePlaying == false)
        {
            //Debug.Log("player health is at " + playerCurrenthealth);
           // Debug.Log("can regen " + canRegen);

            //Find enemy 
            findEnemy();

            //updated animations
            //animationCalls();

            //Check if the player is dead or alive
            if (deathState == false && Physics.gravity.y <= -9.81f)
            {
                manageHealth();
                moveCharacter(speed);
                roll();

                //Raycast for debugging purposes
                Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
                Debug.DrawRay(transform.position, forward, Color.green);

                //if player in attack State start attack combo timer
                if (attackState == true)
                {
                    timer();
                }
                if (isQuickDropping == true)
                {
                    quickDrop();
                }
            }
            else if (deathState == true) 
            {
                ManagedeathState();
                deathState = false;
            }
            manageFall(JfallMultiplier);
        }
    }
    //-----------------------------------------------Animation Calls-----------------------------------------------//
    //moved to player_fx_behaviors script

    //-----------------------------------------------Move-----------------------------------------------//
    public void moveCharacter(float playerSpeed)
    {
        //https://www.youtube.com/watch?v=BJzYGsMcy8Q
        //https://www.youtube.com/watch?app=desktop&v=KjaRQr74jV0&t=210s

        leftStick = pc.Gameplay.Walk.ReadValue<Vector2>();
        Vector3 movementInput = new Vector3(leftStick.x, 0f, leftStick.y);

        Vector3 cameraRelativeMovement = rotationCam.convertToCamSpace(movementInput);
        //player rotation
        Quaternion currentRotation = transform.rotation;

        if (cameraRelativeMovement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraRelativeMovement * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed);
        }

        //Move player using the rigid body
        rb.MovePosition(transform.position + cameraRelativeMovement * Time.deltaTime * playerSpeed + platformMovement);
       

    }

    //-----------------------------------------------Jump-----------------------------------------------//

    //Basic Jump Script using rigidody forces
    public void Jump()
    {
        player.GetComponent<Rigidbody>().velocity = new Vector3(player.GetComponent<Rigidbody>().velocity.x, 0f, player.GetComponent<Rigidbody>().velocity.z);
        player.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        player.GetComponent<Rigidbody>().freezeRotation = true;
     
    }

    //Jump flags handler
    public void handleJump()
    {
        isJumping = false;
        falling = false;
        jumpCounter = 0;
    }
    //Jump fallmultiplier
    public void manageFall(float fallMultiplier)
    {
        if (rb.velocity.y < 0)
        {
            falling = true;
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.deltaTime;
        }
    }
    //Jump Logic here
    public void OnJump(InputAction.CallbackContext context)
    {
        if (mainScript.cutScenePlaying == false)
        {
            isJumping = context.ReadValueAsButton();
            if (ground.onGround == true && jumpCounter == 0 && isJumping)
            {
                Jump();
                jumpCounter++;
                ground.jumpState = true;
            }
            //Double Jump call
            if (ground.onGround == false && jumpCounter == 1 && isJumping)
            {
                Jump();
                jumpCounter = 0;
                ground.doublejumpState = true;
            }
        }
    }

    //-----------------------------------------------Quick Drop-----------------------------------------------//
    //https://www.youtube.com/watch?v=7KiK0Aqtmzc&t=474s
    //Quick drop logic
    public void quickDrop()
    {
        if (ground.onGround == false && (rb.velocity.y < 0))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * quickDropMultiplier * Time.deltaTime;
        }
    }
    //quick drop flag handler
    public void handleQuickDrop()
    {
        isQuickDropping = false;
    }
    //on button press call quick drop and handler
    public void OnQuickDrop(InputAction.CallbackContext context)
    {
        if (mainScript.cutScenePlaying == false)
        {
            isQuickDropping = context.ReadValueAsButton();
        }
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
 /*   public IEnumerator turnOffAnim()
    {
        //always wait a little bit then check if isAttacking was true
        yield return new WaitForSeconds(0.15f);

        if(playerAnimator.GetBool("attack1") == true)
        {
            playerAnimator.SetBool("attack1", false);
        }
        else if (playerAnimator.GetBool("attack2") == true)
        {
            playerAnimator.SetBool("attack2", false);
        }
        else if (playerAnimator.GetBool("attack3") == true)
        {
            playerAnimator.SetBool("attack3", false);
        }

    }*/

    //coroutine call on button press
    public void OnDash(InputAction.CallbackContext context)
    {
        if (mainScript.cutScenePlaying == false)
        {
            //playerAnimator.SetBool("isDashing", true);
            Dashing = context.ReadValueAsButton();
            if (Dashing == true && canDash == true)
            {
                StartCoroutine(Dash());
            }
        }
    }
    //-----------------------------------------------Roll-----------------------------------------------//
    //Roll flags handler
    public void handleRoll()
    {
        Rolling = false;
        rollCounter = 0;
        rollTime = 5.0f;
        rollState = false;
        rollSpeed = 10.0f;
        player.GetComponent<CapsuleCollider>().height = 2.48f;
    }
    //player presses button we play the animation and the animation plays of the player just rolling in place except he is moving but he is rolling in place
    //Execute roll on button press
    public void rollTimer()
    {
        rollTime -= Time.deltaTime;
        rollSpeed += Time.deltaTime;
        //Debug.Log("Roll Speed" + rollSpeed);
        if (rollTime < 0 || rollSpeed > maxRollSpeed)
        {
            rollTime = 0;
            rollSpeed = maxRollSpeed;
        }
    }
    public void roll()
    {
        //animation here
        if (rollCounter == 1)
        {
            player.GetComponent<CapsuleCollider>().height = 1.07f;
            moveCharacter(rollSpeed);
            rollState = true;
            rollTimer();
        }

    }
    public void OnRoll(InputAction.CallbackContext context)
    {
        if (mainScript.cutScenePlaying == false)
        {
            if (inVent == false)
            {
                Rolling = context.ReadValueAsButton();
                if (Rolling == true)
                {
                    rollCounter++;
                    if (rollCounter == 2)
                    {
                        handleRoll();
                    }
                }
            }
            else
            {
                //Debug.Log("nah we are in teh vent");
                //make CANNOT UNROLL SOUND HERE
            }
           
        }
    }
    //-----------------------------------------------Deflect-----------------------------------------------//
    public void OnDeflect(InputAction.CallbackContext context)
    {
        if (mainScript.cutScenePlaying == false)
        {
            Deflecting = context.ReadValueAsButton();
        }
    }
    //-----------------------------------------------Attack-----------------------------------------------//
    //Check which combo state we are in and returns the animation, enemy damage
    /*    public void attackCombo(int counter)
        {
            //Debug.Log("playerController counter: " + counter);
            if (counter == 1)
            {
                //animation call reagrdless of if you collide 
                playerAnimator.SetBool("attack1", true);

                if (enemyCollision.enemyCollision == true)
                {
                    enemy.GetComponent<BossEnemy>().HP_TakeDamage(playerDamage);
                    //play sfx on hit
                    m_audio.playPlayerSFX(3);
                }
                else
                {
                    //play sfx
                    m_audio.playPlayerSFX(0);
                }
                isAttacking = false;
            }
            else if (counter == 2)
            {
                //animation call reagrdless of if you collide 
                playerAnimator.SetBool("attack2", true);

                if (enemyCollision.enemyCollision == true)
                {
                    enemy.GetComponent<BossEnemy>().HP_TakeDamage(playerDamage * 2);
                    //play sfx on hit
                    m_audio.playPlayerSFX(3);
                    StartCoroutine(interalCooldown());
                }
                else
                {
                    //play sfx
                    m_audio.playPlayerSFX(1);
                }
                isAttacking = false;
            }
            else if (counter == 3)
            {
                //animation call reagrdless of if you collide 
                playerAnimator.SetBool("attack3", true);

                if (enemyCollision.enemyCollision == true)
                {
                    enemy.GetComponent<BossEnemy>().HP_TakeDamage(playerDamage * 3 + 2);
                    //play sfx
                    m_audio.playPlayerSFX(3);
                }
                else
                {
                    //play sfx
                    m_audio.playPlayerSFX(2);
                }
                isAttacking = false;
            }

            //set attacking animation back to false
            StartCoroutine(turnOffAnim());

        }*/
    IEnumerator attackCombo(int counter)
    {
        //Debug.Log("playerController counter: " + counter);
        if (counter == 1)
        {
            //animation call reagrdless of if you collide 
            //playerAnimator.SetBool("attack1", true);

            if (enemyCollision.enemyCollision == true)
            {
                enemy.GetComponent<BossEnemy>().HP_TakeDamage(playerDamage);
                //play sfx on hit
                m_audio.playPlayerSFX(3);
                //play vfx on hit
                //attack_1.Play();
            }
            else
            {
                //play sfx
                m_audio.playPlayerSFX(0);
            }
            isAttacking = false;
            yield return new WaitForSeconds(5.0f);
        }
        else if (counter == 2)
        {
            //animation call reagrdless of if you collide 
            //playerAnimator.SetBool("attack2", true);

            if (enemyCollision.enemyCollision == true)
            {
                enemy.GetComponent<BossEnemy>().HP_TakeDamage(playerDamage * 2);
                //play sfx on hit
                m_audio.playPlayerSFX(3);
                //play vfx
                //attack_2.Play();
            }
            else
            {
                //play sfx
                m_audio.playPlayerSFX(1);
            }
            isAttacking = false;
            yield return new WaitForSeconds(5.0f);
        }
        else if (counter == 3)
        {
            //animation call reagrdless of if you collide 
            //playerAnimator.SetBool("attack3", true);

            if (enemyCollision.enemyCollision == true)
            {
                enemy.GetComponent<BossEnemy>().HP_TakeDamage(playerDamage * 3 + 2);
                //play sfx
                m_audio.playPlayerSFX(3);
                //play vfx
                //attack_3.Play();
            }
            else
            {
                //play sfx
                m_audio.playPlayerSFX(2);
            }
            isAttacking = false;
        }

        //set attacking animation back to false
        //StartCoroutine(turnOffAnim());

    }
    //Handles the combo attack flags
    public void handleAttack()
    {
        comboMaxTime = 5.0f;
        isAttacking = false;
        attackState = false;
        attackCounter = 0;
    }
    //Starts the timer and checks whether it is done or not
    //https://discussions.unity.com/t/start-countdown-timer-with-condition/203968
    //when we get all combos remeber to reset timer
    public void timer()
    {
/*        comboMaxTime -= Time.deltaTime;
        if (comboMaxTime < 0)
        {
            handleAttack();
            comboMaxTime = 0;
        }*/
    }
/*    IEnumerator interalCooldown()
    {
        yield return new WaitForSeconds(5.0f);
    }*/
    IEnumerator attackCooldown()
    {
        yield return new WaitForSeconds(attackCD);
        handleAttack();
    }
    //Call the relevant methods on button press or presses
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (mainScript.cutScenePlaying == false)
        {
            isAttacking = context.ReadValueAsButton();
            if (isAttacking == true)
            {
                attackState = true;
                attackCounter++;
                StartCoroutine(attackCombo(attackCounter));
                /*attackCombo(attackCounter);*/
                //StartCoroutine(interalCooldown());


                if (attackCounter == 3)
                {
                    StartCoroutine(attackCooldown());
                }
            }
        }
    }
    //-----------------------------------------------Take Damage-----------------------------------------------//
    public void takeDamage()
    {
        if (collision == true)
        {
            Debug.Log("take damage");
            playerCurrenthealth--;
            invulnerable = true;
            StartCoroutine(Immunity());
            if (playerCurrenthealth < 0)
            {
                playerCurrenthealth = 0;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Damage Source")
        {
            collision = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Damage Source")
        {
            collision = false;
        }
    }

    IEnumerator Immunity()
    {
        if (invulnerable == true || enemy.GetComponent<BossEnemy>().HP_ReturnCurrent() <= 0)
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
            if ((combatState == false) && (deathState == false))
            {

                StartCoroutine(healthRegen());
            }
        }
    }
    //Make sure to add a check if player in combat or not
    IEnumerator healthRegen()
    {
        if (canRegen)
        {
            if (playerCurrenthealth == 1)
            {
                //Debug.Log("in 1 regen");
                yield return new WaitForSeconds(healthRegenDelay);
                playerCurrenthealth = 2;
            }
            else if (playerCurrenthealth == 2)
            {
                //Debug.Log("in 2 regen");
                yield return new WaitForSeconds(healthRegenDelay);
                playerCurrenthealth = 3;
            }
            else if (playerCurrenthealth == 3)
            {
                //Debug.Log("in 3 regen");
                playerCurrenthealth = 4;
            }
            else if (playerCurrenthealth == 4)
            {
                //Debug.Log("in 3 regen");
                playerCurrenthealth = 5;
            }
            else if (playerCurrenthealth == 5)
            {
                //Debug.Log("in 3 regen");
                playerCurrenthealth = playerHealth;
            }
        }
        
    }

    //-----------------------------------------------Death State-----------------------------------------------//
    public void ManagedeathState()
    {
        fadeIn();
        canDash = true;
        canRegen = true;
        inVent = false;
        // Invoke("fadeOut", fadeDelay);
    }

    //Needs to change to use canvas opacity
    public void fadeIn()
    {
        fadingIn = true;
        gameObject.SetActive(false);
    }
    public void fadeOut()
    {
        //Debug.Log("yoooooooooooooo");
        fadingOut = true;
        playerCurrenthealth = playerHealth;
        checkPoint.MoveToCheckpoint();
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
        pc.Gameplay.Pause.performed -= onSkip;
    }
}
