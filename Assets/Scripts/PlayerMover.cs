using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMover : MonoBehaviour
{
    public FloatVariable groundSpeed;
    public FloatVariable jumpForce;
    public FloatVariable jumpTime;
    public FloatVariable maxFallSpeed;
    public BoxCollider2D myBoxCollider;
    public LayerMask groundMask;

    private Rigidbody2D rigidbody;
    
    private Vector2 inputVector;

    private float jumpTimerCounter;
    private bool isJumping;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();  
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(rigidbody.velocity.x, Mathf.Clamp(rigidbody.velocity.y, -maxFallSpeed.Value, maxFallSpeed.Value * 5));

        Move();   
    }

    void Move()
    {
        Vector2 movementDelta;

        movementDelta = new Vector2(inputVector.x * groundSpeed.Value, 0);

        rigidbody.velocity = new Vector2(movementDelta.x, rigidbody.velocity.y);
        Rotate(movementDelta);
    }

    public void OnMove(CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }

    public void Jump(CallbackContext context)
    {
        if(context.started)
        {
            if (isGrounded() && Time.timeScale > 0)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce.Value);
                isJumping = true;
                jumpTimerCounter = jumpTime.Value;
            }
        }

        if (context.performed)
        {
            if (jumpTimerCounter > 0 && isJumping == true)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce.Value);
                jumpTimerCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if(context.canceled) 
        {
            isJumping = false;
        }
    }

    private bool isGrounded()
    {
        return myBoxCollider.IsTouchingLayers(groundMask);
    }

    private void Rotate(Vector2 direction)
    {
        if (direction.x < 0)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, -180, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
        }
        else if (direction.x > 0)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
        }
    }
}
