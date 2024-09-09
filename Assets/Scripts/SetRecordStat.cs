using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class SetRecordStat : MonoBehaviour
{
    public TMP_Text[] records;

    void Start()
    {
        for(int i = 0; i < records.Length;i++)
        {
            records[i].text = $"{GameBack.Instance.waveInStage[i]}/30";
        }
    }
}
