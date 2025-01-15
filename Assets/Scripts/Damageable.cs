using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit; //做击退效果不改变rigidbody的值，而是直接使用unity事件改变动画
    public UnityEvent<int, int> healthChanged;

    Animator animator;

    [SerializeField]
    public int _maxhealth = 100;

    public int Maxhealth
    {
        get
        {
            return _maxhealth;
        }
        set
        {
            _maxhealth = value;
        }
    }

    [SerializeField]
    private int _health = 100;
    public int Health
    {
        get
        {
            return _health;
        }
        set {
            _health = value;
            healthChanged?.Invoke(_health, Maxhealth);
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;

    [SerializeField]
    private bool isInvincible = false;

    public bool IsHit { get {
            return animator.GetBool(AnimationStrings.isHit);
        } private set {
            animator.SetBool(AnimationStrings.isHit, value);
        } }

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            //Debug.Log("IsAlive set " +  value);
        }
    }


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (isInvincible) {
            if(timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if(IsAlive && !isInvincible){
            if(Health - damage > 0)
                Health -= damage;
            else Health = 0;
            isInvincible = true; //当生命低于0之后就无法被攻击

            IsHit = true;
            damageableHit?.Invoke(damage, knockback); //受攻击击退
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);

            return true;
        }
        return false;
    }
    
    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < Maxhealth)
        {
            int maxHeal = Mathf.Max(Maxhealth - Health, 0);
            //Health = Mathf.Min(Health + healthRestore, Maxhealth);
            int acturalHeal = Mathf.Min(maxHeal, healthRestore);
            //Health += Mathf.Min(maxHeal, healthRestore);
            Health += acturalHeal;

            CharacterEvents.characterHealed(gameObject, acturalHeal);
            return true;
        }
        return false;
    }

}
