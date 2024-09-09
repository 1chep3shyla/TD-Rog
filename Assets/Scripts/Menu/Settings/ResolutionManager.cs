using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResolutionManager : MonoBehaviour
{
 public TMP_Dropdown aspectRatioDropdown;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown refreshRateDropdown;
    public TMP_Dropdown screenModeDropdown;

    // Массив разрешений экрана
    Resolution[] resolutions;
    List<Resolution> filteredResolutions;

    // Популярные разрешения для каждого соотношения сторон
    Dictionary<string, List<Vector2Int>> popularResolutions = new Dictionary<string, List<Vector2Int>>()
    {
        { "4:3", new List<Vector2Int> { new Vector2Int(1024, 768), new Vector2Int(1280, 960), new Vector2Int(1600, 1200) } },
        { "16:9", new List<Vector2Int> { new Vector2Int(1280, 720), new Vector2Int(1920, 1080), new Vector2Int(2560, 1440), new Vector2Int(3840, 2160) } },
        { "16:10", new List<Vector2Int> { new Vector2Int(1280, 800), new Vector2Int(1440, 900), new Vector2Int(1680, 1050), new Vector2Int(1920, 1200) } }
    };

    void Start()
    {
        // Получаем доступ к разрешениям экрана
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();

        // Очищаем списки
        aspectRatioDropdown.ClearOptions();
        resolutionDropdown.ClearOptions();
        refreshRateDropdown.ClearOptions();
        screenModeDropdown.ClearOptions();

        // Создаем списки опций для соотношений сторон и режимов экрана
        List<string> aspectRatioOptions = new List<string> { "4:3", "16:9", "16:10" };
        List<string> screenModeOptions = new List<string> { "Windowed", "Fullscreen", "Fullscreen Window" };

        // Добавляем опции в выпадающие списки
        aspectRatioDropdown.AddOptions(aspectRatioOptions);
        screenModeDropdown.AddOptions(screenModeOptions);

        // Устанавливаем текущие значения по умолчанию
        aspectRatioDropdown.value = 1; // 16:9 по умолчанию
        screenModeDropdown.value = GetCurrentScreenModeIndex();

        aspectRatioDropdown.RefreshShownValue();
        screenModeDropdown.RefreshShownValue();

        UpdateResolutions();
    }

    void UpdateResolutions()
    {
        // Получаем текущее выбранное соотношение сторон
        string selectedAspectRatio = aspectRatioDropdown.options[aspectRatioDropdown.value].text;

        // Очищаем список разрешений
        resolutionDropdown.ClearOptions();

        // Фильтруем и добавляем только популярные разрешения для выбранного соотношения сторон
        List<string> resolutionOptions = new List<string>();
        filteredResolutions.Clear();
        foreach (Resolution resolution in resolutions)
        {
            foreach (Vector2Int popularResolution in popularResolutions[selectedAspectRatio])
            {
                if (resolution.width == popularResolution.x && resolution.height == popularResolution.y)
                {
                    string option = resolution.width + " x " + resolution.height;
                    if (!resolutionOptions.Contains(option))
                    {
                        resolutionOptions.Add(option);
                        filteredResolutions.Add(resolution);
                    }
                }
            }
        }

        // Добавляем опции в выпадающий список разрешений
        resolutionDropdown.AddOptions(resolutionOptions);

        // Устанавливаем текущее разрешение
        resolutionDropdown.value = GetCurrentResolutionIndex();
        resolutionDropdown.RefreshShownValue();

        UpdateRefreshRates();
    }

    void UpdateRefreshRates()
    {
        // Очищаем список частот обновления
        refreshRateDropdown.ClearOptions();

        // Создаем список опций для частот обновления
        List<string> refreshRateOptions = new List<string>();
        Resolution currentResolution = filteredResolutions[resolutionDropdown.value];
        foreach (Resolution resolution in resolutions)
        {
            if (resolution.width == currentResolution.width && resolution.height == currentResolution.height)
            {
                string refreshRateOption = resolution.refreshRate + " Hz";
                if (!refreshRateOptions.Contains(refreshRateOption))
                {
                    refreshRateOptions.Add(refreshRateOption);
                }
            }
        }

        // Добавляем опции в выпадающий список частот обновления
        refreshRateDropdown.AddOptions(refreshRateOptions);

        // Устанавливаем текущую частоту обновления
        refreshRateDropdown.value = GetCurrentRefreshRateIndex();
        refreshRateDropdown.RefreshShownValue();
    }

    int GetCurrentResolutionIndex()
    {
        Resolution currentResolution = Screen.currentResolution;

        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            if (filteredResolutions[i].width == currentResolution.width &&
                filteredResolutions[i].height == currentResolution.height)
            {
                return i;
            }
        }

        return 0; // Если текущее разрешение не найдено, возвращаем первое разрешение в списке
    }

    int GetCurrentRefreshRateIndex()
    {
        int currentRefreshRate = Screen.currentResolution.refreshRate;

        for (int i = 0; i < refreshRateDropdown.options.Count; i++)
        {
            if (int.Parse(refreshRateDropdown.options[i].text.Replace(" Hz", "")) == currentRefreshRate)
            {
                return i;
            }
        }

        return 0; // Если текущая частота обновления не найдена, возвращаем первую частоту в списке
    }

    int GetCurrentScreenModeIndex()
    {
        if (Screen.fullScreenMode == FullScreenMode.Windowed)
            return 0;
        else if (Screen.fullScreenMode == FullScreenMode.FullScreenWindow)
            return 2;
        else
            return 1;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        UpdateRefreshRates();
    }

    public void SetRefreshRate(int refreshRateIndex)
    {
        int refreshRate = int.Parse(refreshRateDropdown.options[refreshRateIndex].text.Replace(" Hz", ""));
        Resolution currentResolution = filteredResolutions[resolutionDropdown.value];
        Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen, refreshRate);
    }

    public void SetScreenMode(int screenModeIndex)
    {
        FullScreenMode screenMode = FullScreenMode.Windowed;

        if (screenModeIndex == 1)
            screenMode = FullScreenMode.ExclusiveFullScreen;
        else if (screenModeIndex == 2)
            screenMode = FullScreenMode.FullScreenWindow;

        Screen.fullScreenMode = screenMode;
    }

    public void OnAspectRatioChanged()
    {
        UpdateResolutions();
    }
}