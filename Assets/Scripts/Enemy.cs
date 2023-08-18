using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public int goldGive;
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
        }
        else
        {
            health = curhp;
        }
    }
}
