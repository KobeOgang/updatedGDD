using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    Vector2 moveInput;
    private SpriteRenderer sprite;



    public float CurrentMoveSpeed
    {
        get
        {
            if (IsMoving)
            {
                if (isRunning)
                {
                    return runSpeed;
                }
                else
                {
                    return walkSpeed;
                }
            }
            else
            {
                return 0;
            }
        }
    }

    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            anim.SetBool("isMoving", value);
        }
    }

    [SerializeField]
    private bool running = false;

    public bool isRunning
    {
        get
        {
            return running;
        }
        set
        {
            running = value;
            anim.SetBool("running", value);
        }
    }





    Rigidbody2D rb;
    Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        UpdateAnimationUpdate();
        anim.SetFloat("yVelocity", rb.velocity.y);
    }


    private void UpdateAnimationUpdate()
    {
        if (moveInput.x > 0f)
        {
            sprite.flipX = false;
        }
        else if (moveInput.x < 0f)
        {
            sprite.flipX = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            anim.SetBool("crouching", true);

            if (moveInput.x != 0f)
            {
                anim.SetBool("crouchwalk", true);
            }
            else
            {
                anim.SetBool("crouchwalk", false);
            }
        }
        else
        {
            anim.SetBool("crouching", false);
            anim.SetBool("crouchwalk", false);
        }

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        IsMoving = moveInput != Vector2.zero;

    }



    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isRunning = true;
        }
        else if (context.canceled)
        {
            isRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {

        }
    }

}
