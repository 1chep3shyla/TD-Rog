using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Spawner : MonoBehaviour
{
    public WayScript[] waypoints;

    public WaveMass[] wavesMass;
    public float timeBetweenWaves = 5;
    public float timeCur;
    [SerializeField]
    private GameManager gameManager;

    private float lastSpawnTime;
    private float waveEndTime;
    private int enemiesSpawned = 0;

    public int currentWaveIndex = 0;
    public int currentWaveIndexMain = 0;
    public Wave[] currentWave;
    public bool works;
    public bool skip;
    private bool start;

    private bool shouldStartNextWave = false;



    private void Start()
    {
        if (wavesMass.Length > 0)
        {
            currentWave = wavesMass[currentWaveIndex].wavesAll;
        }
        GameManager.Instance.spawn = this;
    }
    void Update()
    {
        while (currentWaveIndex < wavesMass.Length && !works && start)
        {
            works = true;

            StartCoroutine(StartWave());
            return;

        }

        GameManager.Instance.curWave = currentWaveIndexMain + 1;
    }

    public IEnumerator StartWave()
    {
        float timeNeed = currentWave[currentWaveIndex].timeAll / currentWave[currentWaveIndex].maxEnemies;

        while (currentWaveIndex < currentWave.Length && !skip)
        {
            yield return new WaitForSeconds(timeNeed);
            int RandomEnemy = UnityEngine.Random.Range(0, currentWave[currentWaveIndex].enemyPrefab.Length);
            for (int i = 0; i < currentWave[currentWaveIndex].maxEnemies && !skip; i++)
            {

                SpawnEnemy(RandomEnemy);

                yield return new WaitForSeconds(timeNeed);
            }

            yield return new WaitUntil(() => enemiesSpawned == 0);
            currentWaveIndex++;
        }
        yield return new WaitUntil(() => gameManager.enemiesAll.Count == 0);
        gameManager.ClearEffects();
        Debug.Log("������ ���");
        if(!skip)
        {
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine(ClaimReward());
        Time.timeScale = 0;
        yield return new WaitUntil(() => gameManager.gameObject.GetComponent<PerkRoll>().rollingEvolve == false);
        currentWaveIndexMain++;
        currentWaveIndex = 0;
        currentWave = wavesMass[currentWaveIndexMain].wavesAll;
        gameManager.ClearRounds();
        Time.timeScale = 1f;
        yield return new WaitUntil(() => gameManager.gameObject.GetComponent<PerkRoll>().rollingEvolve == false);
        timeCur = timeBetweenWaves;
        while(timeCur <=0f)
        {
            timeCur -= Time.deltaTime;
        }
        works = false;
        skip = false;
        yield return null;
    }
    public IEnumerator ClaimReward()
    {
        if (wavesMass[currentWaveIndexMain].wavesAll[currentWaveIndex - 1].bossWave == true)
        {
            Debug.Log("���� �����");
            gameManager.gameObject.GetComponent<PerkRoll>().RollPerkEvolve();
            yield return new WaitUntil(() => gameManager.gameObject.GetComponent<PerkRoll>().rollingEvolve == false);
            yield return new WaitForSeconds(0.5f);
            gameManager.gameObject.GetComponent<PerkRoll>().rollingEvolve = true;
            gameManager.gameObject.GetComponent<PerkRoll>().RollPerk();
        }
        else
        {
            Debug.Log("�����");
            gameManager.gameObject.GetComponent<PerkRoll>().rollingEvolve = true;
            gameManager.gameObject.GetComponent<PerkRoll>().RollPerk();
        }
    }

    private void SpawnEnemy(int RandomEnemy)
    {
        for (int i = 0; i < currentWave[currentWaveIndex].curWave + 1 && !skip; i++)
        {
            GameObject newEnemy = Instantiate(currentWave[currentWaveIndex].enemyPrefab[RandomEnemy], waypoints[i].waypoints[0].transform.position, Quaternion.identity);
            GameManager.Instance.AddEnemyToList(newEnemy);
            newEnemy.GetComponent<EnemyMoving>().waypoints = waypoints[i].waypoints;
            newEnemy.GetComponent<Enemy>().health = newEnemy.GetComponent<Enemy>().maxHealth;

            GameBack.Instance.curFormula = SubstituteVariables(GameBack.Instance.curFormula, newEnemy.GetComponent<Enemy>().maxHealth, currentWaveIndexMain);

            float result = EvaluateFormula(GameBack.Instance.curFormula);

            Debug.Log($"Result: {result}");
            newEnemy.GetComponent<Enemy>().maxHealth = (int)result;
            newEnemy.GetComponent<Enemy>().health = newEnemy.GetComponent<Enemy>().maxHealth;
            if (currentWaveIndexMain <= 10)
            {
               // newEnemy.GetComponent<Enemy>().goldGive = newEnemy.GetComponent<Enemy>().goldGive + (2 * currentWaveIndexMain);
            }
            else if (currentWaveIndexMain > 10 && currentWaveIndexMain <= 20)
            {
                //newEnemy.GetComponent<Enemy>().goldGive = (int)(newEnemy.GetComponent<Enemy>().goldGive * Math.Pow(1.137f, currentWaveIndexMain - 10));
            }
            else
            {
              //  newEnemy.GetComponent<Enemy>().goldGive = (int)(newEnemy.GetComponent<Enemy>().goldGive * Math.Pow(1.137f, currentWaveIndexMain - 20));
            }
        }
    }
    public void StartingGame()
    {
        if(!start)
        {
            start = true;
        }
        else
        {
            timeCur = 0;   
        }
    }

    string SubstituteVariables(string formula, int hpEnemy, int curWave)
    {
        formula = formula.Replace("hp", hpEnemy.ToString());
        formula = formula.Replace("curWave", curWave+1.ToString());
        return formula;
    }

    float EvaluateFormula(string formula)
    {
        try
        {
            Debug.Log($"Formula before evaluation: {formula}");
            object evalResult = eval(formula);
            Debug.Log($"Evaluated result: {evalResult}");

            if (evalResult != null)
            {
                return Convert.ToSingle(evalResult);
            }
            else
            {
                return 0.0f;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error evaluating formula: " + ex.Message);
            return 0.0f; // Handle the error as needed
        }
    }


    object eval(string expression)
    {
        System.Data.DataTable table = new System.Data.DataTable();
        table.Columns.Add("expression", typeof(string), expression);
        System.Data.DataRow row = table.NewRow();
        table.Rows.Add(row);
        return row["expression"];
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
    public int curWave;
}

[System.Serializable]
public class WaveMass
{
    public Wave[] wavesAll;
}
[System.Serializable]
public class WayScript
{
    public GameObject[] waypoints;
}