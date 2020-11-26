using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class StudentPerformancePage : MonoBehaviour
{
    public string MainUrl, PerformanceApi;
    private List<GameObject> rows = new List<GameObject> ();
    public GameObject RowPrefeb;
    public Transform RowHandler;
    public Dropdown grade;
    public GameObject statusMsgpanel;
    public Text Showmsg;
    private string gradevalue;
    void Start()
    {
        
    }


    private void OnEnable()
    {
        if(rows.Count == 0)
        {
            StartCoroutine(GetPerformanceLog(1));
        }
    }


   
    IEnumerator GetPerformanceLog(int gradeno)
    {
       
        if (gradeno != 0)
        {
            gradevalue = grade.options[gradeno].text;
            string HittingUrl = $"{MainUrl}{PerformanceApi}?id_user={PlayerPrefs.GetInt("UID")}&Grade={gradevalue}";
            WWW request = new WWW(HittingUrl);
            yield return request;
            if (request.text != null)
            {
                if (request.text != "[]")
                {
                    Debug.Log("log " + request.text);
                    List<StudentPerformanceModel> studentlog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StudentPerformanceModel>>(request.text);
                    studentlog.ForEach(x =>
                    {
                        GameObject gb = Instantiate(RowPrefeb, RowHandler, false);
                        rows.Add(gb);
                        gb.transform.GetChild(0).gameObject.GetComponent<Text>().text = x.Rank.ToString();
                        gb.transform.GetChild(1).gameObject.GetComponent<Text>().text = x.Name;
                        gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = x.Level.ToString();
                        gb.transform.GetChild(4).gameObject.GetComponent<Text>().text = x.Points.ToString();
                    });
                }
            }
        }
        else
        {
            string msg = "Please select Your class.";
            StartCoroutine(Messagedisplay(msg));
        }
       
       
    }

    public void SelectGrade()
    {
        int gradeno = grade.value;
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
        for(int a = 0; a < count; a++)
        {
            Destroy(RowHandler.transform.GetChild(a).gameObject);
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

}
