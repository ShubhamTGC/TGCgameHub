using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using UnityEngine.Networking;
using System;
using UnityEngine.EventSystems;

public class GameFeedPage : MonoBehaviour
{
    public Color LikeColore, NotLikeColor;
    public string MainUrl, GetUserProfileApi, PostGenericPicApi,getFeedPageApi, LikeThePostApi, CommentOnPostApi, PostGamefeedApi,RateOnPostApi;
    private int id_post_image;
    public Text Username, Schoolname, Grade,Stage,StageIndex;
    public List<Sprite> BoyFace, Girlface;
    public Image DP_B, DP_G;
    public Sprite defaultimage;
    private string selectedImgPath;
    [SerializeField] private int sizeValue;
    public Image SelectImgPreview;
    public GameObject PreviewPage;
    private byte[] imageBytes;
    private Texture2D test;
    private Texture2D tex;
    public Text ShareImageStatus;
    public GameObject MsgPanel;

    [Header("Game Feed Objects")]
    public GameObject FeedPrefeb;
    public GameObject ImagePrefeb,CommentBoxPrefeb;
    public Transform FeedTransform;
    private List<GameObject> FeedBars;
    private List<byte[]> post_image_byte = new List<byte[]>(1);
    public GameObject UserImagePrefeb;
    private Transform Parentobj;
    [SerializeField]
    private List<string> ImageUsrls;
    [SerializeField]
    private List<Transform> ImageParenets;
    private List<string> TitleName, Descriptiondata;
    public GameObject GreengeneralPage;

