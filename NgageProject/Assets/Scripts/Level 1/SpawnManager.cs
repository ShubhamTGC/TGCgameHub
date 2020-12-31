using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using m2ostnextservice.Models;
using Newtonsoft;
using UnityEngine.Networking;
using SimpleSQL;
using System.Linq;

public class SpawnManager : MonoBehaviour
{

    private int roomID = 6;

    private PlayerController player;
    public GameObject PlayerAvatar;
    public GameObject levelPart_prefab; //level part prefab
    public GameObject firstPart; //the current level part in which the game start

    public List<GameObject> pooledLevelParts; //list for object pooling of level parts

    public static SpawnManager instant;

    private GameManager gameManager;

    public GameObject died;
    public GameObject scored;

    private int completeScore;
    private int correct_point;
    private int wrong_point;

    private List<objectgamilist> objectList;
    private int index;

    private bool repeat;

    public int attemptNo;
    private string gameStatus;
    public SimpleSQLManager dbmanager;
    [SerializeField] private int GameId;
    [HideInInspector] public int RoomId;
    /// <summary>
    /// API
    /// </summary>
   

    private const string getAttemptURL = "http://140.238.249.68/TGCGame/api/getattemptNo";

    public GameObject PausePage, Leaderboard,ScratchcardPage;
    private bool IsPassed;

