using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpHave : MonoBehaviour
{
    public int id;
    public Sprite iconCard;
    public GameObject UpVersion;
    public TowerBase baseOf;
    public string name;
    public float curAttackSpeed;
    public float attackSpeed;
    public int damage;
    public int curDamage;
    public int LVL;
    public int critChance;
    public int critChanceCur;
    private bool isBoostedAttack = false;
    private bool isBoosterSpeed;
    private bool isBoosterCrit;
    private float timeOff;
    public DataTower towerDataCur;
    public GameObject cursor;
    public GameObject cursorDelete;
    public bool Imposter;
    public bool Muted;

    void Update()
    {
        damage = (int)towerDataCur.lvlData[LVL, 1];
        attackSpeed = towerDataCur.lvlData[LVL, 3];
        critChance = (int)(towerDataCur.lvlData[LVL, 18] + GameManager.Instance.buff[8]);
        timeOff += Time.deltaTime;
        if (timeOff >= 0.5f)
        {
            timeOff = 0;
            RemoveDamageBoost();
            RemoveSpeedBoost();
            RemoveCritBoost();
        }
    }
    public void Clear()
    {
        Imposter = false;
        Muted = false;
    }
    public void ImposterEffect()
    {
        Imposter = true;
        if (gameObject.GetComponent<Default>() != null)
        {
            gameObject.GetComponent<Default>().UpdateImposter();
        }
    }
    public void MutedEffect()
    {
        Muted = true;
    }
    public void Cursoring()
    {
        Rolling rb = baseOf.rollBase;
        UpHave uh = rb.towerPrefab.GetComponent<UpHave>();
        if (uh.id == id && uh.LVL == LVL && uh.gameObject != gameObject && cursorDelete == null)
        {
            GameObject newCursor = Instantiate(cursor, transform.position , Quaternion.identity);
            newCursor.transform.position = new Vector3(newCursor.transform.position.x, newCursor.transform.position.y + 0.05f, newCursor.transform.position.z);
            cursorDelete = newCursor;
        }
        else if (uh.id == id && uh.LVL == LVL && uh.gameObject != gameObject && cursorDelete != null)
        {
            Destroy(cursorDelete);
            GameObject newCursor = Instantiate(cursor, transform.position, Quaternion.identity);
            newCursor.transform.position = new Vector3(newCursor.transform.position.x, newCursor.transform.position.y + 0.05f, newCursor.transform.position.z);
            cursorDelete = newCursor;
        }
    }
    public void DeleteCursor()
    {
        if (cursorDelete != null)
        {
            Destroy(cursorDelete); 
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

    public void ApplyCritBoost(float upSpeed)
    {
        isBoosterCrit = true;
        critChanceCur = (int)(critChance + (float)critChance * upSpeed);
    }

    public void RemoveCritBoost()
    {
        isBoosterCrit = true;
        critChanceCur = critChance;
    }
}
