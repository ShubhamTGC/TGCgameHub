using m2ostnextservice.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CouponsPageHandler : MonoBehaviour
{
    public GameObject CouponPrefeb;
    public Transform CouponHandler;
    private List<GameObject> CouponList = new List<GameObject>();
    private Sprite defaultCoupon;
    public GameObject msg;
    public GameObject ErrorPage;
    public Text MSgbox;
    void Start()
    {
        
    }

     void OnEnable()
    {
        if(CouponList.Count == 0)
        {
            StartCoroutine(Coupongeneration());
        }
    }

    IEnumerator Coupongeneration()
    {
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.GetCouponListApi}?UID={PlayerPrefs.GetInt("UID")}";
        using (UnityWebRequest Request = UnityWebRequest.Get(HittingUrl))
        {
            Request.method = UnityWebRequest.kHttpVerbGET;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                if (Request.responseCode.Equals(200))
                {
                    msg.SetActive(false);
                    string response = Request.downloadHandler.text.TrimStart('"').TrimEnd('"');
                    AESAlgorithm aes = new AESAlgorithm();
                    string encrypted = aes.getDecryptedString(response);
                    Debug.Log("coupon list " + encrypted);
                    if(encrypted != "[]")
                    {
                        List<CouponPurchaseModel> model = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CouponPurchaseModel>>(encrypted);
                        model.ForEach(x =>
                        {
                            GameObject gb = Instantiate(CouponPrefeb, CouponHandler, false);
                            GameObject ImageHolder = gb.transform.GetChild(0).transform.GetChild(1).gameObject;
                            StartCoroutine(GetTexture(x.Image, ImageHolder));
                            gb.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>().text = x.PointsUsed.ToString();
                            gb.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Coupon ID: " + x.CouponID.ToString();
                            gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = "Coupon Code: " + x.CouponCode.ToString();
                            gb.transform.GetChild(4).gameObject.GetComponent<Text>().text = "Expiry Date: " + x.ExpiryDate.ToString();
                            gb.transform.GetChild(5).gameObject.GetComponent<Text>().text = "Coupon Name:: " + x.CouponTitle.ToString();
                            gb.transform.GetChild(6).transform.GetChild(0).gameObject.GetComponent<Text>().text = x.CouponDescription;
                            gb.transform.GetChild(7).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                            gb.transform.GetChild(7).gameObject.GetComponent<Button>().onClick.AddListener(delegate { UseCoupon(x.Link); });
                            CouponList.Add(gb);
                        });
                    }
                    else
                    {
                        msg.SetActive(true);
                    }
                }
                else
                {
                    string msg = "Please check you internet connection and try again!";
                    ShowErrorBox(msg);
                }
            }
        }
    }

    //GET COUPON IMAGE
    IEnumerator GetTexture(string Url,GameObject imageholder)
    {
        if (Url != null)
        {
            Debug.Log("running");
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
                            if(texture2d.width > 12 && texture2d.height > 12)
                            {
                                byte[] bytes = texture2d.EncodeToPNG();
                                sprite = GetAvatarSprite(bytes);
                                imageholder.GetComponent<Image>().sprite = sprite;
                            }
                            else
                            {
                                imageholder.GetComponent<Image>().sprite = defaultCoupon;
                            }
                         
                        }
                    }
                    else
                    {
                        Debug.Log("running");
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }
            }
        }

    }

    private Sprite GetAvatarSprite(byte[] imagesbyte)
    {
        if (imagesbyte.Length > 0)
        {
            Texture2D texture2d = new Texture2D(1, 1);
            Sprite sprite;
            texture2d.LoadImage(imagesbyte);
            sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }


    void UseCoupon(string Link)
    {
        Application.OpenURL(Link);
    }

    public void ResetPage()
    {
        int count = CouponHandler.transform.childCount;
        for (int a = 0; a < count; a++)
        {
            Destroy(CouponHandler.transform.GetChild(a).gameObject);
        }
        CouponList.Clear();
    }
    public void ShowErrorBox(string msg)
    {
        MSgbox.text = msg;
        ErrorPage.SetActive(true);
    }
}
