using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllHpGet : MonoBehaviour
{
    public GameObject particles;
    void Start()
    {
        StartingGet();
    }
    public void StartingGet()
    {
        StartCoroutine("Getiing");
    }
    public IEnumerator Getiing()
    {
        for(int i = 0; i < GameManager.Instance.enemiesAll.Count; i++)
        {
            Enemy enemy = GameManager.Instance.enemiesAll[i].GetComponent<Enemy>();
            enemy.Death();
            GetComponent<Enemy>().maxHealth += enemy.health;
            GetComponent<Enemy>().health += enemy.health;
            GameObject particle = Instantiate(particles, enemy.gameObject.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.25f);
        }
        yield return null;
    }
}
