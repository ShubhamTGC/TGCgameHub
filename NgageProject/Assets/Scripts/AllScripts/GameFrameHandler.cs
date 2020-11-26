using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SimpleSQL;
using System.Linq;
public class GameFrameHandler : MonoBehaviour
{
    public GameObject buttonpanel,normalbuttonpanel, targetobj,normaltargetobj;
    private Vector3 outpos, incomepos, dashboard_inpos, dashboard_outpos,normalpanelin_pos,nomalpanelout_pos;
    private int clickcount = 1,clickcount2=1, playcount = 1,volume_count =1,dashboard_count =1;
    public Sprite in_sprite,out_sprite,play,pause,slient,volume;
    public GameObject panel_btn,panel_btn2,play_btn,volume_btn,Stage_voluBtn,pausepanel,dashboard_btn,SettingPage;
    //public GameObject leftdashboard, dashboard_target;

    [Header("======For count down time====")]
    public float mint;
    public Text timertext;
    public float sec;
    private float totalsecond,TotalSec;
    public Image timerimage;
    public Sprite boyface, girlface;
    public GameObject Status_msg_panel;

    [Header("dashboard pages")]
    public float sliderOffTime;
    private bool is_closed = false;
    public GameObject dashboard_panel;
    private int dashboard_show = 1;
    public GameObject gallery_panel;
    private int gallery_count = 1;
    public GameObject exitpanel,logoutpage,logoutpanel,logoutmsg,loadinganim;
    public GameObject updatemsg;
    public GameObject Leaderboard;
    public GameObject OverallPage;
    public AudioSource mainaudio;
    public Image brightness;
    static StartpageController instance;
    public SimpleSQLManager dbmanager;
    public GameObject FeedBackPage;
    public void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        instance = StartpageController.Home_instane;
       
