using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    public Transform exitPortal; // Добавляем переменную для второго портала
    public float detectionRadius = 0.4f; // Радиус обнаружения объектов для портала

    void Update()
    {
        // Проверяем вход объектов в зону гизмоса
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                MoveEnemyToPortal(collider.gameObject);
                collider.gameObject.GetComponent<EnemyMoving>().currentWaypoint++;
            }
        }
    }

    private void MoveEnemyToPortal(GameObject enemy)
    {
        enemy.transform.position = exitPortal.position;
    }

    // Добавляем отрисовку гизмоса для наглядности
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}