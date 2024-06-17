using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewGlobalBuff", menuName = "GlobalBuff")]
public class GlobalUp : ScriptableObject
{
    public string name;
    [TextArea(1,2)]
    public string description;
    [Space]
    public Sprite icon;
    public int cost;
    public int onWhichUp;
    public bool bought;
    public float[] buffGlobaly = new float[9];

    public void ApplyBuff(int index)
    {
        if(GameBack.Instance.souls >= cost && !bought)
        {
            bought = true;
            GameBack.Instance.souls-= cost;
            for(int i =0; i < GameBack.Instance.buffGlobal.Length;i++)
            {
                GameBack.Instance.buffGlobal[i] += buffGlobaly[i];
            }
            GameBack.instance.boughtStat[index] = true;
            SaveManager.instance.SaveGameData();
        }
    }
    public void ApplyBuffSave()
    {
            for(int i =0; i < GameBack.Instance.buffGlobal.Length;i++)
            {
                GameBack.Instance.buffGlobal[i] += buffGlobaly[i];
            }
    }
}
