using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Linq;
using UnityEngine.UI;

public class StageOneDashboard : MonoBehaviour
{
    // Start is called before the first frame update
    public string MainUrl, DashBoard_Api, GetGamesIDApi, RoomData_api;
    public int zoneNo,id_game_content;
    public Text Totalscore, zonescore, room1_total, room2_total, room3_total;
    public Image total_score_filler, zone_score_filler;
    public int total_gamescore;
    public List<GameObject> Room1items;
    public List<GameObject> Room2items;
    public List<GameObject> Room3items;
    private string correct_option;
    public Button LeftPage, Rightpage;
    public Text RoomHeading;
    private int RoomPageCounter;
    private int CurrentPageCounter;
    [SerializeField]
    private int TotalRooms;
    public List<string> RoomNames;
    public Stage1Menus stage1gamedata;

    //=================private lists========================================//
    //[SerializeField]
    private List<string> room1_obj = new List<string>();
    //[SerializeField]
    private List<string> room2_obj = new List<string>();
    //[SerializeField]
    private List<string> room3_obj = new List<string>();

    //[SerializeField]
    private List<int> room1_score = new List<int>();
    //[SerializeField]
    private List<int> room2_score = new List<int>();
    //[SerializeField]
    private List<int> room3_score = new List<int>();
    [HideInInspector]
    private List<int> room1_correct = new List<int>();
    [HideInInspector]
    private List<int> room2_correct = new List<int>();
    [HideInInspector]
    private List<int> room3_correct = new List<int>();
    [HideInInspector]
    private List<string> dustin_room1 = new List<string>();
    [HideInInspector]
    private List<string> dustin_room2 = new List<string>();
    [HideInInspector]
    private List<string> dustin_room3 = new List<string>();
    [HideInInspector]
    private List<string> correct_option_1 = new List<string>();
    [HideInInspector]
    private List<string> correct_option_2 = new List<string>();
    [HideInInspector]
    private List<string> correct_option_3 = new List<string>();
    public Sprite right, wrong,correctoption,partiallycorrect;
    [HideInInspector]
    private List<GameObject> room1_obejcts = new List<GameObject>();
    [HideInInspector]
    private List<GameObject> room2_obejcts = new List<GameObject>();
    [HideInInspector]
    private List<GameObject> room3_obejcts = new List<GameObject>();
    [HideInInspector]
    private List<int> RoomScores = new List<int>();
    public GameObject Dashboard_row;
    public List<GameObject> tabs;
    [HideInInspector]
    public int totalscore_room1, totalscore_room2, totalscore_room3;
    public GameObject showmsg, dashboardpanel;
    private string CorrectansRoom1, CorrectansRoom2, CorrectansRoom3;
    [SerializeField]
    private int Gamelevel = 1;

    //================================== GAME CONTENT AND ROOM ID===========================

    private List<int> RoomIds;
    public string ZoneName;



    void Start()
    {
       
    }
    private void OnEnable()
    {
        RoomIds = new List<int>();
        RoomPageCounter = 0;
        StartCoroutine(GetGamesIDactivity());
        LeftPage.onClick.RemoveAllListeners();
        Rightpage.onClick.RemoveAllListeners();
        LeftPage.onClick.AddListener(delegate { SwitchLeftPage(); });
        Rightpage.onClick.AddListener(delegate { SwitchRightPage(); });
    }
    private void OnDisable()
    {
        
    }

    public void resetTask()
    {
        RoomPageCounter = 0;
        CurrentPageCounter = 0;
        //Debug.Log("data clearing");
        for(int a = 0; a < room1_obejcts.Count; a++)
        {
            DestroyImmediate(room1_obejcts[a].gameObject);
        }
        for (int a = 0; a < room2_obejcts.Count; a++)
        {
            DestroyImmediate(room2_obejcts[a].gameObject);
        }
        for (int a = 0; a < room3_obejcts.Count; a++)
        {
            DestroyImmediate(room3_obejcts[a].gameObject);
        }

        room1_obejcts.Clear();
        room2_obejcts.Clear();
        room3_obejcts.Clear();
        room1_obj.Clear();
        room2_obj.Clear();
        room3_obj.Clear();
        totalscore_room1 = 0;
        totalscore_room2 = 0;
        totalscore_room3 = 0;
        room1_score.Clear();
        room2_score.Clear();
        room3_score.Clear();
        room1_correct.Clear();
        room2_correct.Clear();
        room3_correct.Clear();
        dustin_room1.Clear();
        dustin_room2.Clear();
        dustin_room3.Clear();
        correct_option_1.Clear();
        correct_option_2.Clear();
        correct_option_3.Clear();
        RoomScores.Clear();
        //StartCoroutine(ClosurTask());
    }

