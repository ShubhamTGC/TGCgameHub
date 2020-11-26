using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using SimpleSQL;
using System.Linq;
using UnityEngine.UI;

public class Stage3handler : MonoBehaviour
{
    public GameObject TriviaPage, OnbordingVideoPage, SkipButton, AvatarPage, Stage3LandingPage, Gamemanager, GamePoints;
    private bool videoPlayed, checkforEnd;
    private bool EndvideoBool, Endvideocheck;
    public Text username;
    [Header("API INTEGRATION PART")]
    public string MainUrl;
    public string StageUnlockApi, GetTruckCenterApi, GetMonsterCmsApi, TruckSeqApi, GetDustbinScoreApi;
    public int Gamelevel;
    public bool BonusGameBool;
    public int GameScore;


    public GameObject FinalMsgPage, AssessmentCanvas;

    [Header("Game Closure Video")]
    public GameObject EndVideo;
    public GameObject EndvideoSkip, showvideobtn,Okbtn,FinalTrivia;
    public GameBoard MainBoard;
    public GameObject debrifingbtn;
    public GameBoard GameBoardpage;
    private int TotalScoreOfGame;
    public SimpleSQLManager dbmanager;
    public YoutubePlayer.YoutubePlayer YoutubeVideoPage,FinalVideoPage;


    void Start()
    {
     
    }

     void OnEnable()
    {
        StartCoroutine(GetYoutubeLink());
        StartCoroutine(GetTruckCenterData());
        StartCoroutine(GetMonsterdata());
    }

    IEnumerator GetYoutubeLink()
    {
        yield return new WaitForSeconds(0.1f);
        var LocalLog = dbmanager.Table<VideoUrls>().FirstOrDefault(x => x.LevelId == 3);
        var finalLog = dbmanager.Table<VideoUrls>().FirstOrDefault(x => x.LevelId == 5);
        if (LocalLog != null)
        {
            YoutubeVideoPage.youtubeUrl = LocalLog.VideoLink;
        }
        if(finalLog != null)
        {
            FinalVideoPage.youtubeUrl = finalLog.VideoLink;
        }
        OnbordingVideoPage.SetActive(true);
        Camera.main.gameObject.GetComponent<AudioSource>().enabled = false;
        TriviaPage.SetActive(true);
        videoPlayed = true;
    }

  
    void Update()
    {
        if (videoPlayed)
        {
            if (OnbordingVideoPage.GetComponent<VideoPlayer>().isPlaying)
            {
                SkipButton.SetActive(true);
                checkforEnd = true;
            }
            if (checkforEnd)
            {
                if (!OnbordingVideoPage.GetComponent<VideoPlayer>().isPlaying)
                {
                    videoPlayed = false;
                    checkforEnd = false;
                    SkipVideo();
                }
            }
        }
        if (EndvideoBool)
        {
            if (EndVideo.GetComponent<VideoPlayer>().isPlaying)
            {
                EndvideoSkip.SetActive(true);
                Endvideocheck = true;
            }
            if (Endvideocheck)
            {
                if (!EndVideo.GetComponent<VideoPlayer>().isPlaying)
                {
                    EndvideoBool = false;
                    Endvideocheck = false;
                    SkipEndVideo();
                }
            }
        }
    }
    public void SkipVideo()
    {
        
        SkipButton.SetActive(false);
        TriviaPage.SetActive(false);
        OnbordingVideoPage.SetActive(false);
        Camera.main.gameObject.GetComponent<AudioSource>().enabled = true;
        videoPlayed = checkforEnd = false;
        int avatar_data = PlayerPrefs.GetInt("characterType");
        if (avatar_data > 4)
        {
            StartCoroutine(ClosingTask());
        }
        else
        {
            username.text = PlayerPrefs.GetString("username");
            AvatarPage.SetActive(true);
        }


    }


    public void BacktoHome()
    {
        SceneManager.LoadScene(0);
    }

    public void CloseAvatarPage()
    {
        StartCoroutine(ClosingTask());
    }

    IEnumerator ClosingTask()
    {
        iTween.ScaleTo(AvatarPage, Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.5f);
        username.text = "";
        Stage3LandingPage.SetActive(true);

    }


    IEnumerator GetStageScore()
    {
        
        string Hitting_url = $"{MainUrl}{StageUnlockApi}?UID={PlayerPrefs.GetInt("UID")}&id_level={Gamelevel}&id_org_game={1}";
        WWW StageData = new WWW(Hitting_url);
        yield return StageData;
        if (StageData.text != null)
        {
            StageUnlockModel StageModel = Newtonsoft.Json.JsonConvert.DeserializeObject<StageUnlockModel>(StageData.text);
            BonusGameBool = int.Parse(StageModel.ConsolidatedScore) > GameScore;
            debrifingbtn.SetActive(BonusGameBool);
           
        }
    }

