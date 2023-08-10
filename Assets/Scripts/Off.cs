using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Off : MonoBehaviour
{
    public Rolling roll;

     void OnMouseDown()
    {
        roll.choosing = false;
    }
}
