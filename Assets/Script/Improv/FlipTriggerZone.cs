using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipTriggerZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider is an enemy
        Enemies enemy = other.GetComponent<Enemies>();
        if (enemy != null)
        {
            // Flip the enemy's direction
            enemy.FlipDirection();
        }
    }
}
