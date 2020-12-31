using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using LitJson;
using SimpleSQL;
using AngleSharp.Extensions;
using UnityEngine.SceneManagement;
using m2ostnextservice.Models;

public class GameBoard : MonoBehaviour
{

    private static int Boardwidth = 5000;
    private static int BoardHeight = 5000;
    public GameObject[,] board = new GameObject[Boardwidth, BoardHeight];
    public Transform Capsuletransform;
    public List<GameObject> Capsule;
    private List<int> randomindex = new List<int>();
    private List<GameObject> CurrentActive = new List<GameObject>();
    private List<GameObject> PlasticActive = new List<GameObject>();
    private List<GameObject> GlassActive = new List<GameObject>();
    private List<GameObject> MetalActive = new List<GameObject>();
    private int ObjCounter = 0;
    private int num = 0;
    public GameObject TruckGamePlatform;
    private int selectionCounter = 1;
    private List<KeyValuePair<string, List<GameObject>>> AllBins;
    private int ActiveTruckCount = 0;
    public List<GameObject> StationaryTrucks = new List<GameObject>();
    private int Taskcounter = 0;
    private bool CheckedCollision;
    



    [Header("Game Ui elements")]
    [Space(15)]
    private int DustbinCollectCount;
    public Text ScorePoints;
    public int ScorePointCounter;
    public List<MonsterMovement> monsters;
    public Text ScoreText;
    public GameObject Correcteffect, WrongEffect;
    public Canvas Maincanavs;
    public GameObject BackBtn;


    [Header("Dashboard Fileds")]

    public GameObject PriorityPrefeb, AlignPrefeb;
    private List<int> ItemCollectionCount = new List<int>();
    private List<int> AttemptScore = new List<int>();
    private List<int> SCoreCollected = new List<int>();
    private List<string> PriorityUserdata;
    public List<GameObject> CorrectSequence;
    public List<string> TruckSequence = new List<string>();
    public List<int> TruckID = new List<int>();
    public List<int> UserSelectedId = new List<int>();
    public int AttemptNumber;
    public int CorrectPoint, WrongPoint;
    public List<GameObject> tableSequence;
    public List<GameObject> ALignTableseq;
    public List<GameObject> PriorityObj, AlignObj;
    private List<int> CorrectAlignStatus = new List<int>();
    [SerializeField]
    private List<string> CenterNames = new List<string>();
    private int DustbinCounter;
    private int DustinCollectScore;
    public float AllGameScore;
    public Button SkipButton;
    public GameObject TruckGamePage;
    [SerializeField]
    private List<Vector3> TrucksPos = new List<Vector3>();
    public List<GameObject> MainTrucks;
    public GameObject monster1, monster2, monster3,Monster4;


    public GameObject GameDonepanel;
    public Text Gamedonemsg;
    public Image avatar;
    public Sprite goodmood, badmood;


    [Header("Timer Fields ==")]
    private bool TimePaused = true;
    public float minut;
    public float second;
    private float Totaltimer, RunningTimer;
    private float sec;
    private float Mintvalue;
    private bool helpingbool = true;
    public Image Timerbar;
    public Text Timer;
    public AudioClip GameSoundTrack,Rightsound,WrongSound;



    private int TotaltruckScore, GameAttemptNumber, level2GameBadgeId;
    public int Gamelevel;
     private List<int> game_content = new List<int>();
     private string ZoneName;
     private List<int> RoomIds = new List<int>();
     private string FirstGame, SecondGame;
    public SimpleSQLManager dbmanager;
     private List<int> is_correct_PR;
     private List<int> Truckscorevalue;
     private List<string> UserselectedTruck;
    private List<string> CorrectSeqOfgame;


    //=========================== Truck Game list ==================
   private List<string> TruckNamePlayed  = new List<string>();
   private List<int>    dustbinCounts    = new List<int>();
   private List<int>    TruckDustbinScore = new List<int>();
   private List<string> Reachedcentername = new List<string>();
   private List<int>    CenterScoreOfUser = new List<int>();
   private List<int>    Is_correctreached = new List<int>();
    private List<int> Reached_center = new List<int>();
   private int finalgameScore;
    private int OverallScore;