    IEnumerator ClosurTask()
    {
        
        yield return new WaitForSeconds(0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (RoomPageCounter == 0)
        {
            LeftPage.gameObject.SetActive(false);
        }else if(RoomPageCounter >0 && RoomPageCounter < tabs.Count-1)
        {
            Rightpage.gameObject.SetActive(true);
            LeftPage.gameObject.SetActive(true);
        }
        else if(RoomPageCounter < tabs.Count)
        {
            Rightpage.gameObject.SetActive(false);
        }
    }

    public void SwitchLeftPage()
    {

        CurrentPageCounter = RoomPageCounter;
        RoomPageCounter--;
        tabs[RoomPageCounter].SetActive(true);
        room1_total.text = RoomScores[RoomPageCounter].ToString();
        RoomHeading.text = RoomNames[RoomPageCounter];
        tabs[CurrentPageCounter].SetActive(false);
    }

    public void SwitchRightPage()
    {
        CurrentPageCounter = RoomPageCounter;
        RoomPageCounter++;
        tabs[RoomPageCounter].SetActive(true);
        room1_total.text = RoomScores[RoomPageCounter].ToString();
        RoomHeading.text = RoomNames[RoomPageCounter];
        tabs[CurrentPageCounter].SetActive(false);
    }



   

    IEnumerator GetGamesIDactivity()
    {
        string HittingUrl = MainUrl + GetGamesIDApi + "?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID") +
            "&id_org_game=" + 1 + "&id_level=" + Gamelevel;
        WWW GameResponse = new WWW(HittingUrl);
        yield return GameResponse;
        if (GameResponse.text != null)
        {
            //Debug.Log("game id data " + GameResponse.text);
            GetLevelIDs gameIDs = Newtonsoft.Json.JsonConvert.DeserializeObject<GetLevelIDs>(GameResponse.text);
            var ContentList = gameIDs.content.ToList();
            id_game_content = ContentList.FirstOrDefault(x => x.title == ZoneName).id_game_content;
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
           // Debug.Log("rooom id " + roominfo.text);
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
        int MaxAttemptnum = 0;
        //resetTask();
        string Response_url = MainUrl + DashBoard_Api + "?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID") +
          "&id_org_game=" + 1;

        WWW dashboard_res = new WWW(Response_url);
        yield return dashboard_res;

        if (dashboard_res.text != null)
        {
            Debug.Log(" Zone data " + dashboard_res.text);
            List<DashboardItem> DashboardHandler = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DashboardItem>>(dashboard_res.text);
            var Contentlist = DashboardHandler.Where(x => x.id_level == Gamelevel).Select(x => x.ContentList).FirstOrDefault();
            var listUserLog = Contentlist?.Where(x => x.id_game_content == id_game_content).SelectMany(x => x.UserLog).ToList();
            if(listUserLog.Count > 0)
            {
                var data = listUserLog.Max(x => x.attempt_no);
                MaxAttemptnum = data;
            }
            
            //Debug.Log("got the final data " + data);
            var GotZoneData = Contentlist?.Where(x => x.id_game_content == id_game_content).Select(x => x.UserLog).FirstOrDefault();
            if(GotZoneData.Count > 0)
            {
                showmsg.SetActive(false);
                dashboardpanel.SetActive(true);
                GotZoneData.ForEach(x =>
                {
                    if (x.id_room == RoomIds[0] && x.attempt_no == MaxAttemptnum)
                    {
                        room1_obj.Add(x.item_collected);
                        room1_score.Add(x.score);
                        room1_correct.Add(x.is_right);
                        correct_option = x.correct_option != "" ? x.correct_option : null;
                        correct_option_1.Add(correct_option);
                        dustin_room1.Add(x.dustbin);
                    }
                    if (x.id_room == RoomIds[1] && x.attempt_no == MaxAttemptnum)
                    {
                        room2_obj.Add(x.item_collected);
                        room2_score.Add(x.score);
                        room2_correct.Add(x.is_right);
                        correct_option = x.correct_option != "" ? x.correct_option : null;
                        correct_option_2.Add(correct_option);
                        dustin_room2.Add(x.dustbin);
                    }
                    if (x.id_room == RoomIds[2] && x.attempt_no == MaxAttemptnum)
                    {
                        room3_obj.Add(x.item_collected);
                        room3_score.Add(x.score);
                        room3_correct.Add(x.is_right);
                        correct_option = x.correct_option != "" ? x.correct_option : null;
                        correct_option_3.Add(correct_option);
                        dustin_room3.Add(x.dustbin);
                    }

                });
                Room1_sorting();
                Room2_sorting();
                Room3_sorting();
            }
            else
            {
                dashboardpanel.SetActive(false);
                showmsg.SetActive(true);
            }



            //JsonData response = JsonMapper.ToObject(dashboard_res.text);
            //string zonename = response[0]["ContentList"][zoneNo]["title"].ToString();
            //Debug.Log("zone name " + zonename);
            //int loopcount = int.Parse(response[0]["ContentList"][zoneNo]["UserLog"].Count.ToString());
            //if (loopcount > 0)
            //{
            //    //Totalscore.text = response[0]["ContentList"][zoneNo]["totalscore"].ToString();
            //    //zonescore.text = response[0]["ContentList"][zoneNo]["totalscore"].ToString();
            //    float scorevalue = float.Parse(response[0]["ContentList"][zoneNo]["totalscore"].ToString());
            //    // total_score_filler.fillAmount = (float)scorevalue / total_gamescore;
            //    // zone_score_filler.fillAmount = (float)scorevalue / total_gamescore;
            //    showmsg.SetActive(false);
            //    dashboardpanel.SetActive(true);

            //    for (int a = 0; a < Room1items.Count; a++)
            //    {
            //        for (int i = 0; i < loopcount; i++)
            //        {
            //            string obj_name = response[0]["ContentList"][zoneNo]["UserLog"][i]["item_collected"].ToString();
            //            int score = int.Parse(response[0]["ContentList"][zoneNo]["UserLog"][i]["score"].ToString());
            //            int is_correct = int.Parse(response[0]["ContentList"][zoneNo]["UserLog"][i]["is_right"].ToString());
            //            string dustbin_name = response[0]["ContentList"][zoneNo]["UserLog"][i]["dustbin"].ToString();
            //            if (response[0]["ContentList"][zoneNo]["UserLog"][i]["correct_option"] != null)
            //            {
            //                correct_option = response[0]["ContentList"][zoneNo]["UserLog"][i]["correct_option"].ToString();
            //            }
            //            else
            //            {
            //                correct_option = "";
            //            }

            //            if (obj_name == Room1items[a].gameObject.name)
            //            {
            //                room1_obj.Add(obj_name);
            //                room1_score.Add(score);
            //                room1_correct.Add(is_correct);
            //                correct_option_1.Add(correct_option);
            //                dustin_room1.Add(dustbin_name);
            //            }
            //            if (obj_name == Room2items[a].gameObject.name)
            //            {
            //                room2_obj.Add(obj_name);
            //                room2_score.Add(score);
            //                room2_correct.Add(is_correct);
            //                correct_option_2.Add(correct_option);
            //                dustin_room2.Add(dustbin_name);

            //            }
            //            if (obj_name == Room3items[a].gameObject.name)
            //            {
            //                room3_obj.Add(obj_name);
            //                room3_score.Add(score);
            //                room3_correct.Add(is_correct);
            //                correct_option_3.Add(correct_option);
            //                dustin_room3.Add(dustbin_name);
            //            }
            //        }

            //    }
            //    Room1_sorting();
            //    Room2_sorting();
            //    Room3_sorting();
            //    //extrapanel.SetActive(false);
            //}

            //else
            //{

            //    //extrapanel.SetActive(false);
            //    //dashboard_btn.interactable = true;
            //    //Debug.Log("null values");
            //    //NotPlayedMsg.SetActive(true);
            //    //this.gameObject.SetActive(false);
            //}


        }

    }

