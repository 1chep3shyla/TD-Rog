using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deleter : MonoBehaviour
{
    private float timeDelete;
    public float maxTime;
    void Update()
    {
        timeDelete += Time.deltaTime;
        if (timeDelete >= maxTime)
        {
            Destroy(gameObject);
        }
    }
}