    public GameObject InstructionPanel;
    private RectTransform textRect;
    private Vector2 uiOffset;
    public List<GameObject> MosterSound;
    private AudioSource DustbinSound;
    public List<GameObject> CenterSound;
    [HideInInspector] public List<string> centernames = new List<string>();
    [HideInInspector] public List<int> CenterCorrectPoint = new List<int>();
    [HideInInspector] public List<int> CenterWrongPoint = new List<int>();
    [HideInInspector]  public int monsterAttackScore;
    [HideInInspector] public int CenterRightpoints,CenterWrongpoint;
    private HomePageScript homeinstance;
    [SerializeField]private int Id_game;
    private int UserAttemptnumber=3;
    private int TruckId;
    public GameObject GameStatuspage,GameLeaderBoard;
    public Text Statusmsg;
    public Image StatusMood;
    private bool IsPassed;
    public GameObject ScartchcardPage;
    private float percentageScore;
    private void Awake()
    {
        DustbinCollectCount = 0;
        UnityEngine.Object[] objects = GameObject.FindObjectsOfType(typeof(Transform));

        foreach (Transform o in objects)
        {
            Vector2 pos = o.transform.position;
            if (o.name == "Ambulance" || o.name == "Ambulance" || o.name == "Ambulance" )
            {
                Debug.Log(" Gameobject " + o.name);
            }
            else
            {
                board[(int)pos.x, (int)pos.y] = o.gameObject;
            }
        }

        //Capsule = new GameObject[Capsuletransform.childCount];
        for (int a = 0; a < Capsuletransform.childCount; a++)
        {
            Capsule.Add(Capsuletransform.GetChild(a).gameObject);
        }
        AllBins = new List<KeyValuePair<string, List<GameObject>>>()
        {
            new KeyValuePair<string, List<GameObject>>("Ambulance", Capsule)
        };

        int maxLength = 0;

        while (maxLength < Capsule.Count)
        {
            int num = UnityEngine.Random.Range(0, Capsule.Count);
            if (!randomindex.Contains(num))
            {
                randomindex.Add(num);
                maxLength++;
            }
        }

    }


    void OnEnable()
    {
        ObjCounter = 0;
        IsPassed = false;
        homeinstance = HomePageScript.Homepage;
        var localLog = dbmanager.Table<TruckGameList>().FirstOrDefault();
        if (localLog != null)
        {
            CorrectPoint = localLog.CorrectPoint;
            WrongPoint = localLog.WrongPoint;
        }

        var centerdata = dbmanager.Table<TruckCenterDetails>().FirstOrDefault();
        if(centerdata != null)
        {
            CenterWrongpoint = centerdata.WrongPoint;
            CenterRightpoints = centerdata.CorrectPoint;
        }
        Id_game = localLog.GameId;
        TruckId = localLog.TruckId;

        var Monsterdata = dbmanager.Table<MonsterDetails>().FirstOrDefault();
        if(Monsterdata != null)
        {
            monsterAttackScore = Monsterdata.CatchPoint;
        }

        percentageScore = dbmanager.Table<GameListDetails>().FirstOrDefault(x => x.GameId == Id_game).CompletePer;
        StartCoroutine(GetGameAttemptNoTask());
    }

    public void GamePlay()
    {
        monster1.SetActive(true);
        monster2.SetActive(true);
        monster3.SetActive(true);
        Monster4.SetActive(true);
        DustbinSound = this.gameObject.GetComponent<AudioSource>();
        textRect = ScoreText.GetComponent<RectTransform>();
        // StartCoroutine(GetAssessmentQues());
        AllCommonTask();
    }

    void AllCommonTask()
    {
        TimePaused = true;
        Mintvalue = minut;
        sec = second;
        Totaltimer = (Mintvalue * 60) + second;
        RunningTimer = Totaltimer;
        DustbinCounter = 0;
        DustinCollectScore = 0;
        PriorityObj = new List<GameObject>();
        AlignObj = new List<GameObject>();
        ScorePointCounter = 0;
        ScorePoints.text = ScorePointCounter.ToString();
        DustbinCollectCount = 0;
        BackBtn.SetActive(true);
        //GameScore.text = ScorePointCounter.ToString();
        //GamescoreFiller.fillAmount = 0f;
        //TotalGamescoreFiller.fillAmount = 0f;
        //TotalGameScore.text = ScorePointCounter.ToString();
        for (int a = 0; a < MainTrucks.Count; a++)
        {
            TrucksPos[a] = MainTrucks[a].transform.position;
        }

        PlayGame();
    }
    void Start()
    {

    }

