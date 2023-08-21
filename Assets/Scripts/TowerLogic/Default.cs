using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Default : MonoBehaviour
{
    public float attackRadius = 3f;
    public float attackSpeed = 1f; // Attacks per second
    public int damage = 10;
    public Transform firePoint;
    public GameObject bulletPrefab;

    private float attackCooldown = 0f;
    private List<Transform> enemiesInRange = new List<Transform>();
    public float slowPower;
    public int firePower;
    public int stanChance;
    private Transform currentTarget;

    void Update()
    {
        damage = gameObject.GetComponent<UpHave>().curDamage;
        attackSpeed = gameObject.GetComponent<UpHave>().curAttackSpeed;

        // If there's no current target or the current target is out of range, find a new one
        if (currentTarget == null || Vector3.Distance(transform.position, currentTarget.position) > attackRadius)
        {
            currentTarget = GetNearestEnemy();
        }

        if (CanAttack() && currentTarget != null)
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
            // Instantiate a bullet at the firePoint position and rotate it towards the enemy
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            CannonBull bulletCannonController = bullet.GetComponent<CannonBull>();

            if (bulletController != null)
            {
                bulletController.Initialize(currentTarget, damage);
                if (bulletController.type == TypeBull.ice)
                {
                    bulletController.powerOfIce = slowPower;
                }
                else if (bulletController.type == TypeBull.fire)
                {
                    bulletController.powerOfFire = firePower;
                }
                else if (bulletController.type == TypeBull.stan)
                {
                    bulletController.chanceStan = stanChance;
                }
            }
            else if (bulletCannonController != null)
            {
                bulletCannonController.enemyTarget = currentTarget;
                bulletCannonController.dmg = damage;
            }

            attackCooldown = attackSpeed;
        }
    }



    Transform GetNearestEnemy()
    {
        Transform nearestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (Transform enemy in enemiesInRange)
        {
            if (enemy == null)
                continue; // Skip destroyed enemies

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