using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpiderScript : MonoBehaviour
{
    public float duration;
    public GameObject EnemyWhichSpawn;
    public Sprite eggSprite;
    public int countSpawn;
    public bool work;
    private EnemyMoving enemyMoving; 

    void Start()
    {
        StartCoroutine("Spidering");
        enemyMoving = GetComponent<EnemyMoving>();
        //GetComponent<Enemy>().Death.AddListener(OnDestroy);
    }
    public IEnumerator Spidering()
    {
        yield return new WaitForSeconds(Random.Range(7f,12f));
        GetComponent<Animator>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = eggSprite;
        enemyMoving.Stun(duration);
        work = true;
        yield return new WaitForSeconds(duration);
        GetComponent<Animator>().enabled = true;
        work = false;
    }
    private void OnDestroy()
    {
        if(work)
        {
            Debug.Log("Работает призыв");
            for(int i = 0; i < countSpawn; i++)
            {
                Debug.Log("Спавнит" + i);
                GameManager.Instance.spawn.StartCoroutine(GameManager.Instance.spawn.SpawnInPos(EnemyWhichSpawn, transform.position += new Vector3(Random.Range(-0.25f,0.25f),Random.Range(-0.25f,0.25f),0f),
                enemyMoving.currentWaypoint, enemyMoving.waypoints));
            }
        }
    }
}
