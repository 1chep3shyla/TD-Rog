using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneScript : MonoBehaviour
{
      public int healthStone;
    public ParticleSystem ps;
    public float detectionRadius = 5f; // Радиус обнаружения
    public LayerMask enemyLayer; // Слой врагов
   
    private void Update()
    {
        // Вызываем функцию для проверки врагов в радиусе
        CheckForEnemies();
    }

    private void CheckForEnemies()
    {
        // Используем функцию OverlapCircle для проверки наличия коллайдеров врагов в заданном радиусе
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);

        // Если есть враги в радиусе
        if (colliders.Length > 0)
        {
            Debug.Log("Враги обнаружены!");

            // Обработка врагов в радиусе
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    if (collider.gameObject.GetComponent<EnemyMoving>().typeEnemy != EnemyType.boss)
                    {
                        Destroy(collider.gameObject);
                        healthStone -= 1;
                        ps.Play();
                        if (healthStone <= 0)
                        {
                            Destroy(gameObject);
                        }
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Отрисовываем радиус обнаружения в редакторе Unity
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}