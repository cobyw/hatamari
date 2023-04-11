using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{

    [SerializeField] GameObject pauseMenuUI;

    public bool gameIsPaused = false;

    public void OnPause(InputValue value)
    {
        if (value.Get<float>() == 1.0f)
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }



    private void Start()
    {
        Resume();
    }

    public void Resume()
    {
        if (BackgroundMusic.Instance != null)
        {
            BackgroundMusic.Instance.PauseVolume(false);
        }
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    public void Pause()
    {
        if (BackgroundMusic.Instance != null)
        {
            BackgroundMusic.Instance.PauseVolume(true);
        }
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
}
