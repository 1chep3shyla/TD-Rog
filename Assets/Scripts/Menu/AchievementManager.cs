using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;
    public List<Achievement> achievements;
    public TMP_Text descriptionText;
    public TMP_Text nameText;
    public Image icon;
    public GameObject prefab;
    public GameObject prefabCursor;
    public Transform trans;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Создаем ачивки
        CreateAchievements();
    }

    private void CreateAchievements()
    {
        GameObject CursorAchiGM = new GameObject("CursorAchi");
        CursorAchiGM.AddComponent<CursorWork>();
        CursorAchiGM.GetComponent<CursorWork>().buttons = new Button[achievements.Count];
        for (int i = 0; i < achievements.Count; i++)
        {
            GameObject achievementPrefab = Instantiate(prefab);
            achievementPrefab.transform.SetParent(trans);
            if(i == 0)
            {
                GameObject cursorPrefab = Instantiate(prefabCursor, new Vector3(0,0,0), Quaternion.identity, achievementPrefab.transform);
                CursorAchiGM.GetComponent<CursorWork>().cursor = cursorPrefab.transform;
            }

            // Получаем кнопку внутри префаба ачивки
            Button button = achievementPrefab.GetComponent<Button>();
            CursorAchiGM.GetComponent<CursorWork>().buttons[i] = button;
            if (button != null)
            {
                int index = i; // сохраняем индекс для использования внутри обратного вызова
                button.onClick.AddListener(() => SetAchi(index));
            }
            AchiContainer ac = achievementPrefab.GetComponent<AchiContainer>();
            ac.icon.sprite = achievements[i].Icon;
            ac.achievement = achievements[i];
        }
        SetAchi(0);
    }

    public void SetAchi(int index)
    {
        if (index >= 0 && index < achievements.Count)
        {
            Achievement achievement = achievements[index];
            descriptionText.text = achievement.description;
            nameText.text = achievement.title;
            icon.sprite = achievement.Icon;
        }
    }

    public void UnlockAchievement(string achievementID)
    {
        Achievement achievement = achievements.Find(a => a.id == achievementID);
        if (achievement != null && !achievement.isUnlocked)
        {
            achievement.isUnlocked = true;
            Debug.Log("Unlocked Achievement: " + achievement.title);
        }
    }
}

