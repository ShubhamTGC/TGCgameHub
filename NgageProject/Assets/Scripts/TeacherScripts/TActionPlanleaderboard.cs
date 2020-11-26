using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TActionPlanleaderboard : MonoBehaviour
{
    public string MainUrl, ActionLeaderBoardApi;
    private List<GameObject> rows = new List<GameObject>();
    public GameObject RowPrefeb;
    public Transform RowHandler;
    public Sprite Rated, notRated;
    public int levelId;
    public Dropdown Grade;
    private string gradevalue;
    public GameObject statusMsgpanel;
    public Text Showmsg;
    void Start()
    {

    }


    private void OnEnable()
    {
        if (rows.Count == 0)
        {
            StartCoroutine(GetPerformanceLog(1));
        }

    }
    IEnumerator GetPerformanceLog(int value)
    {
        if(value != 0)
        {
            gradevalue = Grade.options[value].text;
            string HittingUrl = $"{MainUrl}{ActionLeaderBoardApi}?UID={PlayerPrefs.GetInt("UID")}&id_level={levelId}&Grade={gradevalue}";
            WWW request = new WWW(HittingUrl);
            yield return request;
            if (request.text != null)
            {
                if (request.text != "[]")
                {
                    Debug.Log("log " + request.text);
                    List<ActionPlanLeaderboardModel> studentlog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ActionPlanLeaderboardModel>>(request.text);
                    studentlog.ForEach(x =>
                    {
                        GameObject gb = Instantiate(RowPrefeb, RowHandler, false);
                        rows.Add(gb);
                       // gb.transform.GetChild(0).gameObject.GetComponent<Text>().text = x.RollNo != null ? x.RollNo : "0";
                        gb.transform.GetChild(1).gameObject.GetComponent<Text>().text = x.Name;

                        if (x.Is_Rated == 0)
                        {
                            gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Pending";
                        }
                        else
                        {
                            int childcount = gb.transform.GetChild(2).transform.childCount;
                            for (int a = 0; a < childcount; a++)
                            {
                                gb.transform.GetChild(2).transform.GetChild(a).gameObject.SetActive(true);
                                gb.transform.GetChild(2).transform.GetChild(a).gameObject.GetComponent<Image>().sprite = a <= x.Rate ? Rated : notRated;
                            }
                        }
                    });
                }
                else
                {
                    Debug.Log("log " + request.text);
                }
            }
        }
        else
        {
            string msg = "Please select Your class.";
            StartCoroutine(Messagedisplay(msg));
        }
  
    }

    public IEnumerator Messagedisplay(string msg)
    {
        statusMsgpanel.SetActive(true);
        Showmsg.text = msg;
        yield return new WaitForSeconds(3f);
        iTween.ScaleTo(statusMsgpanel, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.4f);
        Showmsg.text = "";
        statusMsgpanel.SetActive(false);
    }


    public void SelectGrade()
    {
        int gradeno = Grade.value;
        if (this.gameObject.activeInHierarchy)
        {
            BackToMainpage();
            StartCoroutine(GetPerformanceLog(gradeno));
        }


    }

    public void BackToMainpage()
    {
        rows.Clear();
        int count = RowHandler.transform.childCount;
        for (int a = 0; a < count; a++)
        {
            Destroy(RowHandler.transform.GetChild(a).gameObject);
        }
    }
}
