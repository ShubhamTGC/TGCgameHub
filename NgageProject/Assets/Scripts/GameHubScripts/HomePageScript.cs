using m2ostnextservice.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SimpleSQL;
using System.Linq;
using System.Security.Cryptography;

public class HomePageScript : MonoBehaviour
{
    public static HomePageScript Homepage;
    // Start is called before the first frame update
    public GameObject startpage, RegistrationPage,LoginPage,ProfileSetuppage,GameSelectionpage;
    public AESalgorithm aeslog;
    public GameObject PopupPage;
    public Text Msgbox;
    public Image MoodImage;
    public Sprite happy, sad;
    [Header("LOGIN ELEMENTS=====")]
    [Space(15)]
    public InputField Emailid;
    public InputField Password;
    public Toggle rememberMe;
    private Dictionary<int, string> AvatarCmsdata = new Dictionary<int, string>();
    [SerializeField]private List<string> Urls = new List<string>();
    [SerializeField] private List<int> Ids = new List<int>();
    public GameObject StartingPage, landingpage;
    public Sprite Closeeye, Openeye;
    public Button Hidepasswordbtn;
    public Text GRegistration;
    public SimpleSQLManager dbmanager;
    public GameObject Loadingpage;
    public Image LoadingBar;
    private AESAlgorithm aes = new AESAlgorithm();
    [HideInInspector]public bool DownloadDone;
    [SerializeField]private List<string> AnagramImageUrl = new List<string>();
    [SerializeField] private List<int> Anagrameimageid = new List<int>();
    private string AvatarDir = "AvatarFile";
    private string AnagramDir = "AnagramFiles";
    private string MatchTheTileFlagFolder = "MatchTheTileGameFlag";
    private string MatchTheTileLoactionFolder = "MatchTheTileGameLocation";
    private string RoundimagePreviewFolder = "RoundPreview";
    private string RectImagepreviewFolder = "RectPreview";
    private List<string> FlagUrl = new List<string>(), LocationUrl = new List<string>();
    private List<int> LocationId = new List<int>(), FlagID = new List<int>();
    private List<int> RectImageId = new List<int>(), RoundimageId = new List<int>();
    private List<string> RectImageUrl = new List<string>(), RoundImageUrl = new List<string>();

    public GameObject LogOutpage;
    public GameObject AppupdatePage;
    private bool GoogleLogin;

    public void Awake()
    {
        GetSounddata();
    
    }

    void GetSounddata()
    {
        var GameSetting = dbmanager.Table<SettingPage>().FirstOrDefault();
        AudioSource audioSource = Camera.main.gameObject.GetComponent<AudioSource>();
        audioSource.volume = GameSetting != null ? GameSetting.Music : 1;
   
        
    }

