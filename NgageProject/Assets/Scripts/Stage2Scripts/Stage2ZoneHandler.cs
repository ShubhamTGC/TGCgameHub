using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using LitJson;
using SimpleSQL;
using UnityEngine.SceneManagement;
using m2ostnextservice.Models;
using System.Xml.Serialization;

public class Stage2ZoneHandler : MonoBehaviour
{
    [Header("API SECTION======")]
    public string MainUrl;
    public string GetGamesIDApi,PostMasterScoreApi, RoomData_api,PostZoneDataApi;
    public int id_game_content;
    [SerializeField] private int Gamelevel;
    public string ZoneName;
    public GameObject level1, level2,level1Mainpage,level2MainPage;
    public ThrowWasteHandler Level1Controller, Level2Controller;
    public GameObject PopUpstatus;
    public Text PopupMsg;
    public Button LevelButton;
    public GameObject Dashboard;
    public Button skip,Back;
    public List<int> RoomIds;
    public GameObject MainZone, Startpage, ZoneselectionPage, ZonePag, Gamecanvas;
    [HideInInspector]
    public List<ObjectGamePostModel> logs = new List<ObjectGamePostModel>();
    public Text Zonetext;
    [SerializeField] private string ZoneHeading;
    public Text Username, Gradeno;
    public Text GameScore, totalScoreText;
    public Image GreenscoreFiller, totalScorefiller;
    [SerializeField] private float GameScoretotal;

    [Space(20)]

    //============= achivement shelf api task========================
    public string GetBadgeConfigApi;
    public string MostActivePlayerApi, PostBadgeUserApi, LeaderBoardApi, CheckHighscoreApi,MostObservantApi, levelClearnessApi, GetlevelWisedataApi, StageUnlockApi;
    [SerializeField] private string Highscorename;
    [SerializeField] private string mostActiveName;
    [SerializeField] private string MostobservantName;
    private int HighScoreBadgeid, ActivebadgeId,MostObservantBadgeId, MyTotalScore;
    private int level2GameBadgeId;
    public string Level2BadgeName;
    [SerializeField] private int Leve2number;
    private int totalscoreOfUser;
    private bool Stage2unlocked;
    public int Stage2UnlockScore;
    public bool ZoneCleared;
    public int GameAttemptNumber =0;
    public string Zonenumber;
    public List<ThrowWasteHandler> ZonesLevelHolder;
    public SimpleSQLManager dbmanager;
    [HideInInspector] public int GameBonusPoint;
    public GameObject MenuButton;
    public GameObject lastpageOptions;
    public Text popupMsg;
    public Image AvatarImage;
    public Sprite happy, sad;
    private HomePageScript homeinstance;
    [SerializeField] private int Id_game =3;
    public List<int> roomid = new List<int>();
    private int TotalScore;
    private int TotalGameScore;
    private float percentageValue;
    public GameObject LeaderBoardPage;
    public GameObject popUpPage;
    public Transform Level1objecthandler, Level2Objecthandler;
    public GameObject ScartchcardPage;
    private bool IsPassed;
    void Start()
    {
        
    }

     void OnEnable()
    {
        IsPassed = false;
        homeinstance = HomePageScript.Homepage;
        level1.SetActive(true);
        level1Mainpage.SetActive(true);
        Level1Controller.level1 = true;
        StartCoroutine(GetGameAttemptNoTask());
        StartCoroutine(GetGameScoredata());
        //skip.onClick.RemoveAllListeners();
        //skip.onClick.AddListener(delegate { Skiplevel();});

    }

