using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SimpleSQL;
using System.Linq;
using UnityEngine.Networking;
using System;
using UnityEngine.EventSystems;

public class ParentPageHandler : MonoBehaviour
{
    public Text Parentsname;
    public GameObject logoutpage, loadinganim, logoutpanel, logoutmsg;
    static StartpageController HomepageInstance;


    [Header("API INTEGRATION")]
    public string MainUrl;
    public string GetParentLogApi,AchivementShelfApi;
    public GameObject childBtnPrefeb;
    public Transform ChildBtnHandler;
    private List<string> ChildBtnName = new List<string>();

    //LOCAL DB CONNECTION
    public SimpleSQLManager dbmanager;

    public Sprite Clicked, NotCliked;
    public Text ChildNameTxt, GradeTxt, SchoolNameTxt, OverallSCoreTxt, OverallScorePoint,ZonePoint,BonusPoint;

    [Header("ACHIVEMENT BADGE INFO")]
    public Image Scorebadge;
    public Image ActiveBadge, EagleBadge;
    public Sprite HighScoreBadge, MostActiveplayer, EagleEyeBadge;
    public Sprite FadeHighScoreBadge, FadeMostActiveplayer, FadeEagleBadge;
    public Image GameBadge;
    public List<Sprite> Badges;
    private List<string> Badgename = new List<string> { "rookie", "officer", "leader", "champion" };


    //ON GOING AND UPCOMING DIY VARIABLES
    public Image OngoingDiy, UpcomingDiy;
    public Text CurrentDiy, ComingDiy;
    [HideInInspector] public string baseUrl;
    private List<GameObject> Childbtn = new List<GameObject>();
    void Start()
    {
        HomepageInstance = StartpageController.Home_instane;
    }

    private void OnEnable()
    {
        Childbtn.Clear();
        ChildBtnName.Clear();
        Parentsname.text = PlayerPrefs.GetString("parentname");
        StartCoroutine(GetParentdetails());
    }

