using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Farm : MonoBehaviour
{
    public int goldGive;
    public float timeNeed;
    private float curTime;
    public GameObject addCoin;
    private DataTower basa;
    private UpHave uh;
    
    void Start()
    {
        curTime = timeNeed;
        uh = gameObject.GetComponent<UpHave>();
        basa = uh.towerDataCur;
    }
    void Update()
    {
        goldGive = (int)basa.lvlData[uh.LVL, 17];
        if(GameManager.Instance.spawn.works)
        {
            curTime -= Time.deltaTime;
        }
        if (curTime <= 0f)
        {
            StartCoroutine(GiveGold());
        }
    }
    private IEnumerator GiveGold()
    {
        int givemoney = (int)((float)goldGive * GameManager.Instance.buff[4] / 100);
        int all = goldGive + givemoney;
        curTime = timeNeed;
        GameObject coin = Instantiate(addCoin, transform.position, Quaternion.identity);
        coin.GetComponent<GoldMoving>().gold = all;
        coin.transform.GetChild(0).GetComponent<TMP_Text>().text = all.ToString("");
        yield return null;
    }
}
