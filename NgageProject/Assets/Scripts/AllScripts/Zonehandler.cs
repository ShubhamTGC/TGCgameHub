using SimpleSQL;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.Linq;
using UnityEngine.Networking;
using UnityEditor;
using UnityEngine.SceneManagement;
using m2ostnextservice.Models;

public class Zonehandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject zonepage;
    public Sprite mainzone;
    public List<GameObject> subzones;
    public List<GameObject> room1, room2, room3;
    public List<GameObject> reduce, reuse, recycle, partially_reduce, partially_reuse, partially_recycle;
    public List<Sprite> subzonesprite;
    public Generationlevel startpage;
    public GameObject startpageobj;
    public GameObject dustbin, dustbintarget;
    private Vector3 initailpos_dusbin, initial_pos_timer;
    public int level1score = 0;
    public GameObject timerpanel, scoreknob;
    public Text scoretext;
    public Button backbtn, nextroombtn, yesbtn;
    public List<List<GameObject>> test;
    public bool room1_check = false, room2_check = false, room3_check = false;
    public bool room1_clear = false, room2_clear = false, room3_clear = false;
    public int waste_count = 0;
    public GameObject Done_msg_panel, exit_panel;
    private GameObject gb;
    // private List<Vector2> objectpos;
    [Header("====extra elements====")]
    public GameObject fridge;
    public GameObject tubelight, selectionpage;
    public List<GameObject> rooms;
    //public GameFrameHandler gameframe;
    private float totalobjs, totalscore, knobangle;
    public bool score_check = false;
    private bool timerstart, timerwarining = true;
    public GameObject timesuppage, timer;
    public Button timesupbtn;
    public int countDown_mint;
    public bool zone_completed, final_completed;

    [Header("======For count down time====")]
    public float mint;
    public Text timertext;
    public float sec;
    private float totalsecond, TotalSec;
    public Image timerimage;
    public GameObject scorepanel;
    private bool timerstop = false;


    public Dictionary<string, string> room1_data = new Dictionary<string, string>();
    public Dictionary<string, string> room2_data = new Dictionary<string, string>();
    public Dictionary<string, string> room3_data = new Dictionary<string, string>();

    [Header("-====for dashbaord====")]
    public List<string> room1_data_collected = new List<string>(8);
    public List<string> room2_data_collected = new List<string>(8);
    public List<string> room3_data_collected = new List<string>(8);
    public List<GameObject> tabs;
    public GameObject tab_prefeb, data_row_prefeb;
    private List<GameObject> tab1_object, tab2_object, tab3_object;
    private string room1name, room2name, room3name;
    public List<GameObject> tab_obj;
    public Button next_Zone;
    private bool is_win = false;
    private GameObject tabs_obj;
    public GameObject collected_text;
    public int collected_count;
    public Coloreffect timer_color_object;
    public GameObject action_plan_page;
    private bool action_check = true;
    public List<GameObject> rowhandler_parent;
    private int room1_score = 0, room2_score = 0, room3_score = 0;
    public Text User_grade_field;
    [HideInInspector]
    public int active_room_end;
    public GameObject bonus_page;



    [Header("home_page")]
    public GameObject Home_page_dashboard;
    public string Zone_name;
    public int id_content;
    public bool is_zome_completed;
    public string MainUrl, dashboard_api, ScorePostApi, GetGamesIDApi;
    public Button home_btn;



    [Header("For leaderboard Data")]
    [Space(10)]

    public string RoomData_api;
    public string GetlevelWisedataApi;
    [SerializeField] private List<int> RoomIds = new List<int>();
    public int Bonus_Score;
    private float Bonusscore_room1, Bonusscore_room2, Bonusscore_room3;
    public Image BonusScoreFiller;
    public float TotalBonusGameScore;
    public Text CollectedBonusScore;
    private List<float> RoomsScores = new List<float>();
    // private int Room_oneId,Room_twoId,Room_thirdId;

    //========================== CMS CONFIGURABLE SCORE OF OBJECTS ======================================
   public List<string> CMsobjectRoom1 = new List<string>();
   public List<string> CMsobjectRoom2 = new List<string>();
   public List<string> CMsobjectRoom3 = new List<string>();
   public List<int> CmsObjectId1 = new List<int>();
   public List<int> CmsObjectId2 = new List<int>();
   public List<int> CmsObjectId3 = new List<int>();
   public List<int> CMsObjectCscore1 = new List<int>();
   public List<int> CMsObjectCscore2 = new List<int>();
   public List<int> CMsObjectCscore3 = new List<int>();
   public List<int> CMsObjectPCscore1 = new List<int>();
   public List<int> CMsObjectPCscore2 = new List<int>();
   public List<int> CMsObjectPCscore3 = new List<int>();

    public List<string> CurrentItemList = new List<string>();
    public List<int> CurrentPCscore = new List<int>();
    public List<int> CurrentCscore = new List<int>();
    public List<int> currentItemId = new List<int>();
    public Dictionary<string, string> MasterList = new Dictionary<string, string>();
    public SimpleSQLManager dbmanager;


    //======== BONUS SCORE CONFIGURATION BASED ON Time ===================
    private float RunningTime;
    private int totalBonusScore;
    private int Time0to30;
    private int Time30to45;
    private int Bonuspt1, Bonuspt2;
    public List<GameObject> DustbinSound;
    public GameObject TimeSound;
    private bool CloseDoublePopup;

    public Image Winelamp, wiskeylamp, Beerlamp;
    public GameObject TimerPage;
    public List<Sprite> BeersName, WinsName, WiskeyName;
    public List<string> Ale, Lager, Mead, Stout, Bourbon, Rye, Scotch, Smw, Red, Rose, Sparkling, white;
    private List<KeyValuePair<string, List<string>>> Bardata;
    public List<string> BeerContainername, WiskeyConatinerName, WinsContainername;
    public List<GameObject> Containers;
    public Text popupScore, Scorestatus;
    public GameObject CorrectEfffect, WrongEffect;
    [HideInInspector] public bool collisioncheck;

    public GameObject PopupPage;
    public Image AvatarMood;
    public Sprite happy, sad;
    private HomePageScript homeinstance;
    private int GameAttemptNumber;
    [SerializeField] private int Id_game;
    public Text msgbox;
    [SerializeField] private List<string> Bottlecollected = new List<string>();
    [SerializeField] private List<int> Bottle_is_correct = new List<int>();
    [SerializeField] private List<int> Bottle_score = new List<int>();
    private List<string> Bottle_correct_opt = new List<string>();
    private List<string> Bottle_container = new List<string>();
    [SerializeField] private List<int> BottleId = new List<int>();
    private List<int> LocalroomId = new List<int>();
    public GameObject LeaderBoardPage;
    public GameObject BarHeading;
    public GameObject LightObject, barobject;
    void Start()
    {


    }
    void OnEnable()
    {
        homeinstance = HomePageScript.Homepage;
        Bardata = new List<KeyValuePair<string, List<string>>>()
        {
            new KeyValuePair<string,List<string>>("Ale",Ale),
            new KeyValuePair<string,List<string>>("Lager",Lager),
            new KeyValuePair<string,List<string>>("Mead",Mead),
            new KeyValuePair<string,List<string>>("Stout",Stout),
            new KeyValuePair<string,List<string>>("Bourbon",Bourbon),
            new KeyValuePair<string,List<string>>("Rye",Rye),
            new KeyValuePair<string,List<string>>("Scotch",Scotch),
            new KeyValuePair<string,List<string>>("Smw",Smw),
            new KeyValuePair<string,List<string>>("Red",Red),
            new KeyValuePair<string,List<string>>("Rose",Rose),
            new KeyValuePair<string,List<string>>("Sparkling",Sparkling),
            new KeyValuePair<string,List<string>>("white",white),

        };
        StartCoroutine(GetLocalRoomsdata());
        tabs = new List<GameObject>(new GameObject[subzones.Count]);
        timerstop = false;
        room1_score = room2_score = room3_score = 0;
        room1_clear = room2_clear = room3_clear = false;
        //backbtn.onClick.RemoveAllListeners();
        //backbtn.onClick.AddListener(delegate { initialback(); });
        score_check = false;
        float totalscore = room1.Count + room2.Count + room3.Count;
        totalscore = totalobjs * 10;
        timerpanel.SetActive(false);
        selectionpage.SetActive(true);
        //NextPAge.onClick.RemoveAllListeners();
        //NextPAge.onClick.AddListener(delegate { RightPageEnable(); });
        //backPAge.onClick.RemoveAllListeners();
        //backPAge.onClick.AddListener(delegate { LeftPageEnable(); });
        CloseDoublePopup = false;
        // Initialtask(0);

    }

    IEnumerator GetGameAttemptNoTask()
    {
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.GetAttemopNo}?UID={PlayerPrefs.GetInt("UID")}&GID={Id_game}&RID={RoomIds[0]}";
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


    IEnumerator GetLocalRoomsdata()
    {
        var gamedata = dbmanager.Table<ObjectGameList>().Where(x => x.GameId == 1).Select(y => y.RoomId).ToList();
        var roomids = gamedata.Distinct().ToList();
        Debug.Log("room length " + roomids.Count);
        for (int b = 0; b < roomids.Count; b++)
        {
            RoomIds.Add(roomids[b]);
        }
        yield return new WaitForSeconds(0.1f);
        var wasteCollection = dbmanager.Table<ObjectGameList>().ToList();
        //for (int a = 0; a < RoomIds.Count; a++)
        //{
        //    if (a == 0)
        //    {
        //        var roomidLog = wasteCollection.Where(x => x.RoomId == RoomIds[0]).Select(y => y.ItemName.Trim()).ToList();
        //        var RoomPcscore = wasteCollection.Where(x => x.RoomId == RoomIds[0]).Select(y => y.WrongPoint).ToList();
        //        var RoomCscore = wasteCollection.Where(x => x.RoomId == RoomIds[0]).Select(y => y.CorrectPoint).ToList();
        //        var Objectid = wasteCollection.Where(x => x.RoomId == RoomIds[0]).Select(y => y.ItemId).ToList();
        //        CMsobjectRoom1 = roomidLog;
        //        CMsObjectPCscore1 = RoomPcscore;
        //        CMsObjectCscore1 = RoomCscore;
        //        CmsObjectId1 = Objectid;

        //    }
        //    if (a == 1)
        //    {
        //        var roomidLog = wasteCollection.Where(x => x.RoomId == RoomIds[1]).Select(y => y.ItemName.Trim()).ToList();
        //        var RoomPcscore = wasteCollection.Where(x => x.RoomId == RoomIds[1]).Select(y => y.WrongPoint).ToList();
        //        var RoomCscore = wasteCollection.Where(x => x.RoomId == RoomIds[1]).Select(y => y.CorrectPoint).ToList();
        //        var Objectid = wasteCollection.Where(x => x.RoomId == RoomIds[1]).Select(y => y.ItemId).ToList();
        //        CMsobjectRoom2 = roomidLog;
        //        CMsObjectPCscore2 = RoomPcscore;
        //        CMsObjectCscore2 = RoomCscore;
        //        CmsObjectId2 = Objectid;
        //    }
        //    if (a == 2)
        //    {
        //        var roomidLog = wasteCollection.Where(x => x.RoomId == RoomIds[2]).Select(y => y.ItemName.Trim()).ToList();
        //        var RoomPcscore = wasteCollection.Where(x => x.RoomId == RoomIds[2]).Select(y => y.WrongPoint).ToList();
        //        var RoomCscore = wasteCollection.Where(x => x.RoomId == RoomIds[2]).Select(y => y.CorrectPoint).ToList();
        //        var Objectid = wasteCollection.Where(x => x.RoomId == RoomIds[2]).Select(y => y.ItemId).ToList();
        //        CMsobjectRoom3 = roomidLog;
        //        CMsObjectPCscore3 = RoomPcscore;
        //        CMsObjectCscore3 = RoomCscore;
        //        CmsObjectId3 = Objectid;
        //    }
        //}
        StartCoroutine(GetGameAttemptNoTask());
    }




    public void Initialtask(int roomno)
    {
        StartCoroutine(Initialactivity(roomno));
    }

    IEnumerator Initialactivity(int roomno)
    {
        room1_check = room2_check = room3_check = false;
        selectionpage.SetActive(false);
        Winelamp.color = new Color(1f, 1f, 1f, 0f);
        wiskeylamp.color = new Color(1f, 1f, 1f, 0f);
        Beerlamp.color = new Color(1f, 1f, 1f, 0f);
        this.gameObject.GetComponent<Image>().enabled = true;
        BarHeading.SetActive(true);
        TimerPage.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        waste_count = 0;
        knobangle = 0;
        collected_count = 0;
        room1_score = room2_score = room3_score = 0;
        level1score = 0;
        initial_pos_timer = timerpanel.GetComponent<RectTransform>().localPosition;
        //timerpanel.SetActive(true);
        timer.SetActive(true);
        mint = countDown_mint;
        Timeraction();
        timer.GetComponent<AudioSource>().enabled = true;
        timerstart = true;
        initailpos_dusbin = dustbin.GetComponent<RectTransform>().localPosition;
        Vector3 dustbinpos = dustbintarget.GetComponent<RectTransform>().localPosition;
        iTween.MoveTo(dustbin, iTween.Hash("position", dustbinpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 1f));
        subzones[roomno].SetActive(true);
        //home_btn.onClick.AddListener(delegate { yesclose(roomno); });
        yesbtn.onClick.AddListener(delegate { yesclose(roomno); });
        if (roomno == 0)
        {
            for (int a = 0; a < Containers.Count; a++)
            {
                Containers[a].gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = WinsName[a];
                Containers[a].gameObject.name = WinsContainername[a];
            }
            CurrentItemList = CMsobjectRoom1;
            CurrentPCscore = CMsObjectPCscore1;
            CurrentCscore = CMsObjectCscore1;
            currentItemId = CmsObjectId1;
            room1_check = true;
            rooms = room1;
            //StartCoroutine(GetBonusPointsLocal(RoomIds[0]));
        }
        else if (roomno == 1)
        {
            for (int a = 0; a < Containers.Count; a++)
            {
                Containers[a].gameObject.name = WiskeyConatinerName[a];
                Containers[a].gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = WiskeyName[a];
            }
            rooms = room2;
            CurrentItemList = CMsobjectRoom2;
            CurrentPCscore = CMsObjectPCscore2;
            CurrentCscore = CMsObjectCscore2;
            currentItemId = CmsObjectId2;
            room2_check = true;
            //StartCoroutine(GetBonusPointsLocal(RoomIds[1]));
        }
        else if (roomno == 2)
        {
            for (int a = 0; a < Containers.Count; a++)
            {
                Containers[a].gameObject.name = BeerContainername[a];
                Containers[a].gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = BeersName[a];
            }
            rooms = room3;
            CurrentItemList = CMsobjectRoom3;
            CurrentPCscore = CMsObjectPCscore3;
            CurrentCscore = CMsObjectCscore3;
            currentItemId = CmsObjectId3;
            room3_check = true;
            // StartCoroutine(GetBonusPointsLocal(RoomIds[2]));
        }
        backbtn.gameObject.SetActive(true);
        backbtn.onClick.RemoveAllListeners();
        backbtn.onClick.AddListener(delegate { backtozonepage(rooms); });

    }

    IEnumerator GetBonusPointsLocal(int room)
    {
        yield return new WaitForSeconds(0.1f);
        var BonusLog = dbmanager.Table<BonusTable>().FirstOrDefault(x => x.RoomId == room);
        Time0to30 = BonusLog.Time0to30;
        Time30to45 = BonusLog.Time30to45;
        Bonuspt1 = BonusLog.BonusPoint1;
        Bonuspt2 = BonusLog.BonusPoint2;
    }

    void Timeraction()
    {
        totalsecond = mint * 60f;
        mint = mint - 1;
        sec = 60;
        TotalSec = totalsecond;
        StartCoroutine(Countdowntimer());
    }
    public void OnDisable()
    {
        RoomIds.Clear();
        level1score = 0;
        room1_clear = room2_clear = room3_clear = false;
        mint = 0;
        scoretext.text = "";
        room1_data.Clear();
        room2_data.Clear();
        room3_data.Clear();
        is_win = false;
        timerwarining = true;
        scoreknob.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, 0f);
        level1score = 0;
        //leftdashboardbtn.SetActive(false);
    }


    void Update()
    {
        //---------------------------------------timer section with 30 sec warning condition--------------//
        if (mint == 0 && sec == 59 && timerwarining)
        {
            timerwarining = false;
            //timer_color_object.timertask();
        }

        if (mint == 0 && sec == 0 && timerstart)
        {
            Debug.Log("time don");
            timer.GetComponent<Coloreffect>().isdone = true;
            if (!is_win)
            {
                if (room1_check)
                {
                    room1_check = false;
                    timerstart = false;
                    room1_clear = true;
                    room1_score = level1score - (room2_score + room3_score);
                    //Bonusscore_room1 = 0;
                    timer.GetComponent<AudioSource>().enabled = false;
                    if (!room2_clear)
                    {
                        nextroombtn.onClick.RemoveAllListeners();
                        yesbtn.onClick.RemoveAllListeners();
                        string msg = "Times up for this room, you can proceed with next room.";
                        backbtn.gameObject.SetActive(false);
                        StartCoroutine(showstatus(msg));
                        for (int a = 0; a < subzones.Count; a++)
                        {
                            if (subzones[a].gameObject.activeInHierarchy)
                            {
                                subzones[a].SetActive(false);
                            }
                        }
                        CurrentItemList = CMsobjectRoom2;
                        CurrentPCscore = CMsObjectPCscore2;
                        CurrentCscore = CMsObjectCscore2;
                        currentItemId = CmsObjectId2;
                        // StartCoroutine(GetBonusPointsLocal(RoomIds[1]));
                        yesbtn.onClick.RemoveAllListeners();
                        //home_btn.onClick.RemoveAllListeners();
                        //home_btn.onClick.AddListener(delegate { yesclose(1); });
                        yesbtn.onClick.AddListener(delegate { yesclose(1); });
                        nextroombtn.onClick.AddListener(delegate { movetonext(1, 0, 1); });
                    }
                    else if (!room3_clear)
                    {
                        nextroombtn.onClick.RemoveAllListeners();
                        yesbtn.onClick.RemoveAllListeners();
                        string msg = "Times up for this room, you can proceed with next room.";
                        backbtn.gameObject.SetActive(false);
                        StartCoroutine(showstatus(msg));
                        for (int a = 0; a < subzones.Count; a++)
                        {
                            if (subzones[a].gameObject.activeInHierarchy)
                            {
                                subzones[a].SetActive(false);
                            }
                        }
                        CurrentItemList = CMsobjectRoom3;
                        CurrentPCscore = CMsObjectPCscore3;
                        CurrentCscore = CMsObjectCscore3;
                        currentItemId = CmsObjectId3;
                        //StartCoroutine(GetBonusPointsLocal(RoomIds[2]));
                        yesbtn.onClick.RemoveAllListeners();
                        //home_btn.onClick.RemoveAllListeners();
                        //home_btn.onClick.AddListener(delegate { yesclose(2); });
                        yesbtn.onClick.AddListener(delegate { yesclose(2); });
                        nextroombtn.onClick.AddListener(delegate { movetonext(2, 1, 2); });
                    }
                    else
                    {
                        timerstart = false;
                        timertext.text = "Times Up!";
                        iTween.ScaleTo(timesuppage, Vector3.one, 1f);
                        backbtn.gameObject.SetActive(false);
                        for (int a = 0; a < subzones.Count; a++)
                        {
                            if (subzones[a].gameObject.activeInHierarchy)
                            {
                                subzones[a].SetActive(false);
                            }
                        }
                        timer.GetComponent<Coloreffect>().isdone = true;
                        timesupbtn.onClick.AddListener(delegate { timesup_action(); });
                    }

                }
                else if (room2_check)
                {
                    room2_check = false;
                    timerstart = false;
                    room2_clear = true;
                    timer.GetComponent<AudioSource>().enabled = false;
                    room2_score = level1score - (room1_score + room3_score);
                    // Bonusscore_room2 = 0;
                    if (!room3_clear)
                    {
                        nextroombtn.onClick.RemoveAllListeners();
                        yesbtn.onClick.RemoveAllListeners();
                        string msg = "Times up for this room, you can proceed with next room.";
                        backbtn.gameObject.SetActive(false);
                        StartCoroutine(showstatus(msg));
                        for (int a = 0; a < subzones.Count; a++)
                        {
                            if (subzones[a].gameObject.activeInHierarchy)
                            {
                                subzones[a].SetActive(false);
                            }
                        }
                        CurrentItemList = CMsobjectRoom3;
                        CurrentPCscore = CMsObjectPCscore3;
                        CurrentCscore = CMsObjectCscore3;
                        currentItemId = CmsObjectId3;
                        //StartCoroutine(GetBonusPointsLocal(RoomIds[2]));
                        yesbtn.onClick.RemoveAllListeners();
                        //home_btn.onClick.RemoveAllListeners();
                        //home_btn.onClick.AddListener(delegate { yesclose(2); });
                        yesbtn.onClick.AddListener(delegate { yesclose(2); });
                        nextroombtn.onClick.AddListener(delegate { movetonext(2, 1, 2); });
                    }
                    else
                    {
                        timerstart = false;
                        timertext.text = "Times Up!";
                        iTween.ScaleTo(timesuppage, Vector3.one, 1f);
                        backbtn.gameObject.SetActive(false);
                        for (int a = 0; a < subzones.Count; a++)
                        {
                            if (subzones[a].gameObject.activeInHierarchy)
                            {
                                subzones[a].SetActive(false);
                            }
                        }
                        timer.GetComponent<Coloreffect>().isdone = true;
                        timesupbtn.onClick.AddListener(delegate { timesup_action(); });
                    }

                }
                else if (room3_check)
                {
                    room3_check = false;
                    timerstart = false;
                    room3_clear = true;
                    timer.GetComponent<AudioSource>().enabled = false;
                    //Bonusscore_room3 = 0;
                    room3_score = level1score - (room1_score + room2_score);
                    if (!room1_clear)
                    {
                        nextroombtn.onClick.RemoveAllListeners();
                        yesbtn.onClick.RemoveAllListeners();
                        string msg = "Times up for this room, you can proceed with next room.";
                        backbtn.gameObject.SetActive(false);
                        StartCoroutine(showstatus(msg));
                        for (int a = 0; a < subzones.Count; a++)
                        {
                            if (subzones[a].gameObject.activeInHierarchy)
                            {
                                subzones[a].SetActive(false);
                            }
                        }
                        CurrentItemList = CMsobjectRoom1;
                        CurrentPCscore = CMsObjectPCscore1;
                        CurrentCscore = CMsObjectCscore1;
                        currentItemId = CmsObjectId1;
                        //StartCoroutine(GetBonusPointsLocal(RoomIds[0]));
                        yesbtn.onClick.RemoveAllListeners();
                        //home_btn.onClick.RemoveAllListeners();
                        //home_btn.onClick.AddListener(delegate { yesclose(0); });
                        yesbtn.onClick.AddListener(delegate { yesclose(0); });
                        nextroombtn.onClick.AddListener(delegate { movetonext(0, 1, 0); });
                    }
                    else
                    {
                        timerstart = false;
                        timertext.text = "Times Up!";
                        iTween.ScaleTo(timesuppage, Vector3.one, 1f);
                        backbtn.gameObject.SetActive(false);
                        for (int a = 0; a < subzones.Count; a++)
                        {
                            if (subzones[a].gameObject.activeInHierarchy)
                            {
                                subzones[a].SetActive(false);
                            }
                        }
                        timer.GetComponent<Coloreffect>().isdone = true;
                        timesupbtn.onClick.AddListener(delegate { timesup_action(); });
                    }

                }
            }
        }
        //-==========================================================================================//
        if (waste_count == room1.Count && room1_check)
        {
            room1_check = false;
            room1name = "Kitchen";
            room1_clear = true;
            room1_score = level1score - (room2_score + room3_score);
            float runningTime = 60 - sec;
            int TempScore = 0;
            CurrentCscore.ForEach(x =>
            {
                TempScore += x;
            });

            if (runningTime < Time0to30 && TempScore == room1_score)
            {
                totalBonusScore += Bonuspt1;
            }
            else if (runningTime > Time0to30 && runningTime < Time30to45 && TempScore == room1_score)
            {
                totalBonusScore += Bonuspt2;
            }

            if (room1_clear && room2_clear && room3_clear)
            {
                // Bonusscore_room1 = Bonus_Score;
                StartCoroutine(zone_completiontask(0));
            }
            else
            {

                timerstop = true;
                nextroombtn.onClick.RemoveAllListeners();
                yesbtn.onClick.RemoveAllListeners();
                string msg = "You have collected all the bottles. You can move to next bar room.";
                backbtn.gameObject.SetActive(false);
                StartCoroutine(showstatus(msg));
                if (!room2_clear)
                {
                    yesbtn.onClick.RemoveAllListeners();
                    //home_btn.onClick.RemoveAllListeners();
                    //home_btn.onClick.AddListener(delegate { yesclose(1); });
                    yesbtn.onClick.AddListener(delegate { yesclose(1); });
                    nextroombtn.onClick.AddListener(delegate { movetonext(1, 0, 1); });
                    CurrentItemList = CMsobjectRoom2;
                    CurrentPCscore = CMsObjectPCscore2;
                    CurrentCscore = CMsObjectCscore2;
                    currentItemId = CmsObjectId2;
                    // StartCoroutine(GetBonusPointsLocal(RoomIds[1]));
                }
                else if (!room3_clear)
                {
                    yesbtn.onClick.RemoveAllListeners();
                    //home_btn.onClick.RemoveAllListeners();
                    //home_btn.onClick.AddListener(delegate { yesclose(2); });
                    yesbtn.onClick.AddListener(delegate { yesclose(2); });
                    nextroombtn.onClick.AddListener(delegate { movetonext(2, 0, 2); });
                    CurrentItemList = CMsobjectRoom3;
                    CurrentPCscore = CMsObjectPCscore3;
                    CurrentCscore = CMsObjectCscore3;
                    currentItemId = CmsObjectId3;
                    //StartCoroutine(GetBonusPointsLocal(RoomIds[2]));
                }

            }

        }
        if (waste_count == room2.Count && room2_check)
        {
            room2_check = false;
            room2_clear = true;
            room2name = "Bedroom";
            float runningTime = 60 - sec;
            room2_score = level1score - (room1_score + room3_score);
            int TempScore = 0;
            CurrentCscore.ForEach(x =>
            {
                TempScore += x;
            });

            if (runningTime < Time0to30 && TempScore == room2_score)
            {
                totalBonusScore += Bonuspt1;
            }
            else if (runningTime > Time0to30 && runningTime < Time30to45 && TempScore == room2_score)
            {
                totalBonusScore += Bonuspt2;
            }
            if (room1_clear && room2_clear && room3_clear)
            {
                // Bonusscore_room2 = Bonus_Score;
                StartCoroutine(zone_completiontask(1));
            }
            else
            {
                timerstop = true;
                nextroombtn.onClick.RemoveAllListeners();
                yesbtn.onClick.RemoveAllListeners();
                string msg = "You have collected all the bottles. You can move to next bar room.";
                backbtn.gameObject.SetActive(false);
                StartCoroutine(showstatus(msg));

                if (!room1_clear)
                {
                    yesbtn.onClick.RemoveAllListeners();
                    //home_btn.onClick.RemoveAllListeners();
                    //home_btn.onClick.AddListener(delegate { yesclose(0); });
                    yesbtn.onClick.AddListener(delegate { yesclose(0); });
                    nextroombtn.onClick.AddListener(delegate { movetonext(0, 1, 0); });
                    CurrentItemList = CMsobjectRoom1;
                    CurrentPCscore = CMsObjectPCscore1;
                    CurrentCscore = CMsObjectCscore1;
                    currentItemId = CmsObjectId1;
                    // StartCoroutine(GetBonusPointsLocal(RoomIds[0]));
                }
                else if (!room3_clear)
                {
                    yesbtn.onClick.RemoveAllListeners();
                    //home_btn.onClick.RemoveAllListeners();
                    //home_btn.onClick.AddListener(delegate { yesclose(2); });
                    yesbtn.onClick.AddListener(delegate { yesclose(2); });
                    nextroombtn.onClick.AddListener(delegate { movetonext(2, 1, 2); });
                    CurrentItemList = CMsobjectRoom3;
                    CurrentPCscore = CMsObjectPCscore3;
                    CurrentCscore = CMsObjectCscore3;
                    currentItemId = CmsObjectId3;
                    //StartCoroutine(GetBonusPointsLocal(RoomIds[2]));
                }
            }

        }
        if (waste_count == room3.Count && room3_check)
        {
            room3_check = false;
            room3_clear = true;
            room3name = "Livingroom";
            float runningTime = 60 - sec;
            room3_score = level1score - (room2_score + room1_score);
            int TempScore = 0;
            CurrentCscore.ForEach(x =>
            {
                TempScore += x;
            });

            if (runningTime < Time0to30 && TempScore == room3_score)
            {
                totalBonusScore += Bonuspt1;
            }
            else if (runningTime > Time0to30 && runningTime < Time30to45 && TempScore == room3_score)
            {
                totalBonusScore += Bonuspt2;
            }
            if (room1_clear && room2_clear && room3_clear)
            {
                // Bonusscore_room3 = Bonus_Score; 
                StartCoroutine(zone_completiontask(2));
            }
            else
            {
                timerstop = true;
                nextroombtn.onClick.RemoveAllListeners();
                yesbtn.onClick.RemoveAllListeners();
                string msg = "You have collected all the bottles. You can move to next bar room.";
                backbtn.gameObject.SetActive(false);
                StartCoroutine(showstatus(msg));
                if (!room1_clear)
                {
                    yesbtn.onClick.RemoveAllListeners();
                    //home_btn.onClick.RemoveAllListeners();
                    //home_btn.onClick.AddListener(delegate { yesclose(0); });
                    yesbtn.onClick.AddListener(delegate { yesclose(0); });
                    nextroombtn.onClick.AddListener(delegate { movetonext(0, 2, 0); });
                    CurrentItemList = CMsobjectRoom1;
                    CurrentPCscore = CMsObjectPCscore1;
                    CurrentCscore = CMsObjectCscore1;
                    currentItemId = CmsObjectId1;
                }
                else if (!room2_clear)
                {
                    yesbtn.onClick.RemoveAllListeners();
                    //home_btn.onClick.RemoveAllListeners();
                    //home_btn.onClick.AddListener(delegate { yesclose(1); });
                    yesbtn.onClick.AddListener(delegate { yesclose(1); });
                    nextroombtn.onClick.AddListener(delegate { movetonext(1, 2, 1); });
                    CurrentItemList = CMsobjectRoom2;
                    CurrentPCscore = CMsObjectPCscore2;
                    CurrentCscore = CMsObjectCscore2;
                    currentItemId = CmsObjectId2;
                }
            }

        }

        //if (score_check)
        //{
        //    score_check = false;
        //    collected_text.GetComponent<Text>().text = collected_count.ToString();
        //    scorepanel.GetComponent<shakeeffect>().enabled = true;
        //    Invoke("stopshake", 1.5f);
        //    scoretext.text = level1score.ToString();
        //    knobangle = (level1score / totalscore) * 200;

        //}
        //var rotationangle = Quaternion.Euler(0f, 0f, -knobangle);
        //scoreknob.GetComponent<RectTransform>().rotation = Quaternion.Lerp(scoreknob.GetComponent<RectTransform>().rotation, rotationangle, 10 * 1 * Time.deltaTime);

        //if (action_plan_page.activeInHierarchy && action_check)
        //{
        //    if (actionplan_score == 0)
        //    {

        //    }
        //    else
        //    {
        //        action_check = false;
        //        float total_scored;
        //        float level_score;
        //        score_text.text = level1score.ToString();
        //        total_score_text.text = (level1score + actionplan_score).ToString();
        //        total_scored = level1score + actionplan_score;
        //        total_score_img.fillAmount = total_scored / total_score_game;
        //        level_score = level1score;
        //        scored_image.fillAmount = level_score / total_score_game;
        //        float plan_score = (float)actionplan_score;
        //        action_plan_image.fillAmount = plan_score / 100.00f;
        //        actionplan_text.text = actionplan_score.ToString();
        //    }
        //}

        //if (RoomPageCounter == 0)
        //{
        //    LeftPage.gameObject.SetActive(false);
        //    RightPage.gameObject.SetActive(true);

        //}
        //else if (RoomPageCounter > 0 && RoomPageCounter < RoomNames.Count - 1)
        //{
        //    LeftPage.gameObject.SetActive(true);
        //    RightPage.gameObject.SetActive(true);
        //}
        //else if (RoomPageCounter < RoomNames.Count)
        //{
        //    RightPage.gameObject.SetActive(false);
        //}
        //if (!Game_over_time)
        //{
        //    Time_taken();
        //}

    }

    //private void Time_taken()
    //{
    //    pri_sec += pri_sec + Time.deltaTime;
    //    if (pri_sec > 59.00)
    //    {
    //        pri_time += 1;
    //        pri_sec = 0.0f;
    //    }
    //}


    void stopshake()
    {
        scorepanel.GetComponent<shakeeffect>().enabled = false;
    }
    public void scoreupdated()
    {
        scoretext.text = level1score.ToString();
        float knobangle = (level1score / totalscore) * 200;
        scoreknob.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, -knobangle);
    }

    IEnumerator showstatus(string msg)
    {
        yield return new WaitForSeconds(1.5f);
        waste_count = 0;
        Done_msg_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = msg;
        iTween.ScaleTo(Done_msg_panel, Vector3.one, 1f);
    }

    void movetonext(int roomsprite, int lastroom, int zoneactive)
    {

        timer.GetComponent<Coloreffect>().isdone = true;
        if (zoneactive == 0)
        {
            for (int a = 0; a < Containers.Count; a++)
            {
                Containers[a].gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = WinsName[a];
                Containers[a].gameObject.name = WinsContainername[a];
            }
            backbtn.onClick.RemoveAllListeners();
            collected_count = 0;
            backbtn.onClick.AddListener(delegate { backtozonepage(room1); });
            room1_check = true;
        }
        else if (zoneactive == 1)
        {
            for (int a = 0; a < Containers.Count; a++)
            {
                Containers[a].gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = WiskeyName[a];
                Containers[a].gameObject.name = WiskeyConatinerName[a];
            }
            collected_count = 0;
            backbtn.onClick.RemoveAllListeners();
            backbtn.onClick.AddListener(delegate { backtozonepage(room2); });
            room2_check = true;
        }
        else if (zoneactive == 2)
        {
            for (int a = 0; a < Containers.Count; a++)
            {
                Containers[a].gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = BeersName[a];
                Containers[a].gameObject.name = BeerContainername[a];
            }
            collected_count = 0;
            room3_check = true;
            backbtn.onClick.RemoveAllListeners();
            backbtn.onClick.AddListener(delegate { backtozonepage(room3); });
        }
        waste_count = 0;
        iTween.ScaleTo(Done_msg_panel, Vector3.zero, 1f);
        Done_msg_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        StartCoroutine(nextroom(roomsprite, lastroom, zoneactive));
    }

    IEnumerator nextroom(int roomsprite, int lastroom, int zoneactive)
    {
        subzones[lastroom].SetActive(false);
        yield return new WaitForSeconds(0.1f);
        //StartCoroutine(startpage.scenechanges(startpageobj, subzonesprite[roomsprite]));
        yield return new WaitForSeconds(1.5f);
        subzones[zoneactive].SetActive(true);
        mint = countDown_mint;
        if (timerstop)
        {
            timerstop = false;
            Timeraction();
        }
        else
        {
            totalsecond = mint * 60f;
            mint = mint - 1;
            sec = 60;
            TotalSec = totalsecond;
        }

        timerwarining = true;
        backbtn.gameObject.SetActive(true);
        timer.GetComponent<AudioSource>().enabled = true;
        timerstart = true;
        collected_text.GetComponent<Text>().text = "0";
        Vector3 dustbinpos = dustbintarget.GetComponent<RectTransform>().localPosition;
        iTween.MoveTo(dustbin, iTween.Hash("position", dustbinpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 1f));

    }
    public void backtozonepage(List<GameObject> roomobject)
    {
        if (waste_count == roomobject.Count)
        {

        }
        else
        {
            if (Done_msg_panel.transform.localScale == Vector3.one)
            {
                iTween.ScaleTo(Done_msg_panel, Vector3.zero, 0.2f);
                exit_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "You have not found all the bottles. Do you really want to exit the game!";
                iTween.ScaleTo(exit_panel, Vector3.one, 0.6f);
                CloseDoublePopup = true;

            }
            else
            {
                exit_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "You have not found all the bottles. Do you really want to exit the game!";
                iTween.ScaleTo(exit_panel, Vector3.one, 0.6f);
                CloseDoublePopup = false;
            }


        }
    }


    void yesclose(int subzonevalue)
    {
        zone_completed = true;
        StartCoroutine(backtozone(subzonevalue));
        mint = 0;
        collected_text.GetComponent<Text>().text = "0";
        iTween.MoveTo(timerpanel, iTween.Hash("position", initial_pos_timer, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 1f));
        iTween.MoveTo(dustbin, iTween.Hash("position", initailpos_dusbin, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 1f));
    }

    IEnumerator backtozone(int subzone)
    {
        yield return new WaitForSeconds(0.1f);
        iTween.ScaleTo(exit_panel, Vector3.zero, 0.8f);
        exit_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        yield return new WaitForSeconds(1f);
        timerpanel.SetActive(false);
        subzones[subzone].SetActive(false);
        Destroy(homeinstance);
        SceneManager.LoadScene(0);
    }
    public void noclose()
    {
        iTween.ScaleTo(exit_panel, Vector3.zero, 0.5f);
        exit_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        if (CloseDoublePopup)
        {
            iTween.ScaleTo(Done_msg_panel, Vector3.one, 0.2f);
            CloseDoublePopup = false;
        }


    }

    public void zonedone(int lastroom)
    {
        StartCoroutine(zonecomplete(lastroom));
        iTween.MoveTo(dustbin, iTween.Hash("position", initailpos_dusbin, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 1f));
    }

    IEnumerator zonecomplete(int lastroom)
    {
        zone_completed = true;
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < tab1_object.Count; i++)
        {
            Destroy(tab1_object[i].gameObject);
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < tab2_object.Count; i++)
        {
            Destroy(tab2_object[i].gameObject);
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < tab3_object.Count; i++)
        {
            Destroy(tab3_object[i].gameObject);
            yield return new WaitForSeconds(0.1f);
        }
        for (int a = 0; a < tabs.Count; a++)
        {
            Destroy(tabs[a].gameObject);
            yield return new WaitForSeconds(0.2f);
        }
        for (int a = 0; a < tabs.Count; a++)
        {

            Destroy(tabs[a].gameObject);
            yield return new WaitForSeconds(0.2f);
        }
        collected_text.GetComponent<Text>().text = "0";
        room1_data_collected.Clear();
        room2_data_collected.Clear();
        room3_data_collected.Clear();
        bonus_page.SetActive(false);
        level1score = 0;
        iTween.ScaleTo(Done_msg_panel, Vector3.zero, 0.6f);
        Done_msg_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        yield return new WaitForSeconds(0.6f);
        subzones[lastroom].SetActive(false);
        StartCoroutine(startpage.scenechanges(startpageobj, mainzone));
        yield return new WaitForSeconds(1.5f);
        Camera.main.GetComponent<AudioSource>().enabled = true;
        zonepage.SetActive(true);
        this.gameObject.SetActive(false);
    }

    void timesup_action()
    {
        mint = 0;
        timesupbtn.onClick.RemoveAllListeners();
        iTween.ScaleTo(timesuppage, Vector3.zero, 1f);
        //next_Zone.onClick.RemoveAllListeners();
        //next_Zone.onClick.AddListener(delegate { after_timeup_action(); });
        knobangle = 0;
        iTween.MoveTo(timerpanel, iTween.Hash("position", initial_pos_timer, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.6f));
        iTween.MoveTo(dustbin, iTween.Hash("position", initailpos_dusbin, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.6f));
        Final_dashboard();
    }

    void after_timeup_action()
    {
        StartCoroutine(after_timeup());
    }
    IEnumerator after_timeup()
    {
        zone_completed = true;
        yield return new WaitForSeconds(0.8f);
        timerpanel.SetActive(false);
        timer.GetComponent<Coloreffect>().enabled = false;
        for (int i = 0; i < tab1_object.Count; i++)
        {
            Destroy(tab1_object[i].gameObject);
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < tab2_object.Count; i++)
        {
            Destroy(tab2_object[i].gameObject);
            yield return new WaitForSeconds(0.1f);
        }
        for (int i = 0; i < tab3_object.Count; i++)
        {
            Destroy(tab3_object[i].gameObject);
            yield return new WaitForSeconds(0.1f);
        }
        collected_text.GetComponent<Text>().text = "0";
        room1_data_collected.Clear();
        room2_data_collected.Clear();
        room3_data_collected.Clear();
        for (int a = 0; a < tabs.Count; a++)
        {
            Destroy(tabs[a].gameObject);
            yield return new WaitForSeconds(0.2f);
        }
        timer.SetActive(false);
        for (int a = 0; a < subzones.Count; a++)
        {
            if (subzones[a].gameObject.activeInHierarchy)
            {
                subzones[a].SetActive(false);
            }
        }
        level1score = 0;
        StartCoroutine(startpage.scenechanges(startpageobj, mainzone));
        yield return new WaitForSeconds(1.5f);
        Camera.main.GetComponent<AudioSource>().enabled = true;
        zonepage.SetActive(true);
        this.gameObject.SetActive(false);
    }

    //------------------stopwatchtimer---------------------------------//
    public IEnumerator Countdowntimer()
    {

        yield return new WaitForSecondsRealtime(1f);
        if (sec > 0)
        {
            sec--;
        }

        if (sec == 0 && mint != 0)
        {
            mint--;
            sec = 60;
        }
        if (sec.ToString().Length > 1)
        {
            timertext.text = "0" + mint + " : " + sec;
        }
        else
        {
            timertext.text = "0" + mint + " : 0" + sec;
        }

        Timefilling();
        if (!timerstop)
        {
            StartCoroutine(Countdowntimer());
        }

    }

    void Timefilling()
    {
        totalsecond--;
        float fill = (float)totalsecond / TotalSec;
        timerimage.fillAmount = fill;
    }

    void initialback()
    {
        StartCoroutine(initialback_task());

    }
    IEnumerator initialback_task()
    {
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(startpage.scenechanges(startpageobj, mainzone));
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(false);
        Camera.main.GetComponent<AudioSource>().enabled = true;
        zonepage.SetActive(true);
    }



    IEnumerator zone_completiontask(int zone)
    {
        is_win = true;
        //Game_over_time = true;
        yield return new WaitForSeconds(0f);
        timer.GetComponent<AudioSource>().enabled = false;
        string donemsg = "Ok, you have completed the room, By the way, you have to get at least 70% score to get the clear bar game badge";
        nextroombtn.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Go Ahead!";
        nextroombtn.onClick.RemoveAllListeners();
        nextroombtn.onClick.AddListener(delegate { Final_dashboard(); });
        timerstop = true;
        mint = 0;
        final_completed = true;
        timer.GetComponent<Coloreffect>().isdone = false;
        yield return new WaitForSeconds(1.5f);
        iTween.MoveTo(dustbin, iTween.Hash("position", initailpos_dusbin, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
        iTween.MoveTo(timerpanel, iTween.Hash("position", initial_pos_timer, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
        yield return new WaitForSeconds(0.6f);
        timerpanel.SetActive(false);
        StartCoroutine(showstatus(donemsg));
        //next_Zone.onClick.RemoveAllListeners();
        //next_Zone.onClick.AddListener(delegate { zonedone(zone); });
        active_room_end = zone;

    }


    public void CheckcollisionResult(string container, string bottlename)
    {
        float containerpos = 0f;
        Containers.ForEach(x =>
        {
            if (x.name.Equals(container, System.StringComparison.OrdinalIgnoreCase))
            {
                containerpos = x.transform.localPosition.x;
            }
        });
        var wasteType = Bardata.FirstOrDefault(x => x.Key.Equals(container, System.StringComparison.OrdinalIgnoreCase));
        var isBelongToBottleType = wasteType.Value.
            Any(x => x.Equals(bottlename, System.StringComparison.OrdinalIgnoreCase));


        if (isBelongToBottleType)
        {
            LightObject.GetComponent<LightEffect>().iscorrect = true;
            LightObject.GetComponent<LightEffect>().enabled = true;
            barobject.GetComponent<LightEffect>().iscorrect = true;
            barobject.GetComponent<LightEffect>().enabled = true;

            var index = CurrentItemList.FindIndex(x => x.Equals(bottlename, System.StringComparison.OrdinalIgnoreCase));
            CorrectEfffect.transform.localPosition = new Vector3(containerpos, CorrectEfffect.transform.localPosition.y, 0);
            CorrectEfffect.SetActive(true);
            popupScore.GetComponent<RectTransform>().localPosition = new Vector3(containerpos, popupScore.GetComponent<RectTransform>().localPosition.y, 0);
            Scorestatus.GetComponent<RectTransform>().localPosition = new Vector3(containerpos, Scorestatus.GetComponent<RectTransform>().localPosition.y, 0);
            popupScore.text = "+" + CurrentCscore[index].ToString();
            Scorestatus.text = "Spot on!";
            Debug.Log("Same type");
            StartCoroutine(reseteffect());
            level1score += CurrentCscore[index];
            Bottle_score.Add(CurrentCscore[index]);
            Bottle_is_correct.Add(1);
            scoretext.text = level1score.ToString();
            BottleId.Add(currentItemId[index]);
            Bottle_correct_opt.Add(container);
            Bottlecollected.Add(bottlename.ToLower().Trim());
            Bottle_container.Add(container);
        }
        else
        {
            LightObject.GetComponent<LightEffect>().iscorrect = false;
            LightObject.GetComponent<LightEffect>().enabled = true;
            barobject.GetComponent<LightEffect>().iscorrect = false;
            barobject.GetComponent<LightEffect>().enabled = true;
            var RelatedDustbin = (from k in Bardata
                                  where k.Value.Any(x => x.Equals(bottlename, System.StringComparison.OrdinalIgnoreCase))
                                  select k.Key).FirstOrDefault();
            var index = CurrentItemList.FindIndex(x => x.Equals(bottlename, System.StringComparison.OrdinalIgnoreCase));
            WrongEffect.transform.localPosition = new Vector3(containerpos, WrongEffect.transform.localPosition.y, 0);
            WrongEffect.SetActive(true);
            popupScore.GetComponent<RectTransform>().localPosition = new Vector3(containerpos, popupScore.GetComponent<RectTransform>().localPosition.y, 0);
            Scorestatus.GetComponent<RectTransform>().localPosition = new Vector3(containerpos, Scorestatus.GetComponent<RectTransform>().localPosition.y, 0);
            popupScore.text = "+" + CurrentPCscore[index].ToString();
            Scorestatus.text = "You are high!";
            Debug.Log("wrong type");
            StartCoroutine(reseteffect());
            level1score += CurrentPCscore[index];
            Bottle_score.Add(CurrentPCscore[index]);
            Bottle_is_correct.Add(0);
            scoretext.text = level1score.ToString();
            BottleId.Add(currentItemId[index]);
            Bottle_correct_opt.Add(RelatedDustbin);
            Bottlecollected.Add(bottlename.ToLower().Trim());
            Bottle_container.Add(container);
        }
        Debug.Log("waste count " + waste_count);
    }

    IEnumerator reseteffect()
    {
        yield return new WaitForSeconds(1.25f);
        popupScore.text = "";
        Scorestatus.text = "";
    }


    public void GotoHome()
    {
        int index = 0;
        Destroy(homeinstance);
        StartCoroutine(Hometask(index));
    }
    IEnumerator Hometask(int index)
    {
        iTween.ScaleTo(PopupPage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        msgbox.text = "";
        PopupPage.SetActive(false);
        SceneManager.LoadScene(index);
    }

    public void restartGame()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(Hometask(index));
    }

  


    void Final_dashboard()
    {
        Done_msg_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        iTween.ScaleTo(Done_msg_panel, Vector3.zero, 0.4f);
        int totalScore = 0;
        
        var data = dbmanager.Table<ObjectGameList>().ToList();

        data.ForEach(d =>
        {
            if (d.RoomId == RoomIds[0] || d.RoomId == RoomIds[1] || d.RoomId == RoomIds[2])
            {
                totalscore += d.CorrectPoint;
            }
        });

        float percentage = ((float)level1score / (float)totalscore) * 100;
        string Msg;
        if (percentage > 75.0f)
        {
             Msg = "Congraulation! you have cleared this Game.";
            AvatarMood.sprite = happy;
            //StartCoroutine(showstatus(msg));
        }
        else
        {
            AvatarMood.sprite = sad;
             Msg = "Sorry! you are too high for this game.";
            //StartCoroutine(showstatus(msg));
        }
        msgbox.text = Msg;
        PopupPage.SetActive(true);
        var AllItems = data.Where(x => x.RoomId == RoomIds[0] || x.RoomId == RoomIds[1] || x.RoomId == RoomIds[2]).Select(y => new
        {
            ItemId = y.ItemId,
            ItemName = y.ItemName
        }).ToList();

        var DistinctElements = Bottlecollected != null && Bottlecollected.Count > 0 ? AllItems.Where(a => !Bottlecollected.Contains(a.ItemName.ToLower().Trim())).Select(b => b.ItemName.ToLower()).ToList() : new List<string>();
        var DistinctItemId = BottleId != null && BottleId.Count > 0 ? AllItems.Where(c => !BottleId.Contains(c.ItemId)).Select(d => d.ItemId).ToList() : new List<int>();


        for (int a = 0; a < DistinctElements.Count; a++)
        {
            if (!Bottlecollected.Contains(DistinctElements[a]))
            {
                Bottlecollected.Add(DistinctElements[a]);
            }
            BottleId.Add(DistinctItemId[a]);
            Bottle_is_correct.Add(0);
            Bottle_container.Add("null");
            Bottle_score.Add(0);
            var RelatedDustbin = (from k in Bardata
                                  where k.Value.Any(x => x.Equals(DistinctElements[a], System.StringComparison.OrdinalIgnoreCase))
                                  select k.Key).FirstOrDefault();
            Bottle_correct_opt.Add(RelatedDustbin);
        }


        var AllRoomIdLog = dbmanager.Table<ObjectGameList>().ToList();
        Bottlecollected.ForEach(a =>
        {
            var Log = AllRoomIdLog.Where(x => x.ItemName.ToLower().Trim() == a).FirstOrDefault();
            LocalroomId.Add(Log.RoomId);
        });


        var Logs = new List<ObjectGamePostModel>();
        int l = 0;
        Bottlecollected.ForEach(x =>
        {
            var log = new ObjectGamePostModel()
            {
                item_collected = Bottlecollected[l],
                is_right = Bottle_is_correct[l],
                id_user = PlayerPrefs.GetInt("UID"),
                Id_Game = Id_game,
                attempt_no = GameAttemptNumber + 1,
                item_Id = BottleId[l],
                dustbin = Bottle_container[l],
                score = Bottle_score[l],
                id_room = LocalroomId[l],
                correct_option = Bottle_correct_opt[l]
            };
            l = l + 1;
            Logs.Add(log);
        });

        string jsondata = Newtonsoft.Json.JsonConvert.SerializeObject(Logs);
        Debug.Log("Complete log " + jsondata);

        StartCoroutine(Post_data(jsondata));
        StartCoroutine(GameScorePosting());

    }

    public void ClosePopupPage()
    {
        StartCoroutine(CloselastPage());
    }

    IEnumerator CloselastPage()
    {
        iTween.ScaleTo(PopupPage, Vector3.zero,0.4f);
        yield return new WaitForSeconds(0.3f);
        msgbox.text = "";
        PopupPage.SetActive(false);
        LeaderBoardPage.SetActive(true);
    }
    IEnumerator GameScorePosting()
    {
        yield return new WaitForSeconds(0.1f);
        string hittingUrl = $"{MainUrls.BaseUrl}{MainUrls.MasterLogApi}";
        NgageMasterPostLog PostField = new NgageMasterPostLog()
        {
            id_user = PlayerPrefs.GetInt("UID"),
            ID_ORGANIZATION = PlayerPrefs.GetInt("OID"),
            score = level1score,
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

    IEnumerator Post_data(string Jsondata) 
    {
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.ObjectGamePostApi}";

        AESAlgorithm aes = new AESAlgorithm();
        string Encrypteddata = aes.getEncryptedString(Jsondata);
        CommonModel model = new CommonModel()
        {
            Data = Encrypteddata
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

}
