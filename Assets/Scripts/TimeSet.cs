using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TimeSet : MonoBehaviour
{
    public Slider timeSlider;
    public Spawner spawn;
    void Update()
    {
        spawn = GameManager.Instance.spawn;
        timeSlider.maxValue = spawn.timeBetweenWaves;
        timeSlider.value = spawn.timeCur;
    }
}
