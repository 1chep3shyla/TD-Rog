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
    assasin,
    magnet,
    wind,
    storm,
    blockhead,
    gear,
    fairy,
    rascal
}

public class BulletController : MonoBehaviour
{
    public TypeBull type;
    public bool needRotate;
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
        if (target != null && transform.childCount > 0 && needRotate)
        {
            // Direction from the object to the target
            Vector3 direction = bulletTarget.position - transform.position;

            // Calculate the angle between the forward direction and the target direction in the X-Y plane
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply the rotation only on the Z axis
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void FixedUpdate()
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
                enemyHealth.SetBulletType(type);
                if (type == TypeBull.ice)
                {
                    enemyMove.Slow(IceTimer, powerOfIce);
                    enemyHealth.DefaultAttack(damage, (int)GameManager.Instance.buff[8]);
                }
                else if (type == TypeBull.def)
                {
                    enemyHealth.DefaultAttack(damage, (int)GameManager.Instance.buff[8]);
                }
                else if (type == TypeBull.fire)
                {
                    enemyHealth.SetOnFire(2.5f, powerOfFire); // Specify fire duration and damage
                    enemyHealth.DefaultAttack(damage, (int)GameManager.Instance.buff[8]);
                }
                else if (type == TypeBull.minHp)
                {
                    if ((int)((float)enemyHealth.maxHealth / (float)enemyHealth.health) > 0)
                    {
                        if ((int)((float)damage * ((float)enemyHealth.maxHealth * (float)enemyHealth.health)) > 0 && (int)((float)damage / ((float)enemyHealth.maxHealth * (float)enemyHealth.health)) < damage * 5)
                        {
                            enemyHealth.DefaultAttack((int)((float)damage * ((float)enemyHealth.maxHealth / (float)enemyHealth.health)), (int)GameManager.Instance.buff[8]);
                        }
                        else if ((int)((float)damage * ((float)enemyHealth.maxHealth / (float)enemyHealth.health)) > damage * 5)
                        {
                            enemyHealth.DefaultAttack(damage * 5, (int)GameManager.Instance.buff[8]);
                        }
                        else if ((int)((float)damage * ((float)enemyHealth.maxHealth / (float)enemyHealth.health)) < 0)
                        {
                            enemyHealth.DefaultAttack(1, (int)GameManager.Instance.buff[8]);
                        }
                    }
                }
                else if (type == TypeBull.stan)
                {
                    int randomPoc = Random.Range(0, 100);
                    if (randomPoc < chanceStan)
                    {
                        enemyHealth.DefaultAttack(damage, (int)GameManager.Instance.buff[8]);
                        enemyMove.Stun(1f+GameManager.Instance.secondsBuff[7]);
                    }
                    else
                    {
                        enemyHealth.DefaultAttack(damage, (int)GameManager.Instance.buff[8]);
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
                        enemyHealth.DefaultAttack(damage * 5, (int)GameManager.Instance.buff[8]);
                    }
                }
                else if (type == TypeBull.portal)
                {
                    enemyHealth.DefaultAttack(damage, (int)GameManager.Instance.buff[8]);
                    enemyMove.Portaling(chancePortaling);
                }
                else if (type == TypeBull.radar)
                {
                    if (enemyMove.isSlowed == false)
                    {
                        enemyHealth.DefaultAttack(damage, (int)GameManager.Instance.buff[8]);
                    }
                    else
                    {
                        enemyHealth.DefaultAttack(damage + (int)((float)damage * buffRadar / 100), (int)GameManager.Instance.buff[8]);
                    }
                }
                else if (type == TypeBull.poison)
                {

                    enemyHealth.SetPoison(2.5f, powerOfPoison);
                    enemyHealth.DefaultAttack(damage, (int)GameManager.Instance.buff[8]);
                }
                else if (type == TypeBull.thief)
                {
                    enemyHealth.Thiefed(ThiefPower, thiefCount);
                    enemyHealth.DefaultAttack(damage, (int)GameManager.Instance.buff[8]);
                }
                else if (type == TypeBull.armorReduce)
                {
                    enemyHealth.ArmorReducePublic(armorDis);
                    enemyHealth.DefaultAttack(damage, (int)GameManager.Instance.buff[8]);
                }
                else if (type == TypeBull.deathBoom)
                {
                    enemyHealth.SetOnFire(2.5f, powerOfFire);
                    enemyHealth.BoomOn(boomDamage, powerOfFire);
                    enemyHealth.DefaultAttack(damage, (int)GameManager.Instance.buff[8]);
                }
                else if (type == TypeBull.divine)
                {
                    enemyHealth.DefaultAttack(damage, (int)GameManager.Instance.buff[8]);
                }
                else if (type == TypeBull.fury)
                {
                    enemyHealth.DefaultAttack(damage * furyCount, (int)GameManager.Instance.buff[8]);
                    Debug.Log(damage * furyCount);
                }
                else if (type == TypeBull.cum)
                {
                    Instantiate(cumGM, target.position, Quaternion.identity);
                    enemyHealth.DefaultAttack(damage, (int)GameManager.Instance.buff[8]);
                }
                else if (type == TypeBull.magnat)
                {
                    enemyHealth.DefaultAttack(damage+(int)((float)GameManager.Instance.Gold * magnatTower/100), (int)GameManager.Instance.buff[8]);
                }
                else if (type == TypeBull.assasin)
                {
                    enemyHealth.DefaultAttack(damage, (int)GameManager.Instance.buff[8]);
                }
                else if (type == TypeBull.moon)
                {
                    enemyHealth.DefaultAttack(damage, (int)GameManager.Instance.buff[8]);
                }
                else if (type == TypeBull.sun)
                {
                    enemyHealth.DefaultAttack(damage, (int)GameManager.Instance.buff[8]);
                }
                else if (type == TypeBull.random || type == TypeBull.rage)
                {
                    int randomDamage = Random.Range(damage, damage*5);
                    enemyHealth.DefaultAttack(randomDamage, (int)GameManager.Instance.buff[8]);
                }
                else if( type == TypeBull.fairy)
                {
                    enemyHealth.DefaultAttack((int)((float)damage * 1.5f * (float)GameManager.Instance.curWave), (int)GameManager.Instance.buff[8]);
                }
                else if( type == TypeBull.rascal)
                {
                    enemyHealth.DefaultAttack(damage + (int)((float)damage * (0.25f * (float)enemyHealth.debuffCount)), (int)GameManager.Instance.buff[8]);
                }
                else
                {
                     enemyHealth.DefaultAttack(damage, (int)GameManager.Instance.buff[8]);
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