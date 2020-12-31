using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Networking;
using LitJson;
using UnityEngine.SceneManagement;
using m2ostnextservice.Models;
using SimpleSQL;
public class MatchTheTile : MonoBehaviour
{
    public GameObject ButtonPrefeb;
    public Transform TileParent;
    public int TileCount, wasteCount;
    public Sprite DefaultSprite,WrongSprite,RightSprite, BlankTile,WrongbgSprite,RightbgSprite,Defaultobj;
    public Sprite[] DustbinSprite, WasteSprite;
    [SerializeField]
    private List<Sprite> GeneratedSprite = new List<Sprite>();
    private List<GameObject> GeneratedTiles = new List<GameObject>();
    public string DustbinPath, WastePath;

    //========== TILE GAME MAIN LOGIC VARIABLES================
    private bool FirstGuess, SecondGuess;
    private GameObject FirstSelectedobj, SecondSelectedobj;
    private string FirstGuessName, SecondGuessName;
    private Sprite FirstObjSprite, SecondObjSprite;
    private int CorrectGuess;
    public List<string> DustbinNames;

  
    //==========TIMER AND SCORE PANEL DATRA ==========================
    public Text TotalTils, CorrectTiles;
    public int Timercount;
    public float second;
    private float totalTimercount, RunningTimeCount;
    public Text TimerText;
    public Image TimerImage;
    private Coloreffect colorglow;
    private bool helpingbool = true;
    public GameObject TimesUp;
    //======================== SCORE CAPTURING ELEMENTS ==================
    public GameObject ScoreKnob;
    private float knobAngle;
    private bool ScoreCheckNow = false;
    [SerializeField]
    private float BonusGameScore;
    public Text ScoreText;
    private int ScoreConuter;
    public GameObject ZoneA, ZoneB;
    public string MainUrl, UserScorePosting, GetlevelWisedataApi;
    private PostScoreInMasterTable PostMasterTable;
    public GameObject SCorePanel;
    public Stage2SceneOpenup Stage2objects;
    public GameObject Stage2LeaderBaord;
    public int Gamelevel;
    private int GameAttemptNumber;
    public GameObject WinPage,MainPage;
    private bool TimeBool = true;

    // DROP DOWN INFO OBJECTS
  
    [SerializeField] private Vector3 targetPos;
    private Vector3 startingpos;
    public GameObject Hiddenpanel;
    public Image Circulartimer;
    public GameObject PopupPage;
    public Text msgbox;
    public Image Avatarmood;
    public Sprite happy, Sad;
    private HomePageScript homeinstance;
    public GameObject PausePage;
    public int Id_game;
    public SimpleSQLManager dbmanager;
    [SerializeField]private List<KeyValuePair<string, string>> Masterpair = new List<KeyValuePair<string, string>>();
    private int correctPoint, Wrongpoint,totalgamescore;
    private float gamePercentage;
    [SerializeField]private List<string> location = new List<string>();
    [SerializeField] private List<int> IdlocationMapped = new List<int>();
    [SerializeField] private List<int> Idflag = new List<int>();
    [SerializeField] private List<string> Flag = new List<string>();
    private List<int> ScoreTile = new List<int>();
    private List<int> is_correct = new List<int>();
    private List<string> locationNameList = new List<string>();
    private List<int> flagLoaclId = new List<int>();
    public GameObject Leaderboard;
    public AudioSource SoundEffect;
    public AudioClip wrongclip, Rightclip;
    public GameObject Scartchcardpage;
    private bool IsPassed;
    void Start()
    {
        homeinstance = HomePageScript.Homepage;
        PostMasterTable = new PostScoreInMasterTable();

    }

     void OnEnable()
    {
        IsPassed = false;
        StartCoroutine(GetGameAttemptNoTask());
        TimeBool = true;
        helpingbool = true;
        totalTimercount = Timercount * 60  + second;
        RunningTimeCount = totalTimercount;
        TotalTils.text = TileCount.ToString();
        StartCoroutine(GamespriteGenerator());
        GenerateMasterpairs();
    
    }


