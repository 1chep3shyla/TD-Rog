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
    public float speed;
    public BatController[] allBats;
    private UpHave uh;

    void Update()
    {
        damage = uh.curDamage;
        speed = uh.attackSpeed;
        for (int i = 0; i < allBats.Length; i++)
        {
            allBats[i].orbitSpeed = gameObject.GetComponent<UpHave>().attackSpeed + gameObject.GetComponent<UpHave>().attackSpeed - gameObject.GetComponent<UpHave>().curAttackSpeed;
            allBats[i].dmg = gameObject.GetComponent<UpHave>().curDamage;
        }
    }
    private void Start()
    {
        uh = gameObject.GetComponent<UpHave>();
        CreateObjects();
    }

    private void CreateObjects()
    {
        allBats = new BatController[numberOfObjects];
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
                allBats[i] = batController;
                batController.SetTarget(orbitTarget);
                batController.SetOrbitRadius(orbitRadius);
                batController.SetOrbitAngle(angle); // Pass the angle to the BatController
                batController.dmg = damage;
                batController.orbitSpeed = speed;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, orbitRadius);
    }
}