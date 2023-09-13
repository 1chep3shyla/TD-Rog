using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // ��������� ��������� ����� (������ + 1)
        SceneManager.LoadScene(nameScene);

        // ���� ��� ��������� �����, �� ���������� �� ��������� ����� ��� ������ ����� �� ������ ������
        // if (currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1)
        // {
        //     SceneManager.LoadScene("MainMenu"); // ������ �������� �� ������� ����
        // }
    }
}