using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    public float inputDeadZone;

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
    private float healthRegenDelay = 5.0f;
    private float regenTimer = 0.0f;
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
    public bool isQuickDropping = false;
    public bool quickDropState = false;
    public int jumpCounter = 0;
    private float rotationSpeed = 1.0f;
    public bool jumpState = false;
    public bool falling = false;
    private bool collisionPostule = false;
    private float quickDropTime = 0.0f;
    private float quickDropDelay = 1.0f;

    //Dash vars
    public bool canDash = true;
    public bool isDashing = false;
    private float dashingPower = 40.0f;
    private float dashingTime = 0.2f;
    public float dashingCooldown = 1.75f;
    private bool Dashing = false;
    private float gravityScale = 1.0f;
    private static float globalGravity = -9.81f;
    public Transform orientation;
    private float dashUpwardForce = 10.0f;
    public Coroutine dashAction;

    //Attack vars
    public bool isAttacking = false;
    private bool attackState = false;
    public int attackCounter = 0;
    private float comboMaxTime = 1.5f;
    private float attackCD = 0.5f;
    public bool runAttack = false;
    public bool runAttackAnim = false;
    public Coroutine attackCooldown;

    //Roll vars
    public bool Rolling = false;
    public int rollCounter = 0;
    private float rollSpeed = 10.0f;
    private float rollTime = 3.0f;
    private float maxRollSpeed = 12.0f;
    public bool rollState = false;
    public bool inVent = false;

    //Deflect vars
    private bool Deflecting = false;
    public bool deflectState = false;

    //pause vars
    public bool isPaused = false;
    public GameObject pauseMenu;

    //cutscene vars
    public GameObject worldManager;
    private mainGameScript mainScript;

    //Death vars
    UnityEngine.SceneManagement.Scene currentScene;
    public bool deathState = false;
    public bool diedOnce = false;

    //canvas fade bool
    public bool isFading = false; //only allow 1 fade at a time
    public GameObject deathCanvas;

    //Player taken damage vars
    public bool collision = false;
    private float immunityTime = 2.0f;
    public Coroutine immunity;
    public bool immunity_on = false;

    //animator
    private Animator playerAnimator;

    //Game Vars
    public string[] Combatscenes = new[] { "Combat1", "Combat2", "Combat3" };
    private GameObject enemy;
    private GameObject projectile;

    //platforming vars
    public Vector3 platformMovement;

    //health regen
    public bool canRegen = true;

    //sound stuff
    player_fx_behaviors fxBehave;
    public bool runTakeDamageOnce = false;
    
    //Raycast
    public LayerMask groundMask;
    public float height;

    //EndScene
    public GameObject endScene;
    public GameObject endTrigger;
    public bool foundScene = false;
    private bool triggerFound = false;
    void Start()
    {
        pc = new PlayerControls();
        pc.Gameplay.Enable();

        currentScene = SceneManager.GetActiveScene();

        rb = player.gameObject.GetComponent<Rigidbody>();
        ground = player.GetComponent<groundCheck>();
        enemyCollision = player.GetComponent<EnemyCollision>();
        rotationCam = player.GetComponent<cameraRotation>();
        checkPoint = player.GetComponent<checkPointScript>();

        playerCurrenthealth = playerHealth;

        //get audio
        m_audio = GameObject.Find("AudioManager").GetComponent<audioManager>();
        fxBehave = player.GetComponent<player_fx_behaviors>();


        pc.Gameplay.Jump.performed += OnJump;
        pc.Gameplay.QuickDrop.performed += OnQuickDrop;
        pc.Gameplay.Dash.performed += OnDash;
        pc.Gameplay.Attack.performed += OnAttack;
        pc.Gameplay.Roll.performed += OnRoll;
        pc.Gameplay.Deflect.performed += OnDeflect;
        pc.Gameplay.Pause.performed += onPause;
        pc.Gameplay.Skip.performed += onSkip;
        pc.UI.UnPause.performed += onUnPause;
        pc.UI.Cancel.performed += GoBackPause;
    }

    public void SwitchActionMap(string mapToSwitch)
    {
        //disable everything
        pc.Gameplay.Disable();
        pc.UI.Disable();

        // Enable the requested action map
        if (mapToSwitch == "Gameplay")
        {
            pc.Gameplay.Enable();
        }
        else if (mapToSwitch == "UI")
        {
            pc.UI.Enable();
        }
    }

    //when enemy dies, push player away
    //ref: https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Mathf.Sin.html
    public void combatPush(float camPos)
    {
        pc.Gameplay.Disable(); //disable temporarily

        Rigidbody rb = this.GetComponent<Rigidbody>();
        float angle = camPos * 360f;

        //angle to radians
        float radians = angle * Mathf.Deg2Rad;

        // Calculate directional force
        float xDir = Mathf.Sin(radians);  //0 to 1 on x axis
        float zDir = Mathf.Cos(radians);  //0 to 1 on z axis
        //Debug.Log("xDir " + xDir);
        //Debug.Log("zDir " + zDir);

        Vector3 force = new Vector3(xDir * 70, 40f, -zDir * 70);

        //add impulse to player
        rb.AddForce(force, ForceMode.Impulse);
        //Debug.Log("force " + force);

        Invoke("turnOnControls", 0.5f);

        //SwitchActionMap("Gameplay");
    }

    public void turnOnControls()
    {
        SwitchActionMap("Gameplay");
    }

    //open pause menu
    public void onPause(InputAction.CallbackContext context)
    {

        //if we are currently unpaused
        if (GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused == false)
        {
            SwitchActionMap("UI");
            pauseMenu.GetComponent<PauseMenuScript>().PauseGame();

        }
    }

    //B button pressed in menus
    public void GoBackPause(InputAction.CallbackContext context)
    {
        if (GameObject.FindWithTag("Player"))
        {
            //if we are currently paused
            if (GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused == true)
            {
                pauseMenu.GetComponent<PauseMenuScript>().backButton();

            }
        }
    }

    //unpause --> from ui input action system
    public void onUnPause(InputAction.CallbackContext context)
    {

        //if we are currently paused
        if (GameObject.FindWithTag("Player").GetComponent<PlayerController>().isPaused == true)
        {
            SwitchActionMap("Gameplay");
            pauseMenu.GetComponent<PauseMenuScript>().UnPause();

        }
    }

    //skip cinematic
    public void onSkip(InputAction.CallbackContext context)
    {
        mainScript.GetComponent<mainGameScript>().SkipCutScene();
    }

    public void findEnemy()
    {
        //update current scene reference
        currentScene = SceneManager.GetActiveScene(); 

        if (currentScene.name == "Combat1" || currentScene.name == "Combat2" || currentScene.name == "Combat3")
        {
            //assign current enemy
            enemy = GameObject.FindGameObjectWithTag("Boss Enemy");
            projectile = GameObject.Find("Projectile_Bullet(Clone)");
            if (projectile == null)
            {
                //Debug.Log("Not found");
                deflectState = false;
            }
            else
            {
                //Debug.Log("Found");
            }

            //set battle state to true
            combatState = true;
        }
        else
        {
            combatState = false;
        }
    }
    public void findEndScene()
    {
        //update current scene reference
        currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "EndScene")
        {
            //assign current enemy
            endScene = GameObject.Find("endSceneStartObj");
            endTrigger = GameObject.Find("endSceneActionTrigger");
            if(endScene != null)
            {
                foundScene = true;
            }
            if(endTrigger != null)
            {
                triggerFound = true;
            }
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
            //Find enemy 
            findEnemy();
            //find end scene triggers
            findEndScene();
            //Disable actions for end
            stopActions();


            //Check if the player is dead or alive
            if (deathState == false && Physics.gravity.y <= -9.81f)
            {
                manageHealth();
                moveCharacter(speed);
                roll();

                //Raycast for debugging purposes
                Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
                Vector3 down = transform.TransformDirection(Vector3.down);

               
                Debug.DrawRay(transform.position, forward, Color.green);
                Debug.DrawRay(transform.position, down, Color.red);
                height = GetGroundDistance();
                //Debug.Log(height);
                //if player in attack State start attack combo timer
                if (attackState == true)
                {
                    timer();
                }
                if (deflectState == true)
                {
                    deflectstate();
                }

                if (isQuickDropping == true)
                {
                    quickDrop();
                }
                //stopActions();
            }
            else if (deathState == true && diedOnce == false)
            {
                ManagedeathState();
                diedOnce = true;

            }
            manageFall(JfallMultiplier);
        }
        //Debug.Log("quickDrop state" + quickDropState);
    }
    //-----------------------------------------------Animation Calls-----------------------------------------------//
    //moved to player_fx_behaviors script
    //-----------------------------------------------Move-----------------------------------------------//
    public void stopActions()
    {
        if (triggerFound == true && endTrigger.GetComponent<TriggerPlayerActionOFF>().stopAction == true)
        {
            Debug.Log("Hello we are disabling the controls");
            pc.Gameplay.Attack.Disable();
            pc.Gameplay.Roll.Disable();
            pc.Gameplay.Deflect.Disable();
            pc.Gameplay.Jump.Disable();
            pc.Gameplay.QuickDrop.Disable();
            pc.Gameplay.Dash.Disable();
            pc.Gameplay.FreeLook.Disable();
        }
    }
    public void moveCharacter(float playerSpeed)
    {
        //https://www.youtube.com/watch?v=BJzYGsMcy8Q
        //https://www.youtube.com/watch?app=desktop&v=KjaRQr74jV0&t=210s

        leftStick = pc.Gameplay.Walk.ReadValue<Vector2>();
        Vector3 movementInput = new Vector3(leftStick.x, 0f, leftStick.y);

        if (leftStick.magnitude < inputDeadZone)
        {
            leftStick = Vector2.zero;
        }

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
    //https://www.youtube.com/watch?v=R7-5qUvOYg4
    public float GetGroundDistance()
    {
        float height = 3;
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask.value))
        {
            //Debug.Log("Hello");
            height = hit.distance;
        }
        return height;
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
                m_audio.playPlayerSFX(5);
                jumpCounter++;
                ground.jumpState = true;
            }
            //Double Jump call
            if (ground.onGround == false && jumpCounter == 1 && isJumping)
            {
                Jump();
                m_audio.playPlayerSFX(6);
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
            quickDropState = true;
        }
    }
    public void quickDropStatetimer()
    {
        quickDropTime += Time.deltaTime;
        if (quickDropDelay <= quickDropTime)
        {
            quickDropState = false;
            quickDropTime = 0.0f;
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
        m_audio.playPlayerSFX(7); //play dash audio
        yield return new WaitForSeconds(dashingTime);
        gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    //coroutine call on button press
    public void OnDash(InputAction.CallbackContext context)
    {
        if (mainScript.cutScenePlaying == false)
        {

            //playerAnimator.SetBool("isDashing", true);
            Dashing = context.ReadValueAsButton();
            if (Dashing == true && canDash == true)
            {
                dashAction = StartCoroutine(Dash());
                GameObject.Find("Player_UI").GetComponent<Player_UI>().dash_Bar();
            }
            else if (Dashing == true && canDash == false)
            {
                m_audio.playPlayerSFX(8);
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
                m_audio.playPlayerSFX(8);
            }

        }
    }
    //-----------------------------------------------Deflect-----------------------------------------------//
    public void OnDeflect(InputAction.CallbackContext context)
    {
        if (mainScript.cutScenePlaying == false)
        {
            Deflecting = context.ReadValueAsButton();
            if (Deflecting == true)
            {
                deflectState = true;
                Debug.Log("deflectState" + deflectState);
            }
        }
    }
    public void handleDeflect()
    {
        deflectState = false;
        Deflecting = false;
        Debug.Log("Here");
    }
    public void deflectstate()
    {
        if (collision == true && deflectState == true && projectile.GetComponent<Projectile_Bullet>().Deflection_IsDeflectable() == true)
        {
            Debug.Log("Here1");
            projectile.GetComponent<Projectile_Bullet>().Deflection_Perform();
            Debug.Log("Here2");
            if (projectile.GetComponent<Projectile_Bullet>().Deflection_HasBeenDeflected() == true)
            {
                Debug.Log("Here3");
                handleDeflect();
            }
        }
    }
    //-----------------------------------------------Attack-----------------------------------------------//

    public void attackCombo(int counter)
    {
        if (counter == 1)
        {
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
            runAttack = false;
            runAttackAnim = false;
            isAttacking = false;
        }
        else if (counter == 2)
        {

            if (enemyCollision.enemyCollision == true)
            {
                enemy.GetComponent<BossEnemy>().HP_TakeDamage(playerDamage * 2);
                //play sfx on hit
                m_audio.playPlayerSFX(3);
            }
            else
            {
                //play sfx
                m_audio.playPlayerSFX(1);
            }
            runAttack = false;
            runAttackAnim = false;
            isAttacking = false;
        }
        else if (counter == 3)
        {

            if (enemyCollision.enemyCollision == true)
            {
                enemy.GetComponent<BossEnemy>().HP_TakeDamage(playerDamage * 3 + 2);
                //play sfx
                m_audio.playPlayerSFX(3);
                //play vfx
            }
            else
            {
                //play sfx
                m_audio.playPlayerSFX(2);
            }
            runAttack = false;
            runAttackAnim = false;
            isAttacking = false;
        }


    }
    //Handles the combo attack flags
    public void handleAttack()
    {
        comboMaxTime = 5.0f;
        isAttacking = false;
        attackState = false;
        attackCounter = 0;
        //attackCD = 1.0f;
        runAttack = false;
        runAttackAnim = false;
    }
    //Starts the timer and checks whether it is done or not
    //https://discussions.unity.com/t/start-countdown-timer-with-condition/203968
    //when we get all combos remeber to reset timer
    public void timer()
    {
        //comboMaxTime -= Time.deltaTime;
        //if (comboMaxTime < 0)
        //{
        //    handleAttack();
        //    comboMaxTime = 0;
        //}
        comboMaxTime -= Time.deltaTime;
        if (comboMaxTime < 0)
        {
            comboMaxTime = 0;
            if (comboMaxTime == 0)
            {
                handleAttack();
            }
        }
    }

/*    public void attackCooldown()
    {
        Debug.Log("we are here");
        attackCD -= Time.deltaTime;
        if (attackCD < 0)
        {
            attackCD = 0;
            Debug.Log("we are here 2");
            handleAttack();
            Debug.Log("attackCounter" + attackCounter);
        }
    }*/
    public IEnumerator cooldown()
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
                attackCombo(attackCounter);
                if (attackCounter == 3)
                {
                    attackCooldown = StartCoroutine(cooldown());
                }
            }
        }
    }
    //-----------------------------------------------Take Damage-----------------------------------------------//
    public void takeDamage()
    {
        if (collision == true)
        {
            //Debug.Log("take damage");
            //m_audio.playPlayerSFX(4); //should have if here to check for if hit by fungus or projectile bc diff sounds
            //Debug.Log("Payer current health" + playerCurrenthealth);
            if (playerCurrenthealth < 0)
            {
                playerCurrenthealth = 0;
            }
        }
    }
    //https://www.youtube.com/watch?v=YSzmCf_L2cE
    public IEnumerator Immunity()
    {
        //Debug.Log("Hello");
        immunity_on = true;
        Physics.IgnoreLayerCollision(7, 6, true);
        yield return new WaitForSeconds(immunityTime);
        Physics.IgnoreLayerCollision(7, 6, false);
        immunity_on = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile" || other.gameObject.tag == "Damage Source" || (other.gameObject.tag == "Projectile" && other.gameObject.GetComponent<Projectile_Bullet>() != null))
        {
            collision = true;
            playerCurrenthealth -= 1;

        }
        if(other.gameObject.tag == "postule")
        {
            collisionPostule = true;
            Debug.Log("touchign postules");
        }

        //sfx call based on what hit you
        if (other.gameObject.name.Contains("fungus"))
        {
            //m_audio.playPlayerSFX(8); //doesn't work atm, things will need unique name checks bc not all are called fungus
        }
        else if (other.gameObject.name.Contains("Projectile"))
        {
            m_audio.playPlayerSFX(4);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        collision = false;
        runTakeDamageOnce = false;
    }

    //-----------------------------------------------Health Regen-----------------------------------------------//
    //https://www.youtube.com/watch?v=uGDOiq1c7Yc
    public void manageHealth()
    {
        healthRegen();

        if (collision == true && combatState == true && deathState == false)
        {
            immunity = StartCoroutine(Immunity());
        }
        if (combatState == true)
        {
            takeDamage();
        }

        if (playerCurrenthealth == 0 && deathState == false)
        {
            deathState = true;
            // Debug.Log("am dead");
        }

    }
    //Make sure to add a check if player in combat or not
    public void  healthRegen()
    {
        if (canRegen == false || playerCurrenthealth == playerHealth)
        {
            //Debug.Log("regen stopped");
        }
        else if (canRegen == true && (playerCurrenthealth < playerHealth) && deathState == false)
        {
            regenTimer += Time.deltaTime;
            if (healthRegenDelay <= regenTimer)
            {
                playerCurrenthealth++;
                regenTimer = 0.0f;
            }

        }

    }

    //-----------------------------------------------Death State-----------------------------------------------//
    public void ManagedeathState()
    {

        //Debug.Log("manage death");
        //fxBehave.StopCoroutine(fxBehave.walkSFX());
        //fxBehave.StopCoroutine(fxBehave.walkCoroutine);
        if (fxBehave.walkCoroutine != null)
        {
            fxBehave.StopCoroutine(fxBehave.walkCoroutine);
            fxBehave.walkCoroutine = null; // Clear reference after stopping
        }
        fxBehave.takeDamage.Stop();

        if (isFading) //start only if no fade is in progress
        {
           // Debug.Log("still in previous fading");
            deathCanvas.GetComponent<FadeOut>().ResetCanvas();
        }

        FadeSequence();
        
    }

    private void FadeSequence()
    {
       // Debug.Log("fadeSequence");
        isFading = true;  //we are now fading
        deathCanvas.GetComponent<FadeOut>().fadingIn = true; // this will run FadeOut script and call MoveToCheckpoint()

        SwitchActionMap("Gameplay");

        //reset player vars
        canDash = true;
        canRegen = true;
        inVent = false;
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
        pc.Gameplay.Skip.performed -= onSkip;
        pc.UI.UnPause.performed -= onUnPause;
        pc.UI.Cancel.performed -= GoBackPause;
    }
}
