using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShowStats : MonoBehaviour
{
    public bool isWork;
    public TMP_Text[] textStat;
    public GameObject iconGM;

    void Update()
    {
        if(iconGM.activeSelf == true)
        {
            Set();
        }
    }
    void Awake()
    {
        Set();
    }
    public void Set()
    {
        for(int i = 0; i < textStat.Length;i++)
        {
            textStat[i].text = GameManager.Instance.buff[i] + "%";
        }
    }
}
