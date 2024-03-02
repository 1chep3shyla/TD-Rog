using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BloodyAltar : MonoBehaviour
{
    public int goldGive;
    public int healthRemove;
    public Spawner spawner;

    void Start()
    {
        spawner.waveStart += Use;
    }

    private void OnDestroy()
    {
        spawner.waveStart += Use;
    }

    public void Use()
    {
        StartCoroutine("UseCor");
    }
    public IEnumerator UseCor()
    {
        Debug.Log("Вызывается");
        if(GameManager.Instance.Health - healthRemove >= 1)
        {
            GameManager.Instance.Health -= healthRemove;
            GameManager.Instance.AddMoney(goldGive);
            GameManager.Instance.TakeDamagePlayer();
        }

        yield return null; 
    }
}