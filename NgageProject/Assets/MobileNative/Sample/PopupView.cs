using UnityEngine;
using System.Collections;
using pingak9;
using UnityEngine.UI;
using System;
using System.Globalization;

public class PopupView : MonoBehaviour
{
    public Text txtLog;
    private DeailyTimeTable Mainpage;
     void Start()
    {
        Mainpage = FindObjectOfType<DeailyTimeTable>();
    }

    public void DebugLog(string log)
    {
        //txtLog.text = log;
       
        Debug.Log(log);
    }
    

    // Dialog Button click event
    public void OnDialogInfo()
    {
        NativeDialog.OpenDialog("Info popup", "Welcome To Native Popup", "Ok", 
            ()=> {
                DebugLog("OK Button pressed");
            });
    }

    public void OnDialogConfirm()
    {
        NativeDialog.OpenDialog("Confirm popup", "Do you wants about app?", "Yes", "No",
            () => {
                DebugLog("Yes Button pressed");
            },
            () => {
                DebugLog("No Button pressed");
            });
    }
    public void OnDialogNeutral()
    {
        NativeDialog.OpenDialog("Like this game?", "Please rate to support future updates!", "Rate app", "later", "No, thanks",
            () =>
            {
                DebugLog("Rate Button pressed");
            },
            () =>
            {
                DebugLog("Later Button pressed");
            },
            () =>
            {
                DebugLog("No Button pressed");
            });
    }

    public void OnDatePicker()
    {
        DateTime todate = DateTime.Today;
        int date = todate.Day; 
        int month = todate.Month; 
        int year = todate.Year; 

        NativeDialog.OpenDatePicker(year,month,date,
            (DateTime _date) =>
            {
                Mainpage.todate = _date;
                Mainpage.TempDateTime = _date;
                Mainpage.CurrentdateTime = _date;
                Mainpage.Updatedate();
                DebugLog(_date.ToString());
            },
            (DateTime _date) =>
            {
                Mainpage.todate = _date;
                Mainpage.TempDateTime = _date;
                Mainpage.CurrentdateTime = _date;
                Mainpage.Updatedate();
                DebugLog(_date.ToString());
            });        
    }
    public void OnTimePicker()
    {
        NativeDialog.OpenTimePicker(
            (DateTime _date) =>
            {
                DebugLog(_date.ToString());
            },
            (DateTime _date) =>
            {
                DebugLog(_date.ToString());
            });
    }
}