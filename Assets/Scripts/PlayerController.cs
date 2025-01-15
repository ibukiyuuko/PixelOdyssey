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
    public float walkSpeed = 5f; //��·�ٶ�
    public float runSpeed = 8f; //�ܲ�
    public float airWalkSpeed = 3f; //����ƽ���ٶ�
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
    /*SerializeField : ��ʾ�����ɱ����л���������֪�����б��������ڼ�������п������༭����˽�кͱ����������С�SerializeField��private��protected���ʹ�ÿ��Դﵽ�ýű��ı����ڼ����������ӻ��༭��ͬʱ��������˽���Ե�Ŀ�ġ� https://docs.unity3d.com/cn/2023.2/ScriptReference/SerializeField.html */
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

    //localScale������ڸ��������ű���
    public bool _isFacingRight = true; //defult����
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

    private void Awake() //��������
      //��Component������ڵ�ʱ�򽫻��ѡ�start�ڻ���֮�󣬲���ֻʹ��һ��
      //��ҪһЩ��Ʒ����һʱ�䷢�ֵ�ʱ��ͷ�awake
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //��rigidbody�����õ�ʱ�򣬻�ʹ��unity�е�Ԥ��
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    // Start is called before the first frame update
    void Start()//��������
    {
        
    }

    // Update is called once per frame
    //��MonoBehaviour����ʱ������ÿһ֡�����ã������������µġ� ����fixupdatedʱ���ǹ̶��ģ���Ⱦ��
    //Update�ܵ�ǰ��Ⱦ������Ӱ�죬���뵱ǰ���������ڱ���Ⱦ�������йأ���������������������ȣ�����ʱ����ʱ����֡�ʻ�仯��Update�����õ�ʱ�����ͻᷢ���仯������FixedUpdate����֡�ʵı仯Ӱ�죬�����Թ̶���ʱ�����������á� 
    //�ڹ����ĵ���Ҳ�ᵽ˵�� ����Rigidbodyʱ����Ҫ��FixedUpdate����Update������:�������һ��������ʱ�������Ӧ����������FixedUpdate��Ĺ̶�֡��������Update�е�֡��(����֡����ͬ)��������ΪFixedUpdate���Թ̶���ʱ�����������ã����������˶������Ƚ�ƽ�����˶����������к�������ĸо���������֡�Ƶģ�����Ҳ����Ϊʲô����RigidbodyʱҪ��FixedUpdate�ˣ���Ȼ��������һЩԭ�򣩡�FixedUpdate���Թ̶���ʱ�����������õģ�������̶���ʱ���ǿ����޸ĵġ� 
    void Update()
    {
        
    }

    public void FixedUpdate()
    {
        if(!damageable.IsHit)
            rb.velocity = new Vector2(moveInput.x* CurrentMoveSpeed/**Time.fixedDeltaTime*/, rb.velocity.y); //move=����2��y=����
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
            IsMoving = moveInput != Vector2.zero; //moveinput������0ʱΪ�棬����0Ϊ��

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
        IsMoving = false; //�ܻ�ʱ�޷�����ת��
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
