using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Linq;
[System.Serializable]
public class GameManager : MonoBehaviour
{

    public bool gameOver = false;
    [SerializeField]
    public int Gold;
    [SerializeField]
    public int Health;
    [SerializeField]
    public int maxHealth;
    [SerializeField]
    public int restoreHeal;
    public int whichEnemyKill;
    public int GoldGivenPerWave;
    public float enemyBuff = 1f;
    public SpriteRenderer[] allTower;
    public Image charIcon;
    public GameObject needMore;
    public TMP_Text goldCount;
    public TMP_Text healthCount;
    public TMP_Text waveCount;
    public TMP_Text DiscriptionText;
    public TMP_Text NameText;
    public TMP_Text[] DiscriptionStatText;
    public GameObject discItemGM;
    public int curWave;
    public int enemyHave;
    public ItemOpenner itemOpenner;
    private bool anima;
    public GameObject menu;
    public GameObject con;
    public GameObject losing;
    public GameObject startBut;
    public GameObject boomImpactCrit;
    public GameObject boomIconGM;
    private static GameManager instance;
    public List<GameObject> enemiesAll = new List<GameObject>();
    [SerializeField]
    public float[] buff; // 0 - global damage, 1- ice buff, 2 - fire damage, 3 - poison, 4 - moneyMine, 5 - AttackSpeed, 6 - moneyThief+Enemy, 7 - crit DMG, 8 - crit Chance
    public float[] newBuff;
    public float[] secondsBuff = new float[15]; 
    // 0 - iceTime+, 1 - poisonTime+, 2 - fireTime+, 3 - iceDMG, 4 - poisonDMG+, 5 - fireDMG+, 6 - stanChance+, 
    //7 - stanUp+, 8 - critBF+, 9 - critical Crit+, 10 - saleRoll+, 11 - moreSouls+, 12 - addRange+, 13 - addTarget+, 14 - add Gold After Wave+, 15 - goldEnemy+
    public Evolve[] allEvolution;
    public DMGTower[] allTowerDMG;
    public ICharSet charData;
    public GameObject[] states;
    public Transform enemyWhichGM;
    public Tilemap[] maps;
    public Transform goldPos;
    public Transform chestPos;
    public Spawner spawn;
    public AudioSource aS;
    public Item item;
    public ParticleSystem takeDamagePS;
    public ParticleSystem healing;
    public ParticleSystem goldParticle;
    public BoxCollider2D tilemapCollider;
    public BoxCollider2D[] collidersTile;
    public Sprite whatSprite;
    public int healthBreak;
    [SerializeField]
    public int damageAll;
    public int currentWaveIndexMain;
    public LightUsing lightUsing;
    public Animator cardAnim;
    public GameObject thiefPart;
    public GameObject difficultyGM;
    [Space]
    public AudioClip takeHPSFX;
    public AudioClip creappyLaught;
    public AudioClip clickTowerSFX;
    public AudioClip upgradeTowerSFX;
    [Space]
    public Animator demonAnim;
    [Space]
    public Material goldMaterial;
    private Coroutine[] typingCoroutine = new Coroutine[100]; // Переменная для отслеживания текущей корутины
    private int[] giveMoneyCheat;

    void Update()
    {
        healthCount.text = Health.ToString("");
        waveCount.text = curWave.ToString("") + "/30";
        if(itemOpenner.countChest > 0)
        {
            chestPos.parent.gameObject.SetActive(true);
        }
        else
        {
            chestPos.parent.gameObject.SetActive(false);
        }
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
        if(GameBack.Instance.charData != null)
        {
            charData = GameBack.Instance.charData;
            charData.SetData();
            charIcon.sprite = GameBack.Instance.iconChar;
            if(!GameBack.Instance.saveThis)
            {
                states[GameBack.Instance.indexState].SetActive(true);
                difficultyGM.SetActive(true);
                for (int i = 0; i < GameBack.Instance.secondsBuff.Length; i++)
                {
                    secondsBuff[i] = GameBack.Instance.secondsBuff[i];
                }
            }
            if(GameBack.Instance.saveThis == true)
            {
                SaveManager.instance.LoadData();
            }
            tilemapCollider = collidersTile[GameBack.Instance.indexState];
            gameObject.GetComponent<Rolling>().tilemap = maps[GameBack.Instance.indexState];
        }

    }
    public void SetDataBack()
    {
        Debug.Log("Выйди сообщение");
        charData = GameBack.Instance.charData;
        charData.SetData();
        charIcon.sprite = GameBack.Instance.iconChar;
        states[GameBack.Instance.indexState].SetActive(true);
        tilemapCollider = collidersTile[GameBack.Instance.indexState];
        gameObject.GetComponent<Rolling>().tilemap = maps[GameBack.Instance.indexState];
    }

