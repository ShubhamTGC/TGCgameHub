using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SaveImageToServer : MonoBehaviour
{
    string URL;
    public string Mainurl, upload_image_API, PostGamefeedApi;
    public GameObject closebtn, capturebtn, camera_preview;
    public GameObject capturebtn1;
    public Sprite default_sprite;
    public CameraManager cameraController;
    public Sprite image_sprite;
    public InputField user_text, plan_title;
    public Text respose_text;
    private byte[] bytes;
    private byte[] imageBytes;
    private byte[] Without_image;
    private string image_path;
    public Button save;
    public Text statusmsg;
    public GameObject statusObj;
    Texture2D test;
    private GameObject zone_data;
    private GameObject show_img_obj;
    private string zone;
    private int level_zone =0;
    public List<byte[]> post_image_byte = new List<byte[]>(3);
    public static SaveImageToServer instance;
    public GameObject loadingAnim;
    public Button CaptureButton;
    private int id_tag_photo;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        Initialtask();
        Without_image = new byte[1];
        test = new Texture2D(128, 128, TextureFormat.RGB24, true);
        test.name = "test.png";
    }



     void OnEnable()
    {
       Initialtask();
    }

    void Initialtask()
    {
        
    }
    void OnDisable()
    {
       
        camera_preview.SetActive(false);
        capturebtn1.gameObject.GetComponent<Image>().sprite = default_sprite;
        user_text.text = "";
        plan_title.text = "";
        level_zone = 0;
        zone = "";
        zone_data = null;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void saveimage()
    {
        show_img_obj = cameraController.click_button;
        StartCoroutine(SaveToServer(show_img_obj));
    }


    //================******************** Camera image capture Portion ***********************8============================//
    public IEnumerator SaveToServer(GameObject captureimage)
    {
        post_image_byte.Clear();
        closebtn.SetActive(false);
        capturebtn.SetActive(false);
        yield return new WaitForSeconds(0.1f);      
        int width = Screen.width;
        int height = Screen.height;

        
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, true);
        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        Sprite image_taken = SpriteFromTexture2D(tex);
        yield return new WaitForSeconds(1f);
        captureimage.gameObject.GetComponent<Image>().sprite = image_taken;
        yield return new WaitForSeconds(0.5f);
        camera_preview.gameObject.SetActive(false);
        // Encode texture into PNG

        Texture2D newScreenshot = ScaleTexture(tex, 256, 256);

        var bytes = newScreenshot.EncodeToPNG();
        post_image_byte.Add(bytes);
        tex.name = "test.Png";
        imageBytes = bytes;
        test = tex;
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



    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    public void reset_objects()
    {

        capturebtn1.gameObject.GetComponent<Image>().sprite = default_sprite;
    }


    public void Capture_data_post()
    {

        if (user_text.text == "" && plan_title.text == "" && post_image_byte.Count == 0)
        {
            string msg = "Please make your plan!";
            StartCoroutine(statusmsgshow(msg));
        }
        else
        {
            StartCoroutine(Upload());
        }

    }

    public IEnumerator show_image_mathod(byte[] image_byte, GameObject selected_btn)
    {
        post_image_byte.Clear();
        yield return new WaitForSeconds(0.1f);
        Texture2D texture = new Texture2D(1024, 1024, TextureFormat.RGB24, true);
        texture.LoadImage(image_byte);
        texture.Apply();
        texture.name = "test.png";
        test = texture;
        Texture2D newScreenshot = ScaleTexture(texture, 256, 256);
        var bytes = newScreenshot.EncodeToPNG();
        string imagestr = Convert.ToBase64String(bytes);
        Texture2D tex = new Texture2D(1, 1);
        tex.LoadImage(Convert.FromBase64String(imagestr));
        if (selected_btn.name == "Pic")
        {
            post_image_byte.Add(bytes);
            Sprite btn_sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),Vector2.zero);
            selected_btn.GetComponent<Image>().sprite = btn_sprite;
        }
     
    }


    IEnumerator statusmsgshow(string msg)
    {
        statusmsg.text = msg;
        statusObj.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        statusmsg.text = "";
        iTween.ScaleTo(statusObj, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.5f);
        statusObj.SetActive(false);

    }

    public IEnumerator Upload()
    {
       
        string post_url = "https://www.skillmuni.in/wsmapi/api/PostPhotoUpload/TagPhotoUpload";
        CaptureButton.interactable = false;
        string usertext = user_text.text;
        string plan = plan_title.text;
        string level = PlayerPrefs.GetInt("game_id").ToString();
        string lati = PlayerPrefs.GetFloat("LAT").ToString();
        string longi = PlayerPrefs.GetFloat("LONG").ToString();
        string uid = PlayerPrefs.GetInt("UID").ToString();
        string oid = PlayerPrefs.GetInt("OID").ToString();
        if(post_image_byte.Count == 0)
        {
            post_image_byte.Add(Without_image);
        }
 
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("UID", uid));
        formData.Add(new MultipartFormDataSection("OID", oid));
        formData.Add(new MultipartFormDataSection("EXTN", "png"));
        formData.Add(new MultipartFormFileSection("Media", post_image_byte[0], test.name, "image/png"));
        formData.Add(new MultipartFormDataSection("GCI", level));
        formData.Add(new MultipartFormDataSection("Level", level_zone.ToString()));
        formData.Add(new MultipartFormDataSection("LATI", lati));
        formData.Add(new MultipartFormDataSection("LONGI", longi));
        formData.Add(new MultipartFormDataSection("DETAIL", user_text.text));
        formData.Add(new MultipartFormDataSection("KEYINFO", plan_title.text));


        UnityWebRequest www = UnityWebRequest.Post(post_url, formData);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            respose_text.text = www.downloadHandler.text;

            string msg = "PLEASE TRY LATER!";
            StartCoroutine(statusmsgshow(msg));
            yield return new WaitForSeconds(2.5f);
            CaptureButton.interactable = true;
            user_text.text = "";
            plan_title.text = "";
            post_image_byte.Clear();
            capturebtn1.gameObject.GetComponent<Image>().sprite = default_sprite;
            formData.Clear();
        }
        else
        {
           
            Debug.Log(www.downloadHandler.text);
            ActionPlanResModel actionModelRes = Newtonsoft.Json.JsonConvert.DeserializeObject<ActionPlanResModel>(www.downloadHandler.text);
            if (actionModelRes.STATUS.Equals("success", System.StringComparison.OrdinalIgnoreCase))
            {
                id_tag_photo = actionModelRes.id_tag_photo;
                if(id_tag_photo > 0)
                {
                    StartCoroutine(PostThisToGameFeed());
                    yield return new WaitForSeconds(0.5f);
                    string msg = "Plan Successfully Generated!";
                    StartCoroutine(statusmsgshow(msg));
                    yield return new WaitForSeconds(2.5f);
                    CaptureButton.interactable = true;
                    user_text.text = "";
                    plan_title.text = "";
                    post_image_byte.Clear();
                    capturebtn1.gameObject.GetComponent<Image>().sprite = default_sprite;
                    formData.Clear();
                }
                else
                {
                    string msg = "Something went wrong, Please try again later!";
                    StartCoroutine(statusmsgshow(msg));
                    yield return new WaitForSeconds(2.5f);
                    CaptureButton.interactable = true;
                    user_text.text = "";
                    plan_title.text = "";
                    post_image_byte.Clear();
                    capturebtn1.gameObject.GetComponent<Image>().sprite = default_sprite;
                    formData.Clear();
                }
            }
            else
            {
                string msg = "Something went wrong, Please try again later!";
                StartCoroutine(statusmsgshow(msg));
                CaptureButton.interactable = true;
                user_text.text = "";
                plan_title.text = "";
                post_image_byte.Clear();
                capturebtn1.gameObject.GetComponent<Image>().sprite = default_sprite;
                formData.Clear();
            }
        }
    }

    IEnumerator PostThisToGameFeed()
    {
        string HittingUrl = Mainurl + PostGamefeedApi;
        PostImagesGameFeedModel PostModel = new PostImagesGameFeedModel();
        PostModel.id_user = PlayerPrefs.GetInt("UID");
        PostModel.id_org = PlayerPrefs.GetInt("OID");
        PostModel.id_data = 0;
        PostModel.id_DIY = 0;
        PostModel.id_tag_photo = id_tag_photo;
        PostModel.feed_type = 2;

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

                //MasterTabelResponse masterRes = Newtonsoft.Json.JsonConvert.DeserializeObject<MasterTabelResponse>(Request.downloadHandler.text);

            }
            else
            {
                Debug.Log(Request.downloadHandler.text);

                Debug.Log("data not done" + Request.error);
            }

        }
    }

}
