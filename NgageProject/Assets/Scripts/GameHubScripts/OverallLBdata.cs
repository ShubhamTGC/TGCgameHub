using m2ostnextservice.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverallLBdata : MonoBehaviour
{
    // Start is called before the first frame update
    public HomePageCardSection Homepage;
    public GameObject RowPrefeb;
    public Transform Rowhandler;
    private List<GameObject> Datarows = new List<GameObject>();
    public List<Text> Username;
    public List<Text> Score;
    public List<Image> AvatarImage;
    public Sprite defaultImage;


    void Start()
    {
        
    }

    private void OnEnable()
    {
        if(Datarows.Count == 0)
        {
            StartCoroutine(GetOverallLeaderdata());
        }
    }


    IEnumerator GetOverallLeaderdata()
    {
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.OverallLeaderBoardApi}";
        WWW request = new WWW(HittingUrl);
        yield return request;
        if(request.text != null)
        {
            if(request.text != "")
            {
                AESAlgorithm aes = new AESAlgorithm();
                string initialResponse = request.text.TrimStart('"').TrimEnd('"');
                string decrypteddata = aes.getDecryptedString(initialResponse);
                Debug.Log("LeaderBoard Log " + decrypteddata);
                List<OverallLBModel> boardData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OverallLBModel>>(decrypteddata);
                int index = 0;
                boardData.ForEach(x =>
                {
                    if(index < 3)
                    {
                        Username[index].text = x.Name;
                        Score[index].text = x.score.ToString();
                        Debug.Log("null avatar id " + x.Id_Avatar);
                        if(x.Id_Avatar > 0)
                        {
                            Homepage.avatarSprite.ForEach(a =>
                            {
                                if (a.name.Equals(x.Id_Avatar.ToString(), System.StringComparison.OrdinalIgnoreCase))
                                {
                                    AvatarImage[index].gameObject.SetActive(true);
                                    AvatarImage[index].sprite = a;
                                }
                            });
                        }
                        else
                        {
                            AvatarImage[index].gameObject.SetActive(true);
                            AvatarImage[index].sprite = defaultImage;
                        }
                     
                       
                    }
                    else
                    {
                        GameObject gb = Instantiate(RowPrefeb, Rowhandler, false);
                        Datarows.Add(gb);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = x.Rank.ToString();
                        gb.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = x.Name;
                        gb.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.GetComponent<Text>().text = x.score.ToString();
                        Debug.Log("null avatar id " + x.Id_Avatar);
                        if (x.Id_Avatar > 0)
                        {
                            Homepage.avatarSprite.ForEach(a =>
                            {
                                if (a.name.Equals(x.Id_Avatar.ToString(), System.StringComparison.OrdinalIgnoreCase))
                                {
                                    gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                                    gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = a;
                                }
                            });
                        }
                        else
                        {
                            gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = defaultImage;
                        }
                    }
                    index +=1;
                });
            }
        }
    }

    public void ResetLeaderBoard()
    {
        Datarows.Clear();
        int count = Rowhandler.transform.childCount;
        for (int a=0; a < count; a++)
        {
            Destroy(Rowhandler.transform.GetChild(a).gameObject);
        }
    }
}