    IEnumerator GetAppVersion()
    {
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.AppVersionApi}";
        WWW versionlog = new WWW(HittingUrl);
        yield return versionlog;
        if(versionlog.text != null)
        {
            if(versionlog.text != "")
            {
                string InitialLog = versionlog.text.TrimStart('"').TrimEnd('"');
                AESAlgorithm aes = new AESAlgorithm();
                string decryptedLog = aes.getDecryptedString(InitialLog);
                Debug.Log("version log " + decryptedLog);
                List<AppVersionModel> Log = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AppVersionModel>>(decryptedLog);
                Log.ForEach(x =>
                {
                    if (x.Application_Type.Equals("android", System.StringComparison.OrdinalIgnoreCase))
                    {
                        if(Application.version != x.version_number)
                        {
                            AppupdatePage.SetActive(true);
                        }
                        else
                        {
                            AppupdatePage.SetActive(false);
                        }
                    }
                    if (x.Application_Type.Equals("ios", System.StringComparison.OrdinalIgnoreCase))
                    {
                        if (Application.version != x.version_number)
                        {
                            AppupdatePage.SetActive(true);
                        }
                        else
                        {
                            AppupdatePage.SetActive(false);
                        }
                    }
                });
                LoginCheckTask();
            }
        }
    }

    public void UpdateApp()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            Application.OpenURL("market://details?q=pname:com.TGC.GameHub/");
        }
        else
        {
            Application.OpenURL("itms-apps://itunes.apple.com/app/com.TGC.GameHub");
        }
    }

    IEnumerator GetuserAvatar()
    {
        
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.GetavatarPicApi}";
        WWW AvatarLog = new WWW(HittingUrl);
        yield return AvatarLog;
        if (AvatarLog.text != null)
        {
            string log = AvatarLog.text.TrimStart('"').TrimEnd('"');
            
            string Datalog = aes.getDecryptedString(log);
            List<AvatarLogModel> LogModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AvatarLogModel>>(Datalog);
            LogModel.ForEach(x =>
            {
                Urls.Add(x.url);
                Ids.Add(x.Id_Avatar);

            });
            if (GoogleLogin)
            {
                StartCoroutine(GoogleLoginTask());
            }
            else
            {
                StartCoroutine(AfterLoginTask());
            }
            
        }
    }

    IEnumerator GetTexture(int id, string Url,string dirname)
    {
        Debug.Log("running");
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(Url, true);
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
                        var dirpath = UnityEngine.Application.persistentDataPath + "/"+ dirname;
                        if (!Directory.Exists(dirpath))
                        {
                            Directory.CreateDirectory(dirpath);
                            if (Directory.Exists(dirpath))
                            {
                                dirpath = dirpath + "/" + id.ToString() + ".png";
                                byte[] bytes = texture2d.EncodeToPNG();
                                File.WriteAllBytes(dirpath,bytes);
                                Debug.Log("File saved " + dirpath);
                            }
                        }
                        else
                        {
                            dirpath = dirpath + "/" + id.ToString() + ".png";
                            byte[] bytes = texture2d.EncodeToPNG();
                            File.WriteAllBytes(dirpath, bytes);
                            Debug.Log("File saved " + dirpath);
                        }
                    }
                }
                else
                {
                    Debug.Log("running");
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }


    void Start()
    {
         
    }
     void OnEnable()
    {
        GoogleLogin = false;
        if (Homepage != null)
        {
            Destroy(Homepage);
            LoginPage.SetActive(false);
            startpage.SetActive(false);
            GameSelectionpage.SetActive(true);
            Homepage = this;
        }
        else
        {
            //StartCoroutine(GetAppVersion());
            LoginCheckTask();
            Homepage = this;
            
        }
        DontDestroyOnLoad(Homepage);
        StartCoroutine(GetPercentiledata());
    }

    IEnumerator GetPercentiledata()
    {
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.GetPercentileApi}?OrgID={PlayerPrefs.GetInt("OID")}";
        using (UnityWebRequest Request = UnityWebRequest.Get(HittingUrl))
        {
            Request.method = UnityWebRequest.kHttpVerbGET;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                if (Request.responseCode.Equals(200))
                {
                    if (Request.downloadHandler.text != null)
                    {
                        string response = Request.downloadHandler.text.TrimStart('"').TrimEnd('"');
                        AESAlgorithm aes = new AESAlgorithm();
                        string encrypted = aes.getDecryptedString(response);
                        List<GetPercentileLog> PercentileLog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetPercentileLog>>(encrypted);
                        PercentileLog.ForEach(x =>
                        {
                            var LocalLog = dbmanager.Table<PercentileTable>().FirstOrDefault(a => a.PercentileId == x.id_percentage);
                            if (LocalLog == null)
                            {
                                PercentileTable tablleLog = new PercentileTable
                                {
                                    PercentileId = x.id_percentage,
                                    Percentile = x.percentage
                                };
                                dbmanager.Insert(tablleLog);
                            }
                            else
                            {
                                LocalLog.PercentileId = x.id_percentage;
                                LocalLog.Percentile = x.percentage;
                                dbmanager.UpdateTable(LocalLog);
                            }
                        });
                    }
                }
            }
        }
    }

    void LoginCheckTask()
    {
      
        if (!PlayerPrefs.HasKey("Loggedin"))
        {
            StartingPage.SetActive(true);
            landingpage.SetActive(false);   
        }
        else
        {
            var AvatarLog = dbmanager.Table<ProfileSetup>().FirstOrDefault();
            if (AvatarLog.AvatarId == 0)
            {
                StartingPage.SetActive(false);
                LoginPage.SetActive(false);
                ProfileSetuppage.SetActive(true);
            }
            else
            {
                StartingPage.SetActive(false);
                LoginPage.SetActive(false);
                GameSelectionpage.SetActive(true);
            }
        }
        
    
    }

    public void Showpassword()
    {
        if (Hidepasswordbtn.image.sprite.name.Equals("closeeye", System.StringComparison.OrdinalIgnoreCase))
        {
            Hidepasswordbtn.image.sprite = Openeye;
            Password.inputType = InputField.InputType.Standard;
            Password.ForceLabelUpdate();
        }
        else
        {
            Hidepasswordbtn.image.sprite = Closeeye;
            Password.inputType = InputField.InputType.Password;
            Password.ForceLabelUpdate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InitiateGame(int SceneNo)
    {
        StartCoroutine(OpenSelectedScene(SceneNo));
       
    }

    IEnumerator OpenSelectedScene(int Scene)
    {
        iTween.ScaleTo(GameSelectionpage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        GameSelectionpage.SetActive(false);
        SceneManager.LoadScene(Scene);
    }



    public void regularRegistration()
    {
        
        RegistrationPage.SetActive(true);
    }

 IEnumerator ShowPopUp(string msg,Sprite mood)
    {
        PopupPage.SetActive(true);
        MoodImage.sprite = mood;
        Msgbox.text = msg;
        yield return new WaitForSeconds(2.5f);
        iTween.ScaleTo(PopupPage, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.35f);
        Msgbox.text = "";
        PopupPage.SetActive(false);
    }
   
    public void LoginTask()
    {
        if(Emailid.text != "" && Password.text != "")
        {
            StartCoroutine(checkLogin());
        }
        else
        {
            string msg = "Please enter your datails!";
            StartCoroutine(ShowPopUp(msg,sad));
        }
    }
    IEnumerator checkLogin()
    {
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.LoginApi}";
        loginModel loginlog = new loginModel
        {
            Name = "",
            Email = Emailid.text,
            Password = aeslog.getEncryptedString(Password.text),
            login_type = 1
        };

        string dataLog = Newtonsoft.Json.JsonConvert.SerializeObject(loginlog);
        AESAlgorithm Aes = new AESAlgorithm();
        string Encryptedlog = Aes.getEncryptedString(dataLog);

        CommonModel postlog = new CommonModel
        {
            Data = Encryptedlog
        };

        string datalog = Newtonsoft.Json.JsonConvert.SerializeObject(postlog);

        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, datalog))
        {
            Request.method = UnityWebRequest.kHttpVerbPOST;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                if (Request.responseCode.Equals(200))
                {
                    if (Request.downloadHandler.text != null)
                    {
                        if (Request.downloadHandler.text != "")
                        {
                            var log = Request.downloadHandler.text.TrimStart('"').TrimEnd('"');
                            string response = Aes.getDecryptedString(log);
                            Debug.Log("Login response " + response);
                            LoginResModel LoginLog = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResModel>(response);
                            PlayerPrefs.SetInt("UID", LoginLog.Id_User);
                            PlayerPrefs.SetInt("OID", LoginLog.ID_ORGANIZATION);
                            string msg = "Logged in Successfully!!!";
                            StartCoroutine(ShowPopUp(msg,happy));
                            int avatarid =Convert.ToInt32(LoginLog.Id_Avatar != null ? LoginLog.Id_Avatar : 0);
                            if(rememberMe.isOn == true)
                            {
                                PlayerPrefs.SetString("Loggedin", "true");
                            }
                           
                            var localLog = dbmanager.Table<ProfileSetup>().FirstOrDefault();
                            if (localLog == null)
                            {
                                ProfileSetup profilelog = new ProfileSetup
                                {
                                    UserId = LoginLog.Id_User,
                                    Oid = LoginLog.ID_ORGANIZATION,
                                    Username = LoginLog.Name,
                                    EmailId = LoginLog.Email,
                                    Mobileno = LoginLog.Phone_No,
                                    Orgname = LoginLog.Organization_Name,
                                    LoginType = 1,
                                    AvatarId = avatarid
                                };
                                dbmanager.Insert(profilelog);
                            }
                            else
                            {
                                localLog.UserId = LoginLog.Id_User;
                                localLog.Oid = LoginLog.ID_ORGANIZATION;
                                localLog.Username = LoginLog.Name;
                                localLog.EmailId = LoginLog.Email;
                                localLog.Mobileno = LoginLog.Phone_No;
                                localLog.Orgname = LoginLog.Organization_Name;
                                localLog.LoginType = 1;
                                localLog.AvatarId = avatarid;
                                dbmanager.UpdateTable(localLog);

                            }
                            
                            yield return new WaitForSeconds(3.5f);
                            DownloadDone = false;
                            Loadingpage.SetActive(true);
                            StartCoroutine(GetGameDetailsProcess());
                            
                            
                        }
                    }
                }
                else
                {
                    Debug.Log("request error " + Request.downloadHandler.text);
                    string msg = "Invaild email id or password!";
                    StartCoroutine(ShowPopUp(msg,sad));
                }
            }
            else
            {
                Debug.Log("request error " + Request.downloadHandler.text);
                string msg = "Invaild email id or password!";
                StartCoroutine(ShowPopUp(msg,sad));
            }
        }
    }



    public void ExitApp()
    {
       Application.Quit();
    }


    //====COMMEN METHOD FOR GOOGLE AND FACEBOOK REGISTRATION =======//


    public void Googleregister(string name, string emailid)
    {
        StartCoroutine(GooglecheckLogin(name, emailid));
    }



    //Login with google and login method
    IEnumerator GooglecheckLogin(string name,string emailid)
    {
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.LoginApi}";
        loginModel loginlog = new loginModel
        {
            Name = name,
            Email = emailid,
            Password = "",
            login_type =2
        };

        //User details log collection
        string dataLog = Newtonsoft.Json.JsonConvert.SerializeObject(loginlog);
        AESAlgorithm Aes = new AESAlgorithm();

        //Encrypted data of the user details log
        string Encryptedlog = Aes.getEncryptedString(dataLog);

        CommonModel postlog = new CommonModel
        {
            Data = Encryptedlog
        };

        //Final log for posting with encrypted data object
        string datalog = Newtonsoft.Json.JsonConvert.SerializeObject(postlog);

        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, datalog))
        {
            Request.method = UnityWebRequest.kHttpVerbPOST;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                if (Request.responseCode.Equals(200))
                {
                    if (Request.downloadHandler.text != null)
                    {
                        if (Request.downloadHandler.text != "")
                        {
                            var log = Request.downloadHandler.text.TrimStart('"').TrimEnd('"');
                            //Converting Encrypted data to decrypted
                            string response = Aes.getDecryptedString(log);

                            //Json model for the decrypted response
                            LoginResModel LoginLog = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResModel>(response);
                            PlayerPrefs.SetInt("UID", LoginLog.Id_User);
                            PlayerPrefs.SetInt("OID", LoginLog.ID_ORGANIZATION);
                            int avatarid = Convert.ToInt32(LoginLog.Id_Avatar != null ? LoginLog.Id_Avatar : 0);
                            PlayerPrefs.SetString("Loggedin", "true");
                            string msg = "Logged in Successfully!!!";
                            GoogleLogin = true;
                            StartCoroutine(ShowPopUp(msg,happy));
                            var localLog = dbmanager.Table<ProfileSetup>().FirstOrDefault();
                            if(localLog == null)
                            {
                                ProfileSetup profilelog = new ProfileSetup
                                {
                                    UserId = LoginLog.Id_User,
                                    Oid = LoginLog.ID_ORGANIZATION,
                                    Username = LoginLog.Name,
                                    EmailId = LoginLog.Email,
                                    Mobileno = LoginLog.Phone_No,
                                    Orgname = LoginLog.Organization_Name,
                                    LoginType = 2,
                                    AvatarId = avatarid
                                };
                                dbmanager.Insert(profilelog);
                            }
                            else
                            {
                                localLog.UserId = LoginLog.Id_User;
                                localLog.Oid = LoginLog.ID_ORGANIZATION;
                                localLog.Username = LoginLog.Name;
                                localLog.EmailId = LoginLog.Email;
                                localLog.Mobileno = LoginLog.Phone_No;
                                localLog.Orgname = LoginLog.Organization_Name;
                                localLog.LoginType = 2;
                                localLog.AvatarId = avatarid;
                                dbmanager.UpdateTable(localLog);

                            }

                            yield return new WaitForSeconds(3.5f);
                            StartCoroutine(GetGameDetailsProcess());
                            
                            
                        }
                    }
                }
                else
                {
                  
                    string msg = "Something went wrong please try later!!!";
                    StartCoroutine(ShowPopUp(msg, sad));
                }
            }
            else
            {
           
                string msg = "Please check internet connection!!!";
                StartCoroutine(ShowPopUp(msg, sad));
            }
        }
    }

    IEnumerator AfterLoginTask()
    {
        var AvatarLog = dbmanager.Table<ProfileSetup>().FirstOrDefault();
        yield return new WaitForSeconds(0.2f);
        if(AvatarLog.AvatarId == 0)
        {
            LoginPage.SetActive(false);
            ProfileSetuppage.SetActive(true);
            DownloadDone = true;
        }
        else
        {
            LoginPage.SetActive(false);
            GameSelectionpage.SetActive(true);
            DownloadDone = true;
        }

    }

    IEnumerator GoogleLoginTask()
    {
        var AvatarLog = dbmanager.Table<ProfileSetup>().FirstOrDefault();
        yield return new WaitForSeconds(0.2f);
        if (AvatarLog.AvatarId == 0)
        {
            startpage.SetActive(false);
            RegistrationPage.SetActive(false);
            LoginPage.SetActive(false);
            ProfileSetuppage.SetActive(true);
            DownloadDone = true;
        }
        else
        {
            startpage.SetActive(false);
            RegistrationPage.SetActive(false);
            LoginPage.SetActive(false);
            GameSelectionpage.SetActive(true);
            DownloadDone = true;
        }
    }

    IEnumerator GetGameDetailsProcess()
    {
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.GamesetupApi}";
        WWW Request = new WWW(HittingUrl);
        yield return Request;
        if(Request.text != null)
        {
            if(Request.text != "")
            {
                string CompleteLog = Request.text.TrimStart('"').TrimEnd('"');
                string Log = aes.getDecryptedString(CompleteLog);
                Debug.Log("Game setup data " + Log);
                GameSetupModel Gamelog = Newtonsoft.Json.JsonConvert.DeserializeObject<GameSetupModel>(Log);
                List<Gamelist> gameconfig = Gamelog.gamelist;
                List<Anagramlist> anagramconfig = Gamelog.anagramlist;
                List<Quizelist> Quizconfig = Gamelog.quizelist;
                List<Objectgamilist> ObjectgameConfig = Gamelog.objectgamilist;
                List<Monstercatchlist> MonsterConfig = Gamelog.monstercatchlist;
                List<Truckgamelist> TruckConfig = Gamelog.truckgamelist;
                List<Truckscoretypelist> TruckScoreConfig = Gamelog.truckscoretypelist;
                List<Truckdestinationdrivinglist> TruckcenterConfig = Gamelog.truckdestinationdrivinglist;
                List<matchthetiletypelist> MatchTheTileLocation = Gamelog.matchthetiletypelist;
                List<matchthetilelist> MatchTheTileFlag = Gamelog.matchthetilelist;


                //Debug.Log(" gameconfig " + gameconfig.Count + " anagramconfig " + anagramconfig.Count + " Quizconfig " + Quizconfig.Count + " ObjectgameConfig : "  + ObjectgameConfig.Count + " MonsterConfig : " + MonsterConfig.Count + " TruckConfig :  " + TruckConfig.Count
                //    + " TruckcenterConfig:  " + TruckcenterConfig.Count);
                string RoundlocalPath = Application.persistentDataPath + "/" + RoundimagePreviewFolder;
                string Rectlocalpath = Application.persistentDataPath + "/" + RectImagepreviewFolder;
                gameconfig.ForEach(x =>
                {
                    var LocalgameConfig = dbmanager.Table<GameListDetails>().FirstOrDefault(y=> y.GameId == x.Id_Game);
                    if (LocalgameConfig == null)
                    {
                        GameListDetails Gamedetails = new GameListDetails
                        {
                            GameId = x.Id_Game,
                            Oid = x.ID_ORGANIZATION,
                            GameName = x.GameName,
                            GameType = x.GameType,
                            UpdateFlag = x.UpdatedFlag,
                            CompletePer = int.Parse(x.completePer),
                            RoundImageUrl = RoundlocalPath + "/" +  x.Id_Game + ".png",
                            RectImageUrl = Rectlocalpath + "/" + x.Id_Game + ".png",

                        };
                        dbmanager.Insert(Gamedetails);
                        if (x.Id_Game == 1)
                        {
                            // Drag and drop game data save and update
                            ObjectgameConfig.ForEach(a =>
                            {
                                var objectLog = dbmanager.Table<ObjectGameList>().FirstOrDefault(t => t.ItemId == a.item_Id);
                                if (objectLog == null)
                                {
                                    ObjectGameList ObjectNewLog = new ObjectGameList
                                    {
                                        GameId = a.Id_Game,
                                        RoomId = a.Id_Room,
                                        ItemId = a.item_Id,
                                        ItemName = a.item_Name,
                                        CorrectPoint = a.correct_point,
                                        WrongPoint = a.Wrong_point
                                    };
                                    dbmanager.Insert(ObjectNewLog);
                                }
                                else
                                {
                                    objectLog.GameId = a.Id_Game;
                                    objectLog.RoomId = a.Id_Room;
                                    objectLog.ItemId = a.item_Id;
                                    objectLog.ItemName = a.item_Name;
                                    objectLog.CorrectPoint = a.correct_point;
                                    objectLog.WrongPoint = a.Wrong_point;
                                    dbmanager.UpdateTable(objectLog);
                                }
                            });
                        }

                        if (x.Id_Game == 2 )
                        {
                            anagramconfig.ForEach(b =>
                            {
                                var anagramlog = dbmanager.Table<AnagramGameList>().FirstOrDefault(t=> t.IdWord == b.id_word);
                                if (anagramlog == null)
                                {
                                    AnagramGameList AGlog = new AnagramGameList
                                    {
                                        IdWord = b.id_word,
                                        GameId = b.Id_Game,
                                        Question = b.question,
                                        Answer = b.answer,
                                        ImageUrl = b.Path,
                                        CorrectPoint = b.correct_point,
                                        WrongPoint = b.Wrong_point,
                                        BackgroundImage = b.BackgrounImageURL
                                    };
                                    dbmanager.Insert(AGlog);
                                }
                                else
                                {
                                    anagramlog.IdWord = b.id_word;
                                    anagramlog.GameId = b.Id_Game;
                                    anagramlog.Question = b.question;
                                    anagramlog.Answer = b.answer;
                                    anagramlog.ImageUrl = b.Path;
                                    anagramlog.CorrectPoint = b.correct_point;
                                    anagramlog.WrongPoint = b.Wrong_point;
                                    anagramlog.BackgroundImage = b.BackgrounImageURL;
                                    dbmanager.UpdateTable(anagramlog);
                                }
                                AnagramImageUrl.Add(b.BackgrounImageURL);
                                Anagrameimageid.Add(b.id_word);
                            });
                           
                        }

                        if (x.Id_Game == 3)
                        {
                            ObjectgameConfig.ForEach(c =>
                            {
                                var ObjectLog = dbmanager.Table<ObjectGameList>().FirstOrDefault(e => e.ItemId == c.item_Id);
                                if (ObjectLog == null)
                                {
                                    ObjectGameList objectddata = new ObjectGameList
                                    {
                                        GameId = c.Id_Game,
                                        RoomId = c.Id_Room,
                                        ItemId = c.item_Id,
                                        ItemName = c.item_Name,
                                        CorrectPoint = c.correct_point,
                                        WrongPoint = c.Wrong_point
                                    };
                                    dbmanager.Insert(objectddata);
                                }
                                else
                                {
                                    ObjectLog.GameId = c.Id_Game;
                                    ObjectLog.RoomId = c.Id_Room;
                                    ObjectLog.ItemId = c.item_Id;
                                    ObjectLog.ItemName = c.item_Name;
                                    ObjectLog.CorrectPoint = c.correct_point;
                                    ObjectLog.WrongPoint = c.Wrong_point;
                                    dbmanager.UpdateTable(ObjectLog);
                                }
                            });
                        }

                        if (x.Id_Game == 4 )
                        {
                            MatchTheTileLocation.ForEach(p =>
                            {
                                var TileLog = dbmanager.Table<MatchTheTileLocation>().FirstOrDefault(a => a.Type == p.type);
                                if (TileLog == null)
                                {
                                    MatchTheTileLocation LocationLog = new MatchTheTileLocation()
                                    {
                                        LocationTypeId = p.id_waste_type,
                                        Type = p.type,
                                        ImageUrl = p.ImageUrl,
                                        GameId = p.Id_Game
                                    };
                                    dbmanager.Insert(LocationLog);
                                }
                                else
                                {
                                    TileLog.LocationTypeId = p.id_waste_type;
                                    TileLog.Type = p.type;
                                    TileLog.ImageUrl = p.ImageUrl;
                                    TileLog.GameId = p.Id_Game;
                                    dbmanager.UpdateTable(TileLog);
                                }

                                LocationUrl.Add(p.ImageUrl);
                                LocationId.Add(p.id_waste_type);
                                
                            });

                            MatchTheTileFlag.ForEach(a =>
                            {
                                var MTTLog = dbmanager.Table<MatchTheTileFlag>().FirstOrDefault(u => u.IdFlag == a.id_waste);
                                if (MTTLog == null)
                                {
                                    MatchTheTileFlag flagLog = new MatchTheTileFlag()
                                    {
                                        IdFlag = a.id_waste,
                                        Flag = a.waste,
                                        LocationMatchId = a.waste_type,
                                        CorrectPoint = a.Score,
                                        WrongPoint = a.Wrong_point,
                                        GameId = a.Id_Game

                                    };
                                    dbmanager.Insert(flagLog);
                                }
                                else
                                {
                                    MTTLog.IdFlag = a.id_waste;
                                    MTTLog.Flag = a.waste;
                                    MTTLog.LocationMatchId = a.waste_type;
                                    MTTLog.CorrectPoint = a.Score;
                                    MTTLog.WrongPoint = a.Wrong_point;
                                    MTTLog.GameId = a.Id_Game;
                                    dbmanager.UpdateTable(MTTLog);
                                }
                                FlagUrl.Add(a.ImageUrl);
                                FlagID.Add(a.id_waste);
                            });

                        }

                        if (x.Id_Game == 5 )
                        {
                            MonsterConfig.ForEach(m =>
                            {
                                var monsterlog = dbmanager.Table<MonsterDetails>().FirstOrDefault(u=> u.MonsterId == m.monsterId);
                                if (monsterlog == null)
                                {
                                    MonsterDetails monsterdata = new MonsterDetails
                                    {
                                        MonsterId = m.monsterId,
                                        CatchPoint = m.catch_point,
                                        GameId = m.Id_Game
                                    };

                                    dbmanager.Insert(monsterdata);
                                }
                                else
                                {
                                    monsterlog.MonsterId = m.monsterId;
                                    monsterlog.CatchPoint = m.catch_point;
                                    monsterlog.GameId = m.Id_Game;
                                    dbmanager.UpdateTable(monsterlog);
                                }
                            });
                            TruckConfig.ForEach(v =>
                            {
                                var TruckGameLog = dbmanager.Table<TruckGameList>().FirstOrDefault(l => l.DustbinId == v.Dustbins_Map_Id);
                                if (TruckGameLog == null)
                                {
                                    TruckGameList TGlog = new TruckGameList
                                    {
                                        TruckId = v.id_truck,
                                        TruckName = v.truck_name,
                                        GameId = v.Id_Game,
                                        GameName = v.GameName,
                                        DustbinId = v.Dustbins_Map_Id,
                                        DustbinName = v.Dustbins_Name,
                                        CorrectPoint = v.Correct_Dustbin_Point,
                                        WrongPoint = v.Wrong_Dustbin_Point,
                                        ScoreId = v.ScoreId
                                    };
                                    dbmanager.Insert(TGlog);
                                }
                                else
                                {
                                    TruckGameLog.TruckId = v.id_truck;
                                    TruckGameLog.TruckName = v.truck_name;
                                    TruckGameLog.GameId = v.Id_Game;
                                    TruckGameLog.GameName = v.GameName;
                                    TruckGameLog.DustbinId = v.Dustbins_Map_Id;
                                    TruckGameLog.DustbinName = v.Dustbins_Name;
                                    TruckGameLog.CorrectPoint = v.Correct_Dustbin_Point;
                                    TruckGameLog.WrongPoint = v.Wrong_Dustbin_Point;
                                    TruckGameLog.ScoreId = v.ScoreId;
                                    dbmanager.UpdateTable(TruckGameLog);
                                }
                            });
                            TruckcenterConfig.ForEach(n =>
                            {
                                var TClog = dbmanager.Table<TruckCenterDetails>().FirstOrDefault(j => j.TruckId == n.id_truck);
                                if(TClog == null)
                                {
                                    TruckCenterDetails TCdata = new TruckCenterDetails
                                    {
                                        TruckId = n.id_truck,
                                        CenterName = n.destination_name,
                                        CorrectPoint = n.correct_bonus_point,
                                        WrongPoint = n.wrong_point,
                                        ScoreId = n.ScoreId
                                    };
                                    dbmanager.Insert(TCdata);
                                }
                                else
                                {
                                    TClog.TruckId = n.id_truck;
                                    TClog.CenterName = n.destination_name;
                                    TClog.CorrectPoint = n.correct_bonus_point;
                                    TClog.WrongPoint = n.wrong_point;
                                    TClog.ScoreId = n.ScoreId;
                                    dbmanager.UpdateTable(TClog);
                                }
                            });

                        }

                        if (x.Id_Game == 6 )
                        {
                            //QUIZ KBC GAME SAVE AND UPDATE
                            Quizconfig.ForEach(q =>
                            {
                                var Quizlog = dbmanager.Table<QuizGameList>().FirstOrDefault(d=> q.Id_Quiz == d.QuizId);
                                if (Quizlog == null)
                                {
                                    QuizGameList quizdataLog = new QuizGameList
                                    {
                                        GameId = q.Id_Game,
                                        CorrectPoint = q.Correct_point,
                                        WrongPoint = q.Wrong_point,
                                        Question = q.Question,
                                        CorrectOption = q.Correct_Options,
                                        Option1 = q.Correct_Options,
                                        Option2 = q.Options_1,
                                        Option3 = q.Options_2,
                                        Option4 = q.Options_3,
                                        QuizId = q.Id_Quiz

                                        
                                    };
                                    dbmanager.Insert(quizdataLog);
                                }
                                else
                                {
                                    Quizlog.GameId = q.Id_Game;
                                    Quizlog.CorrectPoint = q.Correct_point;
                                    Quizlog.WrongPoint = q.Wrong_point;
                                    Quizlog.Question = q.Question;
                                    Quizlog.CorrectOption = q.Correct_Options;
                                    Quizlog.Option1 = q.Correct_Options;
                                    Quizlog.Option2 = q.Options_1;
                                    Quizlog.Option3 = q.Options_2;
                                    Quizlog.Option4 = q.Options_3;
                                    dbmanager.UpdateTable(Quizlog);
                                }
                            });
                        }

                        RectImageId.Add(x.Id_Game);
                        RectImageUrl.Add(x.RectangleImgURL);
                        RoundimageId.Add(x.Id_Game);
                        RoundImageUrl.Add(x.RoundedImgURL);

                    }
                    else
                    {

                        Debug.Log("Updated tables");
                        if(x.Id_Game == 1 && LocalgameConfig.GameId ==1)
                        {
                            if(x.UpdatedFlag > LocalgameConfig.UpdateFlag)
                            {
                                // Drag and drop game data save and update
                                ObjectgameConfig.ForEach(a =>
                                {
                                    var objectLog = dbmanager.Table<ObjectGameList>().FirstOrDefault(t=> t.ItemId == a.item_Id);
                                    if (objectLog == null)
                                    {
                                        ObjectGameList ObjectNewLog = new ObjectGameList
                                        {
                                            GameId = a.Id_Game,
                                            RoomId = a.Id_Room,
                                            ItemId = a.item_Id,
                                            ItemName = a.item_Name,
                                            CorrectPoint = a.correct_point,
                                            WrongPoint = a.Wrong_point
                                        };
                                        dbmanager.Insert(ObjectNewLog);
                                    }
                                    else
                                    {
                                        objectLog.GameId = a.Id_Game;
                                        objectLog.RoomId = a.Id_Room;
                                        objectLog.ItemId = a.item_Id;
                                        objectLog.ItemName = a.item_Name;
                                        objectLog.CorrectPoint = a.correct_point;
                                        objectLog.WrongPoint = a.Wrong_point;
                                        dbmanager.UpdateTable(objectLog);
                                    }
                                });
                            }
                        }

                        if(x.Id_Game == 2 && LocalgameConfig.GameId == 2)
                        {
                            if(x.UpdatedFlag > LocalgameConfig.UpdateFlag)
                            {
                                // ANAGRAM GAME DATA SAVE AND UPDATE
                                anagramconfig.ForEach(b =>
                                {
                                    var anagramlog = dbmanager.Table<AnagramGameList>().FirstOrDefault(h=> h.IdWord == b.id_word);
                                    if(anagramlog == null)
                                    {
                                        AnagramGameList AGlog = new AnagramGameList
                                        {
                                            IdWord = b.id_word,
                                            GameId = b.Id_Game,
                                            Question = b.question,
                                            Answer = b.answer,
                                            ImageUrl = b.Path,
                                            CorrectPoint = b.correct_point,
                                            WrongPoint = b.Wrong_point,
                                            BackgroundImage = b.BackgrounImageURL
                                        };
                                        dbmanager.Insert(AGlog);
                                    }
                                    else
                                    {
                                        anagramlog.IdWord = b.id_word;
                                        anagramlog.GameId = b.Id_Game;
                                        anagramlog.Question = b.question;
                                        anagramlog.Answer = b.answer;
                                        anagramlog.ImageUrl = b.Path;
                                        anagramlog.CorrectPoint = b.correct_point;
                                        anagramlog.WrongPoint = b.Wrong_point;
                                        anagramlog.BackgroundImage = b.BackgrounImageURL;
                                        dbmanager.UpdateTable(anagramlog);
                                    }
                                    AnagramImageUrl.Add(b.BackgrounImageURL);
                                    Anagrameimageid.Add(b.id_word);
                                });
                            }
                        }

                        if(x.Id_Game == 3 && LocalgameConfig.GameId == 3)
                        {
                            if(x.UpdatedFlag > LocalgameConfig.UpdateFlag)
                            {
                                //ANGRY BIRD GAME DATA SAVING AND UPDATE
                                ObjectgameConfig.ForEach(c =>
                                {
                                    var ObjectLog = dbmanager.Table<ObjectGameList>().FirstOrDefault(e => e.ItemId == c.item_Id);
                                    if (ObjectLog == null)
                                    {
                                        ObjectGameList objectddata = new ObjectGameList
                                        {
                                            GameId = c.Id_Game,
                                            RoomId = c.Id_Room,
                                            ItemId = c.item_Id,
                                            ItemName = c.item_Name,
                                            CorrectPoint = c.correct_point,
                                            WrongPoint = c.Wrong_point
                                        };
                                        dbmanager.Insert(objectddata);
                                    }
                                    else
                                    {
                                        ObjectLog.GameId = c.Id_Game;
                                        ObjectLog.RoomId = c.Id_Room;
                                        ObjectLog.ItemId = c.item_Id;
                                        ObjectLog.ItemName = c.item_Name;
                                        ObjectLog.CorrectPoint = c.correct_point;
                                        ObjectLog.WrongPoint = c.Wrong_point;
                                        dbmanager.UpdateTable(ObjectLog);
                                    }
                                });
                            }
                        }

                        if (x.Id_Game == 4 && LocalgameConfig.GameId == 4)
                        {
                            if (x.UpdatedFlag > LocalgameConfig.UpdateFlag)
                            {
                                //MATCH THE TILE GAME SAVE AND UPDATE
                                MatchTheTileLocation.ForEach(p =>
                                {
                                    var TileLog = dbmanager.Table<MatchTheTileLocation>().FirstOrDefault(a => a.Type == p.type);
                                    if (TileLog == null)
                                    {
                                        MatchTheTileLocation LocationLog = new MatchTheTileLocation()
                                        {
                                            LocationTypeId = p.id_waste_type,
                                            Type = p.type,
                                            ImageUrl = p.ImageUrl,
                                            GameId = p.Id_Game
                                        };
                                        dbmanager.Insert(LocationLog);
                                    }
                                    else
                                    {
                                        TileLog.LocationTypeId = p.id_waste_type;
                                        TileLog.Type = p.type;
                                        TileLog.ImageUrl = p.ImageUrl;
                                        TileLog.GameId = p.Id_Game;
                                        dbmanager.UpdateTable(TileLog);
                                    }
                                    LocationUrl.Add(p.ImageUrl);
                                    LocationId.Add(p.id_waste_type);
                                });

                                MatchTheTileFlag.ForEach(a =>
                                {
                                    var MTTLog = dbmanager.Table<MatchTheTileFlag>().FirstOrDefault(u => u.IdFlag == a.id_waste);
                                    if (MTTLog == null)
                                    {
                                        MatchTheTileFlag flagLog = new MatchTheTileFlag()
                                        {
                                            IdFlag = a.id_waste,
                                            Flag = a.waste,
                                            LocationMatchId = a.waste_type,
                                            CorrectPoint = a.Score,
                                            WrongPoint = a.Wrong_point,
                                            GameId = a.Id_Game

                                        };
                                        dbmanager.Insert(flagLog);
                                    }
                                    else
                                    {
                                        MTTLog.IdFlag = a.id_waste;
                                        MTTLog.Flag = a.waste;
                                        MTTLog.LocationMatchId = a.waste_type;
                                        MTTLog.CorrectPoint = a.Score;
                                        MTTLog.WrongPoint = a.Wrong_point;
                                        MTTLog.GameId = a.Id_Game;
                                        dbmanager.UpdateTable(MTTLog);
                                    }
                                    FlagUrl.Add(a.ImageUrl);
                                    FlagID.Add(a.id_waste);
                                });

                            }
                        }

                        if(x.Id_Game == 5 && LocalgameConfig.GameId == 5)
                        {
                            if(x.UpdatedFlag > LocalgameConfig.UpdateFlag)
                            {
                                MonsterConfig.ForEach(m =>
                                {
                                    var monsterlog = dbmanager.Table<MonsterDetails>().FirstOrDefault();
                                    if (monsterlog == null)
                                    {
                                        MonsterDetails monsterdata = new MonsterDetails
                                        {
                                            MonsterId = m.monsterId,
                                            CatchPoint = m.catch_point,
                                            GameId = m.Id_Game
                                        };

                                        dbmanager.Insert(monsterdata);
                                    }
                                    else
                                    {
                                        monsterlog.MonsterId = m.monsterId;
                                        monsterlog.CatchPoint = m.catch_point;
                                        monsterlog.GameId = m.Id_Game;
                                        dbmanager.UpdateTable(monsterlog);
                                    }
                                });
                                TruckConfig.ForEach(v =>
                                {
                                    var TruckGameLog = dbmanager.Table<TruckGameList>().FirstOrDefault(l => l.DustbinId == v.Dustbins_Map_Id);
                                    if (TruckGameLog == null)
                                    {
                                        TruckGameList TGlog = new TruckGameList
                                        {
                                            TruckId = v.id_truck,
                                            TruckName = v.truck_name,
                                            GameId = v.Id_Game,
                                            GameName = v.GameName,
                                            DustbinId = v.Dustbins_Map_Id,
                                            DustbinName = v.Dustbins_Name,
                                            CorrectPoint = v.Correct_Dustbin_Point,
                                            WrongPoint = v.Wrong_Dustbin_Point,
                                            ScoreId = v.ScoreId
                                        };
                                        dbmanager.Insert(TGlog);
                                    }
                                    else
                                    {
                                        TruckGameLog.TruckId = v.id_truck;
                                        TruckGameLog.TruckName = v.truck_name;
                                        TruckGameLog.GameId = v.Id_Game;
                                        TruckGameLog.GameName = v.GameName;
                                        TruckGameLog.DustbinId = v.Dustbins_Map_Id;
                                        TruckGameLog.DustbinName = v.Dustbins_Name;
                                        TruckGameLog.CorrectPoint = v.Correct_Dustbin_Point;
                                        TruckGameLog.WrongPoint = v.Wrong_Dustbin_Point;
                                        TruckGameLog.ScoreId = v.ScoreId;
                                        dbmanager.UpdateTable(TruckGameLog);
                                    }
                                });
                                TruckcenterConfig.ForEach(n =>
                                {
                                    var TClog = dbmanager.Table<TruckCenterDetails>().FirstOrDefault(j => j.TruckId == n.id_truck);
                                    if (TClog == null)
                                    {
                                        TruckCenterDetails TCdata = new TruckCenterDetails
                                        {
                                            TruckId = n.id_truck,
                                            CenterName = n.destination_name,
                                            CorrectPoint = n.correct_bonus_point,
                                            WrongPoint = n.wrong_point,
                                            ScoreId = n.ScoreId
                                        };
                                        dbmanager.Insert(TCdata);
                                    }
                                    else
                                    {
                                        TClog.TruckId = n.id_truck;
                                        TClog.CenterName = n.destination_name;
                                        TClog.CorrectPoint = n.correct_bonus_point;
                                        TClog.WrongPoint = n.wrong_point;
                                        TClog.ScoreId = n.ScoreId;
                                        dbmanager.UpdateTable(TClog);
                                    }
                                });
                            }
                        }


                        if(x.Id_Game ==6 && LocalgameConfig.GameId == 6)
                        {
                            if(x.UpdatedFlag > LocalgameConfig.UpdateFlag)
                            {
                                //QUIZ KBC GAME SAVE AND UPDATE
                                Quizconfig.ForEach(q =>
                                {
                                    var Quizlog = dbmanager.Table<QuizGameList>().FirstOrDefault(d=> d.QuizId == q.Id_Quiz);
                                    if (Quizlog == null)
                                    {
                                        QuizGameList quizdataLog = new QuizGameList
                                        {
                                            GameId = q.Id_Game,
                                            CorrectPoint = q.Correct_point,
                                            WrongPoint = q.Wrong_point,
                                            Question = q.Question,
                                            CorrectOption = q.Correct_Options,
                                            Option1 = q.Correct_Options,
                                            Option2 = q.Options_1,
                                            Option3 = q.Options_2,
                                            Option4 = q.Options_3
                                        };
                                        dbmanager.Insert(quizdataLog);
                                    }
                                    else
                                    {
                                        Quizlog.GameId = q.Id_Game;
                                        Quizlog.CorrectPoint = q.Correct_point;
                                        Quizlog.WrongPoint = q.Wrong_point;
                                        Quizlog.Question = q.Question;
                                        Quizlog.CorrectOption = q.Correct_Options;
                                        Quizlog.Option1 = q.Correct_Options;
                                        Quizlog.Option2 = q.Options_1;
                                        Quizlog.Option3 = q.Options_2;
                                        Quizlog.Option4 = q.Options_3;
                                        dbmanager.UpdateTable(Quizlog);
                                    }
                                });
                            }
                        }

                        LocalgameConfig.GameId = x.Id_Game;
                        LocalgameConfig.Oid = x.ID_ORGANIZATION;
                        LocalgameConfig.GameName = x.GameName;
                        LocalgameConfig.GameType = x.GameType;
                        LocalgameConfig.CompletePer = int.Parse(x.completePer);
                        LocalgameConfig.RoundImageUrl = RoundlocalPath + "/" + x.Id_Game + ".png";
                        LocalgameConfig.RectImageUrl = Rectlocalpath + "/" + x.Id_Game + ".png";
                        LocalgameConfig.UpdateFlag = x.UpdatedFlag;
                        dbmanager.UpdateTable(LocalgameConfig);
                        RectImageId.Add(x.Id_Game);
                        RectImageUrl.Add(x.RectangleImgURL);
                        RoundimageId.Add(x.Id_Game);
                        RoundImageUrl.Add(x.RoundedImgURL);
                    }
                });

                for (int a = 0; a < AnagramImageUrl.Count; a++)
                {
                    yield return new WaitForSeconds(0.2f);
                    StartCoroutine(GetTexture(Anagrameimageid[a], AnagramImageUrl[a], AnagramDir));
                }
                yield return new WaitForSeconds(1f);
                for (int a = 0; a < Urls.Count; a++)
                {
                    yield return new WaitForSeconds(0.2f);
                    StartCoroutine(GetTexture(Ids[a], Urls[a], AvatarDir));
                }
                yield return new WaitForSeconds(1f);
                for (int a = 0; a < LocationUrl.Count; a++)
                {
                    yield return new WaitForSeconds(0.2f);
                    StartCoroutine(GetTexture(LocationId[a], LocationUrl[a], MatchTheTileLoactionFolder));
                }
                yield return new WaitForSeconds(1f);
                for (int a = 0; a < FlagUrl.Count; a++)
                {
                    yield return new WaitForSeconds(0.2f);
                    StartCoroutine(GetTexture(FlagID[a], FlagUrl[a], MatchTheTileFlagFolder));
                }
                yield return new WaitForSeconds(1f);
                for (int a = 0; a < RoundImageUrl.Count; a++)
                {
                    yield return new WaitForSeconds(0.2f);
                    StartCoroutine(GetTexture(RoundimageId[a], RoundImageUrl[a], RoundimagePreviewFolder));
                }
                yield return new WaitForSeconds(1f);
                for (int a = 0; a < RectImageUrl.Count; a++)
                {
                    yield return new WaitForSeconds(0.2f);
                    StartCoroutine(GetTexture(RectImageId[a], RectImageUrl[a], RectImagepreviewFolder));
                }
                yield return new WaitForSeconds(2f);
                StartCoroutine(GetuserAvatar());
                
            }
        }
    }


    public void LogoutTask()
    {
        StartCoroutine(Logout());
    }

    IEnumerator Logout()
    {
        yield return new WaitForSeconds(0.1f);
        dbmanager.Execute("DELETE from GameListDetails");
        dbmanager.Execute("DELETE from AnagramGameList");
        dbmanager.Execute("DELETE from MonsterDetails");
        dbmanager.Execute("DELETE from ObjectGameList");
        dbmanager.Execute("DELETE from QuizGameList");
        dbmanager.Execute("DELETE from TruckCenterDetails");
        dbmanager.Execute("DELETE from TruckGameList");
        dbmanager.Execute("DELETE from MatchTheTileFlag");
        dbmanager.Execute("DELETE from MatchTheTileLocation");
        dbmanager.Execute("DELETE from PercentileTable");
        PlayerPrefs.DeleteKey("Loggedin");
        iTween.ScaleTo(LogOutpage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        LogOutpage.SetActive(false);
        GameSelectionpage.SetActive(false);
        StartingPage.SetActive(true);
        string msg = "Logout Successfully!!!";
        StartCoroutine(ShowPopUp(msg, happy));

    }
}
