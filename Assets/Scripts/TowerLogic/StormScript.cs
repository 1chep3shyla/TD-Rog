using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormScript : MonoBehaviour
{
    public float slowPower;
    public float freezeRadius;
    public List<EnemyMoving> enemiesInRange = new List<EnemyMoving>(); // Initialize the list here

    void Update()
    {
        enemiesInRange.Clear(); // Clear the list each frame before populating it

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, freezeRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyMoving enemyMoving = collider.GetComponent<EnemyMoving>();
                if (enemyMoving != null && !enemiesInRange.Contains(enemyMoving))
                {
                    enemiesInRange.Add(enemyMoving);
                }
            }
        }
        FreezeEnemies();
    }

    public void FreezeEnemies()
    {
        foreach (EnemyMoving enemy in enemiesInRange)
        {
            enemy.Slow(1, slowPower);
        }

        enemiesInRange.Clear(); // Clear the list after applying the freeze effect
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the freeze radius using Gizmos
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, freezeRadius);
    }
}