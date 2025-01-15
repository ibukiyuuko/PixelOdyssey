using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(TouchingDirections))]
public class Knight : MonoBehaviour
{
    public float walkAcceleration = 3f;
    public float maxSpeed = 3f;
    public float walkStopRate = 0.05f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;

    public enum WalkableDirection { Right, Left}

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set { 
            if(value != _walkDirection)
            {
                gameObject.transform.localScale *= new Vector2(-1, 1); //角色面向翻转
                if(value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            
            _walkDirection = value; }
    }

    public bool _hasTarget = false;

    public bool HasTarget {
        get
        {
            return _hasTarget;
        }
         private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
        }

    public bool CanMove
    {
        get { return animator.GetBool(AnimationStrings.canMove); }
    }

    /*public float AttackCooldown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.attackCoolDown);
        }
        private set {
            animator.SetFloat(AnimationStrings.attackCoolDown, Mathf.Max(value, 0));
        }
    }*/


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = attackZone.detectedcolliders.Count > 0;
        /*if(AttackCooldown > 0 ) 
            AttackCooldown -= Time.deltaTime;*/
    }

    public void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall || cliffDetectionZone.detectedcolliders.Count == 0)
        {
            FlipDirection();
        }
        if (CanMove && touchingDirections.IsGrounded)
            //float xVelocity = Mathf.Clamp(rb.velocity.x + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed), rb.velocity.y);
        else
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
    }

    private void FlipDirection()
    {
        if(WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }else if(WalkDirection == WalkableDirection.Left) { WalkDirection = WalkableDirection.Right; } else
        {
            Debug.LogError("Current walkable direction is not set to legal value to right or left.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        //IsMoving = false; //受击时无法自主转向
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }


}
