using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;

public class DashBoardData : MonoBehaviour
{
    // Start is called before the first frame update
    public string Mainurl, dashbaord_API;
    public string Zone;
    public int zoneNo;
    public List<GameObject> Room1items,Room2items,Room3items;
  
  
    public List<string> room1_obj = new List<string>();

    public List<string> room2_obj = new List<string>();
 
    public List<string> room3_obj = new List<string>();

    [HideInInspector]
    public List<int> room1_score = new List<int>();
    [HideInInspector]
    public List<int> room2_score = new List<int>();
    [HideInInspector]
    public List<int> room3_score = new List<int>();
    [HideInInspector]
    public List<int> room1_correct = new List<int>();
    [HideInInspector]
    public List<int> room2_correct = new List<int>();
    [HideInInspector]
    public List<int> room3_correct = new List<int>();
    [HideInInspector]
    public List<string> dustin_room1 = new List<string>();
    [HideInInspector]
    public List<string> dustin_room2 = new List<string>();
    [HideInInspector]
    public List<string> dustin_room3 = new List<string>();
    [HideInInspector]
    public List<string> correct_option_1 = new List<string>();
    [HideInInspector]
    public List<string> correct_option_2 = new List<string>();
    [HideInInspector]
    public List<string> correct_option_3 = new List<string>();
    public Sprite right, wrong;
    [HideInInspector]
    public int totalscore_room1, totalscore_room2, totalscore_room3;
    
    public Text room1_total, room2_total, room3_total;
    [Header("Prefebs and tabs ")]
    public GameObject tab_prefeb;
    public GameObject Dashboard_row;
    public List<GameObject> tabs;
    public List<GameObject> tab_parent;
    public Text Username, Totalscore, zonescore,grade;
    public Image zone_score_filler, total_score_filler;
    public float total_gamescore;
    public Image userprofile;
    public Sprite boy, girl;
    private List<GameObject> room1_obejcts = new List<GameObject>();
    private List<GameObject> room2_obejcts = new List<GameObject>();
    private List<GameObject> room3_obejcts = new List<GameObject>();
    public GameObject NotPlayedMsg;
    public Button dashboard_btn;
    public GameObject extrapanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDisable()
    {
     
    
    }

    private void OnEnable()
    {
        //StartCoroutine(get_dashboard());
    }
    public void startDashboard()
    {
        totalscore_room1 = 0;
        totalscore_room3 = 0;
        totalscore_room2 = 0;

        for (int j = 0; j < 3; j++)
        {
            GameObject gb = Instantiate(tab_prefeb, tab_parent[j].transform, false);
            gb.name = "Tab" + j;
            tabs.Add(gb);
            gb.SetActive(true);
        }
        StartCoroutine(get_dashboard());

    }
    public void reset_task()
    {
        StartCoroutine(resetall());
    }

    IEnumerator resetall()
    {
        yield return new WaitForSeconds(0);
      
        foreach(GameObject e in tabs)
        {
            Destroy(e);
            yield return new WaitForSeconds(0.1f);
        }
        dashboard_btn.interactable = true;
        room1_obj.Clear();
        room2_obj.Clear();
        room3_obj.Clear();
        room1_correct.Clear();
        room2_correct.Clear();
        room3_correct.Clear();
        tabs.Clear();
    }
  
 



    IEnumerator get_dashboard()
    {
        int character = PlayerPrefs.GetInt("characterType");
        if(character == 1)
        {
            userprofile.sprite = boy;
        }
        else
        {
            userprofile.sprite = girl;
        }
        Username.text = PlayerPrefs.GetString("username");
        grade.text = PlayerPrefs.GetString("User_grade");
        string correct_option;
        yield return new WaitForSeconds(0.1f);
        string Response_url = Mainurl + dashbaord_API + "?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID") +
            "&id_org_game=" + 1;//PlayerPrefs.GetInt("game_id");//

        WWW dashboard_res = new WWW(Response_url);
        yield return dashboard_res;
       
