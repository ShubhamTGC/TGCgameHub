using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpaceShooterMainPage : MonoBehaviour
{
    public static SpaceShooterMainPage instant;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void LoadGames(int index)
    {
        SceneManager.LoadScene(index);
    }


    

}
