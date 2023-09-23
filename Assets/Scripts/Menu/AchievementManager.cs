using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    // ������ ���� ��������� ����������
    public List<Achievement> achievements;

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