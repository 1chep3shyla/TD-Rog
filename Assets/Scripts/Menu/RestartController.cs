using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class RestartController : MonoBehaviour
{
    public Button button;
    void Update()
    {
        string filePath = Application.persistentDataPath + "/levelData.dat";
        if (File.Exists(filePath))
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
}
