using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{ 
 
    public GameObject mainMenu;
    public GameObject optionsMenu;

    void Start()
    {
        mainMenu.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("CristalineWaterfallsMainArea");
    }

    public void Options()
    {
        OptionsMernu.prevMenu = mainMenu;
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit(); 
    }
}
