using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TruckSelectionPageHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public string MainUrl, TruckSeqApi,TruckLogApi;
    public Color NotAssigned, Assigned;
    public List<GameObject> Lights; 
    public List<GameObject> Truckcounter;
    public GameBoard Gamemanager;
    public List<GameObject> TrucksPriority = new List<GameObject>();
    public List<GameObject> TruckPoits;
    public List<GameObject> StationaryTrucks = new List<GameObject>();
    private int Counter = 1;
    private bool Helpingbool;
    public GameObject ConfirmationOPage,InformationPage;
    public GameObject StartGamePage,truckGameUi,TimerPanel,Monster,monster2, monster3;
    public List<GameObject> Trucks;
    public GameObject landingpage,backbtn;
    [Header("Game Obeject ")]
    public GameObject TruckMainPage;
    public List<GameObject> TrucksBtn;
    public AudioClip GameSoundTrack;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        initialTask();
        StationaryTrucks.Clear();
        //StartCoroutine(getTruckSeq());
        StartCoroutine(getAttemptNumber());
      
    }

    void initialTask()
    {
        Counter = 1;
        Truckcounter.ForEach(x =>
        {
            x.transform.GetChild(0).gameObject.GetComponent<Text>().color = NotAssigned;
            x.transform.GetChild(0).gameObject.GetComponent<Text>().text = "00";
        });
        Lights.ForEach(x =>
        {
            x.transform.GetChild(1).gameObject.SetActive(false);
            x.transform.GetChild(0).gameObject.SetActive(true);
        });

        Helpingbool = true;
        InformationPage.SetActive(true);
    }

    //IEnumerator getTruckSeq()
    //{
    //    Gamemanager.UserSelectedId.Clear();
    //    StationaryTrucks.Clear();
    //    Gamemanager.TrucksPriority.Clear();
    //    Gamemanager.StationaryTrucks.Clear();
    //    Gamemanager.TruckSequence.Clear();
    //    Gamemanager.TruckID.Clear();
    //    string HittingUrl = MainUrl + TruckSeqApi + "?UID=" + PlayerPrefs.GetInt("UID");
    //    WWW GetTruckSeq = new WWW(HittingUrl);
    //    yield return GetTruckSeq;
    //    if(GetTruckSeq.text != null)
    //    {
    //        List<TruckSeqModel> Truckdata = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TruckSeqModel>>(GetTruckSeq.text);
    //        Truckdata.ForEach(x =>
    //        {
    //            Gamemanager.TruckSequence.Add(x.truck_name);
    //            Gamemanager.TruckID.Add(x.id_truck);
    //            Gamemanager.CorrectPoint = x.correct_priority_point;
    //            Gamemanager.WrongPoint = x.wrong_point;
    //        });
    //    }
    //}
    IEnumerator getAttemptNumber()
    {
        string Hittingurl = MainUrl + TruckLogApi + "?UID=" + PlayerPrefs.GetInt("UID");
        WWW Attempt_www = new WWW(Hittingurl);
        yield return Attempt_www;
        if(Attempt_www.text != null)
        {
            if(Attempt_www.text != "[]")
            {
                List<TruckLogModel> TruckLog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TruckLogModel>>(Attempt_www.text);
                var MaxValue = TruckLog.Max(x => x.attempt_no);
                Gamemanager.AttemptNumber = MaxValue;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Gamemanager.StationaryTrucks.Count == TruckPoits.Count && Helpingbool)
        {
            Helpingbool = false;
            getPriorityTableAns();
            //ConfirmationOPage.SetActive(true);
        }
    }


    public void SelectTrucksPriority(GameObject Trucks)
    {
        //GameObject gb = null;
        //if (!Gamemanager.TrucksPriority.Contains(Trucks))
        //{
        //    Gamemanager.TrucksPriority.Add(Trucks);
        //}
        for (int a = 0; a < TruckPoits.Count; a++)
        {
            //if (Gamemanager.TruckPoits[a].name == Trucks.name)
            //{
            //    if (!Gamemanager.StationaryTrucks.Contains(Gamemanager.TruckPoits[a]))
            //    {
            //        StationaryTrucks.Add(Gamemanager.TruckPoits[a]);
            //        Gamemanager.UserSelectedId.Add(Gamemanager.TruckID[a]);
            //        Gamemanager.StationaryTrucks.Add(Gamemanager.TruckPoits[a]);
            //        Truckcounter[a].transform.GetChild(0).gameObject.GetComponent<Text>().color = Assigned;
            //        Truckcounter[a].transform.GetChild(0).gameObject.GetComponent<Text>().text = "0" + Counter;
            //        Lights[a].transform.GetChild(0).gameObject.SetActive(false);
            //        Lights[a].transform.GetChild(1).gameObject.SetActive(true);
            //        Counter++;
            //    }
            //    else
            //    {
            //        Debug.Log("Already added");
            //    }
            //}
        }
    }
   

    public void Letsplaygame()
    {
        StartCoroutine(GamePlayTask());
    }
    IEnumerator GamePlayTask()
    {
        iTween.ScaleTo(ConfirmationOPage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        ConfirmationOPage.SetActive(false);
        StartGamePage.SetActive(false);
        Monster.SetActive(true);
        monster2.SetActive(true);
        monster3.SetActive(true);
        TruckMainPage.SetActive(true);
        truckGameUi.SetActive(true);
        TimerPanel.SetActive(true);
        Gamemanager.getGamedata();
        Gamemanager.PlayGame();
        Camera.main.gameObject.GetComponent<AudioSource>().enabled = false;
        this.gameObject.SetActive(false);


    }

    public void ResetTrucks()
    {
        Gamemanager.UserSelectedId.Clear();
        StationaryTrucks.Clear();
       // Gamemanager.TrucksPriority.Clear();
        Gamemanager.StationaryTrucks.Clear();
        StartCoroutine(resetTask());
    }

    IEnumerator resetTask()
    {
        iTween.ScaleTo(ConfirmationOPage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        ConfirmationOPage.SetActive(false);
        initialTask();
    }

    public void ClosedInform()
    {
        StartCoroutine(closePage());
    }

    IEnumerator closePage()
    {
        iTween.ScaleTo(InformationPage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        Trucks.ForEach(x =>
        {
            x.SetActive(true);
        });
        InformationPage.SetActive(false);

    }


    void getPriorityTableAns()
    {
        int SelectionCounter = 0;
       for(int a = 0; a < Gamemanager.StationaryTrucks.Count; a++)
        {
            if(Gamemanager.StationaryTrucks[a].name == Gamemanager.TruckSequence[a])
            {
                SelectionCounter += 1;
            }
        }
       if(Gamemanager.StationaryTrucks[0].name == Gamemanager.TruckSequence[0])
        {
            string msg = "You are all set to play. Click on 'Start' to start the game.";
            StartCoroutine(ShowPriorityAns(msg, 2, 1));
        }
        else
        {
            string msg = "Oops! You have selected the wrong sequence. Retry";
            StartCoroutine(ShowPriorityAns(msg, 1, 2));
        }

        //if (SelectionCounter == Gamemanager.TruckSequence.Count)
        //{
      
        //}
       
    }

    IEnumerator ShowPriorityAns(string msg,int enable, int disable)
    {
        yield return new WaitForSeconds(0.1f);
        ConfirmationOPage.transform.GetChild(0).gameObject.GetComponent<Text>().text = msg;
        ConfirmationOPage.transform.GetChild(enable).gameObject.SetActive(true);
        ConfirmationOPage.transform.GetChild(disable).gameObject.SetActive(false);
        ConfirmationOPage.SetActive(true);
    }


    public void BackTolanding()
    {
        StartCoroutine(LandingPageTask());
    }

    IEnumerator LandingPageTask()
    {
        yield return new WaitForSeconds(0.1f);
        Gamemanager.UserSelectedId.Clear();
        StationaryTrucks.Clear();
        //Gamemanager.TrucksPriority.Clear();
        Gamemanager.StationaryTrucks.Clear();
        for(int a=0;a< TrucksBtn.Count; a++)
        {
            iTween.ScaleTo(TrucksBtn[a].gameObject, Vector3.zero, 0.2f);
            yield return new WaitForSeconds(0.2f);
            TrucksBtn[a].SetActive(false);
        }
        yield return new WaitForSeconds(1f);
        landingpage.SetActive(true);
        backbtn.SetActive(true);
        this.gameObject.SetActive(false);

    }
}
