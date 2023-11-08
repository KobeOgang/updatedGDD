using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    [System.Serializable]
    public class DamageEvent : UnityEvent<int , Vector2> { }
    public DamageEvent damageableHit;

    Animator anim;


    [SerializeField]
    private int _maxHealth;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
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
        set
        {
            _health = value;

            if(_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;
                                        
    [SerializeField]
    private bool isInvinsible = false;


  

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
            anim.SetBool("isAlive", value);
            Debug.Log("Is alive set "+ value);
        }
    }


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInvinsible)
        {
            if(timeSinceHit > invincibilityTime)
            {
                isInvinsible=false;
                timeSinceHit = 0;
            }
            timeSinceHit += Time.deltaTime;
        }
        
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvinsible)
        {
            Health -= damage;
            isInvinsible = true;

            anim.SetTrigger("hit");
            damageableHit.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);
        }
        return false;
    }

}