    void TruckGameSetup()
    {
        
        CurrentActive = Capsule;
        CheckedCollision = true;
        for(int a = 0; a < 3; a++)
        {
            CurrentActive[a].SetActive(true);
            //ObjCounter++;
        }
        ActiveTruckCount++;

    }

    public void getGamedata()
    {
        StartCoroutine(GetSounddata());
        game_content.Clear();
        TruckNamePlayed.Clear();
        dustbinCounts.Clear();
        TruckDustbinScore.Clear();
        Reachedcentername.Clear();
        CenterScoreOfUser.Clear();
        Is_correctreached.Clear();
        ItemCollectionCount.Clear();
        CorrectAlignStatus.Clear();
        CenterNames.Clear();
        Truckscorevalue.Clear();
        UserselectedTruck.Clear();
        // StartCoroutine(GetGameAttemptNoTask());
        //StartCoroutine(GetGamesIDactivity());

    }

    IEnumerator GetSounddata()
    {
        var SettingLog = dbmanager.Table<GameSetting>().FirstOrDefault();
        if (SettingLog != null)
        {
            DustbinSound.volume = SettingLog.Sound;
            CenterSound.ForEach(x =>
            {
                x.GetComponent<AudioSource>().volume = SettingLog.Sound;
            });
            MosterSound.ForEach(y =>
            {
                y.GetComponent<AudioSource>().volume = SettingLog.Sound;
            });
           TruckGamePlatform.GetComponent<AudioSource>().volume = SettingLog.Music;
            PlayerPrefs.SetString("VibrationEnable", SettingLog.Vibration == 1 ? "true" : "false");
            yield return new WaitForSeconds(0.2f);
        }
    }
    IEnumerator GetGameAttemptNoTask()
    {

        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.GetAttemopNo}?UID={PlayerPrefs.GetInt("UID")}&GID={Id_game}&RID={0}";
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


    //IEnumerator GetGamesIDactivity()
    //{
    //    string HittingUrl = MainUrl + GetGamesIDApi + "?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID") +
    //        "&id_org_game=" + 1 + "&id_level=" + Gamelevel;
    //    WWW GameResponse = new WWW(HittingUrl);
    //    yield return GameResponse;
    //    if (GameResponse.text != null)
    //    {
    //        Debug.Log("game id data " + GameResponse.text);
    //        GetLevelIDs gameIDs = Newtonsoft.Json.JsonConvert.DeserializeObject<GetLevelIDs>(GameResponse.text);
    //        var ContentList = gameIDs.content.ToList();
    //        game_content.Add(ContentList.FirstOrDefault(x => x.title == FirstGame).id_game_content);
    //        game_content.Add(ContentList.FirstOrDefault(x => x.title == SecondGame).id_game_content);
    //        //StartCoroutine(CollectRoomdata());
    //    }
    //}



    //IEnumerator CollectRoomdata()
    //{

    //    string Hittingurl = MainUrl + RoomData_api + "?id_user=" + PlayerPrefs.GetInt("UID") + "&id_org_content=" + game_content;
    //    Debug.Log("main Url " + Hittingurl);
    //    WWW roominfo = new WWW(Hittingurl);
    //    yield return roominfo;
    //    if (roominfo.text != null)
    //    {
    //        Debug.Log("rooom id " + roominfo.text);
    //        JsonData RoomrRes = JsonMapper.ToObject(roominfo.text);
    //        for (int a = 0; a < RoomrRes.Count; a++)
    //        {
    //            RoomIds.Add(int.Parse(RoomrRes[a]["id_room"].ToString()));
    //        }

    //    }
    //}


    void Update()
    {
        if (!TimePaused)
        {
            if (sec >= 0 && Mintvalue >= 0 && helpingbool)
            {
                sec = sec - Time.deltaTime;
                RunningTimer = RunningTimer - Time.deltaTime;
                Timerbar.fillAmount = RunningTimer / Totaltimer;
                if (sec.ToString("0").Length > 1)
                {
                    Timer.text = "0" + Mintvalue.ToString("0") + ":" + sec.ToString("0");
                }
                else
                {
                    Timer.text = "0" + Mintvalue.ToString("0") + ":" + "0" + sec.ToString("0");
                }

                if (sec.ToString("0") == "0" && Mintvalue >= 0)
                {
                    sec = 60;
                    Mintvalue = Mintvalue - 1;
                }
            }
            else if (helpingbool)
            {
                StartCoroutine(GameEndProcess());

            }
        }
    }