    public void AfterWaveGoldGive()
    {
        if(secondsBuff[14]/100 > 0)
        {
            int goldNeedGive = (int)((float)GoldGivenPerWave * (secondsBuff[14]/100));
            var emission = goldParticle.emission;
            emission.rateOverTime = 30f+((float)goldNeedGive/10);
            Gold+=goldNeedGive;
            goldParticle.Play();
            ChangeMoney();
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        aS.pitch = Random.Range(0.9f,1.1f);
        aS.PlayOneShot(clip);
    }
    public void Heal()
    {
        if (Health + restoreHeal > maxHealth)
        {
            Health = maxHealth;
            healing.Play();
            var emission = healing.emission;
            emission.rateOverTime = 15f*restoreHeal;
        }
        else if(Health + restoreHeal > 0)
        {
            healing.Play();
            var emission = healing.emission;
            emission.rateOverTime = 15f*restoreHeal;
            Health += restoreHeal;
        }
        else
        {
            Health = 1;
        }
    }
    public void TakeDamageHealth(int dmg)
    {
          if (Health > dmg)
            {
                Health -= dmg;
                healthBreak+=dmg;
                UpItemsHit(dmg);
                GameBack.Instance.healthBreak+=dmg;
                aS.PlayOneShot(takeHPSFX);
            }
            else
            {
                Health = 0;
                healthBreak+=dmg;
                Losing();
            }
    
    }
    public void UpItemsHit(int dmg)
    {
        if (itemOpenner.items.Contains(itemOpenner.rareItem[10]))
        {
            for(int i = 0; i < buff.Length; i++)
                {
                    buff[i] +=((1*itemOpenner.rareItem[10].count)*dmg);
                }
            demonAnim.gameObject.SetActive(true);
            aS.PlayOneShot(creappyLaught);
            demonAnim.Play("demonRing_anim", -1, 0f);
        }
        if (itemOpenner.items.Contains(itemOpenner.rareItem[9]))
        {
            maxHealth +=((1*itemOpenner.rareItem[9].count)*dmg);
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
    public void TakeDamagePlayer()
    {
        takeDamagePS.Play();
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
        if(losing.activeSelf == false)
        {
            menu.SetActive(true);
            
            GetComponent<LoseWinScript>().pauseGM.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void Losing()
    {
        menu.SetActive(true);
        gameObject.GetComponent<LoseWinScript>().Result(false);
        con.SetActive(false);
        losing.SetActive(true);
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
        if(Gold > GameBack.Instance.gold)
        {
            GameBack.Instance.gold = Gold;
        }
        Gold += add + (int)((float)add* (buff[4]/100));
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
        gameObject.GetComponent<PerkRoll>().costReroll = 300;
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
        startBut.SetActive(false);
    }
    public void SetEnemyAddHealth(float addBuff)
    {
        enemyBuff += addBuff;
    }

    public void ChestClaim()
    {
        itemOpenner.countChest++;
    }
    void OnDestroy()
    {
       Destroy(GameManager.Instance.gameObject);
    }
     // Функция для запуска корутины по отображению текста
    public void StartTypingText(TMP_Text[] textComponents, string[] texts, float totalDuration)
    {
        // Остановка текущих корутин, если они запущены
        for (int i = 0; i < typingCoroutine.Length; i++)
        {
            if (typingCoroutine[i] != null)
            {
                StopCoroutine(typingCoroutine[i]);
                typingCoroutine[i] = null;
            }
        }

        typingCoroutine[0] = StartCoroutine(TypeTextsSimultaneously(textComponents, texts, totalDuration));
    }

    // Корутина для поэтапного отображения текста одновременно во всех текстовых компонентах
    public IEnumerator TypeTextsSimultaneously(TMP_Text[] textComponents, string[] texts, float totalDuration)
    {
        // Запуск корутин для каждого текстового компонента
        for (int i = 0; i < textComponents.Length; i++)
        {
            if (i + 4 >= typingCoroutine.Length) // Проверка выхода за пределы массива
            {
                Debug.LogError("Размер массива typingCoroutine недостаточен");
                yield break;
            }

            typingCoroutine[i + 4] = StartCoroutine(TypeText(textComponents[i], texts[i], totalDuration));
        }

        yield return null;
    }

    // Корутина для поэтапного отображения текста в одном текстовом компоненте
    public IEnumerator TypeText(TMP_Text textComponent, string text, float totalDuration)
    {
        textComponent.text = "";
        float delay = totalDuration / text.Length; // Задержка между символами
        string processedText = ProcessTextWithColor(text);
        int index = 0;

        while (index < processedText.Length)
        {
            if (processedText[index] == '<')
            {
                while (processedText[index] != '>')
                {
                    textComponent.text += processedText[index];
                    index++;
                }
                textComponent.text += '>';
                index++;
            }
            else
            {
                textComponent.text += processedText[index];
                index++;
                if (totalDuration != 0)
                {
                    yield return new WaitForSecondsRealtime(delay);
                }
            }
        }

        // Убедитесь, что текст полностью отображен
        textComponent.text = processedText;
    }

    private string ProcessTextWithColor(string text)
    {
        // Заменяем плейсхолдеры на цветной текст
        string colorStartTag = "<color=#808080>";
        string colorEndTag = "</color>";

        // Замените эту строку на нужный вам текст
        string replacementValueStan = "замененный текст";

        return text.Replace("{replacementValueStan}", colorStartTag + replacementValueStan + colorEndTag);
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