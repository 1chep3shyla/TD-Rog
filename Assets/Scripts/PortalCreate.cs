using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCreate : MonoBehaviour
{
    public Transform[] points; // Массив с точками
public GameObject objectPrefab; // Префаб объекта, который нужно создать между точками
private GameObject inPortal;
public int numberOfObjects = 1; // Количество объектов для создания
public bool repeatBetweenPoints = true; // Флаг для повторения объектов между теми же точками
public float distancePercentage = 0.5f; // Процентное расстояние между точками (от 0 до 1)
public float delayBetweenGenerations = 1f; // Задержка между созданием объектов
public bool repeatFunction = true; // Флаг для повторения всей функции
public float delayBetweenRepeats = 5f; // Задержка между применением повторов

void Start()
{
    StartCoroutine(GenerateObjects());
}

IEnumerator GenerateObjects()
{
     while (true)
        {
            if (points.Length < 2 || objectPrefab == null || numberOfObjects <= 0)
            {
                Debug.LogError("Необходимо установить хотя бы две точки, указать префаб объекта и установить количество объектов больше 0.");
                yield break;
            }

            int randomIndex = Random.Range(0, points.Length - 1);
            Transform startPoint = points[randomIndex];
            Transform endPoint = points[randomIndex + 1];

            float totalDistance = Vector3.Distance(startPoint.position, endPoint.position);
            float stepDistance = totalDistance * distancePercentage;

            for (int i = 0; i < numberOfObjects; i++)
            {
                float t = repeatFunction ? (float)i / numberOfObjects : Random.Range(0f, 1f);
                Vector3 randomPosition = Vector3.Lerp(startPoint.position, endPoint.position, t);
                Vector3 offset = (endPoint.position - startPoint.position).normalized * stepDistance * (float)i / numberOfObjects;
                randomPosition += offset;

                GameObject newObj = Instantiate(objectPrefab, randomPosition, Quaternion.identity);

                if (i == 0)
                {
                    newObj.AddComponent<PortalScript>();
                    inPortal = newObj;
                }
                else
                {
                    inPortal.GetComponent<PortalScript>().exitPortal = newObj.transform;
                }

                yield return new WaitForSeconds(delayBetweenGenerations);
            }

            if (!repeatFunction)
                yield break;

            yield return new WaitForSeconds(delayBetweenRepeats);
        }
    }
}