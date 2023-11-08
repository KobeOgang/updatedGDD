using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    
    public UnityEvent<float, Vector2> damageableHit;

    Animator anim;


    [SerializeField]
    private float _maxHealth;
    public float MaxHealth
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
    private float _health = 100;

    public float Health
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

    public bool Hit(float damage, Vector2 knockback)
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
