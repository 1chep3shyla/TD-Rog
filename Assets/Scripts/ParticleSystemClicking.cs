using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemClicking : MonoBehaviour
{
     public ParticleSystem particleSystem;

    // Кулдаун между запусками ParticleSystem
    public float cooldown = 2f;

    // Время, когда ParticleSystem может быть снова запущена
    private float nextPlayTime = 0f;

    // Метод, который будет вызываться при клике по коллайдеру
    private void OnMouseDown()
    {
        if (particleSystem != null && Time.time >= nextPlayTime)
        {
            PlayParticles();
        }
    }

    // Метод для запуска ParticleSystem
    private void PlayParticles()
    {
        particleSystem.Play();
        // Обновляем время, когда ParticleSystem может быть снова запущена
        nextPlayTime = Time.time + cooldown;
    }
}