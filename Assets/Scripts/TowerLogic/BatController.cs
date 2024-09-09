using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    public float orbitSpeed = 20f; // Speed of orbiting in degrees per second
    private Transform target; // Target around which to orbit
    private float orbitRadius = 5f; // Orbit radius
    private float orbitAngle = 0f; // Angle of orbit
    public int dmg;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetOrbitRadius(float radius)
    {
        orbitRadius = radius;
    }

    public void SetOrbitAngle(float angle)
    {
        orbitAngle = angle;
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }

        Vector3 targetPosition = target.position;
        Vector3 orbitPosition = targetPosition + new Vector3(Mathf.Cos(orbitAngle * Mathf.Deg2Rad), Mathf.Sin(orbitAngle * Mathf.Deg2Rad),-1f) * orbitRadius;

        transform.position = orbitPosition;
        orbitAngle += orbitSpeed *75 * Time.deltaTime;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().DefaultAttack(dmg, target.gameObject.GetComponent<UpHave>().critChance);
        }
    }
}