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
    
    void Start()
    {
        curTime = timeNeed;
    }
    void Update()
    {
        curTime -= Time.deltaTime;
        if (curTime <= 0f)
        {
            StartCoroutine(GiveGold());
        }
    }
    private IEnumerator GiveGold()
    {
        GameManager.Instance.AddMoney(goldGive);
        curTime = timeNeed;
        addCoin.gameObject.SetActive(true);
        addCoin.Play("give_gold_anim");
        waveCount.text = goldGive.ToString("");
        yield return new WaitForSeconds(0.5f);
        addCoin.gameObject.SetActive(false);
    }
}
