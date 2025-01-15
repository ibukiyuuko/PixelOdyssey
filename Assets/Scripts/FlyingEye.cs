using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public float flightSpeed = 2f;
    private float waypointReachedDistance = 0.1f;
    public DetectionZone biteDetectionZone;
    public List<Transform> waypoints;

    Damageable damageable;
    Animator animator;
    Rigidbody2D rb;

    Transform nextWaypoint;
    int waypointNum = 0;

    public bool _hasTarget = false;
    

    public bool HasTarget
    {
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

    private void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = biteDetectionZone.detectedcolliders.Count > 0;
    }

    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            rb.gravityScale = 2f;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void Flight()
    {
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);
        rb.velocity = directionToWaypoint * flightSpeed;
        UpdateDirection();

        if (distance <= waypointReachedDistance)
        {
            waypointNum++;
            if(waypointNum >= waypoints.Count)
            {
                waypointNum = 0;
            }
            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void UpdateDirection()
    {
        Vector3 localScale = transform.localScale;
        if(transform.localScale.x > 0)
        {
            //facing right
            if(rb.velocity.x < 0){
                transform.localScale = new Vector3(-1 * localScale.x,
                    localScale.y, localScale.z);
            }
        }
        else
        {
            //facing left
            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * localScale.x,
                    localScale.y, localScale.z);
            }
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        //IsMoving = false; //受击时无法自主转向
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

}

