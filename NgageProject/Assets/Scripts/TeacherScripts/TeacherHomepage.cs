using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using SimpleSQL;
using System;

public class TeacherHomepage : MonoBehaviour
{
    public string MainUrl, LiveStudentApi,EventLogApi;
    public Text StudentLive, StudentCleared,teachername;
    public Dropdown Grade;
    public GameObject Performancepage, GameFeed, GreenJournalpage;
    public SimpleSQLManager dbmanager;

    public Sprite Clicked, notClicked;
    public List<GameObject> StageTabs;
    void Start()
    {
        
    }
    private void OnEnable()
    {
        for (int a = 0; a < StageTabs.Count; a++)
        {
            if (a == 0)
            {
                StageTabs[a].GetComponent<Image>().sprite = Clicked;
            }
            else
            {
                StageTabs[a].GetComponent<Image>().sprite = notClicked;
            }
        }
        StudentLive.text = "0";
        StudentCleared.text = "0";
        StartCoroutine(GetStudentStatus(0));
        StartCoroutine(GetEventDataLog());

    }

    public void SelectStages(GameObject Tab)
    {
        for (int a = 0; a < StageTabs.Count; a++)
        {
            if (StageTabs[a].name == Tab.name)
            {
                StageTabs[a].GetComponent<Image>().sprite = Clicked;
            }
            else
            {
                StageTabs[a].GetComponent<Image>().sprite = notClicked;
            }
        }
    }

    public void StudentLiveData(int a)
    {
        StartCoroutine(GetStudentStatus(a));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ShowMoreUserPerformance()
    {
        Performancepage.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void ShowGameFeed()
    {
        GameFeed.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void ShowGreenJournal()
    {
        GreenJournalpage.SetActive(true);
        this.gameObject.SetActive(false);
    }


    IEnumerator GetStudentStatus(int level)
    {
        string HittingUrl = $"{MainUrl}{LiveStudentApi}?UID={PlayerPrefs.GetInt("UID")}&id_level={level}";
        Debug.Log("level is " + level);
        WWW request = new WWW(HittingUrl);
        yield return request;
        if(request.text!= null)
        {
            if(request.text != "[]")
            {
                LiveStudentsModel studentlog = Newtonsoft.Json.JsonConvert.DeserializeObject<LiveStudentsModel>(request.text);
                Debug.Log("Teacher log " + request.text);
                StudentLive.text = studentlog.StudentsPlaying.ToString();
                StudentCleared.text = studentlog.StudentsCleared.ToString();
                teachername.text = studentlog.Name;
                Grade.options.Clear();
                Grade.value = 0;
                Grade.options.Add(new Dropdown.OptionData() { text = "Grade" });
                Grade.options.Add(new Dropdown.OptionData() { text = studentlog.Class });

            }
        }
    }

    public void GetlievEvents()
    {
        StartCoroutine(GetEventDataLog());
    }
    IEnumerator GetEventDataLog()
    {
        DateTime Eventdate;
        string HittingUrl = $"{MainUrl}{EventLogApi}?UID={PlayerPrefs.GetInt("UID")}";
        WWW request = new WWW(HittingUrl);
        yield return request;
        if(request.text != null)
        {
           
            if (request.text != "[]")
            {
                List<EventUpdatedModel> Log = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EventUpdatedModel>>(request.text);
                Log.ForEach(x =>
                {
                    Eventdate = x.event_date;
                    var eventLog = dbmanager.Table<TeacherEvent>().FirstOrDefault(y => y.Date == Eventdate.Day && y.Month == Eventdate.Month && y.Year == Eventdate.Year);
                    if(eventLog == null)
                    {
                        TeacherEvent templog = new TeacherEvent
                        {
                            Date = Eventdate.Day,
                            Month = Eventdate.Month,
                            Year = Eventdate.Year,
                            IdEvent = x.id_event,
                            Event = x.description
                        };
                        dbmanager.Insert(templog);
                    }
                    else
                    {
                        eventLog.Date = Eventdate.Day;
                        eventLog.Month = Eventdate.Month;
                        eventLog.Year = Eventdate.Year;
                        eventLog.IdEvent = x.id_event;
                        eventLog.Event = x.description;
                        dbmanager.UpdateTable(eventLog);
                    }
                });
            }
            else
            {
                Debug.Log("Null data");
            }
        }
    }
}
