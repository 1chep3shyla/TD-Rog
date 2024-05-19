using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ColorChanger : MonoBehaviour
{
   // Начальный и конечный цвет
    public Color startColor = Color.red;
    public Color endColor = Color.blue;
    
    // Время перехода в секундах
    public float duration = 3.0f;
    
    // Интервал обновления
    public float updateInterval = 0.1f;

    // Компонент, который будет менять цвет
    private TMP_Text objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<TMP_Text>();
        StartCoroutine(ColorTransitionCoroutine());
    }

    IEnumerator ColorTransitionCoroutine()
    {
        bool movingTowardsEnd = true; // направление перехода
        float timeElapsed = 0f;

        while (true) // бесконечная корутина
        {
            // Вычисляем процент времени прошедшего от общей длительности
            float t = timeElapsed / duration;
            
            // Выбираем направление
            Color currentColor = movingTowardsEnd 
                ? Color.Lerp(startColor, endColor, t) 
                : Color.Lerp(endColor, startColor, t);

            // Применяем цвет к объекту
            if (objectRenderer != null)
            {
                objectRenderer.color = currentColor;
            }
            
            // Ждем следующий интервал
            yield return new WaitForSeconds(updateInterval);
            
            // Увеличиваем время
            timeElapsed += updateInterval;

            // Если время достигло или превысило длительность, меняем направление
            if (timeElapsed >= duration)
            {
                timeElapsed = 0f; // сброс времени
                movingTowardsEnd = !movingTowardsEnd; // меняем направление
            }
        }
    }
}