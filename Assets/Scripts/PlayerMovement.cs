using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public PlayerMovementStats moveStats;
    [SerializeField] private Collider2D feetCollider;
    [SerializeField] private Collider2D bodyCollider;
    public Vector3Variable playerPos;

    private Rigidbody2D rigidbody;

    //Animation vars
    private Animator animator;

    //Movement Vars
    private Vector2 moveVelocity;
    private bool isFacingRight;

    //Collision Check Vars
    private RaycastHit2D groundHit;
    private RaycastHit2D headHit;
    private bool isGrounded;
    private bool bumpedHead;

    //Jump Vars
    public float verticalVelocity { get; private set; }
    private bool isJumping;
    private bool isFastFalling;
    private bool isFalling;
    private float fastFallTime;
    private float fastFallReleaseSpeed;
    private int numberOfJumpsUsed;

    //Apex vars
    private float apexPoint;
    private float timePastApexThreshold;
    private bool isPastApexThreshold;

    //Jump buffer vars
    private float jumpBufferTimer;
    private bool jumpReleasedDuringBuffer;

    //Coyote time vars
    private float coyoteTimer;

    // Start is called before the first frame update
    void Awake()
    {
        isFacingRight = true;

        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        playerPos.Value = transform.position;

        CountTimers();
        JumpChecks();
    }

    private void FixedUpdate()
    {
        CollisionChecks();
        Jump();

        if (isGrounded)
        {
            Move(moveStats.GroundAcceleration, moveStats.GroundDeceleration, InputManager.Movement);
        }
        else
        {
            Move(moveStats.AirAcceleration, moveStats.AirDeceleration, InputManager.Movement);
        }
    }

    #region Movement

    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if(moveInput != Vector2.zero)
        {
            animator.SetBool("isRunning", true);
            TurnCheck(moveInput);

            Vector2 targetVelocity = Vector2.zero;
            targetVelocity = new Vector2(moveInput.x, 0f) * moveStats.MaxWalkSpeed;

            moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            rigidbody.velocity = new Vector2(moveVelocity.x, rigidbody.velocity.y);
        }
        else if(moveInput == Vector2.zero)
        {
            animator.SetBool("isRunning", false);
            moveVelocity = Vector2.Lerp(moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            rigidbody.velocity = new Vector2(moveVelocity.x, rigidbody.velocity.y);
        }
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if (isFacingRight && moveInput.x < 0)
            Turn(false);
        else if (!isFacingRight && moveInput.x > 0)
            Turn(true);
    }

    private void Turn(bool turnRight)
    {
        if (turnRight)
        {
            isFacingRight = true;
            transform.Rotate(0, 180f, 0);
        }
        else
        {
            isFacingRight = false;
            transform.Rotate(0, -180f, 0); 
        }
    }

    #endregion

    #region Jump

    private void JumpChecks()
    {
        //When we press the button
        if (InputManager.JumpWasPressed)
        {
            jumpBufferTimer = moveStats.JumpBufferTime;
            jumpReleasedDuringBuffer = false;
        }

        //When we release the button
        if (InputManager.JumpWasReleased)
        {
            if(jumpBufferTimer > 0f)
            {
                jumpReleasedDuringBuffer = true;
            }

            if(isJumping && verticalVelocity > 0f)
            {
                if (isPastApexThreshold)
                {
                    isPastApexThreshold = false;
                    isFastFalling = true;
                    fastFallTime = moveStats.TimeForUpwardsCancel;
                    verticalVelocity = 0f;
                }
                else
                {
                    isFastFalling = true;
                    fastFallReleaseSpeed = verticalVelocity;
                }
            }
        }

        //Initiate jump with buffering and coyote time
        if(jumpBufferTimer > 0f && !isJumping && (isGrounded || coyoteTimer > 0f))
        {
            InitiateJump(1);

            if (jumpReleasedDuringBuffer)
            {
                isFastFalling = true;
                fastFallReleaseSpeed = verticalVelocity;
            }
        }

        //Landed
        if((isJumping || isFalling) && isGrounded && verticalVelocity <= 0)
        {
            isJumping = false;
            isFalling = false;
            isFastFalling = false;
            fastFallTime = 0;
            isPastApexThreshold = false;
            numberOfJumpsUsed = 0;

            verticalVelocity = Physics2D.gravity.y;
        }
    }

    private void InitiateJump(int a_numberOfJumpsUsed)
    {
        if (!isJumping)
        {
            isJumping = true;
        }

        jumpBufferTimer = 0;
        numberOfJumpsUsed += a_numberOfJumpsUsed;
        verticalVelocity = moveStats.InitialJumpVelocity;
    }

    private void Jump()
    {
        //apply gravity while jumping
        if (isJumping)
        {
            //check for head bump
            if (bumpedHead)
            {
                isFastFalling = true;
            }

            //gravity on ascending
            if(verticalVelocity >= 0f)
            {
                //apex controls
                apexPoint = Mathf.InverseLerp(moveStats.InitialJumpVelocity, 0, verticalVelocity);

                if(apexPoint > moveStats.ApexThreshold)
                {
                    if (!isPastApexThreshold)
                    {
                        isPastApexThreshold = true;
                        timePastApexThreshold = 0f;
                    }

                    if (isPastApexThreshold)
                    {
                        timePastApexThreshold += Time.deltaTime;
                        if(timePastApexThreshold < moveStats.ApexHangTime)
                        {
                            verticalVelocity = 0f;
                        }
                        else
                        {
                            verticalVelocity = -0.01f;
                        }
                    }
                }

                //gravity on ascending but not past apex threshold
                else
                {
                    verticalVelocity += moveStats.Gravity * Time.fixedDeltaTime;
                    if (isPastApexThreshold)
                    {
                        isPastApexThreshold = false;
                    }
                }

            }

            //Gravity on descending
            else if (!isFastFalling)
            {
                verticalVelocity += moveStats.Gravity * moveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }

            else if(verticalVelocity < 0f)
            {
                if (!isFalling)
                {
                    isFalling = true;
                }
            }
        }

        //jump cut
        if (isFastFalling)
        {
            if(fastFallTime >= moveStats.TimeForUpwardsCancel)
            {
                verticalVelocity += moveStats.Gravity * moveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }
            else if(fastFallTime < moveStats.TimeForUpwardsCancel)
            {
                verticalVelocity = Mathf.Lerp(fastFallReleaseSpeed, 0, (fastFallTime / moveStats.TimeForUpwardsCancel));
            }

            fastFallTime += Time.fixedDeltaTime;
        }

        //Normal gravity while falling
        if(!isGrounded && !isJumping)
        {
            if (!isFalling)
            {
                isFalling = true;
            }

            verticalVelocity += moveStats.Gravity * Time.fixedDeltaTime;
        }

        //clamp fall speed

        verticalVelocity = Mathf.Clamp(verticalVelocity, -moveStats.MaxFallSpeed, moveStats.MaxFallSpeed);

        rigidbody.velocity = new Vector2(rigidbody.velocity.x, verticalVelocity);
    }

    #endregion

    #region Collision Checks

    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(feetCollider.bounds.center.x, feetCollider.bounds.min.y);
        Vector2 boxCastSize = new Vector2(feetCollider.bounds.size.x, moveStats.GroundDetectionRayLength);

        groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down,moveStats.GroundDetectionRayLength, moveStats.GroundLayer);
        if(groundHit.collider != null)
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
        }
        else
        {
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
    }

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(feetCollider.bounds.center.x, feetCollider.bounds.max.y);
        Vector2 boxCastSize = new Vector2(feetCollider.bounds.size.x * moveStats.HeadWidth, moveStats.HeadDetectionRayLength);

        headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, moveStats.HeadDetectionRayLength, moveStats.GroundLayer);
        if (headHit.collider != null)
        {
            bumpedHead = true;
        }
        else
        {
            bumpedHead = false;
        }
    }

    private void CollisionChecks()
    {
        BumpedHead();
        IsGrounded();
    }
    #endregion

    #region Timers

    private void CountTimers()
    {
        jumpBufferTimer -= Time.deltaTime;

        if (!isGrounded)
        {
            coyoteTimer -= Time.deltaTime;
        }
        else
        {
            coyoteTimer = moveStats.JumpCoyoteTime;
        }
    }

    #endregion
}
