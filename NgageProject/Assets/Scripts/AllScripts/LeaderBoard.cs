using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    [Header("Leader Board API SETUP")]
    public string MainUrl;
    public string LeaderBoardApi,GradeDataApi,SchoolDataApi;
    public Transform PrenetOject,GradeParent,SchoolParent;
    public GameObject Dataprefeb;
    public Sprite FirstRank, SecondRank, ThirdRank;
    private List<GameObject> UserlistCreated = new List<GameObject>();
    public Sprite Mydata,RowHighlighter;
    public Color MydataColor;

    [Header("Tabs wise data of leaderboard")]
    [Space(10)]
    public Color pressedColor;
    public Color RealsedColor;
    public List<GameObject> Tabs;
    public GameObject OverallScrollBar, GradeScrollBar, SchoolScrollBar;
    public List<Sprite> BoyFace,GirlFace;
    void Start()
    {
       
    }

     void OnEnable()
    {
        Tabsetup();
        StartCoroutine(TakeLeaderBoardData());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Tabsetup()
    {
        Tabs[0].GetComponent<Image>().color = pressedColor;
        Tabs[1].GetComponent<Image>().color = RealsedColor;
        Tabs[2].GetComponent<Image>().color = RealsedColor;
    }

    IEnumerator TakeLeaderBoardData()
    {
        OverallScrollBar.SetActive(true);
        GradeScrollBar.SetActive(false);
        SchoolScrollBar.SetActive(false);
        if (PrenetOject.childCount == 0)
        {
            string Hitting_Url = MainUrl + LeaderBoardApi + "?id_user=" + PlayerPrefs.GetInt("UID") + "&org_id=" + PlayerPrefs.GetInt("OID");

            WWW Leaderborad_url = new WWW(Hitting_Url);
            yield return Leaderborad_url;
            if (Leaderborad_url.text != null)
            {
                Debug.Log(Leaderborad_url.text);
                JsonData leader_res = JsonMapper.ToObject(Leaderborad_url.text);
                for (int a = 0; a < leader_res.Count; a++)
                {

                    GameObject gb = Instantiate(Dataprefeb, PrenetOject, false);
                    if (int.Parse(leader_res[a]["Rank"].ToString()) == 1)
                    {
                        gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = FirstRank;
                    }
                    else if (int.Parse(leader_res[a]["Rank"].ToString()) == 2)
                    {
                        gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = SecondRank;
                    }
                    else if (int.Parse(leader_res[a]["Rank"].ToString()) == 3)
                    {
                        gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = ThirdRank;
                    }
                    else if (int.Parse(leader_res[a]["Rank"].ToString()) == 0)
                    {
                        gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = leader_res[a]["Rank"].ToString();
                    }
                    else
                    {
                        gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = leader_res[a]["Rank"].ToString();
                    }
                    if (leader_res[a]["Name"] != null)
                    {
                        gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = leader_res[a]["Name"].ToString();
                    }
                    if (PlayerPrefs.GetInt("UID") == int.Parse(leader_res[a]["id_user"].ToString()))
                    {
                        gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color = MydataColor;
                        gb.transform.GetChild(2).gameObject.GetComponent<Text>().color = MydataColor;
                        gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = PlayerPrefs.GetString("username");
                        gb.transform.GetChild(3).gameObject.GetComponent<Text>().color = MydataColor;
                        gb.transform.GetChild(4).gameObject.GetComponent<Text>().color = MydataColor;
                        gb.transform.GetChild(5).gameObject.GetComponent<Text>().color = MydataColor;
                        gb.transform.GetChild(6).gameObject.GetComponent<Image>().enabled = false;
                        gb.transform.GetChild(6).gameObject.transform.GetChild(0).GetComponent<Text>().color = MydataColor;
                        gb.GetComponent<BarHeighter>().heighlitedimage = RowHighlighter;
                        gb.GetComponent<BarHeighter>().normalimage = RowHighlighter;
                        gb.GetComponent<Image>().sprite = RowHighlighter;

                    }
                    gb.SetActive(true);
                  

                    if (leader_res[a]["Level"] != null)
                    {
                        gb.transform.GetChild(4).gameObject.GetComponent<Text>().text = leader_res[a]["Level"].ToString();
                    }
                    else
                    {
                        gb.transform.GetChild(4).gameObject.GetComponent<Text>().text = "0";
                    }
                    if (leader_res[a]["Grade"] != null)
                    {
                        gb.transform.GetChild(5).gameObject.GetComponent<Text>().text = leader_res[a]["Grade"].ToString();
                    }
                    else
                    {
                        gb.transform.GetChild(5).gameObject.GetComponent<Text>().text = "0";
                    }
                    if (leader_res[a]["SchoolName"] != null)
                    {
                        gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = leader_res[a]["SchoolName"].ToString();
                    }
                    else
                    {
                        gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = "----";
                    }

                    if (leader_res[a]["Score"] != null)
                    {
                        gb.transform.GetChild(6).gameObject.transform.GetChild(0).GetComponent<Text>().text = leader_res[a]["Score"].ToString();
                    }
                    else
                    {
                        gb.transform.GetChild(6).gameObject.transform.GetChild(0).GetComponent<Text>().text = "0";
                    }

                    if(leader_res[a]["Gender"] != null)
                    {
                        if (leader_res[a]["Gender"].ToString().Equals("m",System.StringComparison.OrdinalIgnoreCase))
                        {
                            for (int c = 0; c < BoyFace.Count; c++)
                            {
                                if (int.Parse(leader_res[a]["avatar_type"].ToString()) == c)
                                {
                                    gb.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = BoyFace[c];
                                }

                            }
                        }
                        else
                        {
                            for (int c = 0; c < GirlFace.Count; c++)
                            {
                                if (int.Parse(leader_res[a]["avatar_type"].ToString()) == c)
                                {
                                    gb.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = GirlFace[c];
                                }

                            }
                        }
                    }
                
                    UserlistCreated.Add(gb);

                }
            }
        }


    }

    public void CloseLeaderBoard()
    {
        for(int a = 0; a < UserlistCreated.Count; a++)
        {
            DestroyImmediate(UserlistCreated[a].gameObject);
        }
        UserlistCreated.Clear();
        this.gameObject.SetActive(false);
    }

    public void TabsClickHandler(string currentTab)
    {
        for(int a = 0; a < Tabs.Count; a++)
        {
            if(Tabs[a].gameObject.name == currentTab)
            {
                Tabs[a].gameObject.GetComponent<Image>().color = pressedColor;
                TabDataOpener(currentTab);
            }
            else
            {
                Tabs[a].gameObject.GetComponent<Image>().color = RealsedColor;
            }
        }
    }

    void TabDataOpener(string currentTab)
    {
        switch (currentTab.ToLower())
        {
            case "overall":
                StartCoroutine(TakeLeaderBoardData());
                break;
            case "grade":
                StartCoroutine(GardewiseLeaderBoard());
                break;
            case "school":
                StartCoroutine(SchoolWiseLeaderBoard());
                break;
            default:
                Debug.LogError("out of range");
                break;
        }
    }

    IEnumerator GardewiseLeaderBoard()
    {
        OverallScrollBar.SetActive(false);
        GradeScrollBar.SetActive(true);
        SchoolScrollBar.SetActive(false);
        if(GradeParent.childCount == 0) 
        { 
        string Hitting_Url = MainUrl + GradeDataApi + "?id_user=" + PlayerPrefs.GetInt("UID") + "&org_id=" + PlayerPrefs.GetInt("OID") + 
                "&Usergrade=" +int.Parse(PlayerPrefs.GetString("User_grade"));

        WWW Leaderborad_url = new WWW(Hitting_Url);
        yield return Leaderborad_url;
        if (Leaderborad_url.text != null)
        {
            Debug.Log(Leaderborad_url.text);
            JsonData leader_res = JsonMapper.ToObject(Leaderborad_url.text);
            for (int a = 0; a < leader_res.Count; a++)
            {

                GameObject gb = Instantiate(Dataprefeb, GradeParent, false);
                if (int.Parse(leader_res[a]["Rank"].ToString()) == 1)
                {
                    gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    gb.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = FirstRank;
                }
                else if (int.Parse(leader_res[a]["Rank"].ToString()) == 2)
                {
                    gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    gb.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = SecondRank;
                }
                else if (int.Parse(leader_res[a]["Rank"].ToString()) == 3)
                {
                    gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    gb.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = ThirdRank;
                }
                else if (int.Parse(leader_res[a]["Rank"].ToString()) == 0)
                {
                    gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = leader_res[a]["Rank"].ToString();
                }
                else
                {
                    gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = leader_res[a]["Rank"].ToString();
                }
                if (PlayerPrefs.GetInt("UID") == int.Parse(leader_res[a]["id_user"].ToString()))
                {
                    gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color = MydataColor;
                    gb.transform.GetChild(2).gameObject.GetComponent<Text>().color = MydataColor;
                    gb.transform.GetChild(3).gameObject.GetComponent<Text>().color = MydataColor;
                    gb.transform.GetChild(4).gameObject.GetComponent<Text>().color = MydataColor;
                    gb.transform.GetChild(5).gameObject.GetComponent<Text>().color = MydataColor;
                    gb.transform.GetChild(6).gameObject.GetComponent<Image>().enabled = false;
                    gb.transform.GetChild(6).gameObject.transform.GetChild(0).GetComponent<Text>().color = MydataColor;
                    gb.GetComponent<BarHeighter>().heighlitedimage = RowHighlighter;
                    gb.GetComponent<BarHeighter>().normalimage = RowHighlighter;
                    gb.GetComponent<Image>().sprite = RowHighlighter;

                }
                gb.SetActive(true);
                if (leader_res[a]["Name"] != null)
                {
                    gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = leader_res[a]["Name"].ToString();
                }

                if (leader_res[a]["Level"] != null)
                {
                    gb.transform.GetChild(4).gameObject.GetComponent<Text>().text = leader_res[a]["Level"].ToString();
                }
                else
                {
                    gb.transform.GetChild(4).gameObject.GetComponent<Text>().text = "0";
                }
                if (leader_res[a]["Grade"] != null)
                {
                    gb.transform.GetChild(5).gameObject.GetComponent<Text>().text = leader_res[a]["Grade"].ToString();
                }
                else
                {
                    gb.transform.GetChild(5).gameObject.GetComponent<Text>().text = "0";
                }
                if (leader_res[a]["SchoolName"] != null)
                {
                    gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = leader_res[a]["SchoolName"].ToString();
                }
                else
                {
                    gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = "----";
                }

                if (leader_res[a]["Score"] != null)
                {
                    gb.transform.GetChild(6).gameObject.transform.GetChild(0).GetComponent<Text>().text = leader_res[a]["Score"].ToString();
                }
                else
                {
                    gb.transform.GetChild(6).gameObject.transform.GetChild(0).GetComponent<Text>().text = "0";
                }
                if (leader_res[a]["Gender"].ToString().Equals("m",System.StringComparison.OrdinalIgnoreCase))
                {
                    for (int c = 0; c < BoyFace.Count; c++)
                    {
                        if (int.Parse(leader_res[a]["avatar_type"].ToString()) == c)
                        {
                            gb.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = BoyFace[c];
                        }

                    }
                }
                else
                {
                    for (int c = 0; c < GirlFace.Count; c++)
                    {
                        if (int.Parse(leader_res[a]["avatar_type"].ToString()) == c)
                        {
                            gb.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = GirlFace[c];
                        }

                    }
                }
                UserlistCreated.Add(gb);
            }
         }
        }


    }

    IEnumerator SchoolWiseLeaderBoard()
    {
        OverallScrollBar.SetActive(false);
        GradeScrollBar.SetActive(false);
        SchoolScrollBar.SetActive(true);
        if (SchoolParent.childCount == 0)
        {
            string Hitting_Url = MainUrl + SchoolDataApi + "?id_user=" + PlayerPrefs.GetInt("UID") + "&org_id=" + PlayerPrefs.GetInt("OID") 
                + "&id_school=" + PlayerPrefs.GetInt("id_school");

            WWW Leaderborad_url = new WWW(Hitting_Url);
            yield return Leaderborad_url;
            if (Leaderborad_url.text != null)
            {
                Debug.Log(Leaderborad_url.text);
                JsonData leader_res = JsonMapper.ToObject(Leaderborad_url.text);
                for (int a = 0; a < leader_res.Count; a++)
                {

                    GameObject gb = Instantiate(Dataprefeb, SchoolParent, false);
                    if (int.Parse(leader_res[a]["Rank"].ToString()) == 1)
                    {
                        gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = FirstRank;
                    }
                    else if (int.Parse(leader_res[a]["Rank"].ToString()) == 2)
                    {
                        gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = SecondRank;
                    }
                    else if (int.Parse(leader_res[a]["Rank"].ToString()) == 3)
                    {
                        gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(false);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<Image>().sprite = ThirdRank;
                    }
                    else if (int.Parse(leader_res[a]["Rank"].ToString()) == 0)
                    {
                        gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = leader_res[a]["Rank"].ToString();
                    }
                    else
                    {
                        gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = leader_res[a]["Rank"].ToString();
                    }
                    if (PlayerPrefs.GetInt("UID") == int.Parse(leader_res[a]["id_user"].ToString()))
                    {
                        gb.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().color = MydataColor;
                        gb.transform.GetChild(2).gameObject.GetComponent<Text>().color = MydataColor;
                        gb.transform.GetChild(3).gameObject.GetComponent<Text>().color = MydataColor;
                        gb.transform.GetChild(4).gameObject.GetComponent<Text>().color = MydataColor;
                        gb.transform.GetChild(5).gameObject.GetComponent<Text>().color = MydataColor;
                        gb.transform.GetChild(6).gameObject.GetComponent<Image>().enabled = false;
                        gb.transform.GetChild(6).gameObject.transform.GetChild(0).GetComponent<Text>().color = MydataColor;
                        gb.GetComponent<BarHeighter>().heighlitedimage = RowHighlighter;
                        gb.GetComponent<BarHeighter>().normalimage = RowHighlighter;
                        gb.GetComponent<Image>().sprite = RowHighlighter;

                    }
                    gb.SetActive(true);
                    if (leader_res[a]["Name"] != null)
                    {
                        gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = leader_res[a]["Name"].ToString();
                    }

                    if (leader_res[a]["Level"] != null)
                    {
                        gb.transform.GetChild(4).gameObject.GetComponent<Text>().text = leader_res[a]["Level"].ToString();
                    }
                    else
                    {
                        gb.transform.GetChild(4).gameObject.GetComponent<Text>().text = "0";
                    }
                    if (leader_res[a]["Grade"] != null)
                    {
                        gb.transform.GetChild(5).gameObject.GetComponent<Text>().text = leader_res[a]["Grade"].ToString();
                    }
                    else
                    {
                        gb.transform.GetChild(5).gameObject.GetComponent<Text>().text = "0";
                    }
                    if (leader_res[a]["SchoolName"] != null)
                    {
                        gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = leader_res[a]["SchoolName"].ToString();
                    }
                    else
                    {
                        gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = "----";
                    }

                    if (leader_res[a]["Score"] != null)
                    {
                        gb.transform.GetChild(6).gameObject.transform.GetChild(0).GetComponent<Text>().text = leader_res[a]["Score"].ToString();
                    }
                    else
                    {
                        gb.transform.GetChild(6).gameObject.transform.GetChild(0).GetComponent<Text>().text = "0";
                    }
                    if (leader_res[a]["Gender"].ToString().Equals("m", System.StringComparison.OrdinalIgnoreCase))
                    {
                        for (int c = 0; c < BoyFace.Count; c++)
                        {
                            if (int.Parse(leader_res[a]["avatar_type"].ToString()) == c)
                            {
                                gb.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = BoyFace[c];
                            }

                        }
                    }
                    else
                    {
                        for (int c = 0; c < GirlFace.Count; c++)
                        {
                            if (int.Parse(leader_res[a]["avatar_type"].ToString()) == c)
                            {
                                gb.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = GirlFace[c];
                            }

                        }
                    }
                    UserlistCreated.Add(gb);
                }
            }
        }


    }


}
