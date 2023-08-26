using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
    public void TakeDamage(int dmg)
    {
        if (!armor)
        {
            int curhp = health -= dmg;
            if (curhp <= 0)
            {
                Death();
            }
            else
            {
                par.Play();
                health = curhp;
            }
        }
        else
        {
            int curhp = health -= (int)((float)dmg * armorReduce);
            if (curhp <= 0)
            {
                Death();
            }
            else
            {
                par.Play();
                health = curhp;
            }
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
        GameManager.Instance.Gold += goldGive;
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
        if (gameObject.GetComponent<SpriteRenderer>().color == Color.white)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
        while (dur > 0)
        {

            TakeDamage(dmg);
            yield return new WaitForSeconds(0.25f); // Apply fire damage every second
            dur -= 0.25f;
        }
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        inFire = false;
    }

    public void SetPoison(float duration, int damage)
    {
        if (!inPoison)
        {
            Debug.Log("ßÄ");
            inPoison = true;
            StartCoroutine(Poisoned(duration, damage));
        }
    }

    private IEnumerator Poisoned(float dur, int dmg)
    {
        if (gameObject.GetComponent<SpriteRenderer>().color == Color.white)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
        gameObject.GetComponent<EnemyMoving>().Slow(1f, 0.1f);
        while (dur > 0)
        {
            TakeDamage(dmg);
            yield return new WaitForSeconds(0.5f); // Apply fire damage every second
            dur -= 0.5f;
        }
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        inPoison = false;
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
            GameManager.Instance.Gold += 5;
        }
    }

}
