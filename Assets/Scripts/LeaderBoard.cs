using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LeaderBoard : MonoBehaviour
{
    public TMP_Text leaderboardText; // ��������� ���� ��� ����������� ����������
    public GameObject leaderboardItemPrefab; // ������ �������� ����������
    public Transform leaderboardContainer; // ������������ ������ ��� ��������� ����������

    private bool leaderboardInitialized = false;

    void Start()
    {
        StartCoroutine(UpdateLeaderboardPeriodically());
    }

    private IEnumerator UpdateLeaderboardPeriodically()
    {
        while (true)
        {
            UpdateLeaderboard();
            yield return new WaitForSeconds(0.5f); // ���������� ������ 5 ������ (��� ����� ������ ������)
        }
    }

    private void UpdateLeaderboard()
    {
        // ���������� ������ �� �������� �����
        List<DMGTower> sortedTowerDamage = new List<DMGTower>(GameManager.Instance.allTowerDMG);
        sortedTowerDamage.Sort((a, b) => b.countDamage.CompareTo(a.countDamage));

        // �������� ���� ������������ ��������� ����������
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }

        // �������� ����� ��������� ���������� �� ������ ���������������� ������
        for (int i = 0; i < sortedTowerDamage.Count; i++)
        {
            DMGTower towerDamage = sortedTowerDamage[i];

            // ��������, ��� ���� �� ����� 0
            if (towerDamage.countDamage != 0)
            {
                // �������� ������ �������� ���������� �� �������
                GameObject leaderboardItem = Instantiate(leaderboardItemPrefab, leaderboardContainer);
                // ���������� ���������� �� ����� � ����� �����
                leaderboardItem.GetComponent<Board>().leaderboardText.text = $"{i + 1}. {towerDamage.name}: \n All:{towerDamage.countDamage} \n Round:{towerDamage.roundDamage}";
                leaderboardItem.GetComponent<Board>().icon.sprite = towerDamage.iconTower;
            }
        }
    }
}











