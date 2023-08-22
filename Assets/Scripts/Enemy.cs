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
    private bool isStunned;
    private float stunDuration;
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
        int curhp = health -= dmg;
        if (curhp <= 0)
        {
            GameManager.Instance.Gold += goldGive;
            Destroy(gameObject);
            GameManager.Instance.enemyHave -= 1;
            par.Play();
        }
        else
        {
            par.Play();
            health = curhp;
        }
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

}
