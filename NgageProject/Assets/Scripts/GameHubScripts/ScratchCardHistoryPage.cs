using m2ostnextservice.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.UI;

public class ScratchCardHistoryPage : MonoBehaviour
{
    public GameObject CardPrefeb,CardPanelPrefeb;
    public Transform CardHandler,MainObj;
    private List<GameObject> CardList = new List<GameObject>();
    public GameObject viewpage;
    public GameObject Msg;
    public GameObject ErrorPage;
    public Text MSgbox;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if(CardList.Count == 0)
        {
            StartCoroutine(Generatedcardpage());
        }
    }

    IEnumerator Generatedcardpage()
    {
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.GetUserCards}?OrgID={PlayerPrefs.GetInt("OID")}&UID={PlayerPrefs.GetInt("UID")}";
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
                    Msg.SetActive(false);
                    string response = Request.downloadHandler.text.TrimStart('"').TrimEnd('"');
                    AESAlgorithm aes = new AESAlgorithm();
                    string encrypted = aes.getDecryptedString(response);
                    if(encrypted != "[]")
                    {
                        List<ScratchCardHistoryModel> historyModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ScratchCardHistoryModel>>(encrypted);
                        historyModel = historyModel.OrderByDescending(a => a.id_log_scratchcard).ToList();
                        historyModel.ForEach(x =>
                        {
                            GameObject gb = Instantiate(CardPrefeb, CardHandler, false);
                            gb.name = x.id_log_scratchcard.ToString();
                            if (x.scratch_point == 0)
                            {
                                gb.transform.GetChild(0).gameObject.SetActive(true);
                                gb.transform.GetChild(0).GetComponent<Text>().text = x.scratchcard;
                                gb.transform.GetChild(1).gameObject.SetActive(false);

                            }
                            else
                            {
                                gb.transform.GetChild(0).gameObject.SetActive(false);
                                gb.transform.GetChild(1).GetComponent<Text>().text = "You won " + x.scratch_point.ToString() + " points.";
                                gb.transform.GetChild(1).gameObject.SetActive(true);
                            }

                            gb.transform.GetChild(2).gameObject.SetActive(x.IsScratch == 0 ? true : false);
                            gb.GetComponent<Button>().enabled = x.IsScratch == 0 ? true : false;
                            gb.GetComponent<Button>().onClick.RemoveAllListeners();
                            gb.GetComponent<Button>().onClick.AddListener(delegate { CardCreator(x.id_log_scratchcard.ToString(), x.scratch_point); });
                            CardList.Add(gb);
                        });
                    }
                    else
                    {
                        Msg.SetActive(true);
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

    void CardCreator(string name,int point)
    {
        Debug.Log(" created ");
        GameObject Panel = Instantiate(CardPanelPrefeb, MainObj, false);
        Panel.GetComponent<CardHistoryController>().viewpage = viewpage;
        Panel.transform.GetChild(2).GetComponent<ScratchCardHistoryHandler>().Cardmainepage = this.gameObject.GetComponent<ScratchCardHistoryPage>();
        Panel.name = name;
        if(point == 0)
        {
            Panel.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            Panel.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(true);
            Panel.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = "Better luck next time!";
        }
        else
        {
            Panel.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.SetActive(false);
            Panel.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Panel.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "You won " + point.ToString() + " points.";
        }
        viewpage.SetActive(false);
        Panel.SetActive(true);
    }

    public void CardSractched(int cardId)
    {
        CardList.ForEach(x =>
        {
            if (x.name.Equals(cardId.ToString()))
            {
                x.transform.GetChild(2).gameObject.SetActive(false);
                x.GetComponent<Button>().enabled = false;
            }
        });
        viewpage.SetActive(true);
    }


    public void ResetPage()
    {
        int count = CardHandler.transform.childCount;
        for(int a = 0; a < count; a++)
        {
            Destroy(CardHandler.transform.GetChild(a).gameObject);
        }
        CardList.Clear();
    }
    public void ShowErrorBox(string msg)
    {
        MSgbox.text = msg;
        ErrorPage.SetActive(true);
    }

}
