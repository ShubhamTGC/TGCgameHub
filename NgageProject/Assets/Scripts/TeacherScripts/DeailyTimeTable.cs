using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using SimpleSQL;
using UnityEngine.Networking;

public class DeailyTimeTable : MonoBehaviour
{
    [HideInInspector] public List<string> Months = new List<string> { "January", "February", "March", "April","May","June","July","August","September","October","November","December" };
    public Text Date, Month;
    public List<InputField> events;
    public SimpleSQLManager dbmanager;
    public PopupView NativePopup;
    public DateTime todate;
    public DateTime CurrentdateTime;
    public DateTime TempDateTime;
    private string  eventdata;
    private int datecounter;

    [Header("DATA POSTING API")]
    [Space(15)]
    public string MainUrl;
    public string EventPostingApi,UpdateEventApi;
    public Button Savebtn;
    public InputField Event1, Event2, Event3;
    public GameObject msgPanel;
    public Text Info;
    public TeacherHomepage teacherpage;
    void Start()
    {
        
    }
     void OnEnable()
    {
        datecounter = 0;

        CurrentDaySetup();
    }

    void CurrentDaySetup()
    {

        todate = DateTime.Today;
        Date.text = todate.Day.ToString();
        Month.text = Months[todate.Month-1];
        TempDateTime = todate;
        GetTeacherEvent(todate);
      
    }

    void GetTeacherEvent(DateTime date)
    {
        var tableLog = dbmanager.Table<TeacherEvent>().FirstOrDefault(x => x.Date == date.Day && x.Month == date.Month && x.Year == date.Year);
        if (tableLog != null)
        {
            string[] Eventmsg = tableLog.Event.Split("@"[0]);
            for (int a = 0; a < Eventmsg.Length; a++)
            {
                if (Eventmsg[a] != "")
                {
                    events[a].text = Eventmsg[a] == "null"? "No events..." : Eventmsg[a];
                }
            }
        }
        else
        {
            events.ForEach(x =>
            {
                x.text = "No events...";
            });
        }
    }
    public void Updatedate()
    {
        Date.text = todate.Day.ToString();
        Month.text = Months[todate.Month - 1];
    }


    void Update()
    {
        
    }


     void insertevent()
    {
        string tempstring = "";
        for(int a = 0; a < events.Count; a++)
        {
            if(events[a].text == "No events..." || events[a].text == "")
            {
                tempstring = "null";
               
            }
            else
            {
                tempstring = events[a].text;
            }
             eventdata = tempstring + "@" +eventdata;
        }
    }

    public void Savedata()
    {
        insertevent();
        //var log = dbmanager.Table<TeacherEvent>().FirstOrDefault(x => x.Date == todate.Day && x.Month == todate.Month && x.Year == todate.Year);
        //if(log == null)
        //{
        //    TeacherEvent Log = new TeacherEvent
        //    {
        //        Date = CurrentdateTime.Day,
        //        Month = CurrentdateTime.Month,
        //        Year = CurrentdateTime.Year,
        //        Event = eventdata
        //    };
        //    dbmanager.Insert(Log);
        //}
        //else
        //{
        //    log.Date = CurrentdateTime.Day;
        //    log.Month = CurrentdateTime.Month;
        //    log.Year = CurrentdateTime.Year;
        //    log.Event = eventdata;
        //    dbmanager.UpdateTable(log);
        //}

        if ((Event1.text != "No events..."  || Event2.text != "No events..." || Event3.text != "No events...") && (Event1.text != "" 
            || Event2.text != "" || Event3.text != ""))
        {
            //Debug.Log("data inserted");
           StartCoroutine(PostEvent());
        }
        else
        {
            string msg = "Please make any event!!!";
            StartCoroutine(ShowMsginfo(msg));
        }

    }


    public void NextDateEvent()
    {
        datecounter++;
     
        DateTime Updateeddate = todate.AddDays(datecounter);
        Date.text = Updateeddate.Day.ToString();
        Month.text = Months[Updateeddate.Month - 1];
        TempDateTime = Updateeddate;
        GetTeacherEvent(Updateeddate);
    }

    public void PreviousDateEvent()
    {
        datecounter--;
        DateTime Updateeddate = todate.AddDays(datecounter);
        Date.text = Updateeddate.Day.ToString();
        Month.text = Months[Updateeddate.Month - 1];
        TempDateTime = Updateeddate;
        GetTeacherEvent(Updateeddate);
    }

    IEnumerator PostEvent()
    {
        string HittingUrl = "";
        string logdata = "";
        var eventLog = dbmanager.Table<TeacherEvent>().FirstOrDefault(x => x.Date == TempDateTime.Day && x.Month == TempDateTime.Month && x.Year == TempDateTime.Year)?.IdEvent.ToString();
        if(eventLog == null)
        {
            HittingUrl = $"{MainUrl}{EventPostingApi}";
            PostEventModel postlog = new PostEventModel
            {
                id_event = 0,
                id_user = PlayerPrefs.GetInt("UID"),
                title = "",
                description = eventdata,
                event_date = TempDateTime
            };
            logdata = Newtonsoft.Json.JsonConvert.SerializeObject(postlog);
        }
        else
        {
            HittingUrl = $"{MainUrl}{UpdateEventApi}";
            PostEventModel postlog = new PostEventModel
            {
                id_event =int.Parse(eventLog),
                id_user = PlayerPrefs.GetInt("UID"),
                title = "",
                description = eventdata,
                event_date = TempDateTime
            };
            logdata = Newtonsoft.Json.JsonConvert.SerializeObject(postlog);
        }
       
      
        Debug.Log(logdata);
        yield return new WaitForSeconds(0.1f);
        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, logdata))
        {
            Request.method = UnityWebRequest.kHttpVerbPOST;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                Debug.Log(Request.downloadHandler.text);
                string msg = "Your Event recorded successfully.";
                teacherpage.GetlievEvents();
                StartCoroutine(ShowMsginfo(msg));



            }
            else
            {
                Debug.Log("data not done");
                string msg = "Something went wrong!";
                StartCoroutine(ShowMsginfo(msg));
            }
        }


    }

    IEnumerator ShowMsginfo(string msg)
    {
        msgPanel.SetActive(true);
        Info.text = msg;
        iTween.ScaleTo(msgPanel, Vector3.one, 0.4f);
        yield return new WaitForSeconds(3f);
        iTween.ScaleTo(msgPanel, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        Info.text = "";
        msgPanel.SetActive(false);

    }
}
