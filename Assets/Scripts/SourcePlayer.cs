using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourcePlayer : MonoBehaviour
{
    public AudioClip clip;
    public void Start()
    {
        GameManager.Instance.aS.PlayOneShot(clip);
        GameManager.Instance.aS.pitch = Random.Range(0.8f, 1.1f);
    }
}