using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Video;
using System.Net;
using UnityEngine.Networking;
using YoutubeExplode.Models;
using SimpleSQL;

public class Zonenarrations : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> zones;
    public List<string> zone_info;
    public GameObject superhero, msg1, msg2, msg3, popupobject, skipbtn, touchinfo, backbtn;
    private string onetime_narration;
    public GameObject videplayer;

    [Header("===zone narrarion======")]
    public GameObject textbox;
    public GameObject zone_text, video_msg_panel,ZoneinfoObject;
    private bool isskipped = false;
    public GameObject startpage;
    public Zonehandler hoomzone, schoolzone, Hospitalzone, officezone, industryzone, parkzone;
    public GameObject last_msg;
    public GameObject YoutubePlayer, skipbutton, videomsg;
    bool VideoPlayed, CheckForEnd; 

    [Header("Stage 2 unlock Portion")]
    [Space(10)]
    public string MainUrl;
    public string  GetRoomDataCMsApi, StageUnlockApi;
    public int ZoneNo, Gamelevel;
    [SerializeField]
    private int Stage2UnlockScore;
    private int totalscoreOfUser;
    public GameObject Stage2popup;
    private bool Stage2unlocked = false;

    public GameObject startpageobj;
    private Generationlevel Mainpage;
    public Sprite greenBackground, CityPage;
    public GameObject Bonusgamepage;
    public GameObject StageWiseLeaderBOard;
    public SimpleSQLManager dbmanager;
    [HideInInspector] public bool AnagramPlayed;
    public GameObject TriviaPage, DeberfingBtn;
    public GameObject deberifingPage;
    [SerializeField] private List<Zonehandler> ZoneScores;
    public YoutubePlayer.YoutubePlayer YoutubeVideoPage;
    void Start()
    {
        AnagramPlayed = false;
        StartCoroutine(GetZoneCMSdata());
        Mainpage = FindObjectOfType<Generationlevel>();
        StartCoroutine(GetYoutubeLink());
    }

    IEnumerator GetYoutubeLink()
    {
        yield return new WaitForSeconds(0.1f);
        var LocalLog = dbmanager.Table<VideoUrls>().FirstOrDefault(x => x.LevelId == 1);
        if(LocalLog != null)
        {
            YoutubeVideoPage.youtubeUrl = LocalLog.VideoLink;
        }
    }

    void OnEnable()
    {

       
       
    }
    void OffInitialPops()
    {
        if (hoomzone.zone_completed || schoolzone.zone_completed || Hospitalzone.zone_completed || officezone.zone_completed
          || industryzone.zone_completed || parkzone.zone_completed)
        {
            skip();
        }
        else
        {
            StartCoroutine(startNarration());
        }
    }

    void SkippedTask()
    {
        StartCoroutine(closevideo_action());
        if (!AnagramPlayed && Stage2unlocked)
        {
            Stage2popup.SetActive(Stage2unlocked);
        }
    }


    IEnumerator last_msg_task()
    {
        last_msg.SetActive(true);
        yield return new WaitForSeconds(5f);
        last_msg.SetActive(false);
    }
    private void OnDisable()
    {
        StopCoroutine("startguide");
        touchinfo.SetActive(false);
        skipbtn.SetActive(false);
        msg1.SetActive(false);
        video_msg_panel.SetActive(false);
        msg2.SetActive(false);
        superhero.SetActive(false);
        popupobject.SetActive(false);
        zone_text.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        zone_text.SetActive(false);
        msg1.SetActive(false);
        msg2.SetActive(false);
        msg3.SetActive(false);
        popupobject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (VideoPlayed)
        {
            if (YoutubePlayer.GetComponent<VideoPlayer>().isPlaying)
            {
                skipbutton.SetActive(true);
                CheckForEnd = true;
            }
            if (CheckForEnd)
            {
                if (!YoutubePlayer.GetComponent<VideoPlayer>().isPlaying)
                {
                    Debug.Log("done video");
                    CheckForEnd = false;
                    VideoPlayed = false;
                    close_video();
                }
            }
        }
    
      
    }

    IEnumerator startguide()
    {
        yield return new WaitForSeconds(0.1f);
        touchinfo.SetActive(true);
        PlayerPrefs.SetString("zone_guide", "done");
        superhero.SetActive(true);
        yield return new WaitForSeconds(1f);
        popupobject.SetActive(true);
        msg1.SetActive(true);
        yield return new WaitForSeconds(10f);
        msg1.SetActive(false);
        msg2.SetActive(true);


    }
    public void for_msg2()
    {
        StartCoroutine(msg2_task());
    }

    IEnumerator msg2_task()
    {
        msg1.SetActive(false);
        msg2.SetActive(true);
        yield return new WaitForSeconds(0.1f);
    }

    public void playVideo()
    {
        video_msg_panel.SetActive(false);
        popupobject.SetActive(false);
        Camera.main.GetComponent<AudioSource>().enabled = false;
        skipbtn.SetActive(true);
        //videplayer.SetActive(true);
        videomsg.SetActive(true);
        VideoPlayed = true;
        YoutubePlayer.SetActive(true);
       // skipbutton.SetActive(true);
        backbtn.SetActive(false);
        msg2.SetActive(false);
    }

    public void close_video()
    {
        VideoPlayed = false;
        StartCoroutine(closevideo_action());
    }

    IEnumerator closevideo_action()
    {
        yield return new WaitForSeconds(0.1f);
        Camera.main.GetComponent<AudioSource>().enabled = true;
        backbtn.SetActive(true);
        videomsg.SetActive(false);
        YoutubePlayer.SetActive(false);
        skipbutton.SetActive(false);
        popupobject.SetActive(true);
        msg3.SetActive(true);
        yield return new WaitForSeconds(10f);
        touchinfo.SetActive(false);
        skipbtn.SetActive(false);
        msg1.SetActive(false);
        msg2.SetActive(false);
        msg3.SetActive(false);
        superhero.SetActive(false);
        popupobject.SetActive(false);
        for (int a = 0; a < zones.Count; a++)
        {
            zones[a].gameObject.SetActive(true);
        }
        Stage2popup.SetActive(Stage2unlocked);
    }
    public void skip()
    {
        StopCoroutine("startguide");
        touchinfo.SetActive(false);
        skipbtn.SetActive(false);
        msg1.SetActive(false);
        msg2.SetActive(false);
        superhero.SetActive(false);
        video_msg_panel.SetActive(false);
        popupobject.SetActive(false);
        StopAllCoroutines();
        startpage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);

        foreach (GameObject e in zones)
        {
            e.transform.GetChild(0).gameObject.SetActive(false);
            e.GetComponent<BoxCollider2D>().enabled = true;
            e.SetActive(false);
        }
        for (int a = 0; a < zones.Count; a++)
        {
            zones[a].gameObject.SetActive(true);
        }

        Stage2popup.SetActive(Stage2unlocked);

    }


    //=======================================================latest pop up ====================================================//


    IEnumerator startNarration()
    {
        for (int a = 0; a < zones.Count; a++)
        {
            zones[a].gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(0.1f);
        superhero.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        textbox.SetActive(true);
        yield return new WaitForSeconds(8f);
        StartCoroutine(zone_highlighter());
    }

    //------------------need to be change========//
    IEnumerator zone_highlighter()
    {
        textbox.SetActive(false);
        skipbtn.SetActive(true);
        zone_text.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        for (int a = 0; a < zones.Count; a++)
        {
            zone_text.transform.GetChild(0).gameObject.GetComponent<Text>().text = zone_info[a];
            startpage.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1);
            zones[a].SetActive(true);
            zones[a].GetComponent<BoxCollider2D>().enabled = false;
            zones[a].transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(6f);
            zone_text.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
            startpage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
            zones[a].transform.GetChild(0).gameObject.SetActive(false);
            zones[a].GetComponent<BoxCollider2D>().enabled = true;
            zones[a].SetActive(false);

        }
        zone_text.SetActive(false);
        video_msg_panel.SetActive(true);

    }

 

    IEnumerator CheckForStage2()
    {
        TriviaPage.SetActive(true);

        string Hitting_url = $"{MainUrl}{StageUnlockApi}?UID={PlayerPrefs.GetInt("UID")}&id_level={1}&id_org_game={1}";
        WWW StageData = new WWW(Hitting_url);
        yield return StageData;
        if (StageData.text != null)
        {
            StageUnlockModel StageModel = Newtonsoft.Json.JsonConvert.DeserializeObject<StageUnlockModel>(StageData.text);
            Stage2unlocked = int.Parse(StageModel.ConsolidatedScore) >= Stage2UnlockScore;
            Debug.Log(" user score  " + StageModel.ConsolidatedScore);
            DeberfingBtn.SetActive(Stage2unlocked);
        }

        TriviaPage.SetActive(false);
        OffInitialPops();

    }



    public void ClosePopup()
    {
        iTween.ScaleTo(Stage2popup, Vector3.zero, 0.4f);
        startpage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
        foreach (GameObject e in zones)
        {
            e.transform.GetChild(0).gameObject.SetActive(false);
            e.GetComponent<BoxCollider2D>().enabled = true;
            e.SetActive(false);
        }
        for (int a = 0; a < zones.Count; a++)
        {
            zones[a].gameObject.SetActive(true);    
        }

    }

    public void PlayBonusGame()
    {
        StartCoroutine(BonusgameTask());
    }

    IEnumerator BonusgameTask()
    {
        ZoneinfoObject.GetComponent<Text>().text = "";
        zone_text.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        startpage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
        for (int a = 0; a < zones.Count; a++)
        {
            zones[a].gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(0.5f);
        iTween.ScaleTo(Stage2popup, Vector3.zero, 0.5f);
        StartCoroutine(Mainpage.scenechanges(startpage, greenBackground));
        yield return new WaitForSeconds(1.2f);
        Bonusgamepage.SetActive(true);

    }

    public void CloseBonusGame()
    {
        StartCoroutine(ClosingTask());
    }
    IEnumerator ClosingTask()
    {
        Bonusgamepage.SetActive(false);
        iTween.ScaleTo(Stage2popup, Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.6f);
        Stage2popup.SetActive(false);
        StartCoroutine(Mainpage.scenechanges(startpage, CityPage));
        yield return new WaitForSeconds(1.2f);
        startpage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
        StageWiseLeaderBOard.SetActive(true);

    }

    public void CloseStageDashboard()
    {
        StageWiseLeaderBOard.SetActive(false);
        deberifingPage.SetActive(true);
        foreach (GameObject e in zones)
        {
            e.transform.GetChild(0).gameObject.SetActive(false);
            e.GetComponent<BoxCollider2D>().enabled = true;
            e.SetActive(false);
        }
        for (int a = 0; a < zones.Count; a++)
        {
            zones[a].gameObject.SetActive(true);
        }
      
    }

    public void closeStagedeberfingPage()
    {
        deberifingPage.SetActive(false);
        foreach (GameObject e in zones)
        {
            e.transform.GetChild(0).gameObject.SetActive(false);
            e.GetComponent<BoxCollider2D>().enabled = true;
            e.SetActive(false);
        }
        for (int a = 0; a < zones.Count; a++)
        {
            zones[a].gameObject.SetActive(true);
        }
    }


    // UPDATED METHODS 
    IEnumerator GetZoneCMSdata()
    {
        string HittingUrl = $"{MainUrl}{GetRoomDataCMsApi}?id_level={1}";
        WWW GetCmsdata = new WWW(HittingUrl);
        yield return GetCmsdata;
        if (GetCmsdata.text != null)
        {
            if (GetCmsdata.text != "[]")
            {
                List<Stage1CMSModel> CmsLog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Stage1CMSModel>>(GetCmsdata.text);
                Debug.Log("list of cms log" + CmsLog.Count);
               int TScore=0;
                CmsLog.ForEach(x =>
                {
                    var LocalCmsLog = dbmanager.Table<WasteGeneration>().FirstOrDefault(y => y.ItemId == x.item_Id);
                    if (LocalCmsLog == null)
                    {
                        WasteGeneration WasteLog = new WasteGeneration
                        {
                            ItemId = x.item_Id,
                            ItemName = x.item_Name,
                            PCscore = x.partialcorrect_point,
                            Cscore = x.correct_point,
                            RoomId = x.id_room
                        };
                        dbmanager.Insert(WasteLog);
                    }
                    else
                    {
                        LocalCmsLog.ItemId = x.item_Id;
                        LocalCmsLog.ItemName = x.item_Name;
                        LocalCmsLog.PCscore = x.partialcorrect_point;
                        LocalCmsLog.Cscore = x.correct_point;
                        LocalCmsLog.RoomId = x.id_room;
                        dbmanager.UpdateTable(LocalCmsLog);

                    }
                    TScore += x.correct_point;
                });
                var percentlog = dbmanager.Table<LevelPercentageTable>().FirstOrDefault(c => c.LevelId == 1).LevelPercentage;
                float value = (float)TScore / 100;
                int FinalLevelScore = (int)value * percentlog;
                Debug.Log("value " + value + "final percantge " + FinalLevelScore);
                //ZoneScores.ForEach(x =>
                //{
                //    x.Stage2UnlockScore = FinalLevelScore;
                //});

                Stage2UnlockScore = FinalLevelScore;
                StartCoroutine(CheckForStage2());
                var scoreconfig = dbmanager.Table<ScoreConfiguration>().FirstOrDefault(a => a.levelId == 1);
                if (scoreconfig == null)
                {
                    ScoreConfiguration log = new ScoreConfiguration
                    {
                        levelId = 1,
                        TotalScore = TScore,
                        PercentScore = percentlog,
                        UnlockScore = FinalLevelScore
                    };
                    dbmanager.Insert(log);
                }
                else
                {
                    scoreconfig.PercentScore = percentlog;
                    scoreconfig.levelId = 1;
                    scoreconfig.TotalScore = TScore;
                    scoreconfig.UnlockScore = FinalLevelScore;
                    dbmanager.UpdateTable(scoreconfig);
                }
            }
        }
    }
}
