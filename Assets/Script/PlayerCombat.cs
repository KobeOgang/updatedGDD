using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator anim;
    private float dirX = 0f;
    private SpriteRenderer sprite;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 50;
    private bool isAttacking = false;

    void Update()
    {
        if (!isAttacking)
        {
            dirX = Input.GetAxisRaw("Horizontal");
            // Your movement logic here
        }

        Attack();
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking) // 0 represents the left mouse button
        {
            isAttacking = true;
            anim.SetBool("attacking", true);

            // Check if facing left while attacking and flip the sprite.
            if (dirX < 0f)
            {
                sprite.flipX = true;
            }

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                Debug.Log("We hit");
                enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }
        else
        {
            isAttacking = false;
            anim.SetBool("attacking", false);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;


            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


}
