using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
 
    void Start()
    {
        CursorLockMode lockMode = CursorLockMode.Confined;
        Cursor.lockState = lockMode;
        AudioListener.volume = PlayerPrefs.GetFloat("masterVolume");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
