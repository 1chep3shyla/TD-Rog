using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssasinTower : MonoBehaviour
{
    public float attackRadius = 3f;
    public float attackSpeed = 1f; // Attacks per second
    public int damage;
    public Transform firePoint;
    public GameObject bulletPrefab;

    private float attackCooldown = 0f;
    private List<Transform> enemiesInRange = new List<Transform>();
    public float slowPower;
    public float chance;
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
                    Transform target = enemyCollider.transform;

                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                    BulletController bulletController = bullet.GetComponent<BulletController>();
                    if (enemyCollider.gameObject.GetComponent<EnemyMoving>().typeEnemy != EnemyType.boss)
                    {
                        int random = Random.Range(1, (int)((float)100 / chance));
                        if (bulletController != null && random == 1)
                        {
                            bulletController.Initialize(target, 100000000);
                            Debug.Log("ÃŒŸÕ¿ﬂ ¿“¿ ¿");
                        }
                        else if (random != 1)
                        {
                            bulletController.Initialize(target, damage);
                        }

                        attackCooldown = attackSpeed;
                    }
                    else
                    {
                        int random = Random.Range(1, (int)(1000 / chance) );
                        if (bulletController != null && random == 1)
                        {
                            bulletController.Initialize(target, 100000000);
                            Debug.Log("ÃŒŸÕ¿ﬂ ¿“¿ ¿");
                        }
                        else if (random != 1)
                        {
                            bulletController.Initialize(target, damage);
                        }

                        attackCooldown = attackSpeed;
                    }

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