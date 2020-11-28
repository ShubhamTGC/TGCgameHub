using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using m2ostnextservice.Models;
using Newtonsoft;
using UnityEngine.Networking;

public class Spawn2Manager : MonoBehaviour
{

    private int roomID = 7;

    private Player2Controller player;

    public GameObject levelPart_prefab; //level part prefab
    public GameObject firstPart; //the current level part in which the game start

    public List<GameObject> pooledLevelParts; //list for object pooling of level parts

    public static Spawn2Manager instant;

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

    /// <summary>
    /// API
    /// </summary>
    private const string postApiURL = "http://140.238.249.68/TGCGame/api/shootandrundetailsuserlog";

    private const string getAttemptURL = "http://140.238.249.68/TGCGame/api/getattemptNo";

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        repeat = false;
    }

    private void OnEnable()
    {
        if (repeat)
        {
            player.completeScore = completeScore;
            player.correct_Point = correct_point;
            player.wrong_Point = wrong_point;
        }

        StartCoroutine(getAttemptNo());
    }

    // Start is called before the first frame update
    void Start()
    {

        objectList = gameManager.objects;

        for (int i = 0; i < objectList.Count; i++)
        {
            if (objectList[i].Id_Room == roomID)
            {
                index = i;
            }
        }

        completeScore = objectList[index].Complete_Score;
        correct_point = objectList[index].correct_point;
        wrong_point = objectList[index].Wrong_point;

        instant = this;
        pooledLevelParts = new List<GameObject>();

        player = FindObjectOfType<Player2Controller>();

        died.gameObject.SetActive(false);
        scored.gameObject.SetActive(false);

        for (int i = 0; i < 2; i++) //instantiating level parts and adding them to the list
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
        for (int i = 0; i < pooledLevelParts.Count; i++)
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
            if (!pooledLevelParts[i].GetComponent<Level2Manager>().isCurrent)
            {
                pooledLevelParts[i].SetActive(false);
                pooledLevelParts[i].GetComponent<Level2Manager>().coinSpawned = false; //this will be reset the coin spawn boolean
                pooledLevelParts[i].GetComponent<Level2Manager>().disbleCoins(); //disabling coins which are active in their poollist
                pooledLevelParts[i].GetComponent<Level2Manager>().disableEnemies(); // disabling all the enemies on the level part
                pooledLevelParts[i].GetComponent<Level2Manager>().disablePlatforms();
            }
        }
    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////     GAME OVER    /////////////////////////////////////////////////////////////

    public void gameOver(bool status)
    {
        player.isGamePaused = true;
        Time.timeScale = 0;

        if (status == false)
        {
            gameStatus = "not_completed";
            died.gameObject.SetActive(true);
        }
        else
        {
            gameStatus = "completed";
            scored.gameObject.SetActive(true);
        }
    }

    public void exitGame()
    {
        StartCoroutine(PostData(false));
    }

    public void replayGame()
    {
        gameManager.isReplaying[1] = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void goToMenu()
    {
        for (int i = 0; i < gameManager.isReplaying.Length; i++)
        {
            gameManager.isReplaying[i] = false;
        }
        StartCoroutine(PostData(true));
        
    }

    IEnumerator getAttemptNo()
    {
        AESAlgorithm AES = new AESAlgorithm();
        string cred = "?UID=" + gameManager.ID_USER.ToString() + "&GID=7&RID=7";
        string hittingURL = $"{getAttemptURL}{cred}";
        WWW Request = new WWW(hittingURL);
        yield return Request;
        if (Request.text != null)
        {
            if (Request.text != "")
            {
                string CompleteLog = Request.text.TrimStart('"').TrimEnd('"');
                string Log = AES.getDecryptedString(CompleteLog);
                AttemptNoModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<AttemptNoModel>(Log);
                attemptNo = Convert.ToInt32(model.Detail_AttempNo);
                if (gameManager.isReplaying[1])
                {
                    attemptNo++;
                }
                //Debug.Log(attemptNo);
            }
        }
    }

    IEnumerator PostData(bool stat)
    {
        yield return null;
        string hittingURL = $"{postApiURL}";
        PostModel postmodel = new PostModel
        {
            score = player.currentScore,
            status = gameStatus,
            updated_date_time = DateTime.Now,
            id_user = gameManager.ID_USER,
            Id_Game = 7,
            Id_Room = roomID,
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

        if (stat)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            Application.Quit();
        }

    }

}
