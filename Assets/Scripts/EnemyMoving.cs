using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    boss,
    elite,
    defaultEnemy,
    flying,
    fast,
    reduction
}

public class EnemyMoving : MonoBehaviour
{
    public EnemyType typeEnemy;
    public Transform[] waypoints;
    public int currentWaypoint;
    private float lastWaypointSwitchTime;
    public float speed = 1.0f;
    public float maxSpeed = 1.0f;
    public float slowTimer;
    public bool isSlowed;
    private int damageEnemy;
    private bool isStunned;
    private float stunDuration;
    void Start()
    {
        lastWaypointSwitchTime = Time.time;
        if (typeEnemy == EnemyType.defaultEnemy || typeEnemy == EnemyType.flying || typeEnemy == EnemyType.fast || typeEnemy == EnemyType.reduction)
        {
            damageEnemy = 1;
        }
        else if (typeEnemy == EnemyType.elite)
        {
            damageEnemy = 3;
        }
        else if (typeEnemy == EnemyType.boss)
        {
            damageEnemy = 5;
        }
        speed = maxSpeed;
    }

    void Update()
    {
        if (typeEnemy == EnemyType.flying)
        {
            currentWaypoint = waypoints.Length - 1;
        }
        if (!isStunned)
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
                  GameManager.Instance.TakeDamageHealth(damageEnemy);
                }
            }
        }
        if (isSlowed)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0)
            {
                isSlowed = false;
                speed = maxSpeed;
            }
        }
        
    }
    public void Slow(float time, float slowPower)
    {
        float powering = maxSpeed - (maxSpeed * ((slowPower + GameManager.Instance.buff[1]) / 100));
        if (powering < speed)
        {
            StartCoroutine(SlowCoroutine(time, slowPower,powering));
        }
    }

    private IEnumerator SlowCoroutine(float time, float slowPower, float powering)
    {
        if (powering < speed && powering > 0f)
        {
            speed = powering;
            slowTimer = time;
        }
        else if (powering < speed && powering < 0f)
        {
            speed = 0.1f;
            slowTimer = time;
        }
        else if (powering > speed)
        {
            slowTimer = time;
        }
        isSlowed = true;

        yield return new WaitForSeconds(time);

    }

    public void Stun(float duration)
    {
        if (!isStunned)
        {
            isStunned = true;
            stunDuration = duration;
            StartCoroutine(Stunned());
        }
    }

    private IEnumerator Stunned()
    {
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }
    public void Portaling(int chance)
    {
        int random = Random.Range(0, 100);
        if (random < chance)
        {
            currentWaypoint = 0;
            transform.position = waypoints[0].transform.position;
        }

    }
    private void OnDestroy()
    {
        GameManager.Instance.RemoveEnemyFromList(gameObject);
    }
}
