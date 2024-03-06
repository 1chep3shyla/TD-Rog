using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cocon : MonoBehaviour
{
    public float timeToNeed;
    public Transform[] ways;
    private EnemyMoving enemyMov;
    void Start()
    {
        enemyMov = GetComponent<EnemyMoving>();
        StartCoroutine("SetButter");
    }
    public IEnumerator SetButter()
    {
        yield return new WaitForSeconds(5f);
        GetComponent<Animator>().SetTrigger("Spell");
        ways = new Transform[2];
        ways[0] = enemyMov.waypoints[0];
        ways[1] = enemyMov.waypoints[enemyMov.waypoints.Length - 1];
        enemyMov.waypoints = new Transform[2];
        enemyMov.waypoints = ways;
        enemyMov.currentWaypoint = 1;

    }
}
