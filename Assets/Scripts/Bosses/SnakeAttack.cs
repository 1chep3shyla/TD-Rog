using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAttack : MonoBehaviour
{
    public float timeToAttack;
    private float timeAttack;
    void Update()
    {
        timeAttack += Time.deltaTime;
        if(timeAttack >= timeToAttack)
        {
            GameManager.Instance.TakeDamageHealth(1);
            timeAttack = 0;
        }
    }
}
