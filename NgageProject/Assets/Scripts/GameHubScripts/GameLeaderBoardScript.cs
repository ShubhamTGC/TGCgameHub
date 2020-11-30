using m2ostnextservice.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLeaderBoardScript : MonoBehaviour
{
    //public HomePageCardSection Homepage;
    public GameObject RowPrefeb;
    public Transform Rowhandler;
    private List<GameObject> Datarows = new List<GameObject>();
    public List<Text> Username;
    public List<Text> Score;
    public List<Image> AvatarImage;
     [HideInInspector]public List<Sprite> AvatarModel = new List<Sprite>();
    [SerializeField]private int currentgameId;
    void Start()
    {

    }

    private void OnEnable()
    {
       
       // Homepage = HomePageCardSection.;
        if (Datarows.Count == 0)
        {
            StartCoroutine(GetGameDetails());
        }
    }


    IEnumerator GetGameDetails()
    {
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.GameLeaderboardApi}?GID={currentgameId}&Min_Percentile=1" +
            $"&Max_Percentile=100&OrgID={PlayerPrefs.GetInt("OID")}";
        WWW Request = new WWW(HittingUrl);
        yield return Request;
        if (Request.text != null)
        {
            if (Request.text != "")
            {
                AESAlgorithm aes = new AESAlgorithm();
                string response = Request.text.TrimStart('"').TrimEnd('"');
                string Decryptedlog = aes.getDecryptedString(response);
                List<OverallLBModel> boardData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OverallLBModel>>(Decryptedlog);
                int index = 0;
                boardData.ForEach(x =>
                {
                    if (index < 3)
                    {
                        Username[index].text = x.Name;
                        Score[index].text = x.score.ToString();
                        AvatarImage[index].gameObject.SetActive(true);
                        AvatarImage[index].sprite = AvatarModel[index];
                    }
                    else
                    {
                        GameObject gb = Instantiate(RowPrefeb, Rowhandler, false);
                        Datarows.Add(gb);
                        gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = x.Rank.ToString();
                        gb.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = x.Name;
                        gb.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.GetComponent<Text>().text = x.score.ToString();
                    }


                    index++;
                });
            }
        }


    }
}