    //?UID=1&GID=1&RID=2

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        repeat = false;
    }

    private void OnEnable()
    {
        //if (repeat)
        //{
        //    player.completeScore = completeScore;
        //    player.correct_Point = correct_point;
        //    player.wrong_Point = wrong_point;
        //}

        IsPassed = false;
        StartCoroutine(getAttemptNo());
        
    }

    // Start is called before the first frame update
    void Start()
    {
        var LocalLog = dbmanager.Table<ObjectGameList>().Where(x => x.GameId == GameId).Select(y => y.RoomId).ToList();
        RoomId = LocalLog[0];
        completeScore = dbmanager.Table<ObjectGameList>().FirstOrDefault(a => a.RoomId == RoomId).CompletionScore;
        correct_point = dbmanager.Table<ObjectGameList>().FirstOrDefault(a => a.RoomId == RoomId).CorrectPoint;
        wrong_point = dbmanager.Table<ObjectGameList>().FirstOrDefault(a => a.RoomId == RoomId).WrongPoint;
 

        instant = this;
        pooledLevelParts = new List<GameObject>();

        player = FindObjectOfType<PlayerController>();

        died.gameObject.SetActive(false);
        scored.gameObject.SetActive(false);

        for(int i = 0; i < 2; i++) //instantiating level parts and adding them to the list
        {
            GameObject levelPart = (GameObject)Instantiate(levelPart_prefab);
            levelPart.SetActive(false);
            pooledLevelParts.Add(levelPart);
        }

        pooledLevelParts.Add(firstPart);

        player.completeScore = completeScore;
        player.correct_Point = correct_point;
        player.wrong_Point = wrong_point;

        repeat = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject getPooledLevel() //getting the pooled level part for use
    {
        for(int i = 0; i < pooledLevelParts.Count; i++)
        {
            if (!pooledLevelParts[i].activeInHierarchy) //if they are disabled, we will enable them and use them
            {
                return pooledLevelParts[i];
            }
        }
        return null;
    }


    public void removeOthers() //disabling all level parts other than current level part
    {
        for (int i = 0; i < pooledLevelParts.Count; i++)
        {
            if (!pooledLevelParts[i].GetComponent<LevelManager>().isCurrent)
            {
                pooledLevelParts[i].SetActive(false);
                pooledLevelParts[i].GetComponent<LevelManager>().coinSpawned = false; //this will be reset the coin spawn boolean
                pooledLevelParts[i].GetComponent<LevelManager>().disbleCoins(); //disabling coins which are active in their poollist
                pooledLevelParts[i].GetComponent<LevelManager>().disableEnemies(); // disabling all the enemies on the level part
                pooledLevelParts[i].GetComponent<LevelManager>().disablePlatforms();
            }
        }
    }
      


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////     GAME OVER    /////////////////////////////////////////////////////////////

    public void gameOver(bool status)
    {
        player.isGamePaused = true;
        //Time.timeScale = 0;
        PlayerAvatar.SetActive(false);
        StartCoroutine(PostData());
        StartCoroutine(PostMasterLog());
        if(status == false)
        {
            //gameStatus = "not_completed";
            died.gameObject.SetActive(true);
            IsPassed = false;
        }
        else
        {
            //gameStatus = "completed";
            scored.gameObject.SetActive(true);
            IsPassed = true;
        }
    }

    public void exitGame()
    {
        //StartCoroutine(PostData(false));
    }

    public void replayGame()
    {
        gameManager.isReplaying[0] = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void goToMenu()
    {
        for (int i = 0; i < gameManager.isReplaying.Length; i++)
        {
            gameManager.isReplaying[i] = false;
        }
        //StartCoroutine(PostData(true));
        
    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///     API
    ///     

    IEnumerator getAttemptNo()
    {
        AESAlgorithm AES = new AESAlgorithm();
        string hittingURL = $"{MainUrls.BaseUrl}{MainUrls.GetAttemopNo}?UID={PlayerPrefs.GetInt("UID")}&GID={GameId}&RID={RoomId}";
        WWW Request = new WWW(hittingURL);
        yield return Request;
        if (Request.text != null)
        {
            if (Request.text != "")
            {
                string CompleteLog = Request.text.TrimStart('"').TrimEnd('"');
                string Log = AES.getDecryptedString(CompleteLog);
                AttemptNoModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<AttemptNoModel>(Log);
                attemptNo = Convert.ToInt32(model.Master_AttemptNo);
            }
        }
    }

    IEnumerator PostData()
    {
        yield return null;
        string hittingURL = $"{MainUrls.BaseUrl}{MainUrls.PostShootnRunApi}";
        PostModel postmodel = new PostModel
        {
            score = player.currentScore,
            status = "a",
            updated_date_time = DateTime.Now,
            id_user = PlayerPrefs.GetInt("UID"),
            Id_Game = GameId,
            Id_Room = RoomId,
            Killed_Enemies = player.enemiesKilled,
            Life_count = player.lives,
            attempt_no = attemptNo + 1
        };

        AESAlgorithm aes = new AESAlgorithm();
        string postLog = Newtonsoft.Json.JsonConvert.SerializeObject(postmodel);
        Debug.Log("master log data " + postLog);

        string EncryptedData = aes.getEncryptedString(postLog);
        CommonModel model = new CommonModel()
        {
            Data = EncryptedData
        };

        string finalData = Newtonsoft.Json.JsonConvert.SerializeObject(model);

        using (UnityWebRequest request = UnityWebRequest.Put(hittingURL, finalData))
        {
            Debug.Log(request);
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();
            if (!request.isNetworkError && !request.isHttpError)
            {

                Debug.Log(" response " + request.downloadHandler.text);
            }
            else
            {
                Debug.Log("prob : " + request.error);
            }
        }


    }

    IEnumerator PostMasterLog()
    {
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.MasterLogApi}";
        NgageMasterPostLog PostField = new NgageMasterPostLog()
        {
            id_user = PlayerPrefs.GetInt("UID"),
            ID_ORGANIZATION = PlayerPrefs.GetInt("OID"),
            score = player.currentScore,
            attempt_no = attemptNo + 1,
            timetaken_to_complete = "00:00",
            is_completed = 1,
            game_type = 1,
            Id_Game = GameId
        };

        AESAlgorithm aes = new AESAlgorithm();
        string PostLog = Newtonsoft.Json.JsonConvert.SerializeObject(PostField);

        string EncryptedData = aes.getEncryptedString(PostLog);
        CommonModel model = new CommonModel()
        {
            Data = EncryptedData
        };

        string finaldata = Newtonsoft.Json.JsonConvert.SerializeObject(model);
        Debug.Log("master data log " + finaldata);
        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, finaldata))
        {
            Request.method = UnityWebRequest.kHttpVerbPOST;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                Debug.Log(Request.downloadHandler.text);

            }
            else
            {
                Debug.Log("Error " + Request.downloadHandler.text);
            }

        }
    }

    public void GotoHome()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetString("From", "level");
        StartCoroutine(Hometask());
    }
    IEnumerator Hometask()
    {
        iTween.ScaleTo(PausePage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        PausePage.SetActive(false);
        SceneManager.LoadScene(7);

    }

    public void BackAction()
    {
        StartCoroutine(PauseTask());
    }

    IEnumerator PauseTask()
    {
        PausePage.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        Time.timeScale = 0f;
    }

    public void ClosePausepage()
    {
        Time.timeScale = 1f;
        StartCoroutine(CloserOfPausePage());
    }

    IEnumerator CloserOfPausePage()
    {
        iTween.ScaleTo(PausePage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        PausePage.SetActive(false);
       
    }
    
    public void OkDonePage(GameObject Panel)
    {
        Time.timeScale = 1f;
        player.gameObject.SetActive(false);
        Panel.SetActive(false);
        if (IsPassed)
        {
            ScratchcardPage.SetActive(true);
        }
        else
        {
            Leaderboard.SetActive(true);
        }

       
    }

    public void BacktoHomepage()
    {
        PlayerPrefs.SetString("From", "level");
        SceneManager.LoadScene(7);
    }


}
