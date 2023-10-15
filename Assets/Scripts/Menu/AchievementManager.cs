using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    // ������ ���� ��������� ����������
    public List<Achievement> achievements;
    public GameObject prefab;
    public Transform trans;

    private void Awake()
    {
        for (int i = 0; i < achievements.Count; i++)
        {
            // ������� ������
            GameObject achievementPrefab = Instantiate(prefab);

            // ������������� ��������
            achievementPrefab.transform.SetParent(trans);

            AchiContainer ac = achievementPrefab.GetComponent<AchiContainer>();
            ac.icon.sprite = achievements[i].Icon;
            ac.discription.text = achievements[i].description;
            ac.name.text = achievements[i].title;
            ac.achievement = achievements[i];
        }
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void UnlockAchievement(string achievementID)
    {
        Achievement achievement = achievements.Find(a => a.id == achievementID);
        if (achievement != null && !achievement.isUnlocked)
        {
            achievement.isUnlocked = true;
            Debug.Log("���������� ��������������: " + achievement.title);

            // ����� ����� �������� ��� ��� ����������� ����������� ��� �������
        }
    }
}