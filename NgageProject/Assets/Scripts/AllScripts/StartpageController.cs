using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Linq;
using UnityEngine.Networking;
using System;
using SimpleSQL;


public class StartpageController : MonoBehaviour
{
    public static StartpageController Home_instane;
    public GameObject HomepageObject;
    [Header("login section")]
    public InputField username_input;
    public InputField password_input;
    public GameObject loginpage, gameheading, profilesetup_page, statusMsgpanel, loginbutton, gameFrame;
    public AESalgorithm asealgorithm;
    public GameObject firstscreen;
    public Narrationmanagement narration_activity;
    public string Mainurl, login_API, UpdateApkApi, GetBonusScoreApi, levelMovementApi, StageUnlockApi, VideoUrlApi;
    public JsonData login_json;
    public GameObject login_msg;
    public float login_msg_time;
    public Toggle remeberme;
    [Header("-----profile setup elements-----")]
    public GameObject boy;
    public GameObject girl, submitbutton, characterpage, userdetailspage;
    public Image characterimage;
    public InputField profile_name;
    public Dropdown grade;
    private string charactertype;
    public Sprite boysprite, girlsprite, boyface, girlface;
    public GameObject buttonpanel, profileimage;
    public GameObject headingframe;
    public Text username_leftdashboard;
    public GameObject SignUp_panel;


    //------------------------------------//
    private Vector3 homebuttonpos, loginpage_pos;
    public GameObject homebuttonpage, homebutton;


    [Header("==superher narration part==")]
    public GameObject superhero;
    public Text narrationtext;
    public GameObject generation, seperation, management, next_btn_one_time;
    // Start is called before the first frame update
    public List<GameObject> levels;
    public List<GameObject> sublevels;
    public GameObject toplayer, midlayer, zonelayer;

    [Header("=====generation level sprites=====")]
    public Sprite toplayer_sprite;
    public Sprite midlayer_sprite, home_sprite, industry_sprite, hospital_sprite, forest_sprite, school_sprite, park_sprite, Dirty_Sky, Moderate_sky, Clear_sky, Stage2Bg;
    public Button Back, dashboardbtn, settingpanelbtn;


    [Header("====Generation level===")]
    public GameObject Generation_level;
    public List<GameObject> G_levels;
    public Text popupNotification;
    public Text zoneinfo;
    [Header("=====for double click======")]
    private int tap_count;
    public GameObject showMsg;
    int TapCount;
    public float MaxDubbleTapTime;
    float NewTime;
    public GameObject loadinganim;

    [Header("button slider panels")]
    public GameObject stagesliderpanel;
    public GameObject homesliderpanel;
    public GameObject UpdatedHomePage, UpdatedLoginPage;

    //===================== ON BORDING VARIABLES================= //
    public GameObject TriviaPage;
    public GameObject YoutubeVideopage, skipVideo;

    private bool videoPlayed, checkforEnd;
    public string nextSceneAddress;
    public GameObject UpdatePage, LoginPage;
    private bool UpdateAvailable;
    public SimpleSQLManager dbmanager;
    public StageUnlockingPage StageunlockScores;
    public GameObject ForgotpasswordPage;
    private int Stage1Score, Stage2Score;
    public YoutubePlayer.YoutubePlayer youtubePage;
    public Button ShoePasswordBtn;
    public Sprite OpenEye, CloseEye;
    private void Awake()
    {
        Debug.Log(Application.persistentDataPath);
        loginpage_pos = loginpage.GetComponent<RectTransform>().localPosition;
        homebuttonpos = homebuttonpage.GetComponent<RectTransform>().localPosition;

        // METHOD FOR FORCE FULLY UPDATE FOR USER
        StartCoroutine(CheckUpdatedApk());
        StartCoroutine(GetCmsVideoLinks());
    }

    IEnumerator CheckUpdatedApk()
    {

        string HittingUrl = $"{Mainurl}{UpdateApkApi}?vid={Application.version}";
        WWW checkApkwww = new WWW(HittingUrl);
        yield return checkApkwww;
        if (checkApkwww.text != null)
        {
            UpdateAvailable = checkApkwww.text == "\"1|Success\"" ? false : true;
        }
    }

