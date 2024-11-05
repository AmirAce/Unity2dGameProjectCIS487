using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Removed the typo

public class MainMenue : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync("Level1");  // Corrected the method name and added the semicolon
    }

    public void QuitGame()
    {
        Application.Quit();
        
    }
}
