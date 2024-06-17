using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeChanger : MonoBehaviour
{
   public AudioSource[] musicAudioSources; // Массив для музыкальных аудио
    public AudioSource[] sfxAudioSources;   // Массив для звуковых эффектов аудио
    public Slider musicSlider;              // Слайдер для управления громкостью музыки
    public Slider sfxSlider;                // Слайдер для управления громкостью звуковых эффектов
    public bool gameIs;                     // Флаг, указывающий, является ли игра активной

    void Start()
    {
        if (!gameIs)
        {
            ChangeVolume();
        }
        else
        {
            ChangeVolumeGame();
        }
    }

    // Метод для изменения громкости звуков
    public void ChangeVolume()
    {
        float musicVolume = musicSlider.value / 1.6f;
        float sfxVolume = sfxSlider.value / 2.5f;

        // Установка громкости для каждого AudioSource в массиве
        foreach (AudioSource audioSource in musicAudioSources)
        {
            if (audioSource != null)
            {
                audioSource.volume = musicVolume;
            }
        }

        foreach (AudioSource audioSource in sfxAudioSources)
        {
            if (audioSource != null)
            {
                audioSource.volume = sfxVolume;
            }
        }
    }

    // Метод для установки громкости в игре
    public void ChangeVolumeGame()
    {
        musicSlider.value = GameBack.Instance.volumeMusic * 1.6f;
        sfxSlider.value = GameBack.Instance.volumeSFX * 2.5f;
        ChangeVolume();
    }
}