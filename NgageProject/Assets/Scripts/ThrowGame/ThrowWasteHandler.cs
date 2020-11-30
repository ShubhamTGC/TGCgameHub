using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using SimpleSQL;
using UnityEngine.SceneManagement;

public class ThrowWasteHandler : MonoBehaviour
{
    public int Objectcount;
    private Sprite[] WasteObejctSprites;
    public string wasteImagePath;
    public GameObject WastePrefeb;
    public Transform WastePerent;
    public LineRenderer ForntLine, BackLine;
    public Rigidbody2D catapultback;
    [SerializeField]
    private LineRenderer BackLineRendere, ForntLinerendere;
    [SerializeField]
    private List<Sprite> LevelObjects;
   
    private List<GameObject> GeneratedObj;
    public Image Previewimage1, Previewimage2, Previewimage3, Previewimage4;
    public GameObject FirstObjectName;
    private int Objectlive = 0;
    private Vector3 timerPanelPos;
    public GameObject TimerPanel;
    public GameObject Gamecanvas, ZonePag,ZoneselectionPage, MainZone, Startpage;
    public Button closeButton;
    public AudioSource gameSound;
    public AudioClip RightAns, WrongAns;
    public AudioClip Shoot, Outside,Stretching;
    public GameObject PreviewCard;
    //============== COMPLETE GAME SCORE VARIABLE AND HELPING VARIABLE======================
    [Header("Time portion and score ")]
    [Space(10)]
    public float minut;
    public float second;
    private float Totaltimer, RunningTimer;
    private float sec;
    private float Mintvalue;
    private float CurrentTime;
    private bool helpingbool = true;
    public Image Timerbar;
    public Text Timer;
    public Text CorrectGuesscount, Totalwaste;
    public Text TotalscoreText;
    public int SCore;
    private int correctGoal = 0;
    private bool gameend = false;
    private bool gameclose = false;
    [SerializeField]
    private int totalwasteCount;
    public Text Gamestaus;
    public List<Sprite> paper, glass, biohazard, ewaste, plastic, organic, metal;
    private List<KeyValuePair<string, List<Sprite>>> WasteCollectionList;
    public GameObject ScoreKnob;
    [SerializeField]
    private int TotalGameScore;
    private bool checkScore;
    private float Knobangle;
    [HideInInspector]public bool TimePaused = false;
    [SerializeField]
    private Color WrongEffect;

    public Image PowerBar;
    [SerializeField]
    private float Maxstrecthvalue = 9.30f;
    //========================================== PRIVATE OBJECTS FOR GAME==================== 
    private List<string> Dustbins = new List<string>();
    [SerializeField]private List<string> ItemColleted = new List<string>();
    private List<int> ObjectScore = new List<int>();
    private List<int> is_correct = new List<int>();
    private List<string> CorrectOption = new List<string>();
    private List<string> DustbinsL2 = new List<string>();
    private List<string> ItemColletedL2 = new List<string>();
    private List<int> ObjectScoreL2 = new List<int>();
    private List<int> is_correctL2 = new List<int>();
    private List<string> CorrectOptionL2 = new List<string>();


    public GameObject onstrike;
    private bool closeGame;
    [HideInInspector]
    public bool LevelCompleted;
    [SerializeField]
    private List<GameObject> RowsLevel1;
    [SerializeField]
    private List<GameObject> RowsLevel2;
 
