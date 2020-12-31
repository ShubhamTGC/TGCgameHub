using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;
using LitJson;
using UnityEngine.SceneManagement;
using SimpleSQL;
using m2ostnextservice.Models;

public class AnagrameController : MonoBehaviour
{
   
    [SerializeField]private string[] Answordlist;
    private string[] Questionlist;
    private int[] correctPoints,wrongpoints;
    public Sprite[] AnswerSprite;
    public Text Currentquestion;
    public Image HintImage;
    private int CurrentWordCount =0;
    private string AnswerWord;
    private string[] WordSplits;
    public GameObject WordPrefebs,DashPrefeb;
    public Transform WordPrenet, DashPrenet;
    private List<GameObject> Words = new List<GameObject>();
    private List<GameObject> Dashs = new List<GameObject>();
    private string[] CorrectAns;
    [SerializeField] private string[] SelectedWord;
    private List<string> AnsStatus = new List<string>();
    private List<string> UserGivenWord = new List<string>();
    private int[] OriginalIds;
    private int[] id_word;
    private int SelectionCounter = 0;
    private int CorrectAnsCounter;
    private int RunningWordCount;
    [Header("timer section")]
    [Space(10)]
    public Text Timer;
    public Image Timerbar;
    public float minute, second;
    private float sec, Totaltimer, RunningTimer;
    private bool helpingbool = true;
    private bool WrongGuess = true;
    private float wrongGuessTimer;
    private bool Timepaused = true;
    public GameObject GameoverObj;
    public Text ScoreText;
    public Text QuestionNumber;
    private int score;
    public GameObject BlastEffect;
    //============ TEMP WORKING VARIABLES===========================//
    private List<int> randomindex = new List<int>();
    private List<int> scoreList = new List<int>();
    [SerializeField]
    private AudioSource AudioObject;
    public AudioClip CorrectSound, WrongSound;

    public string MainUrl, AnagramApi,PostDataApi,UserScorePosting, GetlevelWisedataApi;
    private AnagramPostData anagramePostdata;
    private ZoneShowHandler ZoneHandlers;
    public int Gamelevel;
   [SerializeField] private int GameAttemptNumber;
    public GameObject Pausepage;
    public Text msgbox;
    public SimpleSQLManager dbmanager;
    [SerializeField] private int Timecounter;
    private HomePageScript homeinstance;
    public GameObject PausePage;
    public Image Avatarmood;
    public Sprite Happy, sad;
    private int Id_game,Id_room;
    private float percentageScore;
    public GameObject leaderboardPage;
    //public GameObject Loadingbar;
    //public CustomLoader Loading;
    private bool IsPassed;
    public GameObject ScartchcardPage;
    public GameObject WinText, LoseText;
    public GameObject ScratchCard, CardResult;
    private int CardIDWin,PostCardID;
    private int CardScore;
    public ScratchCardEffect cardEffectpage;
    public GameObject cardPanelpage;
    void Start()
    {


    }

