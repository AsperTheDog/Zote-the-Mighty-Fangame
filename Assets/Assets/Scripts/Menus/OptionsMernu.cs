using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMernu : MonoBehaviour
{

    public GameObject optionsMenu;
    public static GameObject prevMenu;

    void Start()
    {
        optionsMenu.SetActive(false);
    }

    public void Back()
    {
        optionsMenu.SetActive(false);
        prevMenu.SetActive(true);
    }
}
