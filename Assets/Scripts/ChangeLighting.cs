using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLighting : MonoBehaviour
{
     public Light targetLight; // Свет, которым мы будем управлять
    public float duration = 2f; // Длительность изменения

    // Начальные и конечные значения для range и intensity
    public float initialRange = 10f;
    public float finalRange = 20f;
    public float initialIntensity = 0.5f;
    public float finalIntensity = 1f;

    void Start()
    {
        if (targetLight == null)
        {
            targetLight = GetComponent<Light>();
        }

        // Устанавливаем начальные значения
        targetLight.range = initialRange;
        targetLight.intensity = initialIntensity;

        // Запускаем корутину для изменения range и intensity
        StartCoroutine(ChangeLightProperties(initialRange, finalRange, initialIntensity, finalIntensity, duration));
    }

    // Корутин для плавного изменения range и intensity света
    private IEnumerator ChangeLightProperties(float startRange, float endRange, float startIntensity, float endIntensity, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Линейная интерполяция для range и intensity
            targetLight.range = Mathf.Lerp(startRange, endRange, t);
            targetLight.intensity = Mathf.Lerp(startIntensity, endIntensity, t);

            yield return null;
        }

        // Устанавливаем конечные значения для точности
        targetLight.range = endRange;
        targetLight.intensity = endIntensity;
    }
}