        if(dashboard_res.text!= null)
        {
            Debug.Log(dashboard_res.text);
            JsonData response = JsonMapper.ToObject(dashboard_res.text);
            string zonename = response[0]["ContentList"][zoneNo]["title"].ToString();
            Debug.Log("zone name " + zonename);
            int loopcount = int.Parse(response[0]["ContentList"][zoneNo]["UserLog"].Count.ToString());
            if(loopcount > 0)
            {
                Totalscore.text = response[0]["ContentList"][zoneNo]["totalscore"].ToString();
                zonescore.text = response[0]["ContentList"][zoneNo]["totalscore"].ToString();
                float scorevalue = float.Parse(response[0]["ContentList"][zoneNo]["totalscore"].ToString());
                total_score_filler.fillAmount = (float)scorevalue / total_gamescore;
                zone_score_filler.fillAmount = (float)scorevalue / total_gamescore;
                for (int a = 0; a < Room1items.Count; a++)
                {
                    for (int i = 0; i < loopcount; i++)
                    {
                        string obj_name = response[0]["ContentList"][zoneNo]["UserLog"][i]["item_collected"].ToString();
                        int score = int.Parse(response[0]["ContentList"][zoneNo]["UserLog"][i]["score"].ToString());
                        int is_correct = int.Parse(response[0]["ContentList"][zoneNo]["UserLog"][i]["is_right"].ToString());
                        string dustbin_name = response[0]["ContentList"][zoneNo]["UserLog"][i]["dustbin"].ToString();
                        if (response[0]["ContentList"][zoneNo]["UserLog"][i]["correct_option"] != null)
                        {
                            correct_option = response[0]["ContentList"][zoneNo]["UserLog"][i]["correct_option"].ToString();
                        }
                        else
                        {
                            correct_option = "";
                        }

                        if (obj_name == Room1items[a].gameObject.name)
                        {
                            room1_obj.Add(obj_name);
                            room1_score.Add(score);
                            room1_correct.Add(is_correct);
                            correct_option_1.Add(correct_option);
                            dustin_room1.Add(dustbin_name);
                        }
                        if (obj_name == Room2items[a].gameObject.name)
                        {
                            room2_obj.Add(obj_name);
                            room2_score.Add(score);
                            room2_correct.Add(is_correct);
                            correct_option_2.Add(correct_option);
                            dustin_room2.Add(dustbin_name);

                        }
                        if (obj_name == Room3items[a].gameObject.name)
                        {
                            room3_obj.Add(obj_name);
                            room3_score.Add(score);
                            room3_correct.Add(is_correct);
                            correct_option_3.Add(correct_option);
                            dustin_room3.Add(dustbin_name);
                        }
                    }

                }
                Room1_sorting();
                Room2_sorting();
                Room3_sorting();
                extrapanel.SetActive(false);
            }
          
             else
            {
                extrapanel.SetActive(false);
                dashboard_btn.interactable = true;
                Debug.Log("null values");
                NotPlayedMsg.SetActive(true);
                this.gameObject.SetActive(false);
            }


        }
      


    }

    void Room1_sorting()
    {
        
        for (int a = 0; a < room1_obj.Count; a++)
        {
            GameObject  gb = Instantiate(Dashboard_row, tabs[0].gameObject.transform, false);
            gb.transform.GetChild(0).gameObject.GetComponent<Text>().text = room1_obj[a];
            room1_obejcts.Add(gb);
            gb.transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<Text>().text = room1_score[a].ToString();
            if (correct_option_1[a].ToLower() == "reduce")
            {
                gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "CORRECT OPTION";
            }
            if (correct_option_1[a].ToLower() == "reuse")
            {
                gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "CORRECT OPTION";
            }
            if (correct_option_1[a].ToLower() == "recycle")
            {
                gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "CORRECT OPTION";
            }
            totalscore_room1 += room1_score[a];
            room1_total.text = totalscore_room1.ToString();
            if(dustin_room1[a].ToLower() == "reduce")
            {
                if(room1_correct[a] == 1)
                {
                    gb.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    gb.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = right;
                }
                else
                {
                    gb.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    gb.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = wrong;
                }
            }
            if (dustin_room1[a].ToLower() == "reuse")
            {
                if (room1_correct[a] == 1)
                {
                    gb.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    gb.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = right;
                }
               
                else
                {
                    gb.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    gb.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = wrong;
                }
            }
            if (dustin_room1[a].ToLower() == "recycle")
            {
                if (room1_correct[a] == 1)
                {
                    gb.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    gb.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = right;
                }
                else
                {
                    gb.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    gb.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = wrong;
                }
            }
            if(dustin_room1[a].ToLower() == "null")
            {
                if (room1_correct[a] == 2)
                {
                    gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "---";
                    gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "---";
                    gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "---";
                }
            }



        }
        
    }

    void Room2_sorting()
    {
        Debug.Log("started room2");
        for (int a = 0; a < room2_obj.Count; a++)
        {
            GameObject gb = Instantiate(Dashboard_row, tabs[1].gameObject.transform, false);
            room2_obejcts.Add(gb);
            gb.transform.GetChild(0).gameObject.GetComponent<Text>().text = room2_obj[a];
            gb.transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<Text>().text = room2_score[a].ToString();
            if (correct_option_2[a].ToLower() == "reduce")
            {
                gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "CORRECT OPTION";
            }
            if (correct_option_2[a].ToLower() == "reuse")
            {
                gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "CORRECT OPTION";
            }
            if (correct_option_2[a].ToLower() == "recycle")
            {
                gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "CORRECT OPTION";
            }
            totalscore_room2 += room2_score[a];
            room2_total.text = totalscore_room2.ToString();
            if (dustin_room2[a].ToLower() == "reduce")
            {
                if (room2_correct[a] == 1)
                {
                    gb.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    gb.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = right;
                }
                else
                {
                    gb.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    gb.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = wrong;
                }
            }
            if (dustin_room2[a].ToLower() == "reuse")
            {
                if (room2_correct[a] == 1)
                {
                    gb.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    gb.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = right;
                }
                else
                {
                    gb.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    gb.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = wrong;
                }
            }
            if (dustin_room2[a].ToLower() == "recycle")
            {
                if (room2_correct[a] == 1)
                {
                    gb.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    gb.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = right;
                }
                else
                {
                    gb.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    gb.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = wrong;
                }
                }
                if (dustin_room2[a].ToLower() == "null")
                {
                    if (room2_correct[a] == 2)
                    {
                        gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "---";
                    }
                }

            }
    }
    


        void Room3_sorting()
        {
            Debug.Log("started room3");
            for (int a = 0; a < room3_obj.Count; a++)
            {
                GameObject gb = Instantiate(Dashboard_row, tabs[2].gameObject.transform, false);
                gb.transform.GetChild(0).gameObject.GetComponent<Text>().text = room3_obj[a];
                room3_obejcts.Add(gb);
                gb.transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<Text>().text = room3_score[a].ToString();
                if (correct_option_3[a].ToLower() == "reduce")
                {
                    gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "CORRECT OPTION";
                }
                if (correct_option_3[a].ToLower() == "reuse")
                {
                    gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "CORRECT OPTION";
                }
                if (correct_option_3[a].ToLower() == "recycle")
                {
                    gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "CORRECT OPTION";
                }
                totalscore_room3 += room3_score[a];
                room3_total.text = totalscore_room3.ToString();
                if (dustin_room3[a].ToLower() == "reduce")
                {
                    if (room3_correct[a] == 1)
                    {
                        gb.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        gb.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = right;
                    }
                   
                    else
                    {
                        gb.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        gb.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = wrong;
                    }
                }
                if (dustin_room3[a].ToLower() == "reuse")
                {
                    if (room3_correct[a] == 1)
                    {
                        gb.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        gb.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = right;
                    }
                    else
                    {
                        gb.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        gb.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = wrong;
                    }
                }
                if (dustin_room3[a].ToLower() == "recycle")
                {
                    if (room3_correct[a] == 1)
                    {
                        gb.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        gb.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = right;
                    }
                    else
                    {
                        gb.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.SetActive(true);
                        gb.transform.GetChild(3).gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = wrong;
                    }
                }
                if (dustin_room3[a].ToLower() == "null")
                {
                    if (room3_correct[a] == 2)
                    {
                        gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "---";
                        gb.transform.GetChild(3).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "---";
                    }
                }

            }
        }
    
}
