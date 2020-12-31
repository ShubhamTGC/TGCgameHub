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
                    if (Request.downloadHandler.text != null)
                    {
                        string response = Request.downloadHandler.text.TrimStart('"').TrimEnd('"');
                        AESAlgorithm aes = new AESAlgorithm();
                        string encrypted = aes.getDecryptedString(response);
                        //Debug.Log("history data "+ encrypted);
                        List<ScratchCardHistoryModel> historyModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ScratchCardHistoryModel>>(encrypted);
                        historyModel = historyModel.OrderByDescending(a=>a.id_log_scratchcard).ToList();
                        historyModel.ForEach(x =>
                        {
                            GameObject gb = Instantiate(CardPrefeb, CardHandler, false);
                            gb.name = x.id_log_scratchcard.ToString();
                            if(x.scratch_point == 0)
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
                            gb.GetComponent<Button>().onClick.AddListener(delegate { CardCreator(x.id_log_scratchcard.ToString()); });
                            CardList.Add(gb);
                        });
                    }
                }
            }
        }
    }

    void CardCreator(string name)
    {
        GameObject Panel = Instantiate(CardPanelPrefeb, MainObj, false);
        Panel.name = name;
        Panel.SetActive(true);

    }


}
