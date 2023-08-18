using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : MonoBehaviour
{
    public float boostRadius = 5f;
    public float boostMultiplierSpeed = 1.5f; // Damage multiplier for boosted towers

    private void Update()
    {
        // Find towers within the boost radius and apply damage boost
        Collider2D[] towers = Physics2D.OverlapCircleAll(transform.position, boostRadius);
        foreach (Collider2D towerCollider in towers)
        {
            if (towerCollider.CompareTag("Tower"))
            {
                UpHave tower = towerCollider.GetComponent<UpHave>();
                if (tower != null)
                {
                    tower.ApplyAttackBoost(boostMultiplierSpeed);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the boost radius using Gizmos
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, boostRadius);
    }
}