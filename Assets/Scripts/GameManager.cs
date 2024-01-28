using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{

    public bool gameOver = false;
    public int Gold;
    public int Health;
    public int maxHealth;
    public int restoreHeal;
    public SpriteRenderer[] allTower;
    public Image charIcon;
    public GameObject needMore;
    public TMP_Text goldCount;
    public TMP_Text healthCount;
    public TMP_Text waveCount;
    public int curWave;
    public int enemyHave;
    private bool anima;
    public GameObject menu;
    private static GameManager instance;
    public List<GameObject> enemiesAll = new List<GameObject>();
    public float[] buff; // 0 - global damage, 1- ice buff, 2 - fire damage, 3 - poison, 4 - moneyMine, 5 - AttackSpeed, 6 - moneyThief+Enemy, 7 - crit DMG, 8 - crit Chance
    public Evolve[] allEvolution;
    public DMGTower[] allTowerDMG;
    public ICharSet charData;
    public GameObject[] states;
    public Tilemap[] maps;
    public Transform goldPos;
    public Spawner spawn;
    public AudioSource aS;
    [SerializeField]
    private int[] giveMoneyCheat;

    void Update()
    {
        healthCount.text = Health.ToString("");
        waveCount.text = curWave.ToString("") + "/20";

        if (Input.GetKeyDown("escape"))
        {
            Pause();
        }

    }
    void Start()
    {
        curWave = 0;
        Health = maxHealth;
        if (instance == null)
        {
            instance = this;
        }
        charData = GameBack.Instance.charData;
        charData.SetData();
        charIcon.sprite = GameBack.Instance.iconChar;
        states[GameBack.Instance.indexState].SetActive(true);
        gameObject.GetComponent<Rolling>().tilemap = maps[GameBack.Instance.indexState];
    }
    public void Heal()
    {
        if (Health + restoreHeal > maxHealth)
        {
            Health = maxHealth;
        }
        else
        {
            Health += restoreHeal;
        }
    }
    public int Wave
    {
        get
        {
            return curWave;
        }
        set
        {
            curWave = value;
            if (!gameOver)
            {

            }
        }
    }

    public void UpLay()
    {
        for (int i = 0; i < allTower.Length; i++)
        {
            if (allTower[i] != null)
            {
                allTower[i].gameObject.GetComponent<UpHave>().Cursoring();
            }
        }

    }
    public void DownLay()
    {
        for (int i = 0; i < allTower.Length; i++)
        {
            if (allTower[i] != null)
            {
                Destroy(allTower[i].gameObject.GetComponent<UpHave>().cursorDelete);
            }
        }
    }
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = "GameManagerSingleton";
                }
            }
            return instance;
        }
    }

    public void notEnought()
    {
        if (!anima)
        {
            anima = true;
            StartCoroutine(needMoreActive());
        }
    }
    public IEnumerator needMoreActive()
    {
        needMore.SetActive(true);
        needMore.GetComponent<Animator>().Play("needMore_anim");
        yield return new WaitForSeconds(0.52f);
        anima = false;
        needMore.SetActive(false);
    }
    public void Pause()
    {
        menu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    public void AddEnemyToList(GameObject newEnemy)
    {
        enemiesAll.Add(newEnemy);
    }
    public void RemoveEnemyFromList(GameObject enemyToRemove)
    {
        enemiesAll.Remove(enemyToRemove);
    }
    public void AddMoney(int add)
    {
        Gold += add + (add* (int)(buff[4]/100));
        goldCount.text = Gold.ToString("");
    }
    public void StealMoney(int add)
    {
        Gold += add + (add * (int)(buff[6] / 100));
        goldCount.text = Gold.ToString("");
    }
    public void ChangeMoney()
    {
        goldCount.text = Gold.ToString("");
    }
    public void UpSome(int lvlUp, GameObject who)
    {
        for (int i = 0; i < allTower.Length; i++)
        {
            if (allTower[i] != null && allTower[i].gameObject != who && allTower[i].gameObject.GetComponent<UpHave>().LVL == lvlUp)
            {
                allTower[i].gameObject.GetComponent<UpHave>().baseOf.JustUpBoost();
                break;
            }
        }
    }
    public void ClearEffects()
    {
        for (int i = 0; i < allTower.Length; i++)
        {
            if (allTower[i] != null)
            {
                allTower[i].gameObject.GetComponent<UpHave>().Clear();
            }
        }
    }
    public void AddDamageByBulletType(TypeBull bulletType, int damage)
    {
        // ����� ��������������� ������� ������� allTowerDMG �� ���� ���� � �������� ����
        foreach (DMGTower towerDamage in allTowerDMG)
        {
            if (towerDamage.EvolveScript == bulletType)
            {
                towerDamage.countDamage += damage;
                towerDamage.roundDamage += damage;
            }
        }
    }
    public void ClearRounds()
    {
        Heal();
        for (int i = 0; i < allTowerDMG.Length; i++)
        {
            allTowerDMG[i].roundDamage = 0;
        }
    }
    public void SetGameSpeed(int speed)
    {
        Time.timeScale = speed;
    }
    public void NextWave()
    {
        AddMoney(giveMoneyCheat[curWave]);
        spawn.skip = true;
        for(int i = 0; i < enemiesAll.Count;i++)
        {
            if(enemiesAll!=null)
            {
            Destroy(enemiesAll[i]);
            }
        }
    }
    public void Starting()
    {
        spawn.StartingGame();
    }
}
[System.Serializable]
public class Evolve
{
    public GameObject EvolveScript;
    public bool work;
    public int index;

    public void WorkThis()
    {
        work = true;
    }
}
[System.Serializable]
public class DMGTower
{
    public TypeBull EvolveScript;
    public int countDamage;
    public int roundDamage;
    public string name;
    public Sprite iconTower;

}