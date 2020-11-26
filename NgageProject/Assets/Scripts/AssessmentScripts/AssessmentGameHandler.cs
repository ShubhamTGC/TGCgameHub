using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Networking;
using LitJson;
using SimpleSQL;
using UnityEngine.SceneManagement;
using m2ostnextservice.Models;

public class AssessmentGameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public Text QuestionBar;
    public List<GameObject> Buttons;
    private int QuestionCounter = 0;
    public Sprite Correctans, WrongAns;
    public int Timercount;
    public float second;
    private float totalTimercount, RunningTimeCount;
    private bool TimeBool, helpingbool;
    [SerializeField]
    private float TimerToChnageQue;
    private string correctans = "";
    [SerializeField] private Color Normalcolor;
    [SerializeField] private Color correctcolor;
    public GameObject Coundown, Finaltestmsg;
    private List<string> UserAnswer = new List<string>();
    private List<string> UserQues  = new List<string>();
    private List<int> UserScore= new List<int>();
    private List<int> is_correct = new List<int>();
    private List<int> stageQues  = new List<int>();
    private List<int> randomindex = new List<int>();
    public Text QuestionCounterText;
    private bool changeQuest;


   
    public GameObject DataBarPrefeb;
   [SerializeField] private List<string> TotalQuestionSet = new List<string>();
    private List<int>  QuesIds = new List<int>();
    private List<string> Options = new List<string>();
    [SerializeField] private List<string> CorrectAns  = new List<string>();
    private List<string> QuestionForUser = new List<string>();
    private List<int>  QuesIdsForUser = new List<int>();
    [SerializeField] private List<string> OptionsForUser = new List<string>();
    [SerializeField] private List<string> CorrectAnsForUser = new List<string>();
    private List<int> Correctpoints = new List<int>();
    private List<int> Wrongpoints = new List<int>();
    [SerializeField] private List<int> CorrectpointsForuser = new List<int>();
    [SerializeField] private List<int> WrongpointsForUser = new List<int>();
    private int id = 1;
    private int quescounter = 1;
    [HideInInspector] public int Stage1UserScore, Stage2UserScore, Stage3UserScore;
    private List<int>  QuestionId = new List<int>();
    private List<string> AnswersID = new List<string>();
    private List<int>  QuestionIdUser = new List<int>();
    private List<string> AnswersIDUser= new List<string>();
    private List<int> UserGivenAnsId = new List<int>();
    private List<int> TotallistScore = new List<int>();
    private int UserSelectedId;
    private int GameAttemptNumber;
    public GameObject FinalPage;
    public AudioSource GameSound;
    public AudioClip CorrectAnsmusic, WrongAnsmusic;
    public SimpleSQLManager dbmanager;
    [SerializeField]private List<int> optionIndex = new List<int>();
    [SerializeField] private List<Image> Dots;
    private bool TimerBool;
    private int DotCounter = 0;
    public Text Scoretext;
    private int Score;

    public GameObject lastPage;
    public Text msgbox;
    private HomePageScript homeinstance;
    public GameObject PausePage;
    private int Id_game;
    private float Gamepercentage;
    public GameObject LeaderboardPage;
    void Start()
    {
      
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


    IEnumerator GameStarting()
    {

        yield return new WaitForSeconds(0.1f);
        TimerToChnageQue = 0f;
        QuestionCounter = 0;
        TimeBool = true;
        helpingbool = true;
        changeQuest = true;
        totalTimercount = Timercount * 60 + second;
        RunningTimeCount = totalTimercount;
        ChangeQuestion();

    }

    private void OnEnable()
    {
        homeinstance = HomePageScript.Homepage;
        QuestionBar.text = "";
        Buttons.ForEach(x =>
        {
            x.GetComponent<Button>().enabled = false;
            x.GetComponent<Image>().color = Normalcolor;
            x.GetComponent<Image>().sprite = null;
            x.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        });
        Score = 0;
        Scoretext.text = Score.ToString();
        //StartCoroutine(GetQuestionTask());
        StartCoroutine(GetlocalQuestion());
        //StartCoroutine(getAssessmentLog());
    }

    IEnumerator GetSounddata()
    {

        var SettingLog = dbmanager.Table<GameSetting>().FirstOrDefault();
        if (SettingLog != null)
        {
            GameSound.volume = SettingLog.Sound;
            PlayerPrefs.SetString("VibrationEnable", SettingLog.Vibration == 1 ? "true" : "false");
        }
        else
        {
            GameSound.volume = 1;
        }
        yield return new WaitForSeconds(0.2f);
    }


    void Update()
    {
      
    }

    void ChangesQuestionAfterTimeout()
    {
        //UserGivenAnsId.Add(0);
        UserAnswer.Add("---");
        UserQues.Add(QuestionForUser[QuestionCounter - 1]);
        Score += 0;
        Scoretext.text = Score.ToString();
        //stageQues.Add(QuesIdsForUser[QuestionCounter - 1]);
        UserScore.Add(0);
        is_correct.Add(0);
        ChangeQuestion();
    }


    void ChangeQuestion()
    {
        if (QuestionCounter < QuestionForUser.Count)
        {
            DotCounter = 0;
            Dots.ForEach(x =>
            {
                x.enabled = true;
            });
            TimerBool = true;
            StartCoroutine(Dottimer());
            QuestionBar.text = "Q"+ (QuestionCounter +1).ToString() +"." + QuestionForUser[QuestionCounter];
            string[] Ansoption = OptionsForUser[QuestionCounter].Split("@"[0]);
        
            System.Random rnd = new System.Random();
            string[] MyRandomArray = Ansoption.OrderBy(x => rnd.Next()).ToArray();

            for (int a = 0; a < MyRandomArray.Length; a++)
            {
                if (MyRandomArray[a] == CorrectAnsForUser[QuestionCounter])
                {
                    correctans = (a + 1).ToString() + "." + MyRandomArray[a];
                }
                Buttons[a].gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = (a+1).ToString()+ "."+ MyRandomArray[a];
            }
            Buttons.ForEach(x =>
            {
                x.GetComponent<Button>().enabled = true;
                x.GetComponent<Image>().color = Normalcolor;
                x.GetComponent<Image>().sprite = null;
                x.GetComponent<Button>().onClick.RemoveAllListeners();
                x.GetComponent<Button>().onClick.AddListener(delegate { UserbuttonClick(correctans,CorrectpointsForuser[QuestionCounter],WrongpointsForUser[QuestionCounter]); });
            });
           
            //QuestionCounterText.text = QuestionCounter.ToString();
        }
        else
        {
            TimeBool = false;
            UserGivenAnsRecord();
        }
    }


    public void UserbuttonClick(string Answer,int Cp,int Wp)
    {
        GameObject SelecteObj = EventSystem.current.currentSelectedGameObject;
        Debug.Log("ans" + SelecteObj.transform.GetChild(0).gameObject.GetComponent<Text>().text);
        UserScore.Add(Answer == SelecteObj.transform.GetChild(0).gameObject.GetComponent<Text>().text ? Cp : Wp);
        Score += Answer == SelecteObj.transform.GetChild(0).gameObject.GetComponent<Text>().text ? Cp : Wp;
        Scoretext.text = Score.ToString();
        is_correct.Add(Answer == SelecteObj.transform.GetChild(0).gameObject.GetComponent<Text>().text ? 1 : 0);
        for (int a = 0; a < Buttons.Count; a++)
        {
            if (Buttons[a].transform.GetChild(0).gameObject.GetComponent<Text>().text == Answer)
            {
                Buttons[a].gameObject.GetComponent<Image>().sprite = Correctans;
                Buttons[a].gameObject.GetComponent<Image>().color = correctcolor;
                GameSound.clip = CorrectAnsmusic;
                GameSound.Play();

                if (Buttons[a].name != SelecteObj.name)
                {
                    GameSound.clip = WrongAnsmusic;
                    GameSound.Play();
                    SelecteObj.GetComponent<Image>().sprite = WrongAns;
                    SelecteObj.GetComponent<Image>().color = correctcolor;
                }
            }
            Buttons[a].GetComponent<Button>().enabled = false;
        }
        QuestionCounter++;
        string Userans = SelecteObj.transform.GetChild(0).gameObject.GetComponent<Text>().text;
        UserAnswer.Add(Userans);
        UserQues.Add(QuestionForUser[QuestionCounter - 1]);
        StartCoroutine(USerInputDone());
    }
    IEnumerator USerInputDone()
    {
        TimerBool = false;
        yield return new WaitForSeconds(1f);
        TimerToChnageQue = 0;
        ChangeQuestion();
    }

    void UserGivenAnsRecord()
    {
        int totalscore = 0;
        CorrectpointsForuser.ForEach(x =>
        {
            totalscore += x;
        });
        StartCoroutine(PostAssessmentLog());
        StartCoroutine(PostMasterLog());

        float percentage = ((float)Score / (float)totalscore) * 100;
        if(percentage >= Gamepercentage)
        {
            string msg = "Congratulation! You have cleared this game.";
            StartCoroutine(ShowPopuppage(msg));
        }
        else
        {
            string msg = "Sorry! You have failed in this game.";
            StartCoroutine(ShowPopuppage(msg));
        }
    }

    IEnumerator ShowPopuppage(string msg)
    {
        lastPage.SetActive(true);
        msgbox.text = msg;
        yield return new WaitForSeconds(0.1f);
    }

    public void closegameStatus()
    {
        StartCoroutine(closeStatustask());
    }

    IEnumerator closeStatustask()
    {
        iTween.ScaleTo(lastPage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        lastPage.SetActive(false);
        msgbox.text = "";
        LeaderboardPage.SetActive(true);
    }


    IEnumerator GetlocalQuestion()
    {
        yield return new WaitForSeconds(0.1f);
        var localLog = dbmanager.Table<QuizGameList>().ToList();
        if(localLog != null)
        {
            localLog.ForEach(x =>
            {
                TotalQuestionSet.Add(x.Question);
                QuestionId.Add(x.QuizId);
                string options = x.Option1 + "@" + x.Option2 + "@" + x.Option3 + "@" + x.Option4 ;
                Options.Add(options);
                CorrectAns.Add(x.CorrectOption);
                Correctpoints.Add(x.CorrectPoint);
                Wrongpoints.Add(x.WrongPoint);
                Id_game = x.GameId;
            });

            int maxLength = 0;
            while (maxLength < TotalQuestionSet.Count)
            {
                int num = UnityEngine.Random.Range(0, TotalQuestionSet.Count);
                if (!randomindex.Contains(num))
                {
                    randomindex.Add(num);
                    maxLength++;
                }
            }

            for (int a = 0; a < randomindex.Count; a++)
            {
                string tempque = TotalQuestionSet[randomindex[a]];
                string tempoption = Options[randomindex[a]];
                string tempcorrectans = CorrectAns[randomindex[a]];
                int tempqueID = QuestionId[randomindex[a]];
                int tempCp = Correctpoints[randomindex[a]];
                Correctpoints[randomindex[a]] = Correctpoints[a];
                Correctpoints[a] = tempCp;
                int tempWp = Wrongpoints[randomindex[a]];
                Wrongpoints[randomindex[a]] = Wrongpoints[a];
                Wrongpoints[a] = tempWp;
                TotalQuestionSet[randomindex[a]] = TotalQuestionSet[a];
                TotalQuestionSet[a] = tempque;
                Options[randomindex[a]] = Options[a];
                Options[a] = tempoption;
                CorrectAns[randomindex[a]] = CorrectAns[a];
                CorrectAns[a] = tempcorrectans;
                QuestionId[randomindex[a]] = QuestionId[a];
                QuestionId[a] = tempqueID;
            }

            for (int b = 0; b < 10; b++)
            {

                QuestionForUser.Add(TotalQuestionSet[b]);
                OptionsForUser.Add(Options[b]);
                CorrectAnsForUser.Add(CorrectAns[b]);
                QuestionIdUser.Add(QuestionId[b]);
                CorrectpointsForuser.Add(Correctpoints[b]);
                WrongpointsForUser.Add(Wrongpoints[b]);

            }
            Gamepercentage = dbmanager.Table<GameListDetails>().FirstOrDefault(x => x.GameId == Id_game).CompletePer;
            StartCoroutine(GetGameAttemptNoTask());
            StartCoroutine(GameStarting());
        }
        else
        {
            Debug.Log("null");
        }
    }


    IEnumerator PostAssessmentLog()
    {
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.QuizGamePostApi}";
   
        var Logs = new List<QuizGamePostModel>();
        int l = 0;
        UserQues.ForEach(x =>
        {
            var log = new QuizGamePostModel()
            {
                id_user = PlayerPrefs.GetInt("UID"),
                is_right = is_correct[l],
                attempt_no = GameAttemptNumber + 1,
                Id_Game = Id_game,
                id_question = QuestionIdUser[l],
                user_Ans = UserAnswer[l],
                score = UserScore[l]
            };
            l = l + 1;
            Logs.Add(log);
        });


        AESAlgorithm aes = new AESAlgorithm();
        string PostLog = Newtonsoft.Json.JsonConvert.SerializeObject(Logs);

        string EncryptedData = aes.getEncryptedString(PostLog);
        CommonModel model = new CommonModel()
        {
            Data = EncryptedData
        };

        string finaldata = Newtonsoft.Json.JsonConvert.SerializeObject(model);

        Debug.Log("data log for final assessment " + finaldata);
        using (UnityWebRequest request = UnityWebRequest.Put(HittingUrl, finaldata))
        {
            Debug.Log(request);
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();
            if (!request.isNetworkError && !request.isHttpError)
            {
                Debug.Log(request.downloadHandler.text);
              
            }
            else
            {
                Debug.Log("prob : " + request.error);
            }
        }

    }



    IEnumerator PostMasterLog()
    {
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.MasterLogApi}";
        NgageMasterPostLog PostField = new NgageMasterPostLog()
        {
            id_user = PlayerPrefs.GetInt("UID"),
            ID_ORGANIZATION = PlayerPrefs.GetInt("OID"),
            score = Score,
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
        Debug.Log("master data log " + finaldata);
        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, finaldata))
        {
            Request.method = UnityWebRequest.kHttpVerbPOST;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                Debug.Log(Request.downloadHandler.text);
            }
            else
            {
                Debug.Log("Error "+Request.downloadHandler.text);
            }
        }
    }


    public void CloseAssessment()
    {
        StartCoroutine(ClearAssessment());
    }

    IEnumerator ClearAssessment()
    {
        UserAnswer.Clear();
        UserQues.Clear();
        UserScore.Clear();
        is_correct.Clear();
        stageQues.Clear();
        randomindex.Clear();
        TotalQuestionSet.Clear();
        QuesIds.Clear();
        Options.Clear();
        CorrectAns.Clear();
        QuestionForUser.Clear();
        QuesIdsForUser.Clear();
        OptionsForUser.Clear();
        CorrectAnsForUser.Clear();
        QuestionId.Clear();
        AnswersID.Clear();
        QuestionIdUser.Clear();
        AnswersIDUser.Clear();
        UserGivenAnsId.Clear();
        TotallistScore.Clear();
        yield return new WaitForSeconds(1f);
        FinalPage.SetActive(true);
        this.gameObject.SetActive(false);
    }

    IEnumerator Dottimer()
    {
        yield return new WaitForSecondsRealtime(1);
        if (TimerBool)
        {
            Dots[DotCounter].enabled = false;
            if(QuestionCounter < QuestionForUser.Count)
            {
                if(DotCounter == Dots.Count-1)
                {
                    QuestionCounter++;
                    ChangesQuestionAfterTimeout();
                }
                else
                {

                    DotCounter++;
                    StartCoroutine(Dottimer());
                }
            }
            else
            {
                UserGivenAnsRecord();
            }
        }
    }

    public void GotoHome()
    {
        int index = 0;
        Destroy(homeinstance);
        StartCoroutine(Hometask(index));
    }
    IEnumerator Hometask(int index)
    {
        iTween.ScaleTo(lastPage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        msgbox.text = "";
        lastPage.SetActive(false);
        SceneManager.LoadScene(index);
    }

    public void restartGame()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(Hometask(index));
    }

    public void BackAction()
    {
        TimerBool = false;
        PausePage.SetActive(true);
    }

    public void ClosePausepage()
    {
        StartCoroutine(CloserOfPausePage());
    }

    IEnumerator CloserOfPausePage()
    {
        iTween.ScaleTo(PausePage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        PausePage.SetActive(false);
        TimerBool = true;
        StartCoroutine(Dottimer());
    }
}
