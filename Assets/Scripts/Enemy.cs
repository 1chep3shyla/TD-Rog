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
    private float armorReduceBase = 1f;
    public TMP_Text damageText;
    void Start()
    {
        health = maxHealth;
    }
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        if (inPoison )
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (inFire )
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (gameObject.GetComponent<EnemyMoving>().isSlowed) 
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    public void TakeDamage(int dmg)
    {
        if (!armor)
        {
            int curhp = health - (dmg + (int)GameManager.Instance.buff[0]);
            if (curhp <= 0)
            {
                Death();
            }
            else
            {
                par.Play();
                health = curhp;
            }
            GameObject dmgText = Instantiate(damageText.gameObject, transform.position, Quaternion.identity);
            dmgText.GetComponent<TMP_Text>().text = "" + (dmg + (int)GameManager.Instance.buff[0]);
        }
        else
        {
            int curhp = health -= (int)((float)(dmg + (int)GameManager.Instance.buff[0]) * armorReduce);
            if (curhp <= 0)
            {
                Death();
            }
            else
            {
                par.Play();
                health = curhp;
            }
            GameObject dmgText = Instantiate(damageText.gameObject, transform.position , Quaternion.identity);
            dmgText.GetComponent<TMP_Text>().text = "" + (int)((float)(dmg + (int)GameManager.Instance.buff[0]) * armorReduce);
        }
    }
    public void BoomOn(int dmg)
    {
        StartCoroutine(BoomCor(dmg));
    }
    private void Death()
    {
        if (cursedBoom)
        {
            GameObject boom = Instantiate(BoomGM, transform.position, Quaternion.identity);
            boom.GetComponent<Radius>().damage = damageBoom;
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

            TakeDamage(dmg + (int)GameManager.Instance.buff[2]);
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
            TakeDamage(dmg + (int)GameManager.Instance.buff[3]);
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

    private IEnumerator BoomCor(int dmgBoom)
    {
        cursedBoom = true;
        damageBoom = dmgBoom;
        yield return new WaitForSeconds(10f); // Apply fire damage every second
        cursedBoom = false;
        damageBoom = 0;

    }
    public void Thiefed(int power)
    {
        int random = Random.Range(0, 100);
        if (random <= power)
        {
            GameManager.Instance.StealMoney(5+((int)(float)goldGive/10));
        }
    }

}
