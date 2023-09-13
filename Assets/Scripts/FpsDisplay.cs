using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsDisplay : MonoBehaviour
{
    public TMP_Text textFPS;
    private float deltaTime = 0.0f;


    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        string fpsString = string.Format("{0:0.} FPS", fps);
        textFPS.text = fpsString;
    }
}
