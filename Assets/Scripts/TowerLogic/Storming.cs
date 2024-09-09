using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storming : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnInterval = 5f;

    private bool isSpawning = false;

    private void Start()
    {
        StartCoroutine(SpawnObjectRoutine());
    }

    private IEnumerator SpawnObjectRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (GameManager.Instance.enemiesAll == null || GameManager.Instance.enemiesAll.Count == 0)
                continue;

            isSpawning = true;

            int randomIndex = Random.Range(0, GameManager.Instance.enemiesAll.Count);
            GameObject enemyObject = GameManager.Instance.enemiesAll[randomIndex];

            if (enemyObject != null)
            {
                Transform enemyTransform = enemyObject.transform;
                GameObject newGm = Instantiate(objectToSpawn, enemyTransform.position, Quaternion.identity);
                newGm.GetComponent<StormScript>().slowPower = gameObject.GetComponent<Default>().slowPower;
                yield return new WaitForSeconds(5);
            }

            isSpawning = false;
        }
    }
}