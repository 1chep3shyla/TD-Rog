    using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    public TMP_Text descriptionText;
    public TMP_Text nameText;
    public Image icon;
    public GameObject prefab;
    public GameObject prefabCursor;
    public Transform trans;
    public GameObject[] achievementsPrefabs;
    public Sprite notUnlock;

    public List<Achievement> achievements; // Список достижений

    private Dictionary<string, Achievement> achievementsDictionary = new Dictionary<string, Achievement>();

    // Событие вызывается при разблокировке достижения
    public event Action<Achievement> OnAchievementUnlocked;
    public bool need;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Создаем словарь достижений
        foreach (var achievement in achievements)
        {
            if (!achievementsDictionary.ContainsKey(achievement.id))
            {
                achievementsDictionary.Add(achievement.id, achievement);
            }
        }

        // Создаем ачивки
        if(need)
        {
            CreateAchievements();
        }
    }

    private void Update()
    {
        // Проверяем условия для разблокировки достижений
        CheckWaveCountAchievements();
        CheckGamePlayedAchievements();
        CheckWinGameAchievements();
        CheckLoseFirstWaveAchievement();
        CheckEnemiesKilledAchievements();
        CheckBuffAchievements();
        CheckGoldAchievements();
    }

    public void UpdateAchi()
    {
        for(int i = 0; i < achievementsPrefabs.Length; i++)
        {
            if(!achievements[i].isUnlocked)
            {
                achievementsPrefabs[i].transform.GetChild(0).GetComponent<Image>().sprite = notUnlock;
            }
            else
            {
                achievementsPrefabs[i].transform.GetChild(0).GetComponent<Image>().sprite = achievements[i].Icon;
            }
        }
    }
    // Методы проверки достижений опущены для краткости

    public void UnlockAchievement(string achievementID)
    {
        if (achievementsDictionary.TryGetValue(achievementID, out Achievement achievement))
        {
            if (!achievement.isUnlocked)
            {
                achievement.isUnlocked = true;
                Debug.Log("Unlocked Achievement: " + achievement.title);
                OnAchievementUnlocked?.Invoke(achievement);
            }
        }
        else
        {
            Debug.LogWarning("Achievement with ID " + achievementID + " not found!");
        }
    }
     private void CheckWaveCountAchievements()
    {
        if (GameBack.Instance.waveCount >= 10) UnlockAchievement("waveCountAchievement_10");
        if (GameBack.Instance.waveCount >= 25) UnlockAchievement("waveCountAchievement_25");
        if (GameBack.Instance.waveCount >= 100) UnlockAchievement("waveCountAchievement_100");
        if (GameBack.Instance.waveCount >= 500) UnlockAchievement("waveCountAchievement_500");
    }

    private void CheckGamePlayedAchievements()
    {
        if (GameBack.Instance.gamePlayed >= 5) UnlockAchievement("gamePlayedAchievement_5");
        if (GameBack.Instance.gamePlayed >= 25) UnlockAchievement("gamePlayedAchievement_25");
        if (GameBack.Instance.gamePlayed >= 50) UnlockAchievement("gamePlayedAchievement_50");
    }

    private void CheckWinGameAchievements()
    {
        if (GameBack.Instance.winGame >= 1) UnlockAchievement("winGameAchievement_1");
        if (GameBack.Instance.winGame >= 5) UnlockAchievement("winGameAchievement_5");
        if (GameBack.Instance.winGame >= 10) UnlockAchievement("winGameAchievement_10");
        if (GameBack.Instance.winGame >= 50) UnlockAchievement("winGameAchievement_50");
    }

    private void CheckLoseFirstWaveAchievement()
    {
        if (GameBack.Instance.loseFirstWave) UnlockAchievement("loseFirstWaveAchievement");
    }

    private void CheckEnemiesKilledAchievements()
    {
        if (GameBack.Instance.enemiesKilled >= 100) UnlockAchievement("enemiesKilledAchievement_100");
        if (GameBack.Instance.enemiesKilled >= 1000) UnlockAchievement("enemiesKilledAchievement_1000");
        if (GameBack.Instance.enemiesKilled >= 10000) UnlockAchievement("enemiesKilledAchievement_10000");
        if (GameBack.Instance.enemiesKilled >= 100000) UnlockAchievement("enemiesKilledAchievement_100000");
    }

    private void CheckBuffAchievements()
    {
        for (int i = 0; i < GameBack.Instance.buff.Length; i++)
        {
            if (GameBack.Instance.buff[i] >= 100) UnlockAchievement("buffAchievement_100");
            if (GameBack.Instance.buff[i] >= 150) UnlockAchievement("buffAchievement_150");
        }
    }

    private void CheckGoldAchievements()
    {
        if (GameBack.Instance.gold >= 1000) UnlockAchievement("goldAchievement_1000");
        if (GameBack.Instance.gold >= 10000) UnlockAchievement("goldAchievement_10000");
        if (GameBack.Instance.gold >= 100000) UnlockAchievement("goldAchievement_100000");
    }
    public void CreateAchievements()
    {
        GameObject CursorAchiGM = new GameObject("CursorAchi");
        CursorAchiGM.AddComponent<CursorWork>();
        CursorAchiGM.GetComponent<CursorWork>().buttons = new Button[achievementsDictionary.Count];
        achievementsPrefabs = new GameObject[achievements.Count];
        int index = 0;
        foreach (var achievement in achievementsDictionary.Values)
        {
            GameObject achievementPrefab = Instantiate(prefab);
            achievementPrefab.transform.SetParent(trans);
            achievementsPrefabs[index] = achievementPrefab;
            if (index == 0)
            {
                GameObject cursorPrefab = Instantiate(prefabCursor, Vector3.zero, Quaternion.identity, achievementPrefab.transform);
                CursorAchiGM.GetComponent<CursorWork>().cursor = cursorPrefab.transform;
            }

            Button button = achievementPrefab.GetComponent<Button>();
            CursorAchiGM.GetComponent<CursorWork>().buttons[index] = button;

            if (button != null)
            {
                button.onClick.AddListener(() => SetAchievementInfo(achievement));
            }

            AchiContainer ac = achievementPrefab.GetComponent<AchiContainer>();
            ac.icon.sprite = achievement.Icon;
            ac.achievement = achievement;

            index++;
        }

        SetAchievementInfo(achievementsDictionary.Values.GetEnumerator().Current);
    }

    public void SetAchievementInfo(Achievement achievement)
    {
        if (achievement != null)
        {
            descriptionText.text = achievement.GetDesc();
            nameText.text = achievement.title;
            if(achievement.isUnlocked)
            {
                icon.sprite = achievement.Icon;
            }
            else
            {
                icon.sprite = notUnlock;
            }
        }
    }
}