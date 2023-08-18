using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public bool gameOver = false;
    public int Gold;
    public SpriteRenderer[] allTower;
    public GameObject needMore;
    public TMP_Text goldCount;
    public TMP_Text waveCount;
    public int curWave;
    public int enemyHave;
    private bool anima;

    private static GameManager instance;
    void Update()
    {
        goldCount.text = Gold.ToString("");
        waveCount.text = curWave.ToString("") + "/10";

    }
    void Start()
    {
        curWave = 0;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int Wave
    {
        get
        {
            return curWave;
        }
        set
        {
            curWave = value;
            if (!gameOver)
            {

            }
        }
    }

    public void UpLay()
    {
        for (int i = 0; i < allTower.Length; i++)
        {
            if (allTower[i] != null)
            {
                allTower[i].sortingOrder = 4;
            }
        }

    }
    public void DownLay()
    {
        for (int i = 0; i < allTower.Length; i++)
        {
            if (allTower[i] != null)
            {
                allTower[i].sortingOrder = 3;
            }
        }
    }
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<GameManager>();
                    singletonObject.name = "GameManagerSingleton";
                }
            }
            return instance;
        }
    }

    public void notEnought()
    {
        if (!anima)
        {
            anima = true;
            StartCoroutine(needMoreActive());
        }
    }
    public IEnumerator needMoreActive()
    {
        needMore.SetActive(true);
        needMore.GetComponent<Animator>().Play("needMore_anim");
        yield return new WaitForSeconds(0.52f);
        anima = false;
        needMore.SetActive(false);
    }

}
