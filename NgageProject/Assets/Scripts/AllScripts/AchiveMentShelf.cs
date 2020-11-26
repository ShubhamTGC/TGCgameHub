using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AchiveMentShelf : MonoBehaviour
{
    public Text Username;
    public List<Sprite> BoyFace,GirlFace;
    public Image PlayerFace,GirlPlayerFace;
    public string MainUrl, AchivementApi, PostGenericPicApi, PostGamefeedApi,PostDiyApi;
    public Sprite LockedFrame, UnlockFrame;
    public List<GameObject> BadgeFrames;
    [Header("Api response data")]
    [Space(15)]
    public Text Playername;
    public Text HighscoreBadgeCount, MostObsBadgeCount, MostActiveCount;
    public Color UnlockColor;
    public GameObject Msgpanel;
    public Text ShareImageStatus;
    private  List<byte[]> post_image_byte = new List<byte[]>(1);
    private int id_post_image;
    public Sprite HighScoreBadge, MostActiveplayer, EagleEyeBadge;
    public Sprite FadeHighScoreBadge, FadeMostActiveplayer, FadeEagleBadge;
    public Image Scorebadge, ActiveBadge, EagleBadge;
    void Start()
    {
        
    }

    private void OnEnable()
    {

        if (PlayerPrefs.GetString("gender").Equals("m",System.StringComparison.OrdinalIgnoreCase))
        {
            PlayerFace.gameObject.SetActive(true);
            GirlPlayerFace.gameObject.SetActive(false);
            PlayerSetup(BoyFace,PlayerFace);
        }
        else
        {
            PlayerFace.gameObject.SetActive(false);
            GirlPlayerFace.gameObject.SetActive(true);
            PlayerSetup(GirlFace, GirlPlayerFace);
        }
        StartCoroutine(GetAchivementdata());
    }
   
    void Update()
    {
        
    }

    void PlayerSetup(List<Sprite> Face,Image profilepic)
    {
        Username.text = PlayerPrefs.GetString("username");
        for(int a = 0; a < Face.Count; a++)
        {
            if(a == PlayerPrefs.GetInt("characterType"))
            {
                profilepic.sprite = Face[a];
            }
        }

    }

    IEnumerator GetAchivementdata()
    {
        
        string HittingUrl = MainUrl + AchivementApi + "?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID");
        WWW Achivement_www = new WWW(HittingUrl);
        yield return Achivement_www;
        if(Achivement_www.text != null)
        {
            Debug.Log(Achivement_www.text);
            AchivementModel AchivementLog = Newtonsoft.Json.JsonConvert.DeserializeObject<AchivementModel>(Achivement_www.text);
            Playername.text = AchivementLog.User_name;
            Scorebadge.sprite =int.Parse(AchivementLog.HighscorerBadge_count) > 0 ? HighScoreBadge : FadeHighScoreBadge;
            ActiveBadge.sprite = int.Parse(AchivementLog.MostactiveplayerBadge_count) > 0 ? MostActiveplayer : FadeMostActiveplayer;
            EagleBadge.sprite =int.Parse(AchivementLog.MostObserventBadge_count) > 0 ? EagleEyeBadge : FadeEagleBadge;



            HighscoreBadgeCount.text = AchivementLog.HighscorerBadge_count;
            MostObsBadgeCount.text = AchivementLog.MostObserventBadge_count;
            MostActiveCount.text = AchivementLog.MostactiveplayerBadge_count;
            var CurrentBadgeName = AchivementLog.Current_badge != null ? AchivementLog.Current_badge : "rookie";

            SetCurrentBadge(CurrentBadgeName);

        }
    }

    void SetCurrentBadge(string currentBadge)
    {
        int Counter = 0;
        for(int a = 0; a < BadgeFrames.Count; a++)
        {
            if (BadgeFrames[a].gameObject.name.Equals(currentBadge, System.StringComparison.OrdinalIgnoreCase))
            {
                for (int b = 0; b <= a; b++)
                {
                    BadgeFrames[b].gameObject.GetComponent<Image>().sprite = UnlockFrame;
                    BadgeFrames[b].gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    BadgeFrames[b].gameObject.GetComponent<Image>().color = UnlockColor;
                    BadgeFrames[b].gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().color = UnlockColor;

                }
            }
        }
    }

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = ((float)1 / source.width) * ((float)source.width / targetWidth);
        float incY = ((float)1 / source.height) * ((float)source.height / targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth),
                              incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }

    public void CaptureAchivementShelf()
    {
        StartCoroutine(CpatureTask());
    }

    IEnumerator CpatureTask()
    {
        yield return new WaitForEndOfFrame();

        string path = Application.persistentDataPath + "/AchiveMent"
                + "_" + Screen.width + "X" + Screen.height + "" + ".png";

        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();
        //Convert to png
        byte[] imageBytes = screenImage.EncodeToPNG();

        //Save image to file
        System.IO.File.WriteAllBytes(path, imageBytes);
        Debug.Log("File PAth " + Application.persistentDataPath);
        Texture2D newScreenshot = ScaleTexture(screenImage, 256, 256);
        byte[] bytes = newScreenshot.EncodeToPNG();
        post_image_byte.Add(bytes);
        Texture2D test;
        test = newScreenshot;
        newScreenshot.name = "achivement.png";


        GenericPhotoUpload photoupload = new GenericPhotoUpload();
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("EXTN", "png"));
        formData.Add(new MultipartFormFileSection("IMAGE", post_image_byte[0], test.name, "image/png"));
        formData.Add(new MultipartFormDataSection("UID", PlayerPrefs.GetInt("UID").ToString()));
        formData.Add(new MultipartFormDataSection("OID", PlayerPrefs.GetInt("OID").ToString()));
        formData.Add(new MultipartFormDataSection("TITLE", " "));
        formData.Add(new MultipartFormDataSection("DESCRIPTION", " "));

        string post_url = MainUrl + PostGenericPicApi;
        UnityWebRequest www = UnityWebRequest.Post(post_url, formData);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            ShareImageStatus.text = "Failed to share , Try Again!";
            Msgpanel.SetActive(true);
            formData.Clear();
            post_image_byte.Clear();
            yield return new WaitForSeconds(3f);
            iTween.ScaleTo(Msgpanel, Vector3.zero, 0.3f);
            yield return new WaitForSeconds(0.4f);
            ShareImageStatus.text = "";
            Msgpanel.SetActive(false);

        }
        else
        {

            Debug.Log(www.downloadHandler.text);
            GenericPostModel GenericModel = Newtonsoft.Json.JsonConvert.DeserializeObject<GenericPostModel>(www.downloadHandler.text);
            id_post_image = GenericModel.id_general_feed;
            if(id_post_image > 0)
            {
                if (GenericModel.status.Equals("success", System.StringComparison.OrdinalIgnoreCase))
                {
                    StartCoroutine(PostThisToGameFeed());
                }
                ShareImageStatus.text = "Your Achivements Shared successfully to Game Feed Page!!";
                Msgpanel.SetActive(true);
                formData.Clear();
                post_image_byte.Clear();
                yield return new WaitForSeconds(3f);
                iTween.ScaleTo(Msgpanel, Vector3.zero, 0.3f);
                yield return new WaitForSeconds(0.4f);
                ShareImageStatus.text = "";
                Msgpanel.SetActive(false);
            }
            else
            {
                ShareImageStatus.text = "Something went wrong Please try later!";
                Msgpanel.SetActive(true);
                formData.Clear();
                post_image_byte.Clear();
                yield return new WaitForSeconds(3f);
                iTween.ScaleTo(Msgpanel, Vector3.zero, 0.3f);
                yield return new WaitForSeconds(0.4f);
                ShareImageStatus.text = "";
                Msgpanel.SetActive(false);
            }
       
        }
    }


    IEnumerator PostThisToGameFeed()
    {
        string HittingUrl = MainUrl + PostGamefeedApi;
        PostImagesGameFeedModel PostModel = new PostImagesGameFeedModel();
        PostModel.id_user = PlayerPrefs.GetInt("UID");
        PostModel.id_org = PlayerPrefs.GetInt("OID");
        PostModel.id_data = id_post_image;
        PostModel.id_DIY = 0;
        PostModel.id_tag_photo = 0;
        PostModel.feed_type = 3;

        string data_log = Newtonsoft.Json.JsonConvert.SerializeObject(PostModel);
        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, data_log))
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
                Debug.Log("data not done");
            }

        }
    }



}
