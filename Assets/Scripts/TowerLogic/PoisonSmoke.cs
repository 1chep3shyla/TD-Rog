using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSmoke : MonoBehaviour
{
    public Default tower;
    public int damage;
    void Start()
    {
        damage = tower.poisonPower;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Poisoned(other.gameObject);
        }
    }


    private void Poisoned(GameObject enemy)
    {
        // Apply damage to the enemy
        Enemy enemyHealth = enemy.GetComponent<Enemy>();
        if (enemyHealth != null)
        {
            enemyHealth.SetPoison(4f,damage);
        }
    }
}
