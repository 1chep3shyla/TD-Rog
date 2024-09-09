using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radius : MonoBehaviour
{
    public int damage;
    public int stanChance;
    public int fireDamage;
    public int critChance;
    public float explosionRadius = 2f;
    public GameObject particle;

    void Awake()
    {
        Instantiate(particle, transform.position, Quaternion.identity);
    }
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
        EnemyMoving enemyMove = enemy.GetComponent<EnemyMoving>();
        if (enemyHealth != null)
        {
            enemyHealth.DefaultAttack(damage, critChance);
        }
        int randomPoc = Random.Range(0, 100);
        if (randomPoc <= stanChance && stanChance !=0)
        {
            enemyMove.Stun(1f);
        }
        if (fireDamage != 0)
        {
            enemyHealth.SetOnFire(2.5f, fireDamage); // Specify fire duration and damage
        }
        // You can add additional effects here if needed

        // Destroy the bullet after causing damage to all enemies
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the attack radius using Gizmos
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}