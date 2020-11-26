using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class DiyPostingHandler : MonoBehaviour
{
    public string MainUrl, DiyPostApi, PostGamefeedApi;
    public InputField UserTitle, Userdetails,Stageno;
    public Text ShareImageStatus;
    public GameObject Msgpanel;
    private string selectedImgPath;
    private Texture2D tex;
    private Texture2D test;
    private List<byte[]> post_image_byte = new List<byte[]>(1);
    private int id_post_image;
    [SerializeField] private Sprite defaultImage;
    private GameObject Previewobj;
    public DIYpageHandler DiyMainPage;
    public GameObject DiyUploadPage, downloadbtn, backbtn, nextbtn, previousbtn,uploadbtn;
    private int Currentrday;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        setBtns(false);
        DateTime Currentdate = DateTime.Today;
        Currentrday = Currentdate.Day;
    }

    void setBtns(bool enable)
    {
        DiyMainPage.Disablebtn = enable;
        downloadbtn.SetActive(enable);
        backbtn.SetActive(enable);
        nextbtn.SetActive(enable);
        previousbtn.SetActive(enable);
        uploadbtn.SetActive(enable);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ImageSelection(GameObject PreviewGb)
    {
        Previewobj = PreviewGb;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) => {

                if (path != null)
                {
                    selectedImgPath = path;
                    Texture2D texture = NativeGallery.LoadImageAtPath(path, 1024, false);
                    if (texture != null)
                    {
                        // imageBytes = texture.EncodeToPNG();
                        texture.name = "test.png";
                        test = texture;
                        tex = texture;
                        Sprite ImageSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                        PreviewGb.GetComponent<Image>().sprite = ImageSprite;
                        byte[] bytes = tex.EncodeToPNG();
                        post_image_byte.Add(bytes);

                    }
                }


            }, "Select Your Image", "image/jpeg");
        }
        else
        {
            string[] image_path = StandaloneFileBrowser.OpenFilePanel("Select Image to Upload", "", "jpg",false);
            if (image_path.Length != 0)
            {
               
                byte[] image_data = File.ReadAllBytes(image_path[0]);
                Texture2D texture = new Texture2D(1024, 1024, TextureFormat.RGB24, true);
                texture.LoadImage(image_data);
                texture.Apply();
                texture.name = "test.png";
                tex = texture;
                test = texture;
                Texture2D newScreenshot = ScaleTexture(texture, 256, 256);
                var bytes = newScreenshot.EncodeToPNG();
                string imagestr = Convert.ToBase64String(bytes);
                Texture2D tex2 = new Texture2D(1, 1);
                tex2.LoadImage(Convert.FromBase64String(imagestr));
                Sprite btn_sprite = Sprite.Create(tex2, new Rect(0, 0, tex2.width, tex2.height), Vector2.zero);
                PreviewGb.GetComponent<Image>().sprite = btn_sprite;
               
                post_image_byte.Add(bytes);
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


    public void PostDiyActivity()
    {
        if(post_image_byte.Count >0)
        {
            if (PlayerPrefs.HasKey("Todaysdate"))
            {
                if (Currentrday == PlayerPrefs.GetInt("Todaysdate"))
                {
                    string msg = "You have already uploaded the activity, Please Try again later.";
                    StartCoroutine(ShowPopupTask(msg));
                    clearFields();
                }
                else
                {
                    var Stagedata = Stageno.text != null ? Stageno.text : "1";
                    var Userdetail = UserTitle.text + "/" + Userdetails.text;
                    StartCoroutine(PostgenericImage(Userdetail, Stagedata));
                }
            }
            else
            {
                var Stagedata = Stageno.text != null ? Stageno.text : "1";
                var Userdetail = UserTitle.text + "/" + Userdetails.text;
                StartCoroutine(PostgenericImage(Userdetail, Stagedata));
            }

        }
        else
        {
            string msg = "Please select your Diy first!";
            StartCoroutine(ShowPopupTask(msg));
        }
    }


    IEnumerator PostgenericImage(string userinfo,string stagenum)
    {
       
        DateTime currentdate = DateTime.Today;

        int randomdate =  UnityEngine.Random.Range(3, 6);
        DateTime UpcomingDate = currentdate.AddDays(randomdate);
        string tempdata = UpcomingDate.ToString("yyyy-MM-ddTHH:mm:ss");
        yield return new WaitForSeconds(0.5f);
        Debug.Log(" image data " + post_image_byte[0].Length);
        yield return new WaitForSeconds(0.5f);

        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("DIYImage", post_image_byte[0], test.name, "image/png"));
        formData.Add(new MultipartFormDataSection("EXTN", "png"));
        formData.Add(new MultipartFormDataSection("UID", PlayerPrefs.GetInt("UID").ToString()));
        formData.Add(new MultipartFormDataSection("OID", PlayerPrefs.GetInt("OID").ToString()));
        formData.Add(new MultipartFormDataSection("DETAIL", userinfo));
        formData.Add(new MultipartFormDataSection("Level", stagenum));
        formData.Add(new MultipartFormDataSection("DIYDATE", tempdata));

        string post_url = MainUrl + DiyPostApi;
        UnityWebRequest www = UnityWebRequest.Post(post_url, formData);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            post_image_byte.Clear();
            formData.Clear();
            string msg = "Failed to share , Try Again!";
            StartCoroutine(ShowPopupTask(msg));
            clearFields();

        }
        else
        {

            Debug.Log(www.downloadHandler.text);
            DiyModel GenericModel = Newtonsoft.Json.JsonConvert.DeserializeObject<DiyModel>(www.downloadHandler.text);
            id_post_image = GenericModel.id_DIY;
            if(id_post_image > 0)
            {
                if (GenericModel.STATUS.Equals("success", System.StringComparison.OrdinalIgnoreCase))
                {
                    StartCoroutine(PostThisToGameFeed());
                    DateTime Todate = DateTime.Today;
                    PlayerPrefs.SetInt("Todaysdate", Todate.Day);
                }
                formData.Clear();
                post_image_byte.Clear();
                Msgpanel.SetActive(false);
                string msg = "Your Diy Activity shared successfully to Game Feed Page!!";
                StartCoroutine(ShowPopupTask(msg));
                clearFields();
            }
            else
            {
                formData.Clear();
                post_image_byte.Clear();
                Msgpanel.SetActive(false);
                string msg = "Something went wrong Please try later!";
                StartCoroutine(ShowPopupTask(msg));
                clearFields();
            }
          

        }
    }

    IEnumerator PostThisToGameFeed()
    {
        string HittingUrl = MainUrl + PostGamefeedApi;
        PostImagesGameFeedModel PostModel = new PostImagesGameFeedModel();
        PostModel.id_user = PlayerPrefs.GetInt("UID");
        PostModel.id_org = PlayerPrefs.GetInt("OID");
        PostModel.id_data = 0;
        PostModel.id_DIY = id_post_image;
        PostModel.id_tag_photo = 0;
        PostModel.feed_type = 1;

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


    IEnumerator ShowPopupTask(string msg)
    {
        ShareImageStatus.text = msg;
        Msgpanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        iTween.ScaleTo(Msgpanel, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.4f);
        ShareImageStatus.text = "";
        Msgpanel.SetActive(false);
    }

    void clearFields()
    {
        UserTitle.text = "";
        Userdetails.text = "";
        Stageno.text = "";
        Previewobj.GetComponent<Image>().sprite = defaultImage;
    }

    public void BackFromDiy()
    {
        setBtns(true);
        DiyUploadPage.SetActive(false);
    }
}
