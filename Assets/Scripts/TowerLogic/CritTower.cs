using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritTower : MonoBehaviour
{
    public float boostRadius = 5f;
    public float boostMultiplierCrit = 1.5f; // Damage multiplier for boosted towers

    private void Update()
    {
        // Find towers with the "Tower" tag within the boost radius and apply damage boost
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        foreach (GameObject tower in towers)
        {
            Vector2 towerPosition = tower.transform.position;
            if (Vector2.Distance(transform.position, towerPosition) <= boostRadius)
            {
                UpHave towerScript = tower.GetComponent<UpHave>();
                if (towerScript != null)
                {
                    towerScript.ApplyCritBoost(boostMultiplierCrit);
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