using m2ostnextservice.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Linq;

public class PercentileRangeLb : MonoBehaviour
{
    private List<GameObject> RowList = new List<GameObject>();
    public GameObject Rowprefeb;
    public Transform Rowhandler;
    public HomePageCardSection homepage;
    void Start()
    {
        
    }


    private void OnEnable()
    {
        
        if(RowList.Count == 0)
        {
            StartCoroutine(GenerateBoard());
        }
    }
  
    IEnumerator GenerateBoard()
    {
        yield return new WaitForSeconds(0.2f);
        string Objectname = this.gameObject.name;
        string[] Values = Objectname.Split("/"[0]);
        Debug.Log(" object name " + Objectname + " " + Values[0] + " " + Values[1]);
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.OverallLeaderBoardApi}?Min_Percentile={Values[0]}" +
            $"&Max_Percentile={Values[1]}&OrgID={PlayerPrefs.GetInt("OID")}";
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
                        List<OverallLBModel> boardData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OverallLBModel>>(encrypted);
                        for(int a = 0; a < boardData.Count; a++)
                        {
                            GameObject gb = Instantiate(Rowprefeb, Rowhandler, false);
                            if(boardData[a].UserId == PlayerPrefs.GetInt("UID"))
                            {
                                if(gb.transform.GetSiblingIndex() > 2)
                                {
                                    gb.transform.SetSiblingIndex(2);
                                }
                               
                            }
                            RowList.Add(gb);
                            gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = (a + 1).ToString();
                            gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = boardData[a].Name;
                            gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = boardData[a].score.ToString();
                            gb.transform.GetChild(4).transform.GetChild(0).gameObject.GetComponent<Text>().text = boardData[a].Percentiles.ToString() + "th";
                            int index = homepage.avatarSprite.FindIndex(x => x.name.Equals(boardData[a].Id_Avatar.ToString()));
                            gb.transform.GetChild(1).transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = homepage.avatarSprite[index];
                        
                        }
                    }
                    else
                    {
                        Debug.Log("daat " + Request.downloadHandler.text);
                    }
                }
                else
                {
                    Debug.Log("daat " + Request.downloadHandler.text);
                }
            }
            else
            {
                Debug.Log("daat " + Request.downloadHandler.text);
            }
        }
    }
}