    public void CheckCollision(GameObject dustbinName, GameObject Truck)
    {

        num = CurrentActive.IndexOf(dustbinName);
        if (ObjCounter < randomindex.Count && CheckedCollision)
        {
            Taskcounter++;
            DustbinCounter++;
            CheckedCollision = false;
            CurrentActive[num].SetActive(false);
            DustbinCollectCount++;
            ScorePointCounter += CorrectPoint;
            DustinCollectScore += CorrectPoint;
            ScorePoints.text = ScorePointCounter.ToString();
            CurrentActive[num] = num != -1 ? CurrentActive[randomindex[ObjCounter]] : null;
            CurrentActive[num].SetActive(true);
            StartCoroutine(ResetBool());
            ObjCounter++;
            Debug.Log(" counter of object " + ObjCounter);
        }
        else
        {
            DustbinCollectCount++;
            DustbinCounter++;
            ScorePointCounter += CorrectPoint;
            DustinCollectScore += CorrectPoint;
            ScorePoints.text = ScorePointCounter.ToString();
            Taskcounter++;
            CurrentActive[num].SetActive(false);
        }

    }

    IEnumerator ResetBool()
    {
        yield return new WaitForSeconds(1f);
        CheckedCollision = true;
    }


    public void CheckCorrectAns(GameObject dustbin, GameObject truck)
    {
        Debug.Log(" data " + dustbin.name + "   " + truck.name);
        var wasteDustbin = AllBins.FirstOrDefault(x => x.Key.Equals(truck.name, System.StringComparison.OrdinalIgnoreCase));
        var BinValid = wasteDustbin.Value.Any(x => x.name.Equals(dustbin.name, System.StringComparison.OrdinalIgnoreCase));
        if (BinValid)
        {
            CheckCollision(dustbin, truck);
            DustbinSound.clip = Rightsound;
            DustbinSound.Play();
            Correcteffect.transform.position = dustbin.transform.position;
            ScoreText.gameObject.SetActive(true);
            Vector2 pos = Camera.main.WorldToViewportPoint(dustbin.transform.position);
            ScoreText.text = "+" + CorrectPoint;
            var screen = Camera.main.WorldToScreenPoint(dustbin.transform.position);
            screen.z = (Maincanavs.transform.position - Camera.main.transform.position).magnitude;
            var position = Camera.main.ScreenToWorldPoint(screen);
            ScoreText.gameObject.transform.position = position;
            Correcteffect.SetActive(true);
            StartCoroutine(ResetEffectgame());
         
        }
        else
        {
            WrongCollision();
            dustbin.SetActive(false);
            ScoreText.gameObject.SetActive(true);
            DustbinSound.clip = WrongSound;
            DustbinSound.Play();
            Vector2 pos = Camera.main.WorldToViewportPoint(dustbin.transform.position);
            ScoreText.text = "+" + WrongPoint;
            var screen = Camera.main.WorldToScreenPoint(dustbin.transform.position);
            screen.z = (Maincanavs.transform.position - Camera.main.transform.position).magnitude;
            var position = Camera.main.ScreenToWorldPoint(screen);
            ScoreText.gameObject.transform.position = position;
            WrongEffect.transform.position = dustbin.transform.position;
            WrongEffect.SetActive(true);
            StartCoroutine(ResetEffectgame());
        }

    }

    IEnumerator ResetEffectgame()
    {
        yield return new WaitForSeconds(1f);
        ScoreText.text = "";
        ScoreText.gameObject.SetActive(false);
    }

    void WrongCollision()
    {
        ScorePointCounter += 0;
        DustinCollectScore += 0;
        ScorePoints.text = ScorePointCounter.ToString();
    }



