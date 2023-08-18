using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBull : MonoBehaviour
{
    public GameObject impactPrefab; // Prefab to instantiate when bullet hits the enemy
    public Transform enemyTarget;
    public float arcHeight = 5.0f;
    public float bulletSpeed = 10.0f;
    public int dmg;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float horizontalDistance;
    private float totalTime;
    private bool hasHit = false;

    private void Start()
    {
        startPosition = transform.position;
        endPosition = enemyTarget.position;

        horizontalDistance = Vector3.Distance(startPosition, endPosition);

        float yOffset = endPosition.y - startPosition.y;
        float timeToTop = Mathf.Sqrt((2 * arcHeight) / Mathf.Abs(Physics2D.gravity.y));
        float timeToTarget = Mathf.Sqrt((2 * (arcHeight - yOffset)) / Mathf.Abs(Physics2D.gravity.y));
        totalTime = timeToTop + timeToTarget;

        StartCoroutine(MoveAlongArc());
    }

    private IEnumerator MoveAlongArc()
    {
        float elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            float t = elapsedTime / totalTime;
            Vector3 parabolicPosition = Vector3.Lerp(startPosition, endPosition, t) + Vector3.up * Mathf.Sin(Mathf.PI * t) * arcHeight;

            transform.position = parabolicPosition;

            elapsedTime += Time.deltaTime * bulletSpeed;
            yield return null;
        }
        InstantiateImpactEffect();

        // Instantiate impact object and destroy the bullet
        if (!hasHit)
        {
            InstantiateImpactEffect();
            Destroy(gameObject);
        }
    }


    private void InstantiateImpactEffect()
    {
        Vector3 impactPosition = Vector3.Lerp(startPosition, endPosition, 1f);
        Instantiate(impactPrefab, impactPosition, Quaternion.identity).GetComponent<Radius>().damage = dmg;
        Destroy(gameObject);
    }
}