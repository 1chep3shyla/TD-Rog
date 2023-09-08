using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CumLogic : MonoBehaviour
{
    public int damage;
    private float timeLife = 1f;

    void Update()
    {
        timeLife -= Time.deltaTime;
        if (timeLife <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Firing(other.gameObject);
        }
    }

    private void Firing(GameObject enemy)
    {
        Enemy enemyHealth = enemy.GetComponent<Enemy>();
        if (enemyHealth != null)
        {
            enemyHealth.SetOnFire(3f, damage);
        }
    }


}