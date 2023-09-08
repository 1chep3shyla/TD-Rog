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
    public int poisonPower;
    public int stanChance;
    public int portalChange;
    public bool charge;
    public int maxTargets = 1; // Maximum number of targets that can be attacked simultaneously
    private List<Transform> currentTargets = new List<Transform>();
    public int thiefPower;
    public int dmgBoom;
    public float reduceArmor;
    public int chanceDivine;
    public int dmgDivine;
    public int countOfAttack;
    private Transform currentTarget;
    [SerializeField]
    private GameObject DivineAttackGM;
    private Transform whichEnemy;
    public DataTower dt;
    private int lvl;

    void Start()
    {
        dt = gameObject.GetComponent<UpHave>().towerDataCur;
    }
    void Update()
    {
        damage = gameObject.GetComponent<UpHave>().curDamage;
        lvl = gameObject.GetComponent<UpHave>().LVL;
        if (dt != null)
        {
            attackRadius = dt.lvlData[lvl, 2];
            slowPower = dt.lvlData[lvl, 4];
            firePower = (int)dt.lvlData[lvl, 5];
            poisonPower = (int)dt.lvlData[lvl, 6];
            stanChance = (int)dt.lvlData[lvl, 7];
            portalChange = (int)dt.lvlData[lvl, 8];
            maxTargets = (int)dt.lvlData[lvl, 9];
            //maxTargets = (int)dt.lvlData[LVL, 10];
            dmgBoom = (int)dt.lvlData[lvl, 11];
            reduceArmor = dt.lvlData[lvl, 12];
            chanceDivine = (int)dt.lvlData[lvl, 13];
            dmgDivine = (int)dt.lvlData[lvl, 14];
        }
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
                        }
                        else if (bulletController.type == TypeBull.armorReduce)
                        {
                            bulletController.armorDis = reduceArmor;
                        }
                        else if (bulletController.type == TypeBull.deathBoom)
                        {
                            bulletController.boomDamage = dmgBoom;
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
                    }
                    else if (bulletCannonController != null)
                    {   
                        bulletCannonController.enemyTarget = target;
                        bulletCannonController.dmg = damage;
                    }

                    attackCooldown = attackSpeed;
                    if (bulletController != null)
                    {
                        if (bulletController.type == TypeBull.moon && GameManager.Instance.gameObject.GetComponent<SunMoonScript>().moonCount >= GameManager.Instance.gameObject.GetComponent<SunMoonScript>().sunCount)
                        {
                            attackCooldown /= 2;
                        }
                    }

                }
            }

        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the attack radius using Gizmos
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