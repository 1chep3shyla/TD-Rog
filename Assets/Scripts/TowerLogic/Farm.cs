using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Farm : MonoBehaviour
{
    public int goldGive;
    public float timeNeed;
    private float curTime;
    public TMP_Text waveCount;
    public Animator addCoin;
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
        curTime -= Time.deltaTime;
        if (curTime <= 0f)
        {
            StartCoroutine(GiveGold());
        }
    }
    private IEnumerator GiveGold()
    {
        int givemoney = (int)((float)goldGive * GameManager.Instance.buff[4] / 100);
        int all = goldGive + givemoney;
        GameManager.Instance.AddMoney(all);
        curTime = timeNeed;
        addCoin.gameObject.SetActive(true);
        addCoin.Play("give_gold_anim");
        waveCount.text = all.ToString("");
        yield return new WaitForSeconds(0.5f);
        addCoin.gameObject.SetActive(false);
    }
}