    void Room1_sorting()
    {
        showmsg.SetActive(false);
        if (room1_obejcts.Count == 0)
        {
            for (int a = 0; a < room1_obj.Count; a++)
            {
                GameObject gb = Instantiate(Dashboard_row, tabs[0].gameObject.transform, false);
                gb.transform.GetChild(0).gameObject.GetComponent<Text>().text = room1_obj[a];
                room1_obejcts.Add(gb);
                var Query_room1 = gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
                var Query_room2 = gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
                var Query_room3 = gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject;
                gb.transform.GetChild(7).GetComponent<Text>().text = room1_score[a].ToString();

                totalscore_room1 += room1_score[a];
                room1_total.text = totalscore_room1.ToString();
                if (dustin_room1[a].ToLower() == "reduce")
                {
                    if (room1_score[a] == 10)
                    {

                        gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = right;
                    }
                    else
                    {
                        if (room1_score[a] == 5)
                        {
                          
                            if (correct_option_1[a].ToLower() == "reduce")
                            {
                                Query_room1.SetActive(true);
                                CorrectansRoom1 = "Reduce";
                                Query_room1.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_1[a].ToLower() == "reuse")
                            {
                                Query_room2.SetActive(true);
                                CorrectansRoom1 = "Reuse";
                                Query_room2.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_1[a].ToLower() == "recycle")
                            {
                                Query_room3.SetActive(true);
                                CorrectansRoom1 = "Recycle";
                                Query_room3.GetComponent<Image>().sprite = correctoption;
                            }
                            gb.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = partiallycorrect;
                            gb.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectansRoom1;
                        }
                        else
                        {
                            if (correct_option_1[a].ToLower() == "reduce")
                            {
                                Query_room1.SetActive(true);
                                CorrectansRoom1 = "Reduce";
                                Query_room1.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_1[a].ToLower() == "reuse")
                            {
                                Query_room2.SetActive(true);
                                CorrectansRoom1 = "Reuse";
                                Query_room2.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_1[a].ToLower() == "recycle")
                            {
                                Query_room3.SetActive(true);
                                CorrectansRoom1 = "Recycle";
                                Query_room3.GetComponent<Image>().sprite = correctoption;
                            }
                            gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = wrong;
                            gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectansRoom1;
                        }

                    }
                }
                if (dustin_room1[a].ToLower() == "reuse")
                {
                    if (room1_score[a] == 10)
                    {
                        gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = right;
                    }

                    else
                    {
                        if (room1_score[a] == 5)
                        {
                         
                            if (correct_option_1[a].ToLower() == "reduce")
                            {
                                Query_room1.SetActive(true);
                                CorrectansRoom1 = "Reduce";
                                Query_room1.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_1[a].ToLower() == "reuse")
                            {
                                Query_room2.SetActive(true);
                                CorrectansRoom1 = "Reuse";
                                Query_room2.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_1[a].ToLower() == "recycle")
                            {
                                Query_room3.SetActive(true);
                                CorrectansRoom1 = "Recycle";
                                Query_room3.GetComponent<Image>().sprite = correctoption;
                            }
                            gb.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = partiallycorrect;
                            gb.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectansRoom1;
                        }
                        else
                        {
                            if (correct_option_1[a].ToLower() == "reduce")
                            {
                                Query_room1.SetActive(true);
                                CorrectansRoom1 = "Reduce";
                                Query_room1.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_1[a].ToLower() == "reuse")
                            {
                                Query_room2.SetActive(true);
                                CorrectansRoom1 = "Reuse";
                                Query_room2.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_1[a].ToLower() == "recycle")
                            {
                                Query_room3.SetActive(true);
                                CorrectansRoom1 = "Recycle";
                                Query_room3.GetComponent<Image>().sprite = correctoption;
                            }
                            gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = wrong;
                            gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectansRoom1;
                        }

                    }
                }
                if (dustin_room1[a].ToLower() == "recycle")
                {
                    if (room1_score[a] == 10)
                    {
                        gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = right;
                    }
                    else
                    {
                        if (room1_score[a] == 5)
                        {
                          
                            if (correct_option_1[a].ToLower() == "reduce")
                            {
                                Query_room1.SetActive(true);
                                CorrectansRoom1 = "Reduce";
                                Query_room1.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_1[a].ToLower() == "reuse")
                            {
                                Query_room2.SetActive(true);
                                CorrectansRoom1 = "Reuse";
                                Query_room2.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_1[a].ToLower() == "recycle")
                            {
                                Query_room3.SetActive(true);
                                CorrectansRoom1 = "Recycle";
                                Query_room3.GetComponent<Image>().sprite = correctoption;
                            }
                            gb.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = partiallycorrect;
                            gb.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectansRoom1;
                        }
                        else
                        {
                            if (correct_option_1[a].ToLower() == "reduce")
                            {
                                Query_room1.SetActive(true);
                                CorrectansRoom1 = "Reduce";
                                Query_room1.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_1[a].ToLower() == "reuse")
                            {
                                Query_room2.SetActive(true);
                                CorrectansRoom1 = "Reuse";
                                Query_room2.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_1[a].ToLower() == "recycle")
                            {
                                Query_room3.SetActive(true);
                                CorrectansRoom1 = "Recycle";
                                Query_room3.GetComponent<Image>().sprite = correctoption;
                            }
                            gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = wrong;
                            gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectansRoom1;
                        }

                    }
                }
                if (dustin_room1[a].ToLower() == "null")
                {
                    if (room1_correct[a] == 2)
                    {
                        gb.transform.GetChild(1).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(4).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(5).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(6).gameObject.GetComponent<Text>().text = "---";
                    }
                }

            }
            RoomScores.Add(totalscore_room1);
        }
        
    }

