using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TouchingDirections : MonoBehaviour
{

 
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.02f;
    public float ceilingDistance = 0.05f;
    CapsuleCollider2D col;
    Animator anim;
    PlayerController controller;
    

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    [SerializeField]
    private bool _isGrounded;

    public bool IsGrounded { get 
        {
            return _isGrounded;
        } private set {
            _isGrounded = value;
            anim.SetBool("isGrounded", value);
        } }

    [SerializeField]
    private bool _isOnWall;
    //private Vector2 wallCheckDirection => controller.moveInput.x > 0 ? Vector2.right : Vector2.left;
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;



    public bool IsOnWall
    {
        get
        {
            return _isOnWall;
        }
        private set
        {
            _isOnWall = value;
            anim.SetBool("isOnWall", value);
        }
    }

    [SerializeField]
    private bool _isOnCeiling;

    public bool IsOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }
        private set
        {
            _isOnCeiling = value;
            anim.SetBool("isOnCeiling", value);
        }
    }

    private void Awake()
    {
    
        col = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
    }


    
    
    private void FixedUpdate()
    {
        IsGrounded = col.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = col.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = col.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }


}
