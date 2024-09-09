using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AchievementWorker : MonoBehaviour
{
    public Image icon;
    [Space]
    public TMP_Text name;
    public TMP_Text dis;
    [Space]
    public Animator achievementAnimator;
    [Space]
    public List<Achievement> allAchievements = new List<Achievement>();
    [Space]
    private bool work;

    void Update()
    {
        if(allAchievements.Count >0 && !work)
        {
            StartCoroutine(ProcessAchievements());
        }
    }
    public IEnumerator ProcessAchievements()
    {
        work = true;
        achievementAnimator.gameObject.SetActive(true);
        achievementAnimator.Rebind();
        achievementAnimator.Update(0f);
        achievementAnimator.Play("achievement_anim");

        icon.sprite = allAchievements[0].Icon;
        name.text = allAchievements[0].title;
        dis.text = allAchievements[0].localDiscription;
        yield return new WaitForSeconds(3.1f);
        allAchievements.RemoveAt(0);
        work = false;
    }
}