    void GenerateMasterpairs()
    {
        var LocationLog = dbmanager.Table<MatchTheTileFlag>().ToList();
        LocationLog.ForEach(x =>
        {
            Masterpair.Add(new KeyValuePair<string, string>(x.Flag, x.LocationMatchId.ToString()));
            DustbinNames.Add(x.Flag);
            correctPoint = x.CorrectPoint;
            totalgamescore += x.CorrectPoint;
            Wrongpoint = x.WrongPoint;
            flagLoaclId.Add(x.IdFlag);
        });

        var Locationdata = dbmanager.Table<MatchTheTileLocation>().ToList();
        Locationdata.ForEach(y =>
        {
            locationNameList.Add(y.Type);
        });

        var GameScoredata = dbmanager.Table<GameListDetails>().FirstOrDefault(x => x.GameId == Id_game).CompletePer;
        gamePercentage = GameScoredata;

    }
    IEnumerator getLocaldata() 
    {
        yield return new WaitForSeconds(0.1f);
        var LocalLog = dbmanager.Table<ObjectGameList>().ToList();
        if(LocalLog!= null)
        {

        }


    }
    private void OnDisable()
    {
        TimerImage.fillAmount = 1f;
        Circulartimer.fillAmount = 1f;
        RunningTimeCount = 0f;
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

    // Update is called once per frame
    void Update()
    {

        if (TimeBool)
        {
            if (second >= 0.0f && Timercount >= 0 && helpingbool)
            {
                second = second - Time.deltaTime;
                RunningTimeCount = RunningTimeCount - Time.deltaTime;
                TimerImage.fillAmount = RunningTimeCount / totalTimercount;
                Circulartimer.fillAmount = RunningTimeCount / totalTimercount;
                if (second.ToString("0").Length > 1)
                {
                    TimerText.text = "0" + Timercount.ToString("0") + ":" + second.ToString("0");
                }
                else
                {
                    TimerText.text = "0" + Timercount.ToString("0") + ":" + "0" + second.ToString("0");
                }

                if (second.ToString("0") == "0" && Timercount >= 0)
                {
                    second = 60;
                    Timercount = Timercount - 1;
                }
            }
            else if (helpingbool)
            {
                Debug.Log(" done timer");
                helpingbool = false;
                StartCoroutine(TimeUpTask());
            }
        }
       

        if (ScoreCheckNow)
        {
            ScoreCheckNow = false;
            knobAngle =  ((float)ScoreConuter / (float)BonusGameScore) * 200;

        }
        var rotationAngle = Quaternion.Euler(0f, 0f, -knobAngle);
        ScoreKnob.GetComponent<RectTransform>().rotation = Quaternion.Lerp(ScoreKnob.GetComponent<RectTransform>().rotation, rotationAngle, 10 * 1 * Time.deltaTime);
    }

    IEnumerator TimeUpTask()
    {
        GeneratedTiles.ForEach(DestroyImmediate);
        yield return new WaitForSeconds(0.1f);
        TimesUp.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        iTween.ScaleTo(TimesUp, Vector3.zero, 0.5f);
        GeneratedSprite.Clear();
        Array.Clear(DustbinSprite, 0, DustbinSprite.Length);
        Array.Clear(WasteSprite, 0, WasteSprite.Length);
        totalTimercount = 0f;
        CorrectGuess = 0;
        CorrectTiles.text = "0";
        var NotCollectedFlag = DustbinNames.Where(x => !Flag.Contains(x)).Select(x => x).ToList();
        NotCollectedFlag.ForEach(x =>
        {
            Flag.Add(x);
            ScoreTile.Add(0);
            is_correct.Add(0);
            IdlocationMapped.Add(dbmanager.Table<MatchTheTileFlag>().FirstOrDefault(y => y.Flag.Equals(x, System.StringComparison.OrdinalIgnoreCase)).LocationMatchId);
        });
        var notSelectedlocation =  locationNameList.Where(a => !location.Contains(a)).Select(b => b).ToList() ;
        notSelectedlocation.ForEach(t =>
        {
            location.Add(t);
        });

        var NotSelectedIdFlag =  flagLoaclId.Where(o => !Idflag.Contains(o)).Select(n => n).ToList() ;
        NotSelectedIdFlag.ForEach(p =>
        {
            Idflag.Add(p);
        });


        yield return new WaitForSeconds(0.5f);
        TimesUp.SetActive(false);
        msgbox.text = "Oops! Your Time is up.";
        PopupPage.SetActive(true);
        Avatarmood.sprite = Sad;
        StartCoroutine(PostGameDetails());
        StartCoroutine(PostMasterLog());

    }

 
  

    IEnumerator GamespriteGenerator()
    {
        int index = 0;
        for (int a = 0; a < TileCount; a++)
        {
            if (index == TileCount)
            {
                index = 0;
            }
            GeneratedSprite.Add(DustbinSprite[index]);
            index++;
        }
        yield return new WaitForSeconds(0.5f);

        for (int b = 0; b < wasteCount; b++)
        {
            GeneratedSprite.Add(WasteSprite[b]);
        }
        SpriteMixer();
    }

    void TileSetup()
    {
        for (int a = 0; a < GeneratedSprite.Count; a++)
        {
            GameObject gb = Instantiate(ButtonPrefeb, TileParent, false);
            gb.GetComponent<Image>().sprite = DefaultSprite;
            gb.GetComponent<Button>().onClick.AddListener(delegate { TilesAction(); });
            GeneratedTiles.Add(gb);
            gb.name = a.ToString();
            gb.SetActive(true);
        }

    }

    void SpriteMixer()
    {
        for (int a = 0; a < GeneratedSprite.Count; a++)
        {
            Sprite temp = GeneratedSprite[a];
            int randomindex = UnityEngine.Random.Range(1, GeneratedSprite.Count);
            GeneratedSprite[a] = GeneratedSprite[randomindex];
            GeneratedSprite[randomindex] = temp;

        }
        TileSetup();
    }

    void TilesAction()
    {
        if (!FirstGuess)
        {
            //string buttonName = EventSystem.current.currentSelectedGameObject.name;
            FirstGuess = true;
            FirstSelectedobj = EventSystem.current.currentSelectedGameObject;
            FirstObjSprite = FirstSelectedobj.GetComponent<Image>().sprite;
            FirstSelectedobj.GetComponent<Image>().sprite = BlankTile;
            FirstSelectedobj.transform.GetChild(0).GetComponent<Image>().sprite = GeneratedSprite[int.Parse(EventSystem.current.currentSelectedGameObject.name)];
            FirstSelectedobj.transform.GetChild(0).gameObject.SetActive(true);
            FirstSelectedobj.GetComponent<Button>().enabled = false;
            FirstGuessName = GeneratedSprite[int.Parse(EventSystem.current.currentSelectedGameObject.name)].name;
            Debug.Log("first selection " + FirstGuessName);
            FirstSelectedobj.tag = DustbinNames.Contains(FirstGuessName) ? "Dustbin" : "Waste";
        }
        else if (!SecondGuess)
        {
            SecondGuess = true;
            SecondSelectedobj = EventSystem.current.currentSelectedGameObject;
            SecondObjSprite = SecondSelectedobj.GetComponent<Image>().sprite;
            SecondSelectedobj.GetComponent<Image>().sprite = BlankTile;
            SecondSelectedobj.transform.GetChild(0).GetComponent<Image>().sprite = GeneratedSprite[int.Parse(EventSystem.current.currentSelectedGameObject.name)];
            SecondSelectedobj.transform.GetChild(0).gameObject.SetActive(true);
            SecondSelectedobj.GetComponent<Button>().enabled = false;
            SecondGuessName = GeneratedSprite[int.Parse(EventSystem.current.currentSelectedGameObject.name)].name;
            Debug.Log("Second selection " + SecondGuessName);
            SecondSelectedobj.tag = DustbinNames.Contains(SecondGuessName) ? "Dustbin" : "Waste";

            StartCoroutine(CheckAns());

        }
    }

    private string FirstSelectedName => FirstSelectedobj.transform.GetChild(0).GetComponent<Image>().sprite.name;
    private string SecondSelectedName => SecondSelectedobj.transform.GetChild(0).GetComponent<Image>().sprite.name;

    IEnumerator CheckAns()
    {
        yield return new WaitForSeconds(0.5f);
        if (FirstSelectedobj.tag == "Waste" && SecondSelectedobj.tag == "Dustbin")
        {
            //var wasteType = Masterpair.FirstOrDefault(x => x.Key.Equals(SecondSelectedName, System.StringComparison.OrdinalIgnoreCase));
            var FlagKey = Masterpair.FirstOrDefault(a => a.Key.Equals(SecondSelectedName, System.StringComparison.OrdinalIgnoreCase));
            var isBelongToWasteType = FlagKey.Value.Equals(FirstSelectedName, System.StringComparison.OrdinalIgnoreCase);
            if (isBelongToWasteType)
            {
                SoundEffect.clip = Rightclip;
                SoundEffect.Play();
                Flag.Add(SecondSelectedName);     //flag name
                ScoreTile.Add(correctPoint);
                is_correct.Add(1);
                IdlocationMapped.Add(int.Parse(FlagKey.Value));   //Correct mapped id
                location.Add(locationNameList[int.Parse(FirstSelectedName) -1]);  //location name 
                int indexId = DustbinNames.FindIndex(x => x.Equals(SecondSelectedName, System.StringComparison.OrdinalIgnoreCase));
                Idflag.Add(flagLoaclId[indexId]);  // id flag 
                FirstSelectedobj.transform.GetChild(0).GetComponent<Image>().sprite = RightSprite;
                SecondSelectedobj.transform.GetChild(0).GetComponent<Image>().sprite = RightSprite;
                FirstSelectedobj.GetComponent<Image>().sprite = RightbgSprite;
                SecondSelectedobj.GetComponent<Image>().sprite = RightbgSprite;
                FirstSelectedobj.GetComponent<Button>().enabled = false;
                SecondSelectedobj.GetComponent<Button>().enabled = false;
                FirstGuess = SecondGuess = false;
                ScoreCheckNow = true;
                ScoreConuter += correctPoint;
                ScoreText.text = ScoreConuter.ToString();
                CorrectGuess++;
                CorrectTiles.text = CorrectGuess.ToString();
            
            }
            else
            {
                Debug.Log(" Found object");
                SoundEffect.clip = wrongclip;
                SoundEffect.Play();
                FirstSelectedobj.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = WrongSprite;
                SecondSelectedobj.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = WrongSprite;
                FirstSelectedobj.GetComponent<Image>().sprite = WrongbgSprite;
                SecondSelectedobj.GetComponent<Image>().sprite = WrongbgSprite;
                StartCoroutine(ResetTiles(FirstSelectedobj, SecondSelectedobj));
            }
  
        }
        else if(SecondSelectedobj.tag == "Waste" && FirstSelectedobj.tag == "Dustbin")
        {
            var FlagKey = Masterpair.FirstOrDefault(a => a.Key.Equals(FirstSelectedName, System.StringComparison.OrdinalIgnoreCase));
            var isBelongToWasteType = FlagKey.Value.Equals(SecondSelectedName, System.StringComparison.OrdinalIgnoreCase);
            if (isBelongToWasteType)
            {
                SoundEffect.clip = Rightclip;
                SoundEffect.Play();
                Flag.Add(FirstSelectedName);     //flag name
                ScoreTile.Add(correctPoint);
                is_correct.Add(1);
                IdlocationMapped.Add(int.Parse(FlagKey.Value));   //Correct mapped id
                location.Add(locationNameList[int.Parse(SecondSelectedName)-1]);  //location name 
                int indexId = DustbinNames.FindIndex(x => x.Equals(FirstSelectedName, System.StringComparison.OrdinalIgnoreCase));
                Idflag.Add(flagLoaclId[indexId]);  // id flag 
                FirstSelectedobj.transform.GetChild(0).GetComponent<Image>().sprite = RightSprite;
                SecondSelectedobj.transform.GetChild(0).GetComponent<Image>().sprite = RightSprite;
                FirstSelectedobj.GetComponent<Image>().sprite = RightbgSprite;
                SecondSelectedobj.GetComponent<Image>().sprite = RightbgSprite;
                FirstGuess = SecondGuess = false;
                FirstSelectedobj.GetComponent<Button>().enabled = false;
                SecondSelectedobj.GetComponent<Button>().enabled = false;
                ScoreCheckNow = true;
                ScoreConuter += correctPoint;
                ScoreText.text = ScoreConuter.ToString();
                CorrectGuess++;
                CorrectTiles.text = CorrectGuess.ToString();
          


            }
            else
            {
                SoundEffect.clip = wrongclip;
                SoundEffect.Play();
                Debug.Log(" Found object");
                FirstSelectedobj.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = WrongSprite;
                SecondSelectedobj.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = WrongSprite;
                FirstSelectedobj.GetComponent<Image>().sprite = WrongbgSprite;
                SecondSelectedobj.GetComponent<Image>().sprite = WrongbgSprite;
                StartCoroutine(ResetTiles(FirstSelectedobj, SecondSelectedobj));
            }
        }
        else
        {
            SoundEffect.clip = wrongclip;
            SoundEffect.Play();
            Debug.Log("Found object");
            FirstSelectedobj.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = WrongSprite;
            SecondSelectedobj.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = WrongSprite;
            FirstSelectedobj.GetComponent<Image>().sprite = WrongbgSprite;
            SecondSelectedobj.GetComponent<Image>().sprite = WrongbgSprite;
            StartCoroutine(ResetTiles(FirstSelectedobj, SecondSelectedobj));
          
        }
        if (CorrectGuess == TileCount)
        {
            StartCoroutine(GameWinTssk());
        }

        SCorePanel.GetComponent<shakeeffect>().enabled = true;
        yield return new WaitForSeconds(1.5f);
        SCorePanel.GetComponent<shakeeffect>().enabled = false;
    }

    IEnumerator ResetTiles(GameObject FrstObj,GameObject secondObj)
    {
        yield return new WaitForSeconds(0.6f);
        FirstSelectedobj.GetComponent<Image>().sprite = DefaultSprite;
        SecondSelectedobj.GetComponent<Image>().sprite = DefaultSprite;     
        FirstSelectedobj.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Defaultobj;
        SecondSelectedobj.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Defaultobj;

        FirstSelectedobj.GetComponent<Button>().enabled = true;
        SecondSelectedobj.GetComponent<Button>().enabled = true;
        FirstGuess = SecondGuess = false;
    }

    IEnumerator GameWinTssk()
    {
        string Winmsg;
        TimeBool = false;
        float percentage = ((float)ScoreConuter / (float)totalgamescore) * 100;
        StartCoroutine(PostGameDetails());
        StartCoroutine(PostMasterLog());
        if(percentage >= gamePercentage)
        {
            Winmsg = "Congratulations! You have cleared this game";
            IsPassed = true;
        }
        else
        {
            Winmsg ="Sorry!You have faild in this game";
        }
        msgbox.text = Winmsg;
        Avatarmood.sprite = Sad;
        PopupPage.SetActive(true);
   
        yield return new WaitForSeconds(0.1f);
     
    }

    public void OkDonePage()
    {
        StartCoroutine(CloseStatusPageTask());
    }

    IEnumerator CloseStatusPageTask()
    {
        iTween.ScaleTo(PopupPage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
  
        PopupPage.SetActive(false);
        msgbox.text = "";
        if (IsPassed)
        {
            Scartchcardpage.SetActive(true);
        }
        else
        {
            Leaderboard.SetActive(true);
        }
    }
    IEnumerator PostGameDetails()
    {
        yield return new WaitForSeconds(0.1f);
        string hittingurl = $"{MainUrls.BaseUrl}{MainUrls.MatchTheTilepostApi}";
        var Logs = new List<MatchTheTilePostModel>();
        int l = 0;
        Flag.ForEach(x =>
        {
            var log = new MatchTheTilePostModel()
            {
                id_user = PlayerPrefs.GetInt("UID"),
                attempt_no = GameAttemptNumber + 1,
                score = ScoreTile[l],
                is_correct = is_correct[l],
                Id_Game = Id_game,
                id_waste = Idflag[l],
                waste = Flag[l],
                id_waste_type = IdlocationMapped[l],
                type = location[l]
            };
            l = l + 1;
            Logs.Add(log);
        });

        string CompleteLog = Newtonsoft.Json.JsonConvert.SerializeObject(Logs);
        Debug.Log("Final log " + CompleteLog);
        AESAlgorithm aes = new AESAlgorithm();
        string Encrypteddata = aes.getEncryptedString(CompleteLog);
        CommonModel Model = new CommonModel
        {
            Data = Encrypteddata
        };

        string Finallog = Newtonsoft.Json.JsonConvert.SerializeObject(Model);
        using (UnityWebRequest Request = UnityWebRequest.Put(hittingurl, Finallog))
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
                Debug.Log("Error " + Request.downloadHandler.text);
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
            score = ScoreConuter,
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
                Debug.Log("Error " + Request.downloadHandler.text);
            }

        }
    }

    public void GotoHome()
    {
        StartCoroutine(Hometask());
    }
    IEnumerator Hometask()
    {
        iTween.ScaleTo(PopupPage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        msgbox.text = "";
        PopupPage.SetActive(false);
        Destroy(homeinstance);
        SceneManager.LoadScene(0);

    }

    public void BackAction()
    {
        TimeBool = false;
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
        TimeBool = true;
    }
}
