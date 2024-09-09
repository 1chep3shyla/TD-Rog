using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
[CreateAssetMenu(fileName = "Achi", menuName = "Achievement")][System.Serializable]
public class Achievement : ScriptableObject
{
    public string id;
    public string title;
    public string description;
    public string localDiscription;
    public string nameData;
    public Sprite Icon;
    public bool isUnlocked;
    public ScriptableObject characterUnlock;
    
  public string GetDesc()
    {
        // Проверяем, существует ли переменная GameBack.Instance
            if(!string.IsNullOrEmpty(nameData))
            {
                var gameBackInstance = GameBack.Instance;
                if (gameBackInstance == null)
                {
                    Debug.Log("GameBack.Instance не инициализирован.");
                    return "Ошибка: GameBack.Instance не инициализирован.";
                }

                // Получаем значение переменной по ее имени
                var field = gameBackInstance.GetType().GetField(nameData);
                if (field == null)
                {
                    Debug.Log($"Переменная с именем {nameData} не найдена в GameBack.Instance.");
                    return $"Ошибка: Переменная {nameData} не найдена в GameBack.Instance.";
                }

                // Проверяем, является ли значение числом
                var fieldValue = field.GetValue(gameBackInstance);
                if (!(fieldValue is int || fieldValue is float || fieldValue is double))
                {
                    Debug.Log($"Переменная {nameData} в GameBack.Instance не является числом.");
                    return $"Ошибка: Переменная {nameData} в GameBack.Instance не является числом.";
                }

                // Возвращаем строку с описанием, подставив значение переменной
                return string.Format(description, fieldValue);
            }
            else
            {
                return description;
            }
    }
}