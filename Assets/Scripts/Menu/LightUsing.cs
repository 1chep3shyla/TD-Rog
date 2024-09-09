using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LightUsing : MonoBehaviour
{
      public Light targetLight; // Свет, которым мы будем управлять
    public float duration = 2f; // Длительность изменения интенсивности
    public GameObject parentObject; // Родительский объект, дочерние объекты которого будем включать/выключать
    public Image timeIndicator; // Показатель времени
    public Sprite[] sunSprites; // Массив спрайтов для солнца
    public Sprite[] moonSprites; // Массив спрайтов для луны
    public Image celestialBodyImage; // Изображение для смены спрайтов солнца и луны
    [Space]
    public AudioClip onSFX;
    public AudioClip offSFX;

    void Start()
    {
        if (targetLight == null)
        {
            targetLight = GetComponent<Light>();
        }

        if (timeIndicator != null)
        {
            timeIndicator.fillAmount = 0;
        }
    }

    // Функция для увеличения интенсивности света от 0 до 1
    public void IncreaseIntensity(float duration)
    {
        StartCoroutine(ChangeIntensity(0, 0.75f, duration, true));
        parentObject = GameObject.Find($"{GameManager.Instance.states[GameBack.Instance.indexState].name}/lights");
        StartCoroutine(ChangeSprites(sunSprites, 12));
    }

    // Функция для уменьшения интенсивности света от 1 до 0
    public void DecreaseIntensity(float duration)
    {
        StartCoroutine(ChangeIntensity(0.75f, 0, duration, false));
        parentObject = GameObject.Find($"{GameManager.Instance.states[GameBack.Instance.indexState].name}/lights");
        StartCoroutine(ChangeSprites(moonSprites, 50f));
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
                    //yield return new WaitForSeconds(0.12f);
                
                }
                GameManager.Instance.aS.PlayOneShot(offSFX);
                GameManager.Instance.gameObject.GetComponent<FieldCleaner>().StartCleaning();
            }
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newIntensity = Mathf.Lerp(from, to, elapsedTime / duration);
            targetLight.intensity = newIntensity;

            if (timeIndicator != null)
            {
                timeIndicator.fillAmount = elapsedTime / duration;
            }

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
                    GameManager.Instance.aS.PlayOneShot(onSFX);
                    yield return new WaitForSeconds(0.12f);
                }
            }
        }
    }

    // Корутин для смены спрайтов
    private IEnumerator ChangeSprites(Sprite[] sprites, float totalDuration)
    {
        if (sprites.Length == 0 || celestialBodyImage == null) yield break;

        float spriteChangeInterval = totalDuration / sprites.Length;
        int spriteIndex = 0;

        while (spriteIndex < sprites.Length)
        {
            celestialBodyImage.sprite = sprites[spriteIndex];
            spriteIndex++;
            yield return new WaitForSeconds(spriteChangeInterval);
        }
    }
}
