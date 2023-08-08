using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public Rolling rollBase;
    private GameObject monster;
    

    private bool CanPlaceMonster()
    {
        return monster == null;
    }

    void OnMouseUp()
    {
        //2
        if (CanPlaceMonster() && rollBase.towerPrefab != null)
        {
            //3
            monster = (GameObject)
              Instantiate(rollBase.towerPrefab, transform.position, Quaternion.identity);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            rollBase.towerPrefab = null;
        }
    }
}