    void Room2_sorting()
    {
        if(room2_obejcts.Count == 0)
        {
            for (int a = 0; a < room2_obj.Count; a++)
            {
                GameObject gb = Instantiate(Dashboard_row, tabs[1].gameObject.transform, false);
                gb.transform.GetChild(0).gameObject.GetComponent<Text>().text = room2_obj[a];
                room2_obejcts.Add(gb);
                var Query_room1 = gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
                var Query_room2 = gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
                var Query_room3 = gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject;
                gb.transform.GetChild(7).GetComponent<Text>().text = room2_score[a].ToString();

                totalscore_room2 += room2_score[a];
                //room2_total.text = totalscore_room2.ToString();
                if (dustin_room2[a].ToLower() == "reduce")
                {
                    if (room2_score[a] == 10)
                    {

                        gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = right;
                    }
                    else
                    {
                        if (room2_score[a] == 5)
                        {
                          
                            if (correct_option_2[a].ToLower() == "reduce")
                            {
                                Query_room1.SetActive(true);
                                CorrectansRoom2 = "Reduce";
                                Query_room1.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_2[a].ToLower() == "reuse")
                            {
                                Query_room2.SetActive(true);
                                CorrectansRoom2 = "Reuse";
                                Query_room2.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_2[a].ToLower() == "recycle")
                            {
                                Query_room3.SetActive(true);
                                CorrectansRoom2 = "Recycle";
                                Query_room3.GetComponent<Image>().sprite = correctoption;
                            }
                            gb.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectansRoom2;
                            gb.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = partiallycorrect;
                        }
                        else
                        {
                            if (correct_option_2[a].ToLower() == "reduce")
                            {
                                Query_room1.SetActive(true);
                                CorrectansRoom2 = "Reduce";
                                Query_room1.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_2[a].ToLower() == "reuse")
                            {
                                Query_room2.SetActive(true);
                                CorrectansRoom2 = "Reuse";
                                Query_room2.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_2[a].ToLower() == "recycle")
                            {
                                Query_room3.SetActive(true);
                                CorrectansRoom2 = "Recycle";
                                Query_room3.GetComponent<Image>().sprite = correctoption;
                            }
                            gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = wrong;
                            gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectansRoom2;
                        }

                    }
                }
                if (dustin_room2[a].ToLower() == "reuse")
                {
                    if (room2_score[a] == 10)
                    {
                        gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = right;
                    }

                    else
                    {
                        if (room2_score[a] == 5)
                        {
                           
                            if (correct_option_2[a].ToLower() == "reduce")
                            {
                                Query_room1.SetActive(true);
                                CorrectansRoom2 = "Reduce";
                                Query_room1.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_2[a].ToLower() == "reuse")
                            {
                                Query_room2.SetActive(true);
                                CorrectansRoom2 = "Reuse";
                                Query_room2.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_2[a].ToLower() == "recycle")
                            {
                                Query_room3.SetActive(true);
                                CorrectansRoom2 = "Recycle";
                                Query_room3.GetComponent<Image>().sprite = correctoption;
                            }
                            gb.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectansRoom2;
                            gb.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = partiallycorrect;
                        }
                        else
                        {
                            if (correct_option_2[a].ToLower() == "reduce")
                            {
                                Query_room1.SetActive(true);
                                CorrectansRoom2 = "Reduce";
                                Query_room1.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_2[a].ToLower() == "reuse")
                            {
                                Query_room2.SetActive(true);
                                CorrectansRoom2 = "Reuse";
                                Query_room2.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_2[a].ToLower() == "recycle")
                            {
                                Query_room3.SetActive(true);
                                CorrectansRoom2 = "Recycle";
                                Query_room3.GetComponent<Image>().sprite = correctoption;
                            }
                            gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = wrong;
                            gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectansRoom2;
                        }

                    }
                }
                if (dustin_room2[a].ToLower() == "recycle")
                {
                    if (room2_score[a] == 10)
                    {
                        gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = right;
                    }
                    else
                    {
                        if (room2_score[a] == 5)
                        {
                        
                            if (correct_option_2[a].ToLower() == "reduce")
                            {
                                Query_room1.SetActive(true);
                                CorrectansRoom2 = "Reduce";
                                Query_room1.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_2[a].ToLower() == "reuse")
                            {
                                Query_room2.SetActive(true);
                                CorrectansRoom2 = "Reuse";
                                Query_room2.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_2[a].ToLower() == "recycle")
                            {
                                Query_room3.SetActive(true);
                                CorrectansRoom2 = "Recycle";
                                Query_room3.GetComponent<Image>().sprite = correctoption;
                            }
                            gb.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectansRoom2;
                            gb.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = partiallycorrect;
                        }
                        else
                        {
                            if (correct_option_2[a].ToLower() == "reduce")
                            {
                                Query_room1.SetActive(true);
                                CorrectansRoom2 = "Reduce";
                                Query_room1.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_2[a].ToLower() == "reuse")
                            {
                                Query_room2.SetActive(true);
                                CorrectansRoom2 = "Reuse";
                                Query_room2.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_2[a].ToLower() == "recycle")
                            {
                                Query_room3.SetActive(true);
                                CorrectansRoom2 = "Recycle";
                                Query_room3.GetComponent<Image>().sprite = correctoption;
                            }
                            gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = wrong;
                            gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectansRoom2;
                        }

                    }
                }
                if (dustin_room2[a].ToLower() == "null")
                {
                    if (room2_correct[a] == 2)
                    {
                        gb.transform.GetChild(1).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(4).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(5).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(6).gameObject.GetComponent<Text>().text = "---";
                    }
                }


            }
            RoomScores.Add(totalscore_room2);
        }
        
    }

