using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Enemy : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int goldGive;
    public ParticleSystem par;
    public bool inFire;
    public bool inPoison;
    public GameObject BoomGM;
    private bool isStunned;
    public bool cursedBoom;
    private int damageBoom;
    private float stunDuration;
    private float armorReduce;
    private bool armor;
    public int dmgFire;
    private float armorReduceBase = 1f;
    public TMP_Text damageText;
    public float[] resistance; // 0 - ice, 1 - fire, 2 - poison, 3 - stan
    private SpriteRenderer SR;
    public TypeBull damageType;
    void Start()
    {
        health = maxHealth;
        SR = gameObject.GetComponent<SpriteRenderer>();
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
            GameObject dmgText = Instantiate(damageText.gameObject, transform.position, Quaternion.identity);
            dmgText.GetComponent<TMP_Text>().text = "" + dmg;
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
            GameObject dmgText = Instantiate(damageText.gameObject, transform.position, Quaternion.identity);
            dmgText.GetComponent<TMP_Text>().text = "" + dmg;
        }
        
    }
    public void DefaultAttack(int dmg, int critChance)
    {
        int randomChance = Random.Range(0, 100);
        if (randomChance < critChance)
        {
            Debug.Log("Êðèò");
            float critDamage = 2 + ((2 * (GameManager.Instance.buff[7] / 100)));
            TakeDamage((int)((float)(dmg + (int)((float)dmg * (GameManager.Instance.buff[0] / 100 + armorReduce / 100))) * critDamage));
        }
        else
        {
            TakeDamage((int)((float)(dmg + (int)((float)dmg * (GameManager.Instance.buff[0] / 100 + armorReduce / 100)))));
        }
    }
    public void BoomOn(int dmg, int fireDmg)
    {
        StartCoroutine(BoomCor(dmg, fireDmg));
    }
    private void Death()
    {
        if (cursedBoom)
        {
            GameObject boom = Instantiate(BoomGM, transform.position, Quaternion.identity);
            boom.GetComponent<Radius>().damage = damageBoom;
            boom.GetComponent<Radius>().fireDamage = dmgFire;
        }
        GameManager.Instance.StealMoney(goldGive);
        Destroy(gameObject);
        GameManager.Instance.enemyHave -= 1;
        par.Play();
    }
    public void SetOnFire(float duration, int damage)
    {
        if (!inFire)
        {
            Debug.Log("ÃÎÐÈÒ");
            inFire = true;
            StartCoroutine(Burn(duration, damage));
        }
    }

    private IEnumerator Burn(float dur, int dmg)
    {
        while (dur > 0)
        {
            int addDMG = dmg * (int)(GameManager.Instance.buff[2] / 100);
            int removeDMG = (int)((float)(dmg + addDMG) * resistance[1]/100);
            TakeDamage(dmg + addDMG - removeDMG);
            yield return new WaitForSeconds(0.25f); // Apply fire damage every second
            dur -= 0.25f;
        }
        inFire = false;
    }

    public void SetPoison(float duration, int damage)
    {
        if (!inPoison)
        {
            Debug.Log("ßÄ");
            StartCoroutine(Poisoned(duration, damage));
        }
    }

    private IEnumerator Poisoned(float dur, int dmg)
    {
        if (inPoison) // Check if already poisoned
        {
            yield break; // If already poisoned, exit the Coroutine
        }

        inPoison = true; // Set the flag to indicate poisoning

        gameObject.GetComponent<EnemyMoving>().Slow(1f, 0.1f);
        while (dur > 0)
        {
            int addDMG = dmg * (int)(GameManager.Instance.buff[3] / 100);
            int removeDMG = (int)((float)(dmg + addDMG) * resistance[2]/100);
            TakeDamage(dmg + addDMG - removeDMG);
            yield return new WaitForSeconds(0.5f); // Apply fire damage every second
            dur -= 0.5f;
        }

        // Check if still poisoned (another coroutine may have ended it)
        if (inPoison)
        {
            inPoison = false; // Reset the flag
        }
    }
 

    public void ArmorReducePublic(float power)
    {
        StartCoroutine(ArmorReducing(power));
    }
    private IEnumerator ArmorReducing(float reducing)
    {
        armor = true;
        armorReduce = armorReduceBase + reducing;
        yield return new WaitForSeconds(2f); // Apply fire damage every second
        armorReduce = armorReduceBase;
        armor = false;
    }

    private IEnumerator BoomCor(int dmgBoom, int firedDMG)
    {
        cursedBoom = true;
        damageBoom = dmgBoom;
        dmgFire = firedDMG;
        yield return new WaitForSeconds(10f); // Apply fire damage every second
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
