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

    public float attackCooldown = 10f;
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
    public AudioClip hitSFX;
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
        UpdateStatTwo();
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
        currentTargets.RemoveAll(target => target == null);

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
            else if (bulletPrefab.GetComponent<CannonBull>() != null)
            {
                Attack();
            }
            else if (bulletPrefab.GetComponent<BulletController>() == null )
            {
                AttackLight();
            }
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
        gameObject.GetComponent<CircleCollider2D>().radius = attackRadius - (attackRadius * 0.19f);
    }
    public void UpdateStatTwo()
    {
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

            // Проверяем, есть ли враг уже в массиве currentTargets
            if (!currentTargets.Contains(enemyTransform))
            {
                // Проверяем, есть ли место для большего количества целей на основе maxTargets
                if (currentTargets.Count < maxTargets)
                {
                    // Добавляем врага в список целей
                    currentTargets.Add(enemyTransform);
                }

                // Всегда добавляем врага в список врагов в радиусе действия
                enemiesInRange.Add(enemyTransform);
            }
        }
    }

   void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            Transform enemyTransform = collider.transform;

            // Удаляем врага из списка currentTargets
            currentTargets.Remove(enemyTransform);

            // Удаляем врага из списка enemiesInRange
            enemiesInRange.Remove(enemyTransform);

            // Удаляем все null объекты из списка enemiesInRange
            enemiesInRange.RemoveAll(target => target == null);

            // Проверяем, если у нас меньше целей, чем maxTargets, то добавляем из enemiesInRange[0]
            if (currentTargets.Count < maxTargets && enemiesInRange.Count > 0)
            {
                // Добавляем первый элемент из enemiesInRange в currentTargets, если он не уже в списке
                Transform newTarget = enemiesInRange[0];
                if (!currentTargets.Contains(newTarget))
                {
                    currentTargets.Add(newTarget);
                }
            }
        }
    }

    void Attack()
    {
       Debug.Log("ATTACK");
        if (attackCooldown <= 0f)
        {
            HashSet<Transform> attackedTargets = new HashSet<Transform>();
            foreach (var target in currentTargets)
            {
                if (target != null && !attackedTargets.Contains(target))
                {
                    UpdateFlip(target);
                    animator.SetTrigger("Attacking");
                    GameManager.Instance.aS.PlayOneShot(hitSFX);
                    GameManager.Instance.aS.pitch = Random.Range(0.8f, 1.1f);

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
                            bulletController.furyCount = countOfAttack+1;
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
                        else if(bulletController.type == TypeBull.gear)
                        {
                            bulletController.damage = damage + (int)((float)damage * (float)GetComponent<GearTower>().objectCount* 0.15f);
                            attackCooldown = 1 / (attackSpeed + (attackSpeed * GameManager.Instance.buff[5] / 100)) + (0.08f * (float)GetComponent<GearTower>().objectCount);
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
    public void UpdateFlip(Transform target)
    {
        if(target !=null)
        {
            if(target.position.x < transform.position.x)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
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
                 UpdateFlip(target);
                animator.SetTrigger("Attacking");
                    GameManager.Instance.aS.PlayOneShot(hitSFX);
                    GameManager.Instance.aS.pitch = Random.Range(0.8f, 1.1f);
                // Create a bullet and set its parameters
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                BulletController bulletController = bullet.GetComponent<BulletController>();
                CannonBull bulletCannonController = bullet.GetComponent<CannonBull>();

            

                attackCooldown = 1 / (attackSpeed + (attackSpeed * GameManager.Instance.buff[5] / 100));
            }
        }
        
    }
    void AttackLight()
    {
        if (attackCooldown <= 0f && currentTargets.Count >= 3 && enemiesInRange.Count >= maxTargets)
        {
            GameManager.Instance.aS.PlayOneShot(hitSFX);
            GameManager.Instance.aS.pitch = Random.Range(0.8f, 1.1f);
            animator.SetTrigger("Attacking");
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            LightBull bulletScript = bullet.GetComponent<LightBull>();
            bulletScript.targetEnemies = new Transform[maxTargets];
            bulletScript.damage = damage;
            bulletScript.critChance = critChance;
            for (int i = 0; i < maxTargets; i++)
            {
                bulletScript.targetEnemies[i] = enemiesInRange[i];
            }
            if (GetComponent<UpHave>().id == 23)
            {
                int random = Random.Range(0, 100);
                if (random <= chanceDivine)
                {
                    StartCoroutine(DivineAttack());
                }
            }
            UpdateFlip(enemiesInRange[0]);
            attackCooldown = 1 / (attackSpeed + (attackSpeed * GameManager.Instance.buff[5] / 100));
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