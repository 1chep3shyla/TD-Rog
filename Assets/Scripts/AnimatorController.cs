using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;
    public AnimationClip animClip; // Публичное поле для анимационного клипа
    private Default tower;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        tower = gameObject.GetComponent<Default>();
    }

    void Update()
    {
        ChangeSpeed();
    }

    void ChangeSpeed()
    {
        bool isPlaying = IsAnimationClipPlaying(animClip);
        if(isPlaying)
        {
            float speed = tower.attackSpeed;
            animator.speed = speed / GetAnimationClipLength(animClip) ;
        }
        else
        {
            animator.speed = 1;
        }
    }
     float GetAnimationClipLength(AnimationClip clip)
    {
        // Проверяем, что клип не равен null
        if (clip != null)
        {
            // Возвращаем длительность указанного клипа
            return clip.length;
        }

        // Если клип не указан, возвращаем 0
        return 0f;
    }
     bool IsAnimationClipPlaying(AnimationClip clip)
    {
        if (clip != null)
        {
            // Получаем информацию о текущем состоянии аниматора
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // Проверяем, проигрывается ли анимационный клип
            return stateInfo.IsName(clip.name);
        }
        return false;
    }


}