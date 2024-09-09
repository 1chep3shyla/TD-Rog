using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GlobalSee : MonoBehaviour
{
 public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text costText;
    public TMP_Text soulText;
    [Space]
    public Button buyBut;
    public Button[] buttonUps;
    [Space]
    public Image iconImage;
    public GameObject linePrefab; // Префаб линии (Slider)
    public Transform centralPoint; // Центральный объект, от которого создаются линии
    public Transform lineParent; // Родитель для линий (если требуется)

    public List<SkillButton> skillButtons; // Список кнопок навыков

    private List<Slider> sliders = new List<Slider>(); // Список для хранения слайдеров

    [Space]
    public GlobalUp[] allUps;
    public GlobalUp allUpsCur;
    public int curIndex;

    void Start()
    {
        InitializeSkillButtons();
        UpdateFillAmounts();

        for (int i = 0; i < allUps.Length; i++)
        {
            allUps[i].bought = GameBack.Instance.boughtStat[i];
            if (allUps[i].bought)
            {
                allUps[i].ApplyBuffSave();
            }
        }
    }

    void Update()
    {
        soulText.text = GameBack.Instance.souls.ToString("");
        //UpdateFillAmounts();
    }
    public void Cheating()
    {
        GameBack.Instance.souls +=1000;
    }

    void InitializeSkillButtons()
    {
        sliders.Clear(); // Очистить список слайдеров перед созданием новых

        // Создаем линии от центрального объекта к основным кнопкам навыков
        foreach (var skillButton in skillButtons)
        {

            CreateLinesRecursively(skillButton);

            if (IsMainSkillButton(skillButton))
            {
                CreateLineFromCentral(skillButton);
            }
        }
    }

    bool IsMainSkillButton(SkillButton skillButton)
    {
        // Определите логику для проверки, является ли кнопка основной
        // Например, это может быть основано на какой-то свойства кнопки или списка
        return skillButton.isMain; // Добавьте соответствующее поле в SkillButton
    }

    void CreateLineFromCentral(SkillButton skillButton)
    {
        Slider slider = Instantiate(linePrefab, lineParent).GetComponent<Slider>();
        skillButton.slider = slider;
        skillButton.slider.fillRect.GetComponent<Image>().color = skillButton.sliderColor;

        GameObject particles = Instantiate(skillButton.particle, slider.transform.GetChild(1).GetChild(0));
        particles.transform.SetParent(slider.transform.GetChild(1).GetChild(0).GetChild(0), false);
        RectTransform rectTransform = particles.GetComponent<RectTransform>();
        particles.name = "Particle System (Clone)";
        if (rectTransform != null)
        {
            rectTransform.anchorMin = new Vector2(1, 0.5f); // Правая середина
            rectTransform.anchorMax = new Vector2(1, 0.5f); // Правая середина
            rectTransform.pivot = new Vector2(1, 0.5f);     // Пивот также в правой середине
            rectTransform.anchoredPosition = Vector2.zero;  // Сброс позиции
        }
        CreateLine(centralPoint, skillButton.button.transform, slider);
    }

    void CreateLinesRecursively(SkillButton skillButton)
    {
        foreach (var connection in skillButton.connections)
        {
            Slider slider = Instantiate(linePrefab, lineParent).GetComponent<Slider>();
            connection.connectedButton.slider = slider;
            connection.connectedButton.slider.fillRect.GetComponent<Image>().color = skillButton.sliderColor;
            connection.connectedButton.sliderColor = skillButton.sliderColor;
            connection.connectedButton.particle = skillButton.particle;
            GameObject particles = Instantiate(skillButton.particle, connection.connectedButton.slider.transform.GetChild(1).GetChild(0));
            particles.transform.SetParent(slider.transform.GetChild(1).GetChild(0).GetChild(0), false);
            particles.name = "Particle System (Clone)";
            RectTransform rectTransform = particles.GetComponent<RectTransform>();
             if (rectTransform != null)
            {
                rectTransform.anchorMin = new Vector2(1, 0.5f); // Правая середина
                rectTransform.anchorMax = new Vector2(1, 0.5f); // Правая середина
                rectTransform.pivot = new Vector2(1, 0.5f);     // Пивот также в правой середине
                rectTransform.anchoredPosition = Vector2.zero;  // Сброс позиции
            }
            CreateLine(skillButton.button.transform, connection.connectedButton.button.transform, slider);
            CreateLinesRecursively(connection.connectedButton);
        }
    }

    void CreateLine(Transform start, Transform end, Slider slider)
    {
        Vector3 startPos = start.position;
        Vector3 endPos = end.position;

        RectTransform rectTransform = slider.GetComponent<RectTransform>();
        rectTransform.position = (startPos + endPos) / 2;

        // Задаем фиксированные размеры
        rectTransform.sizeDelta = new Vector2(380, 15);

        Vector3 direction = (endPos - startPos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rectTransform.rotation = Quaternion.Euler(0, 0, angle);

        // Устанавливаем начальное значение слайдера
        slider.value = 0;
    }

    public void SetData(int index)
    {
        UpdateFillAmounts();
        allUpsCur = allUps[index];
        curIndex = index;
        nameText.text = allUps[index].name;
        iconImage.sprite = allUps[index].icon;
        descriptionText.text = string.Format(allUps[index].description, allUps[index].onWhichUp);
        if (allUps[index].bought)
        {
            buyBut.gameObject.SetActive(false);
            costText.text = "Bought";
        }
        else
        {
            bool allPreviousBought = true;

            foreach (GlobalUp up in allUps[index].allUpNeed)
            {
                if (up != null && !up.bought)
                {
                    allPreviousBought = false;
                    break; // Если находим хотя бы один не купленный Up, выходим из цикла
                }
            }

            if (allPreviousBought)
            {
                buyBut.gameObject.SetActive(true);
                costText.text = allUps[index].cost.ToString("");
            }
            else
            {
                buyBut.gameObject.SetActive(false);
                costText.text = "Need Up";
            }
        }
    }

    public void Buy()
    {
        if (GameBack.Instance.souls >= allUpsCur.cost && !allUpsCur.bought)
        {
            allUpsCur.ApplyBuff(curIndex);
            costText.text = "Bought";
            buyBut.gameObject.SetActive(false);
        }
        UpdateFillAmounts();
    }

    public void UpdateFillAmounts()
    {
        foreach (var skillButton in skillButtons)
        {
            UpdateSkillButtonAndConnections(skillButton);
            StartCoroutine(AnimateUp(skillButton));
        }
        
    }
    IEnumerator AnimateUp(SkillButton skillButton)
    {
            if (skillButton.whichIsLvl.bought)
            {
                if(!skillButton.slider.GetComponent<Animator>().GetBool("WasSet"))
                {
                    skillButton.slider.GetComponent<Animator>().SetTrigger("Set");
                    yield return new WaitForSeconds(1f);
                    skillButton.button.GetComponent<Image>().color = skillButton.sliderColor;
                    yield return new WaitForSeconds(0.15f);
                }
            }
        foreach (var connection in skillButton.connections)
        {
            StartCoroutine(AnimateUp(connection.connectedButton));
        }
    }
    
    void UpdateSkillButtonAndConnections(SkillButton skillButton)
    {
        // Проверка, куплены ли все предыдущие апгрейды
        bool allPreviousBought = true;
        
        if (skillButton.whichIsLvl.allUpNeed != null)
        {
            foreach (GlobalUp up in skillButton.whichIsLvl.allUpNeed)
            {
                if (up != null && !up.bought)
                {
                    allPreviousBought = false;
                    break; // Если находим хотя бы один не купленный апгрейд, выходим из цикла
                }
            }
        }

        // Обновляем цвет кнопки и слайдера в зависимости от купленных апгрейдов
        if (skillButton.whichIsLvl.bought)
        {
            // skillButton.slider.GetComponent<Animator>().SetBool("WasSet", true);
        }
        else
        {
            skillButton.button.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1);
        }

        // Обновление цвета первого дочернего элемента кнопки
        if (allPreviousBought)
        {
            skillButton.button.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            skillButton.button.transform.GetChild(0).GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1);
        }

        // Рекурсивно обновляем значение слайдера для всех связанных кнопок
        foreach (var connection in skillButton.connections)
        {
            UpdateSkillButtonAndConnections(connection.connectedButton);
        }
    }

}

[System.Serializable]
public class SkillButton
{
    public Button button; // Кнопка навыка
    public GlobalUp whichIsLvl;
    public List<SkillConnection> connections; // Список связей с другими кнопками
    public Slider slider;
    public Color sliderColor;
    public GameObject particle;
    public bool isMain; // Определяет, является ли кнопка основной

    public SkillButton(Button button, bool isMain = false)
    {
        this.button = button;
        this.isMain = isMain;
        connections = new List<SkillConnection>();
    }
}

[System.Serializable]
public class SkillConnection
{
    public SkillButton connectedButton; // Кнопка, с которой есть связь
}