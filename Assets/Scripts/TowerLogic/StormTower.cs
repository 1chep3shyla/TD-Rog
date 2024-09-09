using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormTower : MonoBehaviour
{
    public float boostDuration = 5f; // Duration of the attack speed boost
    public float boostInterval = 10f; // Interval between boosts
    public float boostMultiplier = 1.5f; // Amount to multiply the attack speed by during the boost
    public GameObject particle;

    private Default tower;
    private float nextBoostTime;

    private void Start()
    {
        tower = GetComponent<Default>();
        nextBoostTime = Time.time + boostInterval;
    }

    private void Update()
    {
        if (Time.time >= nextBoostTime)
        {
            BoostAttackSpeed();
            nextBoostTime = Time.time + boostInterval;
        }
    }

    private void BoostAttackSpeed()
    {
        tower.charge = true;
        particle.SetActive(true);
        tower.attackSpeed /= boostMultiplier;
        StartCoroutine(RemoveAttackSpeedBoost());
    }

    private IEnumerator RemoveAttackSpeedBoost()
    {
        yield return new WaitForSeconds(boostDuration);
        particle.SetActive(false);
        tower.charge = false;
        tower.attackSpeed *= boostMultiplier;
    }
}