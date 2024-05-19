using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public AudioSource aS;
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ������� ��� �������� � ��������� �����
    public void LoadNextScene(string nameScene)
    {
        // �������� ������ ������� �������� �����
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // ��������� ��������� ����� (������ + 1)
        SceneManager.LoadScene(nameScene);

        // ���� ��� ��������� �����, �� ���������� �� ��������� ����� ��� ������ ����� �� ������ ������
        // if (currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1)
        // {
        //     SceneManager.LoadScene("MainMenu"); // ������ �������� �� ������� ����
        // }
    }
    public void PlaySFX(AudioClip clip)
    {
        aS.PlayOneShot(clip);
        aS.pitch = Random.Range(0.9f, 1.1f);
    }
    public void SetZero(Transform trans)
    {
        trans.position = new Vector3(0,0,0);
    }
}