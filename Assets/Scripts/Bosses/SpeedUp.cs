using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    public float reload;
    public float upSpeed;
    void Start()
    {
        StartCoroutine("SpeedUpping");
    }
    public IEnumerator SpeedUpping()
    {
        yield return new WaitForSeconds(reload/2);
        while(true)
        {
            GetComponent<EnemyMoving>().maxSpeed += upSpeed;
            GetComponent<EnemyMoving>().speed = GetComponent<EnemyMoving>().maxSpeed;
            yield return new WaitForSeconds(1.2f);
            GetComponent<EnemyMoving>().maxSpeed -= upSpeed;
            GetComponent<EnemyMoving>().speed = GetComponent<EnemyMoving>().maxSpeed;
            yield return new WaitForSeconds(reload);
        }
    }
}
