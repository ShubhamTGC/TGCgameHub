using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSQL;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using m2ostnextservice.Models;

public class HomePageCardSection : MonoBehaviour
{
    public GameObject cardbar;
    [SerializeField] private float cardsize;
    [SerializeField] private float bufferspace;
    [SerializeField] private float initialPos;
    [SerializeField] private float lastPos;
    private float initialtemppos,LastTempPos;
    public SimpleSQLManager dbmanager;
    [HideInInspector]public List<Sprite> avatarSprite = new List<Sprite>();
    public Image Avatarimage;
    public Text userName;

    private Vector3 targetPos,startingpos;
    public GameObject TargetObj,SideBar,Sidepanel;
    public Text Appversion;
    public GameObject Exitpage;
    public GameObject Settingpage, RewardPage;
    public Text userScore;
    public GameObject LeaderBoard;
    [HideInInspector]public int Uservalue;
    public List<GameObject> PreviewCards;
    public List<Text> Gamenames;
    private void Awake()
    {
        StartCoroutine(AvatarSelection());
    }
    void Start()
    {
        startingpos = SideBar.GetComponent<RectTransform>().localPosition;
        targetPos = TargetObj.GetComponent<RectTransform>().localPosition;
    }
    private void OnEnable()
    {
        StartCoroutine(GetCardImage());
        StartCoroutine(GetOverAllScore());
        initialtemppos = initialPos;
        LastTempPos = lastPos;
        Appversion.text = "Version : " + Application.version;
        startingpos = SideBar.GetComponent<RectTransform>().localPosition;
        targetPos = TargetObj.GetComponent<RectTransform>().localPosition;
        cardbar.GetComponent<RectTransform>().localPosition = new Vector3(initialPos, 0f, 0f);
    }

    IEnumerator GetCardImage()
    {
        yield return new WaitForSeconds(0.1f);
        PreviewCards.ForEach(x =>
        {
            x.transform.GetChild(2).gameObject.SetActive(true);
        });
        var gameName = dbmanager.Table<GameListDetails>().Select(b => b.GameName).ToList();
        for(int c = 0; c < gameName.Count; c++)
        {
            Gamenames[c].text = gameName[c].ToUpperInvariant();
        }
        var LocalLog = dbmanager.Table<GameListDetails>().Select(x=>x.RectImageUrl).ToList();
        
        for(int a = 0; a < LocalLog.Count; a++)
        {
            string spritename = Path.GetFileNameWithoutExtension(LocalLog[a]);
            Sprite Preview = GetPreviewSprites(LocalLog[a], spritename);
            PreviewCards[a].GetComponent<Image>().sprite = Preview;
            PreviewCards[a].transform.GetChild(2).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            
        }
       

    }
    IEnumerator GetOverAllScore()
    {
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.OverallScore}?UID={PlayerPrefs.GetInt("UID")}&OrgID={PlayerPrefs.GetInt("OID")}";
        WWW zone_www = new WWW(HittingUrl);
        yield return zone_www;
        if (zone_www.text != null)
        {
            Debug.Log("Log " + zone_www.text);
            string ResponseLog = zone_www.text.TrimStart('"').TrimEnd('"');
            AESAlgorithm aes = new AESAlgorithm();
            string decryptedLog = aes.getDecryptedString(ResponseLog);
            Debug.Log("Log " + decryptedLog);
            List<OverallScoreModel> Model = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OverallScoreModel>>(decryptedLog);
            userScore.text =  Model[0].score.ToString();
            Uservalue =  Model[0].score.GetValueOrDefault(0);
        }
        else
        {
            Debug.Log("isseu "+zone_www.text);
        }
    }
    IEnumerator AvatarSelection()
    {
        yield return new WaitForSeconds(0.05f);
        string path = Application.persistentDataPath + "/AvatarFile";
        foreach (string file in System.IO.Directory.GetFiles(path))
        {
            string spritename = Path.GetFileNameWithoutExtension(file);
            Sprite avatar = GetAvatarSprite(file, spritename);
            avatarSprite.Add(avatar);
        }
        var LocalLog = dbmanager.Table<ProfileSetup>().FirstOrDefault();
        if(LocalLog != null)
        {
            userName.text = LocalLog.Username;
            avatarSprite.ForEach(x =>
            {
                if (int.Parse(x.name) == LocalLog.AvatarId)
                {
                    Avatarimage.sprite = x;
                }
            });
        }
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

    private Sprite GetPreviewSprites(string path, string spritename)
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Nextcard()
    {
        if(initialPos != lastPos)
        {
            float tempPos = initialPos - cardsize - bufferspace;
            cardbar.GetComponent<RectTransform>().localPosition = new Vector3(tempPos, 0f, 0f);
            initialPos = tempPos;
        }
     
    }

    public void PreviousCard()
    {
        if (initialtemppos != cardbar.GetComponent<RectTransform>().localPosition.x)
        {
            float tempPos = initialPos + cardsize + bufferspace;
            cardbar.GetComponent<RectTransform>().localPosition = new Vector3(tempPos, 0f, 0f);
            initialPos = tempPos;
        }
    }


    public void ShowSideCard()
    {
        if(SideBar.GetComponent<RectTransform>().localPosition == startingpos)
        {
            Sidepanel.SetActive(true);
            StartCoroutine(MoveBar(targetPos));
        }
        else
        {
            Sidepanel.SetActive(false);
            StartCoroutine(MoveBar(startingpos));
        }
    }

    IEnumerator MoveBar(Vector3 pos)
    {
        iTween.MoveTo(SideBar, iTween.Hash("position", pos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.4f));
        yield return new WaitForSeconds(0.2f);
    }

    public void openSettingPage()
    {
        ShowSideCard();
        Settingpage.SetActive(true);
    }

    public void ShowLeaderboard()
    {
        ShowSideCard();
        LeaderBoard.SetActive(true);
    }

    public void Exittask()
    {
        ShowSideCard();
        Exitpage.SetActive(true);
    }

    public void Closelogout()
    {
        StartCoroutine(closepage());
    }

    IEnumerator closepage()
    {
        iTween.ScaleTo(Exitpage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        Exitpage.SetActive(false);
    }

    public void ShowRewardspage()
    {
        ShowSideCard();
        RewardPage.SetActive(true);
    }

    public void CloseRewardsPage()
    {
        StartCoroutine(rewardClose());
        
    }
    IEnumerator rewardClose()
    {
        iTween.ScaleTo(RewardPage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        RewardPage.SetActive(false);
    }
}
