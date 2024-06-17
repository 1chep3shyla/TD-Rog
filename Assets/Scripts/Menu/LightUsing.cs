using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUsing : MonoBehaviour
{
     public Light targetLight; // Свет, которым мы будем управлять
    public float duration = 2f; // Длительность изменения интенсивности
    public GameObject parentObject; // Родительский объект, дочерние объекты которого будем включать/выключать

    void Start()
    {
        if (targetLight == null)
        {
            targetLight = GetComponent<Light>();
        }
    }

    // Функция для увеличения интенсивности света от 0 до 1
    public void IncreaseIntensity(float duration)
    {
        StartCoroutine(ChangeIntensity(0, 0.75f, duration, true));
        parentObject = GameObject.Find($"{GameManager.Instance.states[GameBack.Instance.indexState].name}/lights");
    }

    // Функция для уменьшения интенсивности света от 1 до 0
    public void DecreaseIntensity(float duration)
    {
        StartCoroutine(ChangeIntensity(0.75f, 0, duration, false));
        parentObject = GameObject.Find($"{GameManager.Instance.states[GameBack.Instance.indexState].name}/lights");

    }

    // Корутин для плавного изменения интенсивности света и управления дочерними объектами
    private IEnumerator ChangeIntensity(float from, float to, float duration, bool onOrOff)
    {
        float elapsedTime = 0;
         if (parentObject != null)
        {
            if (onOrOff)
            {
                foreach (Transform child in parentObject.transform)
                {
                    child.gameObject.SetActive(false);
                    yield return new WaitForSeconds(0.25f);
                }
            }
        }
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newIntensity = Mathf.Lerp(from, to, elapsedTime / duration);
            targetLight.intensity = newIntensity;
            yield return null;
        }
        targetLight.intensity = to;

        // Включение или выключение дочерних объектов с задержкой
        if (parentObject != null)
        {
            if (!onOrOff)
            {
                foreach (Transform child in parentObject.transform)
                {
                    child.gameObject.SetActive(true);
                    yield return new WaitForSeconds(0.35f);
                }
            }
        }
    }
}