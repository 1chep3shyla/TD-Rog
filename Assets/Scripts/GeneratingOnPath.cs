using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratingOnPath : MonoBehaviour
{
    public Transform[] points; // Массив с точками
    public GameObject objectPrefab; // Префаб объекта, который нужно создать между точками
    public int numberOfObjects = 1; // Количество объектов для создания
    public bool repeatBetweenPoints = true; // Флаг для повторения объектов между теми же точками
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
            if(!repeatBetweenPoints)
            {
                for (int i = 0; i < numberOfObjects; i++)
                {
                    // Выбор двух случайных соседних точек
                    int randomIndex = Random.Range(0, points.Length - 1);
                    Transform startPoint = points[randomIndex];
                    Transform endPoint = points[randomIndex + 1];

                    // Создание объекта в случайной точке между двумя выбранными точками
                    float t = repeatBetweenPoints ? (float)i / numberOfObjects : Random.Range(0f, 1f);
                    Vector3 randomPosition = Vector3.Lerp(startPoint.position, endPoint.position, t);
                    GameObject newObj = Instantiate(objectPrefab, randomPosition, Quaternion.identity);

                    yield return new WaitForSeconds(delayBetweenGenerations);
                }
            }
            else
            {
                int randomIndex = Random.Range(0, points.Length - 1);
                Transform startPoint = points[randomIndex];
                Transform endPoint = points[randomIndex + 1];
                for (int i = 0; i < numberOfObjects; i++)
                {

                    float t = repeatBetweenPoints ? (float)i / numberOfObjects : Random.Range(0f, 1f);
                    Vector3 randomPosition = Vector3.Lerp(startPoint.position, endPoint.position, t);
                    GameObject newObj = Instantiate(objectPrefab, randomPosition, Quaternion.identity);

                    yield return new WaitForSeconds(delayBetweenGenerations);
                }
            }

            if (!repeatFunction)
                yield break;

            yield return new WaitForSeconds(delayBetweenRepeats);
        }
    }
}