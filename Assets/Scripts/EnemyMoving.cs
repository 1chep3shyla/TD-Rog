using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    public GameObject[] waypoints;
    private int currentWaypoint;
    private float lastWaypointSwitchTime;
    public float speed = 1.0f;
    public float maxSpeed = 1.0f;
    public float slowTimer;
    public bool isSlowed;

    void Start()
    {
        lastWaypointSwitchTime = Time.time;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, waypoints[currentWaypoint].transform.position);

        float step = speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypoint].transform.position, step);

        if (distance < step)
        {
            if (currentWaypoint < waypoints.Length - 1)
            {
                // 3.a 
                currentWaypoint++;
                lastWaypointSwitchTime = Time.time;
            }
            else
            {
                // 3.b 
                Destroy(gameObject);

                // TODO: вычитать здоровье
            }
        }

        if (isSlowed)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0)
            {
                 gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                isSlowed = false;
                speed = maxSpeed;
            }
        }

     

    }
    public void Slow(float time, float slowPower)
    {

        float powering = maxSpeed - slowPower;
        if (powering < speed)
        {
            speed = powering;
            slowTimer = time;
        }
        else
        {
            slowTimer = time;
        }
        isSlowed = true;
    }
}