    private void OnEnable()
    {
        IsPassed = false;
        homeinstance = HomePageScript.Homepage;
        ZoneHandlers = new ZoneShowHandler();
       
        sec = second;
        Timepaused = true;
        Timer.text = "00:" + Timecounter;
        Totaltimer = (minute * 60) + second;
        RunningTimer = Totaltimer;
        score = 0;
        ScoreText.text = score.ToString();
       // Loadingbar.SetActive(true);
        StartCoroutine(GetAnagramedata());
        
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

    void Mainsetup()
    {
        
        int maxLength = 0;
        while (maxLength < Answordlist.Length)
        {
            int num = UnityEngine.Random.Range(0, Answordlist.Length );
            if (!randomindex.Contains(num))
            {
                randomindex.Add(num);
                maxLength++;
            }
        }
        Debug.Log("total length " + randomindex.Count);
        id_word = new int[Answordlist.Length];
        for (int a = 0; a < randomindex.Count; a++)
        {
            int num = randomindex[a];
            Debug.Log(num);
            id_word[a] = OriginalIds[num];
            string temp = Answordlist[a];
            string temp2 = Questionlist[a];
            Sprite tempsprite = AnswerSprite[a];
            Questionlist[a] = Questionlist[num];
            Answordlist[a] = Answordlist[num];
            AnswerSprite[a] = AnswerSprite[num];
            AnswerSprite[num ] = tempsprite;
            Answordlist[num ] = temp;
            Questionlist[num ] = temp2;
            int tempCp = correctPoints[num];
            correctPoints[num] = correctPoints[a];
            correctPoints[a] = tempCp;
            int tempWp = wrongpoints[num];
            wrongpoints[num] = wrongpoints[a];
            wrongpoints[a] = tempWp;
            int tempid = OriginalIds[a];
            OriginalIds[a] = OriginalIds[num];
            OriginalIds[num] = tempid;

        }
        GameSetup(CurrentWordCount);
    }

    IEnumerator GetAnagramedata()
    {

        yield return new WaitForSeconds(0.1f);
        int arrayindex = 0;
        var LocalLog = dbmanager.Table<AnagramGameList>().ToList();
      
        correctPoints = new int[LocalLog.Count];
        wrongpoints = new int[LocalLog.Count];
        Answordlist = new string[LocalLog.Count];
        Questionlist = new string[LocalLog.Count];
        OriginalIds = new int[LocalLog.Count];
        if (LocalLog != null)
        {
            LocalLog.ForEach(x =>
            {
                Questionlist[arrayindex] = x.Question;
                Answordlist[arrayindex] = x.Answer.Trim();
                OriginalIds[arrayindex] = x.IdWord;
                correctPoints[arrayindex] = x.CorrectPoint;
                wrongpoints[arrayindex] = x.WrongPoint;
                Id_game = x.GameId;
                Id_room = 0;
                arrayindex++;
            });
        }
        percentageScore = dbmanager.Table<GameListDetails>().FirstOrDefault(x => x.GameId == Id_game).CompletePer;
        StartCoroutine(GetGameAttemptNoTask());
        //Loading.Isdone = true;
        Mainsetup();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    void GameSetup(int WordNumber)
    {
        //this will split the word into letters
        Timecounter =15;
        Timepaused = true;
        if (Timecounter.ToString().Length > 1)
        {
            Timer.text = "00:" + Timecounter.ToString();
        }
        else
        {
            Timer.text = "00:" + "0" + Timecounter.ToString();
        }
        StartCoroutine(TimerCounter());
        AnswerWord = Answordlist[WordNumber];
        Currentquestion.text = "";
        Currentquestion.text = Questionlist[WordNumber];
        QuestionNumber.text = "Q" + (WordNumber + 1).ToString() + "/" + Questionlist.Length.ToString();
        for(int c = 0; c < AnswerSprite.Length; c++)
        {
            if (AnswerSprite[c].name.Equals(OriginalIds[WordNumber].ToString(), System.StringComparison.OrdinalIgnoreCase))
            {
                HintImage.sprite = AnswerSprite[c];
            }
        }
        
        WordSplits = new string[AnswerWord.Length];
        CorrectAns = new string[AnswerWord.Length];
        SelectedWord = new string[AnswerWord.Length];
        for (int a = 0; a < AnswerWord.Length; a++)
        {
            WordSplits[a] = Convert.ToString(AnswerWord[a]);
            CorrectAns[a] = WordSplits[a];

        }

        //Randomize the letters
        for(int b =0;b< WordSplits.Length; b++)
        {
            string temp = WordSplits[b];
            int Randomindex = UnityEngine.Random.Range(1, WordSplits.Length);
            WordSplits[b] = WordSplits[Randomindex];
            WordSplits[Randomindex] = temp;
        }

        for(int c =0;c < WordSplits.Length; c++)
        {
            GameObject _wordgb = Instantiate(WordPrefebs, WordPrenet, false);
            GameObject _dashgb = Instantiate(DashPrefeb, DashPrenet, false);
            _wordgb.transform.GetChild(0).GetComponent<Text>().text = WordSplits[c];
            _wordgb.GetComponent<Button>().onClick.AddListener(delegate { WordButtonMethod(); });
            Words.Add(_wordgb);
            Dashs.Add(_dashgb);

        }

    }

    public void WordButtonMethod()
    {
        GameObject selectedobj = EventSystem.current.currentSelectedGameObject;
        string word = selectedobj.transform.GetChild(0).GetComponent<Text>().text;
        selectedobj.GetComponent<Button>().enabled = false;
        selectedobj.GetComponent<Image>().enabled = false;
        Dashs[SelectionCounter].transform.GetChild(0).GetComponent<Text>().text = word;
        Dashs[SelectionCounter].GetComponent<Button>().onClick.RemoveAllListeners();
        Dashs[SelectionCounter].GetComponent<Button>().onClick.AddListener(delegate { CorrectionTask(selectedobj); });
        selectedobj.transform.GetChild(0).GetComponent<Text>().text = "";
        SelectedWord[SelectionCounter] = word;
        SelectionCounter++;
        selectedobj.GetComponent<Button>().enabled = false;
        if(SelectionCounter == Dashs.Count)
        {
            StartCoroutine(CheckForAns());
        }
    }
    
    public void CorrectionTask(GameObject selectedbtn)
    {
        GameObject selectedobj = EventSystem.current.currentSelectedGameObject;
        string word = selectedobj.transform.GetChild(0).gameObject.GetComponent<Text>().text;
        selectedobj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        selectedbtn.transform.GetChild(0).gameObject.GetComponent<Text>().text = word;
        SelectionCounter--;
        SelectedWord[SelectionCounter] = "";
        selectedbtn.GetComponent<Button>().enabled = true;
        selectedbtn.GetComponent<Image>().enabled = true;
        selectedobj.GetComponent<Button>().onClick.RemoveAllListeners();
    }


    
    IEnumerator CheckForAns()
    {
        yield return new WaitForSeconds(1f);
        Timepaused = false;
        for (int a = 0; a < CorrectAns.Length; a++)
        {
            if(CorrectAns[a] == SelectedWord[a])
            {
                CorrectAnsCounter++;
            }
        }
        if(CorrectAnsCounter == CorrectAns.Length)
        {
     
            wrongGuessTimer = 0;
            score += correctPoints[CurrentWordCount];
            ScoreText.text = score.ToString();
            AnsStatus.Add("1");
            scoreList.Add(correctPoints[CurrentWordCount]);
            AudioObject.clip = CorrectSound;
            StartCoroutine(ResetAnagramFields());
        }
        else
        {
            Debug.Log("wrong ans");
            // WrongGuess = true;
            AnsStatus.Add("0");
            BlastEffect.SetActive(true);
            wrongGuessTimer = 0;
            score += wrongpoints[CurrentWordCount];
            scoreList.Add(wrongpoints[CurrentWordCount]);
            ScoreText.text = score.ToString();
            AudioObject.clip = WrongSound;
            StartCoroutine(ResetAnagramFields());
        }
     
    }
    IEnumerator ResetAnagramFields()
    {

        if(SelectedWord.Length > 0)
        {
            string UserWord = string.Join("", SelectedWord);
            UserGivenWord.Add(UserWord);
        }
        else
        {
            UserGivenWord.Add("null");
        }
       
        for (int a = 0; a < Words.Count; a++)
        {
            DestroyImmediate(Words[a].gameObject);
            DestroyImmediate(Dashs[a].gameObject);
        }
        AudioObject.Play();
        SelectionCounter = 0;
        Words.Clear();
        Dashs.Clear();
        Array.Clear(CorrectAns, 0, CorrectAns.Length);
        Array.Clear(WordSplits, 0, WordSplits.Length);
        Array.Clear(SelectedWord, 0, SelectedWord.Length);
        CorrectAnsCounter = 0;
        yield return new WaitForSeconds(0.6f);
        iTween.ScaleTo(BlastEffect, Vector3.zero, 0.2f);
        yield return new WaitForSeconds(0.4f);
        BlastEffect.SetActive(false);
        CurrentWordCount++;
        WrongGuess = true;
        if (CurrentWordCount < Answordlist.Length)
        {
            GameSetup(CurrentWordCount);
        }
        else
        {
            GameScoreCheck();
         
        }
        
        
    }

    void GameScoreCheck()
    {
        int totalScore = 0;
        for(int a = 0; a < correctPoints.Length; a++)
        {
            totalScore += correctPoints[a];
        }

        float percentage = ((float)score / (float)totalScore) * 100;
        StartCoroutine(GameScorePosting());
        StartCoroutine(PostGameData());
        if (percentage >= percentageScore)
        {
            string msg = "Congratulatuons! \n you have cleared this Game!!!";
            IsPassed = true;
            StartCoroutine(GameoverPage(msg,Happy));
        }
        else
        {
            IsPassed = false;
            string msg = "Sorry \n you have failed in this Game!!!";
            StartCoroutine(GameoverPage(msg,sad));
        }
    }



    public void closeLastPage()
    {
        StartCoroutine(CloseGameStatusPage());
    }

    IEnumerator CloseGameStatusPage()
    {
        iTween.ScaleTo(Pausepage, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.4f);
        PausePage.SetActive(false);
        msgbox.text = "";
        if (IsPassed)
        {
            ScartchcardPage.SetActive(true);
        }
        else
        {
            leaderboardPage.SetActive(true);
        }
    }

 

    IEnumerator GameoverPage(string msg,Sprite mood)
    {
        //StartCoroutine(GameScorePosting());
        yield return new WaitForSeconds(0.5f);
        for (int a = 0; a < Words.Count; a++)
        {
            DestroyImmediate(Words[a].gameObject);
            DestroyImmediate(Dashs[a].gameObject);
        }
        msgbox.text = msg;
        Avatarmood.sprite = mood;
        Pausepage.SetActive(true);
        SelectionCounter = 0;
        CorrectAnsCounter = 0;
        Words.Clear();
        Dashs.Clear();
        WrongGuess = false;
        Timepaused = false;
        
        

    }
    IEnumerator GameScorePosting()
    {
        yield return new WaitForSeconds(0.1f);
        string hittingUrl = $"{MainUrls.BaseUrl}{MainUrls.MasterLogApi}";
        NgageMasterPostLog PostField = new NgageMasterPostLog()
        {
            id_user = PlayerPrefs.GetInt("UID"),
            ID_ORGANIZATION = PlayerPrefs.GetInt("OID"),
            score = score,
            attempt_no = GameAttemptNumber +1,
            timetaken_to_complete = "00:00",
            is_completed =1,
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

    IEnumerator PostGameData()
    {
        AESAlgorithm aes = new AESAlgorithm();
        var logs = new List<NgageAnagramPostModel>();
        int a = 0;
        if (UserGivenWord.Count > 0)
        {
            UserGivenWord.ForEach(x =>
            {
                var log = new NgageAnagramPostModel()
                {
                    id_word = OriginalIds[a],
                    is_right = int.Parse(AnsStatus[a]),
                    user_input = UserGivenWord[a],
                    id_user = PlayerPrefs.GetInt("UID"),
                    Id_Game = Id_game,
                    score = scoreList[a],
                    attempt_no = GameAttemptNumber + 1
                };
                a = a + 1;
                logs.Add(log);
            });
           
        }
        string PostLog = Newtonsoft.Json.JsonConvert.SerializeObject(logs);
        string Encrypteddata = aes.getEncryptedString(PostLog);
        CommonModel model = new CommonModel()
        {
            Data = Encrypteddata
        };

        string FinalLog = Newtonsoft.Json.JsonConvert.SerializeObject(model);
        Debug.Log("game log " + FinalLog);
        string HIttingUrl = $"{MainUrls.BaseUrl}{MainUrls.AnagramPostApi}";
        yield return new WaitForSeconds(0.1f);
        using (UnityWebRequest request = UnityWebRequest.Put(HIttingUrl, FinalLog))
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


    public void GotoHome()
    {
        int index = 0;
        Destroy(homeinstance);
        StartCoroutine(Hometask(index));
    }
    IEnumerator Hometask(int index)
    {
        iTween.ScaleTo(Pausepage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        msgbox.text = "";
        Pausepage.SetActive(false);
        SceneManager.LoadScene(index);
    }

    public void restartGame()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(Hometask(index));
    }

    //TIME DOWN COUNTER COROUTINE
    IEnumerator TimerCounter()
    {
        yield return new WaitForSecondsRealtime(1);
        if (Timepaused)
        {
            if (Timecounter == 0)
            {
               
                if (CurrentWordCount < Questionlist.Length)
                {
                    if (Timecounter.ToString().Length > 1)
                    {
                        Timer.text = "00:" + Timecounter.ToString();
                    }
                    else
                    {
                        Timer.text = "00:" + "0" + Timecounter.ToString();
                    }
                    StartCoroutine(CheckForAns());
                }
                else
                {
                    if (Timecounter.ToString().Length > 1)
                    {
                        Timer.text = "00:" + Timecounter.ToString();
                    }
                    else
                    {
                        Timer.text = "00:" + "0" + Timecounter.ToString();
                    }
                    GameScoreCheck();
                }
            }
            else
            {
                 Timecounter--;
                if (Timecounter.ToString().Length > 1)
                {
                    Timer.text = "00:" + Timecounter.ToString();
                }
                else
                {
                    Timer.text = "00:" + "0" + Timecounter.ToString();
                }
                StartCoroutine(TimerCounter());
            }
        }
    }


    public void BackAction()
    {
        Timepaused = false;
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
        Timepaused = true;
        StartCoroutine(TimerCounter());
    }

    public void AfterCardPostingtask()
    {
        StartCoroutine(CloseCardpageTask());
    }

    IEnumerator CloseCardpageTask()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(CardResult);
        yield return new WaitForSeconds(0.5f);
        cardPanelpage.SetActive(false);
        leaderboardPage.SetActive(true);
    }



}
