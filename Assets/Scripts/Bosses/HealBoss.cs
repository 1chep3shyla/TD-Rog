using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBoss : MonoBehaviour
{
    public float timeHeal;
    public float percent;
    private float timeNeed;

    void Start()
    {
        timeNeed = timeHeal;
    }
    void Update()
    {
        timeNeed -= Time.deltaTime;
        if( timeNeed <=0)
        {
            float healing = GetComponent<Enemy>().maxHealth * percent;
            
            if(GetComponent<Enemy>().health + healing >  GetComponent<Enemy>().maxHealth)
            {
                GetComponent<Enemy>().health = GetComponent<Enemy>().maxHealth;
            }
            else
            {
                GetComponent<Enemy>().health += (int)healing;
            }
            timeNeed = timeHeal;
        }
    }
}
