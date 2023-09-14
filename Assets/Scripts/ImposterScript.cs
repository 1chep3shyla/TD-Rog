using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImposterScript : MonoBehaviour
{
    public string effect;
    void Start()
    {
        StartCoroutine(Mute());
    }

    public IEnumerator Mute()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f);
            if (GameManager.Instance.allTower.Length > 0)
            {
                int tower = Random.Range(0, GameManager.Instance.allTower.Length);


                // �������� ��������� UpHave � ��������� �����
                if (GameManager.Instance.allTower[tower] != null)
                {
                    UpHave upHave = GameManager.Instance.allTower[tower].gameObject.GetComponent<UpHave>();


                    if (upHave != null)
                    {
                        // ���� ����� � ��������� ������ � UpHave
                        System.Reflection.MethodInfo method = upHave.GetType().GetMethod(effect);

                        if (method != null)
                        {
                            // �������� ��������� �����
                            method.Invoke(upHave, null);
                        }
                        else
                        {
                            Debug.LogError("Method not found in UpHave: " + effect);
                        }
                    }
                    else
                    {
                        Debug.LogError("UpHave component not found on tower.");
                    }
                }
            }
        }
    }
}