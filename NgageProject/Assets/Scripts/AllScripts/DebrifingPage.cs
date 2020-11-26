using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DebrifingPage : MonoBehaviour
{
    // Start is called before the first frame update
    public string MainUrl = "https://www.skillmuni.in/wsmapi/api/", StageUnlockApi = "getConsolidatedScoeOfLevelWMS", PdfUrl;
    
    [SerializeField]private int Gamelevel;
    [SerializeField]private string Stagename;

    public Text Username, UserScore;

    public Text PopUpMsgBox;
    public GameObject POpMsgPage;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(getlevelScore());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator getlevelScore()
    {
        string Hitting_url = $"{MainUrl}{StageUnlockApi}?UID={PlayerPrefs.GetInt("UID")}&id_level={Gamelevel}&id_org_game={1}";
        WWW StageData = new WWW(Hitting_url);
        yield return StageData;
        if (StageData.text != null)
        {
            StageUnlockModel StageModel = Newtonsoft.Json.JsonConvert.DeserializeObject<StageUnlockModel>(StageData.text);
            Username.text = PlayerPrefs.GetString("username");
            UserScore.text = StageModel.ConsolidatedScore.ToString();
        }
    }



    public void DownloadPdfFile()
    {
        FileDownloader fileDownloader = new FileDownloader();

        string Diypage = Stagename + "Debriefing";
        string savingPath = "";
        string pathStart = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                pathStart = "/storage/emulated/0/UrbanWasteManagement/Debriefing";
                if (!Directory.Exists(pathStart))
                {
                    Directory.CreateDirectory(pathStart);
                    savingPath = pathStart + "/" + Diypage + ".pdf";
                }
                else
                {
                    savingPath = pathStart + "/" + Diypage + ".pdf";
                }
            }

        }
        else
        {
            savingPath = UnityEngine.Application.persistentDataPath + "/" + Diypage + ".pdf";
        }
        Debug.Log("download file name  " + Diypage);
        if (!File.Exists(savingPath))
        {
            fileDownloader.DownloadFileAsync(PdfUrl, savingPath);
            string msg = "You have successfully downloaded Debriefing Book!";
            StartCoroutine(ShowMsgPop(msg));
        }
        else
        {
            string msg = "You have already downloaded Debriefing Book!";
            StartCoroutine(ShowMsgPop(msg));
        }
    }

    IEnumerator ShowMsgPop(string msg)
    {
        yield return new WaitForSeconds(0.1f);
        PopUpMsgBox.text = msg;
        POpMsgPage.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        iTween.ScaleTo(POpMsgPage, Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.5f);
        PopUpMsgBox.text = "";
        POpMsgPage.SetActive(false);

    }

  
}
