using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), 
    typeof(TouchingDirections), typeof(Damageable))] //defult

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f; //走路速度
    public float runSpeed = 8f; //跑步
    public float airWalkSpeed = 3f; //空中平移速度
    public float jumpImpulse = 5f;
    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable damageable;
    public GameObject loseMenuUI;
    public GameObject healthBar;
    public GameObject winMenuUI;

    public float CurrentMoveSpeed { get {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning)
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
                        return airWalkSpeed;
                    }
                }
                else
                {
                    return 0;
                }
            }else return 0;
        } }


    [SerializeField]
    /*SerializeField : 表示变量可被序列化。众所周知，公有变量可以在检视面板中看到并编辑，而私有和保护变量不行。SerializeField与private，protected结合使用可以达到让脚本的变量在检视面板里可视化编辑，同时保持它的私有性的目的。 https://docs.unity3d.com/cn/2023.2/ScriptReference/SerializeField.html */
    private bool _isMoving = false;

    public bool IsMoving { get {
                return _isMoving;
            }
        private set {
                _isMoving = value;
                animator.SetBool(AnimationStrings.isMoving, value);
            }
        }

    [SerializeField]
    private bool _isRunning = false;

    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    //localScale：相对于父级的缩放比例
    public bool _isFacingRight = true; //defult向右
    private bool isFacingRight { get { return _isFacingRight; } 
        set
        {
            if(_isFacingRight != value) { 
                transform.localScale *= new Vector2(-1,1);
            }
            _isFacingRight = value;
        }
    }

    public bool CanMove {  get
        {
            return animator.GetBool(AnimationStrings.canMove);
        } }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    Rigidbody2D rb;
    Animator animator;

    private void Awake() //生命周期
      //当Component组件存在的时候将唤醒。start在唤醒之后，并且只使用一次
      //想要一些物品被第一时间发现的时候就放awake
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //当rigidbody被调用的时候，会使用unity中的预设
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    // Start is called before the first frame update
    void Start()//生命周期
    {
        
    }

    // Update is called once per frame
    //当MonoBehaviour启用时，其在每一帧被调用，都是用来更新的。 但是fixupdated时间是固定的（渲染）
    //Update受当前渲染的物体影响，这与当前场景中正在被渲染的物体有关（比如人物的面数，个数等），有时快有时慢，帧率会变化，Update被调用的时间间隔就会发生变化。但是FixedUpdate则不受帧率的变化影响，它是以固定的时间间隔来被调用。 
    //在官网文档中也提到说： 处理Rigidbody时，需要用FixedUpdate代替Update。例如:给刚体加一个作用力时，你必须应用作用力在FixedUpdate里的固定帧，而不是Update中的帧。(两者帧长不同)。正是因为FixedUpdate是以固定的时间间隔来被调用，所以人物运动起来比较平滑（运动起来不会有忽快忽慢的感觉，像是跳帧似的），这也就是为什么处理Rigidbody时要用FixedUpdate了（当然还有其他一些原因）。FixedUpdate是以固定的时间间隔来被调用的，而这个固定的时间是可以修改的。 
    void Update()
    {
        
    }

    public void FixedUpdate()
    {
        if(!damageable.IsHit)
            rb.velocity = new Vector2(moveInput.x* CurrentMoveSpeed/**Time.fixedDeltaTime*/, rb.velocity.y); //move=向量2，y=重力
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
        /*if (!IsAlive)
        {
            IsDead();
            
            //healthBar.SetActive(false);
            //Time.timeScale = 0f;
        }*/
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if(IsAlive)
        {
            IsMoving = moveInput != Vector2.zero; //moveinput不等于0时为真，等于0为假

            setFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }

    }

    private void setFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !isFacingRight) {
            //facing right
            isFacingRight = true;
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            //facing left
            isFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }else if (context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }


    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }

    public void OnHit (int damage, Vector2 knockback) {
        IsMoving = false; //受击时无法自主转向
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void IsDead()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        loseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("deadline");
        if (collision.tag == "DeadLine") damageable.Hit(100, new Vector2(0,0));
        if(collision.tag == "Win")
        {
            winMenuUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

}