    public void PlayGame()
    {
        Taskcounter = 0;
        //ObjCounter = 0;
        if (ActiveTruckCount > 0)
        {
            //CurrentActive.Clear();
            StationaryTrucks[ActiveTruckCount-1].SetActive(false);
            StationaryTrucks[ActiveTruckCount].SetActive(true);

        }
        else
        {
            TimePaused = false;
            Mintvalue = minut;
            sec = second;
            Totaltimer = (Mintvalue * 60) + second;
            RunningTimer = Totaltimer;
            StationaryTrucks[ActiveTruckCount].SetActive(true);
            StartCoroutine(InstructionTask());

        }

        TruckGameSetup();

    }

    IEnumerator InstructionTask()
    {
        InstructionPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;
    }

    public void CloseInstruction()
    {
        StartCoroutine(CloseInstructionTask());
    }

    IEnumerator CloseInstructionTask()
    {
        Time.timeScale = 1f;
        iTween.ScaleTo(InstructionPanel, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.4f);
        InstructionPanel.SetActive(false);
    }


    public void TruckCenterResult(int score, string truckName, string CenterName)
    {
        int cmsScore = 0;
        if(CenterName != "null")
        {
            if (score > 0)
            {
                cmsScore = CenterRightpoints;
            }
            else
            {
                cmsScore = CenterWrongpoint;
            }
        }
        else
        {
            cmsScore = score;
        }
       
       
        CenterNames.Add(CenterName != "" ? CenterName : "null");
        Is_correctreached.Add(0);
        CorrectAlignStatus.Add(score > 0? 1 : 0);
        CenterScoreOfUser.Add(0);
        Reached_center.Add(0);
        if (ActiveTruckCount < StationaryTrucks.Count)
        {
            monsters.ForEach(x =>
            {
                x.StartMoving = false;
                x.gameObject.transform.position = x.MonsterInitialPos;
            });
            ScorePointCounter += cmsScore;
            ScorePoints.text = ScorePointCounter.ToString();
            CurrentActive.ForEach(x =>
            {
                x.gameObject.SetActive(false);
            });
            ItemCollectionCount.Add(DustbinCounter);
            int Attemptvalue = 0;
            Attemptvalue = DustbinCounter * CorrectPoint;
            AttemptScore.Add(Attemptvalue);
            DustbinCounter = 0;
            StartCoroutine(ShowCenterResult(truckName));
        }
        else
        {
            int Attemptvalue = 0;
            Attemptvalue = DustbinCounter * CorrectPoint;
            AttemptScore.Add(Attemptvalue);
            ScorePointCounter += cmsScore;
            ScorePoints.text = ScorePointCounter.ToString();
            ItemCollectionCount.Add(DustbinCounter);
            DustbinCounter = 0;
            Debug.Log(" total score point == " + ScorePointCounter);
            StartCoroutine(AllDoneTruck());
        }

    }

    public void Truckcentertask(int score, string truckName, string CenterName)
    {
        int cmsScore = 0;
        if (CenterName != "null")
        {
            if (score >0)
            {
                cmsScore = CenterRightpoints;
            }
            else
            {
                cmsScore = CenterWrongpoint;
            }
        }
        else
        {
            cmsScore = score;
        }
        
      

        CenterNames.Add(CenterName != "" ? CenterName : "null");
        CorrectAlignStatus.Add(score > 0 ? 1 : 0);
        Is_correctreached.Add(1);
        CenterScoreOfUser.Add(score);
        Reached_center.Add(1);
        ScorePointCounter += cmsScore;
        int Attemptvalue = 0;
        Attemptvalue = DustbinCounter * CorrectPoint;
        AttemptScore.Add(Attemptvalue);
        ScorePoints.text = ScorePointCounter.ToString();
        ItemCollectionCount.Add(DustbinCounter);
        DustbinCounter = 0;
    
        int remainingAttempt = UserAttemptnumber - ActiveTruckCount;
        for(int a=0;a< remainingAttempt; a++)
        {
            CenterNames.Add("null");
            CorrectAlignStatus.Add(0);
            Is_correctreached.Add(0);
            CenterScoreOfUser.Add(CenterWrongpoint);
            Reached_center.Add(0);
            ItemCollectionCount.Add(0);
            AttemptScore.Add(0);
        }
        Debug.Log(" total score point == " + ScorePointCounter);
        StartCoroutine(AllDoneTruck());
    }


