using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using LitJson;
using UnityEngine.UI;

public class Stage2OervallDashbaord : MonoBehaviour
{
    public string MainUrl, DashBoard_Api, GetGamesIDApi, RoomData_api;
    public int Gamelevel;
    public string ZoneName;
    private int id_game_content;
    public List<int> RoomIds;
    public Transform Level1Holder, level2Holder;
    public GameObject Level1datarow, Level2datarow;
    public GameObject showmsg;
    public GameObject dashboardpanel;
    public Text TotalscoreL1, totalScoreL2;
    public Sprite Correct, Wrong, Pcorrect, Correctopt;
    private int ScoreCounterL1, ScoreCounterL2;
    private List<string> ObjectnameL1 = new List<string>();
    private List<string> ObjectnameL2 = new List<string>();
    private List<int> ObjectSCoreL1 = new List<int>();
    private List<int> ObjectSCoreL2 = new List<int>();
    private List<int> is_correctL1 = new List<int>();
    private List<int> is_correctL2 = new List<int>();
    private List<string> dustbinL1 = new List<string>();
    private List<string> dustbinL2 = new List<string>();
    private List<string> CorrectOptL1 = new List<string>();
    private List<string> CorrectOptL2 = new List<string>();
    private string correct_option;
    private List<GameObject> WasteObjectL1= new List<GameObject>();
    private List<GameObject> WasteObjectL2 = new List<GameObject>();
    public Stage2Menu Stage2data;


    void Start()
    {
        
    }

    private void OnEnable()
    {
        correct_option = "";
        //WasteObjectL1 = new List<GameObject>();
        //WasteObjectL2 = new List<GameObject>();
        RoomIds = new List<int>();
        StartCoroutine(GetGamesIDactivity());
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    IEnumerator GetGamesIDactivity()
    {
        string HittingUrl = MainUrl + GetGamesIDApi + "?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID") +
            "&id_org_game=" + 1 + "&id_level=" + Gamelevel;
        WWW GameResponse = new WWW(HittingUrl);
        yield return GameResponse;
        if (GameResponse.text != null)
        {
            Debug.Log("game id data " + GameResponse.text);
            GetLevelIDs gameIDs = Newtonsoft.Json.JsonConvert.DeserializeObject<GetLevelIDs>(GameResponse.text);
            var ContentList = gameIDs.content.ToList();
            id_game_content = ContentList.FirstOrDefault(x => x.title == ZoneName).id_game_content;
            Debug.Log("Game id for " + ZoneName + " id: " + id_game_content);
            StartCoroutine(CollectRoomdata());
        }
    }
    IEnumerator CollectRoomdata()
    {

        string Hittingurl = MainUrl + RoomData_api + "?id_user=" + PlayerPrefs.GetInt("UID") + "&id_org_content=" + id_game_content;
        Debug.Log("main Url " + Hittingurl);
        WWW roominfo = new WWW(Hittingurl);
        yield return roominfo;
        if (roominfo.text != null)
        {
            Debug.Log("rooom id " + roominfo.text);
            JsonData RoomrRes = JsonMapper.ToObject(roominfo.text);
            for (int a = 0; a < RoomrRes.Count; a++)
            {
                RoomIds.Add(int.Parse(RoomrRes[a]["id_room"].ToString()));
            }
            StartCoroutine(GetDashboardData());
        }
    }

