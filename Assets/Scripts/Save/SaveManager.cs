using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    // Класс для данных о текущем уровне
    [System.Serializable]
    public class LevelData
    {
        public int currentLevel;
        public int gold;
        public int health;
        public int maxHealth;
        public int indexChar;
        public int indexState;
        public int[] countItem = new int[27];
        public float[] buffs;
        public bool[] interactableIs = new bool[5];
        public int[] lvlTower = new int[5];
        public bool[] evolutionGet = new bool[15];
        public int[] indexTowerBut = new int[5];
        public int[,] towerLevels = new int[20, 7];
        public int[,] indexTower = new int[20, 7];
        public int healthBreak;
        public PositionData[,] towerPositions = new PositionData[20, 7]; // Изменение типа массива
    }

    [System.Serializable]
    public class GameData
    {
        public int gold;
        public int soulsCount;
        public int waveCount;
        public int gamePlayed;
        public int winGame;
        public bool loseFirstWave;
        public int enemiesKilled;
        public bool[] boughtStat = new bool[72];
        public bool[] seeEnemy = new bool[50];
        public float[] buff = new float[9];
        public float[] secondsBuff = new float [16];
        public int getItem;
        public int sellItem;
        public int damageAll;
        public int healthBreak;
        public int healthWin;
        public int poisonedCount;
        public int fireCount;
        public int iceCount;
        public int towerSet;
        public int hardWinning;
        public int perfecto;
        public bool firstEvolve;
        public float sfxCount;
        public float musicCount;
        public int[] waveInStage = new int[14];
    }
    // Класс для хранения позиции, который реализует интерфейс Serializable
    [System.Serializable]
    public class PositionData
    {
        public float x;
        public float y;
        public float z;

        public PositionData(Vector3 position)
        {
            x = position.x;
            y = position.y;
            z = position.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }

    public GameObject[] towerSpawning;
    public ItemOpenner itemOpenner;
    public Item[] items;
    public ScriptableObject[] character;
    public GameObject towerBase;
    public GlobalSee globalSee;
    public static SaveManager instance;


    private void Awake()
    {
    
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        LoadGameData();
    }

    public void SaveData()
    {
        SaveGameData();
        LevelData data = new LevelData();
        data.currentLevel = GameManager.Instance.spawn.currentWaveIndexMain;
        for(int evolutionGetInt = 0; evolutionGetInt <15; evolutionGetInt++)
        {
            data.evolutionGet[evolutionGetInt] = GameManager.Instance.allEvolution[evolutionGetInt].work;
        }
        data.indexState = GameBack.Instance.indexState;
        data.gold = GameManager.Instance.Gold;
        data.health = GameManager.Instance.Health;
        data.maxHealth = GameManager.Instance.maxHealth;
        data.buffs = GameManager.Instance.buff;
        data.indexChar = GameBack.Instance.charData.GetIndex();
        for(int iItem = 0; iItem <data.countItem.Length; iItem++)
        {
            data.countItem[iItem] = items[iItem].count;
        }
        for(int a = 0; a < 5; a++)
        {
            data.indexTowerBut[a] = GameManager.Instance.gameObject.GetComponent<Rolling>().slots[a].id;
            data.interactableIs[a] = GameManager.Instance.gameObject.GetComponent<Rolling>().butChoose[a].interactable;
            data.lvlTower[a] = GameManager.Instance.gameObject.GetComponent<Rolling>().slots[a].tower.GetComponent<UpHave>().LVL;
        }
        for (int i = 0; i < 20; i++)
        {
            for (int io = 0; io < 7; io++)
            {
                TowerBase baseObject = GameManager.Instance.gameObject.GetComponent<Rolling>().allBases[i, io];
                if (baseObject != null && baseObject.curGM != null && baseObject.curGM.GetComponent<UpHave>() != null)
                {
                    data.towerLevels[i, io] = baseObject.curGM.GetComponent<UpHave>().LVL;
                    data.indexTower[i, io] = baseObject.curGM.GetComponent<UpHave>().SaveID;
                    data.towerPositions[i, io] = new PositionData(baseObject.curGM.transform.position); // Сохранение позиции как PositionData
                }
            }
        }
        data.healthBreak = GameManager.Instance.healthBreak;

        BinaryFormatter formatter = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "/levelData.dat";
        FileStream stream = new FileStream(filePath, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Данные сохранены");
    }

    public void LoadData()
    {
        string filePath = Application.persistentDataPath + "/levelData.dat";
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);

            LevelData data = formatter.Deserialize(stream) as LevelData;
            stream.Close();
            if (character[data.indexChar] is ICharSet characterObject)
            {
                characterObject.ApplyBuff();
            }
            Rolling roll = GameManager.Instance.gameObject.GetComponent<Rolling>();
            GameBack.Instance.indexState = data.indexState;
            GameManager.Instance.SetDataBack();
            GameManager.Instance.states[data.indexState].SetActive(true);
            GameManager.Instance.spawn = GameObject.Find($"{GameManager.Instance.states[data.indexState].name}/Spawner").GetComponent<Spawner>();
            GameManager.Instance.spawn.currentWaveIndexMain = data.currentLevel;
            GameManager.Instance.Gold = data.gold;
            GameManager.Instance.Health = data.health;
            GameManager.Instance.maxHealth = data.maxHealth;
            GameManager.Instance.buff = data.buffs;
            GameManager.Instance.ChangeMoney();
            for(int evolutionGetInt = 0; evolutionGetInt <15; evolutionGetInt++)
            {
                GameManager.Instance.allEvolution[evolutionGetInt].work = data.evolutionGet[evolutionGetInt];
                if(data.evolutionGet[evolutionGetInt])
                {
                    Rolling rolles =  GameManager.Instance.gameObject.GetComponent<Rolling>();
                    for(int i =0; i < rolles.towers.Length; i++)
                    {
                        if(rolles.towers[i].GetComponent<UpHave>().id == GameManager.Instance.allEvolution[evolutionGetInt].index)
                        {
                            rolles.towers[i] =  GameManager.Instance.allEvolution[evolutionGetInt].EvolveScript;
                            rolles.imageidTower[i] = GameManager.Instance.allEvolution[evolutionGetInt].EvolveScript.GetComponent<SpriteRenderer>().sprite;
                        }
                    }
                }
            }
            for(int a = 0; a < 5; a++)
            {
                GameManager.Instance.gameObject.GetComponent<Rolling>().slots[a].id = data.indexTowerBut[a];
                roll.slots[a].icon.sprite = roll.imageidTower[roll.slots[a].id];
                roll.slots[a].tower = roll.towers[data.indexTowerBut[a]];
                roll.nameOfTowerText[a].text = roll.slots[a].tower.GetComponent<UpHave>().name;
                roll.butChoose[a].interactable = data.interactableIs[a];
                roll.slots[a].tower.GetComponent<UpHave>().LVL = data.lvlTower[a];
            }
            roll.UpdateInfoBut();
            for (int i = 0; i < 20; i++)
            {
                for (int io = 0; io < 7; io++)
                {
                    if (data.indexTower[i, io] != 0)
                    {
                        GameObject towerSome = Instantiate(towerSpawning[data.indexTower[i, io] - 1], data.towerPositions[i, io].ToVector3(), Quaternion.identity);
                        GameObject towerBasing = Instantiate(towerBase, towerSome.transform.position , Quaternion.identity);
                        towerBasing.GetComponent<TowerBase>().rollBase = roll;
                        towerBasing.GetComponent<TowerBase>().curGM = towerSome;
                        roll.AddTower(towerSome.GetComponent<SpriteRenderer>());
                        towerSome.GetComponent<UpHave>().LVL = data.towerLevels[i, io];
                        towerSome.GetComponent<UpHave>().baseOf = towerBasing.GetComponent<TowerBase>();
                        roll.allBases[i, io] = towerBasing.GetComponent<TowerBase>();
                        towerBasing.GetComponent<TowerBase>().monster = towerSome;
                        towerBasing.GetComponent<TowerBase>().level = data.towerLevels[i, io];
                    }
                }
            }
            for(int iItem = 0; iItem <data.countItem.Length; iItem++)
            {
                for(int o = 0; o < data.countItem[iItem]; o++)
                {
                    itemOpenner.curItem = items[iItem];
                    itemOpenner.ClaimSave();
                }
                //items[iItem].count = data.countItem[iItem];
            }
            GameManager.Instance.healthBreak = data.healthBreak;

            Debug.Log("Данные загружены");
        }
        else
        {
            Debug.LogWarning("Файл сохранения не найден");
        }
    }
     public void SaveGameData()
    {
        GameData data = new GameData();

        // Сохраняемые данные из GameBack.Instance
        data.gold = GameBack.Instance.gold;
        data.waveCount = GameBack.Instance.waveCount;
        data.gamePlayed = GameBack.Instance.gamePlayed;
        data.winGame = GameBack.Instance.winGame;
        for(int i = 0; i < data.waveInStage.Length; i++)
        {
            data.waveInStage[i] = GameBack.Instance.waveInStage[i];
        }
        for(int seeEnemyLocal = 0; seeEnemyLocal <data.seeEnemy.Length; seeEnemyLocal++)
        {
            data.seeEnemy[seeEnemyLocal] = Beastiar.Instance.seeThis[seeEnemyLocal];
        }
        data.loseFirstWave = GameBack.Instance.loseFirstWave;
        data.enemiesKilled = GameBack.Instance.enemiesKilled;
        data.damageAll = GameBack.Instance.damageAll;
        data.sfxCount = GameBack.Instance.volumeSFX;
        data.musicCount = GameBack.Instance.volumeMusic;
        Array.Copy(GameBack.Instance.buff, data.buff, data.buff.Length);
        Array.Copy(GameBack.Instance.secondsBuff, data.secondsBuff, data.secondsBuff.Length);
        data.getItem = GameBack.Instance.getItem;
        data.sellItem = GameBack.Instance.sellItem;
        data.soulsCount = GameBack.Instance.souls;
        for(int i = 0; i < GameBack.Instance.boughtStat.Length; i++ )
        {
            data.boughtStat[i] = GameBack.Instance.boughtStat[i];
        }
        data.healthBreak = GameBack.Instance.healthBreak;
        data.healthWin = GameBack.Instance.healthWin;
        data.poisonedCount = GameBack.Instance.poisonedCount;
        data.fireCount = GameBack.Instance.fireCount;
        data.iceCount = GameBack.Instance.iceCount;
        data.towerSet = GameBack.Instance.towerSet;
        data.hardWinning = GameBack.Instance.hardWinning;
        data.perfecto = GameBack.Instance.perfecto;
        data.firstEvolve = GameBack.Instance.firstEvolve;
        BinaryFormatter formatter = new BinaryFormatter();
        string filePath = Application.persistentDataPath + "/achivementData.dat";
        FileStream stream = new FileStream(filePath, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Данные сохранены");
    }

    // Функция для загрузки данных в GameBack.Instance
    public void LoadGameData()
    {
    string filePath = Application.persistentDataPath + "/achivementData.dat";
        if (File.Exists(filePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();
            GameBack.Instance.gold = data.gold;
            GameBack.Instance.waveCount = data.waveCount;
            GameBack.Instance.gamePlayed = data.gamePlayed;
            GameBack.Instance.winGame = data.winGame;
            GameBack.Instance.volumeSFX = data.sfxCount;
            GameBack.Instance.volumeMusic = data.musicCount;
            GameBack.Instance.loseFirstWave = data.loseFirstWave;
            GameBack.Instance.enemiesKilled = data.enemiesKilled;
            Array.Copy(data.secondsBuff, GameBack.Instance.secondsBuff, GameBack.Instance.secondsBuff.Length);
            GameBack.Instance.getItem = data.getItem;
            GameBack.Instance.sellItem = data.sellItem;
            GameBack.Instance.souls = data.soulsCount;
            for(int seeEnemyLocal = 0; seeEnemyLocal <data.seeEnemy.Length; seeEnemyLocal++)
            {
                Beastiar.Instance.seeThis[seeEnemyLocal] = data.seeEnemy[seeEnemyLocal];
            }
            for(int i = 0; i < GameBack.Instance.boughtStat.Length; i++ )
            {
                GameBack.Instance.boughtStat[i] = data.boughtStat[i];
            }
            GameBack.Instance.damageAll = data.damageAll;
            GameBack.Instance.healthBreak = data.healthBreak;
            GameBack.Instance.healthWin = data.healthWin;
            GameBack.Instance.poisonedCount = data.poisonedCount;
            GameBack.Instance.fireCount = data.fireCount;
            GameBack.Instance.iceCount = data.iceCount;
            GameBack.Instance.towerSet = data.towerSet;
            GameBack.Instance.hardWinning = data.hardWinning;
            GameBack.Instance.perfecto = data.perfecto ;
            for(int i = 0;data.waveInStage != null && i < data.waveInStage.Length; i++)
            {
                GameBack.Instance.waveInStage[i] = data.waveInStage[i];
            }
            GameBack.Instance.firstEvolve = data.firstEvolve;
        }
    }
}