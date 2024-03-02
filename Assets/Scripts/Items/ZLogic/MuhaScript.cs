using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuhaScript : MonoBehaviour
{
    public BoxCollider2D moveZoneCollider; // Коллайдер центра зоны перемещения
    public float minWalkTime = 2f; // Минимальное время ходьбы
    public float maxWalkTime = 5f; // Максимальное время ходьбы
    public float standTime = 1f; // Время стояния
    public float moveSpeed = 2f; // Скорость перемещения

    private Vector3 targetPosition;
    private bool isWalking = false;
    private Animator animator;

    private void Start()
    {
        moveZoneCollider = GameManager.Instance.tilemapCollider;
        animator = gameObject.GetComponent<Animator>();
        StartCoroutine(Wander());
    }

    private IEnumerator Wander()
    {
        while (true)
        {
            // Если не движемся, то выбираем новую целевую позицию
            if (!isWalking)
            {
                animator.SetBool("Walk", true);
                targetPosition = RandomPointInCollider(moveZoneCollider);


                // Запускаем ходьбу к целевой позиции
                isWalking = true;
                yield return StartCoroutine(Walk());
            }

            // Ждем время перед следующим перемещением
            //yield return new WaitForSeconds(Random.Range(minWalkTime, maxWalkTime));
        }
    }

    private IEnumerator Walk()
    {
        // Пока не достигнута целевая позиция
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Двигаем существо вперед
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
            if(targetPosition.x < transform.position.x)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        // Прибыли в целевую позицию
        isWalking = false;
        animator.SetBool("Walk", false);
        // Ждем некоторое время перед следующим действием
        yield return new WaitForSeconds(standTime);
    }

    // Возвращает случайную точку внутри коллайдера
    private Vector3 RandomPointInCollider(Collider2D collider)
    {
        Vector3 randomPoint = collider.bounds.center + new Vector3(Random.Range(-collider.bounds.extents.x, collider.bounds.extents.x), Random.Range(-collider.bounds.extents.y, collider.bounds.extents.y), 0f);
        return randomPoint;
    }
}