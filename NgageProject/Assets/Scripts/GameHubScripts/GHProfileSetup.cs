using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSQL;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using m2ostnextservice.Models;
using UnityEngine.Networking;
using System;

public class GHProfileSetup : MonoBehaviour
{
    public SimpleSQLManager dbmanager;
    private List<Sprite> avatarSprite = new List<Sprite>();
    private int AvatarCounter=0, totalcounter;
    public List<Button> PreviewPage;
    public Image Selectedimage;
    public InputField Name, Emailid, Mobileno, Orgname;
    private int playerAvatarId;
    private AESAlgorithm aes;
    public GameObject PopupPage;
    public Text Msgbox;
    public Image moodimage;
    public Sprite Happy, sad;
    public GameObject landingPage;
    private void OnEnable()
    {
        aes = new AESAlgorithm();
        StartCoroutine(AvatarSelection());
        
    }
    IEnumerator AvatarSelection()
    {
        string path = Application.persistentDataPath + "/AvatarFile";
        Debug.Log("Application path" + path);
        foreach (string file in System.IO.Directory.GetFiles(path))
        {
            string spritename = Path.GetFileNameWithoutExtension(file);
            Sprite avatar = GetAvatarSprite(file, spritename);
            avatarSprite.Add(avatar);
            yield return new WaitForSeconds(0.1f);
        }
        AvatarUpdater(AvatarCounter);
        Selectedimage.sprite = PreviewPage[0].GetComponent<Image>().sprite;
        StartCoroutine(getuserLocaldata());
        totalcounter = avatarSprite.Count - PreviewPage.Count;
    }

    void AvatarUpdater(int counter)
    {
        for (int a = 0; a < PreviewPage.Count; a++)
        {
            PreviewPage[a].image.sprite = avatarSprite[a + counter];
            PreviewPage[a].onClick.RemoveAllListeners();
            PreviewPage[a].onClick.AddListener(delegate { SelectYourAvatar(); });
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

    // Update is called once per frame
    void Update()
    {

    }
    public void NextAvatar()
    {
        if (AvatarCounter < totalcounter)
        {
            AvatarCounter++;
            AvatarUpdater(AvatarCounter);
        }
    }

    public void PreviousAvatar()
    {
        if (AvatarCounter > 0)
        {
            AvatarCounter--;
            AvatarUpdater(AvatarCounter);
        }
    }

    IEnumerator ShowPopUp(string msg,Sprite mood)
    {
        PopupPage.SetActive(true);
        moodimage.sprite = mood;
        Msgbox.text = msg;
        yield return new WaitForSeconds(2.5f);
        iTween.ScaleTo(PopupPage, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.35f);
        Msgbox.text = "";
        PopupPage.SetActive(false);
    }

    public void SelectYourAvatar()
    {
        GameObject gb = EventSystem.current.currentSelectedGameObject;
        Selectedimage.sprite = gb.GetComponent<Image>().sprite;
    }

    IEnumerator getuserLocaldata()
    {
        var LocalLog = dbmanager.Table<ProfileSetup>().FirstOrDefault();
        if(LocalLog != null)
        {
            Name.text = LocalLog.Username != null ? LocalLog.Username : "";
            Emailid.text = LocalLog.EmailId != null ? LocalLog.EmailId:"" ;
            Mobileno.text = LocalLog.Mobileno != null ? LocalLog.Mobileno : "";
            Orgname.text = LocalLog.Orgname != null ? LocalLog.Orgname : "";
            playerAvatarId = LocalLog.AvatarId == 0 ? int.Parse(PreviewPage[0].GetComponent<Image>().sprite.name) : LocalLog.AvatarId;
        }
        yield return new WaitForSeconds(0.1f);
    }

    public void SetUserProfile()
    {
        playerAvatarId = int.Parse(Selectedimage.sprite.name);
        StartCoroutine(SaveProfile(playerAvatarId));
    }
  

    IEnumerator SaveProfile(int id)
    {
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.AvatarSetApi}";
        GHProfilePostModel postlog = new GHProfilePostModel
        {
            Id_User = PlayerPrefs.GetInt("UID"),
            Name = Name.text,
            Organization_Name = Orgname.text,
            Phone_No = Mobileno.text,
            Id_Avatar = playerAvatarId
        };

        string Logdata = Newtonsoft.Json.JsonConvert.SerializeObject(postlog);
        string EncryptedLog = aes.getEncryptedString(Logdata);
        CommonModel commonLog = new CommonModel
        {
            Data = EncryptedLog
        };  
        string FinalLog = Newtonsoft.Json.JsonConvert.SerializeObject(commonLog);
        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, FinalLog))
        {
            Request.method = UnityWebRequest.kHttpVerbPOST;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                if (Request.responseCode.Equals(200))
                {
                    if(Request.downloadHandler.text != null)
                    {
                        var log = Request.downloadHandler.text.TrimStart('"').TrimEnd('"');
                        string response = aes.getDecryptedString(log);
                        LoginResModel LoginLog = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResModel>(response);
                        string msg = "Profile Updated Successfully!!!";
                        StartCoroutine(ShowPopUp(msg,Happy));
                        int avatarid = Convert.ToInt32(LoginLog.Id_Avatar != null ? LoginLog.Id_Avatar : 0);
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
                        StartCoroutine(AfterProfileDone());
                    }
                    else
                    {
                        string msg = "Something Went Wrong Please try again!";
                        StartCoroutine(ShowPopUp(msg,sad));
                    }
                }
                else
                {
                    string msg = "Something Went Wrong Please try again!";
                    StartCoroutine(ShowPopUp(msg,sad));
                }
            }
            else
            {
                string msg = "Check Your internet connection and try again!";
                StartCoroutine(ShowPopUp(msg,sad));
            }
        }
    }

    IEnumerator AfterProfileDone()
    {
        yield return new WaitForSeconds(0.1f);
        landingPage.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
