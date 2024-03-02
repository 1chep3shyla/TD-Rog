using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpirit : MonoBehaviour
{
    public float freezeDuration = 3f;
    public float slowPower;
    public GameObject psDef;
    public GameObject slowGM;
    public ParticleSystem psDie;
    private bool isStan;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if(!isStan)
            {
                StartCoroutine(FreezeEnemies(collision.gameObject));
                animator.SetTrigger("Die");
            }
            else
            {
                EnemyMoving otherEnemyMoving = collision.gameObject.GetComponent<EnemyMoving>();
                otherEnemyMoving.Slow(freezeDuration, slowPower);
            }
        }
    }

    IEnumerator FreezeEnemies(GameObject enemy)
    {
        psDie.Play();
        psDef.SetActive(false);
        EnemyMoving enemyMoving = enemy.GetComponent<EnemyMoving>();
        if (enemyMoving != null)
        {
            enemyMoving.Stun(2f);
            isStan = true;

            Destroy(gameObject, 5f);
            yield return new WaitForSeconds(0.32f);
            slowGM.SetActive(true);
            slowGM.transform.parent = null;
        }
        yield return null;
    }
}