    IEnumerator GetParentdetails()
    {
        string HittingUrl = $"{MainUrl}{GetParentLogApi}?UID={PlayerPrefs.GetInt("UID")}";
        WWW request = new WWW(HittingUrl);
        yield return request;
        if(request.text != null)
        {
            Debug.Log("Parent complete log " + request.text);
            if (request.text != "[]" && request.text != "")
            {
                ParentLogModel Log = Newtonsoft.Json.JsonConvert.DeserializeObject<ParentLogModel>(request.text);
                Debug.Log("Parent complete log " + request.text);
                int ChildCounter = 1;
                var ChildLog = Log.ChildList;

                ChildLog.ForEach(y =>
                {
                    baseUrl = y.BaseUrl;
                    GameObject gb = Instantiate(childBtnPrefeb, ChildBtnHandler, false);
                    gb.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Child " + ChildCounter;
                    gb.name = y.id_user.ToString();
                    gb.GetComponent<Button>().onClick.RemoveAllListeners();
                    gb.GetComponent<Button>().onClick.AddListener(delegate { GetChildStoredData(y.id_user); });
                    ChildBtnName.Add(gb.name);
                    Childbtn.Add(gb);

                    //PARENT CHILD DETAILS LOCAL DB CONNECTION
                    var LocalLog = dbmanager.Table<ParentChildLog>().FirstOrDefault(a => a.IdChild == y.id_user);
                    if(LocalLog == null)
                    {
                        ParentChildLog tempLog = new ParentChildLog
                        {
                            ParentName = Log.ParentName,
                            IdChild = y.id_user
                        };
                        dbmanager.Insert(tempLog);
                    }
                    else
                    {
                        LocalLog.ParentName = Log.ParentName;
                        LocalLog.IdChild = y.id_user;
                        dbmanager.UpdateTable(LocalLog);
                    }

                    //CHILD DATA STORING 
                    var LocalChildLog = dbmanager.Table<ChildDetailLog>().FirstOrDefault(b => b.IdChild == y.id_user);
                    if(LocalChildLog == null)
                    {
                        ChildDetailLog child_log = new ChildDetailLog
                        {
                            ChildName = y.Name,
                            IdChild = y.id_user,
                            School = y.School,
                            Grade = y.Grade,
                            OverAllScore = y.OverallScore,
                            Zones = y.Zones,
                            BonusScore = y.BonusScore,
                            BaseUrl = y.BaseUrl
                        };

                        dbmanager.Insert(child_log);
                    }
                    else
                    {
                        LocalChildLog.ChildName = y.Name;
                        LocalChildLog.IdChild = y.id_user;
                        LocalChildLog.School = y.School;
                        LocalChildLog.Grade = y.Grade;
                        LocalChildLog.OverAllScore = y.OverallScore;
                        LocalChildLog.Zones = y.Zones;
                        LocalChildLog.BonusScore = y.BonusScore;
                        LocalChildLog.BaseUrl = y.BaseUrl;
                        dbmanager.UpdateTable(LocalChildLog);
                    }

                    //ON GOING DATA BINDING
                    var CurrentDiyLog = y.OngingActivity;
                    if(CurrentDiyLog.Count > 0)
                    {
                        CurrentDiyLog.ForEach(t =>
                        {
                            var log = dbmanager.Table<OnGoingDiyTable>().FirstOrDefault(e => e.UserId == t.id_user);
                            if (log == null)
                            {
                                OnGoingDiyTable ongoingdata = new OnGoingDiyTable
                                {
                                    IdLog = t.id_log,
                                    IdLevel = t.id_level,
                                    UserId = t.id_user,
                                    OID = t.id_org,
                                    IdGameContent = t.id_game_content,
                                    PhotoUrl = t.photo_filename,
                                    Detail = t.detail_info,
                                    DiyDate = t.diy_date_time.ToString()
                                };
                                dbmanager.Insert(ongoingdata);
                            }
                            else
                            {
                                log.IdLog = t.id_log;
                                log.IdLevel = t.id_level;
                                log.UserId = t.id_user;
                                log.OID = t.id_org;
                                log.IdGameContent = t.id_game_content;
                                log.PhotoUrl = t.photo_filename;
                                log.Detail = t.detail_info;
                                log.DiyDate = t.diy_date_time.ToString();
                                dbmanager.UpdateTable(log);
                            }
                        });
                    }
                 


                    //UPCOMING DATA BINDING
                    var upcomingDiy = y.UpcomingActivity;
                    if (upcomingDiy.Count > 0)
                    {
                        upcomingDiy.ForEach(t =>
                        {
                            var log = dbmanager.Table<UpcomingDiyTable>().FirstOrDefault(e => e.UserId == t.id_user);
                            if (log == null)
                            {
                                UpcomingDiyTable ongoingdata = new UpcomingDiyTable
                                {
                                    IdLog = t.id_log,
                                    IdLevel = t.id_level,
                                    UserId = t.id_user,
                                    OID = t.id_org,
                                    IdGameContent = t.id_game_content,
                                    PhotoUrl = t.photo_filename,
                                    Detail = t.detail_info,
                                    DiyDate = t.diy_date_time.ToString()
                                };
                                dbmanager.Insert(ongoingdata);
                            }
                            else
                            {
                                log.IdLog = t.id_log;
                                log.IdLevel = t.id_level;
                                log.UserId = t.id_user;
                                log.OID = t.id_org;
                                log.IdGameContent = t.id_game_content;
                                log.PhotoUrl = t.photo_filename;
                                log.Detail = t.detail_info;
                                log.DiyDate = t.diy_date_time.ToString();
                                dbmanager.UpdateTable(log);
                            }
                        });
                    }
               

                    ChildCounter++;
                });
                Childbtn[0].GetComponent<Button>().image.sprite = Clicked;
                GetChildStoredData(int.Parse(ChildBtnName[0]));
                Debug.Log("child id " + ChildBtnName[0]);
            }
        }   
    }