    //IEnumerator GetSounddata()
    //{
    //    yield return new WaitForSeconds(0.2f);
    //    var SettingLog = dbmanager.Table<GameSetting>().FirstOrDefault();
    //    if (SettingLog != null)
    //    {
    //        this.gameObject.GetComponent<AudioSource>().volume = SettingLog.Sound;
    //        PlayerPrefs.SetString("VibrationEnable", SettingLog.Vibration == 1 ? "true" : "false");
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GetGameScoredata()
    {
        yield return new WaitForSeconds(0.1f);
        var GameLog = dbmanager.Table<ObjectGameList>().Where(x => x.RoomId == 3).Select(y=>y.CorrectPoint).ToList();
        GameLog.ForEach(x =>
        {
            TotalGameScore += x;
        });

        percentageValue = dbmanager.Table<GameListDetails>().FirstOrDefault(a => a.GameId == 3).CompletePer;

    }

    IEnumerator GetGameAttemptNoTask()
    {

        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.GetAttemopNo}?UID={PlayerPrefs.GetInt("UID")}&GID={Id_game}&RID={roomid[0]}";
        WWW Attempt_res = new WWW(HittingUrl);
        yield return Attempt_res;
        if (Attempt_res.text != null)
        {
            if (Attempt_res.text != "[]")
            {
                AESAlgorithm aes = new AESAlgorithm();
                string Log = Attempt_res.text.TrimStart('"').TrimEnd('"');
                string Decryptedlog = aes.getDecryptedString(Log);
                AttemptNumberModel Attemptlog = Newtonsoft.Json.JsonConvert.DeserializeObject<AttemptNumberModel>(Decryptedlog);
                GameAttemptNumber = Convert.ToInt32(Attemptlog.Master_AttemptNo);
            }
            else
            {
                GameAttemptNumber = 0;
            }
        }
    }



    public void checkLevelStatus(String msg)
    {
        if (Level1Controller.LevelCompleted && !Level2Controller.LevelCompleted)
        {
            StartCoroutine(Level1clear(msg));
        }
        if(Level1Controller.LevelCompleted && Level2Controller.LevelCompleted)
        {
            TotalScore = Level1Controller.SCore + Level2Controller.SCore;
            GameResultAnalysis();
            
            StartCoroutine(MAsterTablePosting());
             Level1Controller.generateDashboardL1();
            Level2Controller.generateDashboardL2();
          
            Level1Controller.LevelCompleted = false;
            Level2Controller.LevelCompleted = false;
          
            //GameScore.text = TotalScore.ToString();
            //totalScoreText.text = TotalScore.ToString();
            //GreenscoreFiller.fillAmount = (float)TotalScore / GameScoretotal;
            //totalScorefiller.fillAmount = (float)TotalScore / GameScoretotal;
            ZoneCleared = true;
        }
    }

    void GameResultAnalysis()
    {
        string msg;
        float UserPercentage = ((float)TotalScore / (float)TotalGameScore) * 100.0f;
        if(UserPercentage >= percentageValue)
        {
            msg = "Congratulations ! You have cleared this game with " + UserPercentage.ToString("0") + "%";
            IsPassed = true;
            AvatarImage.sprite = happy;
        }
        else
        {
            AvatarImage.sprite = sad;
            msg = "Sorry ! You have failed in this game with " + UserPercentage.ToString("0") + "%";
        }
        popupMsg.text = msg;
        lastpageOptions.SetActive(true);
    }

    public void CloseGameStatusPage()
    {
        StartCoroutine(CloseLastpage());
    }
    IEnumerator CloseLastpage()
    {
        iTween.ScaleTo(lastpageOptions, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.4f);
    
        lastpageOptions.SetActive(false);
        popupMsg.text = "";
        if (IsPassed)
        {
            ScartchcardPage.SetActive(true);
        }
        else
        {
            LeaderBoardPage.SetActive(true);
        }
    }
    IEnumerator Level1clear(string msg)
    {
        PopupMsg.text = msg;
        //PopupMsg.text = "Congratulations! You have successfully completed Level 1\nPlease click on Level 2 to play!";
        PopUpstatus.SetActive(true);
        LevelButton.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Level 2";
        LevelButton.onClick.RemoveAllListeners();
        LevelButton.onClick.AddListener(delegate { switchLevel(); });
        iTween.ScaleTo(PopUpstatus, Vector3.one, 0.4f);
        yield return new WaitForSeconds(0.4f);

    }
    void switchLevel()
    {
        StartCoroutine(switchLeveltask());
    }

    IEnumerator switchLeveltask()
    {
        iTween.ScaleTo(PopUpstatus, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.0f);
        PopupMsg.text = "";
        PopUpstatus.SetActive(false);
        level1Mainpage.SetActive(false);
        Level1Controller.level1 = false;
        level2.SetActive(true);
        level2MainPage.SetActive(true);
        Level2Controller.level2 = true;
        Debug.Log(Level2Controller.level2 + " name " + Level2Controller.gameObject.name);
    }

    IEnumerator clearLevel(string msg)
    {
        PopupMsg.text = msg + Zonenumber;// "Congratulations! You have successfully completed " + Zonenumber;
        LevelButton.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Next";
        PopUpstatus.SetActive(true);
        LevelButton.onClick.RemoveAllListeners();
        LevelButton.onClick.AddListener(delegate { showDashboard(); });
        iTween.ScaleTo(PopUpstatus, Vector3.one, 0.4f);
        yield return new WaitForSeconds(0.4f);
    }

   
    void showDashboard()
    {
        StartCoroutine(dashboardTask());
    }
    IEnumerator dashboardTask()
    {
        iTween.ScaleTo(PopUpstatus, Vector3.zero, 0.4f);
        LevelButton.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        PopupMsg.text = "";
        yield return new WaitForSeconds(0.4f);
        PopUpstatus.SetActive(false);
        level2MainPage.SetActive(false);
        Dashboard.SetActive(true);
    }

    public void Skiplevel()
    {
        StartCoroutine(levelReseting());

    }
    IEnumerator levelReseting()
    {
        Level1Controller.CloseGame();
        Level2Controller.CloseGame();
        Level1Controller.ResetTask();
        Level2Controller.ResetTask();
        yield return new WaitForSeconds(1f);
        //MenuButton.SetActive(true);
        Dashboard.SetActive(false);
        Gamecanvas.SetActive(false);

    }

    public void BackFromLevel()
    {
        StartCoroutine(Backtask());
    }
    IEnumerator Backtask()
    {
        Level1Controller.CloseGame();
        Level2Controller.CloseGame();
        yield return new WaitForSeconds(2f);
        Gamecanvas.SetActive(false);
        ZonePag.SetActive(true);
        ZoneselectionPage.SetActive(true);
        Startpage.SetActive(true);
       // MenuButton.SetActive(true);
        Startpage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        MainZone.SetActive(false);

    }
 
    IEnumerator MAsterTablePosting()
    {
       
        yield return new WaitForSeconds(0.1f);
        string hittingUrl = $"{MainUrls.BaseUrl}{MainUrls.MasterLogApi}";
        NgageMasterPostLog PostField = new NgageMasterPostLog()
        {
            id_user = PlayerPrefs.GetInt("UID"),
            ID_ORGANIZATION = PlayerPrefs.GetInt("OID"),
            score = TotalScore,
            attempt_no = GameAttemptNumber + 1,
            timetaken_to_complete = "00:00",
            is_completed = 1,
            game_type = 1,
            Id_Game = Id_game
        };

        AESAlgorithm aes = new AESAlgorithm();
        string PostLog = Newtonsoft.Json.JsonConvert.SerializeObject(PostField);
        Debug.Log("master log data " + PostLog);

        string EncryptedData = aes.getEncryptedString(PostLog);
        CommonModel model = new CommonModel()
        {
            Data = EncryptedData
        };

        string finaldata = Newtonsoft.Json.JsonConvert.SerializeObject(model);


        Debug.Log("Master posting  " + finaldata);

        using (UnityWebRequest request = UnityWebRequest.Put(hittingUrl, finaldata))
        {
            Debug.Log(request);
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();
            if (!request.isNetworkError && !request.isHttpError)
            {
                
                Debug.Log(" response " + request.downloadHandler.text);
            }
            else
            {
                Debug.Log("prob : " + request.error);
            }
        }

    }

    public void PostZoneData(string jsondata)
    {
        //Debug.Log("Game data log " + jsondata);
        StartCoroutine(PostzoneTask(jsondata));
    }
    IEnumerator PostzoneTask(string JsonValue)
    {
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.ObjectGamePostApi}";
        AESAlgorithm aes = new AESAlgorithm();
        

        string EncryptedData = aes.getEncryptedString(JsonValue);
        CommonModel model = new CommonModel()
        {
            Data = EncryptedData
        };

        string finaldata = Newtonsoft.Json.JsonConvert.SerializeObject(model);
        using (UnityWebRequest request = UnityWebRequest.Put(HittingUrl, finaldata))
        {
            Debug.Log(request);
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();
            if (!request.isNetworkError && !request.isHttpError)
            {

                Debug.Log(" response " + request.downloadHandler.text);
            }
            else
            {
                Debug.Log("prob : " + request.error);
            }
        }

    }



    public void VibrateDevice()
    {
        if (!PlayerPrefs.HasKey("VibrationEnable"))
        {
            Vibration.Vibrate(400);
            //Debug.Log("vibration");
        }
        else
        {
            string vibration = PlayerPrefs.GetString("VibrationEnable");

            if (vibration == "true")
            {
                Vibration.Vibrate(400);
                //Debug.Log("vibration");
            }
            else
            {
                //Debug.Log("vibration not enabled");
            }
        }
    }


    public void GotoHome()
    {
        Time.timeScale = 1f;
        int index = 0;
        Destroy(homeinstance);
        StartCoroutine(Hometask(index));
    }
    IEnumerator Hometask(int index)
    {
        iTween.ScaleTo(popUpPage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        PopupMsg.text = "";
        popUpPage.SetActive(false);
        SceneManager.LoadScene(index);
    }

    public void restartGame()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(Hometask(index));
    }
    public void ClosepausePage()
    {
        Time.timeScale = 1f;
        StartCoroutine(ClosurePausePage());
    }

    IEnumerator ClosurePausePage()
    {

        iTween.ScaleTo(popUpPage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.4f);
        popUpPage.SetActive(false);
        int count = Level1objecthandler.childCount;
        for (int a = 0; a < count; a++)
        {
            if (Level1objecthandler.GetChild(a).gameObject.activeInHierarchy)
            {
                Level1objecthandler.GetChild(a).gameObject.GetComponent<CircleCollider2D>().enabled = true;
            }
        }
        int count2 = Level2Objecthandler.childCount;
        for (int b = 0; b < count2; b++)
        {
            if (Level2Objecthandler.GetChild(b).gameObject.activeInHierarchy)
            {
                Level2Objecthandler.GetChild(b).gameObject.GetComponent<CircleCollider2D>().enabled = true;
            }
        }
        Level1Controller.TimePaused = false;

    }

    public void PauseMethod()
    {
        int count = Level1objecthandler.childCount;
        for(int a = 0; a < count; a++)
        {
            if (Level1objecthandler.GetChild(a).gameObject.activeInHierarchy)
            {
                Level1objecthandler.GetChild(a).gameObject.GetComponent<CircleCollider2D>().enabled = false;
            }
        }
        int count2 = Level2Objecthandler.childCount;
        for (int b = 0; b < count2; b++)
        {
            if (Level2Objecthandler.GetChild(b).gameObject.activeInHierarchy)
            {
                Level2Objecthandler.GetChild(b).gameObject.GetComponent<CircleCollider2D>().enabled = false;
            }
        }
        StartCoroutine(BackToHomeMethod());
    }

    IEnumerator BackToHomeMethod()
    {
        Level1Controller.TimePaused = true;
        popUpPage.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        Time.timeScale = 0f;
    }

}
