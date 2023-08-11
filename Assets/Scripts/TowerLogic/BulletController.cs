using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeBull
{
    def,
    ice,
    light,
    giant
}

public class BulletController : MonoBehaviour
{
    public TypeBull type;
    public float speed = 10f;
    public int damage = 10;
    public GameObject hitEffectPrefab;
    private Transform target;
    public float powerOfIce;
    public float IceTimer;

    private Vector3 direction;

    public void Initialize(Transform bulletTarget, int bulletDamage)
    {
        target = bulletTarget;
        damage = bulletDamage;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);

        // If the bullet reaches its target, apply damage and destroy the bullet
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            Enemy enemyHealth = target.GetComponent<Enemy>();
            EnemyMoving enemyMove = target.GetComponent<EnemyMoving>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                if (type == TypeBull.ice)
                {
                    enemyMove.Slow(IceTimer, powerOfIce);
                }
            }

            Destroy(gameObject);
        }

        if (!IsVisibleOnScreen())
        {
            Destroy(gameObject);
        }
    }


    bool IsVisibleOnScreen()
    {
        // Check if the bullet is within the camera's view
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
    }
}