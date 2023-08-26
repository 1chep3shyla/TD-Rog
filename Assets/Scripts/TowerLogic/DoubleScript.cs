using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleScript : MonoBehaviour
{
    public bool work;
    private Rolling rollScript;
    public int[] pos;
    public int mulitpleBuff;
    public GameObject curGMBuff;
    void Start()
    {
        rollScript = GameManager.Instance.gameObject.GetComponent<Rolling>();
    }
    void Update()
    {
        if (work == false)
        {
            Choose();
        }
        else
        {
            if (rollScript.allBases[pos[0], pos[1]] == null)
            {
                work = false;
            }
            if (curGMBuff != rollScript.allBases[pos[0], pos[1]].curGM && rollScript.allBases[pos[0], pos[1]].curGM.GetComponent<Default>() != null)
            {
                rollScript.allBases[pos[0], pos[1]].curGM.GetComponent<Default>().maxTargets += mulitpleBuff;
            }

        }
    }
    void Choose()
    {
        int randomRow = Random.Range(0, 15);
        int randomCol = Random.Range(0, 7);
        if (rollScript.allBases[randomRow, randomCol] != null && rollScript.allBases[randomRow, randomCol].curGM.GetComponent<Default>() != null)
        {
            work = true;
            rollScript.allBases[randomRow, randomCol].curGM.GetComponent<Default>().maxTargets +=mulitpleBuff;
            curGMBuff = rollScript.allBases[randomRow, randomCol].curGM;
            pos[0] = randomRow;
            pos[1] = randomCol;
        }
    }
    public void OnDestroy()
    {
        rollScript.allBases[pos[0], pos[1]].curGM.GetComponent<Default>().maxTargets -= mulitpleBuff;
    }
}
