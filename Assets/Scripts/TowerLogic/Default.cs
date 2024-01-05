using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Default : MonoBehaviour
{
    public float attackRadius = 3f;
    public float attackSpeed = 1f;
    public int damage = 10;
    public Transform firePoint;
    public GameObject bulletPrefab;

    private float attackCooldown = 10f;
     public List<Transform> enemiesInRange = new List<Transform>();
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
    private Animator animator;
    public DataTower dt;
    private int lvl;
    private UpHave upHaveScript;
    private BulletController bc;
    public int chanceAssasin;
    public int Chain;
    public int critChance;
    void Start()
    {
        upHaveScript = gameObject.GetComponent<UpHave>();
        dt = upHaveScript.towerDataCur;
        bc = bulletPrefab.GetComponent<BulletController>();
        animator = GetComponent<Animator>();


        UpdateStat();
    }

    void Update()
    {
        if (upHaveScript.Imposter)
        {
            damage = -upHaveScript.curDamage;
        }
        else
        {
            damage = upHaveScript.curDamage;
        }
        attackSpeed = upHaveScript.curAttackSpeed;
        lvl = upHaveScript.LVL;
        if (!charge)
        {
            attackSpeed = upHaveScript.curAttackSpeed;
        }


        if (CanAttack() && currentTargets.Count > 0 && !upHaveScript.Muted)
        {
            if (bulletPrefab.GetComponent<BulletController>() != null)
            {
                if (bc.type != TypeBull.rage && bc.type != TypeBull.light)
                {
                    Attack();
                }
                else if (bc.type == TypeBull.rage)
                {
                    AttackRage();
                }
            }
            else if (bulletPrefab.GetComponent<BulletController>() == null)
            {
                AttackLight();
            }
            animator.SetTrigger("Attacking");
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
            critChance = upHaveScript.critChance;
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
            Chain = (int)dt.lvlData[lvl, 17];
            chanceAssasin = (int)dt.lvlData[lvl, 16];
            attackCooldown = 1 / (attackSpeed + (attackSpeed * GameManager.Instance.buff[5] / 100));
        }
        gameObject.GetComponent<CircleCollider2D>().radius = attackRadius - (attackRadius * 0.25f);
    }
    public void UpdateImposter()
    {
        if (dt != null)
        {
            firePower = -(int)dt.lvlData[lvl, 6];
            poisonPower = -(int)dt.lvlData[lvl, 7];
            dmgBoom = -(int)dt.lvlData[lvl, 12];
            dmgDivine = -(int)dt.lvlData[lvl, 15];
        }
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

            for (int i = 0; i < currentTargets.Count; i++)
            {
                if (currentTargets[i] == null)
                {
                    currentTargets[i] = enemyTransform;
                }
            }
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
                    enemiesInRange.Remove(enemiesInRange[0]);
                }
            }
        }
    }

    void Attack()
    {
        Debug.Log("ATTACK");
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
                        bulletController.critChance = critChance;
                        // Set other bullet parameters here based on the tower's attributes
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
                        else if (bulletController.type == TypeBull.portal)
                        {
                            bulletController.chancePortaling = portalChange;
                        }
                        else if (bulletController.type == TypeBull.poison)
                        {
                            bulletController.powerOfPoison = poisonPower;
                        }
                        else if (bulletController.type == TypeBull.thief)
                        {
                            bulletController.ThiefPower = thiefPower;
                            bulletController.thiefCount = thiefReward;
                        }
                        else if (bulletController.type == TypeBull.armorReduce)
                        {
                            bulletController.armorDis = reduceArmor;
                        }
                        else if (bulletController.type == TypeBull.deathBoom)
                        {
                            bulletController.boomDamage = dmgBoom;
                            bulletController.powerOfFire = firePower;
                        }
                        else if (bulletController.type == TypeBull.divine)
                        {
                            int random = Random.Range(0, 100);
                            if (random <= chanceDivine)
                            {
                                StartCoroutine(DivineAttack());
                            }
                        }
                        else if (bulletController.type == TypeBull.fury)
                        {
                            bulletController.furyCount = countOfAttack;
                        }
                        else if (bulletController.type == TypeBull.sun)
                        {
                            if (GameManager.Instance.gameObject.GetComponent<SunMoonScript>().sunCount >= GameManager.Instance.gameObject.GetComponent<SunMoonScript>().moonCount)
                            {
                                bulletController.Initialize(currentTarget, damage * 2);
                            }
                        }
                        else if (bulletController.type == TypeBull.cum)
                        {

                        }
                        else if (bulletController.type == TypeBull.radar)
                        {
                            bulletController.buffRadar = radarIncrease;
                        }
                        else if (bulletController.type == TypeBull.magnat)
                        {
                            bulletController.magnatTower = magnatPower;
                        }
                        else if (bulletController.type == TypeBull.assasin)
                        {
                            if (target.gameObject.GetComponent<EnemyMoving>().typeEnemy != EnemyType.boss)
                            {
                                int random = Random.Range(1, (int)((float)100 / chanceAssasin));
                                if (bulletController != null && random == 1)
                                {
                                    bulletController.Initialize(target, 100000000);
                                    Debug.Log("������ �����");
                                }
                                else if (random != 1)
                                {
                                    bulletController.Initialize(target, damage);
                                }
                            }
                            else
                            {
                                int random = Random.Range(1, (int)(1000 / chanceAssasin));
                                if (bulletController != null && random == 1)
                                {
                                    bulletController.Initialize(target, 100000000);
                                    Debug.Log("������ �����");
                                }
                                else if (random != 1)
                                {
                                    bulletController.Initialize(target, damage);
                                }
                            }

                        }
                    }
                    else if (bulletCannonController != null)
                    {
                        bulletCannonController.enemyTarget = target;
                        bulletCannonController.dmg = damage;
                        bulletCannonController.stanChance = stanChance;
                        bulletCannonController.critChance = critChance;
                    }

                    attackCooldown = 1 / (attackSpeed + (attackSpeed * GameManager.Instance.buff[5] / 100));
                    if (bulletController != null)
                    {
                        if (bulletController.type == TypeBull.moon && GameManager.Instance.gameObject.GetComponent<SunMoonScript>().moonCount >= GameManager.Instance.gameObject.GetComponent<SunMoonScript>().sunCount)
                        {
                            attackCooldown = 1 / (attackSpeed + (attackSpeed * GameManager.Instance.buff[5]/100))/2;
                        }
                    }

                }
            }
        }
    }
    void AttackRage()
    {
        if (attackCooldown <= 0f && enemiesInRange.Count > 0)
        {
            int randomIndex = Random.Range(0, enemiesInRange.Count);
            Transform target = enemiesInRange[randomIndex];

            if (target != null)
            {
                // Create a bullet and set its parameters
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                BulletController bulletController = bullet.GetComponent<BulletController>();
                CannonBull bulletCannonController = bullet.GetComponent<CannonBull>();

                if (bulletController != null)
                {
                    bulletController.Initialize(target, damage);
                }


                attackCooldown = 1 / (attackSpeed + (attackSpeed * GameManager.Instance.buff[5] / 100));
            }
        }
    }
    void AttackLight()
    {
        if (attackCooldown <= 0f && enemiesInRange.Count >maxTargets)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            LightBull bulletScript = bullet.GetComponent<LightBull>();
            bulletScript.targetEnemies = new Transform[maxTargets];
            bulletScript.damage = damage;
            for (int i = 0; i < maxTargets; i++)
            {
                if (enemiesInRange.Count >= maxTargets)
                {
                    bulletScript.targetEnemies[i] = enemiesInRange[i];

                    attackCooldown = 1 / (attackSpeed + (attackSpeed * GameManager.Instance.buff[5] / 100));
                    enemiesInRange.RemoveAt(0); // Remove the first enemy from the list
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