    void Room3_sorting()
    {
        if (room3_obejcts.Count == 0)
        {


            for (int a = 0; a < room3_obj.Count; a++)
            {
                GameObject gb = Instantiate(Dashboard_row, tabs[2].gameObject.transform, false);
                gb.transform.GetChild(0).gameObject.GetComponent<Text>().text = room3_obj[a];
                room3_obejcts.Add(gb);
                var Query_room1 = gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
                var Query_room2 = gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
                var Query_room3 = gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject;
                gb.transform.GetChild(7).GetComponent<Text>().text = room3_score[a].ToString();

                totalscore_room3 += room3_score[a];
                //room2_total.text = totalscore_room3.ToString();
                if (dustin_room3[a].ToLower() == "reduce")
                {
                    if (room3_score[a] == 10)
                    {

                        gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = right;
                    }
                    else
                    {
                        if (room3_score[a] == 5)
                        {
                      
                            if (correct_option_3[a].ToLower() == "reduce")
                            {
                                Query_room1.SetActive(true);
                                CorrectansRoom3 = "Reduce";
                                Query_room1.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_3[a].ToLower() == "reuse")
                            {
                                Query_room2.SetActive(true);
                                CorrectansRoom3 = "Reuse";
                                Query_room2.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_3[a].ToLower() == "recycle")
                            {
                                Query_room3.SetActive(true);
                                CorrectansRoom3 = "Recycle";
                                Query_room3.GetComponent<Image>().sprite = correctoption;
                            }
                            gb.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectansRoom3;
                            gb.transform.GetChild(4).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = partiallycorrect;
                        }
                        else
                        {
                            if (correct_option_3[a].ToLower() == "reduce")
                            {
                                Query_room1.SetActive(true);
                                CorrectansRoom3 = "Reduce";
                                Query_room1.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_3[a].ToLower() == "reuse")
                            {
                                Query_room2.SetActive(true);
                                CorrectansRoom3 = "Reuse";
                                Query_room2.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_3[a].ToLower() == "recycle")
                            {
                                Query_room3.SetActive(true);
                                CorrectansRoom3 = "Recycle";
                                Query_room3.GetComponent<Image>().sprite = correctoption;
                            }
                            gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = wrong;
                            gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectansRoom3;
                        }

                    }
                }
                if (dustin_room3[a].ToLower() == "reuse")
                {
                    if (room3_score[a] == 10)
                    {
                        gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = right;
                    }

                    else
                    {
                        if (room3_score[a] == 5)
                        {
                  
                            if (correct_option_3[a].ToLower() == "reduce")
                            {
                                Query_room1.SetActive(true);
                                CorrectansRoom3 = "Reduce";
                                Query_room1.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_3[a].ToLower() == "reuse")
                            {
                                Query_room2.SetActive(true);
                                CorrectansRoom3 = "Reuse";
                                Query_room2.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_3[a].ToLower() == "recycle")
                            {
                                Query_room3.SetActive(true);
                                CorrectansRoom3 = "Recycle";
                                Query_room3.GetComponent<Image>().sprite = correctoption;
                            }
                            gb.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectansRoom3;
                            gb.transform.GetChild(5).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = partiallycorrect;
                        }
                        else
                        {
                            if (correct_option_3[a].ToLower() == "reduce")
                            {
                                Query_room1.SetActive(true);
                                CorrectansRoom3 = "Reduce";
                                Query_room1.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_3[a].ToLower() == "reuse")
                            {
                                Query_room2.SetActive(true);
                                CorrectansRoom3 = "Reuse";
                                Query_room2.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_3[a].ToLower() == "recycle")
                            {
                                Query_room3.SetActive(true);
                                CorrectansRoom3 = "Recycle";
                                Query_room3.GetComponent<Image>().sprite = correctoption;
                            }
                            gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = wrong;
                            gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectansRoom3;
                        }

                    }
                }
                if (dustin_room3[a].ToLower() == "recycle")
                {
                    if (room3_score[a] == 10)
                    {
                        gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = right;
                    }
                    else
                    {
                        if (room3_score[a] == 5)
                        {
                         
                            if (correct_option_3[a].ToLower() == "reduce")
                            {
                                Query_room1.SetActive(true);
                                CorrectansRoom3 = "Reduce";
                                Query_room1.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_3[a].ToLower() == "reuse")
                            {
                                Query_room2.SetActive(true);
                                CorrectansRoom3 = "Reuse";
                                Query_room2.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_3[a].ToLower() == "recycle")
                            {
                                Query_room3.SetActive(true);
                                CorrectansRoom3 = "Recycle";
                                Query_room3.GetComponent<Image>().sprite = correctoption;
                            }
                            gb.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectansRoom3;
                            gb.transform.GetChild(6).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = partiallycorrect;
                        }
                        else
                        {
                            if (correct_option_3[a].ToLower() == "reduce")
                            {
                                Query_room1.SetActive(true);
                                CorrectansRoom3 = "Reduce";
                                Query_room1.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_3[a].ToLower() == "reuse")
                            {
                                Query_room2.SetActive(true);
                                CorrectansRoom3 = "Reuse";
                                Query_room2.GetComponent<Image>().sprite = correctoption;
                            }
                            if (correct_option_3[a].ToLower() == "recycle")
                            {
                                Query_room3.SetActive(true);
                                CorrectansRoom3 = "Recycle";
                                Query_room3.GetComponent<Image>().sprite = correctoption;
                            }
                            gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = wrong;
                            gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = CorrectansRoom3;
                        }

                    }
                }
                if (dustin_room3[a].ToLower() == "null")
                {
                    if (room3_correct[a] == 2)
                    {
                        gb.transform.GetChild(1).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(4).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(5).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(6).gameObject.GetComponent<Text>().text = "---";
                    }
                }


            }
            RoomScores.Add(totalscore_room3);
            tabs[0].SetActive(true);
            tabs[1].SetActive(false);
            tabs[2].SetActive(false);
            room1_total.text = RoomScores[RoomPageCounter].ToString();
            RoomHeading.text = RoomNames[0];
        }


    }

  
}
