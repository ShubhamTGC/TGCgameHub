using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ErrorPageHandler : MonoBehaviour
{
    
    public void RelaodScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
