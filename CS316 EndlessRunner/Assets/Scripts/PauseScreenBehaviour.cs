using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenBehaviour : MainMenuBehaviour
{
    public static bool paused;

    [Tooltip("Reference to the pause menu object to turn on/off")]
    public GameObject pauseMenu;

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SetPauseMenu(bool isPaused)
    {
        paused = isPaused;
        Time.timeScale = (paused) ? 0 : 1;
        pauseMenu.SetActive(paused);
    }
    private void Start()
    {
        paused = false;
    }
}