        StartCoroutine(GetSounddata());
    }
    void Start()
    {
        incomepos = targetobj.GetComponent<RectTransform>().localPosition;
        outpos = buttonpanel.GetComponent<RectTransform>().localPosition;
        normalpanelin_pos = normaltargetobj.GetComponent<RectTransform>().localPosition;
        nomalpanelout_pos = normalbuttonpanel.GetComponent<RectTransform>().localPosition;
    }

    private void OnEnable()
    {
        StartCoroutine(GetSounddata());
    }

    IEnumerator GetSounddata()
    {
      
        var SettingLog = dbmanager.Table<GameSetting>().FirstOrDefault();
        if (SettingLog != null)
        {
            Camera.main.gameObject.GetComponent<AudioSource>().volume = SettingLog.Music;
            PlayerPrefs.SetString("VibrationEnable", SettingLog.Vibration == 1? "true":"false");
        }
        else
        {
            Camera.main.gameObject.GetComponent<AudioSource>().volume =1;
        }
        yield return new WaitForSeconds(0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Timeraction()
    {
        totalsecond = mint * 60f;
        mint = mint - 1;
        sec = 60;
        TotalSec = totalsecond;
        StartCoroutine(Countdowntimer());
    }

    //----------------------------------------for buttons panel animation---------------------------
    public void Movebuttonpanel()
    {
        if(buttonpanel.GetComponent<RectTransform>().localPosition == incomepos)
        {
            panel_btn.GetComponent<Image>().sprite = in_sprite;
            iTween.MoveTo(buttonpanel, iTween.Hash("position", outpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
            is_closed = true;
            StopCoroutine("offsliderpanel");
        }
        else
        {
            panel_btn.GetComponent<Image>().sprite = out_sprite;
            iTween.MoveTo(buttonpanel, iTween.Hash("position", incomepos, "easeType", iTween.EaseType.linear, "isLocal", true, "time", 0.5));

            StartCoroutine(offsliderpanel(buttonpanel, panel_btn, outpos));
        }
       
    }

    public void NormalMovebuttonpanel()
    {
        Debug.Log("Hitting");
        if (normalbuttonpanel.GetComponent<RectTransform>().localPosition == normalpanelin_pos)
        {

            Debug.Log(" out Hitting");
            is_closed = true;
            panel_btn2.GetComponent<Image>().sprite = in_sprite;
            iTween.MoveTo(normalbuttonpanel, iTween.Hash("position", nomalpanelout_pos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
            StopCoroutine("offsliderpanel");
        }
        else
        {

            Debug.Log(" in hitting Hitting");
            panel_btn2.GetComponent<Image>().sprite = out_sprite;
            iTween.MoveTo(normalbuttonpanel, iTween.Hash("position", normalpanelin_pos, "easeType", iTween.EaseType.linear, "isLocal", true, "time", 0.5));
            StartCoroutine(offsliderpanel(normalbuttonpanel, panel_btn2, nomalpanelout_pos));
        }
        
    }
    //this method will close the slider panel if it is open more then 5f;
    IEnumerator offsliderpanel(GameObject sliderpanel,GameObject button,Vector3 offPos)
    {
        yield return new WaitForSeconds(sliderOffTime);
        Debug.Log("running");
        if (!is_closed)
        {
            button.GetComponent<Image>().sprite = in_sprite;
            iTween.MoveTo(sliderpanel, iTween.Hash("position", offPos, "easeType", iTween.EaseType.linear, "isLocal", true, "time", 0.5));
        }
        else
        {
            is_closed = false;
        }
    }

    public IEnumerator Countdowntimer()
    {
       
        yield return new WaitForSecondsRealtime(1f);
        if (sec > 0)
        {
            sec--;
        }
      
        if(sec == 0 && mint != 0)
        {
            mint--;
            sec = 60;
        }
        timertext.text = mint + " : " + sec;
        Timefilling();
        StartCoroutine(Countdowntimer());
    }

    void Timefilling()
    {
        totalsecond--;
        float fill = (float)totalsecond / TotalSec;
        timerimage.fillAmount = fill;
    }


    public void Infobutton()
    {

    }

    public void PlaynPause()
    {
        if (playcount % 2 == 0)
        {
            play_btn.GetComponent<Image>().sprite = pause;
            Time.timeScale = 1f;
            pausepanel.SetActive(false);
            AudioListener.pause = false;
            
        }
        else
        {
            play_btn.GetComponent<Image>().sprite = play;
            Time.timeScale = 0f;
            pausepanel.SetActive(true);
            AudioListener.pause = true;

        }
        playcount += 1;
    }

    public void quit()
    {
        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            Application.Quit();
        }
        
    }

    public void Volumecontrol(GameObject Volume_btn)
    {
        if (Volume_btn.GetComponent<Image>().sprite == slient)
        {
            Volume_btn.GetComponent<Image>().sprite = volume;
            AudioListener.volume = 1;

        }
        else
        {
            Volume_btn.GetComponent<Image>().sprite = slient;
            AudioListener.volume = 0;

        }
        volume_count += 1;
    }

    //public void Movedashborad()
    //{
    //    if (dashboard_count % 2 == 0)
    //    {
    //        Debug.Log("income");
    //        dashboard_btn.GetComponent<Image>().sprite = out_sprite;
    //        iTween.MoveTo(leftdashboard, iTween.Hash("position", dashboard_outpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
    //    }
    //    else
    //    {
    //        Debug.Log("outcome");
    //       dashboard_btn.GetComponent<Image>().sprite = in_sprite;
    //        iTween.MoveTo(leftdashboard, iTween.Hash("position", dashboard_inpos, "easeType", iTween.EaseType.linear, "isLocal", true, "time", 0.5));
    //    }
    //    dashboard_count += 1; 
    //}

    public void gallery_msg()
    {
        string msg = "Coming Soon!!!";
        StartCoroutine(Messagedisplay(msg));
    }


    public void leaderboard_msg()
    {
        string msg = "Coming Soon!!!";
        StartCoroutine(Messagedisplay(msg));
    }

    IEnumerator Messagedisplay(string msg)
    {
        yield return new WaitForSeconds(0.1f);
        Status_msg_panel.SetActive(true);
        Status_msg_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = msg;
        iTween.ScaleTo(Status_msg_panel, Vector3.one, 0.8f);
        yield return new WaitForSeconds(2f);
        iTween.ScaleTo(Status_msg_panel, Vector3.zero, 0.8f);
        yield return new WaitForSeconds(1f);
        Status_msg_panel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        Status_msg_panel.SetActive(false);
    }

    public void dashboard_show_panel()
    {
        if (dashboard_panel.activeInHierarchy)
        {
            dashboard_panel.SetActive(false);
        }
        else
        {
            dashboard_panel.SetActive(true);
            if (gallery_panel.activeInHierarchy)
            {
                gallery_panel.SetActive(false);
            }
            clickcount += 1;
            panel_btn.GetComponent<Image>().sprite = in_sprite;
            iTween.MoveTo(buttonpanel, iTween.Hash("position", outpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
        }
    }

    public void gallery_show_panel(GameObject sliderpanel)
    {

        if (gallery_panel.activeInHierarchy)
        {
            gallery_panel.SetActive(false);
        }
        else
        {

            gallery_panel.SetActive(true);
            panel_btn2.GetComponent<Image>().sprite = in_sprite;
            iTween.MoveTo(normalbuttonpanel, iTween.Hash("position", nomalpanelout_pos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
            if (dashboard_panel.activeInHierarchy)
            {
                dashboard_panel.SetActive(false);
            }

            clickcount += 1;
            if(sliderpanel.name == buttonpanel.name)
            {
                panel_btn.GetComponent<Image>().sprite = in_sprite;
                iTween.MoveTo(buttonpanel, iTween.Hash("position", outpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
            }
            else
            {
                panel_btn2.GetComponent<Image>().sprite = in_sprite;
                iTween.MoveTo(normalbuttonpanel, iTween.Hash("position", nomalpanelout_pos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
            }
    
        }

    }


    public void exitpanelopen(GameObject exitpage)
    {
        if (exitpage.activeInHierarchy)
        {
            exitpage.SetActive(false);
        }
        else
        {
            exitpage.SetActive(true);
        }
    }

    //public void logoutaction(GameObject exitpage)
    //{
    //    exitpage.SetActive(false);
    //    StartCoroutine(logouttask());
    //}
    public void logoutaction()
    {
        StartCoroutine(logouttask());
    }

    IEnumerator logouttask()
    {
        panel_btn2.GetComponent<Image>().sprite = in_sprite;
        iTween.MoveTo(normalbuttonpanel, iTween.Hash("position", nomalpanelout_pos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
        yield return new WaitForSeconds(0.6f);
        logoutpage.SetActive(true);
        //Time.timeScale = 0f;
    }

    public void YesLogout()
    {
        
        logoutpage.SetActive(false);
        StartCoroutine(afterlogout());
    }
    public void LogoutCancel()
    {
        
    }

    IEnumerator afterlogout()
    {
        yield return new WaitForSeconds(0.1f);
        loadinganim.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        loadinganim.SetActive(false);
        logoutpanel.SetActive(true);
        iTween.ScaleTo(logoutmsg, Vector3.one, 0.8f);
        yield return new WaitForSeconds(1.8f);
        iTween.ScaleTo(logoutmsg, Vector3.zero, 0.8f);
        PlayerPrefs.DeleteKey("logged");
        yield return new WaitForSeconds(0.8f);
        Destroy(instance.gameObject);
        SceneManager.LoadScene(0);
    }

    public void info_updatemsgaction(string msg)
    {
        panel_btn2.GetComponent<Image>().sprite = in_sprite;
        iTween.MoveTo(normalbuttonpanel, iTween.Hash("position", nomalpanelout_pos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
        StartCoroutine(updatemsgtask(msg));
    }

    public void leader_updatemsgaction(string msg)
    {
        panel_btn.GetComponent<Image>().sprite = in_sprite;
        iTween.MoveTo(buttonpanel, iTween.Hash("position", outpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
        StartCoroutine(updatemsgtask(msg));
    }

    IEnumerator updatemsgtask(string msg)
    {
        updatemsg.transform.GetChild(0).gameObject.GetComponent<Text>().text = msg;
        iTween.ScaleTo(updatemsg, Vector3.one, 0.8f);
        yield return new WaitForSeconds(2f);
        iTween.ScaleTo(updatemsg, Vector3.zero, 0.8f);
        yield return new WaitForSeconds(0.8f);
        updatemsg.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
    }

    public void showleaderbaord(GameObject panel)
    {
        Leaderboard.SetActive(true);
        panel_btn2.GetComponent<Image>().sprite = in_sprite;
        if(panel.name == normalbuttonpanel.name)
        {
            iTween.MoveTo(normalbuttonpanel, iTween.Hash("position", nomalpanelout_pos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
        }
        else
        {
            iTween.MoveTo(buttonpanel, iTween.Hash("position", outpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
        }
    }

    public void ShowFeedBack(GameObject panel)
    {
        FeedBackPage.SetActive(true);
        if (panel.name == normalbuttonpanel.name)
        {
            iTween.MoveTo(normalbuttonpanel, iTween.Hash("position", nomalpanelout_pos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
        }
        else
        {
            iTween.MoveTo(buttonpanel, iTween.Hash("position", outpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
        }
    }

    public void ShowOverallDashboard()
    {
        if (OverallPage.activeInHierarchy)
        {
            OverallPage.SetActive(false);
        }
        else
        {
            OverallPage.SetActive(true);
            panel_btn.GetComponent<Image>().sprite = in_sprite;
            iTween.MoveTo(buttonpanel, iTween.Hash("position", outpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
        }
    }

    public void OpenSettingPAge(GameObject sliderpanel)
    {
        SettingPage.SetActive(true);
        if (sliderpanel.name == buttonpanel.name)
        {
            panel_btn.GetComponent<Image>().sprite = in_sprite;
            iTween.MoveTo(buttonpanel, iTween.Hash("position", outpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
        }
        else
        {
            panel_btn2.GetComponent<Image>().sprite = in_sprite;
            iTween.MoveTo(normalbuttonpanel, iTween.Hash("position", nomalpanelout_pos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
        }
    }

}
