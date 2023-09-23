using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Achi", menuName = "Achievement")]
public class Achievement : ScriptableObject
{
    public string id;
    public string title;
    public string description;
    public bool isUnlocked;
}
