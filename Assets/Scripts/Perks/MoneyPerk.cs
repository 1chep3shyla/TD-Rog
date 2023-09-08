using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMoneyPerk", menuName = "Perks/MoneyPerk")]
public class MoneyPerk : ScriptableObject, IPerk
{
    public float buffMoney;
    public string name;
    public void ApplyPerk()
    {
        GameManager.Instance.buff[4] += buffMoney;
    }
    public string SetData()
    {
        return name;
    }
}