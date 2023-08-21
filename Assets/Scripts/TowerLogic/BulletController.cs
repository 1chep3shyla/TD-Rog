using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeBull
{
    def,
    ice,
    fire,
    minHp, 
    stan,
    gladiator
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
    public int powerOfFire; 
    public float FireTimer;
    private Vector3 direction;
    public int chanceStan;

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
                if (type == TypeBull.ice)
                {
                    enemyMove.Slow(IceTimer, powerOfIce);
                    enemyHealth.TakeDamage(damage);
                }
                else if (type == TypeBull.def)
                {
                    enemyHealth.TakeDamage(damage);
                }
                else if (type == TypeBull.fire)
                {
                    enemyHealth.SetOnFire(2.5f, powerOfFire); // Specify fire duration and damage
                    enemyHealth.TakeDamage(damage);
                }
                else if (type == TypeBull.minHp)
                {
                    if ((int)((float)enemyHealth.maxHealth / (float)enemyHealth.health) > 0)
                    {
                        if ((int)((float)damage * ((float)enemyHealth.maxHealth * (float)enemyHealth.health)) > 0 && (int)((float)damage / ((float)enemyHealth.maxHealth * (float)enemyHealth.health)) < damage * 5)
                        {
                            enemyHealth.TakeDamage((int)((float)damage * ((float)enemyHealth.maxHealth / (float)enemyHealth.health)));
                        }
                        else if ((int)((float)damage * ((float)enemyHealth.maxHealth / (float)enemyHealth.health)) > damage * 5)
                        {
                            enemyHealth.TakeDamage(damage * 5);
                        }
                        else if ((int)((float)damage * ((float)enemyHealth.maxHealth / (float)enemyHealth.health)) < 0)
                        {
                            enemyHealth.TakeDamage(1);
                        }
                    }
                }
                else if (type == TypeBull.stan)
                {
                    int randomPoc = Random.Range(0, 100);
                    if (randomPoc <= chanceStan)
                    {
                        enemyHealth.TakeDamage(damage);
                        enemyMove.Stun(1.5f);
                    }
                    else
                    {
                        enemyHealth.TakeDamage(damage);
                    }
                }
                else if (type == TypeBull.gladiator)
                {
                    if ((int)(damage * (float)GameManager.Instance.maxHealth / (float)GameManager.Instance.Health) < damage * 5)
                    {
                        enemyHealth.TakeDamage((int)(damage * (float)GameManager.Instance.maxHealth / (float)GameManager.Instance.Health));
                    }
                    else
                    {
                        enemyHealth.TakeDamage(damage * 5);
                    }
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