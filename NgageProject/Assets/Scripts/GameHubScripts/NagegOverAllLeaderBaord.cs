using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class NagegOverAllLeaderBaord : MonoBehaviour
{
    // Start is called before the first frame update
    public Button Overall, Game;
    public Sprite Clicked, NotClicked;
    public GameObject overallHeader, OverallLBhandler,GameHeader,GameLBhandler,OnlyGameLBPage;
    [HideInInspector] public List<Sprite> AvatarModel;
    public OverallLBdata overalldata;
    public GameLeaderBoardPage gameleaderboard;
    public OnlygameLBPage IndividualGameLb;
    public GameObject OverallTopperPage, GameLBtopper;
    public HomePageCardSection HomePagedata;
    public Text userScore;
    public GameObject PercentileTable;
    public PercentileTableCreator PercentileBoard;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        userScore.text = HomePagedata.Uservalue.ToString();
        PercentileTable.SetActive(true);
        //OverallTopperPage.SetActive(true);
        GameLBtopper.SetActive(false);
        Overall.image.sprite = Clicked;
        Game.image.sprite = NotClicked;
        //overallHeader.SetActive(true);
        //OverallLBhandler.SetActive(true);
        GameHeader.SetActive(false);
        GameLBhandler.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenGameLeaderboard()
    {
        PercentileTable.SetActive(false);
        Overall.image.sprite = NotClicked;
        Game.image.sprite = Clicked;
        //overallHeader.SetActive(false);
        //OverallLBhandler.SetActive(false);
        GameHeader.SetActive(true);
        GameLBhandler.SetActive(true);
    }

    public void OpenOverallLB()
    {
        GameLBtopper.SetActive(false);
        PercentileTable.SetActive(true);
        Overall.image.sprite = Clicked;
        Game.image.sprite = NotClicked;
        //overallHeader.SetActive(true);
        //OverallLBhandler.SetActive(true);
        OnlyGameLBPage.SetActive(false);
        GameHeader.SetActive(false);
        GameLBhandler.SetActive(false);
    }

    public void CloseLeaderboard()
    {
        overalldata.ResetLeaderBoard();
        PercentileBoard.CleanBoard();
        gameleaderboard.ResetLeaderBoard();
        IndividualGameLb.BackTOgameLb();
        this.gameObject.SetActive(false);
    }

   
}
