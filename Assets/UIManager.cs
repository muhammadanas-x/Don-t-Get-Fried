using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{


    public GameObject panel;
    public GameObject pauseButton;
    public GameObject resumeButton;
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        panel.SetActive(true);
        resumeButton.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        panel.SetActive(false);
        resumeButton.SetActive(false);
    }

    public void ExitToMain()
    {
        SceneManager.LoadScene("Menu");
    }
}
