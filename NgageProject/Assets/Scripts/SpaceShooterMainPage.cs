using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpaceShooterMainPage : MonoBehaviour
{
    public static SpaceShooterMainPage instant;
    private GameManager gameManager;
    public GameObject GameGuidePage,PreviewPage;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        if (PlayerPrefs.GetString("From").Equals("Main", System.StringComparison.OrdinalIgnoreCase))
        {
            GameGuidePage.SetActive(true);
            PreviewPage.SetActive(false);
        }
        else
        {
            GameGuidePage.SetActive(false);
            PreviewPage.SetActive(true);
        }
        
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
