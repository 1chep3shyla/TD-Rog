using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radius : MonoBehaviour
{
    public int damage ;
    public float explosionRadius = 2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            CauseDamage(other.gameObject);
        }
    }

    private void CauseDamage(GameObject enemy)
    {
        // Apply damage to the enemy
        Enemy enemyHealth = enemy.GetComponent<Enemy>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        // You can add additional effects here if needed

        // Destroy the bullet after causing damage to all enemies
        Destroy(gameObject);
    }
}