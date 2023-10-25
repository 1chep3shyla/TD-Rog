using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;


public class RollingCasino : MonoBehaviour
{
    public Transform[] slotReels;  // Массив с барабанами
    public float[] spinSpeed; // Скорость вращения барабанов
    public float[] spinTimeRow;
    public float spinDuration = 3.0f;  // Длительность вращения
    public AnimationCurve spinCurve;  // Кривая анимации для плавности вращения
    public float Pos;
    public float controlPos;
    private bool isSpinning = false;

    // Изображения символов для каждого барабана
    private List<Image> symbolsReel1 = new List<Image>();
    private List<Image> symbolsReel2 = new List<Image>();
    private List<Image> symbolsReel3 = new List<Image>();
    public Transform[] allRow;

    public Sprite[] allSymbols;

    void Start()
    {
        for(int i = 0; i < allRow.Length; i++)
        {
            FindAllChildImages(allRow[i]);
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isSpinning)
        {
            StartCoroutine(SpinReels());
        }
    }
    IEnumerator SpinReels()
    {
        // Время вращения для каждого барабана (можно настроить)

        for (int j = 0; j < slotReels.Length; j++)
        {
            slotReels[j].localPosition = new Vector3(slotReels[j].localPosition.x, Pos, slotReels[j].localPosition.z);
        }

        isSpinning = true;

        List<Coroutine> spinCoroutines = new List<Coroutine>();

        for (int i = 0; i < slotReels.Length; i++)
        {
            float spinTime = spinTimeRow[i];
            spinCoroutines.Add(StartCoroutine(SpinReels(spinTime, i)));
        }

        // Дождитесь завершения всех корутин вращения
        foreach (var coroutine in spinCoroutines)
        {
            yield return coroutine;
        }

        // Окончание вращения
        isSpinning = false;
        UpdateSymbols(); // Обновляем символы на барабанах
    }

    IEnumerator SpinReels(float spinTime, int i)
    {
        float startTime = Time.time;

        while (Time.time - startTime < spinTime)
        {
            float t = (Time.time - startTime) / spinTime;
            float moveAmount = spinCurve.Evaluate(t) * spinSpeed[i] * Time.deltaTime;

            // Перемещаем барабан вверх по оси Y (или другой, если необходимо)
            slotReels[i].Translate(new Vector3(0, moveAmount, 0));
            yield return null;
        }

        // Выравниваем позицию барабана на окончательную высоту (Pos)
        slotReels[i].localPosition = new Vector3(slotReels[i].localPosition.x, controlPos, slotReels[i].localPosition.z);
    }
    private void FindAllChildImages(Transform parent)
    {
        // Перебираем все дочерние объекты
        foreach (Transform child in parent)
        {
            // Получаем компонент Image, если он есть
            Image image = child.GetComponent<Image>();
            if (image != null)
            {
                // Если компонент Image найден, добавляем его в список
                symbolsReel1.Add(image);
            }

            // Рекурсивно вызываем функцию для поиска внутренних дочерних объектов
            FindAllChildImages(child);
        }
    }
    void UpdateSymbols()
    {
        // Здесь вы можете реализовать логику обновления символов на барабанах.
        // Выберите случайные символы из массивов symbolsReel1, symbolsReel2 и symbolsReel3
        // и установите их на соответствующих барабанах.
    }
}