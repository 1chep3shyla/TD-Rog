using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTower : MonoBehaviour
{
    public float attackRadius = 3f;
    public float attackSpeed = 1f; // Attacks per second
    public int damage = 10;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public int chainHave;

    private float attackCooldown = 0f;
    private List<Transform> enemiesInRange = new List<Transform>();

    void Update()
    {
        damage = gameObject.GetComponent<UpHave>().curDamage;
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

        return enemiesInRange.Count >= chainHave; // Check if enough enemies are available for a chain attack
    }

    void Attack()
    {
        if (attackCooldown <= 0f)
        {
            Transform[] targetEnemyArray = GetTargetEnemies();

            if (targetEnemyArray.Length >= chainHave)
            {
                // Instantiate a bullet at the firePoint position and rotate it towards the enemy
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                LightBull bulletScript = bullet.GetComponent<LightBull>();
                bulletScript.targetEnemies = targetEnemyArray;
                bulletScript.damage = damage;

                attackCooldown = attackSpeed; // Reset attackCooldown based on attackSpeed
                enemiesInRange.RemoveAt(0); // Remove the first enemy from the list
            }
        }
    }

    Transform[] GetTargetEnemies()
    {
        List<Transform> targetEnemyList = new List<Transform>();

        for (int i = 0; i < chainHave && i < enemiesInRange.Count; i++)
        {
            targetEnemyList.Add(enemiesInRange[i]);
        }

        return targetEnemyList.ToArray();
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the attack radius using Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}