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

    public Squisher squisher;

    bool canMoveLeft = false;
    bool canMoveRight = false;
    bool tryJump = false;
    bool trySquish = false;

    bool collidedWithGround = false;

    PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
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
        else if (trySquish && collidedWithGround)
        {
            Squish();
        }
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

    void OnSquishMovement(InputValue value)
    {
        trySquish = value.isPressed;
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
        collidedWithGround = false;
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Sqrt(jumpHeight * -3.0f * gravityValue));
        tryJump = false;
    }

    void Squish()
    {
        squisher.squishing = true;
        trySquish = false;
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

