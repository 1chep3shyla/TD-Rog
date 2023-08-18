using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBull : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public Transform[] targetEnemies; // Array of target enemies

    private int currentTargetIndex = 0;

    private void Update()
    {
        if (currentTargetIndex < targetEnemies.Length && targetEnemies[currentTargetIndex]!=null)
        {
            Vector3 direction = (targetEnemies[currentTargetIndex].position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);
        }
        if (currentTargetIndex >= targetEnemies.Length )
        {
            Destroy(gameObject);
        }
        if (currentTargetIndex < targetEnemies.Length)
        {
            if (targetEnemies[currentTargetIndex] == null)
            {
                Destroy(gameObject);
            }
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && currentTargetIndex < targetEnemies.Length && collision.transform == targetEnemies[currentTargetIndex])
        {
            currentTargetIndex++; // Move to the next target
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }

    }
}






