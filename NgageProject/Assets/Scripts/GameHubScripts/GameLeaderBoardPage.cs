using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSQL;
using System.Linq;
using UnityEngine.UI;

public class GameLeaderBoardPage : MonoBehaviour
{
    public SimpleSQLManager dbmanager;
    public GameObject RowPrefeb;
    public Transform Rowhandler;
    private List<GameObject> GameList = new List<GameObject>();
    public GameObject GameLBPage, GamelistPage;
    [HideInInspector] public int currentgameId=0;
    [HideInInspector] public string currentgamename;
    [HideInInspector] public Sprite Gamecard;
    public List<Sprite> GamePreview;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if(GameList.Count == 0)
        {
            StartCoroutine(GetGameList());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GetGameList()
    {
        yield return new WaitForSeconds(0.1f);
        var GameLog = dbmanager.Table<GameListDetails>().ToList();
        if(GameLog != null)
        {
            GameLog.ForEach(x =>
            {
                GameObject gb = Instantiate(RowPrefeb, Rowhandler, false);
                GameList.Add(gb);
                gb.name = x.GameId.ToString();

                gb.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Text>().text = x.GameId.ToString();
                gb.transform.GetChild(1).transform.GetChild(1).gameObject.GetComponent<Text>().text = x.GameName.ToUpperInvariant();
                Button btn = gb.transform.GetChild(1).gameObject.GetComponent<Button>();
                GamePreview.ForEach(a =>
                {
                    if (x.GameId.ToString().Equals(a.name, System.StringComparison.OrdinalIgnoreCase))
                    {
                        gb.transform.GetChild(1).transform.GetChild(0).gameObject.GetComponent<Image>().sprite = a;
                        btn.onClick.RemoveAllListeners();
                        btn.onClick.AddListener(delegate { OpenGameLB(x.GameId, x.GameName,a); });
                    }
                });
               
            });
        }
    }


    public void OpenGameLB(int Gameid,string gamename,Sprite card)
    {
        currentgameId = Gameid;
        currentgamename = gamename;
        Gamecard = card;
        GameLBPage.SetActive(true);
        GamelistPage.SetActive(false);
    }


    public void ResetLeaderBoard()
    {
        GameList.Clear();
        int count = Rowhandler.transform.childCount;
        for (int a = 0; a < count; a++)
        {
            Destroy(Rowhandler.transform.GetChild(a).gameObject);
        }
    }
}