    void GetChildStoredData(int UserId)
    {
        GameObject clickedobj = EventSystem.current.currentSelectedGameObject;
        if(clickedobj != null)
        {
            for (int a = 0; a < Childbtn.Count; a++)
            {
                if (clickedobj.name == Childbtn[a].name)
                {
                    Childbtn[a].GetComponent<Button>().image.sprite = Clicked;
                }
                else
                {
                    Childbtn[a].GetComponent<Button>().image.sprite = NotCliked;
                }
            }
        }
   
        UpdateAchiveMentdata(UserId);
        var Log = dbmanager.Table<ChildDetailLog>().FirstOrDefault(x => x.IdChild == UserId);
        ChildNameTxt.text = Log.ChildName;
        GradeTxt.text = Log.Grade;
        SchoolNameTxt.text = Log.School;
        OverallSCoreTxt.text = Log.OverAllScore.ToString();
        OverallScorePoint.text = Log.OverAllScore.ToString();
        ZonePoint.text = Log.Zones.ToString();
        BonusPoint.text = Log.BonusScore.ToString();
    }


    void UpdateAchiveMentdata(int Userid)
    {
        StartCoroutine(GetAchivementLog(Userid));
        StartCoroutine(getOngoingDiydata(Userid));
        StartCoroutine(GetUpcomingDiydata(Userid));
    }

    //CHILD ACHIVEMENT DATA BASED ON THERE USER ID
    IEnumerator GetAchivementLog(int UID)
    {
        string HittingUrl = $"{MainUrl}{AchivementShelfApi}?UID={UID}&OID={PlayerPrefs.GetInt("OID")}";
        WWW request = new WWW(HittingUrl);
        yield return request;
        if(request.text != null)
        {
            if(request.text != "[]")
            {
                Debug.Log("achivement log " + request.text);
                AchivementModel AchivementLog = Newtonsoft.Json.JsonConvert.DeserializeObject<AchivementModel>(request.text);
                Scorebadge.sprite = int.Parse(AchivementLog.HighscorerBadge_count) > 0 ? HighScoreBadge : FadeHighScoreBadge;
                ActiveBadge.sprite = int.Parse(AchivementLog.MostactiveplayerBadge_count) > 0 ? MostActiveplayer : FadeMostActiveplayer;
                EagleBadge.sprite = int.Parse(AchivementLog.MostObserventBadge_count) > 0 ? EagleEyeBadge : FadeEagleBadge;
                string currentBadge = AchivementLog.Current_badge;
                if(currentBadge != null)
                {
                    for (int a = 0; a < Badgename.Count; a++)
                    {
                        if (Badgename[a].Equals(currentBadge, System.StringComparison.OrdinalIgnoreCase))
                        {
                            GameBadge.sprite = Badges[a];
                        }

                    }
                }
                else
                {
                    GameBadge.sprite = Badges[0];
                }
                
            }
        }
    }

    IEnumerator getOngoingDiydata(int Userid)
    {
        yield return new WaitForSeconds(0.1f);
        var diyLog = dbmanager.Table<OnGoingDiyTable>().FirstOrDefault(x => x.UserId == Userid);
        if(diyLog != null)
        {
            CurrentDiy.text = diyLog.Detail;
            StartCoroutine(GetProfileImage(diyLog.PhotoUrl, OngoingDiy));
        }
        else
        {
            CurrentDiy.text = "-----";
        }
      
    }
    IEnumerator GetUpcomingDiydata(int Userid)
    {
        yield return new WaitForSeconds(0.1f);
        var diyLog = dbmanager.Table<UpcomingDiyTable>().FirstOrDefault(x => x.UserId == Userid);
        if (diyLog != null)
        {
            ComingDiy.text = diyLog.Detail;
            StartCoroutine(GetProfileImage(diyLog.PhotoUrl,UpcomingDiy));
        }
        else
        {
            ComingDiy.text = "-----";
        }

    }

    //METHOD FOR TAKE IMAGE OF DIY
    IEnumerator GetProfileImage(string image_url,Image DiyImage)
    {
        string HittingUrl = baseUrl + "/" + image_url;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(HittingUrl, true);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            try
            {
                Texture2D texture2d = new Texture2D(1, 1);
                Sprite sprite = null;
                if (www.isDone)
                {
                    if (texture2d.LoadImage(www.downloadHandler.data))
                    {
                        if (texture2d.height <= 12 && texture2d.width <= 12)
                        {
                            Debug.Log("missing");
                        }
                        else
                        {
                            sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
                        }
                    }
                }

                if (sprite != null)
                {
                    DiyImage.sprite = sprite;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }




    void Update()
    {
        
    }

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
