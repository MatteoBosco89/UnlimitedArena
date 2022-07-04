using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Options");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SelectHero()
    {
        Debug.Log("Select Hero");
    }
}
