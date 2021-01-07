using m2ostnextservice.Models;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Globalization;

public class PassBookHandler : MonoBehaviour
{
    public GameObject passbookPrefeb;
    public Transform Cardhandler;
    private List<GameObject> cardList = new List<GameObject>();
    [HideInInspector] public int InitialBalance;
    private int TotalPoints;
    public Button Dates;
    public Sprite Uparrow, DownArrow;
    public GameObject ErrorPage;
    public Text MSgbox;
    //private int FinalPoints;
    void Start()
    {

    }

    private void OnEnable()
    {
        TotalPoints = InitialBalance;
        Dates.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = DownArrow;
        if (cardList.Count == 0)
        {
            StartCoroutine(GetPassBookgenerated());
        }
    }

    IEnumerator GetPassBookgenerated()
    {
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.PassBookApi}?OrgID={PlayerPrefs.GetInt("OID")}&UID={PlayerPrefs.GetInt("UID")}";
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
                    string response = Request.downloadHandler.text.TrimStart('"').TrimEnd('"');
                    AESAlgorithm aes = new AESAlgorithm();
                    string encrypted = aes.getDecryptedString(response);
                    Debug.Log("passbook log " + encrypted);
                    if(encrypted != "[]")
                    {
                        List<GetPassbookModel> model = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetPassbookModel>>(encrypted);
                        model = model.OrderByDescending(x => x.UpdatedDate).ToList();
                        Dates.onClick.RemoveAllListeners();
                        Dates.onClick.AddListener(delegate { AscendingOrder(model); });
                        OrderTask(model, "descending");
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

    void AscendingOrder(List<GetPassbookModel> model)
    {
        Dates.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Uparrow;
        TotalPoints = InitialBalance;
        model = model.OrderBy(x => x.UpdatedDate).ToList();
        OrderTask(model,"ascending");
        Dates.onClick.RemoveAllListeners();
        Dates.onClick.AddListener(delegate { DecendingOrder(model); });
    }

    void DecendingOrder(List<GetPassbookModel> model)
    {
        Dates.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = DownArrow;
        TotalPoints = InitialBalance;
        model = model.OrderByDescending(x => x.UpdatedDate).ToList();
        OrderTask(model, "descending");
        Dates.onClick.RemoveAllListeners();
        Dates.onClick.AddListener(delegate { AscendingOrder(model); });
    }


    void OrderTask(List<GetPassbookModel> model,string order)
    {
        int count = Cardhandler.transform.childCount;
        for(int a = 0; a < count; a++)
        {
            Destroy(Cardhandler.transform.GetChild(a).gameObject);
        }
        cardList.Clear();
        if (order.Equals("descending", System.StringComparison.OrdinalIgnoreCase))
        {
            model.ForEach(x =>
            {
                GameObject gb = Instantiate(passbookPrefeb, Cardhandler, false);
                if (x.Card_type == 2)
                {
                    TotalPoints += x.Credit_Coins.GetValueOrDefault();
                    gb.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = x.UpdatedDate.Date.ToString();
                    gb.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "Scartch Card";
                    gb.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = x.Credit_Coins.ToString();
                    gb.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = "0";
                    gb.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = (TotalPoints).ToString();
                    cardList.Add(gb);
                }
                else
                {

                    gb.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = x.UpdatedDate.Date.ToString();
                    gb.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = x.WebsiteName + " Coupon";
                    gb.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "0";
                    gb.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = x.Debit_Coins.ToString();
                    gb.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = (TotalPoints).ToString();
                    cardList.Add(gb);
                    TotalPoints -= x.Debit_Coins.GetValueOrDefault();
                }

            });
        }
        else
        {
            model.ForEach(x =>
            {
                GameObject gb = Instantiate(passbookPrefeb, Cardhandler, false);
                if (x.Card_type == 2)
                {
                    TotalPoints += x.Credit_Coins.GetValueOrDefault();
                    gb.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = x.UpdatedDate.Date.ToString();
                    gb.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = "Scartch Card";
                    gb.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = x.Credit_Coins.ToString();
                    gb.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = "0";
                    gb.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = (TotalPoints).ToString();
                    cardList.Add(gb);
                }
                else
                {

                    gb.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = x.UpdatedDate.Date.ToString();
                    gb.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = x.WebsiteName + " Coupon";
                    gb.transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "0";
                    gb.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = x.Debit_Coins.ToString();
                    TotalPoints -= x.Debit_Coins.GetValueOrDefault();
                    gb.transform.GetChild(4).transform.GetChild(0).GetComponent<Text>().text = (TotalPoints).ToString();
                    cardList.Add(gb);
                  
                }

            });
        }
       

    }

    public void ResetPage()
    {
        int count = Cardhandler.transform.childCount;
        for (int a = 0; a < count; a++)
        {
            Destroy(Cardhandler.transform.GetChild(a).gameObject);
        }
        cardList.Clear();
    }

    public void OpenWebPage()
    {
        var Scorebytes = System.Text.Encoding.UTF8.GetBytes(PlayerPrefs.GetString("coin"));
        string Encoded = Convert.ToBase64String(Scorebytes);
        string Url = $"{MainUrls.LogicalbaniyaUrl}?clientID=demo&key=demo&userID={PlayerPrefs.GetInt("UID")}&rewards={Encoded}&username={PlayerPrefs.GetString("Username")}&postURL={MainUrls.PostCoupondataApi}&returnURL=https://www.demo-bank.com/&PartnerCode=demo&model=1";
        Application.OpenURL(Url);
    }

    public void ShowErrorBox(string msg)
    {
        MSgbox.text = msg;
        ErrorPage.SetActive(true);
    }

}
