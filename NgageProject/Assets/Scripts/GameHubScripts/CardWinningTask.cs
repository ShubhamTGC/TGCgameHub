using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSQL;
using System.Linq;
using UnityEngine.Networking;
using m2ostnextservice.Models;
using UnityEngine.UI;

public class CardWinningTask : MonoBehaviour
{
    // Start is called before the first frame update
    public SimpleSQLManager dbmanager;
    private int CardScore, CardIDWin;
    public ScratchCardEffect cardEffectpage;
    public GameObject ScartchcardPage, WinText, LoseText, cardPanelpage, ScratchCard, CardResult, leaderboardPage;
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnEnable()
    {
        CardWinner();
    }

    void CardWinner()
    {
        var cardid = dbmanager.Table<ScratchcardList>().Select(x => x.CardId).ToList();
        List<int> ids = new List<int>();
        ids = cardid;
        int Randomindex = UnityEngine.Random.Range(1, ids.Count);
        var Iddata = dbmanager.Table<ScratchcardList>().FirstOrDefault(y => y.CardId == ids[Randomindex]);
        string CardText = Iddata.CardText;
        int cardPoint = Iddata.CardValue;
        CardScore = Iddata.CardValue;
        int cardID = Iddata.CardId;
        CardIDWin = cardID;
        StartCoroutine(PostCardWinning(cardID, CardText, cardPoint));

    }
    IEnumerator PostCardWinning(int cardid, string Msg, int point)
    {
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.PostScratchCard}";
        CardModel model = new CardModel
        {
            Id_User = PlayerPrefs.GetInt("UID"),
            id_scratchcard = cardid,
            scratchcard = Msg,
            scratch_point = point,
            ID_ORGANIZATION = PlayerPrefs.GetInt("OID"),
            IsScratch = 0,
            Card_type = 2
        };


        AESAlgorithm aes = new AESAlgorithm();
        string PostLog = Newtonsoft.Json.JsonConvert.SerializeObject(model);
        string EncryptedLog = aes.getEncryptedString(PostLog);
        CommonModel modelLog = new CommonModel
        {
            Data = EncryptedLog
        };

        string FinalLog = Newtonsoft.Json.JsonConvert.SerializeObject(modelLog);
        using (UnityWebRequest request = UnityWebRequest.Put(HittingUrl, FinalLog))
        {
            Debug.Log(request);
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Accept", "application/json");
            yield return request.SendWebRequest();
            if (!request.isNetworkError && !request.isHttpError)
            {
                Debug.Log(" response " + request.downloadHandler.text);
                int PostCardID = int.Parse(request.downloadHandler.text);
                cardEffectpage.CardID = PostCardID;
            }
            else
            {
                Debug.Log("prob : " + request.error);
            }
        }
    }

    public void OpenScratchCard()
    {
        StartCoroutine(CardOpeingTask());
    }


    IEnumerator CardOpeingTask()
    {
        GameObject card = ScartchcardPage.transform.GetChild(0).gameObject;
        GameObject msg = ScartchcardPage.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        GameObject btn = ScartchcardPage.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        iTween.ScaleTo(card, Vector3.zero, 0.3f);
        iTween.ScaleTo(msg, Vector3.zero, 0.3f);
        iTween.ScaleTo(btn, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.3f);
        ScartchcardPage.SetActive(false);

        if (CardScore != 0)
        {
            WinText.SetActive(true);
            WinText.GetComponent<Text>().text = "You Won " + CardScore.ToString() + "Points";
            LoseText.SetActive(false);
        }
        else
        {
            WinText.SetActive(false);
            LoseText.GetComponent<Text>().text = "Better Luck Next Time!";
            LoseText.SetActive(true);
        }
        cardPanelpage.SetActive(true);
        ScratchCard.SetActive(true);
        CardResult.SetActive(true);
    }


    public void SkipScratchCardPage()
    {
        StartCoroutine(CloseScartchCardPage());
    }

    IEnumerator CloseScartchCardPage()
    {
        GameObject card = ScartchcardPage.transform.GetChild(0).gameObject;
        GameObject msg = ScartchcardPage.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        GameObject btn = ScartchcardPage.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        iTween.ScaleTo(card, Vector3.zero, 0.3f);
        iTween.ScaleTo(msg, Vector3.zero, 0.3f);
        iTween.ScaleTo(btn, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.3f);
        ScartchcardPage.SetActive(false);
        leaderboardPage.SetActive(true);
    }
}
