using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject pauseMenuElems;
    public GameObject optionsMenu;

    public static bool gameIsPaused = false;

    void Start()
    {
        Continue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Continue();
            }
            else
            {
                Pause();
            }
        }
    }


    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

        Animator animT = gameObject.GetComponent<Animator>();
        animT.Play("PauseTopFleur");

        Animator animB = gameObject.GetComponent<Animator>();
        animB.Play("PauseBottomFleur");
    }

    public void Continue()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    
    public void Options()
    {
        OptionsMernu.prevMenu = pauseMenuElems;
        pauseMenuElems.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
