using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SimpleSQL;
using System.Linq;

public class TeacherGameFrame : MonoBehaviour
{
    public SimpleSQLManager dbmanager;
    private Vector3 normalpanelin_pos, nomalpanelout_pos;
    public GameObject normaltargetobj, normalbuttonpanel;
    private bool is_closed;
    [SerializeField] private float sliderOffTime;


    [Header("LogOut elemnts")]
    public GameObject logoutpage;
    public GameObject loadinganim, logoutpanel, logoutmsg;
    static StartpageController HomepageInstance;
    public GameObject FeedBackPage, SettingPage;
    void Start()
    {
        HomepageInstance = StartpageController.Home_instane;
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
        }
        else
        {
            Camera.main.gameObject.GetComponent<AudioSource>().volume = 1;
        }
        yield return new WaitForSeconds(0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void NormalMovebuttonpanel()
    {
        
        if (normalbuttonpanel.GetComponent<RectTransform>().localPosition == normalpanelin_pos)
        {
            is_closed = true;
            iTween.MoveTo(normalbuttonpanel, iTween.Hash("position", nomalpanelout_pos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
            StopCoroutine("offsliderpanel");
        }
        else
        {

            Debug.Log(" in hitting Hitting");
            iTween.MoveTo(normalbuttonpanel, iTween.Hash("position", normalpanelin_pos, "easeType", iTween.EaseType.linear, "isLocal", true, "time", 0.5));
            StartCoroutine(offsliderpanel(normalbuttonpanel, nomalpanelout_pos));
        }

    }

    IEnumerator offsliderpanel(GameObject sliderpanel, Vector3 offPos)
    {
        yield return new WaitForSeconds(sliderOffTime);
        if (!is_closed)
        {
            iTween.MoveTo(sliderpanel, iTween.Hash("position", offPos, "easeType", iTween.EaseType.linear, "isLocal", true, "time", 0.5));
        }
        else
        {
            is_closed = false;
        }
    }


    public void OpenSettingPAge(GameObject sliderpanel)
    {
        SettingPage.SetActive(true);
        if (sliderpanel.name == normalbuttonpanel.name)
        {
            iTween.MoveTo(normalbuttonpanel, iTween.Hash("position", nomalpanelout_pos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
        }
    }
    public void ShowFeedBack(GameObject panel)
    {
        FeedBackPage.SetActive(true);
        if (panel.name == normalbuttonpanel.name)
        {
            iTween.MoveTo(normalbuttonpanel, iTween.Hash("position", nomalpanelout_pos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.5f));
        }
      
    }

    //COMPLETE LOGOUT AND QUIT METHOD SECTION
    public void logoutaction()
    {
        logoutpage.SetActive(true);
    }


    public void YesLogout()
    {
        logoutpage.SetActive(false);
        StartCoroutine(afterlogout());
    }
    public void LogoutCancel()
    {
        logoutpage.SetActive(false);
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
        Destroy(HomepageInstance);
        SceneManager.LoadScene(0);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
