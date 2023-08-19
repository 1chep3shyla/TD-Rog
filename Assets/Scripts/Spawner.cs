using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
        for (int i =0; i< wavesMass.Length; i++)
        {
            for (int o = 0; o < wavesMass[i].wavesAll.Length; o++)
            {
                if (i <= 10)
                {
                    wavesMass[i].wavesAll[o].goldWaveGiving = 10 + (2 * i);
                    wavesMass[i].wavesAll[o].healthEnemy = (int)(wavesMass[i].wavesAll[o].enemyPrefab.GetComponent<Enemy>().maxHealth + wavesMass[i].wavesAll[o].enemyPrefab.GetComponent<Enemy>().maxHealth * (i / 5 ));
                }
                else if (i > 10 && i <= 20)
                {
                    wavesMass[i].wavesAll[o].goldWaveGiving = (int)(30 * Math.Pow(1.137f, i-10));
                    wavesMass[i].wavesAll[o].healthEnemy = (int)(wavesMass[i].wavesAll[o].enemyPrefab.GetComponent<Enemy>().maxHealth + wavesMass[i].wavesAll[o].enemyPrefab.GetComponent<Enemy>().maxHealth * (i / 5 * 5));
                }
                else
                {
                    wavesMass[i].wavesAll[o].goldWaveGiving = (int)(100 * Math.Pow(1.137f, i-20));
                    wavesMass[i].wavesAll[o].healthEnemy = (int)(wavesMass[i].wavesAll[o].enemyPrefab.GetComponent<Enemy>().maxHealth + wavesMass[i].wavesAll[o].enemyPrefab.GetComponent<Enemy>().maxHealth * (i / 5 * 15));
                }
            }
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
    public float spawnInterval;
    public int maxEnemies = 20;
}

[System.Serializable]
public class WaveMass
{
    public Wave[] wavesAll;
}