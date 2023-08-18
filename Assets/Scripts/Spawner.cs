using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] waypoints;

    public WaveMass[] wavesMass;
    public int timeBetweenWaves = 5;
    [SerializeField]
    private GameManager gameManager;

    private float lastSpawnTime;
    private float waveEndTime;
    private int enemiesSpawned = 0;

    public int currentWaveIndex = 0;
    public int currentWaveIndexMain = 0;
    public Wave[] currentWave;
    private bool works;

    private bool shouldStartNextWave = false;



    private void Start()
    {
        if (wavesMass.Length > 0)
        {
            currentWave = wavesMass[currentWaveIndex].wavesAll;
        }
    }
    void Update()
    {
        while (currentWaveIndex < wavesMass.Length && !works)
        {
            works = true;

            StartCoroutine(StartWave());
            return;

        }
        GameManager.Instance.curWave = currentWaveIndexMain +1;
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);

        while (currentWaveIndex < currentWave.Length)
        {
            yield return new WaitForSeconds(currentWave[currentWaveIndex].TimeToNext);
            for (int i = 0; i < currentWave[currentWaveIndex].maxEnemies; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(currentWave[currentWaveIndex].spawnInterval);
            }

            yield return new WaitUntil(() => enemiesSpawned == 0);

            yield return new WaitForSeconds(currentWave[currentWaveIndex].spawnInterval);
            currentWaveIndex++;
            if (currentWaveIndex >= wavesMass[currentWaveIndexMain].wavesAll.Length)
            {
                currentWaveIndexMain++;
                currentWaveIndex = 0;
                currentWave = wavesMass[currentWaveIndexMain].wavesAll;
            }
        }

        works = false;
        yield return null;
    }

    private void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(currentWave[currentWaveIndex].enemyPrefab, waypoints[0].transform.position, Quaternion.identity);
        newEnemy.GetComponent<EnemyMoving>().waypoints = waypoints;
        newEnemy.GetComponent<Enemy>().goldGive = currentWave[currentWaveIndex].goldWaveGiving;
        newEnemy.GetComponent<Enemy>().health = currentWave[currentWaveIndex].healthEnemy;
        newEnemy.GetComponent<Enemy>().maxHealth = currentWave[currentWaveIndex].healthEnemy;
    }



}

[System.Serializable]
public class Wave
{
    public GameObject enemyPrefab;
    public float TimeToNext;
    public int goldWaveGiving;
    public int healthEnemy;
    public float spawnInterval = 2;
    public int maxEnemies = 20;
}

[System.Serializable]
public class WaveMass
{
    public Wave[] wavesAll;
}