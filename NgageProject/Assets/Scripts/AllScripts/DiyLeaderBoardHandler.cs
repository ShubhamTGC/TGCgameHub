using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DiyLeaderBoardHandler : MonoBehaviour
{
    public string MainUrl, DiyLeaderBoardApi;
    public GameObject dataRow;
    public Transform RowHandler;
    public Text UserName, ClassValue;
    private List<GameObject> Rows = new List<GameObject>();
    public Sprite Firstrank, secondrank, Thirdrank;
    public Sprite fisrtmedel, secondmedel, thirdmedel;
    private int rankOrder = 1;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        rankOrder = 1;
        //UserName.text = PlayerPrefs.GetString("username");
        //ClassValue.text = PlayerPrefs.GetString("User_grade");
        if(Rows.Count == 0)
        {
            StartCoroutine(GenerateLeaderBoard());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator GenerateLeaderBoard()
    {
        yield return new WaitForSeconds(0.1f);

        string HittingUrl = $"{MainUrl}{DiyLeaderBoardApi}?id_user={PlayerPrefs.GetInt("UID")}&org_id={PlayerPrefs.GetInt("OID")}";
        WWW diyLog = new WWW(HittingUrl);
        yield return diyLog;
        if(diyLog.text != null)
        {
            if(diyLog.text != "[]")
            {
                Debug.Log("data log " + diyLog.text);
                DiyLeaderBoardModel log = Newtonsoft.Json.JsonConvert.DeserializeObject<DiyLeaderBoardModel>(diyLog.text);
                List<DIYLeaderBoardList> listLog = log.LeaderList.ToList();
                UserName.text = log.Name;
                ClassValue.text = log.Class;
                listLog = listLog.OrderByDescending(x => x.DIYPoints).ToList();
                listLog.ForEach(x =>
                {
                    GameObject gb = Instantiate(dataRow, RowHandler, false);
                    Rows.Add(gb);
                    GameObject row = gb.transform.GetChild(0).gameObject;
                    GameObject user = gb.transform.GetChild(1).gameObject;
                    user.GetComponent<Text>().text = x.Name;
                    gb.transform.GetChild(2).gameObject.GetComponent<Text>().text = x.DIYTitle.ToString();
                    gb.transform.GetChild(3).gameObject.GetComponent<Text>().text = x.DIYPoints.ToString();


                    if (rankOrder == 1)
                    {
                        setRanks(row, Firstrank, user, fisrtmedel);
                    }
                    else if (rankOrder == 2)
                    {
                        setRanks(row, secondrank, user, secondmedel);
                    }
                    else if (rankOrder == 3)
                    {
                        setRanks(row, Thirdrank, user, thirdmedel);
                    }
                    else if (rankOrder > 3)
                    {
                        row.GetComponent<Text>().text = x.Rank.ToString();
                        user.transform.GetChild(0).gameObject.SetActive(false);
                    }
                    rankOrder++;

                });
            }
        }
    }

    void setRanks(GameObject rowobject, Sprite rank,GameObject user,Sprite medel)
    {
        rowobject.GetComponent<Text>().text = "";
        rowobject.transform.GetChild(0).gameObject.SetActive(true);
        rowobject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = rank;
        user.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = medel;
    }

    public void Resetdata()
    {
        Rows.Clear();
        int count = RowHandler.transform.childCount;
        for(int a = 0; a < count; a++)
        {
            Destroy(RowHandler.transform.GetChild(a).gameObject);
        }
        this.gameObject.SetActive(false);
    }
}
