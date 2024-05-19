using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UpPerkGlobal : MonoBehaviour
{
    public GameObject objectToSpawn; 
    public Transform parent;
    public Vector3[] initialPositions;
    public Vector3[] addPositions; 
    public Sprite[] sprites;
    public int baseCount;
    
    private int currentPass = 0; 
    
    private void Start()
    {
        StartCoroutine(SpawnObjects());
    }

    private IEnumerator SpawnObjects()
    {
        while (baseCount > currentPass)
        {
            for(int i =0 ; i < initialPositions.Length;i++)
            {
                GameObject butNew = Instantiate(objectToSpawn, new Vector3(0,0,0), Quaternion.identity);
                butNew.transform.GetChild(0).GetComponent<Image>().sprite = sprites[i];
                butNew.transform.SetParent(parent);
                butNew.transform.localScale = new Vector3(1,1,1);
                butNew.transform.localPosition = initialPositions[i];
                initialPositions[i] +=addPositions[i];

            }
            currentPass++;
            yield return new WaitForSeconds(0.1f);
        }
    }
}