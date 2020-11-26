using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Stage2ZoneGame : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MainZonePage,ZonePage,LevelPage;
    public GameObject Homepage,TriviaPage;
    public GameObject ChildZoneA, ChildZoneB;
    public GameObject ZoneA, ZoneB;
    [SerializeField]
    private Color RelasedEffect;
    public Sprite levelpagesprite,CityZoneSprite;
    public Stage2Controller stage2controller;
    private GameObject selectedobj;

    //====Stage 3 unlocking status =====
    public string MainUrl, levelClearnessApi, GetGameContentIDs, StageUnlockApi;
    public int Stage2UnlockScore ;
    public int ZoneNo;
    private int totalscoreOfUser;
    private bool Stage3unlocked = false;
    public GameObject GameClearedPopup;
    [SerializeField]
    private int Gamelevel;
    public List<int> id_game_content = new List<int>();

    //============= BONUS GAME POP UPS========================
    public GameObject MatchTheTile, Stage2LeaderBoardPage, DeberifingPage,DeberifingBtn;
    public GameObject GameGuidePage;
    private GameObject selectedZone;
    public List<GameObject> PopupinGameGuide;
    public GameObject ZoneTextinfo;
    void Start()
    {
        Debug.Log("checking");
       // StartCoroutine(CheckForStage3());
       // StartCoroutine(GetGameID());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

     void OnEnable()
    {
       
    }


    public void buttonmsg(GameObject Zone)
    {
        this.gameObject.GetComponent<Image>().color = RelasedEffect;
        ChildZoneA.SetActive(false);
        ChildZoneB.SetActive(false);
        Zone.SetActive(true);
        MainZonePage.SetActive(false);
    }

    public void Selectzone(GameObject SelectZonepage)
    {
        StartCoroutine(Zoneselected(SelectZonepage));
    }

    IEnumerator Zoneselected(GameObject SelectedZone)
    {
        yield return new WaitForSeconds(0.1f);
        ZonePage.SetActive(false);
        this.gameObject.GetComponent<Image>().color = RelasedEffect;
        ChildZoneA.SetActive(false);
        ChildZoneB.SetActive(false);
        StartCoroutine(fadeEffect());
        LevelPage.SetActive(false);
        yield return new WaitForSeconds(1f);
        Homepage.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        Homepage.GetComponent<Image>().sprite = CityZoneSprite;
        selectedZone = SelectedZone;
        GameGuidePage.SetActive(true);
        ZoneTextinfo.GetComponent<Text>().text = "";
        ZoneTextinfo.SetActive(false);

    }


    public void BackfromLevel()
    {
        StartCoroutine(Backtask());
     
    }

    IEnumerator Backtask()
    {
        selectedobj.SetActive(false);
        StartCoroutine(stage2controller.scenechanges(Homepage, CityZoneSprite));
        yield return new WaitForSeconds(1.2f);
        LevelPage.SetActive(false);
        ZonePage.SetActive(true);

    }

    public void BacktoHome()
    {
        SceneManager.LoadScene(0);
    }

    public void ZoneALevelSelection(GameObject SelectdLevel)
    {
        StartCoroutine(ZoneSelectionTask(SelectdLevel));
     
    }

    IEnumerator ZoneSelectionTask(GameObject SelectdLevel)
    {
        StartCoroutine(fadeEffect());
        LevelPage.SetActive(false);
        selectedobj.SetActive(false);
        ChildZoneA.SetActive(false);
        ChildZoneB.SetActive(false);
        yield return new WaitForSeconds(1f);
        Homepage.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        Homepage.GetComponent<Image>().sprite = CityZoneSprite;
        GameGuidePage.SetActive(true);
        SelectdLevel.SetActive(true);
       
    }
    public void CloseGameGuide()
    {
        StartCoroutine(CloseGameGuidetask());
    }

    IEnumerator CloseGameGuidetask()
    {
        PopupinGameGuide.ForEach(x =>
        {
            iTween.ScaleTo(x.gameObject, Vector3.zero, 0.2f);
            iTween.MoveTo(x.gameObject, iTween.Hash("position", Vector3.zero, "isLocal", true, "time", 0.2f));
        });
        yield return new WaitForSeconds(0.5f);
        iTween.ScaleTo(GameGuidePage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        GameGuidePage.SetActive(false);
        selectedZone.SetActive(true);
        MainZonePage.SetActive(false);
        //this.gameObject.SetActive(false);
    }


    IEnumerator fadeEffect()
    {
        float shadevalue = Homepage.GetComponent<Image>().color.a;
        while (shadevalue >= 0)
        {
            Homepage.GetComponent<Image>().color = new Color(1f, 1f, 1f, shadevalue);
            shadevalue -= 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator CheckForStage3()
    {
        TriviaPage.SetActive(true);
        string Hitting_url = $"{MainUrl}{StageUnlockApi}?UID={PlayerPrefs.GetInt("UID")}&id_level={2}&id_org_game={1}";
        WWW StageData = new WWW(Hitting_url);
        yield return StageData;
        if (StageData.text != null)
        {
            StageUnlockModel StageModel = Newtonsoft.Json.JsonConvert.DeserializeObject<StageUnlockModel>(StageData.text);
            Stage3unlocked = int.Parse(StageModel.ConsolidatedScore) >= Stage2UnlockScore;
        }
        yield return new WaitForSeconds(0.5f);
        TriviaPage.SetActive(false);
        GameClearedPopup.SetActive(Stage3unlocked);
        if (Stage3unlocked)
        {
            DeberifingBtn.SetActive(true);
        }
     
    }

    IEnumerator GetGameID()
    {
        string HittingUrl = MainUrl + GetGameContentIDs + "?UID=" +PlayerPrefs.GetInt("UID") + "&OID=" +PlayerPrefs.GetInt("OID") +
            "&id_org_game=" + 1 + "&id_level=" + Gamelevel;

        WWW GameIDs_www = new WWW(HittingUrl);
        yield return GameIDs_www;
        if(GameIDs_www.text != null)
        {
            GetLevelIDs Gameid = Newtonsoft.Json.JsonConvert.DeserializeObject<GetLevelIDs>(GameIDs_www.text);
            Gameid.content.ForEach(x =>
            {
                id_game_content.Add(x.id_game_content);
            });
        }
    }

    public void PLayBonusGame()
    {
        StartCoroutine(GameActiveTask());
    }

    public void closeBonusGAme()
    {
        StartCoroutine(CloseGame());
    }

    IEnumerator GameActiveTask()
    {
        ZoneA.GetComponent<PolygonCollider2D>().enabled = false;
        ZoneB.GetComponent<PolygonCollider2D>().enabled = false;
        iTween.ScaleTo(GameClearedPopup, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        GameClearedPopup.SetActive(false);
        MatchTheTile.SetActive(true);
    }

    IEnumerator CloseGame()
    {
        iTween.ScaleTo(GameClearedPopup, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        ZoneA.GetComponent<PolygonCollider2D>().enabled = true;
        ZoneB.GetComponent<PolygonCollider2D>().enabled = true;
        GameClearedPopup.SetActive(false);
    
    }
    public void CancelBonusGAme()
    {
        StartCoroutine(cancelGame());
    }

    IEnumerator cancelGame()
    {
        iTween.ScaleTo(MatchTheTile, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        MatchTheTile.SetActive(false);
        ZoneA.GetComponent<PolygonCollider2D>().enabled = true;
        ZoneB.GetComponent<PolygonCollider2D>().enabled = true;

    }

    public void CloseStageLeaderBoard()
    {
        Stage2LeaderBoardPage.SetActive(false);
        DeberifingPage.SetActive(true);

    }


    public void CloseDeberifingPage()
    {
        DeberifingPage.SetActive(false);
        ZoneA.GetComponent<PolygonCollider2D>().enabled = true;
        ZoneB.GetComponent<PolygonCollider2D>().enabled = true;
    }

}