    void CalculatePercentage()
    {
        var log = dbmanager.Table<LevelPercentageTable>().FirstOrDefault(x => x.LevelId == 3).LevelPercentage;
        float percentage = ((float)TotalScoreOfGame / 100) * log;
        //GameBoardpage.Stage3UnlockScore = (int)percentage;
        GameScore = (int)percentage;
        var Configdata = dbmanager.Table<ScoreConfiguration>().FirstOrDefault(y => y.levelId == 3);
        if(Configdata == null)
        {
            ScoreConfiguration scorelog = new ScoreConfiguration
            {
                levelId = 3,
                PercentScore = log,
                TotalScore = TotalScoreOfGame,
                UnlockScore = (int)percentage
            };
            dbmanager.Insert(scorelog);
        }
        else
        {
            Configdata.levelId = 3;
            Configdata.PercentScore = log;
            Configdata.TotalScore = TotalScoreOfGame;
            Configdata.UnlockScore = (int)percentage;
            dbmanager.UpdateTable(Configdata);
        }
        StartCoroutine(GetStageScore());
    }

    public void FinalPAgeClose()
    {
        StartCoroutine(ClosePage());
    }
    IEnumerator ClosePage()
    {
        iTween.ScaleTo(FinalMsgPage, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.4f);
        FinalMsgPage.SetActive(false);
        AssessmentCanvas.SetActive(false);
        Stage3LandingPage.SetActive(true);
    }

    public void PlayEndvideo()
    {
        EndVideo.SetActive(true);
        Camera.main.gameObject.GetComponent<AudioSource>().enabled = false;
        FinalTrivia.SetActive(true);
        EndvideoBool = true;
    }

    public void SkipEndVideo()
    {
        EndvideoSkip.SetActive(false);
        FinalTrivia.SetActive(false);
        EndVideo.SetActive(false);
        showvideobtn.SetActive(false);
        Okbtn.SetActive(true);
        Camera.main.gameObject.GetComponent<AudioSource>().enabled = true;
        EndvideoBool = Endvideocheck = false;
    }

    IEnumerator getTruckSeq()
    {
        GameBoardpage.UserSelectedId.Clear();
       // GameBoardpage.TrucksPriority.Clear();
        GameBoardpage.StationaryTrucks.Clear();
        GameBoardpage.TruckSequence.Clear();
        GameBoardpage.TruckID.Clear();
        string HittingUrl = MainUrl + TruckSeqApi + "?UID=" + PlayerPrefs.GetInt("UID");
        WWW Request = new WWW(HittingUrl);
        yield return Request;
        if (Request.text != null)
        {
            List<TruckSeqModel> Truckdata = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TruckSeqModel>>(Request.text);
            Truckdata.ForEach(x =>
            {
                GameBoardpage.TruckSequence.Add(x.truck_name);
                GameBoardpage.TruckID.Add(x.id_truck);
                GameBoardpage.CorrectPoint = x.correct_priority_point;
                GameBoardpage.WrongPoint = x.wrong_point;
                TotalScoreOfGame += x.correct_priority_point;
            });

            StartCoroutine(GetDustbinsScore());
        }
    }

    IEnumerator GetTruckCenterData()
    {
        MainBoard.centernames.Clear();
        MainBoard.CenterCorrectPoint.Clear();
        MainBoard.CenterWrongPoint.Clear();
        string HittingUrl = $"{MainUrl}{GetTruckCenterApi}";
        WWW Request = new WWW(HittingUrl);
        yield return Request;
        if(Request.text != null)
        {
            if(Request.text != "[]")
            {
                List<TruckCenterCmsModel> CenterLog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TruckCenterCmsModel>>(Request.text);
                CenterLog.ForEach(x =>
                {
                    MainBoard.centernames.Add(x.destination_name);
                    MainBoard.CenterCorrectPoint.Add(x.correct_bonus_point);
                    MainBoard.CenterWrongPoint.Add(x.wrong_point);
                    TotalScoreOfGame += x.correct_bonus_point;
                });

                StartCoroutine(getTruckSeq());
            }
        }
    }

    IEnumerator GetMonsterdata()
    {
        string HittingUrl = $"{MainUrl}{GetMonsterCmsApi}";
        WWW request = new WWW(HittingUrl);
        yield return request;
        if(request.text != null)
        {
            if(request.text != "[]")
            {
                List<MosterAttackModel> monsterlog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MosterAttackModel>>(request.text);
                monsterlog.ForEach(x =>
                {
                    if (x.monsterId == 1)
                    {
                        MainBoard.monsterAttackScore = x.catch_point;
                    }
                });
            }
        }
    }

    IEnumerator GetDustbinsScore()
    {
        string HittingUrl = $"{MainUrl}{GetDustbinScoreApi}?UID={PlayerPrefs.GetInt("UID")}";
        WWW request = new WWW(HittingUrl);
        yield return request;
        if(request.text != null)
        {
            if(request.text != "[]")
            {
                List<TruckDrivingCMSModel> trucklog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TruckDrivingCMSModel>>(request.text);
                trucklog.ForEach(x =>
                {
                    TotalScoreOfGame += x.Correct_Dustbin_Point;
                });
                CalculatePercentage();
            }
        }
    }
}
