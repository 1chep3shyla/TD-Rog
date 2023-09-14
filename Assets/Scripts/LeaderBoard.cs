using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LeaderBoard : MonoBehaviour
{
    public TMP_Text leaderboardText; // Текстовое поле для отображения лидерборда
    public GameObject leaderboardItemPrefab; // Префаб элемента лидерборда
    public Transform leaderboardContainer; // Родительский объект для элементов лидерборда

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
            yield return new WaitForSeconds(0.5f); // Обновление каждые 5 секунд (или любой другой период)
        }
    }

    private void UpdateLeaderboard()
    {
        // Сортировка списка по убыванию урона
        List<DMGTower> sortedTowerDamage = new List<DMGTower>(GameManager.Instance.allTowerDMG);
        sortedTowerDamage.Sort((a, b) => b.countDamage.CompareTo(a.countDamage));

        // Удаление всех существующих элементов лидерборда
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }

        // Создание новых элементов лидерборда на основе отсортированного списка
        for (int i = 0; i < sortedTowerDamage.Count; i++)
        {
            DMGTower towerDamage = sortedTowerDamage[i];

            // Проверка, что урон не равен 0
            if (towerDamage.countDamage != 0)
            {
                // Создание нового элемента лидерборда из префаба
                GameObject leaderboardItem = Instantiate(leaderboardItemPrefab, leaderboardContainer);
                // Заполнение информации об уроне и имени башни
                leaderboardItem.GetComponent<Board>().leaderboardText.text = $"{i + 1}. {towerDamage.name}: \n All:{towerDamage.countDamage} \n Round:{towerDamage.roundDamage}";
                leaderboardItem.GetComponent<Board>().icon.sprite = towerDamage.iconTower;
            }
        }
    }
}











