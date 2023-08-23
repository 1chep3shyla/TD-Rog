using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfTower : MonoBehaviour
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
    public int poisonPower;
    public int stanChance;
    public int portalChange;
    public bool charge;
    public int maxTargets = 1; // Maximum number of targets that can be attacked simultaneously
    private List<Transform> currentTargets = new List<Transform>();

    void Update()
    {
        damage = gameObject.GetComponent<UpHave>().curDamage;
        if (!charge)
        {
            attackSpeed = gameObject.GetComponent<UpHave>().curAttackSpeed;
        }

        // If there are no current targets or the current targets are out of range, find new ones

            FindNewTargets();
        

        if (CanAttack() && currentTargets.Count > 0)
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

        return currentTargets.Count > 0;
    }

    bool AreTargetsInRange()
    {
        // Check if any of the current targets are in attack range
        foreach (Transform target in currentTargets)
        {
            if (target != null && Vector3.Distance(transform.position, target.position) <= attackRadius)
            {
                return true;
            }
        }
        return false;
    }

    void FindNewTargets()
    {
        enemiesInRange.RemoveAll(enemy => enemy == null || Vector3.Distance(transform.position, enemy.position) > attackRadius);

        // Add new enemies that entered the radius up to the max targets
        int targetsRemaining = maxTargets - currentTargets.Count;
        foreach (Collider2D enemyCollider in Physics2D.OverlapCircleAll(transform.position, attackRadius))
        {
            if (enemyCollider.CompareTag("Enemy") && !enemiesInRange.Contains(enemyCollider.transform) && targetsRemaining > 0)
            {
                enemiesInRange.Add(enemyCollider.transform);
                targetsRemaining--;
            }
        }

        currentTargets.Clear();
        currentTargets.AddRange(enemiesInRange);
    }

    void Attack()
    {
        if (attackCooldown <= 0f)
        {
            foreach (var target in currentTargets)
            {
                if (target != null)
                {
                    // Instantiate a bullet at the firePoint position and rotate it towards the enemy
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                    BulletController bulletController = bullet.GetComponent<BulletController>();
                    CannonBull bulletCannonController = bullet.GetComponent<CannonBull>();

                    if (bulletController != null)
                    {
                        bulletController.Initialize(target, damage);
                        // Set other bullet parameters here based on the tower's attributes
                    }
                    else if (bulletCannonController != null)
                    {
                        bulletCannonController.enemyTarget = target;
                        bulletCannonController.dmg = damage;
                    }
                }
            }

            attackCooldown = attackSpeed;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the attack radius using Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}