using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeChanger : MonoBehaviour
{
    public AudioSource musicAs;
    public AudioSource sfxAs;
    public Slider musicSlider;
    public Slider sfxSlider;
    public bool gameIs;
    void Start()
    {
        if(!gameIs)
        {
            ChangeVolume();
        }
        else
        {
            ChangeVolumeGame();
        }

    }

    // Update is called once per frame
    public void ChangeVolume()
    {
        GameBack.Instance.volumeMusic = musicSlider.value;
        GameBack.Instance.volumeSFX = sfxSlider.value;
        musicAs.volume = GameBack.Instance.volumeMusic;
        sfxAs.volume = GameBack.Instance.volumeSFX/1.6f;
    }

    public void ChangeVolumeGame()
    {
        musicSlider.value = GameBack.Instance.volumeMusic;
        sfxSlider.value = GameBack.Instance.volumeSFX;
    }
}
