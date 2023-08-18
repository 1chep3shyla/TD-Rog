using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatTower : MonoBehaviour
{
    public GameObject objectPrefab; // The prefab for the object with BatController component
    public int numberOfObjects = 5; // Number of objects to create
    public Transform orbitTarget; // The target around which objects will orbit
    public float orbitRadius = 5f; // Radius of the orbit
    public int damage;

    void Update()
    {
        damage = gameObject.GetComponent<UpHave>().curDamage;
    }
    private void Start()
    {
        CreateObjects();
    }

    private void CreateObjects()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angleIncrement = 360f / numberOfObjects;
            float angle = i * angleIncrement;

            Vector3 offset = Quaternion.Euler(0f, 0f, angle) * Vector3.right * orbitRadius;
            Vector3 spawnPosition = transform.position + offset;

            GameObject newObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity, transform);
            BatController batController = newObject.GetComponent<BatController>();

            if (batController != null)
            {
                batController.SetTarget(orbitTarget);
                batController.SetOrbitRadius(orbitRadius);
                batController.SetOrbitAngle(angle); // Pass the angle to the BatController
                batController.dmg = damage;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, orbitRadius);
    }
}