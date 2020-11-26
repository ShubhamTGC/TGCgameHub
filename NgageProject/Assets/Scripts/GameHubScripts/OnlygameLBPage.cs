using m2ostnextservice.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlygameLBPage : MonoBehaviour
{
    public GameLeaderBoardPage GameLB;
    public OverallLBdata OverallBoard;
    public HomePageCardSection Homepage;
    private List<GameObject> databars = new List<GameObject>();
    public GameObject Rowprefeb;
    public Transform Rowhandler;
    public GameObject GameListBar;
    public Text GameNumber, GameName;
    public Image GamePreview;
    public List<Text> Username;
    public List<Text> Score;
    public List<Image> AvatarImage;
    public GameObject gametopperpage;
    public Sprite defaultImage;
    void Start()
    {
        
    }
    private void OnEnable()
    {
        //gametopperpage.SetActive(true);
        GameNumber.text = GameLB.currentgameId.ToString();
        GameName.text = GameLB.currentgamename.ToUpperInvariant();
        GamePreview.gameObject.SetActive(true);
        GamePreview.sprite = GameLB.Gamecard;
        if (databars.Count == 0)
        {
            StartCoroutine(GetGameDetails());
        }
    }

    IEnumerator GetGameDetails()
    {
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.GameLeaderboardApi}?GID={GameLB.currentgameId}&Min_Percentile=1" +
            $"&Max_Percentile=100&OrgID={PlayerPrefs.GetInt("OID")}";
        WWW Request = new WWW(HittingUrl);
        yield return Request;
        if(Request.text != null)
        {
            if(Request.text != "")
            {
                AESAlgorithm aes = new AESAlgorithm();
                string response = Request.text.TrimStart('"').TrimEnd('"');
                string Decryptedlog = aes.getDecryptedString(response);
                Debug.Log("Game log " + Decryptedlog);
                List<OverallLBModel> boardData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OverallLBModel>>(Decryptedlog);
                int index = 0;
                boardData.ForEach(x =>
                {
                    GameObject gb = Instantiate(Rowprefeb, Rowhandler, false);
                    databars.Add(gb);
                    gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = x.Rank.ToString();
                    gb.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = x.Name;
                    gb.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.GetComponent<Text>().text = x.score.ToString();
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
                    //if (index < 3)
                    //{
                    //    Username[index].text = x.Name;
                    //    Score[index].text = x.score.ToString();
                    //    AvatarImage[index].sprite = Homepage.avatarSprite[index];
                    //    if (x.Id_Avatar > 0)
                    //    {
                    //        Homepage.avatarSprite.ForEach(a =>
                    //        {
                    //            if (a.name.Equals(x.Id_Avatar.ToString(), System.StringComparison.OrdinalIgnoreCase))
                    //            {
                    //                AvatarImage[index].gameObject.SetActive(true);
                    //                AvatarImage[index].sprite = a;
                    //            }
                    //        });
                    //    }
                    //    else
                    //    {
                    //        AvatarImage[index].gameObject.SetActive(true);
                    //        AvatarImage[index].sprite = defaultImage;
                    //    }
                    //}
                    //else
                    //{
                    //    GameObject gb = Instantiate(Rowprefeb, Rowhandler, false);
                    //    databars.Add(gb);
                    //    gb.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = x.Rank.ToString();
                    //    gb.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = x.Name;
                    //    gb.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.GetComponent<Text>().text = x.score.ToString();
                    //    if (x.Id_Avatar > 0)
                    //    {
                    //        Homepage.avatarSprite.ForEach(a =>
                    //        {
                    //            if (a.name.Equals(x.Id_Avatar.ToString(), System.StringComparison.OrdinalIgnoreCase))
                    //            {
                    //                gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    //                gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = a;
                    //            }
                    //        });
                    //    }
                    //    else
                    //    {
                    //        gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    //        gb.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = defaultImage;
                    //    }
                    //}
                    //index++;
                });
            }
        }
    }

    public void BackTOgameLb()
    {
        AvatarImage.ForEach(x =>
        {
            x.gameObject.SetActive(false);
        });
        Username.ForEach(a =>
        {
            a.text = "---";
        });
        Score.ForEach(b =>
        {
            b.text = "0";
        });
        for (int a=0;a< Rowhandler.transform.childCount; a++)
        {
            Destroy(Rowhandler.transform.GetChild(a).gameObject);
        }
        //gametopperpage.SetActive(false);
        databars.Clear();
        GameListBar.SetActive(true);
        this.gameObject.SetActive(false);
    }


}
