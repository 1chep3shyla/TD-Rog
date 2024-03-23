using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResolutionManager : MonoBehaviour
{
      public TMP_Dropdown resolutionDropdown;

    // Массив разрешений экрана
    Resolution[] resolutions;

    void Start()
    {
        // Получаем доступ к разрешениям экрана
        resolutions = Screen.resolutions;

        // Очищаем список разрешений
        resolutionDropdown.ClearOptions();

        // Создаем список опций для разрешений
        List<string> options = new List<string>();

        // Заполняем список опциями для разрешений
        foreach (Resolution resolution in resolutions)
        {
            string option = resolution.width + " x " + resolution.height;
            options.Add(option);
        }

        // Добавляем опции в выпадающий список
        resolutionDropdown.AddOptions(options);

        // Устанавливаем текущее разрешение
        resolutionDropdown.value = GetCurrentResolutionIndex();
        resolutionDropdown.RefreshShownValue();
    }

    // Метод для получения индекса текущего разрешения
    int GetCurrentResolutionIndex()
    {
        Resolution currentResolution = Screen.currentResolution;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == currentResolution.width &&
                resolutions[i].height == currentResolution.height)
            {
                return i;
            }
        }

        return 0; // Если текущее разрешение не найдено, возвращаем первое разрешение в списке
    }

    // Метод вызывается при изменении выбора в выпадающем списке
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}