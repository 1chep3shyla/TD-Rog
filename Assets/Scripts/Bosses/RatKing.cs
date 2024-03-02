using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatKing : MonoBehaviour
{
    public float percent;
    public GameObject rats;
    private Enemy enemy;
    private EnemyMoving enemyMoving;
    private bool work;
    void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyMoving = GetComponent<EnemyMoving>();
    }

    void Update()
    {
        if((float)enemy.health / (float)enemy.maxHealth * 100 < percent && !work)
        {
            enemyMoving.Stun(15f);
            StartCoroutine(GameManager.Instance.spawn.SpawnGM(rats, 30));
            work = true;
        }
    }
}
