using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using SFB;
using System.IO;
using System;
using UnityEngine.Networking;

public class FeedbackpageHandler : MonoBehaviour
{
    public GameObject FeedbackPage;
    public string MainUrl, FeedBackPostApi;
    public InputField UserText, UserEmailId;
    public Button Send;
    public List<Toggle> FeedbackOptions;
    private int FeedBackType;
    private string selectedImgPath;
    private Texture2D test, tex;
    public GameObject ImagePreview;
    public Transform PreviewHandler;
    public GameObject TextForImage;
    public RawImage LargePreview;
    private List<byte[]> post_image_byte = new List<byte[]>(1);
    public Text ShareImageStatus;
    public GameObject Msgpanel;
    private byte[] Without_image;
    public Sprite defaultimage;
    private string UseremailId;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        FeedbackPage.SetActive(true);
        UserEmailId.text = PlayerPrefs.GetString("Email");
        Without_image = new byte[1];
        test = new Texture2D(128, 128, TextureFormat.RGB24, true);
        test.name = "test.png";
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    public void GetUserdata()
    {
        if (UserEmailId.text == "" || UserText.text == "")
        {
            string msg = "Please fill required information";
            StartCoroutine(ShowPopupTask(msg));

        }
        else
        {
            for (int a = 0; a < FeedbackOptions.Count; a++)
            {
                if (FeedbackOptions[a].isOn == true)
                {
                    FeedBackType = a + 1;
                }
            }
            Send.interactable = false;
            UseremailId = UserEmailId.text;
            Debug.Log(" Users email id " + UseremailId);
            StartCoroutine(PostFeedBackdata());
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


    public void ImageSelection()
    {
        int count = PreviewHandler.transform.childCount;
        if(count > 0)
        {
            for(int a = 0; a < count; a++)
            {
                Destroy(PreviewHandler.transform.GetChild(a).gameObject);
            }
        }
        StartCoroutine(GetImageTask());
       
    }
    IEnumerator GetImageTask()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) => {

                if (path != null)
                {
                    selectedImgPath = path;
                    Texture2D texture = NativeGallery.LoadImageAtPath(path, 1024, false);
                    if (texture != null)
                    {
                        byte[] bytes;
                        texture.name = "test.png";
                        test = texture;
                        tex = texture;
                        bytes = tex.EncodeToPNG();
                        post_image_byte.Add(bytes);
                        Sprite ImageSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                        GameObject gb = Instantiate(ImagePreview, PreviewHandler, true);
                        gb.GetComponent<Image>().sprite = ImageSprite;
                        gb.name = "preview";
                        gb.SetActive(true);
                        gb.GetComponent<ShowimagePreview>().PreviewTexture = texture;
                        gb.GetComponent<ShowimagePreview>().canvas = this.gameObject.transform;
                        TextForImage.SetActive(false);
                    }
                }


            }, "Select Your Image", "image/jpeg");
        }
        else
        {
            string[] image_path = StandaloneFileBrowser.OpenFilePanel("Select Image to Upload", "", "jpg", false);
            if (image_path.Length != 0)
            {

                byte[] image_data = File.ReadAllBytes(image_path[0]);
                yield return new WaitForSeconds(0.1f);
                Texture2D texture = new Texture2D(1024, 1024, TextureFormat.RGB24, true);
               
                texture.LoadImage(image_data);
                texture.Apply();
                texture.name = "test.png";
                test = texture;
                Texture2D newScreenshot = ScaleTexture(texture, 256, 256);
                var bytes = newScreenshot.EncodeToPNG();
                post_image_byte.Add(bytes);
                string imagestr = Convert.ToBase64String(bytes);
                Texture2D tex = new Texture2D(1, 1);
                tex.LoadImage(Convert.FromBase64String(imagestr));
                Sprite btn_sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                GameObject gb = Instantiate(ImagePreview, PreviewHandler, true);
                gb.name = "preview";
                gb.GetComponent<BoxCollider2D>().enabled = false;
                gb.GetComponent<Image>().sprite = btn_sprite;
                gb.GetComponent<ShowimagePreview>().PreviewTexture = texture;
                gb.GetComponent<ShowimagePreview>().canvas = this.gameObject.transform;
                gb.GetComponent<BoxCollider2D>().enabled = true;
                gb.SetActive(true);
                TextForImage.SetActive(false);

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

    IEnumerator PostFeedBackdata()
    {
        yield return new WaitForSeconds(0.5f);
        if(post_image_byte.Count == 0)
        {
            Texture2D temptex = defaultimage.texture;
            Without_image = temptex.EncodeToPNG();
            post_image_byte.Add(Without_image);
        }
        
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormFileSection("File", post_image_byte[0], test.name, "image/png"));
        formData.Add(new MultipartFormDataSection("EXTN", "png"));
        formData.Add(new MultipartFormDataSection("UID", PlayerPrefs.GetInt("UID").ToString()));
        formData.Add(new MultipartFormDataSection("FEEDBACK", UserText.text));
        formData.Add(new MultipartFormDataSection("FEEDBACKTYPE", FeedBackType.ToString()));
        formData.Add(new MultipartFormDataSection("EMAIL", UseremailId));

        string post_url = MainUrl + FeedBackPostApi;
        UnityWebRequest www = UnityWebRequest.Post(post_url, formData);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            post_image_byte.Clear();
            formData.Clear();
            string msg = "Failed to share , Try Again!";
            StartCoroutine(ShowPopupTask(msg));

        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            if(www.downloadHandler.text != null)
            {
                FeedbackResponseModel FeedbackLog = Newtonsoft.Json.JsonConvert.DeserializeObject<FeedbackResponseModel>(www.downloadHandler.text);
                if (FeedbackLog.STATUS.Equals("success", System.StringComparison.OrdinalIgnoreCase))
                {
                    string msg = "Thanks for your valuable Feedback!";
                    StartCoroutine(ShowPopupTask(msg));
                    ClearFields();
                }
            }
            else
            {
                string msg = "Something went wrong, Please Try later.";
                StartCoroutine(ShowPopupTask(msg));
            }
          
        }
    }

    void ClearFields()
    {
        UserEmailId.text = PlayerPrefs.GetString("Email");
        UserText.text = "";
        Send.interactable = true;
        int count = PreviewHandler.childCount;
        for(int a = 0; a < count; a++)
        {
            Destroy(PreviewHandler.GetChild(a).gameObject);
        }
    }


    public void FeedbackClose()
    {
        StartCoroutine(ClosingTask());
    }

    IEnumerator ClosingTask()
    {
        iTween.ScaleTo(FeedbackPage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        FeedbackPage.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
