using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuhaAttack : MonoBehaviour
{
    public float attackRadius = 3f;
    public float attackSpeed = 1f;
    public int damage = 10;
    public Transform firePoint;
    public GameObject bulletPrefab;

    private float attackCooldown = 10f;
    public List<Transform> enemiesInRange = new List<Transform>();
    private Animator animator;
    public AudioClip hitSFX;

    void Start()
    {
        animator = GetComponent<Animator>();
        gameObject.GetComponent<CircleCollider2D>().radius = attackRadius*1.2f;
    }

    void Update()
    {
        if (CanAttack() )
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
        return enemiesInRange.Count > 0 && attackCooldown <= 0f;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Transform enemyTransform = collider.transform;
            enemiesInRange.Add(enemyTransform);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Transform enemyTransform = collider.transform;
            enemiesInRange.Remove(enemyTransform);
        }
    }

    void Attack()
    {
        if (attackCooldown <= 0f)
        {
            foreach (var target in enemiesInRange)
            {
                if (target != null)
                {
                    animator.SetTrigger("Attacking");
                    GameManager.Instance.aS.PlayOneShot(hitSFX);
                    GameManager.Instance.aS.pitch = Random.Range(0.8f, 1.1f);

                    // Instantiate a bullet at the firePoint position and rotate it towards the enemy
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                    BulletController bulletController = bullet.GetComponent<BulletController>();

                    if (bulletController != null)
                    {
                        bulletController.Initialize(target, damage);
                    }

                    attackCooldown = 1 / (attackSpeed + (attackSpeed * GameManager.Instance.buff[5] / 100));
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}