    // DASHBOARD VARIABLES=================================//
    [Header("====DASHBAORD DATA=====")]
    [Space(10)]
    //public Transform LevelParent;
    public GameObject DatarowPrefeb;
    public Sprite Correct, Wrong, PCorrect, CorrectOpt;
    private GameObject gb;
    public Text OverallScore;
    public bool level1, level2;
    public Stage2ZoneHandler stage2handler;
    public ThrowWasteHandler level1Reference;
    public GameObject level1obj, level2obj, level2Mainpage, Leveel2mainpage;
    private ZoneDataPost Loglevel1,LogLevel2;
    private int wrongans;
    public List<GameObject> wasteItems;
    public GameObject wrongeffect;
    public GameObject initialSmoke, moderateSmoke, HighSmoke;
    private List<GameObject> dashboardL1Rows = new List<GameObject>(), dashboardL2Rows = new List<GameObject>();
    public GameObject DotPrefeb;
    public Transform Dotparent;
    public GameObject MudInWater;
    public GameObject MudInWater2;
    public int LevelRoomid;
    public List<string> ObjectName = new List<string>();
    public List<int> Cscore = new List<int>();
    public List<int> Wscore = new List<int>();
    public List<int> ItemId = new List<int>();
    [SerializeField] private List<int> PlayerObjectId1 = new List<int>();
    [SerializeField] private List<int> PlayerObjectId2 = new List<int>();
    public SimpleSQLManager dbmanager;
    public List<GameObject> DustbinsObj;
    public GameObject PageGameObj;
    public GameObject Pausepage;
    public Button HomeBtn, restartBtn,closepause;
    private HomePageScript Homeinstance;
    [SerializeField] private int id_game;
   [SerializeField] private List<int> randomindex = new List<int>();
    private void Awake()
    {


    }

    private void OnEnable()
    {
        Homeinstance = HomePageScript.Homepage;
        wasteItems.ForEach(x =>
        {
            x.SetActive(false);
        });
        TimePaused = true;
        Mintvalue = minut;
        sec = second;
        Objectlive = 0;
        TotalscoreText.text = "0";
        Totaltimer = (Mintvalue * 60) + second;
        Debug.Log("time data time " + Mintvalue + " sec " + second + "total time " + Totaltimer);
        RunningTimer = Totaltimer;
        CurrentTime = 0f;
        initialSmoke.SetActive(false);
        moderateSmoke.SetActive(false);
        HighSmoke.SetActive(false);
        wrongans = 0;
        GeneratedObj = new List<GameObject>();
        RowsLevel1 = new List<GameObject>();
        RowsLevel2 = new List<GameObject>();
        gameclose = true;
        //closepause.onClick.RemoveAllListeners();
        //closepause.onClick.AddListener(delegate { ClosePuasepage(); });
        //HomeBtn.onClick.RemoveAllListeners();
        //HomeBtn.onClick.AddListener(delegate { GotoHome(); });
        //restartBtn.onClick.RemoveAllListeners();
        //restartBtn.onClick.AddListener(delegate { ClosePuasepage(); });
        //closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(delegate { CloseGame(); });
        Gamecanvas.SetActive(true);
       
       // StartCoroutine(initialdestory());
        initialsetup();
       
    }
    //IEnumerator initialdestory()
    //{
    //    for (int a = 0; a < LevelParent.childCount; a++)
    //    {
    //        DestroyImmediate(LevelParent.GetChild(a).gameObject);
    //        yield return new WaitForSeconds(0.05f);
    //    }
    //}

