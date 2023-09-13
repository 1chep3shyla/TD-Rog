using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageTower : MonoBehaviour
{
    public float attackRadius = 3f;
    public float attackSpeed = 1f;
    public int damage = 10;
    public Transform firePoint;
    public GameObject bulletPrefab;

    private float attackCooldown = 10f;
    List<Transform> enemiesInRange = new List<Transform>();
    public float slowPower;
    public int firePower;
    public int poisonPower;
    public int stanChance;
    public int portalChange;
    public bool charge;
    public int maxTargets = 1;
    public List<Transform> currentTargets = new List<Transform>();
    public int thiefPower;
    public int dmgBoom;
    public float reduceArmor;
    public int chanceDivine;
    public int dmgDivine;
    public int countOfAttack;
    public float SlpashRadius;
    public float radarIncrease;
    public int thiefReward;
    public float magnatPower;
    private Transform currentTarget;
    [SerializeField]
    private GameObject DivineAttackGM;
    public DataTower dt;
    private int lvl;
    private UpHave upHaveScript;

    void Start()
    {
        upHaveScript = gameObject.GetComponent<UpHave>();
        dt = upHaveScript.towerDataCur;

        UpdateStat();
    }

    void Update()
    {
        damage = upHaveScript.curDamage;
        attackSpeed = upHaveScript.curAttackSpeed;
        lvl = upHaveScript.LVL;
        if (!charge)
        {
            attackSpeed = upHaveScript.curAttackSpeed;
        }


        if (CanAttack() && currentTargets.Count > 0)
        {
            Attack();
        }
        if (attackCooldown >= 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    public void UpdateStat()
    {
        if (dt != null)
        {
            attackSpeed = upHaveScript.curAttackSpeed;
            attackRadius = dt.lvlData[lvl, 2];
            slowPower = dt.lvlData[lvl, 5];
            firePower = (int)dt.lvlData[lvl, 6];
            poisonPower = (int)dt.lvlData[lvl, 7];
            stanChance = (int)dt.lvlData[lvl, 8];
            portalChange = (int)dt.lvlData[lvl, 9];
            maxTargets = (int)dt.lvlData[lvl, 10];
            thiefPower = (int)dt.lvlData[lvl, 11];
            dmgBoom = (int)dt.lvlData[lvl, 12];
            reduceArmor = dt.lvlData[lvl, 13];
            chanceDivine = (int)dt.lvlData[lvl, 14];
            dmgDivine = (int)dt.lvlData[lvl, 15];
            magnatPower = dt.lvlData[lvl, 17];
            attackCooldown = 1 / (attackSpeed + (attackSpeed * GameManager.Instance.buff[5] / 100));
        }
        gameObject.GetComponent<CircleCollider2D>().radius = attackRadius - (attackRadius * 0.25f);
    }

    bool CanAttack()
    {
        return currentTargets.Count > 0;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Transform enemyTransform = collider.transform;

            // Check if there's space for more targets based on maxTargets
            if (currentTargets.Count < maxTargets)
            {
                // Add the enemy to the list of targets
                currentTargets.Add(enemyTransform);
            }

            // Always add the enemy to the list of enemies in range
            enemiesInRange.Add(enemyTransform);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Transform enemyTransform = collider.transform;

            // Remove the enemy from both lists
            enemiesInRange.Remove(enemyTransform);
            currentTargets.Remove(enemyTransform);

            // Check if there are fewer targets than maxTargets
            if (currentTargets.Count < maxTargets)
            {
                // If there's space, add another enemy from the range
                if (enemiesInRange.Count > 0)
                {
                    currentTargets.Add(enemiesInRange[0]);
                }
            }
        }
    }

    void Attack()
    {
        if (attackCooldown <= 0f)
        {
            if (currentTargets.Count > 0)
            {
                // Выбираем случайную цель из списка текущих целей
                int randomIndex = Random.Range(0, currentTargets.Count);
                Transform target = currentTargets[randomIndex];

                if (target != null)
                {
                    // Создаем пулю и настраиваем ее параметры
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                    BulletController bulletController = bullet.GetComponent<BulletController>();
                    CannonBull bulletCannonController = bullet.GetComponent<CannonBull>();

                    if (bulletController != null)
                    {
                        bulletController.Initialize(target, damage);
                        // Устанавливаем остальные параметры пули на основе характеристик башни
                        // ...
                    }
                    else if (bulletCannonController != null)
                    {
                        bulletCannonController.enemyTarget = target;
                        bulletCannonController.dmg = damage;
                        bulletCannonController.stanChance = stanChance;
                    }

                    attackCooldown = 1 / (attackSpeed + (attackSpeed * GameManager.Instance.buff[5] / 100));
                    if (bulletController != null)
                    {
                        if (bulletController.type == TypeBull.moon && GameManager.Instance.gameObject.GetComponent<SunMoonScript>().moonCount >= GameManager.Instance.gameObject.GetComponent<SunMoonScript>().sunCount)
                        {
                            attackCooldown = 1 / (attackSpeed + (attackSpeed * GameManager.Instance.buff[5] / 100)) / 2;
                        }
                    }
                }
            }
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    private IEnumerator DivineAttack()
    {
        int randomIndex = Random.Range(0, GameManager.Instance.enemiesAll.Count);
        GameObject enemyObject = GameManager.Instance.enemiesAll[randomIndex];
        if (enemyObject != null)
        {
            Transform enemyTransform = enemyObject.transform;
            GameObject newGm = Instantiate(DivineAttackGM, enemyTransform.position, Quaternion.identity);
            newGm.GetComponent<Radius>().damage = dmgDivine;
            yield return null;
        }
    }
}