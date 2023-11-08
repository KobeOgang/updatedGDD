using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public Vector2 moveInput;
    private SpriteRenderer sprite;
    TouchingDirections touch;
    public float jumpImpulse = 10f;

    public CapsuleCollider2D playerCollider;
    public float crouchingColliderHeight = 0.2f;
    public float originalHeight = 0.5f;

    public Transform swordAttack;





    public float CurrentMoveSpeed { get
        {
            if (CanMove)
            {
                if (IsMoving && !touch.IsOnWall)
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
            else
            {
                return 0;
            }
            
        } }

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

    public bool CanMove { get { return anim.GetBool("canMove"); } }
    public bool IsAlive
    {
        get
        {
            return anim.GetBool("isAlive");
        }
    }

    public bool LockedVelocity { get {
            return anim.GetBool("lockVelocity");
        } }


    public bool _isFacingRight = true;
    public bool IsFacingRight { get { return _isFacingRight; } private set {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight=value;


        } }

    Rigidbody2D rb;
    Animator anim;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent <SpriteRenderer>();
        touch = GetComponent<TouchingDirections>();

        playerCollider = GetComponent<CapsuleCollider2D>();

        swordAttack = transform.Find("SwordAttack");


    }


    private void FixedUpdate()
    {
        if(!LockedVelocity)
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }

        
        UpdateAnimationUpdate();
        anim.SetFloat("yVelocity", rb.velocity.y);

        if (Input.GetKey(KeyCode.S))
        {
            Crouch();
        }
        else
        {
            StandUp();
        }

    }

    private void Crouch()
    {
        anim.SetBool("crouching", true);

        playerCollider.size = new Vector2(playerCollider.size.x, 0.19f);
        playerCollider.offset = new Vector2(0f, -0.07f);


        if (moveInput.x != 0f)
        {
            anim.SetBool("crouchwalk", true);
        }
        else
        {
            anim.SetBool("crouchwalk", false);
        }
    }

    private void StandUp()
    {
        anim.SetBool("crouching", false);

        playerCollider.size = new Vector2(playerCollider.size.x, 0.29f);
        playerCollider.offset = new Vector2(0f, -0.02f);

        anim.SetBool("crouchwalk", false);
    }

    private void UpdateAnimationUpdate()
    {

        if(IsAlive)
        {
            if (moveInput.x > 0f)
            {
                sprite.flipX = false;
                swordAttack.localPosition = new Vector3(0f, swordAttack.localPosition.y, swordAttack.localPosition.z);
            }
            else if (moveInput.x < 0f)
            {
                sprite.flipX = true;
                swordAttack.localPosition = new Vector3(-0.5f, swordAttack.localPosition.y, swordAttack.localPosition.z);
            }
        }
          
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            //SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
             
     
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Mathf.Abs(transform.localScale.z));
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, -Mathf.Abs(transform.localScale.z));
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            isRunning = true;
        }else if(context.canceled)
        {
            isRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touch.IsGrounded && CanMove)
        {
            anim.SetTrigger("Jumping");
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            anim.SetTrigger("attack");
        }
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y * knockback.y);
    }

}