    private void OnDisable()
    {
        sec = 0;
        Totaltimer = 0;
        RunningTimer = 0f;
        Mintvalue = 0;
        CurrentTime = 0;
    }
    void initialsetup()
    {
       
        MudInWater.SetActive(false);
        MudInWater2.SetActive(false);
        ScoreKnob.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
        helpingbool = true;
        
        gameend  = false;
        timerPanelPos = TimerPanel.GetComponent<RectTransform>().localPosition;
        WasteObejctSprites = Resources.LoadAll<Sprite>(wasteImagePath);
      
        StartCoroutine(GetObjectdata());
      
        WasteCollectionList = new List<KeyValuePair<string, List<Sprite>>>()
        {
            new KeyValuePair<string, List<Sprite>>("Paper", paper),
            new KeyValuePair<string, List<Sprite>>("glass", glass),
            new KeyValuePair<string, List<Sprite>>("biohazard", biohazard),
            new KeyValuePair<string, List<Sprite>>("ewaste", ewaste),
            new KeyValuePair<string, List<Sprite>>("plastic", plastic),
            new KeyValuePair<string, List<Sprite>>("organic", organic),
            new KeyValuePair<string, List<Sprite>>("metal", metal)
        };
       
        Totalwaste.text = "0 / " + totalwasteCount.ToString();
        //CorrectGuesscount.text = "0";
        LevelObjects = new List<Sprite>(WasteObejctSprites.Length);
        getRandomeSprite();
        for (int a = 0; a < WasteObejctSprites.Length; a++)
        {
            GameObject gb = Instantiate(WastePrefeb, WastePerent, false);
            gb.SetActive(false);
            gb.GetComponent<ProjectileDragging>().catapultLineBack = BackLine;
            gb.GetComponent<SpringJoint2D>().connectedBody = catapultback;
            gb.GetComponent<ProjectileDragging>().catapultLineFront = ForntLine;
            gb.GetComponent<ProjectileDragging>().dots = DotPrefeb;
            gb.GetComponent<ProjectileDragging>().ProjectionParent = Dotparent;
            gb.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = WasteObejctSprites[a];
            
            gb.GetComponent<ProjectileDragging>().ONstrike = onstrike;
            GeneratedObj.Add(gb);
            LevelObjects.Add(WasteObejctSprites[a]);
        }
        gameclose = false;
        correctGoal = 0;
        SCore = 0;
        Previewimage1.GetComponentInParent<BoxCollider2D>().enabled = false;
        Previewimage1.gameObject.SetActive(true);
        Previewimage2.gameObject.SetActive(true);
        Previewimage3.gameObject.SetActive(true);
        Previewimage4.gameObject.SetActive(true);
        SpriteUpdator(0);
        GeneratedObj[Objectlive].SetActive(true);
        PreviewCard.GetComponent<PreviewPageHandler>().CheckBar();
        TimePaused = false;
        closeGame = false;
        FirstObjectName.SetActive(true);
        FirstObjectName.transform.GetChild(0).gameObject.GetComponent<Text>().text = GeneratedObj[Objectlive].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.name;
       
    }

    IEnumerator GetObjectdata()
    {
        yield return new WaitForSeconds(0.1f);
        var Cmslog = dbmanager.Table<ObjectGameList>().ToList();
        var ObjectLog = Cmslog.Where(x => x.RoomId == LevelRoomid).Select(y => y.ItemName).ToList();
        var Cscorelog = Cmslog.Where(a => a.RoomId == LevelRoomid).Select(b => b.CorrectPoint).ToList();
        var WscoreLog = Cmslog.Where(c => c.RoomId == LevelRoomid).Select(d => d.WrongPoint).ToList();
        var ItemIdLog = Cmslog.Where(e => e.RoomId == LevelRoomid).Select(f => f.ItemId).ToList();
        ObjectName = ObjectLog;
        Cscore = Cscorelog;
        Wscore = WscoreLog;
        ItemId = ItemIdLog;
        //var Bonuslog = dbmanager.Table<BonusTable>().ToList();
        //Time0to30 = Bonuslog.FirstOrDefault(x => x.RoomId == LevelRoomid).Time0to30;
        //Time30to45 = Bonuslog.FirstOrDefault(x => x.RoomId == LevelRoomid).Time30to45;
        //Bonuspt1 = Bonuslog.FirstOrDefault(x => x.RoomId == LevelRoomid).BonusPoint1;
        //Bonuspt2 = Bonuslog.FirstOrDefault(x => x.RoomId == LevelRoomid).BonusPoint2;

    }

    void getRandomeSprite()
    {
        for (int a = 0; a < WasteObejctSprites.Length; a++)
        {
            Sprite temp = WasteObejctSprites[a];
            int randomindex = UnityEngine.Random.Range(1, WasteObejctSprites.Length);
            WasteObejctSprites[a] = WasteObejctSprites[randomindex];
            WasteObejctSprites[randomindex] = temp;
        }
        
    }

