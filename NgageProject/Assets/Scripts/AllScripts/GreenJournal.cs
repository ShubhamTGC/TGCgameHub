using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GreenJournal : MonoBehaviour
{
    public Sprite PressedSprite, Relasedsprite;
    public List<GameObject> TabsButtons;
    public List<GameObject> MainTabs;
    public GameObject ActionPlanPage, GameFeedPage, DiyPage;
    public GameObject GalleryPage,JournalPage;
    public List<Sprite> BoyFace, GirlFace,BoyBody,Girlbody;
    public Image BoyFaceimg,BoyBodyimg,GirlFaceimg,GirlBodyimg;
    public GameObject BoyProfile, GirlProfile;
    public string MainUrl, StageUnlockApi;
    public int StageUnlockScore;
    private bool Stage2unlocked;
    [SerializeField] private int Stagelevel;
    public GameObject Deberfingbtn;
    void Start()
    {
          
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {

        if(PlayerPrefs.GetString("gender").Equals("m",System.StringComparison.OrdinalIgnoreCase))
        {
            BoyProfile.SetActive(true);
            GirlProfile.SetActive(false);
            PlayerSetup(BoyFace, BoyBody, BoyFaceimg, BoyBodyimg);
        }
        else
        {
            BoyProfile.SetActive(false);
            GirlProfile.SetActive(true);
            PlayerSetup(GirlFace, Girlbody, GirlFaceimg, GirlBodyimg);
        }
        TabsButtons[0].gameObject.GetComponent<Image>().sprite = PressedSprite;
        TabsButtons[1].gameObject.GetComponent<Image>().sprite = Relasedsprite;
        TabsButtons[2].gameObject.GetComponent<Image>().sprite = Relasedsprite;
        ActionPlanPage.SetActive(true);
        GameFeedPage.SetActive(false);
        DiyPage.SetActive(false);
        //if(SceneManager.GetActiveScene().buildIndex != 0)
        //{
        //    StartCoroutine(Getlevelclearnessdata());
        //}
       
    }

    void PlayerSetup(List<Sprite> Faces,List<Sprite> Body,Image FaceImage,Image BodyImage)
    {
        for(int a = 0; a < Faces.Count; a++)
        {
            if(a == PlayerPrefs.GetInt("characterType"))
            {
                FaceImage.sprite = Faces[a];
            }
        }
        for (int b = 0; b < Body.Count; b++)
        {
            if (b == PlayerPrefs.GetInt("PlayerBody"))
            {
                BodyImage.sprite = Body[b];
            }
        }
    }


    public void MainButtonsActivity(GameObject currentGameobject)
    {
        TabsButtons.ForEach(x =>
        {
            x.GetComponent<Image>().sprite = x.name == currentGameobject.name ? PressedSprite : Relasedsprite;
        
        });

        PageSelection(currentGameobject.name);

    }

    void PageSelection(string selectedBtn)
    {
        switch (selectedBtn.ToLower())
        {
            case "actionplan":
                ActionPlanActivity();
                break;
            case "gamefeed":
                GameFeedActivity();
                break;
            case "diy":
                DiyActivity();
                break;
            default:
                Debug.Log("unique");
                break;
        }
    }

    void ActionPlanActivity()
    {
        ActionPlanPage.SetActive(true);
        GameFeedPage.SetActive(false);
        DiyPage.SetActive(false);
    }

    void GameFeedActivity()
    {
        ActionPlanPage.SetActive(false);
        GameFeedPage.SetActive(true);
        DiyPage.SetActive(false);
        this.gameObject.SetActive(false);
    }
    void DiyActivity()
    {
        ActionPlanPage.SetActive(false);
        GameFeedPage.SetActive(false);
        DiyPage.SetActive(true);
    }

    public void ShowAllActionPlan()
    {
        GalleryPage.SetActive(true);
    }


    IEnumerator Getlevelclearnessdata()
    {
        string Hitting_url = $"{MainUrl}{StageUnlockApi}?UID={PlayerPrefs.GetInt("UID")}&id_level={Stagelevel}&id_org_game={1}";
        WWW StageData = new WWW(Hitting_url);
        yield return StageData;
        if (StageData.text != null)
        {
            StageUnlockModel StageModel = Newtonsoft.Json.JsonConvert.DeserializeObject<StageUnlockModel>(StageData.text);
            Debug.Log(" score "+StageModel.ConsolidatedScore);
            Stage2unlocked = int.Parse(StageModel.ConsolidatedScore) >= StageUnlockScore;
            Deberfingbtn.SetActive(Stage2unlocked);
        }
    }
}
