using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetTowerIcon : MonoBehaviour
{
    public GameObject imagePrefab; // префаб Image, который будет создаваться
    public Transform trans;
    void RemoveOldTowerImages()
    {
        foreach (Transform child in trans)
        {
            Destroy(child.gameObject);
        }
    }
    public void CreateTowerImages()
    {
         RemoveOldTowerImages();
        GameObject[] towerPull = GameBack.Instance.charData.SetGameObject();

        for (int i = 0; i < towerPull.Length; i++)
        {
            GameObject imageObject = Instantiate(new GameObject("NewEmptyObject"), trans);
            Image imageComponent = imageObject.AddComponent<Image>();

            if (imageComponent != null && i < towerPull.Length)
            {
                imageComponent.sprite = towerPull[i].GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                Debug.LogError("Image component or towerPull item is missing!");
            }
        }
    }
}