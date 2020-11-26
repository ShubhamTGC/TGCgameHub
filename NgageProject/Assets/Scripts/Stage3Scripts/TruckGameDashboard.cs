using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSQL;
using System.Linq;

public class TruckGameDashboard : MonoBehaviour
{
    public SimpleSQLManager dbmanager;
    public Transform objectparent;
    public GameObject dataPrefeb;
    public Sprite Correctans, WrongAns, CorrectOpt;
    public Text OverallScore;
    public int TotalgameScore;
    private int AllScore = 0;

    private List<string> truckname = new List<string>();
    private List<int> dustbinCollection = new List<int>();
    private List<string> reachedcenter = new List<string>();
    private List<int> CenterScore = new List<int>();
    private List<int> is_reached_correct = new List<int>();
    private List<GameObject> dataHandler = new List<GameObject>();
    private int DustinCollectScore;
    [SerializeField] private int correctansScore =10;
    [SerializeField] private List<string> ALignTableseq;
    [SerializeField] private List<string> CorrectSequence;
    public GameObject Showmsg, Mainpage;
    [Header("API INTEGRATION DATA ")]
    public string MainUrl;
    public string DriveLogApi;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        if(dataHandler.Count == 0)
        {
            // StartCoroutine(GetdataFromDB());
            Debug.Log("Api started");
            StartCoroutine(GetDrivingGameLog());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GetDrivingGameLog()
    {
        string HittingUrl = $"{MainUrl}{DriveLogApi}?UID={PlayerPrefs.GetInt("UID")}";
        WWW gameLog = new WWW(HittingUrl);
        yield return gameLog;
        if (gameLog.text != null)
        {
            if (gameLog.text != "[]")
            {
                Mainpage.SetActive(true);
                Showmsg.SetActive(false);
                List<TruckGetLogModel> TruckModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TruckGetLogModel>>(gameLog.text);
                var MaxNum = TruckModel.Max(x => x.attempt_no);
                TruckModel.ForEach(x =>
                {
                    if (x.attempt_no == MaxNum)
                    {
                        truckname.Add(x.truck_name);
                        dustbinCollection.Add(x.dustbin_collected);
                        reachedcenter.Add(x.reached_center);
                        CenterScore.Add(x.center_score);
                        is_reached_correct.Add(x.is_correct_reached);
                    }
                });

                for (int b = 0; b < truckname.Count + 1; b++)
                {
                    GameObject gb = Instantiate(dataPrefeb, objectparent, false);
                    dataHandler.Add(gb);
                }

                dustbinCollection.ForEach(x =>
                {
                    DustinCollectScore += x * correctansScore;
                });


                GeneratedashBoard();
            }
            else
            {
                Mainpage.SetActive(false);
                Showmsg.SetActive(true);
            }
        }
    }





    void GeneratedashBoard()
    {

        for (int a = 0; a < dataHandler.Count; a++)
        {

            if (a == 0)
            {
                for (int b = 0; b < truckname.Count; b++)
                {
                    dataHandler[a].gameObject.transform.GetChild(b).gameObject.GetComponent<Text>().text = dustbinCollection[b].ToString() + "/7";
                    dataHandler[a].gameObject.transform.GetChild(4).gameObject.GetComponent<Text>().text = DustinCollectScore.ToString();

                }
            }

        }
   
        for (int a = 0; a < truckname.Count; a++)
        {
            if (is_reached_correct[a] == 1)
            {
                for (int b = 0; b < ALignTableseq.Count; b++)
                {
                    if (truckname[a] == ALignTableseq[b])
                    {
                        dataHandler[b + 1].gameObject.transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        dataHandler[b + 1].transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = Correctans;
                        dataHandler[b + 1].transform.GetChild(4).gameObject.GetComponent<Text>().text = CenterScore[a].ToString();
                     
                    }
                }
            }
            else if (is_reached_correct[a] == 0)
            {
                if (reachedcenter[a] != "null")
                {
                    int GotWrong = ALignTableseq.FindIndex(x => x == reachedcenter[a]);
                    dataHandler[GotWrong + 1].gameObject.transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    dataHandler[GotWrong + 1].transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = WrongAns;
                    dataHandler[GotWrong + 1].transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                    dataHandler[GotWrong + 1].transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<HoverEffectDashboard>().CorrectAns = truckname[a];
                    int Gotoption = CorrectSequence.FindIndex(y => y == truckname[a]);
                    for (int b = 0; b < ALignTableseq.Count; b++)
                    {
                        if (truckname[a] == ALignTableseq[b])
                        {
                            dataHandler[b + 1].gameObject.transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.SetActive(true);
                            dataHandler[b + 1].transform.GetChild(a).gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = CorrectOpt;
                            dataHandler[b + 1].transform.GetChild(4).gameObject.GetComponent<Text>().text = CenterScore[a].ToString();
                       
                        }
                    }
                }
                else
                {
                    for (int b = 0; b < ALignTableseq.Count; b++)
                    {
                        if (truckname[a] == ALignTableseq[b])
                        {
                            dataHandler[b + 1].gameObject.transform.GetChild(a).gameObject.GetComponent<Text>().text = "---";
                            dataHandler[b + 1].transform.GetChild(4).gameObject.GetComponent<Text>().text = CenterScore[a].ToString();

                        }
                    }
                }

            }
            AllScore += CenterScore[a];
            


        }
        OverallScore.text = (DustinCollectScore + AllScore).ToString();
    }



    public void Resetdata()
    {
        for(int a=0;a< objectparent.transform.childCount; a++)
        {
            Destroy(objectparent.transform.GetChild(a).gameObject, 0.05f);
        }
        truckname.Clear();
        dataHandler.Clear();
        dustbinCollection.Clear();
        reachedcenter.Clear();
        CenterScore.Clear();
        is_reached_correct.Clear();
    }
}
