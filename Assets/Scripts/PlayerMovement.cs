using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;

    [Range(0f, 10f)]
    public float moveSpeed;

    [Range(0f, 90f)]
    public float angularSpeed;

    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    public float timeBetweenJumps = 3.0f;
    private float timeSinceLastJump = 0.0f;
    private bool canJump = true;

    bool canMoveLeft = false;
    bool canMoveRight = false;
    bool tryJump = false;

    bool collidedWithGround = false;

    PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        //controls.Gameplay.LeftMovement.performed += ctx => MoveLeft();
        //controls.Gameplay.RightMovement.performed += ctx => MoveRight();
    }

    // Start is called before the first frame update
    void Start()
    {
        //getting rigidbody component
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void FixedUpdate()
    {
        if (canMoveLeft)
        {
            MoveLeft();
        }
        else if (canMoveRight)
        {
            MoveRight();
        }
        
        if (tryJump && collidedWithGround)
        {
            Jump();
        }
        /*
        if (!canJump)
        {
            timeSinceLastJump += Time.deltaTime;
            if (timeSinceLastJump > timeBetweenJumps)
            {
                canJump = true;
                timeSinceLastJump = 0;
            }
        }
        */
    }

    void OnLeftMovement(InputValue value)
    {
        canMoveLeft = value.isPressed;
    }

    void OnRightMovement(InputValue value)
    {
        canMoveRight = value.isPressed;
    }

    void OnJumpMovement(InputValue value)
    {
        tryJump = value.isPressed;
    }

    void MoveLeft()
    {
        //add negative velocity to go left
        rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        //rb.angularVelocity = -angularSpeed;
    }

    void MoveRight()
    {

        //add positive velocity to go right
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        //rb.angularVelocity = angularSpeed * 2;
    }

    void Jump()
    {

        rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(jumpHeight * -3.0f * gravityValue));
        tryJump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            collidedWithGround = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            collidedWithGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            collidedWithGround = false;
        }
    }
}

