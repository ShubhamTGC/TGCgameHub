using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using m2ostnextservice.Models;
using Newtonsoft;


public class GameManager : MonoBehaviour
{
    /// <summary>
    ///           GAME DATA
    /// </summary>
    public List<gamelist> gameConfig;
    public List<herelist> heroes;
    public List<enemieslist> enemies;
    public List<attacktoollist> attacktools;
    public List<objectgamilist> objects;

    public int ID_USER;
    public int ID_ORGANIZATION;

    public int ID_GAME;

    public bool[] isReplaying;

    /// <summary>
    ///            API
    /// </summary>
    //private const string getApiURL = "http://140.238.249.68/TGCGame/api/shootandrungamesetup";
    private const string getAttemptURL = "http://140.238.249.68/TGCGame/api/getattemptNo?UID=1&GID=1&RID=2";
    //public Text responseText;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        ID_GAME = 7;
        //StartCoroutine(getDetails());
        isReplaying = new bool[3];
        for(int i = 0; i < isReplaying.Length; i++)
        {
            isReplaying[i] = false;
        }
        Debug.Log(isReplaying[0]);
    }

    /*private void OnEnable()
    {
        ID_USER = PlayerPrefs.GetInt("UID");
        ID_ORGANIZATION = PlayerPrefs.GetInt("OID");
    }*/

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //IEnumerator getDetails()
    //{
    //    AESAlgorithm AES = new AESAlgorithm();
    //    string hittingURL = $"{getApiURL}";
    //    WWW Request = new WWW(hittingURL);
    //    yield return Request;
    //    if (Request.text != null)
    //    {
    //        if (Request.text != "")
    //        {
    //            string CompleteLog = Request.text.TrimStart('"').TrimEnd('"');
    //            string Log = AES.getDecryptedString(CompleteLog);
    //            GetModel getLog = Newtonsoft.Json.JsonConvert.DeserializeObject<GetModel>(Log);
    //            gameConfig = getLog.gamelist;
    //            heroes = getLog.herelist;
    //            enemies = getLog.enemieslist;
    //            attacktools = getLog.attacktoollist;
    //            objects = getLog.objectgamilist;
    //        }
    //    }
    //}   

    
}