    public List<Sprite> OtherUserBoy,OtherUserGirl;
    public Sprite Rated, Notrated;
    public GameObject Activitymsg;
    public Text MsgBox;
    public Text InterNetTxt;
    private int ImagePrefebNum;
    private int Id_diy, Id_actionplan;
    private List<GameObject> ImageObject = new List<GameObject>();
    public Sprite Male, female;
    public GameObject DummyPic;
    public Button crossbtn;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        MakePage();

    }

    void MakePage()
    {
        if (PlayerPrefs.GetString("Role").Equals("Teacher", System.StringComparison.OrdinalIgnoreCase) ||
            PlayerPrefs.GetString("Role").Equals("Parent", System.StringComparison.OrdinalIgnoreCase))
        {
            DummyPic.GetComponent<Image>().sprite = PlayerPrefs.GetString("gender").Equals("m", System.StringComparison.OrdinalIgnoreCase) ? Male : female;
        }
        ImageUsrls = new List<string>();
        TitleName = new List<string>();
        Descriptiondata = new List<string>();
        ImageParenets = new List<Transform>();
        FeedBars = new List<GameObject>();
        crossbtn.interactable = false;
        StartCoroutine(GetPrfoile());
        StartCoroutine(GenerateFeedPage());
    }

    void SetUserPic(int avatar_type)
    {
        string Gender = PlayerPrefs.GetString("gender");
        if (Gender.Equals("M", System.StringComparison.OrdinalIgnoreCase))
        {
            DP_B.gameObject.SetActive(true);
            DP_G.gameObject.SetActive(false);
            DP_B.sprite = BoyFace[avatar_type];
        }
        else
        {
            DP_G.gameObject.SetActive(true);
            DP_B.gameObject.SetActive(false);
            DP_G.sprite = Girlface[avatar_type];
        }
    }

   IEnumerator GetPrfoile()
    {
        string HittingUrl = MainUrl + GetUserProfileApi + "?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID");
        WWW profile_www = new WWW(HittingUrl);
        yield return profile_www;
        if(profile_www.text != null)
        {
            GFuserprofile GotUserprofile = Newtonsoft.Json.JsonConvert.DeserializeObject<GFuserprofile>(profile_www.text);
            SetUserPic(GotUserprofile.avatar_type);
            Username.text = GotUserprofile.user_name;
            Schoolname.text =GotUserprofile.school != null ? GotUserprofile.school.ToString() : "---";
            Grade.text =  GotUserprofile.Grade != null ? GotUserprofile.Grade.ToString() : "---";
            Stage.text =  GotUserprofile.stage != null ? GotUserprofile.stage.ToString() : "---";
            StageIndex.text =  GotUserprofile.stage_index != null ? "Stage " + GotUserprofile.stage_index.ToString() : "---";
            
        }
    }



    
    public void CanelImage()
    {
        SelectImgPreview.sprite = null;
        imageBytes = null;
        tex = null;
        post_image_byte.Clear();
        PreviewPage.SetActive(false);
    }

    // METHODS FOR IMAGE SELECTION FOR MOBILE AND LAPTOP DEVICE 
    public void ImageSelection()
    {
       
        if (Application.platform == RuntimePlatform.Android)
        {
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) => {

                if (path != null)
                {
                    selectedImgPath = path;
                    Texture2D texture = NativeGallery.LoadImageAtPath(path,1024,false);
                    if (texture != null)
                    {
                       // imageBytes = texture.EncodeToPNG();
                        texture.name = "test.png";
                        test = texture;
                        tex = texture;
                        Sprite ImageSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                        SelectImgPreview.sprite = ImageSprite;
                        PreviewPage.SetActive(true);
                    }
                }


            }, "Select Your Image", "image/jpeg");
        }
        
        
    }

    //SENDING METHOD OF SELECTED IMAGES
    public void SendImage()
    {
        StartCoroutine(PostgenericImage());
    }


    IEnumerator PostgenericImage()
    {
        //Texture2D newScreenshot = ScaleTexture(tex, 256, 256);
        byte[] bytes = tex.EncodeToPNG();
        post_image_byte.Add(bytes);
        Debug.Log("image size is" + post_image_byte[0].Length);
        yield return new WaitForSeconds(0.5f);
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
            yield return new WaitForSeconds(3f);
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
                ShareImageStatus.text = "Your Diy Shared successfully to Game Feed Page!!";
                MsgPanel.SetActive(true);
                formData.Clear();
                post_image_byte.Clear();
                imageBytes = null;
                tex = null;
                yield return new WaitForSeconds(3f);
                iTween.ScaleTo(MsgPanel, Vector3.zero, 0.3f);
                yield return new WaitForSeconds(0.4f);
                MsgPanel.SetActive(false);
                SelectImgPreview.sprite = null;
                PreviewPage.SetActive(false);
            }
            else
            {
                ShareImageStatus.text = "Something went wrong, Please try later!!";
                MsgPanel.SetActive(true);
                formData.Clear();
                post_image_byte.Clear();
                imageBytes = null;
                tex = null;
                yield return new WaitForSeconds(3f);
                iTween.ScaleTo(MsgPanel, Vector3.zero, 0.3f);
                yield return new WaitForSeconds(0.4f);
                MsgPanel.SetActive(false);
                SelectImgPreview.sprite = null;
                PreviewPage.SetActive(false);
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


    IEnumerator GenerateFeedPage()
    {

        int feedCounter = 0;
        ImagePrefebNum = 0;
        string HittingUrl = MainUrl + getFeedPageApi + "?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID");
        WWW GameFeeddata = new WWW(HittingUrl);
        yield return GameFeeddata;
        if(GameFeeddata.text != null)
        {
           
            Debug.Log("Gamefeed log " + GameFeeddata.text);
            List<FeedPostModel> feedPosts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FeedPostModel>>(GameFeeddata.text);
            feedPosts = feedPosts.OrderByDescending(x =>  x.id_log).ToList();
            Debug.Log("Gamefeed log " + GameFeeddata.text);
            for(int a = 0; a < feedPosts.Count; a++)
            {
                GameObject feed = Instantiate(FeedPrefeb, FeedTransform, false);
                FeedBars.Add(feed);
                //feed.SetActive(true);            
            }
            UnityEngine.Object[] UiObjects = GameObject.FindObjectsOfType(typeof(Transform));
            feedPosts.ForEach(x =>
            {
                string url = "";
               
                if(x.Gender != null)
                {
                    if (x.Gender.Equals("m", System.StringComparison.OrdinalIgnoreCase))
                    {
                        FeedBars[feedCounter].transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = OtherUserBoy[x.avatar_type];
                        FeedBars[feedCounter].transform.GetChild(0).transform.GetChild(6).transform.GetChild(2).gameObject.GetComponent<Image>().sprite = OtherUserBoy[x.avatar_type];
                    }
                    else
                    {
                        FeedBars[feedCounter].transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = OtherUserGirl[x.avatar_type];
                        FeedBars[feedCounter].transform.GetChild(0).transform.GetChild(6).transform.GetChild(2).gameObject.GetComponent<Image>().sprite = OtherUserGirl[x.avatar_type];
                    }
                }
               
                FeedBars[feedCounter].transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = x.user_name;
                FeedBars[feedCounter].transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Grade " + x.Grade;
                FeedBars[feedCounter].name = x.id_log.ToString();
                Parentobj = FeedBars[feedCounter].transform.GetChild(0).transform.GetChild(5).gameObject.transform;
                FeedBars[feedCounter].transform.GetChild(0).transform.GetChild(10).transform.GetChild(1)
                .transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<Text>().text = x.like_count.ToString();

                FeedBars[feedCounter].transform.GetChild(0).transform.GetChild(10).transform.GetChild(1).gameObject.GetComponent<Text>().text = x.Comments.Count.ToString();
                var parenttransform = FeedBars[feedCounter].transform.GetChild(0).transform.GetChild(11).transform.GetChild(0).transform.GetChild(0).transform;
                x.Comments.ForEach(y =>
                {
                    GameObject gb = Instantiate(CommentBoxPrefeb, parenttransform, false);
                    gb.GetComponent<Text>().text = y.FIRSTNAME + " :-\n" + y.comment;
                });
                FeedBars[feedCounter].transform.GetChild(0).gameObject.transform.GetChild(8).transform.GetChild(1).gameObject.GetComponent<Text>().text = x.bonus_points.ToString();
                if (x.average_rating != 0)
                {
                    int ratings = FeedBars[feedCounter].transform.GetChild(0).gameObject.transform.GetChild(9).transform.childCount;
                    GameObject ratingObj = FeedBars[feedCounter].transform.GetChild(0).gameObject.transform.GetChild(9).gameObject;
                    for (int c = 1; c < x.average_rating; c++)
                    {
                        ratingObj.transform.GetChild(c-1).gameObject.GetComponent<Image>().sprite = Rated;
                    }
                }


                GameObject ratingParent = FeedBars[feedCounter].transform.GetChild(0).transform.GetChild(9).transform.gameObject;
                GameObject LikeButton = FeedBars[feedCounter].transform.GetChild(0).transform.GetChild(10).transform.GetChild(1).transform.GetChild(0).gameObject;
                LikeButton.transform.GetChild(0).gameObject.GetComponent<Text>().color = x.is_liked == 1 ? LikeColore : NotLikeColor;
                //Comments Parents and Buttons for Comments functionality
                GameObject CommentInput = FeedBars[feedCounter].transform.GetChild(0).transform.GetChild(6).gameObject;
                GameObject MainParentObj = FeedBars[feedCounter].gameObject;
                Button Bt = FeedBars[feedCounter].transform.GetChild(0).transform.GetChild(6).transform.GetChild(3).gameObject.GetComponent<Button>();
                Bt.onClick.RemoveAllListeners();
                Bt.onClick.AddListener(delegate { CommentOnPost(CommentInput, MainParentObj); });
                LikeButton.GetComponent<Button>().onClick.RemoveAllListeners();
                LikeButton.GetComponent<Button>().onClick.AddListener(delegate { LikeFeedPost(MainParentObj, x.is_liked,LikeButton); });
                GameObject PostType = FeedBars[feedCounter].transform.GetChild(0).gameObject.transform.GetChild(3).gameObject;
                if (x.general_data != null)
                {
                    ratingParent.SetActive(false);
                    PostType.GetComponent<Text>().text = "Generic Post";
                    url = x.general_data.image + ".png";
                    ImageUsrls.Add(url);
                    ImageParenets.Add(Parentobj);
                    TitleName.Add("");
                    Descriptiondata.Add("");
                }
                else if (x.DIYLog != null)
                {
                    ratingParent.SetActive(true);
                    PostType.GetComponent<Text>().text = "DIY";
                    url = x.DIYLog.photo_filename;
                    ImageUsrls.Add(url);
                    ImageParenets.Add(Parentobj);
                    Id_diy = x.DIYLog.id_log;
                    if (x.DIYLog.detail_info.Length >1)
                    {
                        string details = x.DIYLog.detail_info;
                        string[] Data = details.Split("/"[0]);
                        TitleName.Add(Data[0]);
                        Descriptiondata.Add(Data[1]);
                    }
                    else
                    {
                        TitleName.Add("");
                        Descriptiondata.Add("");
                    }
                }
                else if(x.TagLog != null)
                {
                    ratingParent.SetActive(true);
                    PostType.GetComponent<Text>().text = "Action Plan";
                    url = x.TagLog.photo_filename;
                    ImageUsrls.Add(url);
                    ImageParenets.Add(Parentobj);
                    TitleName.Add(x.TagLog.key_info);
                    Descriptiondata.Add(x.TagLog.detail_info);
                    Id_actionplan = x.TagLog.id_tag_photo;
                }

           
                ratingButtonSetup(ratingParent, x.average_rating, FeedBars[feedCounter].name,Id_diy,Id_actionplan);
                feedCounter++;
                
            });

            for (int a = 0; a < ImageUsrls.Count; a++)
            {
                StartCoroutine(GetUserPostImage(ImageParenets[a], ImageUsrls[a], TitleName[a], Descriptiondata[a]));
                yield return new WaitForSeconds(0.1f);
            }
            crossbtn.interactable = true;
        }
        else
        {
            crossbtn.interactable = true;
            InterNetTxt.gameObject.SetActive(true);
            InterNetTxt.text = "Poor internet Connectivity.";
            yield return new WaitForSeconds(3f);
            InterNetTxt.gameObject.SetActive(false);
            InterNetTxt.text = "";
        }     
    }


    void ratingButtonSetup(GameObject ratingParent,int ratingvalue,string Id,int idDiy,int idActionPlan)
    {
        int childcount = ratingParent.transform.childCount;
        List<GameObject> ratinglist = new List<GameObject>();
        for(int a = 0; a < childcount; a++)
        {
            ratinglist.Add(ratingParent.transform.GetChild(a).gameObject);

        }
        for(int b =0;b < ratingvalue; b++)
        {
            ratinglist[b].GetComponent<Image>().sprite = Rated;
        }
        ratinglist.ForEach(x =>
        {
            x.GetComponent<Button>().onClick.RemoveAllListeners();
            x.GetComponent<Button>().onClick.AddListener(delegate { Rating(ratinglist, Id, idDiy, idActionPlan); });
        });
        
    }

    public void Rating(List<GameObject> ratinglist,string id,int idDiy,int idActionPlan)
    {
        int ButtonNum = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        for(int a = 0; a < ratinglist.Count; a++)
        {
            ratinglist[a].GetComponent<Image>().sprite = a < ButtonNum ? Rated : Notrated; 
        }
        StartCoroutine(PostRating(ButtonNum, idDiy, idActionPlan));
    }

    IEnumerator PostRating(int rating,int diy_id,int actionplan_id)
    {
        yield return new WaitForSeconds(1f);
        string HittingUrl = $"{MainUrl}{RateOnPostApi}";
        PostRatingModel Ratinglog = new PostRatingModel
        {
            id_user = PlayerPrefs.GetInt("UID"),
            id_rating = 0,
            id_diy = diy_id,
            id_tag = actionplan_id,
            rating = rating
        };

        string Logdata = Newtonsoft.Json.JsonConvert.SerializeObject(Ratinglog);
        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, Logdata))
        {
            Request.method = UnityWebRequest.kHttpVerbPOST;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                Debug.Log(Request.downloadHandler.text);
                RatingPostLogModel log = Newtonsoft.Json.JsonConvert.DeserializeObject<RatingPostLogModel>(Request.downloadHandler.text);
                if (log.Status.Equals("success", System.StringComparison.OrdinalIgnoreCase))
                {
                    string msg = log.Message;
                    StartCoroutine(ShowMsg(msg));
                }
                else
                {
                    string msg = log.Message;
                    StartCoroutine(ShowMsg(msg));
                }
            }
            else
            {
                string msg = "Check Your internet connection";
                StartCoroutine(ShowMsg(msg));
            }
        }
    }

    IEnumerator GetUserPostImage(Transform Parent,string Url,string Titlename,String Description )
    {
        GameObject gb = Instantiate(UserImagePrefeb, Parent, false);
        ImageObject.Add(gb);
        gb.name = "UserPic" + ImagePrefebNum;
        gb.GetComponent<BoxCollider2D>().enabled = true;
        gb.GetComponent<ImageLargerScript>().canvas = this.gameObject.transform;
        ImagePrefebNum++;
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(Url, true);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            try
            {
                Texture2D texture2d = new Texture2D(1, 1);
                Sprite sprite = null;
                if (www.isDone)
                {
                    if (texture2d.LoadImage(www.downloadHandler.data))
                    {
                        //Debug.Log(" Image size " + texture2d.height + " " + texture2d.width);
                        if (texture2d.height <= 12 && texture2d.width <= 12)
                        {
                            Debug.Log("missing");
                        }
                        else
                        {
                            sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
                        }
                        //sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
                    }
                }
                   

                if (sprite != null)
                {
                    
                    gb.GetComponent<Image>().sprite = sprite;
                    if(Titlename != null && Description != null)
                    {
                        gb.transform.GetChild(0).gameObject.GetComponent<Text>().text = Titlename;
                        gb.transform.GetChild(1).gameObject.GetComponent<Text>().text = Description;

                    }
                }
            }
            catch(Exception e)
            {
                Debug.Log(e);
            }
        }
    }


    public void LikeFeedPost( GameObject Parentname,int likeddata,GameObject LikeButton)
    {
        if(likeddata != 1)
        {
            //Debug.Log(" parenr objevt for like " + Parentname.name);
            int Id_log = int.Parse(Parentname.name.ToString());
            StartCoroutine(PostLikeTask(Id_log));
            LikeButton.transform.GetChild(0).gameObject.GetComponent<Text>().color =  LikeColore;
        }
    }

    IEnumerator PostLikeTask(int id_log)
    {
        string HittingUrl = MainUrl + LikeThePostApi;
        LikePostModel LikeModel = new LikePostModel();
        LikeModel.id_log = id_log;
        LikeModel.id_user = PlayerPrefs.GetInt("UID");
        LikeModel.id_like = 1;

        string data_log = Newtonsoft.Json.JsonConvert.SerializeObject(LikeModel);

        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, data_log))
        {
            Request.method = UnityWebRequest.kHttpVerbPOST;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                Debug.Log(Request.downloadHandler.text);
                LikeResponseModel likeRes = Newtonsoft.Json.JsonConvert.DeserializeObject<LikeResponseModel>(Request.downloadHandler.text);
                if (likeRes.Status.Equals("success", System.StringComparison.OrdinalIgnoreCase))
                {
                    string msg = "You have like the post.";
                    StartCoroutine(ShowMsg(msg));
                }
            }
            else
            {
                string msg = "Check Your internet connection";
                StartCoroutine(ShowMsg(msg));
            }
        }
    }

    IEnumerator ShowMsg(string msg)
    {
        MsgBox.text = msg;
        Activitymsg.SetActive(true);
        yield return new WaitForSeconds(3f);
        iTween.ScaleTo(Activitymsg, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.4f);
        MsgBox.text = "";
        Activitymsg.SetActive(false);
    }


    public void CommentOnPost(GameObject UserText,GameObject parentname)
    {
        string Usercomment = UserText.GetComponent<InputField>().text;
        UserText.GetComponent<InputField>().text = "";
        int id_log = int.Parse(parentname.name.ToString());
        StartCoroutine(commenttask(Usercomment,id_log));
    }

    IEnumerator commenttask(string Msg,int id_log)
    {
        string HittingUrl = MainUrl + CommentOnPostApi;
        CommentOnPostModel CommentPost = new CommentOnPostModel();
        CommentPost.id_comment = 1;
        CommentPost.id_log = id_log;
        CommentPost.id_user = PlayerPrefs.GetInt("UID");
        CommentPost.comment = Msg;

        string Data_log = Newtonsoft.Json.JsonConvert.SerializeObject(CommentPost);

        using (UnityWebRequest comment_req = UnityWebRequest.Put(HittingUrl, Data_log))
        {
            comment_req.method = UnityWebRequest.kHttpVerbPOST;
            comment_req.SetRequestHeader("Content-Type", "application/json");
            comment_req.SetRequestHeader("Accept", "application/json");
            yield return comment_req.SendWebRequest();
            if (!comment_req.isNetworkError && !comment_req.isHttpError)
            {
                Debug.Log(comment_req.downloadHandler.text);

                LikeResponseModel likeRes = Newtonsoft.Json.JsonConvert.DeserializeObject<LikeResponseModel>(comment_req.downloadHandler.text);
                if (likeRes.Status.Equals("success", System.StringComparison.OrdinalIgnoreCase))
                {
                    string msg = "You have commented on the post.";
                    StartCoroutine(ShowMsg(msg));
                }

            }
            else
            {
                string msg = "Check Your internet connection";
                StartCoroutine(ShowMsg(msg));
            }
        }

        yield return new WaitForSeconds(0);
    }


    public void CloseGameFeed()
    {
        StartCoroutine(ClosureTask());
    }

    IEnumerator ClosureTask()
    {
        ImagePrefebNum = 0;
        int count = FeedTransform.childCount;
        ImageObject.ForEach(x =>
        {
            x.GetComponent<BoxCollider2D>().enabled = false;
        });
        ImageObject.Clear();
        for (int a = 0; a < count; a++)
        {
            Destroy(FeedTransform.GetChild(a).gameObject, 0.05f);
        }
        yield return new WaitForSeconds(0.5f);
        if(FeedBars.Count > 0)
        {
            FeedBars.Clear();
        }
        GreengeneralPage.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void RefreshPage()
    {
        ImageObject.ForEach(x =>
        {
            x.GetComponent<BoxCollider2D>().enabled = false;
        });
        int count = FeedTransform.childCount;
        for (int a = 0; a < count; a++)
        {
            Destroy(FeedTransform.GetChild(a).gameObject, 0.05f);
        }
        if (FeedBars.Count > 0)
        {
            FeedBars.Clear();
        }
        ImageObject.Clear();
        MakePage();
    }
}
