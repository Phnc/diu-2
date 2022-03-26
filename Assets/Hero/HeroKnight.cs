using UnityEngine;
using System.Collections;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      speed = 4.0f;
    [SerializeField] float      jumpForce = 7.5f;
    [SerializeField] float      rollForce = 6.0f;
    [SerializeField] bool       noBlood = false;
    [SerializeField] GameObject slideDust;

    private Animator            animator;
    private Rigidbody2D         body;
    private Sensor_HeroKnight   groundSensor;
    private Sensor_HeroKnight   wallSensorR1;
    private Sensor_HeroKnight   wallSensorR2;
    private Sensor_HeroKnight   wallSensorL1;
    private Sensor_HeroKnight   wallSensorL2;
    private bool                isWallSliding = false;
    private bool                grounded = false;
    private bool                rolling = false;
    private int                 facingDirection = 1;
    private int                 currentAttack = 0;
    private float               timeSinceAttack = 0.0f;
    private float               delayToIdle = 0.0f;
    private float               rollDuration = 8.0f / 14.0f;
    private float               rollCurrentTime;


    // Use this for initialization
    void Start ()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
    }

    // Update is called once per frame
    void Update ()
    {
        // Increase timer that controls attack combo
        timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if(rolling)
            rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if(rollCurrentTime > rollDuration)
            rolling = false;

        //Check if character just landed on the ground
        if (!grounded && groundSensor.State())
        {
            grounded = true;
            animator.SetBool("Grounded", grounded);
        }

        //Check if character just started falling
        if (grounded && !groundSensor.State())
        {
            grounded = false;
            animator.SetBool("Grounded", grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            facingDirection = 1;
        }
            
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            facingDirection = -1;
        }

        // Move
        if (!rolling )
            body.velocity = new Vector2(inputX * speed, body.velocity.y);

        //Set AirSpeed in animator
        animator.SetFloat("AirSpeedY", body.velocity.y);

        // -- Handle Animations --
        //Wall Slide
        isWallSliding = (wallSensorR1.State() && wallSensorR2.State()) || (wallSensorL1.State() && wallSensorL2.State());
        animator.SetBool("WallSlide", isWallSliding);

        //Death
        /*if (Input.GetKeyDown("e") && !rolling)
        {
            animator.SetBool("noBlood", noBlood);
            animator.SetTrigger("Death");
        }*/
            
        //Hurt
        /*else if (Input.GetKeyDown("q") && !rolling)
            animator.SetTrigger("Hurt");*/

        //Attack
        /*else if(Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f && !rolling)
        {
            currentAttack++;

            // Loop back to one after third attack
            if (currentAttack > 3)
                currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            animator.SetTrigger("Attack" + currentAttack);

            // Reset timer
            timeSinceAttack = 0.0f;
        }*/

        // Block
        if (Input.GetMouseButtonDown(1) && !rolling)
        {
            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
            animator.SetBool("IdleBlock", false);

        // Roll
        else if (Input.GetKeyDown("left shift") && !rolling && !isWallSliding)
        {
            rolling = true;
            animator.SetTrigger("Roll");
            body.velocity = new Vector2(facingDirection * rollForce, body.velocity.y);
        }
            

        //Jump
        else if (Input.GetKeyDown("space") && grounded && !rolling)
        {
            animator.SetTrigger("Jump");
            grounded = false;
            animator.SetBool("Grounded", grounded);
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            delayToIdle = 0.05f;
            animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            delayToIdle -= Time.deltaTime;
                if(delayToIdle < 0)
                    animator.SetInteger("AnimState", 0);
        }
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (facingDirection == 1)
            spawnPosition = wallSensorR2.transform.position;
        else
            spawnPosition = wallSensorL2.transform.position;

        if (slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(facingDirection, 1, 1);
        }
    }
}
