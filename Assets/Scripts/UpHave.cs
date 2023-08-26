using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpHave : MonoBehaviour
{
    public int id;
    public GameObject UpVersion;
    public TowerBase baseOf;
    public string name;
    public float curAttackSpeed;
    public float attackSpeed;
    public int damage;
    public int curDamage;
    public int LVL;
    private bool isBoostedAttack = false;
    private bool isBoosterSpeed;
    private float timeOff;

    void Update()
    {

        timeOff += Time.deltaTime;
        if (timeOff >= 0.5f)
        {
            timeOff = 0;
            RemoveDamageBoost();
            RemoveSpeedBoost();
        }
    }
    private void Start()
    {
        curDamage = damage;
        curAttackSpeed = attackSpeed;
    }

    public void ApplyDamageBoost(float multiplier)
    {
        isBoostedAttack = true;
        curDamage = Mathf.RoundToInt(damage * multiplier);
    }

    public void RemoveDamageBoost()
    {
        isBoostedAttack = false;
        curDamage = damage;
    }

    public void ApplyAttackBoost(float upSpeed)
    {
        isBoosterSpeed = true;
        curAttackSpeed = attackSpeed - (upSpeed * attackSpeed);
    }

    public void RemoveSpeedBoost()
    {
        isBoosterSpeed = false;
        curAttackSpeed = attackSpeed;
    }
}