    IEnumerator ShowCenterResult(string Truckname)
    {
        PlayGame();
        yield return new WaitForSeconds(0.5f);
        monsters.ForEach(x =>
        {
            x.targetNode = null;
            x.currentnode = null;
            x.InitialSetup();
        });
    }


    IEnumerator AllDoneTruck()
    {
        TimePaused = true;
        yield return new WaitForSeconds(0.5f);
        int totalscore = 0;
        for(int a = 0; a < Capsule.Count; a++)
        {
            totalscore += CorrectPoint;
        }

        float percentage = ((float)ScorePointCounter / (float)totalscore) * 100;
        StartCoroutine(GameScorePosting());
        StartCoroutine(PostruckDrivingGame());
       
        if (percentage > percentageScore)
        {
            Statusmsg.text = "Congratulations! you have cleared this game!!!";
            IsPassed = true;
            StatusMood.sprite = goodmood;
            GameStatuspage.SetActive(true);
        }
        else
        {
            Statusmsg.text = "Sorry ! you have failed in this game!";
            StatusMood.sprite = badmood;
            GameStatuspage.SetActive(true);
        }
    }


    public void closeStatusPage()
    {
        StartCoroutine(closeStatusBar());
    }

    IEnumerator closeStatusBar()
    {
        iTween.ScaleTo(GameStatuspage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
     
        GameStatuspage.SetActive(false);
        Statusmsg.text = "";
        if (IsPassed)
        {
            ScartchcardPage.SetActive(true);
        }
        else
        {
            GameLeaderBoard.SetActive(true);
        }
    }


    IEnumerator GameScorePosting()
    {
        yield return new WaitForSeconds(0.1f);
        string hittingUrl = $"{MainUrls.BaseUrl}{MainUrls.MasterLogApi}";
        NgageMasterPostLog PostField = new NgageMasterPostLog()
        {
            id_user = PlayerPrefs.GetInt("UID"),
            ID_ORGANIZATION = PlayerPrefs.GetInt("OID"),
            score = ScorePointCounter,
            attempt_no = GameAttemptNumber + 1,
            timetaken_to_complete = "00:00",
            is_completed = 1,
            game_type = 1,
            Id_Game = Id_game
        };

        AESAlgorithm aes = new AESAlgorithm();
        string PostLog = Newtonsoft.Json.JsonConvert.SerializeObject(PostField);

        string EncryptedData = aes.getEncryptedString(PostLog);
        CommonModel model = new CommonModel()
        {
            Data = EncryptedData
        };

        string finaldata = Newtonsoft.Json.JsonConvert.SerializeObject(model);

        Debug.Log("Master posting  " + finaldata);

        using (UnityWebRequest request = UnityWebRequest.Put(hittingUrl, finaldata))
        {
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();
            if (!request.isNetworkError && !request.isHttpError)
            {
                if(request.downloadHandler.text != "")
                {
                    Debug.Log(" response " + request.downloadHandler.text);
                }
               
            }
            else
            {
                Debug.Log("prob : " + request.error);
            }
        }


    }


    IEnumerator PostruckDrivingGame()
    {
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.TruckDrivingGamePostApi}";
        List<CovidGamePostModel> Logs = new List<CovidGamePostModel>();
       
        for(int a=0;a< 3; a++)
        {
            var CovidModel = new CovidGamePostModel()
            {
                id_truck = TruckId,
                id_user = PlayerPrefs.GetInt("UID"),
                dustbin_collected = ItemCollectionCount[a],
                truck_score = AttemptScore[a],
                reached_center = CenterNames[a],
                center_score = CenterScoreOfUser[a],
                is_correct_reached = Reached_center[a],
                attempt_no = GameAttemptNumber + 1,
                truck_name = StationaryTrucks[a].name,
                Id_Game = Id_game,
                time_count = a+1,
            };
            Logs.Add(CovidModel);
        }


        AESAlgorithm aes = new AESAlgorithm();
        string PostLog = Newtonsoft.Json.JsonConvert.SerializeObject(Logs);

        string EncryptedData = aes.getEncryptedString(PostLog);
        CommonModel model = new CommonModel()
        {
            Data = EncryptedData
        };

        string finaldata = Newtonsoft.Json.JsonConvert.SerializeObject(model);
        Debug.Log("Master posting  " + finaldata);
        using (UnityWebRequest request = UnityWebRequest.Put(HittingUrl, finaldata))
        {
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();
            if (!request.isNetworkError && !request.isHttpError)
            {
                if (request.downloadHandler.text != "")
                {
                    Debug.Log(" response " + request.downloadHandler.text);
                }
            }
            else
            {
                Debug.Log("prob : " + request.error);
            }
        }
    }

