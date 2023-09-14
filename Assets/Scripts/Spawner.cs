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

        GameManager.Instance.curWave = currentWaveIndexMain +1;
    }

    private IEnumerator StartWave()
    {
        float timeNeed = currentWave[currentWaveIndex].timeAll / currentWave[currentWaveIndex].maxEnemies;

        while (currentWaveIndex < currentWave.Length)
        {
            yield return new WaitForSeconds(timeNeed);
            int RandomEnemy = UnityEngine.Random.Range(0, currentWave[currentWaveIndex].enemyPrefab.Length);
            for (int i = 0; i < currentWave[currentWaveIndex].maxEnemies; i++)
            {

                SpawnEnemy(RandomEnemy);

                yield return new WaitForSeconds(timeNeed);
            }

            yield return new WaitUntil(() => enemiesSpawned == 0);
            currentWaveIndex++;
        }
        yield return new WaitUntil(() => gameManager.enemiesAll.Count == 0);
        gameManager.ClearEffects();
        Debug.Log("Врагов нет");
        yield return new WaitForSeconds(1f);
        if (wavesMass[currentWaveIndexMain].wavesAll[currentWaveIndex-1].bossWave == true)
        {
            Debug.Log("Босс волна");
            gameManager.gameObject.GetComponent<PerkRoll>().RollPerkEvolve();
            yield return new WaitUntil(() => gameManager.gameObject.GetComponent<PerkRoll>().rollingEvolve == false);
            yield return new WaitForSeconds(0.5f);
            gameManager.gameObject.GetComponent<PerkRoll>().rollingEvolve = true;
            gameManager.gameObject.GetComponent<PerkRoll>().RollPerk();
        }
        else
        {
            Debug.Log("Волна");
            gameManager.gameObject.GetComponent<PerkRoll>().rollingEvolve = true;
            gameManager.gameObject.GetComponent<PerkRoll>().RollPerk();
        }
        Time.timeScale = 0;
        yield return new WaitUntil(() => gameManager.gameObject.GetComponent<PerkRoll>().rollingEvolve == false);
        if (currentWaveIndex >= wavesMass[currentWaveIndexMain].wavesAll.Length)
        {
            currentWaveIndexMain++;
            currentWaveIndex = 0;
            currentWave = wavesMass[currentWaveIndexMain].wavesAll;
        }
        gameManager.ClearRounds();
        Time.timeScale = 1f;
        yield return new WaitForSeconds(10f);
        works = false;
        yield return null;
    }

    private void SpawnEnemy(int RandomEnemy)
    {
        GameObject newEnemy = Instantiate(currentWave[currentWaveIndex].enemyPrefab[RandomEnemy], waypoints[0].transform.position, Quaternion.identity);
        GameManager.Instance.AddEnemyToList(newEnemy);
        newEnemy.GetComponent<EnemyMoving>().waypoints = waypoints;
        newEnemy.GetComponent<Enemy>().health = newEnemy.GetComponent<Enemy>().maxHealth;
        newEnemy.GetComponent<Enemy>().maxHealth = newEnemy.GetComponent<Enemy>().maxHealth + (newEnemy.GetComponent<Enemy>().maxHealth*(currentWaveIndexMain/5));
        if (currentWaveIndexMain <= 10)
        {
            newEnemy.GetComponent<Enemy>().maxHealth = newEnemy.GetComponent<Enemy>().maxHealth + (newEnemy.GetComponent<Enemy>().maxHealth * (currentWaveIndexMain / 5));
            newEnemy.GetComponent<Enemy>().health = newEnemy.GetComponent<Enemy>().maxHealth;
            newEnemy.GetComponent<Enemy>().goldGive = newEnemy.GetComponent<Enemy>().goldGive + (2 * currentWaveIndexMain);
        }
        else if (currentWaveIndexMain > 10 && currentWaveIndexMain <= 20)
        {
            newEnemy.GetComponent<Enemy>().maxHealth = newEnemy.GetComponent<Enemy>().maxHealth + (newEnemy.GetComponent<Enemy>().maxHealth * (currentWaveIndexMain / 5 * 5));
            newEnemy.GetComponent<Enemy>().health = newEnemy.GetComponent<Enemy>().maxHealth;
            newEnemy.GetComponent<Enemy>().goldGive = (int)(newEnemy.GetComponent<Enemy>().goldGive * Math.Pow(1.137f, currentWaveIndexMain - 10));
        }
        else
        {
            newEnemy.GetComponent<Enemy>().maxHealth = newEnemy.GetComponent<Enemy>().maxHealth + (newEnemy.GetComponent<Enemy>().maxHealth * (currentWaveIndexMain / 5*15));
            newEnemy.GetComponent<Enemy>().health = newEnemy.GetComponent<Enemy>().maxHealth;
            newEnemy.GetComponent<Enemy>().goldGive = (int)(newEnemy.GetComponent<Enemy>().goldGive * Math.Pow(1.137f, currentWaveIndexMain - 20));
        }
    }



}

[System.Serializable]
public class Wave
{
    public GameObject[] enemyPrefab;
    public float TimeToNext;
    public int goldWaveGiving;
    public int healthEnemy;
    public float timeAll;
    public int maxEnemies = 20;
    public bool bossWave;
}

[System.Serializable]
public class WaveMass
{
    public Wave[] wavesAll;
}