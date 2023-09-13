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

    // Функция для перехода в следующую сцену
    public void LoadNextScene(string nameScene)
    {
        // Получаем индекс текущей активной сцены
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Загружаем следующую сцену (индекс + 1)
        SceneManager.LoadScene(nameScene);

        // Если это последняя сцена, то переходите на начальную сцену или другую сцену по вашему выбору
        // if (currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1)
        // {
        //     SceneManager.LoadScene("MainMenu"); // Пример перехода на главное меню
        // }
    }
}