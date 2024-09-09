using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeScaleMoving : MonoBehaviour
{
    public Button[] buttons; // Массив кнопок
    public float[] timeScales; // Массив значений TimeScale

    private float previousTimeScale;

    void Start()
    {
        previousTimeScale = Time.timeScale;
        UpdateButtons();
    }

    void Update()
    {
        if (Time.timeScale != previousTimeScale)
        {
            previousTimeScale = Time.timeScale;
            UpdateButtons();
        }
    }

    void UpdateButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (Mathf.Approximately(Time.timeScale, timeScales[i]))
            {
                buttons[i].interactable = false;
            }
            else
            {
                buttons[i].interactable = true;
            }
        }
    }
}