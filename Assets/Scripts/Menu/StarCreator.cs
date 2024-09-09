using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.Events;

public class StarCreator : MonoBehaviour
{
  public GameObject parentObject; // Ссылка на объект, откуда берутся дочерние объекты
    public GameObject[] objectToInstantiate; // Массив объектов, которые нужно создать для каждого дочернего объекта
    public Vector3 localPosition; // Заданная локальная позиция для создаваемого объекта

    public List<GameObject> createdObjects = new List<GameObject>(); // Список для хранения созданных объектов
    public List<ScriptableObject> scriptableObjects = new List<ScriptableObject>(); // Список для хранения ScriptableObject

    void Start()
    {
        DuplicateChildren();
    }

    public void DuplicateChildren()
    {
        if (parentObject == null || objectToInstantiate == null || objectToInstantiate.Length == 0)
        {
            Debug.LogError("Parent object or object to instantiate is not assigned or objectToInstantiate array is empty.");
            return;
        }

        createdObjects.Clear(); // Очищаем список перед добавлением новых объектов
        scriptableObjects.Clear(); // Очищаем список ScriptableObject

        foreach (Transform child in parentObject.transform)
        {
            // Получаем Button и проверяем OnClick события
            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                // Проверяем все слушатели OnClick
                for (int i = 0; i < button.onClick.GetPersistentEventCount(); i++)
                {
                    var target = button.onClick.GetPersistentTarget(i);
                    if (target is ScriptableObject scriptableObject)
                    {
                        scriptableObjects.Add(scriptableObject); // Добавляем ScriptableObject в список

                        // Проверяем тип ScriptableObject и существование объекта в массиве
                        GameObject newObject = null;
                        if (scriptableObject is Character character)
                        {
                            int index = 0;
                            if(character.lvlChar<=3)
                            {
                                 index = character.lvlChar;
                            }
                            else
                            {
                                 index =3;
                            }
                            if (index >= 0 && index < objectToInstantiate.Length)
                            {
                                GameObject prefab = objectToInstantiate[index];
                                if (prefab != null)
                                {
                                    newObject = Instantiate(prefab, child);
                                    newObject.transform.localPosition = localPosition;
                                    createdObjects.Add(newObject); // Добавляем новый объект в список
                                    Debug.Log($"Found Character ScriptableObject: {character}");
                                }
                                else
                                {
                                    Debug.LogWarning($"Prefab at index {index} is not assigned.");
                                }
                            }
                            else
                            {
                                Debug.LogError($"Invalid index for Character: {index}");
                            }
                        }
                        else if (scriptableObject is CharWithOutEvolve charWithOutEvolve)
                        {
                            int index = 0;
                            if(charWithOutEvolve.lvlChar<=3)
                            {
                                 index = charWithOutEvolve.lvlChar;
                            }
                            else
                            {
                                 index =3;
                            }
                            if (index >= 0 && index < objectToInstantiate.Length)
                            {
                                GameObject prefab = objectToInstantiate[index];
                                if (prefab != null)
                                {
                                    newObject = Instantiate(prefab, child);
                                    newObject.transform.localPosition = localPosition;
                                    createdObjects.Add(newObject); // Добавляем новый объект в список
                                    Debug.Log($"Found CharWithOutEvolve ScriptableObject: {charWithOutEvolve}");
                                }
                                else
                                {
                                    Debug.LogWarning($"Prefab at index {index} is not assigned.");
                                }
                            }
                            else
                            {
                                Debug.LogError($"Invalid index for CharWithOutEvolve: {index}");
                            }
                        }
                        else
                        {
                            // Обработка для других типов ScriptableObject
                            Debug.Log($"Found other ScriptableObject: {scriptableObject}");
                        }
                    }
                }
            }
        }
    }
}