    IEnumerator GetCmsVideoLinks()
    {
        string HittingUrl = $"{Mainurl}{VideoUrlApi}";
        PostUrlModel postLog = new PostUrlModel
        {
            id_level = "1",
            video_type = "1"
        };

        string dataLog = Newtonsoft.Json.JsonConvert.SerializeObject(postLog);

        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, dataLog))
        {
            Request.method = UnityWebRequest.kHttpVerbPOST;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                if (Request.downloadHandler.text != null)
                {
                    if (Request.downloadHandler.text != "[]")
                    {
                        Debug.Log("data " + Request.downloadHandler.text);
                        List<VideoUrlModel> VideoLog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VideoUrlModel>>(Request.downloadHandler.text);
                        VideoLog.ForEach(x =>
                        {
                            var LocalLog = dbmanager.Table<VideoUrls>().FirstOrDefault(y => y.LevelId == x.id_level);
                            if (LocalLog == null)
                            {
                                VideoUrls videolog = new VideoUrls
                                {
                                    LevelId = x.id_level,
                                    VideoId = x.id_video,
                                    VideoLink = x.video_url,
                                    VideoType = x.video_type,
                                    UrlType = x.url_type
                                };
                                dbmanager.Insert(videolog);
                            }
                            else
                            {
                                LocalLog.LevelId = x.id_level;
                                LocalLog.VideoId = x.id_video;
                                LocalLog.VideoLink = x.video_url;
                                LocalLog.VideoType = x.video_type;
                                LocalLog.UrlType = x.url_type;
                                dbmanager.UpdateTable(LocalLog);
                            }
                            if (x.id_level == 0)
                            {
                                youtubePage.youtubeUrl = x.video_url;
                            }
                        });


                    }
                }
            }
        }
       
    }

    
    void Start()
    {
        TapCount = 0;
        
        // dashboardbtn.gameObject.SetActive(true);

        // Initialtask();
    }

    private void OnEnable()
    {
        Debug.Log("diy path " + UnityEngine.Application.persistentDataPath);
        if(Home_instane != null)
        {
            
            Destroy(Home_instane);
            ComeFromStages();
            Home_instane = this;
        }
        else
        {
            Home_instane = this;
            StartCoroutine(sceneappear());
        }
        DontDestroyOnLoad(Home_instane);
       

    }





    void ComeFromStages()
    {
        //UpdatedHomePage.SetActive(true);
        superhero.SetActive(true);
        settingpanelbtn.gameObject.SetActive(true);
        superhero.GetComponent<CompleteIntroPage>().after_badge();
    }

    IEnumerator sceneappear()
    {
        float shadevalue = HomepageObject.GetComponent<Image>().color.a;
        while(shadevalue < 1)
        {
            HomepageObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, shadevalue);
            shadevalue += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        UpdatedHomePage.SetActive(true);


    }
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (username_input.text != "" && password_input.text != "")
            {
                loginbutton.GetComponent<Button>().interactable = true;
            }
            else
            {
                loginbutton.GetComponent<Button>().interactable = false;
            }

            if (charactertype != "" && profile_name.text != "" && grade.value.ToString() != "")
            {
                submitbutton.GetComponent<Button>().interactable = true;
            }
            else
            {
                submitbutton.GetComponent<Button>().interactable = false;
            }


            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Ended)
                {
                    TapCount += 1;
                }
                if (TapCount == 1)
                {
                    NewTime = Time.time + MaxDubbleTapTime;
                }
                else if (TapCount == 2 && Time.time <= NewTime)
                {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)), Vector2.zero);
                    if (hit.collider.transform.gameObject.name == "home")
                    {
                        StartCoroutine(midlevel_task(home_sprite, 0));
                        TapCount = 0;
                    }
                    if (hit.collider.transform.gameObject.name == "school")
                    {
                        StartCoroutine(midlevel_task(school_sprite, 1));
                        TapCount = 0;
                    }
                }

            }
            if (Time.time > NewTime)
            {
                TapCount = 0;
            }

            if (videoPlayed)
            {
                if (YoutubeVideopage.GetComponent<VideoPlayer>().isPlaying)
                {
                    skipVideo.SetActive(true);
                    checkforEnd = true;
                }
                if (checkforEnd)
                {
                    if (!YoutubeVideopage.GetComponent<VideoPlayer>().isPlaying)
                    {
                        videoPlayed = false;
                        checkforEnd = false;
                        SkipVideoTask();
                    }
                }
            }
        }
    }



    public void Initialtask()
    {
        popupNotification.gameObject.SetActive(false);
        superhero.SetActive(true);
    }

    IEnumerator superheronarration()
    {
        yield return new WaitForSeconds(0.1f);
        narrationtext.text = "generation narration";
        generation.SetActive(true);
        yield return new WaitForSeconds(3f);
        narrationtext.text = "separation narration";
        seperation.SetActive(true);
        yield return new WaitForSeconds(3f);
        narrationtext.text = "management narration";
        management.SetActive(true);
        yield return new WaitForSeconds(3f);
        superhero.SetActive(false);
        toplayer.SetActive(true);
        PlayerPrefs.SetString("isfirst","done");
        for (int a = 0; a < levels.Count; a++)
        {
            levels[a].gameObject.GetComponent<Animator>().enabled = false;
        }
        StartCoroutine(popup());


    }

    public void afternarrationtask()
    {
        int count = superhero.transform.childCount;
        for(int i = 0; i < count; i++)
        {
            superhero.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        for (int a = 0; a < levels.Count; a++)
        {
            levels[a].gameObject.GetComponent<Animator>().enabled = false;
        }
       
        StartCoroutine(popup());
    }

    public IEnumerator scenechanges(GameObject parentobejct, Sprite new_sprite)
    {
        yield return new WaitForSeconds(0.1f);
        float bgvalue = parentobejct.GetComponent<Image>().color.a;
        while (bgvalue > 0)
        {
            bgvalue -= 0.1f;
            yield return new WaitForSeconds(0.05f);
            parentobejct.GetComponent<Image>().color = new Color(1, 1, 1, bgvalue);
        }
        parentobejct.GetComponent<Image>().sprite = new_sprite;
        bgvalue = parentobejct.GetComponent<Image>().color.a;

        while (bgvalue < 1)
        {
            bgvalue += 0.1f;
            yield return new WaitForSeconds(0.05f);
            parentobejct.GetComponent<Image>().color = new Color(1, 1, 1, bgvalue);
        }

    }


    //------------singal method for top layer level like generation, seperation & management -------------------//
    public void Levelselection(GameObject selectedobj)
    {
        string buttonName = selectedobj.name;
        levelaction(buttonName, selectedobj);

       
    }
    IEnumerator levelanim(GameObject selectedobj)
    {
        yield return new WaitForSeconds(0.1f);
        for (int a= 0; a < levels.Count; a++)
        {
            levels[a].gameObject.GetComponent<Animator>().enabled = false;
            iTween.ScaleTo(levels[a].gameObject, Vector3.zero, 0.5f);
        }
        
    }

    void levelaction(string buttobiobj,GameObject selectedobj)
    {
        switch (buttobiobj)
        {
            case "Generation":
                StartCoroutine(ZoneGameActive(1, Dirty_Sky));
                break;
            case "seperation":
                StartCoroutine(ZoneGameActive(2, Moderate_sky));
                break;
            case "Mangement":
                StartCoroutine(ZoneGameActive(3, Clear_sky));
                break;
            default:
                break;
        }
    }

    IEnumerator ZoneGameActive(int Sceneno,Sprite sky)
    {
        yield return new WaitForSeconds(0.2f);
        toplayer.SetActive(false);
        StartCoroutine(scenechanges(HomepageObject, sky));
        yield return new WaitForSeconds(1.4f);
        SceneManager.LoadScene(Sceneno);

    }
    //-------------------end of method----------------------//

    //--------------------singal method for middle layer levels like, home,hospital,factory etc-------------------------//

    public void sublevelmethod(GameObject selectedbtn)
    {
           midlayer.SetActive(false);
           string buttonname = selectedbtn.name;
           selectedbtn.transform.GetChild(0).gameObject.SetActive(false);
           zoneinfo.text = "";
           sublevelactions(buttonname);
    }
    
    void sublevelactions(string name)
    {
        switch (name.ToLower())
        {
            case "home":
                 StartCoroutine(midlevel_task(home_sprite, 0));
                break;
            case "school":
                 StartCoroutine(midlevel_task(school_sprite, 1));
                break;
            case "hospital":
                StartCoroutine(midlevel_task(home_sprite, 2));
                break;
            case "office":
                StartCoroutine(midlevel_task(home_sprite, 3));
                break;
            case "industry":
                StartCoroutine(midlevel_task(home_sprite, 4));
                break;
            case "park":
                StartCoroutine(midlevel_task(home_sprite, 5));
                break;
            default:
                Debug.Log("unique zone");
                break;
        }

    }

    

    IEnumerator showindication(string msg,Text msgbox)
    {
        yield return new WaitForSeconds(0.1f);
        msgbox.text = msg;
        iTween.ScaleTo(msgbox.gameObject, Vector3.one, 0.5f);
        yield return new WaitForSeconds(2f);
        iTween.ScaleTo(msgbox.gameObject, Vector3.zero, 0.5f);
        msgbox.text = "";
    }

    IEnumerator midlevel_task(Sprite scene_sprite,int level_id)
    {
       
        yield return new WaitForSeconds(0.1f);
        Camera.main.GetComponent<AudioSource>().enabled = false;
        zonelayer.SetActive(false);
        StartCoroutine(scenechanges(HomepageObject, scene_sprite));
        yield return new WaitForSeconds(1f);
        Generation_level.SetActive(true);
        G_levels[level_id].SetActive(true);


    }

    IEnumerator popup()
    {
        StartCoroutine(scenechanges(HomepageObject, home_sprite));
        yield return new WaitForSeconds(1.2f);
        toplayer.SetActive(true);
        for (int i =0;i < levels.Count; i++)
        {
            iTween.ScaleTo(levels[i].gameObject, Vector3.one, 0.3f);
            yield return new WaitForSeconds(0.2f);
        }
        for (int a = 0; a < levels.Count; a++)
        {
            levels[a].gameObject.GetComponent<Animator>().enabled = true;
        }

    }

   public void midlayerenable()
    {
        homesliderpanel.SetActive(false);
        stagesliderpanel.SetActive(true);
        toplayer.SetActive(false);
        Back.gameObject.SetActive(true);
        Back.onClick.RemoveAllListeners();
        Back.onClick.AddListener(delegate { backfrommidlayer(); });
        midlayer.SetActive(true);
    }
    
    void fromLevelToMidlayer()
    {
        StartCoroutine(fromleveltimidTask());
        StartCoroutine(scenechanges(HomepageObject, midlayer_sprite));

    }

    IEnumerator fromleveltimidTask()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(scenechanges(HomepageObject, midlayer_sprite));
        yield return new WaitForSeconds(1f);
        for (int a = 0; a < sublevels.Count; a++)
        {
            sublevels[a].transform.localScale = Vector3.one;
            sublevels[a].GetComponent<Animator>().enabled = true;
        }
        midlayer.SetActive(true);
        Back.onClick.RemoveAllListeners();
        Back.onClick.AddListener(delegate { backfrommidlayer(); });

    }


    public void backfrommidlayer()
    {
        foreach(GameObject e in G_levels)
        {
            e.gameObject.GetComponent<Zonehandler>().zone_completed = false;
            e.gameObject.GetComponent<Zonehandler>().final_completed = false;
        }
        homesliderpanel.SetActive(true);
        stagesliderpanel.SetActive(false);
        StartCoroutine(back_from_stage());
       
    }
    IEnumerator back_from_stage()
    {
        midlayer.SetActive(false);
        StartCoroutine(scenechanges(HomepageObject, home_sprite));
        yield return new WaitForSeconds(1.2f);
        toplayer.SetActive(true);
    }
    public void play()
    {
        string remeber_data = PlayerPrefs.GetString("logged");
        if (UpdateAvailable)
        {
            UpdatedLoginPage.SetActive(true);
            LoginPage.SetActive(false);
            UpdatePage.SetActive(true);
        }
        else
        {
            if (remeber_data == "true")
            {
                if (PlayerPrefs.GetString("Role").Equals("Parent", System.StringComparison.OrdinalIgnoreCase))
                {
                    Debug.Log(PlayerPrefs.GetInt("UID") + " " + PlayerPrefs.GetInt("OID"));
                    SceneManager.LoadScene(4);
                }else if(PlayerPrefs.GetString("Role").Equals("teacher", System.StringComparison.OrdinalIgnoreCase))
                {
                    Debug.Log(PlayerPrefs.GetInt("UID") + " " + PlayerPrefs.GetInt("OID"));
                    SceneManager.LoadScene(5);
                }
                else
                {
                    settingpanelbtn.gameObject.SetActive(true);
                    StartCoroutine(game_play());
                }
            }
            else
            {
                StartCoroutine(play_action());
            }
        }
    }

    public void SignUp_page()
    {
        StartCoroutine(signup_task());
    }

   
    IEnumerator signup_task()
    {
        iTween.MoveTo(loginpage, iTween.Hash("position", loginpage_pos, "easeType", iTween.EaseType.linear, "isLocal", true,
      "time", 0.5f));
        yield return new WaitForSeconds(0.6f);
        homebutton.SetActive(false);
        loginpage.SetActive(false);
        SignUp_panel.SetActive(true);
    }
    public void forgorpassword()
    {
        ForgotpasswordPage.SetActive(true);
    }

    IEnumerator game_play()
    {
        yield return new WaitForSeconds(0.3f);
        UpdatedHomePage.SetActive(false);
        Debug.Log(PlayerPrefs.GetInt("UID") + " " + PlayerPrefs.GetInt("OID"));
        StartCoroutine(scenechanges(HomepageObject, toplayer_sprite));
        yield return new WaitForSeconds(1.2f);
        homebuttonpage.SetActive(false);
        string profile_data = PlayerPrefs.GetString("profile_done");
        superhero.SetActive(true);
        StartCoroutine(GetBonusScoreTask());
        StartCoroutine(LevelpercentageScore());
        StartCoroutine(getScoreConfigdata());
        StartCoroutine(CheckStage2Unlock());


    }
    IEnumerator play_action()
    {
        yield return new WaitForSeconds(0.3f);
        string msg = PlayerPrefs.GetString("login_msg");
        if (msg != "done")
        {
            login_msg.SetActive(true);
        }
        else
        {
            login_msg.SetActive(false);
        }
        UpdatedHomePage.SetActive(false);
        LoginPage.SetActive(true);
        UpdatedLoginPage.SetActive(true);
        homebuttonpage.SetActive(false);
    }

    public void backtohome()
    {
        StartCoroutine(backhome_action());
    }

    IEnumerator backhome_action()
    {
        
        homebutton.SetActive(false);
        homebuttonpage.SetActive(true);
        iTween.MoveTo(loginpage, iTween.Hash("position", loginpage_pos, "easeType", iTween.EaseType.linear, "isLocal", true,
          "time", 0.5f));
        yield return new WaitForSeconds(0.6f);
        loginpage.SetActive(false);


    }

    public void HidePassword()
    {
        if (ShoePasswordBtn.image.sprite.name.Equals("closeeye", System.StringComparison.OrdinalIgnoreCase))
        {
            ShoePasswordBtn.image.sprite = OpenEye;
            password_input.contentType = InputField.ContentType.Standard;
            password_input.ForceLabelUpdate();

        }
        else
        {
            ShoePasswordBtn.image.sprite = CloseEye;
            password_input.contentType = InputField.ContentType.Password;
            password_input.ForceLabelUpdate();
        }
    }
    public void login_task()
    {
        PlayerPrefs.SetString("login_msg", "done");
        StartCoroutine(login_action());
    }

    IEnumerator login_action()
    {
        iTween.ScaleTo(UpdatedLoginPage, Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.6f);
        var username = username_input.text;
        var password = password_input.text;
        string encryptedpassword1 = asealgorithm.getEncryptedString(password);
        Debug.Log("encrypred password " + encryptedpassword1);

        UpdatedLoginPage.SetActive(false);
        if (username != "" && password != "")
        {
            loadinganim.SetActive(true);
            username_input.text = "";
            password_input.text = "";
           

            //----------------------api integration -------------//
            string loginlink = Mainurl + login_API;
            var Loginlog = new LoginModel
            {
                IMEI = "",
                USERID = username,
                PASSWORD = encryptedpassword1,
                OS = "",
                Network = "",
                OSVersion = "",
                Details = "",
                REURL = ""
            };

            string LoginLogdata = Newtonsoft.Json.JsonConvert.SerializeObject(Loginlog);
            Debug.Log("Login data " + LoginLogdata);


            using (UnityWebRequest Request = UnityWebRequest.Put(loginlink, LoginLogdata))
            {
                Request.method = UnityWebRequest.kHttpVerbPOST;
                Request.SetRequestHeader("Content-Type", "application/json");
                Request.SetRequestHeader("Accept", "application/json");
                yield return Request.SendWebRequest();
                if (!Request.isNetworkError && !Request.isHttpError)
                {
                    Debug.Log(Request.downloadHandler.text);
                    if (Request.downloadHandler.text != null)
                    {
                        UserAuthenticationModel UserdataLog = Newtonsoft.Json.JsonConvert.DeserializeObject<UserAuthenticationModel>(Request.downloadHandler.text);
                        if (UserdataLog.AuthStatus.Equals("success", System.StringComparison.OrdinalIgnoreCase))
                        {
                            if (remeberme.isOn == true)
                            {
                                PlayerPrefs.SetString("logged", "true");
                            }
                            remeberme.isOn = false;
                            if (UserdataLog.Id_Role == 181)     // LOGIN AS USER
                            {
                                PlayerPrefs.SetInt("UID", UserdataLog.IDUSER);
                                PlayerPrefs.SetInt("OID", UserdataLog.OID);
                                PlayerPrefs.SetInt("game_id", UserdataLog.id_org_game_unit);
                                PlayerPrefs.SetString("gender", Convert.ToString(UserdataLog.GENDER != null ? UserdataLog.GENDER : "m"));
                                PlayerPrefs.SetString("username", Convert.ToString(UserdataLog.FIRST_NAME != null ? UserdataLog.FIRST_NAME : "--"));
                                PlayerPrefs.SetString("User_grade", UserdataLog.UserGrade != null ? UserdataLog.UserGrade.ToString() : "0");
                                PlayerPrefs.SetInt("id_school", UserdataLog.id_school != null ? UserdataLog.id_school : 0);
                                PlayerPrefs.SetInt("characterType", UserdataLog.avatar_type != null ? UserdataLog.avatar_type : 0);
                                PlayerPrefs.SetInt("PlayerBody", UserdataLog.body_type != null ? UserdataLog.body_type : 0);
                                PlayerPrefs.SetString("profile_done", UserdataLog.avatar_type >= 0 ? "done" : "false");
                                PlayerPrefs.SetString("Role", "User");
                                PlayerPrefs.SetString("Email", UserdataLog.Mail != null || UserdataLog.Mail != "" ? UserdataLog.Mail : "");
                                settingpanelbtn.gameObject.SetActive(true);
                                username_leftdashboard.text = PlayerPrefs.GetString("username");
                                username_input.GetComponent<InputField>().enabled = true;
                                password_input.GetComponent<InputField>().enabled = true;
                                loginbutton.GetComponent<Button>().enabled = true;
                                homebutton.SetActive(false);
                                loadinganim.SetActive(false);
                                string msg = "Logged In Successfully!";
                                StartCoroutine(Messagedisplay(msg));
                                yield return new WaitForSeconds(3.6f);
                                loginpage.SetActive(false);
                                videoPlayed = true;
                                YoutubeVideopage.SetActive(true);
                                TriviaPage.SetActive(true);
                                Camera.main.gameObject.GetComponent<AudioSource>().enabled = false;
                                StartCoroutine(GetBonusScoreTask());
                                StartCoroutine(LevelpercentageScore());
                                StartCoroutine(getScoreConfigdata());
                                StartCoroutine(CheckStage2Unlock());

                            }else if(UserdataLog.Id_Role == 180)    // LOGIN AS TEACHER
                            {
                                PlayerPrefs.SetInt("UID", UserdataLog.IDUSER);
                                PlayerPrefs.SetInt("OID", UserdataLog.OID);
                                PlayerPrefs.SetInt("game_id", UserdataLog.id_org_game_unit);
                                PlayerPrefs.SetString("teachername", Convert.ToString(UserdataLog.FIRST_NAME != null ? UserdataLog.FIRST_NAME : "--"));
                                PlayerPrefs.SetString("teacher_grade", UserdataLog.UserGrade != null ? UserdataLog.UserGrade.ToString() : "0");
                                PlayerPrefs.SetInt("teacherschool", UserdataLog.id_school != null ? UserdataLog.id_school : 0);
                                PlayerPrefs.SetString("Role", "Teacher");
                                PlayerPrefs.SetString("gender", Convert.ToString(UserdataLog.GENDER != null ? UserdataLog.GENDER : "m"));
                                PlayerPrefs.SetString("Email", UserdataLog.Mail != null || UserdataLog.Mail != "" ? UserdataLog.Mail : "");
                                username_input.GetComponent<InputField>().enabled = true;
                                password_input.GetComponent<InputField>().enabled = true;
                                loginbutton.GetComponent<Button>().enabled = true;
                                homebutton.SetActive(false);
                                loadinganim.SetActive(false);
                                string msg = "Logged In Successfully!";
                                StartCoroutine(Messagedisplay(msg));
                                yield return new WaitForSeconds(3.6f);
                                SceneManager.LoadScene(5);

                            }
                            else if(UserdataLog.Id_Role == 182)     // LOGIN AS PARENT
                            {
                                PlayerPrefs.SetInt("UID", UserdataLog.IDUSER);
                                PlayerPrefs.SetInt("OID", UserdataLog.OID);
                                PlayerPrefs.SetInt("game_id", UserdataLog.id_org_game_unit);
                                PlayerPrefs.SetString("parentname", Convert.ToString(UserdataLog.FIRST_NAME != null ? UserdataLog.FIRST_NAME : "--"));
                                PlayerPrefs.SetString("Role", "Parent");
                                PlayerPrefs.SetString("gender", Convert.ToString(UserdataLog.GENDER != null ? UserdataLog.GENDER : "m"));
                                PlayerPrefs.SetString("Email", UserdataLog.Mail != null || UserdataLog.Mail != "" ? UserdataLog.Mail : "");
                                username_input.GetComponent<InputField>().enabled = true;
                                password_input.GetComponent<InputField>().enabled = true;
                                loginbutton.GetComponent<Button>().enabled = true;
                                homebutton.SetActive(false);
                                loadinganim.SetActive(false);
                                string msg = "Logged In Successfully!";
                                StartCoroutine(Messagedisplay(msg));
                                yield return new WaitForSeconds(3.6f);
                                SceneManager.LoadScene(4);
                            }
                        

                        }
                        else
                        {
                            username_input.GetComponent<InputField>().enabled = true;
                            password_input.GetComponent<InputField>().enabled = true;
                            loginbutton.GetComponent<Button>().enabled = true;
                            loadinganim.SetActive(false);
                            string msg = "Login In Failed!!";
                            StartCoroutine(Messagedisplay(msg));
                            yield return new WaitForSeconds(3.5f);
                            UpdatedLoginPage.SetActive(true);
                        }
                    }
                }
                else
                {
                    Debug.Log(Request.downloadHandler.text);
                    username_input.GetComponent<InputField>().enabled = true;
                    password_input.GetComponent<InputField>().enabled = true;
                    loginbutton.GetComponent<Button>().enabled = true;
                    loadinganim.SetActive(false);
                    string msg = "Check your internet Connection !!!";
                    StartCoroutine(Messagedisplay(msg));
                    yield return new WaitForSeconds(3.5f);
                    UpdatedLoginPage.SetActive(true);
                }

            }
        }
    }

    public void SkipVideoTask()
    {
        videoPlayed = false;
        StartCoroutine(CoroutineskipVideo());
    }
    IEnumerator CoroutineskipVideo()
    {
        skipVideo.SetActive(false);
        YoutubeVideopage.SetActive(false);
        TriviaPage.SetActive(false);
        if (remeberme.isOn == true)
        {
            PlayerPrefs.SetString("logged", "true");
        }
        remeberme.isOn = false;
        StartCoroutine(scenechanges(HomepageObject, toplayer_sprite));
        yield return new WaitForSeconds(1.2f);
        superhero.SetActive(true);
        Camera.main.gameObject.GetComponent<AudioSource>().enabled = true;
    }


    IEnumerator Enablepage(GameObject enableobject, GameObject disableobject, float time)
    {
        yield return new WaitForSeconds(time);
        enableobject.SetActive(true);
        disableobject.SetActive(false);

    }

    public IEnumerator Messagedisplay(string msg)
    {
        yield return new WaitForSeconds(0.1f);
        statusMsgpanel.SetActive(true);
        statusMsgpanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = msg;
        iTween.ScaleTo(statusMsgpanel, Vector3.one, 0.8f);
        yield return new WaitForSeconds(2f);
        iTween.ScaleTo(statusMsgpanel, Vector3.zero, 0.8f);
        yield return new WaitForSeconds(1f);
        statusMsgpanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        statusMsgpanel.SetActive(false);
    }

    //------------------------avatar seletion--------------//

    public void boyselection()
    {
        charactertype = "boy";
        StartCoroutine(Enablepage(userdetailspage, characterpage, 1f));
        characterimage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 300);
        characterimage.sprite = boysprite;

    }
    public void girlselection()
    {
        charactertype = "girl";
        StartCoroutine(Enablepage(userdetailspage, characterpage, 1f));
        characterimage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 330);
        characterimage.sprite = girlsprite;
    }

    public void submit()
    {
        string profilename = profile_name.text;
        string grade_value = grade.options[grade.value].text;
        if (charactertype == "boy")
        {
            PlayerPrefs.SetInt("characterType", 1);
            profileimage.GetComponent<Image>().sprite = boyface;
        }
        else
        {
            PlayerPrefs.SetInt("characterType", 0);
            profileimage.GetComponent<Image>().sprite = girlface;
        }
        profilesetup_page.SetActive(false);
        narration_activity.AfterAvatar_task();
    }

    public void backpage()
    {
        StartCoroutine(Enablepage(characterpage, userdetailspage, 1f));
    }

   
    public void VibrateDevice()
    {
        if (!PlayerPrefs.HasKey("VibrationEnable"))
        {
            Vibration.Vibrate(400);
            Debug.Log("vibration");
        }
        else
        {
            string vibration = PlayerPrefs.GetString("VibrationEnable");

            if (vibration == "true")
            {
                Vibration.Vibrate(400);
                Debug.Log("vibration");
            }
        }
    }

    IEnumerator GetBonusScoreTask()
    {
        string HittingUrl = $"{Mainurl}{GetBonusScoreApi}";
        WWW Bonuswww = new WWW(HittingUrl);
        yield return Bonuswww;
        if(Bonuswww.text != null)
        {
            if(Bonuswww.text != "[]")
            {
                List<BonusScoreModel> Bonusmodel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BonusScoreModel>>(Bonuswww.text);
                Bonusmodel.ForEach(x =>
                {
                    var Scorelog = dbmanager.Table<BonusTable>().FirstOrDefault(y => y.RoomId == x.id_room);
                    if(Scorelog == null)
                    {
                        BonusTable table = new BonusTable
                        {
                            RoomId = x.id_room,
                            Time0to30 = x.Time0to30,
                            BonusPoint1 = x.BonusPoint1,
                            Time30to45 = x.Time30to45,
                            BonusPoint2 = x.BonusPoint2
                        };
                        dbmanager.Insert(table);
                    }
                    else
                    {
                        Scorelog.RoomId = x.id_room;
                        Scorelog.Time0to30 = x.Time0to30;
                        Scorelog.BonusPoint1 = x.BonusPoint1;
                        Scorelog.Time30to45 = x.Time30to45;
                        Scorelog.BonusPoint2 = x.BonusPoint2;
                        dbmanager.UpdateTable(Scorelog);
                    }
                });
               
            }
        }
    }

    IEnumerator LevelpercentageScore()
    {
        string HittingUrl = $"{Mainurl}{levelMovementApi}?id_org_game=1";
        WWW Logwww = new WWW(HittingUrl);
        yield return Logwww;
        if(Logwww.text != null)
        {
            if(Logwww.text != "[]")
            {
                List<LevelMovement> LevelLog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LevelMovement>>(Logwww.text);
                LevelLog.ForEach(x =>
                {
                    var PercentageLog = dbmanager.Table<LevelPercentageTable>().FirstOrDefault(y => y.LevelId == x.id_level);
                    if(PercentageLog == null)
                    {
                        LevelPercentageTable log = new LevelPercentageTable
                        {
                            LevelId = x.id_level,
                            LevelPercentage = x.completion_score
                        };
                        dbmanager.Insert(log);
                    }
                    else
                    {
                       PercentageLog.LevelId = x.id_level;
                        PercentageLog.LevelPercentage = x.completion_score;
                        dbmanager.UpdateTable(PercentageLog);
                    }
                });
            }
        }
    }


    IEnumerator getScoreConfigdata()
    {
        yield return new WaitForSeconds(0f);
        var ScoreLog = dbmanager.Table<ScoreConfiguration>().ToList();
        if(ScoreLog.Count >0)
        {
            ScoreLog.ForEach(x =>
            {
                if (x.levelId == 1)
                {
                    StageunlockScores.Stage1Score = x.UnlockScore;
                    Stage1Score = x.UnlockScore;
                }
                if (x.levelId == 2)
                {
                    StageunlockScores.Stage2Score = x.UnlockScore;
                    Stage2Score = x.UnlockScore;
                }
            });
        }
    }

    IEnumerator CheckStage2Unlock()
    {
        string Hitting_url = $"{Mainurl}{StageUnlockApi}?UID={PlayerPrefs.GetInt("UID")}&id_level={1}&id_org_game={1}";
        WWW StageData = new WWW(Hitting_url);
        yield return StageData;
        if (StageData.text != null)
        {
            if(StageData.text != "[]")
            {
                StageUnlockModel StageModel = Newtonsoft.Json.JsonConvert.DeserializeObject<StageUnlockModel>(StageData.text);
                var tableLog = dbmanager.Table<StageClearness>().FirstOrDefault(x => x.LevelId == 1);
                int clear = int.Parse(StageModel.ConsolidatedScore) > Stage1Score ? 1 : 0;
                if (tableLog == null)
                {
                    StageClearness log = new StageClearness
                    {
                        LevelId = 1,
                        IsClear = clear
                    };
                    dbmanager.Insert(log);
                }
                else
                {
                    tableLog.LevelId = 1;
                    tableLog.IsClear = clear;
                    dbmanager.UpdateTable(tableLog);

                }
            }
        }
        StartCoroutine(checkstage3Unlock());
    }

    IEnumerator checkstage3Unlock()
    {
        string Hitting_url = $"{Mainurl}{StageUnlockApi}?UID={PlayerPrefs.GetInt("UID")}&id_level={2}&id_org_game={1}";
        WWW StageData = new WWW(Hitting_url);
        yield return StageData;
        if (StageData.text != null)
        {
            Debug.Log(StageData.text);
            StageUnlockModel StageModel = Newtonsoft.Json.JsonConvert.DeserializeObject<StageUnlockModel>(StageData.text);
            var tableLog = dbmanager.Table<StageClearness>().FirstOrDefault(x => x.LevelId == 2);
            int clear = int.Parse(StageModel.ConsolidatedScore) > Stage2Score ? 1 : 0;
            if (tableLog == null)
            {
                StageClearness log = new StageClearness
                {
                    LevelId = 2,
                    IsClear = clear
                };
                dbmanager.Insert(log);
            }
            else
            {
                tableLog.LevelId = 2;
                tableLog.IsClear = clear;
                dbmanager.UpdateTable(tableLog);
            }
        }
    }
}
