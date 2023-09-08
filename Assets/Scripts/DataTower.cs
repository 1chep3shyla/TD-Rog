using UnityEngine;

[CreateAssetMenu(fileName = "NewDataTower", menuName = "Custom/DataTower")]
[System.Serializable]
public class DataTower : ScriptableObject
{
    [SerializeField]
    public float[,] lvlData = new float[5,25];
}
