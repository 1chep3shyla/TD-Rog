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
        Vector3 startPosition = waypoints[currentWaypoint].transform.position;
        Vector3 endPosition = waypoints[currentWaypoint + 1].transform.position;
        // 2 
        float pathLength = Vector3.Distance(startPosition, endPosition);

        float totalTimeForPath = pathLength / maxSpeed;
        float currentTimeOnPath = Time.time - lastWaypointSwitchTime;
        gameObject.transform.position = Vector2.Lerp(startPosition, endPosition, currentTimeOnPath / totalTimeForPath);
        

        if (gameObject.transform.position.Equals(endPosition))
        {
            if (currentWaypoint < waypoints.Length - 2)
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
    public void Slow(float slowPower, float time)
    {

        float powering = maxSpeed - slowPower;
        if (powering < speed)
        {
            speed = powering;
            slowTimer = slowPower;
        }
        else
        {
            slowTimer = slowPower;
        }
        isSlowed = true;
    }
}
