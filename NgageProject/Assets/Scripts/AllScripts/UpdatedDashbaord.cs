using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using SimpleSQL;

public class UpdatedDashbaord : MonoBehaviour
{
    public Text Username, GradeValue;
    public List<Sprite> BoyFace, GirlFace;
    public Image PlayerFace,GirlPlayerFace;
    public List<GameObject> Tabs;
    public Sprite PresssedSprite, RealesedSprite;
    public List<GameObject> MainPages;


    public Image OverallScoreFiller, ZoneScorezfiller, BonusScoreFiller;
    public Text OverallScore, ZoneScore, BonusScore;
    [SerializeField]
    private int Totalscore,BonusTotalscore;
    public List<StageOneDashboard> ZoneDashbaords;
    public List<Stage2OervallDashbaord> stage2Overall;
    public GameObject triviapage;
    public string MainUrl, OverallDataApi, CheckLevelApi, GetlevelWisedataApi;
    [Space(10)]
    public Image GameScorefiller;
    public Image GreenJournalFiller, TotalScoreFiller;
    public Text Gamescoretext, GreenJournalText, TotalScoretext;
    public Text PlayedStages, PlayedZones, PlayedBonusGames, StageText, ZoneText, BonusText;
    
    [SerializeField]
    private float TotalGameScore, TotalGreenJScore, TotalFinalScore;
    public List<Sprite> Badges;
    public GameObject BadgeLogo;
    public Sprite rookie;
    public PrioritizedDashboard Priortygame;
    public TruckGameDashboard Truckgame;

    //Stage 3 dashboard data
    public SimpleSQLManager dbmanager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (PlayerPrefs.GetString("gender").ToLower() == "m")
        {
            PlayerFace.gameObject.SetActive(true);
            GirlPlayerFace.gameObject.SetActive(false);
            PlayerSetup(BoyFace,PlayerFace);
        }
        else
        {
            PlayerFace.gameObject.SetActive(false);
            GirlPlayerFace.gameObject.SetActive(true);
            PlayerSetup(GirlFace, GirlPlayerFace);
        }
        StartCoroutine(Overalldata());
        StartCoroutine(GetCurrentBadge());
        initialSetup();
    }

    void PlayerSetup(List<Sprite> Face,Image profileImage)
    {
        Username.text = PlayerPrefs.GetString("username");
        for (int a = 0; a < Face.Count; a++)
        {
            if (a == PlayerPrefs.GetInt("characterType"))
            {
                profileImage.sprite = Face[a];
            }
        }
    }
    void initialSetup() 
    {
        //var Stage3data = dbmanager.Table<TruckGameModel>().ToList();
        //stage3TotalScore = Stage3data[0].TruckScore;
        Tabs[0].GetComponent<Image>().sprite = PresssedSprite;
        MainPages[0].SetActive(true);
        for(int a = 1; a < Tabs.Count; a++)
        {
            Tabs[a].GetComponent<Image>().sprite = RealesedSprite;
            MainPages[a].SetActive(false);
        }


      

    }

    public void Tabspressed(GameObject PressedTab)
    {
        bool enabled;
        Tabs.ForEach(t =>
        {
            t.GetComponent<Image>().sprite = t.name == PressedTab.name ? PresssedSprite : RealesedSprite;
            
        });

        MainPages.ForEach(p =>
        {
            enabled = p.name == PressedTab.name;
            p.gameObject.SetActive(enabled);
        });

    }


    public void closeDashboard()
    {
        triviapage.SetActive(true);
        StartCoroutine(closeingtask());
       
       
    }

    IEnumerator closeingtask()
    {
        Truckgame.Resetdata();
        Priortygame.Resettask();
        for (int a = 0; a < ZoneDashbaords.Count; a++)
        {
            ZoneDashbaords[a].resetTask();
            yield return new WaitForSeconds(0.5f);
        }
        for(int b=0;b < stage2Overall.Count; b++)
        {
            stage2Overall[b].ResetTask();
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        triviapage.SetActive(false);
        this.gameObject.SetActive(false);
    }

    IEnumerator Overalldata()
    {
        string HittingUrl = MainUrl + OverallDataApi + "?UID=" + PlayerPrefs.GetInt("UID") + "&id_org_game=" + 1;
        WWW response = new WWW(HittingUrl);
        yield return response;
        if(response.text != null)
        {
            Debug.Log("oevrall data " + response.text);
            OverallDashboardModel OverallModel = Newtonsoft.Json.JsonConvert.DeserializeObject<OverallDashboardModel>(response.text);
           
            GradeValue.text = OverallModel.Grade;
            float GameScore = int.Parse(OverallModel.TotalGameScore);
            float GreenJournal = int.Parse(OverallModel.GreenJournelScore);
            Gamescoretext.text = GameScore.ToString();
            GreenJournalText.text = GreenJournal.ToString();
            TotalScoretext.text = (GameScore).ToString() ;
            GameScorefiller.fillAmount = GameScore / TotalGameScore;
            GreenJournalFiller.fillAmount = GreenJournal / TotalGreenJScore;
            TotalScoreFiller.fillAmount = (GameScore) / TotalFinalScore;
            OverallScore.text = (int.Parse(OverallModel.TotalAllLevelScore)).ToString();
            ZoneScore.text =(int.Parse(OverallModel.TotalZonesScore)).ToString();
            BonusScore.text = OverallModel.TotalBonusScore.ToString();
            OverallScoreFiller.fillAmount = float.Parse(OverallModel.TotalGameScore) / (float)Totalscore;
            ZoneScorezfiller.fillAmount = float.Parse(OverallModel.TotalZonesScore) / (float)Totalscore;
            BonusScoreFiller.fillAmount = float.Parse(OverallModel.TotalBonusScore) / (float)BonusTotalscore;
            PlayedStages.text = "Stage Played:"+ OverallModel.TotalStagesPlayed + "/3";
            PlayedZones.text = "Zone Played:" + OverallModel.TotalZonesPlayed ;
            PlayedBonusGames.text ="Bonus Game: " + OverallModel.TotalBonusPlayed;
            StageText.text =(int.Parse(OverallModel.TotalGameScore)).ToString();
            ZoneText.text = (int.Parse(OverallModel.TotalZonesScore)).ToString();
            BonusText.text = OverallModel.TotalBonusScore.ToString();
        }
    }

    IEnumerator GetCurrentBadge()
    {
        string Hittingurl = $"{MainUrl}{CheckLevelApi}?UID={PlayerPrefs.GetInt("UID")}&OID={PlayerPrefs.GetInt("OID")}";
        WWW level_res = new WWW(Hittingurl);
        yield return level_res;
        if(level_res.text != null)
        {
            Debug.Log(" badge name " + level_res.text);
            LevelClearness level_data = Newtonsoft.Json.JsonConvert.DeserializeObject<LevelClearness>(level_res.text);
            if(level_data.LastAchivedBadge != null)
            {
                for (int a = 0; a < Badges.Count; a++)
                {
                    if (level_data.LastAchivedBadge.ToLower() == Badges[a].name)
                    {
                        BadgeLogo.GetComponent<Image>().sprite = Badges[a];
                    }
                }
            }
            else
            {
                BadgeLogo.GetComponent<Image>().sprite = rookie;
            }
          
            
        }
    }



  

}