    IEnumerator GameEndProcess()
    {

        TimePaused = true;
        yield return new WaitForSeconds(0.5f);
        int length = StationaryTrucks.Count - CenterNames.Count;
        MainTrucks.ForEach(x =>
        {
            x.GetComponent<TruckPlayer>().ResetNodes();
            x.GetComponent<TruckPlayer>().NextDirection = Vector2.zero;
            x.SetActive(false);
        });
        for (int a = 0; a < length; a++)
        {
            CenterNames.Add("null");
            CorrectAlignStatus.Add(0);
            ItemCollectionCount.Add(0);
        }
        //iTween.MoveTo(TimerPanel, iTween.Hash("position", new Vector3(0f, -680f, 0f), "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.4));
        yield return new WaitForSeconds(0.5f);
        Gamedonemsg.text = "Time is Up!!!";
        avatar.sprite = badmood;
        GameDonepanel.SetActive(true);


    }
    public void ShowDashboard()
    {
        StartCoroutine(DashBoardTAsk());
    }

    IEnumerator DashBoardTAsk()
    {
        iTween.ScaleTo(GameDonepanel, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        GameDonepanel.SetActive(false);
        //MakePriorityDashborad();
    }




    //RESET MAIN METHODS FOR NEW GAME
    public void ResetAllTask()
    {
        StartCoroutine(ResetActivity());

    }

    //RESET ALL THE VARIABLLS FOR NEW GAME
    IEnumerator ResetActivity()
    {
        AllCommonTask();
        ActiveTruckCount = 0;
        MainTrucks.ForEach(x =>
        {
            x.GetComponent<TruckPlayer>().ResetNodes();
            x.GetComponent<TruckPlayer>().NextDirection = Vector2.zero;
        });
        yield return new WaitForSeconds(1f);
        AlignObj.ForEach(x =>
        {
            Destroy(x.gameObject);
        });
        PriorityObj.ForEach(x =>
        {
            Destroy(x.gameObject);
        });
        yield return new WaitForSeconds(1.5f);
        PriorityObj.Clear();
        AlignObj.Clear();
        StationaryTrucks.Clear();
        CorrectAlignStatus.Clear();
        CenterNames.Clear();
        monsters.ForEach(x =>
        {
            x.gameObject.transform.position = x.MonsterInitialPos;
            x.StartMoving = false;
            x.currentnode = null;
            x.targetNode = null;
            x.gameObject.SetActive(false);
        });
        TruckGamePage.SetActive(false);
        Camera.main.gameObject.GetComponent<AudioSource>().enabled = true;
        TruckGamePage.GetComponent<AudioSource>().enabled = false;

    }


    //FOR ANDROID DEVICE VIRBRATION EFFECT
    public void VibrateDevice()
    {
        if (!PlayerPrefs.HasKey("VibrationEnable"))
        {
            Vibration.Vibrate(400);
            Debug.Log("vibration");
        }
        else
        {
            string vibration = PlayerPrefs.GetString("VibrationEnable");

            if (vibration == "true")
            {
                Vibration.Vibrate(400);
                Debug.Log("vibration");
            }
        }
    }


    public void GoHome()
    {
        Destroy(homeinstance);
        SceneManager.LoadScene(0);
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
        iTween.ScaleTo(GameDonepanel, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        Gamedonemsg.text = "";
        GameDonepanel.SetActive(false);
        SceneManager.LoadScene(index);
    }

    public void restartGame()
    {
        Time.timeScale = 1f;
        int index = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(Hometask(index));
    }

    public void BackTask()
    {
        StartCoroutine(Backfromgame());
    }

    IEnumerator Backfromgame()
    {
        GameDonepanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0f;

    }

    public void ClosepausePage()
    {
        StartCoroutine(ClosurePausePage());
    }

    IEnumerator ClosurePausePage()
    {
        Time.timeScale = 1f;
        iTween.ScaleTo(GameDonepanel, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.4f);
        GameDonepanel.SetActive(false);
    }
}
