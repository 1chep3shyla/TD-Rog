using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeBull
{
    def,//
    ice,//
    fire,//
    minHp, 
    stan,//
    gladiator, 
    portal, //
    radar, 
    poison,//
    thief,
    armorReduce,//
    deathBoom, //
    divine,//
    fury,
    moon,
    sun, 
    cum, 
    magnat,
    rage,
    light, 
    random,
    assasin
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
    public int powerOfPoison;
    public float PoisonTimer;
    public int ThiefPower;
    public float armorDis;
    public int boomDamage;
    private Vector3 direction;
    public int chanceStan;
    public int chancePortaling;
    public int furyCount;
    public float buffRadar;
    public int thiefCount;
    public float magnatTower;
    public float chanceAssasin;
    public int critChance;
    public GameObject cumGM;

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
                    enemyHealth.DefaultAttack(damage, critChance);
                }
                else if (type == TypeBull.def)
                {
                    enemyHealth.DefaultAttack(damage, critChance);
                }
                else if (type == TypeBull.fire)
                {
                    enemyHealth.SetOnFire(2.5f, powerOfFire); // Specify fire duration and damage
                    enemyHealth.DefaultAttack(damage, critChance);
                }
                else if (type == TypeBull.minHp)
                {
                    if ((int)((float)enemyHealth.maxHealth / (float)enemyHealth.health) > 0)
                    {
                        if ((int)((float)damage * ((float)enemyHealth.maxHealth * (float)enemyHealth.health)) > 0 && (int)((float)damage / ((float)enemyHealth.maxHealth * (float)enemyHealth.health)) < damage * 5)
                        {
                            enemyHealth.DefaultAttack((int)((float)damage * ((float)enemyHealth.maxHealth / (float)enemyHealth.health)), critChance);
                        }
                        else if ((int)((float)damage * ((float)enemyHealth.maxHealth / (float)enemyHealth.health)) > damage * 5)
                        {
                            enemyHealth.DefaultAttack(damage * 5, critChance);
                        }
                        else if ((int)((float)damage * ((float)enemyHealth.maxHealth / (float)enemyHealth.health)) < 0)
                        {
                            enemyHealth.DefaultAttack(1, critChance);
                        }
                    }
                }
                else if (type == TypeBull.stan)
                {
                    int randomPoc = Random.Range(0, 100);
                    if (randomPoc <= chanceStan)
                    {
                        enemyHealth.DefaultAttack(damage, critChance);
                        enemyMove.Stun(1.5f);
                    }
                    else
                    {
                        enemyHealth.DefaultAttack(damage, critChance);
                    }
                }
                else if (type == TypeBull.gladiator)
                {
                    if ((int)(damage * (float)GameManager.Instance.maxHealth / (float)GameManager.Instance.Health) < damage * 5)
                    {
                        enemyHealth.DefaultAttack((int)(damage * (float)GameManager.Instance.maxHealth / (float)GameManager.Instance.Health), critChance);
                    }
                    else
                    {
                        enemyHealth.DefaultAttack(damage * 5, critChance);
                    }
                }
                else if (type == TypeBull.portal)
                {
                    enemyHealth.DefaultAttack(damage, critChance);
                    enemyMove.Portaling(chancePortaling);
                }
                else if (type == TypeBull.radar)
                {
                    if (enemyMove.isSlowed == false)
                    {
                        enemyHealth.DefaultAttack(damage, critChance);
                    }
                    else
                    {
                        enemyHealth.DefaultAttack(damage + (int)((float)damage * buffRadar / 100), critChance);
                    }
                }
                else if (type == TypeBull.poison)
                {

                    enemyHealth.SetPoison(2.5f, powerOfPoison);
                    enemyHealth.DefaultAttack(damage, critChance);
                }
                else if (type == TypeBull.thief)
                {
                    enemyHealth.Thiefed(ThiefPower, thiefCount);
                    enemyHealth.DefaultAttack(damage, critChance);
                }
                else if (type == TypeBull.armorReduce)
                {
                    enemyHealth.ArmorReducePublic(armorDis);
                    enemyHealth.DefaultAttack(damage, critChance);
                }
                else if (type == TypeBull.deathBoom)
                {
                    enemyHealth.SetOnFire(2.5f, powerOfFire);
                    enemyHealth.BoomOn(boomDamage, powerOfFire);
                    enemyHealth.DefaultAttack(damage, critChance);
                }
                else if (type == TypeBull.divine)
                {
                    enemyHealth.DefaultAttack(damage, critChance);
                }
                else if (type == TypeBull.fury)
                {
                    enemyHealth.DefaultAttack(damage * furyCount, critChance);
                    Debug.Log(damage * furyCount);
                }
                else if (type == TypeBull.cum)
                {
                    Instantiate(cumGM, target.position, Quaternion.identity);
                    enemyHealth.DefaultAttack(damage, critChance);
                }
                else if (type == TypeBull.magnat)
                {
                    enemyHealth.DefaultAttack(damage+(int)((float)GameManager.Instance.Gold * magnatTower/100), critChance);
                }
                else if (type == TypeBull.assasin)
                {
                    enemyHealth.DefaultAttack(damage, critChance);
                }
                else if (type == TypeBull.moon)
                {
                    enemyHealth.DefaultAttack(damage, critChance);
                }
                else if (type == TypeBull.sun)
                {
                    enemyHealth.DefaultAttack(damage, critChance);
                }
                else if (type == TypeBull.random || type == TypeBull.rage)
                {
                    int randomDamage = Random.Range(damage, damage*5);
                    enemyHealth.DefaultAttack(randomDamage, critChance);
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