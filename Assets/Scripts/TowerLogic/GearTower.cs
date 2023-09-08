using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearTower : MonoBehaviour
{
    public string targetTag = "YourTargetTag"; // The tag to search for
    public float searchRadius = 5f; // The radius to search within

    private int objectCount = 0;

    private void Update()
    {
        // Get all GameObjects with the specified tag in the scene
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);

        // Iterate through the tagged objects and count the ones within the search radius
        objectCount = 0;
        foreach (var taggedObject in taggedObjects)
        {
            if (Vector2.Distance(transform.position, taggedObject.transform.position) <= searchRadius)
            {
                GearTower gearTower = taggedObject.GetComponent<GearTower>();
                if (gearTower != null)
                {
                    objectCount++;
                }
            }
        }

        if (objectCount > 1)
        {
            gameObject.GetComponent<UpHave>().curDamage = gameObject.GetComponent<UpHave>().damage * objectCount;
            gameObject.GetComponent<UpHave>().curAttackSpeed = gameObject.GetComponent<UpHave>().attackSpeed / (objectCount / 2);
        }

    }

    void OnDrawGizmosSelected()
    {
        // Visualize the attack radius using Gizmos
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