    IEnumerator GetDashboardData()
    {
        int MaxAttemptNum = 0;
        string Response_url = MainUrl + DashBoard_Api + "?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID") +
         "&id_org_game=" + 1;
        WWW dashboardRes = new WWW(Response_url);
        yield return dashboardRes;
        if (dashboardRes.text != null)
        {
            Debug.Log("data " + dashboardRes.text);
            List<DashboardItem> DashboardHandler = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DashboardItem>>(dashboardRes.text);
            var Contentlist = DashboardHandler.Where(x => x.id_level == Gamelevel).Select(x => x.ContentList).FirstOrDefault();
            var GotZoneData = Contentlist?.Where(x => x.id_game_content == id_game_content).Select(x => x.UserLog).FirstOrDefault();
            var listUserLog = Contentlist?.Where(x => x.id_game_content == id_game_content).SelectMany(x => x.UserLog).ToList();
        
            if(listUserLog.Count > 0)
            {
                var data = listUserLog.Max(x => x.attempt_no);
                MaxAttemptNum = data;
                Debug.Log("got the final data for stage 2 " + data);
            }
            if (GotZoneData.Count > 0)
            {
                GotZoneData.ForEach(x =>
                {
                    if (x.id_room == RoomIds[0] && x.attempt_no == MaxAttemptNum)
                    {
                        ObjectnameL1.Add(x.item_collected);
                        ObjectSCoreL1.Add(x.score);
                        is_correctL1.Add(x.is_right);
                        correct_option = x.correct_option != "" ? x.correct_option : null;
                        CorrectOptL1.Add(correct_option);
                        dustbinL1.Add(x.dustbin);
                    }
                    if (x.id_room == RoomIds[1] && x.attempt_no == MaxAttemptNum)
                    {
                        ObjectnameL2.Add(x.item_collected);
                        ObjectSCoreL2.Add(x.score);
                        is_correctL2.Add(x.is_right);
                        correct_option = x.correct_option != "" ? x.correct_option : null;
                        CorrectOptL2.Add(correct_option);
                        dustbinL2.Add(x.dustbin);
                    }
                });

                GenerationL1();
                GenerationL2();
            }
            else
            {
                dashboardpanel.SetActive(false);
                showmsg.SetActive(true);    
            }
           

        }

    }

    void GenerationL1()
    {
        showmsg.SetActive(false);
        if(WasteObjectL1.Count == 0)
        {
            for(int a = 0; a < ObjectnameL1.Count; a++)
            {
                GameObject gb = Instantiate(Level1datarow, Level1Holder, false);
                gb.transform.GetChild(0).gameObject.GetComponent<Text>().text = ObjectnameL1[a];
                gb.transform.GetChild(4).gameObject.GetComponent<Text>().text = ObjectSCoreL1[a].ToString();
                ScoreCounterL1 += ObjectSCoreL1[a];
                WasteObjectL1.Add(gb);
                TotalscoreL1.text = ScoreCounterL1.ToString();

                if(ObjectSCoreL1[a] == 10)
                {
                    if(dustbinL1[a].ToLower() == "paper")
                    {
                        gb.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correct;
                    }
                    if (dustbinL1[a].ToLower() == "plastic")
                    {
                        gb.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correct;
                    }
                    if (dustbinL1[a].ToLower() == "organic")
                    {
                        gb.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correct;
                    }
                    if(dustbinL1[a].ToLower() == "null")
                    {
                        gb.transform.GetChild(1).gameObject.GetComponent<Text>().text = "----";
                        gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = "----";
                        gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = "----";
                    }
                }
                else
                {
                    if (dustbinL1[a].ToLower() == "paper")
                    {
                        gb.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Wrong;
                        gb.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectOptL1[a];
                        if (CorrectOptL1[a].ToLower() == "paper")
                        {
                            gb.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                        if (CorrectOptL1[a].ToLower() == "plastic")
                        {
                            gb.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                        if (CorrectOptL1[a].ToLower() == "organic")
                        {
                            gb.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                    }
                    if (dustbinL1[a].ToLower() == "plastic")
                    {
                        gb.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Wrong;
                        gb.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectOptL1[a];
                        if (CorrectOptL1[a].ToLower() == "paper")
                        {
                            gb.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;


                        }
                        if (CorrectOptL1[a].ToLower() == "plastic")
                        {
                            gb.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                        if (CorrectOptL1[a].ToLower() == "organic")
                        {
                            gb.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                    }
                    if (dustbinL1[a].ToLower() == "organic")
                    {
                        gb.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Wrong;
                        gb.transform.GetChild(3).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(3).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectOptL1[a];
                        if (CorrectOptL1[a].ToLower() == "paper")
                        {
                            gb.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                        if (CorrectOptL1[a].ToLower() == "plastic")
                        {
                            gb.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                        if (CorrectOptL1[a].ToLower() == "organic")
                        {
                            gb.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                    }


                    if (dustbinL1[a] == "null")
                    {
                        gb.transform.GetChild(1).gameObject.GetComponent<Text>().text = "----";
                        gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = "----";
                        gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = "----";
                    }
                }
               

            }
        }
    }
    void GenerationL2()
    {
        showmsg.SetActive(false);
        if (WasteObjectL2.Count == 0)
        {
            for (int a = 0; a < ObjectnameL2.Count; a++)
            {
                GameObject gb = Instantiate(Level2datarow, level2Holder, false);
                gb.transform.GetChild(0).gameObject.GetComponent<Text>().text = ObjectnameL2[a];
                gb.transform.GetChild(5).gameObject.GetComponent<Text>().text = ObjectSCoreL2[a].ToString();
                ScoreCounterL2 += ObjectSCoreL2[a];
                WasteObjectL2.Add(gb);
                totalScoreL2.text = ScoreCounterL2.ToString();

                if (ObjectSCoreL2[a] == 10)
                {
                    if (dustbinL2[a].ToLower() == "biohazard")
                    {
                        gb.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correct;
                    }
                    if (dustbinL2[a].ToLower() == "ewaste")
                    {
                        gb.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correct;
                    }
                    if (dustbinL2[a].ToLower() == "metal")
                    {
                        gb.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correct;
                    }
                    if (dustbinL2[a].ToLower() == "glass")
                    {
                        gb.transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(4).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correct;
                    }
                    if (dustbinL2[a].ToLower() == "null")
                    {
                        gb.transform.GetChild(1).gameObject.GetComponent<Text>().text = "----";
                        gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = "----";
                        gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = "----";
                        gb.transform.GetChild(4).gameObject.GetComponent<Text>().text = "----";
                    }
                }
                else
                {
                    if (dustbinL2[a].ToLower() == "biohazard")
                    {
                        gb.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Wrong;
                        gb.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectOptL2[a];
                        if (CorrectOptL2[a].ToLower() == "biohazard")
                        {
                            gb.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                        if (CorrectOptL2[a].ToLower() == "ewaste")
                        {
                            gb.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                        if (CorrectOptL2[a].ToLower() == "metal")
                        {
                            gb.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                        if (CorrectOptL2[a].ToLower() == "glass")
                        {
                            gb.transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(4).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                    }
                    if (dustbinL2[a].ToLower() == "ewaste")
                    {
                        gb.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Wrong;
                        gb.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectOptL2[a];
                        if (CorrectOptL2[a].ToLower() == "biohazard")
                        {
                            gb.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                        if (CorrectOptL2[a].ToLower() == "ewaste")
                        {
                            gb.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                        if (CorrectOptL2[a].ToLower() == "metal")
                        {
                            gb.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                        if (CorrectOptL2[a].ToLower() == "glass")
                        {
                            gb.transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(4).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                    }
                    if (dustbinL2[a].ToLower() == "metal")
                    {
                        gb.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Wrong;
                        gb.transform.GetChild(3).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(3).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectOptL2[a];
                        if (CorrectOptL2[a].ToLower() == "biohazard")
                        {
                            gb.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                        if (CorrectOptL2[a].ToLower() == "ewaste")
                        {
                            gb.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                        if (CorrectOptL2[a].ToLower() == "metal")
                        {
                            gb.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                        if (CorrectOptL2[a].ToLower() == "glass")
                        {
                            gb.transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(4).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                    }
                    if (dustbinL2[a].ToLower() == "glass")
                    {
                        gb.transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(4).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Wrong;
                        gb.transform.GetChild(4).transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(4).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectOptL2[a];
                        if (CorrectOptL2[a].ToLower() == "biohazard")
                        {
                            gb.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                        if (CorrectOptL2[a].ToLower() == "ewaste")
                        {
                            gb.transform.GetChild(2).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(2).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                        if (CorrectOptL2[a].ToLower() == "metal")
                        {
                            gb.transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(3).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                        if (CorrectOptL2[a].ToLower() == "glass")
                        {
                            gb.transform.GetChild(4).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(4).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctopt;
                        }
                    }

                    if (dustbinL2[a] == "null")
                    {
                        gb.transform.GetChild(1).gameObject.GetComponent<Text>().text = "----";
                        gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = "----";
                        gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = "----";
                        gb.transform.GetChild(4).gameObject.GetComponent<Text>().text = "----";
                    }
                }


            }
        }
    }

    public void ResetTask()
    {
        for(int a=0;a< WasteObjectL1.Count; a++)
        {
            Destroy(WasteObjectL1[a].gameObject);
            Destroy(WasteObjectL2[a].gameObject);
        }

        ObjectnameL1.Clear();
        ObjectnameL2.Clear();
        ObjectSCoreL1.Clear();
        ObjectSCoreL2.Clear();
        is_correctL1.Clear();
        is_correctL2.Clear();
        dustbinL1.Clear();
        dustbinL2.Clear();
        WasteObjectL1.Clear();
        WasteObjectL2.Clear();
    }
}
