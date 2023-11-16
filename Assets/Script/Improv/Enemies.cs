using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class Enemies : MonoBehaviour
{
    public float walkAcceleration = 3f;
    public float maxSpeed = 3f;
    public float walkStopRate = 0.02f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;

    public enum WalkableDirection { Right, Left }
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set { 
            if(_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if(value == WalkableDirection.Right )
                {
                    walkDirectionVector = Vector2.right;
                }else if(value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            
            _walkDirection = value; }
    }

    public bool _hasTarget = false;
    public bool HasTarget { get { return _hasTarget; } private set { 
             _hasTarget = value;
            animator.SetBool("hasTarget", value);


        } }

    public bool CanMove
    {
        get
        {
            return animator.GetBool("canMove");
        }
    }

    public float AttackCooldown { get
        {
            return animator.GetFloat("attackCooldown");
           
        } private set {
            animator.SetFloat("attackCooldown", Mathf.Max(value, 0));
        }
    }
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;

        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }

        Attack();

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (touchingDirections.IsOnWall && touchingDirections.IsGrounded || cliffDetectionZone.detectedColliders.Count == 0)
        {
            FlipDirection();
        }

        

        if(CanMove)
        {
            
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed), rb.velocity.y);

        }
        else
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }


    }

    public void FlipDirection()
    {
        if(WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }else if(WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("wut why it not working");
        }
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y * knockback.y);
    }

    public void OnCliffDetected()
    {
        if(touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }


    void Attack()
    {
        if (HasTarget)
        {
            // Generate a random number (0, 1, or 2) to choose between Attack1, Attack2, and Attack3
            int randomAttack = UnityEngine.Random.Range(0, 3);

            // Check the current state of the boolean parameters
            bool currentAttack1 = animator.GetBool("Attack1");
            bool currentAttack2 = animator.GetBool("Attack2");
            bool currentAttack3 = animator.GetBool("Attack3");

            // Set the corresponding boolean parameter based on the random number
            if (randomAttack == 0 && !currentAttack1)
            {
                animator.SetBool("Attack1", true);
                animator.SetBool("Attack2", false);
                animator.SetBool("Attack3", false);
            }
            else if (randomAttack == 1 && !currentAttack2)
            {
                animator.SetBool("Attack1", false);
                animator.SetBool("Attack2", true);
                animator.SetBool("Attack3", false);
            }
            else if (randomAttack == 2 && !currentAttack3)
            {
                animator.SetBool("Attack1", false);
                animator.SetBool("Attack2", false);
                animator.SetBool("Attack3", true);
            }
        }
        else
        {
            // Reset the boolean parameters to allow future random selections
            animator.SetBool("Attack1", false);
            animator.SetBool("Attack2", false);
            animator.SetBool("Attack3", false);
        }

    }

}
