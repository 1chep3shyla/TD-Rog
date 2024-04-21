using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(EnemyMoving))]
public class Enemy : MonoBehaviour
{
    public int index;
    [Space]
    
    public int health;
    public int maxHealth;
    public int goldGive;
    public int chanceDrop;
    public ParticleSystem par;
    public bool inFire;
    public bool inPoison;
    public GameObject BoomGM;
    public GameObject chestPrefab;
    public GameObject goldPrefab;
    private bool isStunned;
    public bool cursedBoom;
    private int damageBoom;
    private float stunDuration;
    private float armorReduce;
    private bool armor;
    public int dmgFire;
    public int debuffCount;
    private float armorReduceBase = 1f;
    public TMP_Text damageText;
    public float[] resistance; // 0 - ice, 1 - fire, 2 - poison, 3 - stan
    private SpriteRenderer SR;
    public TypeBull damageType;
    public EnemyMoving EM;
    public GameObject deathPar;
    public GameObject firePart;
    public GameObject poisonPart;
    public GameObject icePart;
    public GameObject shieldBreak;
    void Start()
    {
        health = maxHealth;
        SR = gameObject.GetComponent<SpriteRenderer>();
        EM = gameObject.GetComponent<EnemyMoving>();
         if (EM.typeEnemy == EnemyType.flying)
        {
            goldGive = 30;
        }
        else if (EM.typeEnemy == EnemyType.elite)
        {
            goldGive = 25;
        }
        else if (EM.typeEnemy == EnemyType.fast)
        {
            goldGive = 25;
        }
        else if (EM.typeEnemy == EnemyType.reduction)
        {
            goldGive = 30;
        }
        else if (EM.typeEnemy == EnemyType.defaultEnemy)
        {
            goldGive = 20;
        }
    }
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        if (inPoison )
        {
            SR.color = Color.green;
        }
        else if (inFire )
        {
            SR.color = Color.red;
        }
        else if (gameObject.GetComponent<EnemyMoving>().isSlowed) 
        {
            SR.color = Color.blue;
        }
        else
        {
            SR.color = Color.white;
        }
    }
    public void TakeDamage(int dmg)
    {

        if (!armor)
        {
            int curhp = health - dmg;
            if (dmg > 0)
            {
                GameManager.Instance.AddDamageByBulletType(damageType, dmg);
            }
            if (curhp <= 0)
            {
                Death();
            }
            else
            {
                par.Play();
                if (curhp <= maxHealth)
                {
                    health = curhp;
                }
                else
                {
                    health = maxHealth;
                }
            }

        }
        else
        {
            int curhp = health -= (int)((float)dmg * armorReduce);
            if (dmg > 0)
            {
                GameManager.Instance.AddDamageByBulletType(damageType, (int)((float)dmg * armorReduce));
            }
            if (curhp <= 0)
            {
                Death();
            }
            else
            {
                par.Play();
                if (curhp <= maxHealth)
                {
                    health = curhp;
                }
                else
                {
                    health = maxHealth;
                }
            }
            par.Play();
        }
        
    }
    public void DefaultAttack(int dmg, int critChance)
    {
        int randomChance = Random.Range(0, 100);
        if (randomChance < critChance)
        {
            Debug.Log("����");
            float critDamage = 2 + ((2 * (GameManager.Instance.buff[7] / 100)));
            int curDmg = (int)((float)(dmg + (int)((float)dmg * (GameManager.Instance.buff[0] / 100 + armorReduce / 100))) * critDamage);
            TakeDamage(curDmg);
            GameObject dmgText = Instantiate(damageText.gameObject, transform.position, Quaternion.identity);
            dmgText.GetComponent<TMP_Text>().text = "" + curDmg;
            dmgText.GetComponent<TMP_Text>().color = Color.red;
            dmgText.GetComponent<Transform>().localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }
        else
        {
            int curDmg = (int)((float)(dmg + (int)((float)dmg * (GameManager.Instance.buff[0] / 100 + armorReduce / 100))));
            TakeDamage(curDmg);
            GameObject dmgText = Instantiate(damageText.gameObject, transform.position, Quaternion.identity);
            dmgText.GetComponent<TMP_Text>().text = "" + curDmg;
        }
    }
    public void BoomOn(int dmg, int fireDmg)
    {
        StartCoroutine(BoomCor(dmg, fireDmg));
    }
    public void Death()
    {
        if (cursedBoom)
        {
            GameObject boom = Instantiate(BoomGM, transform.position, Quaternion.identity);
            boom.GetComponent<Radius>().damage = damageBoom;
            boom.GetComponent<Radius>().fireDamage = dmgFire;
        }
        GameManager.Instance.whichEnemyKill++;
        Destroy(gameObject);
        if(deathPar!=null)
        {
            Instantiate(deathPar, transform.position, Quaternion.identity);
        }
        GameManager.Instance.enemyHave -= 1;
        GameObject gold = Instantiate(goldPrefab, transform.position, Quaternion.identity);
        gold.GetComponent<GoldMoving>().gold = goldGive;
        par.Play();
        int randomDrop = Random.Range(0,100);
        if(randomDrop <= chanceDrop)
        {
            GameObject chest = Instantiate(chestPrefab, transform.position, Quaternion.identity);
        }
    }
    public void SetOnFire(float duration, int damage)
    {
        if (!inFire)
        {
            Debug.Log("�����");
            inFire = true;
            StartCoroutine(Burn(duration, damage));
        }
    }

    private IEnumerator Burn(float dur, int dmg)
    {
        debuffCount++;
         firePart.GetComponent<ParticleSystem>().Play();
        while (dur > 0)
        {
            int addDMG = dmg * (int)(GameManager.Instance.buff[2] / 100);
            int removeDMG = (int)((float)(dmg + addDMG) * resistance[1]/100);
            TakeDamage(dmg + addDMG - removeDMG);
            GameObject dmgText = Instantiate(damageText.gameObject, transform.position, Quaternion.identity);
            int curDMG = dmg + addDMG - removeDMG;
            dmgText.GetComponent<TMP_Text>().text = "" + curDMG;
            dmgText.GetComponent<TMP_Text>().color = new Color(255f / 255f, 120f / 255f, 0);
            yield return new WaitForSeconds(0.25f); // Apply fire damage every second
            dur -= 0.25f;
        }
        debuffCount-=1;
        firePart.GetComponent<ParticleSystem>().Stop();
        inFire = false;
    }

    public void SetPoison(float duration, int damage)
    {
        if (!inPoison)
        {
            Debug.Log("��");
            StartCoroutine(Poisoned(duration, damage));
        }
    }

    private IEnumerator Poisoned(float dur, int dmg)
    {
        if (inPoison) // Check if already poisoned
        {
            yield break; // If already poisoned, exit the Coroutine
        }
        debuffCount++;

        inPoison = true; // Set the flag to indicate poisoning

        gameObject.GetComponent<EnemyMoving>().Slow(3f, 0.15f);
        poisonPart.GetComponent<ParticleSystem>().Play();
        while (dur > 0)
        {
            int addDMG = dmg * (int)(GameManager.Instance.buff[3] / 100);
            int removeDMG = (int)((float)(dmg + addDMG) * resistance[2]/100);
            TakeDamage(dmg + addDMG - removeDMG);
            GameObject dmgText = Instantiate(damageText.gameObject, transform.position, Quaternion.identity);
            int curDMG = dmg + addDMG - removeDMG;
            dmgText.GetComponent<TMP_Text>().text = "" + curDMG;
            dmgText.GetComponent<TMP_Text>().color = new Color(0, 161 / 255f, 8f / 255f);
            yield return new WaitForSeconds(0.5f); // Apply fire damage every second
            dur -= 0.5f;
        }

        // Check if still poisoned (another coroutine may have ended it)
        if (inPoison)
        {
            inPoison = false; // Reset the flag
            debuffCount -=1;
            poisonPart.GetComponent<ParticleSystem>().Stop();
        }
    }
 

    public void ArmorReducePublic(float power)
    {
        StartCoroutine(ArmorReducing(power));
    }
    private IEnumerator ArmorReducing(float reducing)
    {
        debuffCount++;
        shieldBreak.SetActive(true);
        armor = true;
        armorReduce = armorReduceBase + reducing;
        yield return new WaitForSeconds(2f); // Apply fire damage every second
        shieldBreak.SetActive(false);
        debuffCount -=1;
        armorReduce = armorReduceBase;
        armor = false;
    }

    private IEnumerator BoomCor(int dmgBoom, int firedDMG)
    {
        cursedBoom = true;
        debuffCount++;
        damageBoom = dmgBoom;
        dmgFire = firedDMG;
        yield return new WaitForSeconds(10f); // Apply fire damage every second
        debuffCount-=1;
        cursedBoom = false;
        damageBoom = 0;
        dmgFire = 0;

    }
    public void Thiefed(int power, int countGold)
    {
        int random = Random.Range(0, 100);
        if (random <= power)
        {
            GameManager.Instance.StealMoney(countGold);
        }
    }
    public void SetBulletType(TypeBull bulletType)
    {
        this.damageType = bulletType;
    }

}
