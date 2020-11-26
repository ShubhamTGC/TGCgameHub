using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SimpleSQL;
using System.Linq;

public class DIYpageHandler : MonoBehaviour
{
    // Start is called before the first frame updateNext

    public GameObject NextButton, BackButton;
    public List<GameObject> Pagelist;
    private int pagecounter=0;
    private int lastpagecounter=0;
    public Text StageText;
    [Header("PDF file download section")]
    [Space(10)]
    public string Mainurl;
    public string DiyUnlockApi;
    public List<string> DiyUrls;
    private string PdfUrl;
    public GameObject PopUpmsgPage;
    private int StageWiseLimit;
    private string Stagename;
    public string CurrentStage;
    public GameObject DiyuploadPage;
    [SerializeField] private int Stagelimit =5;
    public bool Disablebtn;
    public SimpleSQLManager dbmanager;
    void Start()
    {
        
      
    }
    private void OnEnable()
    {
        Disablebtn = true;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            //StartCoroutine(CheckLevelCleared());
            StartCoroutine(GetUnlockStage());

        }
        else
        {
            Stagename = CurrentStage;
            StageWiseLimit = Stagelimit;
        }

        pagecounter = 0;
        for (int a = 0; a < Pagelist.Count; a++)
        {
            if (a == pagecounter)
            {
                Pagelist[a].SetActive(true);
            }
            else
            {
                Pagelist[a].SetActive(false);
            }
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            StageText.text = pagecounter < 5 ? "Stage 1" : "Stage 2";
            Stagename = pagecounter < 5 ? "Stage1" : "Stage2";
            if (pagecounter > 9)
            {
                StageText.text = "Stage 3";
                Stagename = "Stage3";
            }
        }
        if (Disablebtn)
        {
            if (pagecounter == 0)
            {
                BackButton.SetActive(false);
                NextButton.SetActive(true);
            }
            else if (pagecounter > 0 && pagecounter < StageWiseLimit - 1)
            {
                BackButton.SetActive(true);
                NextButton.SetActive(true);
            }
            else if (pagecounter < StageWiseLimit)
            {
                NextButton.SetActive(false);
            }
        }
       
    }

    public void NextpageEnable()
    {
        lastpagecounter = pagecounter;
        pagecounter++;
        Pagelist[pagecounter].SetActive(true);
        Pagelist[lastpagecounter].SetActive(false);
    }

    public void BackPageEnable()
    {
        lastpagecounter = pagecounter;
        pagecounter--;
        Pagelist[pagecounter].SetActive(true);
        Pagelist[lastpagecounter].SetActive(false);
    }

    public void DownloadPdfFIle()
    {

        FileDownloader fileDownloader = new FileDownloader();

        PdfUrl = DiyUrls[pagecounter];
        string Diypage = Stagename + "DIY" + (pagecounter + 1).ToString();
        string savingPath = "";
        string pathStart = "";
        if (UnityEngine.Application.platform == RuntimePlatform.Android)
        {
            if (UnityEngine.Application.platform == RuntimePlatform.Android)
            {
                 pathStart = "/storage/emulated/0/UrbanWasteManagement";
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
            string msg = "You have successfully downloaded DIY " + (pagecounter + 1) + " book!!";
            StartCoroutine(ShowMsgPop(msg));
        }
        else
        {
            string msg = "You have already downloaded DIY " + (pagecounter + 1) + " book!!";
            StartCoroutine(ShowMsgPop(msg));
        }
        
       
       
    }
    IEnumerator ShowMsgPop(string msg)
    {
        yield return new WaitForSeconds(0.1f);
        PopUpmsgPage.transform.GetChild(0).GetComponent<Text>().text = msg;
        PopUpmsgPage.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        PopUpmsgPage.transform.GetChild(0).GetComponent<Text>().text = "";
        iTween.ScaleTo(PopUpmsgPage, Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.5f);
        PopUpmsgPage.SetActive(false);

    }


    void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
        if (e.Error == null)
        {
            Debug.Log("file downloaded");
        }
    }

    IEnumerator CheckLevelCleared()
    {
        string Hitting_url = Mainurl + DiyUnlockApi + "?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID");
        WWW diy_www = new WWW(Hitting_url);
        yield return diy_www;
        if(diy_www.text != null)
        {
            LevelClearness leveldata = Newtonsoft.Json.JsonConvert.DeserializeObject<LevelClearness>(diy_www.text);
            Debug.Log("level log " + diy_www.text);
            if(leveldata.LastCompletedLevelId == "0")
            {
                StageWiseLimit = 5;
            }
            else if(leveldata.LastCompletedLevelId == "1")
            {
                StageWiseLimit = 10;
            }
            else
            {
                StageWiseLimit = 16;
            }
            //else if(leveldata.LastCompletedLevelId == "2")
            //{
            //    StageWiseLimit = 15;
            //}

        }
        Debug.Log("stage limit " + StageWiseLimit);
       
    }

    IEnumerator GetUnlockStage()
    {
        yield return new WaitForSeconds(0.1f);
        var Log = dbmanager.Table<StageClearness>().ToList();
        if(Log == null)
        {
            StageWiseLimit = 5;
        }
        else
        {
            Log.ForEach(x =>
            {
                if (x.IsClear == 1 &&  x.LevelId == 1)
                {
                    StageWiseLimit = 10;
                }
                if (x.IsClear == 1 && x.LevelId ==2 )
                {
                    StageWiseLimit = 16;
                }
                if(x.IsClear == 0 && x.LevelId ==1)
                {
                    StageWiseLimit = 5;
                }
                if (x.IsClear == 0 && x.LevelId == 2)
                {
                    StageWiseLimit = 5;
                }
            });
        }
      
    }
}
