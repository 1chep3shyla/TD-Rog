using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject circlePrefab;
    public int rows = 4;
    public int columns = 30;
    public float distanceBetweenCirclesX = 1.5f;
    public float distanceBetweenCirclesY = 1.5f;
    public GameObject linePrefab;
    public GameObject[,] allMap = new GameObject[4, 30];
    public WaveSet[] allWaveSetting;
    public bool workCor;
    private bool all;
    public bool connect;
    private bool create;
    private bool connectAll;
    private bool makeAll;
    private List<GameObject> createdLines = new List<GameObject>(); // Store the created lines

    void Update()
    {
        if (all)
        {
            for (int i = 0; i < allWaveSetting.Length; i++)
            {
                if (CheckWave(i) != true)
                {
                    for (int col = 0; col < columns; col++)
                    {
                        SetWave(allMap[col, i], i, col);
                    }
                }
            }

        }
        if ( connectAll)
        {
            connectAll = false;
            CheckConnect();

        }
        if (connect && !create)
        {
            create = true;
            ConnectAdjacentObjects();
        }
        if (makeAll)
        {
            for (int i = 0; i < allWaveSetting.Length; i++)
            {

                for (int col = 0; col < columns; col++)
                {
                    if (allMap[col, i] != null)
                    {
                        if (allMap[col, i].GetComponent<Place>().name.text == "Пустота")
                        {

                            Destroy(allMap[col, i]);
                        }
                    }
                }

            }
        }
    }

    void Start()
    {
        GenerateCircles();
    }




    public void CheckConnect()
    {
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows - 1; row++)
            {

                if (allMap[col, row].GetComponent<Place>().havePath == false && row != 0 && !allMap[col, row - 1].GetComponent<Place>().name.text.Equals("Пустота") && !allMap[col, row].GetComponent<Place>().name.text.Equals("Пустота"))
                {
                    ConnectObjects(allMap[col, row - 1], allMap[col, row]);
                }
                else if (allMap[col, row].GetComponent<Place>().havePath == false && row != 0 && col == 0 && allMap[col, row -1 ].GetComponent<Place>().name.text.Equals("Пустота") &&  !allMap[col, row].GetComponent<Place>().name.text.Equals("Пустота"))
                {
                    ConnectObjectsBack(allMap[col +1, row - 1], allMap[col, row], 40f);
                }
                else if (allMap[col, row].GetComponent<Place>().havePath == false && col == 3 && row != 0 && allMap[col, row - 1].GetComponent<Place>().name.text.Equals("Пустота") &&  !allMap[col, row].GetComponent<Place>().name.text.Equals("Пустота"))
                {
                    ConnectObjectsBack(allMap[col - 1, row - 1], allMap[col, row], -40f);
                }
                else if (allMap[col, row].GetComponent<Place>().havePath == false && col == 1 && row != 0 && allMap[col, row - 1].GetComponent<Place>().name.text.Equals("Пустота") && !allMap[col, row].GetComponent<Place>().name.text.Equals("Пустота"))
                {
                    ConnectObjectsBack(allMap[col + 1, row - 1], allMap[col, row], 40f);
                }
                else if (allMap[col, row].GetComponent<Place>().havePath == false && col == 2 && row != 0 && allMap[col, row - 1].GetComponent<Place>().name.text.Equals("Пустота") && !allMap[col, row].GetComponent<Place>().name.text.Equals("Пустота"))
                {
                    ConnectObjectsBack(allMap[col - 1, row - 1], allMap[col, row], -40f);
                }
            }
        }
        makeAll = true;
    }

    

    void ConnectAdjacentObjects()
    {
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows - 1; row++)
            {
                GameObject currentObject = allMap[col, row];
                GameObject objectBelow = allMap[col, row + 1];
                int random = Random.Range(0, 100);
                if (col == 0)
                {
                    if (random < 70)
                    {
                        int newrandom = Random.Range(0, 1);
                        if (newrandom == 0)
                        {
                            if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота") && !objectBelow.GetComponent<Place>().name.text.Equals("Пустота"))
                            {
                                ConnectObjects(currentObject, objectBelow);
                            }
                            else if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота"))
                            {
                                ConnectObjectsBack(currentObject, allMap[col + 1, row + 1], -40f);
                            }

                        }
                        else if (newrandom == 1)
                        {
                            if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота") && !allMap[col + 1, row + 1].GetComponent<Place>().name.text.Equals("Пустота"))
                            {
                                ConnectObjectsBack(currentObject, allMap[col + 1, row + 1], -40f);
                            }
                            else if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота"))
                            {
                                ConnectObjects(currentObject, objectBelow);
                            }

                        }
                    }
                    else
                    {
                        if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота") && !objectBelow.GetComponent<Place>().name.text.Equals("Пустота"))
                        {
                            ConnectObjects(currentObject, objectBelow);
                        }
                        if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота") && !allMap[col + 1, row + 1].GetComponent<Place>().name.text.Equals("Пустота"))
                        {
                            ConnectObjectsBack(currentObject, allMap[col + 1, row + 1], -40f);
                        }

                    }
                }
                else if (col == 3)
                {
                    if (random < 70)
                    {
                        int newrandom = Random.Range(0, 2);
                        if (newrandom == 0)
                        {
                            if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота") && !objectBelow.GetComponent<Place>().name.text.Equals("Пустота"))
                            {
                                ConnectObjects(currentObject, objectBelow);
                            }
                            else if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота"))
                            {
                                ConnectObjectsBack(currentObject, allMap[col - 1, row + 1], 40f);
                            }

                        }
                        else if (newrandom == 1)
                        {
                            if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота") && !allMap[col - 1, row + 1].GetComponent<Place>().name.text.Equals("Пустота"))
                            {
                                ConnectObjectsBack(currentObject, allMap[col - 1, row + 1], 40f);
                            }
                            else if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота"))
                            {
                                ConnectObjects(currentObject, objectBelow);
                            }

                        }
                    }
                    else
                    {
                        if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота") && !objectBelow.GetComponent<Place>().name.text.Equals("Пустота"))
                        {
                            ConnectObjects(currentObject, objectBelow);
                        }
                        if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота") && !allMap[col - 1, row + 1].GetComponent<Place>().name.text.Equals("Пустота"))
                        {
                            ConnectObjectsBack(currentObject, allMap[col - 1, row + 1], 40f);
                        }

                    }
                }
                else
                {
                    int newrandom = Random.Range(0, 100);
                    if (newrandom < 65)
                    {
                        int newRandom = Random.Range(0, 2);
                        if (newRandom == 0)
                        {
                            if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота") && !objectBelow.GetComponent<Place>().name.text.Equals("Пустота"))
                            {
                                ConnectObjects(currentObject, objectBelow);
                            }
                            else if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота"))
                            {
                                ConnectObjectsBack(currentObject, allMap[col + 1, row + 1], -40f);
                            }

                        }
                        else if (newRandom == 1)
                        {
                            if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота") && !allMap[col + 1, row + 1].GetComponent<Place>().name.text.Equals("Пустота"))
                            {
                                ConnectObjectsBack(currentObject, allMap[col + 1, row + 1], -40f);
                            }
                            else if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота"))
                            {
                                ConnectObjects(currentObject, objectBelow);
                            }
                        }
                    }
                    else if (newrandom > 65 && newrandom < 90)
                    {
                        int newRandom = Random.Range(0, 2);
                        if (newRandom == 0)
                        {
                            if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота") && !objectBelow.GetComponent<Place>().name.text.Equals("Пустота"))
                            {
                                ConnectObjects(currentObject, objectBelow);
                            }
                            if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота") && !allMap[col + 1, row + 1].GetComponent<Place>().name.text.Equals("Пустота"))
                            {
                                ConnectObjectsBack(currentObject, allMap[col + 1, row + 1], -40f);
                            }


                        }
                        else if (newRandom == 1)
                        {
                            if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота") && !objectBelow.GetComponent<Place>().name.text.Equals("Пустота"))
                            {
                                ConnectObjects(currentObject, objectBelow);
                            }
                            if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота") && !allMap[col - 1, row + 1].GetComponent<Place>().name.text.Equals("Пустота"))
                            {
                                ConnectObjectsBack(currentObject, allMap[col - 1, row + 1], 40f);
                            }

                        }
                    }
                    else if (newrandom > 90)
                    {

                        if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота") && !objectBelow.GetComponent<Place>().name.text.Equals("Пустота"))
                        {
                            ConnectObjects(currentObject, objectBelow);
                        }
                        if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота") && !allMap[col + 1, row + 1].GetComponent<Place>().name.text.Equals("Пустота"))
                        {
                            ConnectObjectsBack(currentObject, allMap[col + 1, row + 1], -40f);
                        }
                        if (!currentObject.GetComponent<Place>().name.text.Equals("Пустота") && !allMap[col - 1, row + 1].GetComponent<Place>().name.text.Equals("Пустота"))
                        {
                            ConnectObjectsBack(currentObject, allMap[col - 1, row + 1], 40f);
                        }

                    }
                }

            }
        }
        connectAll = true;
    }

    void ConnectObjects(GameObject obj1, GameObject obj2)
    {

        obj2.GetComponent<Place>().havePath = true;
        Vector3 midPoint = (obj1.transform.position + obj2.transform.position) / 2;
        GameObject newLine = Instantiate(linePrefab, midPoint, Quaternion.identity);
        newLine.transform.parent = transform; // Parent the line to the container
        createdLines.Add(newLine); // Add the new line to the list
                                   // Attach the line to the objects or any other connection logic

    }

    void ConnectObjectsBack(GameObject obj1, GameObject obj2, float rotat)
    {
        if (!obj1.GetComponent<Place>().name.text.Equals("Пустота") && !obj2.GetComponent<Place>().name.text.Equals("Пустота"))
        {
            obj2.GetComponent<Place>().havePath = true;
            Vector3 midPoint = (obj1.transform.position + obj2.transform.position) / 2;
            Quaternion rotatePoint = Quaternion.Euler(0f, 0f, rotat);
            GameObject newLine = Instantiate(linePrefab, midPoint, rotatePoint);
            newLine.transform.parent = transform; // Parent the line to the container
            Vector3 lineScale = new Vector3(0.01317f, 0.46095f, 1.2634f);
            newLine.transform.localScale = lineScale;
            createdLines.Add(newLine); // Add the new line to the list
                                       // Attach the line to the objects or any other connection logic
        }

    }

    void GenerateCircles()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 circlePosition = new Vector3(col * distanceBetweenCirclesX, -row * distanceBetweenCirclesY, 0f);
                GameObject newCircle = Instantiate(circlePrefab, circlePosition, Quaternion.identity);
                newCircle.transform.parent = transform;

                allMap[col, row] = newCircle;
            }
        }
        GenerateType();
    }

    public void GenerateType()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                SetWave(allMap[col, row], row, col);
            }
        }
        all = true;
    }

    public void SetWave(GameObject waveCur, int index, int colIndex)
    {
        if (allWaveSetting.Length == 0)
        {
            Debug.LogWarning("No wave settings defined.");
            return;
        }

        WaveSet selectedWaveSet = allWaveSetting[index];
        string selectedWaveName = "";


        int random = Random.Range(0, 100);
        for (int i = 0; i < selectedWaveSet.chance.Length; i++)
        {

            if (random < selectedWaveSet.chance[i])
            {

                selectedWaveName = selectedWaveSet.nameOfWave[i];
                selectedWaveSet.curCount[i]++;
                waveCur.GetComponent<Place>().name.text = selectedWaveName;
                int curWaveCount = selectedWaveSet.curCount[i];
                int minWaveCount = selectedWaveSet.minCount[i];
                return;

            }

        }

    }
    public bool CheckWave(int waveIndex)
    {
        Debug.Log("Check");
        for (int i = 0; i < allWaveSetting[waveIndex].curCount.Length; i++)
        {
            if (allWaveSetting[waveIndex].curCount[i] < allWaveSetting[waveIndex].minCount[i] && allWaveSetting[waveIndex].curCount[i] > allWaveSetting[waveIndex].maxCount[i])
            {
                for (int o = 0; o < allWaveSetting[waveIndex].curCount.Length; o++)
                {
                    allWaveSetting[waveIndex].curCount[o] = 0;
                }
                return false;
            }
        }
        return true;
    }
}







[System.Serializable]
public class WaveSet
{
    public string[] nameOfWave;
    public int[] chance;
    public int[] minCount;
    public int[] maxCount;
    public int[] curCount;
}