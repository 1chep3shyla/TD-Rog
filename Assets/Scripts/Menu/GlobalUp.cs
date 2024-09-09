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
    public float onWhichUp;
    public bool bought;
    public GlobalUp previosUp;
     public GlobalUp[] allUpNeed = new GlobalUp[1]; // Инициализация массива
    public float[] buffGlobaly = new float[9];
    public float[] newBuffGlobaly = new float[15];
    public Character charAllow;

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
            for(int i =0; i < GameBack.Instance.secondsBuff.Length;i++)
            {
                GameBack.Instance.secondsBuff[i] += newBuffGlobaly[i];
            }
            if(charAllow !=null)
            {
                charAllow.isHave = true;
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
            //for(int i =0; i < GameBack.Instance.secondsBuff.Length;i++)
            //{
                //GameBack.Instance.secondsBuff[i] += newBuffGlobaly[i];
            //}
            if(charAllow !=null)
            {
                charAllow.isHave = true;
            }
            Debug.Log("ДЕБАЖИИИМ СУКА");
    }
  private void OnValidate()
    {
        // Этот метод вызывается при изменении значений в инспекторе
        // Убедимся, что 0 элемент всегда будет previosUp
        if (previosUp != null)
        {
            allUpNeed[0] = previosUp;
        }
    }
}
