using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SimpleSQL;
using System.IO;
using SimpleSQL;

public class Generationlevel : MonoBehaviour
{  
    public GameObject Done_msg_panel,dusbin;
    public GameObject HomepageObject,Backbutton;


    [Header("stage selection")]
    [Space(10)]
    public Sprite home_sprite;
    public List<GameObject> G_levels;
    public Text zoneinfo;
    public GameObject zonelayer,ButtonSlider;


    [Header("GET LEVEL DATA")]
    [Space(10)]
    public List<int> GameIDS = new List<int>();
    public string MainUrl, GetGamesIDApi,GetBadgeIdApi;
    public int Gamelevel,BadgeType;
    public List<GameObject> ZonesButtons;
    public Text Zoneinfo;

    public GameObject GameguidePage;
    public GameObject gamePage;
    public GameLeaderBoardScript GameLb;


    public SimpleSQLManager dbmanager;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = Camera.main.gameObject.GetComponent<AudioSource>();
        StartCoroutine(getSoundData());
    }


    IEnumerator getSoundData()
    {

        var soundLog = dbmanager.Table<SettingPage>().FirstOrDefault();
        if (soundLog != null)
        {
            audioSource.volume = soundLog.Music == 1 ? soundLog.Music / 2f : soundLog.Music;
        }
        else
        {
            audioSource.volume = 0.5f;
        }
        yield return new WaitForSeconds(0.1f);
    }


    void Start()
    {
        
        //StartCoroutine(GetGamesIDactivity());
        //StartCoroutine(GetBadgeinfo());
    }
     void OnEnable()
    {
        StartCoroutine(AvatarfaceSprites());
        GameguidePage.SetActive(true);
    }
  

    void Update()
    {
       
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

    IEnumerator sceneappear()
    {
        float shadevalue = HomepageObject.GetComponent<Image>().color.a;
        while (shadevalue < 1)
        {
            HomepageObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, shadevalue);
            shadevalue += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
        Backbutton.SetActive(true);
    }

    IEnumerator showstatus()
    {
        //waste_count = 0;
        yield return new WaitForSeconds(0.5f);
        iTween.ScaleTo(Done_msg_panel, Vector3.one, 1f);
    }

    IEnumerator GetGamesIDactivity()
    {
        string HittingUrl = MainUrl + GetGamesIDApi + "?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID") +
            "&id_org_game=" + 1 + "&id_level=" + Gamelevel;
        WWW GameResponse = new WWW(HittingUrl);
        yield return GameResponse;
        if(GameResponse.text != null)
        {
            GetLevelIDs gameIDs = Newtonsoft.Json.JsonConvert.DeserializeObject<GetLevelIDs>(GameResponse.text);
            gameIDs.content.ForEach(x =>
            {
                GameIDS.Add(x.id_game_content);
            });
        }
    }
    IEnumerator GetBadgeinfo()
    {
        string HittingUrl = MainUrl + GetBadgeIdApi + "?id_level=" + Gamelevel;
        WWW Hitting_res = new WWW(HittingUrl);
        yield return Hitting_res;
        if(Hitting_res.text != null)
        {
            List<BadgeDetails> Badgeinfo = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BadgeDetails>>(Hitting_res.text);
            var id_badge = Badgeinfo.FirstOrDefault(x => x.id_level == Gamelevel && x.badge_type == BadgeType)?.id_badge;
        }

    }

  

    public void BackToMainPage()
    {
        SceneManager.LoadScene(0);
    }


    //======================== stage selection methos ==============================
    public void sublevelmethod(GameObject selectedbtn)
    {
        //midlayer.SetActive(false);
        string buttonname = selectedbtn.name;
        selectedbtn.transform.GetChild(0).gameObject.SetActive(false);
        zoneinfo.text = "";
        sublevelactions(buttonname);
    }

    void sublevelactions(string name)
    {
        switch (name.ToLower())
        {
            case "homezone":
                StartCoroutine(ZoneSelectionProcess(home_sprite, 0));
                break;
            case "schoolzone":
                StartCoroutine(ZoneSelectionProcess(home_sprite, 1));
                break;
            case "hospitalzone":
                StartCoroutine(ZoneSelectionProcess(home_sprite, 2));
                break;
            case "officezone":
                StartCoroutine(ZoneSelectionProcess(home_sprite, 3));
                break;
            case "factoryzone":
                StartCoroutine(ZoneSelectionProcess(home_sprite, 4));
                break;
            case "parkzone":
                StartCoroutine(ZoneSelectionProcess(home_sprite, 5));
                break;
            default:
                Debug.Log("unique zone");
                break;
        }

    }


    // METHOD FOR CARD VIEW ZONE SELECTION WITH UPDATED UI
    IEnumerator midlevel_task(Sprite scene_sprite, int level_id)
    {

        yield return new WaitForSeconds(0.1f);
        Camera.main.GetComponent<AudioSource>().enabled = false;
        Zoneinfo.text = "";
        ZonesButtons.ForEach(x =>
        {
            x.transform.localPosition = new Vector3(0f, -1160f, 0f);
            x.transform.localScale = Vector3.zero;
            x.SetActive(false);
            x.GetComponent<BoxCollider2D>().enabled = false;
            x.GetComponent<Button>().enabled = false;
            x.GetComponent<Animator>().enabled = false;
        });
        zonelayer.SetActive(false);
        StartCoroutine(scenechanges(HomepageObject, scene_sprite));
        yield return new WaitForSeconds(1.2f);
        //Generation_level.SetActive(true);
        G_levels[level_id].SetActive(true);
    }



    // ZONE SELECTION WITH THE CITY LANDSCAPE STYLE OLD DESIGN 
    IEnumerator ZoneSelectionProcess(Sprite scene_sprite, int level_id)
    {
        yield return new WaitForSeconds(0.1f);
        Camera.main.GetComponent<AudioSource>().enabled = false;
        Zoneinfo.text = "";

        zonelayer.SetActive(false);
        StartCoroutine(scenechanges(HomepageObject, scene_sprite));
        yield return new WaitForSeconds(1.2f);
        G_levels[level_id].SetActive(true);
    }

    public void VibrateDevice()
    {
        if (!PlayerPrefs.HasKey("VibrationEnable"))
        {
            Debug.Log("vibrated");
            Vibration.Vibrate(400);
        }
        else
        {
            string vibration = PlayerPrefs.GetString("VibrationEnable");

            if (vibration == "true")
            {
                Debug.Log("vibrated");
                Vibration.Vibrate(400);
            }
        }


    }


    public void CloseGameGuide()
    {
        StartCoroutine(CloseureTask());
    }
    IEnumerator CloseureTask()
    {
        iTween.ScaleTo(GameguidePage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        GameguidePage.SetActive(false);
        gamePage.SetActive(true);
    }


    private Sprite GetAvatarSprite(string path, string spritename)
    {
        if (path.Length > 0)
        {
            byte[] imagedata = File.ReadAllBytes(path);
            Texture2D texture2d = new Texture2D(1, 1);
            Sprite sprite;
            texture2d.LoadImage(imagedata);
            sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), new Vector2(0.5f, 0.5f));
            sprite.name = spritename;
            return sprite;
        }
        return null;
    }

    IEnumerator AvatarfaceSprites()
    {
        yield return new WaitForSeconds(0.05f);
        string path = Application.persistentDataPath + "/AvatarFile";
        foreach (string file in System.IO.Directory.GetFiles(path))
        {
            string spritename = Path.GetFileNameWithoutExtension(file);
            Sprite avatar = GetAvatarSprite(file, spritename);
            GameLb.AvatarModel.Add(avatar);
        }

    }

}
