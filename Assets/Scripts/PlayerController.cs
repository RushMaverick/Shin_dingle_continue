using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;
    [SerializeField] public Collider2D m_CrouchDisableCollider;
    public float movementInputDirection;
    public float reversemovementInputDirection;
    private float jumpTimer;
    private float turnTimer;
    private float wallJumpTimer;
    public int amountOfJumpsLeft;
    public bool isFacingRight;
    private int lastWallJumpDirection;
    public Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private bool isWalking;
    [SerializeField] public bool isGrounded;
    public bool isLicking0;
    public bool isLicking45;
    public bool isLicking90;
    public bool isTouchingWall;
    //private bool isWallSliding;
    private bool isCrouching;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool canMove;
    private bool canFlip;
    private bool hasWallJumped;
    [SerializeField] private bool wallFlightLeft = false;
    [SerializeField] private bool wallFlightRight = false;
    //[SerializeField] private bool wallFlight = false;
    [SerializeField] private bool isSwinging;
    [SerializeField] private bool isSwingingNoInput;
    public int amountOfJumps = 1;
    [SerializeField] public float movementSpeed = 4.0f;
    public float increaseSpeed = 3.5f;
    public float decreaseSpeed = 8.0f;
    public float maxSpeed = 9.0f;
    public float jumpForce = 8.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlidingSpeed;
    //public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallJumpForce;
    public float jumpTimerSet = 0.15f;
    public float turnTimerSet = 0.1f;
    //BARDENT public float turnTimerSet = 0.1f;
    public float wallJumpTimerSet = 0.5f;
    public Vector2 wallJumpDirection;
    public Transform wallCheck;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;
    public int facingDirection = 1;
    Animator playerAnimator;
    public float normalizedTime;
    public GameObject Tongue;
    public GameObject Tongue_2;
    public GameObject Tongue_3;
    public GameObject line;
    public GameObject Grid;
    public LineRenderer linerenderer;
    SpringJoint2D joint;
    public Vector3 wallJump;
    public GameObject Player;
    public Transform playerPosition;
    public Transform endMarker;
    public float wallJumpSpeed = 0.01f;
    private float startTime;
    private float journeyLength;
    [SerializeField] private bool canChangeDirection = true;
    private bool StopYDecend;
    [SerializeField] public bool walljumpwindow;
    [SerializeField] public bool specialWallslide;
    public bool crouch = false;
    [SerializeField] public Transform CeilingCheck;
    const float k_CeilingRadius = .2f;
    [SerializeField] public bool hasSwinged;
    public float swingSpeed;
    public float playerMagnitude;
    public GameObject Look;
    //new stuff
    public Vector2 exitSling;
    public GameObject Point_Forward;
    public GameObject player_Angle;
    public GameObject player_Direction;
    public float ExitSwingDecreaseFlight = 0.5f;
    public float ExitSwingDecreaseFall = 0.9f;
    public float loweredVector = 0.7f;
    public float previousVelocityY;
    public float currentVelocityY;
    //new lerpstuff
    public Transform lerpStart;
    public Transform lerpStop;
    public float lerpSpeed = 1.0f;
    private float lerpStartTime;
    public float lerpJourneyLenght;
    private bool canLerpAgain = true;
    public bool isTravelingDown = false;
    public GameObject lerpEndMarker;
    public bool swingFall = false;


    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool m_wasCrouching = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        amountOfJumpsLeft = amountOfJumps;
        anim = GetComponent<Animator>();
        joint = GetComponent<SpringJoint2D>();

        playerAnimator = gameObject.GetComponent<Animator>();

        startTime = Time.time;
        journeyLength = Vector3.Distance(playerPosition.position, endMarker.position);

        //new lerpstuff
        lerpStartTime = Time.time;
        lerpJourneyLenght = Vector2.Distance(lerpStart.position, lerpStop.position);

    }

    private void Awake()
    {
        Tongue = GameObject.Find("Tongue");
        Tongue_2 = GameObject.Find("Tongue_2");
        Tongue_3 = GameObject.Find("Tongue_3");
        line = GameObject.Find("line");
        Grid = GameObject.Find("Grid");

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    // Update is called once per frame
    void Update()
    {

        CheckInput();
        CheckMovementDirection();
        CheckIfWallSliding();
        CheckIfCanJump();
        CheckJump();
        CheckIfCrouching();
        UpdateAnimations();

        //Crouching
        if (Input.GetAxisRaw("Vertical") == -1)
        {
            crouch = true;
            isCrouching = true;
        }
        else if (Input.GetAxisRaw("Vertical") == 0)
        {
            crouch = false;
            isCrouching = false;
        }


        //Licking animation 

        //Lick 0
        if (Input.GetKeyDown(KeyCode.L) && !Input.GetButton("Vertical"))
        {
            print("Lick");
            isLicking0 = true;
            isLicking45 = false;
            isLicking90 = false;
        }

        //Lick 45
        if (Input.GetKeyDown(KeyCode.L) && Input.GetButton("Horizontal") && Input.GetButton("Vertical"))
        {
            isLicking45 = true;
            isLicking0 = false;
            isLicking90 = false;
        }

        //Lick 90
        if (Input.GetKeyDown(KeyCode.L) && Input.GetButton("Vertical") && !Input.GetButton("Horizontal"))
        {
            isLicking90 = true;
            isLicking45 = false;
            isLicking0 = false;
        }

        //Stop Simple Licking Animation

        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player_IsLicking0_anim") &&
            playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            isLicking0 = false;
        }

        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player_IsLicking45_anim") &&
            playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            isLicking45 = false;
        }

        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Player_IsLicking90_anim") &&
            playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            isLicking90 = false;
        }

        //Disable movement when licking plainly
        if (isGrounded && isLicking0 == true)
        {
            movementInputDirection = 0;
        }

        //Disable movement when licking in 45 degree
        if (isGrounded && isLicking45 == true)
        {
            movementInputDirection = 0;
        }

        //Movement momentum
        if (movementInputDirection == 1 || movementInputDirection == -1)
        {
            if (movementSpeed < maxSpeed)
            {
                movementSpeed += increaseSpeed * Time.deltaTime;
            }
        }
        else if (movementInputDirection == 0 && movementSpeed > 4)
        {
            movementSpeed -= decreaseSpeed * Time.deltaTime;
        }

        //Airtravel: Flip&Flight

        if (wallFlightRight == true)
        {
            canChangeDirection = false;

            float distCovered = (0.3f - startTime) * 1;
            float fractionOfJourney = distCovered / journeyLength;
            playerPosition.transform.position = Vector3.Lerp(playerPosition.position, endMarker.position, fractionOfJourney);

            facingDirection = -1;
            wallCheckDistance = -0.55f;
            isFacingRight = false;
            Vector3 theScale = transform.localScale;
            theScale.x = -1;
            transform.localScale = theScale;
        }

        if (wallFlightLeft == true)
        {
            canChangeDirection = false;

            float distCovered = (0.3f - startTime) * 1;
            float fractionOfJourney = distCovered / journeyLength;
            playerPosition.transform.position = Vector3.Lerp(playerPosition.position, endMarker.position, fractionOfJourney);

            facingDirection = 1;
            wallCheckDistance = 0.55f;
            isFacingRight = true;
            Vector3 theScale = transform.localScale;
            theScale.x = 1;
            transform.localScale = theScale;
        }

        //Airtravel: Stop current airtravel when hitting wall

        if (isTouchingWall == true && wallFlightLeft == true
            || isTouchingWall == true && wallFlightRight == true)
        {
            StopCoroutine(WallFlightLeftTimer());
            StopCoroutine(WallFlightRightTimer());
            wallFlightLeft = false;
            wallFlightRight = false;
            walljumpwindow = false;
        }

        //Airtravel: Stop current airtravel when hitting ground

        if (isGrounded)
        {
            StopCoroutine(WallFlightLeftTimer());
            StopCoroutine(WallFlightRightTimer());
            StopCoroutine(keepWallsliding());
            wallFlightLeft = false;
            wallFlightRight = false;
            canChangeDirection = true;
            specialWallslide = false;
        }

        //SecialWallsliding: Stop the special wallsiding when grounded or not touching a wall

        if (isTouchingWall == false || isGrounded)
        {
            StopCoroutine(keepWallsliding());
            specialWallslide = false;
            walljumpwindow = false;
        }


        //SecialWallsliding: Keeps the functions of the coroutine in check

        if (walljumpwindow == true)
        {
            canWallJump = true;
            specialWallslide = true;
            canChangeDirection = false;

            //Before: if (rb.velocity.y < -wallSlidingSpeed) 
            if (rb.velocity.y <= 0 && isTouchingWall)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
            }
        }

        if (walljumpwindow == false)
        {
            StopCoroutine(keepWallsliding());
            canWallJump = false;
            specialWallslide = false;
        }

        if (specialWallslide == false)
        {
            StopCoroutine(keepWallsliding());
            walljumpwindow = false;
        }

    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
        Slinging();
        ExitSwing();

        Debug.Log(swingSpeed);


        //Swingspeed based on magnitude
        playerMagnitude = rb.velocity.magnitude;

        if (rb.velocity.magnitude > 0 && rb.velocity.magnitude < 3 && hasSwinged == false)
        {
           swingSpeed = 13.75f * 4;
        }

        if (rb.velocity.magnitude > 3 && rb.velocity.magnitude < 6 && hasSwinged == false)
        {
            swingSpeed = 15 * 4;
        }

        if (rb.velocity.magnitude > 6 && rb.velocity.magnitude < 9 && hasSwinged == false)
        {
            swingSpeed = 16.25f * 4;
        }

        if (rb.velocity.magnitude > 9 && rb.velocity.magnitude < 12 && hasSwinged == false)
        {
            swingSpeed = 17.5f * 4;
        }

        if (rb.velocity.magnitude > 12 && rb.velocity.magnitude < 15 && hasSwinged == false)
        {
            swingSpeed = 18.75f * 4;
        }

        if (rb.velocity.magnitude > 15 && rb.velocity.magnitude < 18 && hasSwinged == false)
        {
            swingSpeed = 20 * 4;
        }

        if (rb.velocity.magnitude > 18 && rb.velocity.magnitude < 21 && hasSwinged == false)
        {
            swingSpeed = 21.25f * 4;
        }

        if (rb.velocity.magnitude > 21 && rb.velocity.magnitude < 24 && hasSwinged == false)
        {
            swingSpeed = 22.5f * 4;
        }

        if (rb.velocity.magnitude > 24 && hasSwinged == false)
        {
            swingSpeed = 23.75f * 4;
        }

        //Check if decending after swingjump
        if (hasSwinged == true)
        {
            previousVelocityY = rb.velocity.y * Time.deltaTime;

            if (currentVelocityY > previousVelocityY)
            {
                isTravelingDown = true;
            }
        }
    }

    //Also related to decend-check after swingjump
    private void LateUpdate()
    {
        if (hasSwinged == true)
        {
            currentVelocityY = rb.velocity.y * Time.deltaTime;
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("YVelocity", rb.velocity.y);
        anim.SetBool("isLicking0", isLicking0);
        anim.SetBool("isLicking45", isLicking45);
        anim.SetBool("isLicking90", isLicking90);
        anim.SetBool("crouch", crouch);
        anim.SetBool("isCrouching", isCrouching);
        anim.SetBool("isSwinging", isSwinging);
        anim.SetBool("isSwingingNoInput", isSwingingNoInput);
        anim.SetBool("isWallSliding", specialWallslide);

    }

    private void CheckIfCrouching()
    {

        // If crouching, check to see if the character can stand up
        if (crouch)
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(CeilingCheck.position, k_CeilingRadius, whatIsGround))
            {
                crouch = true;
            }
        }

        // If crouching
        if (crouch)
        {
            if (!m_wasCrouching)
            {
                m_wasCrouching = true;
                OnCrouchEvent.Invoke(true);
            }

            // Reduce the speed by the crouchSpeed multiplier
            movementInputDirection *= m_CrouchSpeed;

            // Disable one of the colliders when crouching
            if (m_CrouchDisableCollider != null)
                m_CrouchDisableCollider.enabled = false;
        }
        else
        {
            // Enable the collider when not crouching
            if (m_CrouchDisableCollider != null)
                m_CrouchDisableCollider.enabled = true;

            if (m_wasCrouching)
            {
                m_wasCrouching = false;
                OnCrouchEvent.Invoke(false);
            }
        }
    }


    //Shortcut wallsliding
    private void CheckIfWallSliding()
    {
        //org also had: rb.velocity.y < 0
        if (isTouchingWall && movementInputDirection == facingDirection && specialWallslide == false && wallFlightRight == false && wallFlightLeft == false)
        {
            StartCoroutine(keepWallsliding());
        }
        else
        {
            //isWallSliding = false;
        }
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsWall);
        Physics2D.IgnoreLayerCollision(9, 8, true);
    }

    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        //**?
        if (isTouchingWall && wallFlightLeft == false && wallFlightRight == false)
        {
            //canWallJump = true;
            checkJumpMultiplier = false;
            //BARDENT checkJumpMultiplier = false;
        }

        if (amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }
    }

    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0 && wallFlightLeft == false && wallFlightRight == false)
        {
            Flip();

            movementSpeed = 4;
        }
        else if (!isFacingRight && movementInputDirection > 0 && wallFlightLeft == false && wallFlightRight == false)
        {
            Flip();

            movementSpeed = 4;
        }

        //Boolean for walking animation
        if (movementInputDirection == 1 && isGrounded || movementInputDirection == -1 && isGrounded)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void CheckInput()
    {
        if (canChangeDirection == true || wallFlightRight == false || wallFlightLeft == false)
        {
            //remove raw here for controller support
            movementInputDirection = Input.GetAxisRaw("Horizontal");
        }
        if (canChangeDirection == false)
        {
            if (facingDirection == -1 && walljumpwindow == true)
            {
                movementInputDirection = -1f;
            }

            if (facingDirection == -1 && wallFlightRight == true)
            {
                movementInputDirection = -0.9f;
            }

            if (facingDirection == 1 && walljumpwindow == true)
            {
                movementInputDirection = 1f;
            }

            if (facingDirection == 1 && wallFlightLeft == true)
            {
                movementInputDirection = 0.9f;
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || isGrounded && (amountOfJumpsLeft > 0 && isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        //if (Input.GetAxis("Horizontal") && isTouchingWall)
        //{
        //    if (!isGrounded && movementInputDirection != facingDirection)
        //    {
        //        canMove = false;
        //        canFlip = false;

        //        turnTimer = turnTimerSet;
        //    }
        //}

        if (!canMove)
        {
            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        //Fires once when jumping normally of the ground
        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }
    }

    private void CheckJump()
    {
        if (jumpTimer > 0)
        {

            //Should canWallJump == true be in the if statement?
            if (!isGrounded && isTouchingWall && facingDirection == 1 && walljumpwindow == true
                && wallFlightLeft == false && wallFlightRight == false)
            {
                StopCoroutine(keepWallsliding());

                specialWallslide = false;
                walljumpwindow = false;

                WallJumpRight();
            }


            if (!isGrounded && isTouchingWall && facingDirection == -1 && walljumpwindow == true
                && wallFlightLeft == false && wallFlightRight == false)
            {
                StopCoroutine(keepWallsliding());

                specialWallslide = false;
                walljumpwindow = false;

                WallJumpLeft();
            }

            else if (isGrounded)
            {
                NormalJump();
            }
        }
        if (isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }

        if (wallJumpTimer > 0)
        {
            if (hasWallJumped && movementInputDirection == -lastWallJumpDirection)
            {
                //fires once when jumping of a wall
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                hasWallJumped = false;
            }
            else if (wallJumpTimer <= 0)
            {
                hasWallJumped = false;
            }
            else
            {
                wallJumpTimer -= Time.deltaTime;
            }
        }

    }

    private void NormalJump()
    {
        if (canNormalJump)
        {
            //Fires once when jumping normally of the ground
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void WallJumpRight()
    {
        if (canWallJump)
        {
            //lastPos = Player.transform.position;
            wallFlightRight = true;
            StartCoroutine(WallFlightRightTimer());
            isTouchingWall = false;
            //isWallSliding = false;
            amountOfJumpsLeft = amountOfJumps;
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJumped = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;
        }
    }

    private void WallJumpLeft()
    {
        if (canWallJump)
        {
            wallFlightLeft = true;
            StartCoroutine(WallFlightLeftTimer());
            isTouchingWall = false;
            //isWallSliding = false;
            amountOfJumpsLeft = amountOfJumps;
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJumped = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;
        }
    }


    IEnumerator WallFlightRightTimer()
    {
        yield return new WaitForSeconds(0.3f);
        wallFlightRight = false;
        canChangeDirection = false;
        StopCoroutine(WallFlightRightTimer());
    }

    IEnumerator WallFlightLeftTimer()
    {
        yield return new WaitForSeconds(0.3f);
        wallFlightLeft = false;
        canChangeDirection = false;
        StopCoroutine(WallFlightLeftTimer());
    }

    IEnumerator keepWallsliding()
    {
        //isWallSliding = true;
        canChangeDirection = false;

        specialWallslide = true;
        walljumpwindow = true;

        yield return new WaitForSeconds(0.2f);
        specialWallslide = false;
        walljumpwindow = false;
        canChangeDirection = true;
        //isWallSliding = false;
        StopCoroutine(keepWallsliding());
    }

    private void ApplyMovement()
        {
        //In air without left or right input
        if (!isGrounded && movementInputDirection == 0 && line.GetComponent<LineRenderer>().enabled == false)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if (canMove && line.GetComponent<LineRenderer>().enabled == false) //Regular walking
        {
            //Happens always when grounded, and airborn with input apparently
            if (swingFall == false)
            { 
                rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
            }
        }

        //ANIMATION: Swinging with input
        if (line.GetComponent<LineRenderer>().enabled && movementInputDirection <= 1 ||
            line.GetComponent<LineRenderer>().enabled && movementInputDirection >= -1)
        {
            isSwinging = true;
            isSwingingNoInput = false;
            isLicking0 = false;
            isLicking45 = false;
            isLicking90 = false;
        }

        //ANIMATION: Swinging without input
        if (line.GetComponent<LineRenderer>().enabled && movementInputDirection == 0)
        {
            isSwingingNoInput = true;
            isSwinging = true;
            isLicking0 = false;
            isLicking45 = false;
            isLicking90 = false;
            joint.dampingRatio = 0.5f;
            joint.frequency = 10f;
        }

        //ANIMATION:Stop swinging
        if (!line.GetComponent<LineRenderer>().enabled && isSwinging == true || 
            !line.GetComponent<LineRenderer>().enabled && isSwingingNoInput == true)
        {
            isSwinging = false;
            isSwingingNoInput = false;
        }


        //Swinging momentum right
        if (line.GetComponent<LineRenderer>().enabled && movementInputDirection == 1 && !Input.GetButton("Jump"))
        {
            Vector2 SwingForce = new Vector2(20.0f, 5.0f);
            rb.AddForce(SwingForce, ForceMode2D.Force);
            joint.frequency = 0f;
            joint.dampingRatio = 0.5f;
        }

        //Swinging momentum left
        if (line.GetComponent<LineRenderer>().enabled && movementInputDirection == -1 && !Input.GetButton("Jump"))
        {
            Vector2 SwingForce = new Vector2(-20.0f, 5.0f);
            rb.AddForce(SwingForce, ForceMode2D.Force);
            joint.frequency = 0f;
            joint.dampingRatio = 0.5f;
        }

    }

    private void Flip()
    {
        if (canFlip && wallFlightLeft == false && wallFlightRight == false)
        {
            facingDirection *= -1;
            wallCheckDistance *= -1;
            isFacingRight = !isFacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        Gizmos.DrawLine(player_Direction.transform.position, player_Angle.transform.position);
    }

    public void Slinging()
    {
        //New stuff
        //Im working here

        if (line.GetComponent<LineRenderer>().enabled && !Input.GetButtonDown("Jump"))
        {
            //swingSpeed = rb.velocity.magnitude;
            player_Angle.transform.position = Player.transform.position;
            player_Direction.transform.position = Point_Forward.transform.position + Vector3.down * loweredVector;
            lerpEndMarker.transform.position = player_Direction.transform.position + Vector3.down * 1.2f;
        }

        if (line.GetComponent<LineRenderer>().enabled && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(0, 0);
            hasSwinged = true;
            ExitSwing();
        }
    }

    public void ExitSwing()
    {
        if (!isGrounded && hasSwinged == true || !isTouchingWall && hasSwinged == true)
        {

        Vector2 exitSling = player_Direction.transform.position - player_Angle.transform.position;
        rb.AddForce(exitSling * swingSpeed, ForceMode2D.Force);

            //when falling
            if (rb.velocity.y < 0)
            {
            swingSpeed *= ExitSwingDecreaseFlight;

                //when definately falling
                if (currentVelocityY > previousVelocityY)
                {
                    swingSpeed *= 5 * ExitSwingDecreaseFlight;
                    swingFall = true;
                    Debug.Log("peak");
                }

            }

            //when rising
            if (rb.velocity.y > 0)
            {
            swingSpeed *= ExitSwingDecreaseFall;
            }
        }

        if (isGrounded || isTouchingWall)
        {
            hasSwinged = false;
            isTravelingDown = false;
            swingFall = false;
        }

        //Lerp
        if (isTravelingDown == true)
        {
            float lerpDistCovered = (Time.time - lerpStartTime) * lerpSpeed;
            float lerpFractionOfJourney = lerpDistCovered / lerpJourneyLenght;

            player_Direction.transform.position = Vector2.Lerp(lerpStart.position, lerpStop.position, lerpFractionOfJourney);


            //player_Direction.transform.position = player_Direction.transform.position + Vector3.down * 2;
        }
    }

}