    void SpriteUpdator(int a)
    {
        Previewimage1.GetComponent<Image>().sprite = GeneratedObj[a].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        Previewimage2.GetComponent<Image>().sprite = GeneratedObj[a + 1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        Previewimage3.GetComponent<Image>().sprite = GeneratedObj[a + 2].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        Previewimage4.GetComponent<Image>().sprite = GeneratedObj[a + 3].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
    }
    void Start()
    {

    }

    void Update()
    {
        if (!TimePaused)
        {
            if (sec >= 0 && Mintvalue >= 0 && helpingbool)
            {
                sec = sec - Time.deltaTime;
                RunningTimer = RunningTimer - Time.deltaTime;
                Timerbar.fillAmount = RunningTimer / Totaltimer;
                CurrentTime += Time.deltaTime;
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
                string Showmsg = "";
                helpingbool = false;
                if (level1)
                {
                     Showmsg = "Opps ! Times Up for level 1 Please continue with Level 2.";
                }
                else
                {
                   
                    Showmsg = "Opps ! Times Up for level 2. ";
                }
               
                StartCoroutine(GameEndProcess(Showmsg));

            }
        }

        if (!gameclose)
        {
            //if (LeaderBoard.activeInHierarchy || SettingPage.activeInHierarchy || overallDashboard.activeInHierarchy || GreenJurnal.activeInHierarchy)
            //{
            //    TimePaused = true;
            //    ForntLine.enabled = false;
            //    BackLine.enabled = false;
            //    DustbinsObj.ForEach(x =>
            //    {
            //        x.GetComponent<BoxCollider2D>().enabled = false;
            //    });
            //}
            //else
            //{
            //    TimePaused = false;
            //    ForntLine.enabled = true;
            //    BackLine.enabled = true;
            //    DustbinsObj.ForEach(x =>
            //    {
            //        x.GetComponent<BoxCollider2D>().enabled = true;
            //    });

            //}
            ThrowWasteObjectactiveStatus();
        }

        if (checkScore)
        {
            checkScore = false;
            Knobangle = ((float)SCore / (float)TotalGameScore) * 200f;

        }
        var Rotationangle = Quaternion.Euler(0f, 0f, -Knobangle);
        ScoreKnob.GetComponent<RectTransform>().rotation = Quaternion.Lerp(ScoreKnob.GetComponent<RectTransform>().rotation, Rotationangle, 10 * 1 * Time.deltaTime);
        if (!closeGame)
        {
            PowerBar.fillAmount = GeneratedObj[Objectlive].GetComponent<ProjectileDragging>().catapultToMouse.sqrMagnitude / Maxstrecthvalue;
        }


    }

    void ThrowWasteObjectactiveStatus()
    {
       
        if (Objectlive <= GeneratedObj.Count-1)
        {
            if (!GeneratedObj[Objectlive].activeInHierarchy)
            {
                Objectlive++;
                if(Objectlive >= GeneratedObj.Count)
                {

                }
                else
                {
                    GeneratedObj[Objectlive].SetActive(true);
                    if (Objectlive < GeneratedObj.Count)
                    {
                        Previewimage1.GetComponent<Image>().sprite = GeneratedObj[Objectlive].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                        //FirstObjectName.SetActive(true);
                        FirstObjectName.transform.GetChild(0).gameObject.GetComponent<Text>().text = Previewimage1.GetComponent<Image>().sprite.name;
                        if (Objectlive < GeneratedObj.Count - 1)
                        {
                            
                            Previewimage2.GetComponent<Image>().sprite = GeneratedObj[Objectlive + 1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
                        }
                        else
                        {
                            //Previewimage2.GetComponentInParent<BoxCollider2D>().enabled = false;
                            Previewimage2.gameObject.SetActive(false);
                        }
                        if (Objectlive < GeneratedObj.Count - 2)
                        {

                            Previewimage3.GetComponent<Image>().sprite = GeneratedObj[Objectlive + 2].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

                        }
                        else
                        {
                            //Previewimage3.GetComponentInParent<BoxCollider2D>().enabled = false;
                            Previewimage3.gameObject.SetActive(false);
                        }

                        if (Objectlive < GeneratedObj.Count - 3)
                        {
                            Previewimage4.GetComponent<Image>().sprite = GeneratedObj[Objectlive + 3].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

                        }
                        else
                        {
                           // Previewimage4.GetComponentInParent<BoxCollider2D>().enabled = false;
                            Previewimage4.gameObject.SetActive(false);
                        }

                    }
                }
            }

        
        }
        if (Objectlive == GeneratedObj.Count && !gameend)
        {
            string Showmsg = "";
            closeGame = true;
            FirstObjectName.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
            FirstObjectName.SetActive(false);
            Previewimage3.gameObject.SetActive(false);
            Previewimage2.gameObject.SetActive(false);
            Previewimage1.gameObject.SetActive(false);
            Previewimage4.gameObject.SetActive(false);
            gameend = true;
            bool AllowForBonus;
           
            if (level1)
            {
                AllowForBonus = is_correct.Contains(0);
                Showmsg = "Congratulations! You have successfully completed Level 1\nPlease click on Level 2 to play!";
            }
            else
            {
                AllowForBonus = is_correctL2.Contains(0);
                Showmsg = "Congratulations! You have successfully completed ";
            }

            StartCoroutine(GameEndProcess(Showmsg));
        }
       
    }

    IEnumerator GameEndProcess(string msg)
    {
        TimePaused = true;
        gameclose = true;
        BackLineRendere.enabled = false;
        ForntLinerendere.enabled = false;
        yield return new WaitForSeconds(0.1f);
        GeneratedObj.ForEach(x =>
        {
            x.SetActive(false);
        });
        yield return new WaitForSeconds(0.1f);
        LevelCompleted = true;
        stage2handler.checkLevelStatus(msg);

    }

    public void checkCollidedAns(string dustbin, string collidername, GameObject dustbinobj)
    {
        if (level1)
        {
            int objectindex = ObjectName.FindIndex(x => x.Equals(collidername, System.StringComparison.OrdinalIgnoreCase));
            PlayerObjectId1.Add(ItemId[objectindex]);
            if (dustbin != "wall")
            {
                var CollidedDustbin = WasteCollectionList.FirstOrDefault(x => x.Key.Equals(dustbin, System.StringComparison.OrdinalIgnoreCase));
                var iscorrectWaste = CollidedDustbin.Value.Any(x => x.name.Equals(collidername, System.StringComparison.OrdinalIgnoreCase));
             
                if (iscorrectWaste)
                {
                    int index = ObjectName.FindIndex(x => x.Equals(collidername, System.StringComparison.OrdinalIgnoreCase));
                    int TempScore = Cscore[index];
                    gameSound.clip = RightAns;
                    gameSound.Play();
                    SCore += TempScore;
                    TotalscoreText.text = SCore.ToString();
                    checkScore = true;
                    is_correct.Add(1);
                    ObjectScore.Add(TempScore);
                    stage2handler.VibrateDevice();
                    CorrectOption.Add(dustbin);
                }
                else
                {
                    
                    var RelatedDustbin = (from k in WasteCollectionList
                                            where k.Value.Any(x => x.name.Equals(collidername, System.StringComparison.OrdinalIgnoreCase))
                                            select k.Key).FirstOrDefault();
                    CorrectOption.Add(RelatedDustbin);
                    gameSound.clip = WrongAns;
                    gameSound.Play();
                    is_correct.Add(0);
                    ObjectScore.Add(0);
                    wrongans++;
                    stage2handler.VibrateDevice();
                    StartCoroutine(WrongObjectTask(collidername));

                }
                Dustbins.Add(dustbin);
            }
            else
            {
                string RelatedDustbin = "null";
                CorrectOption.Add(RelatedDustbin);
                gameSound.clip = Outside;
                gameSound.Play();
                is_correct.Add(0);
                ObjectScore.Add(0);
                string nullDustbin = "null";
                Dustbins.Add(nullDustbin);
                wrongans++;
                stage2handler.VibrateDevice();
                StartCoroutine(WrongObjectTask(collidername));
            }
            correctGoal++;
            Totalwaste.text = correctGoal.ToString() + " / " + totalwasteCount.ToString();
            ItemColleted.Add(collidername);
        }
        //

        else
        {

            if (dustbin != "wall")
            {
                int objectindex = ObjectName.FindIndex(x => x.Equals(collidername, System.StringComparison.OrdinalIgnoreCase));
                PlayerObjectId2.Add(ItemId[objectindex]);
                var CollidedDustbin = WasteCollectionList.FirstOrDefault(x => x.Key.Equals(dustbin, System.StringComparison.OrdinalIgnoreCase));
                var iscorrectWaste = CollidedDustbin.Value.Any(x => x.name.Equals(collidername, System.StringComparison.OrdinalIgnoreCase));

                if (iscorrectWaste)
                {
                    int index = ObjectName.FindIndex(x => x.Equals(collidername, System.StringComparison.OrdinalIgnoreCase));
                    int TempScore = Cscore[index];
                    gameSound.clip = RightAns;
                    gameSound.Play();
                    SCore += TempScore;
                    TotalscoreText.text = SCore.ToString();
                    checkScore = true;
                    is_correctL2.Add(1);
                    ObjectScoreL2.Add(TempScore);
                    stage2handler.VibrateDevice();
                    CorrectOptionL2.Add(dustbin);
                }
                else
                {

                    var RelatedDustbin = (from k in WasteCollectionList
                                            where k.Value.Any(x => x.name.Equals(collidername, System.StringComparison.OrdinalIgnoreCase))
                                            select k.Key).FirstOrDefault();
                    CorrectOptionL2.Add(RelatedDustbin);
                    gameSound.clip = WrongAns;
                    gameSound.Play();
                    is_correctL2.Add(0);
                    ObjectScoreL2.Add(0);
                    wrongans++;
                    stage2handler.VibrateDevice();
                    StartCoroutine(WrongObjectTask(collidername));

                }
                DustbinsL2.Add(dustbin);
            }
            else
            {
                string RelatedDustbin = "null";
                CorrectOptionL2.Add(RelatedDustbin);
                gameSound.clip = Outside;
                gameSound.Play();
                is_correctL2.Add(0);
                ObjectScoreL2.Add(0);
                string nullDustbin = "null";
                wrongans++;
                stage2handler.VibrateDevice();
                DustbinsL2.Add(nullDustbin);
                StartCoroutine(WrongObjectTask(collidername));
            }
            correctGoal++;
            Totalwaste.text = correctGoal.ToString() + " / " + totalwasteCount.ToString();
            ItemColletedL2.Add(collidername);
        }
        
    
        
    }

    IEnumerator WrongObjectTask(string ObjectName)
    {
        yield return new WaitForSeconds(0.1f);
        wasteItems.ForEach(x =>
        {
            if (x.gameObject.name.Equals(ObjectName))
            {
               wrongeffect.transform.localPosition = x.gameObject.transform.localPosition;
               x.gameObject.SetActive(true);
               wrongeffect.SetActive(true);
            }
           
        });
        if(wrongans == 2)
        {
            MudInWater.SetActive(true);
            initialSmoke.SetActive(true);
        }
        if(wrongans == 5)
        {
            MudInWater2.SetActive(true);
            moderateSmoke.SetActive(true);
        }
        if(wrongans == 7)
        {
            HighSmoke.SetActive(true);
        }

    }
    IEnumerator resetEffect(GameObject dustbinobj)
    {
        yield return new WaitForSeconds(1f);
        dustbinobj.GetComponent<SpriteRenderer>().color = Color.white;
        dustbinobj.GetComponent<ShakeEffect>().enabled = false;
    }

    public void CloseGame()
    {
        TimePaused = true;
        Pausepage.SetActive(true);
        //Previewimage1.GetComponentInParent<BoxCollider2D>().enabled = false;
        //Previewimage2.GetComponentInParent<BoxCollider2D>().enabled = false;
        //Previewimage3.GetComponentInParent<BoxCollider2D>().enabled = false;
        //Previewimage4.GetComponentInParent<BoxCollider2D>().enabled = false;
        //closeGame = true;
        //gameclose = true;
        //StartCoroutine(CloserGame());
    }
    
    public IEnumerator destoryObj()
    {
        GeneratedObj.ForEach(DestroyImmediate);
        yield return new WaitForSeconds(0.5f);
    }
    IEnumerator CloserGame()
    {
        Objectlive = 0;
        iTween.MoveTo(TimerPanel, timerPanelPos, 0.5f);
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(destoryObj());
        ResetTask();
        yield return new WaitForSeconds(0.5f);
        RowsLevel1.Clear();
        RowsLevel2.Clear();
        if (level2)
        {
            level1Reference.CloserGame();
        }
        yield return new WaitForSeconds(1f);
        GeneratedObj.Clear();
        Gamecanvas.SetActive(false);
        ZonePag.SetActive(true);
        ZoneselectionPage.SetActive(true);
        Startpage.SetActive(true);
       // MenuButton.SetActive(true);
        Startpage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        MainZone.SetActive(false);
        level1obj.SetActive(false);
        level2obj.SetActive(false);
        level2Mainpage.SetActive(false);
        Leveel2mainpage.SetActive(false);
    }

    IEnumerator PostZoneData()
    {
        yield return new WaitForSeconds(0.1f);
        ZoneDataPost Zonedata = new ZoneDataPost();
        
    }

    public void generateDashboardL1()
    {
        StartCoroutine(MakeDashbaord());
        //MakePostData();
       

        
    }
    public void generateDashboardL2()
    {
        StartCoroutine(MakeLevel2Dashbaord());
        //MakePostDataLevel2();
    }

    //******************* Zone dashboard method*******************
    IEnumerator MakeDashbaord()
    {
        int a1 = 0;
        yield return new WaitForSeconds(0.1f);
        var distinctElements = ItemColleted != null && ItemColleted.Count > 0 ? LevelObjects.Where(x => !ItemColleted.Contains(x.name)).Select(x => x.name).ToList() : new List<string>();

        for (int a = 0; a < WasteObejctSprites.Length; a++)
        {
            if (a < ItemColleted.Count)
            {
               
            }
            else
            {
                Dustbins.Add("null");
                ObjectScore.Add(0);
                is_correct.Add(0);
                CorrectOption.Add("null");
                if (distinctElements.Count > 0)
                {
                    ItemColleted.Add(distinctElements[a1]);
                    a1++;
                }
                else
                {
                    ItemColleted.Add(WasteObejctSprites[a].name);
                }

            }
        }

        MakePostData();

    }

    void MakePostData()
    {

        int l = 0;
        if (ItemColleted.Count > 0)
        {
            ItemColleted.ForEach(x =>
            {
                var Loglevel1 = new ObjectGamePostModel()
                {
                    item_collected = ItemColleted[l],
                    score = ObjectScore[l],
                    is_right = is_correct[l],
                    correct_option = CorrectOption[l],
                    Id_Game = id_game,
                    item_Id = PlayerObjectId1[l],
                    id_user = PlayerPrefs.GetInt("UID"),
                    dustbin = Dustbins[l],
                    id_room = stage2handler.roomid[0],
                    attempt_no = stage2handler.GameAttemptNumber + 1
                };
                l = l + 1;
                stage2handler.logs.Add(Loglevel1);

            });
        }
        else
        {
            GeneratedObj.ForEach(x =>
            {
                var Loglevel1 = new ObjectGamePostModel()
                {
                    item_collected = WasteObejctSprites[l].name,
                    score = ObjectScore[l],
                    is_right = is_correct[l],
                    correct_option = CorrectOption[l],
                    Id_Game = id_game,
                    item_Id = PlayerObjectId1[l],
                    id_user = PlayerPrefs.GetInt("UID"),
                    dustbin = Dustbins[l],
                    id_room = stage2handler.roomid[0],
                    attempt_no = stage2handler.GameAttemptNumber + 1
                };
                l = l + 1;
                stage2handler.logs.Add(Loglevel1);

            });
        }




    }

    IEnumerator MakeLevel2Dashbaord()
    {
        yield return new WaitForSeconds(0.1f);
        int a1 = 0;
        var distinctElements = ItemColletedL2 != null && ItemColletedL2.Count > 0 ? LevelObjects.Where(x => !ItemColletedL2.Contains(x.name)).Select(x => x.name).ToList() : new List<string>();

        for (int a = 0; a < WasteObejctSprites.Length; a++)
        {
            if (a < ItemColletedL2.Count)
            {
            }
            else
            {
                DustbinsL2.Add("null");
                ObjectScoreL2.Add(0);
                is_correctL2.Add(0);
                CorrectOptionL2.Add("null");
                if (distinctElements.Count > 0)
                {
                    ItemColletedL2.Add(distinctElements[a1]);
                    a1++;
                }
                else
                {
                    ItemColletedL2.Add(WasteObejctSprites[a].name);
                }
            }
        }
        MakePostDataLevel2();
    }
    void MakePostDataLevel2()
    {
        Debug.Log("list count " + ItemColletedL2.Count);
        int l = 0;
        if (ItemColletedL2.Count > 0)
        {
            ItemColletedL2.ForEach(x =>
            {
                var LogLevel2 = new ObjectGamePostModel()
                {
                    item_collected = ItemColletedL2[l],
                    score = ObjectScoreL2[l],
                    is_right = is_correctL2[l],
                    correct_option = CorrectOptionL2[l],
                    Id_Game = 2,
                    item_Id = PlayerObjectId2[l],
                    id_user = PlayerPrefs.GetInt("UID"),
                    dustbin = DustbinsL2[l],
                    id_room = stage2handler.roomid[1],
                    attempt_no = stage2handler.GameAttemptNumber + 1
                };
                l = l + 1;
                stage2handler.logs.Add(LogLevel2);

            });
        }
        else
        {
            GeneratedObj.ForEach(x =>
            {
                Debug.Log("waste name " + WasteObejctSprites[l].name);
                var LogLevel2 = new ObjectGamePostModel()
                {
                    item_collected = WasteObejctSprites[l].name,
                    score = ObjectScoreL2[l],
                    is_right = is_correctL2[l],
                    correct_option = CorrectOptionL2[l],
                    Id_Game = 2,
                    item_Id = PlayerObjectId2[l],
                    id_user = PlayerPrefs.GetInt("UID"),
                    dustbin = DustbinsL2[l],
                    id_room = stage2handler.roomid[1],
                    attempt_no = stage2handler.GameAttemptNumber + 1
                };
                l = l + 1;
                stage2handler.logs.Add(LogLevel2);

            });
        }

        string data = Newtonsoft.Json.JsonConvert.SerializeObject(stage2handler.logs);
        stage2handler.PostZoneData(data);
    }
 

    public void ResetTask()
    {
        StartCoroutine(resetingtask());
    }
    IEnumerator resetingtask()
    {
        yield return new WaitForSeconds(0.1f);
        Dustbins.Clear();
        ItemColleted.Clear();
        ObjectScore.Clear();
        is_correct.Clear();
        CorrectOption.Clear();
        DustbinsL2.Clear();
        ItemColletedL2.Clear();
        ObjectScoreL2.Clear();
        is_correctL2.Clear();
        CorrectOptionL2.Clear();
    }

    public void GotoHome()
    {
        int index = 0;
        StartCoroutine(Hometask(index));
    }
    IEnumerator Hometask(int index)
    {
        iTween.ScaleTo(Pausepage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        Pausepage.SetActive(false);
        Destroy(Homeinstance);
        SceneManager.LoadScene(index);
    }

    public void restartGame()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(Hometask(index));
    }
    public void ClosePuasepage()
    {
        TimePaused = false;
        StartCoroutine(Closepage());
    }
    IEnumerator Closepage()
    {
        iTween.ScaleTo(Pausepage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        Pausepage.SetActive(false);
    }
}
