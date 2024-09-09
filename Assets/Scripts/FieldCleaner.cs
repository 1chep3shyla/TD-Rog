using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCleaner : MonoBehaviour
{
 public GameObject[] itemsToClean;  // Array of items to be cleaned
    public GameObject cleanerPrefab;   // Prefab of the cleaner GameObject
    public float offsetX = 1.0f;       // Offset value for cleaner's spawn position

    private List<GameObject> cleaners = new List<GameObject>();

    // Function to start the cleaning process
    public void StartCleaning()
    {
        StartCoroutine(CleanerRoutineStarting());
    }

    private IEnumerator CleanerRoutineStarting()
    {
        int itemCount = itemsToClean.Length;
        int cleanerCount = Mathf.CeilToInt(itemCount / 10f); // Calculate the number of cleaners needed

        // Destroy existing cleaners if any
        foreach (GameObject cleaner in cleaners)
        {
            Destroy(cleaner);
        }
        cleaners.Clear();

        // Spawn new cleaners
        for (int i = 0; i < cleanerCount; i++)
        {
            // Determine random offset direction (-1 or 1)
            float randomOffsetDirection = Random.value > 0.5f ? 1f : -1f;
            float randomOffsetX = offsetX * randomOffsetDirection;

            GameObject cleaner = Instantiate(cleanerPrefab, new Vector3(randomOffsetX, 0, 0), Quaternion.identity);
            cleaners.Add(cleaner);

            // Set the cleaner's flipX based on the offset direction
            SpriteRenderer spriteRenderer = cleaner.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = randomOffsetDirection > 0;
            }

            StartCoroutine(CleanerRoutine(cleaner, i * 10, Mathf.Min((i + 1) * 10, itemCount)));
            yield return new WaitForSeconds(0.2f);
        }
         yield return new WaitForSeconds(10f);
        itemsToClean = new GameObject[0];
    }

    // Coroutine for each cleaner
    private IEnumerator CleanerRoutine(GameObject cleaner, int startIndex, int endIndex)
    {
        for (int i = startIndex; i < endIndex; i++)
        {
            if (itemsToClean[i] != null)
            {
                cleaner.transform.position = itemsToClean[i].transform.position;
                yield return new WaitForSeconds(0.48f);
                Destroy(itemsToClean[i]);
                itemsToClean[i] = null;  // Mark the item as cleaned
            }
        }

        // Destroy the cleaner after finishing the job
        Destroy(cleaner);
    }

    // Function to add new items to the array
    public void AddItem(GameObject newItem)
    {
        List<GameObject> itemList = new List<GameObject>(itemsToClean);
        itemList.Add(newItem);
        itemsToClean = itemList.ToArray();
    }
}