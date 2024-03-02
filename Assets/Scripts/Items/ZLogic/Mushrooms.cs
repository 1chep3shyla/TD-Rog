using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushrooms : MonoBehaviour
{
    public float duration = 3f;
    public int damage;
    public float timeToDestory;
    private Animator animator;
    private float timer;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= timeToDestory)
        {
            StartCoroutine(Death());
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy otherEnemyMoving = collision.gameObject.GetComponent<Enemy>();
            otherEnemyMoving.SetPoison(duration, damage);
        }
    }

    IEnumerator PoisonEnemies(GameObject enemy)
    {
        Enemy otherEnemyMoving = enemy.GetComponent<Enemy>();
        otherEnemyMoving.SetPoison(duration, (int)((float)damage* GameManager.Instance.curWave * 1.2f));
        yield return null;
    }
    IEnumerator Death()
    {
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}