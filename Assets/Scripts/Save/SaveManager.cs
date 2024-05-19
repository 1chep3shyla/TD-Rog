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
        public int[] countItem = new int[17];
        public float[] buffs;
        public bool[] interactableIs = new bool[5];
        public int[] indexTowerBut = new int[5];
        public int[,] towerLevels = new int[20, 7];
        public int[,] indexTower = new int[20, 7];
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
        public bool[] boughtStat = new bool[32];
        public float[] buff = new float[9];
        public int getItem;
        public int sellItem;
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
    public static SaveManager instance;

    void Start()
    {
        LoadGameData();
    }

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
    }

    public void SaveData()
    {
        SaveGameData();
        LevelData data = new LevelData();
        data.currentLevel = GameManager.Instance.spawn.currentWaveIndexMain;
        data.indexState = GameBack.Instance.indexState;
        data.gold = GameManager.Instance.Gold;
        data.health = GameManager.Instance.Health;
        data.maxHealth = GameManager.Instance.maxHealth;
        data.buffs = GameManager.Instance.buff;
        data.indexChar = GameBack.Instance.charData.GetIndex();
        for(int iItem = 0; iItem <17; iItem++)
        {
            data.countItem[iItem] = items[iItem].count;
        }
        for(int a = 0; a < 5; a++)
        {
            data.indexTowerBut[a] = GameManager.Instance.gameObject.GetComponent<Rolling>().slots[a].id;
            data.interactableIs[a] = GameManager.Instance.gameObject.GetComponent<Rolling>().butChoose[a].interactable;
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
            GameManager.Instance.spawn.currentWaveIndexMain = data.currentLevel;
            GameManager.Instance.Gold = data.gold;
            GameManager.Instance.Health = data.health;
            GameManager.Instance.maxHealth = data.maxHealth;
            GameManager.Instance.buff = data.buffs;
            GameManager.Instance.ChangeMoney();
            for(int a = 0; a < 5; a++)
            {
                GameManager.Instance.gameObject.GetComponent<Rolling>().slots[a].id = data.indexTowerBut[a];
                roll.slots[a].icon.sprite = roll.imageidTower[roll.slots[a].id];
                roll.slots[a].tower = roll.towers[data.indexTowerBut[a]];
                roll.nameOfTowerText[a].text = roll.slots[a].tower.GetComponent<UpHave>().name;
                roll.butChoose[a].interactable = data.interactableIs[a];
            }
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
            for(int iItem = 0; iItem <17; iItem++)
            {
                for(int o = 0; o < data.countItem[iItem]; o++)
                {
                    itemOpenner.curItem = items[iItem];
                    itemOpenner.ClaimSave();
                }
                //items[iItem].count = data.countItem[iItem];
            }

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
        data.loseFirstWave = GameBack.Instance.loseFirstWave;
        data.enemiesKilled = GameBack.Instance.enemiesKilled;
        Array.Copy(GameBack.Instance.buff, data.buff, data.buff.Length);
        data.getItem = GameBack.Instance.getItem;
        data.sellItem = GameBack.Instance.sellItem;
        data.soulsCount = GameBack.Instance.souls;
        for(int i = 0; i < GameBack.Instance.boughtStat.Length; i++ )
        {
            data.boughtStat[i] = GameBack.Instance.boughtStat[i];
        }

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
            GameBack.Instance.loseFirstWave = data.loseFirstWave;
            GameBack.Instance.enemiesKilled = data.enemiesKilled;
            Array.Copy(data.buff, GameBack.Instance.buff, GameBack.Instance.buff.Length);
            GameBack.Instance.getItem = data.getItem;
            GameBack.Instance.sellItem = data.sellItem;
            GameBack.Instance.souls = data.soulsCount;
            for(int i = 0; i < GameBack.Instance.boughtStat.Length; i++ )
            {
                GameBack.Instance.boughtStat[i] = data.boughtStat[i];
            }
        }
    }
}