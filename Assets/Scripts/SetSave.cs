using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSave : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartNewGame()
    {
        GameBack.Instance.saveThis = false;
    }

    public void SaveLoad()
    {
        GameBack.Instance.saveThis = true;
    }
}
