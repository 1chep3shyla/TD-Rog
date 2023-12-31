using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTower : MonoBehaviour
{
    public float attackRadius = 3f;
    public float attackSpeed = 1f; // Attacks per second
    public int mindamage;
    public int maxDamage;
    public Transform firePoint;
    public GameObject bulletPrefab;

    private float attackCooldown = 0f;
    private List<Transform> enemiesInRange = new List<Transform>();
    public float slowPower;

    void Update()
    {
        if ((int)(gameObject.GetComponent<UpHave>().curDamage * 0.25f) != 0)
        {
            mindamage = (int)(gameObject.GetComponent<UpHave>().curDamage * 0.25f);
        }
        else
        {
            mindamage = 1;
        }
        maxDamage = gameObject.GetComponent<UpHave>().curDamage;
        attackSpeed = gameObject.GetComponent<UpHave>().curAttackSpeed;
        if (CanAttack())
        {
            Attack();
        }
        if (attackCooldown >= 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }


    bool CanAttack()
    {
        // Clean up the enemies list by removing destroyed or out-of-range enemies
        enemiesInRange.RemoveAll(enemy => enemy == null || Vector3.Distance(transform.position, enemy.position) > attackRadius);

        // Add new enemies that entered the radius
        foreach (Collider2D enemyCollider in Physics2D.OverlapCircleAll(transform.position, attackRadius))
        {
            if (enemyCollider.CompareTag("Enemy") && !enemiesInRange.Contains(enemyCollider.transform))
            {
                enemiesInRange.Add(enemyCollider.transform);
            }
        }

        return enemiesInRange.Count > 0;
    }

    void Attack()
    {
        if (attackCooldown <= 0f)
        {
            // Find and attack an enemy within the attack radius
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRadius);
            foreach (Collider2D enemyCollider in enemies)
            {
                if (enemyCollider.CompareTag("Enemy"))
                {
                    Transform target = enemyCollider.transform; // Get the enemy's transform

                    // Instantiate a bullet at the firePoint position and rotate it towards the enemy
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                    BulletController bulletController = bullet.GetComponent<BulletController>();
                    int randomDamage = Random.Range(mindamage, maxDamage);
                    if (bulletController != null)
                    {
                        bulletController.Initialize(target, randomDamage);
                    }

                    attackCooldown = attackSpeed;
                    break;
                }
            }
        }
    }


    Transform GetNearestEnemy()
    {
        Transform nearestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (Transform enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(firePoint.position, enemy.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the attack radius using Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}