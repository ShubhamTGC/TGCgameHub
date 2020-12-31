using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSQL;
using UnityEngine.UI;
using System.Linq;

public class PercentileTableCreator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Rowprefeb;
    public Transform Rowhandler;
    private List<GameObject> Rows = new List<GameObject>(0);
    public SimpleSQLManager dbmanager;
    public GameObject dataPrefeb;
    public GameObject ViewPage;
    public HomePageCardSection homepage;
    private int UserPercentile;
    private bool IsInrange;
    [SerializeField] private List<Color> BarColors;
    void Start()
    {
        
    }
    

    private void OnEnable()
    {
        UserPercentile = PlayerPrefs.GetInt("Percentile");
        IsInrange = false;
        if (Rows.Count == 0)
        {
            StartCoroutine(generateTable());
        }
    }

    IEnumerator generateTable()
    {
        yield return new WaitForSeconds(0.1f);
        var TableLog = dbmanager.Table<PercentileTable>().Select(x=>x.Percentile).ToList();

        for(int a = 0; a < TableLog.Count-1; a++)
        {
            GameObject gb = Instantiate(Rowprefeb, Rowhandler, false);
            gb.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = (a + 1).ToString();
            gb.SetActive(true);
            gb.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = "Players between " + TableLog[a + 1].ToString() + "th to " + TableLog[a].ToString() + "th" + " Percentile";
            gb.GetComponent<Image>().color = BarColors[a];
            Rows.Add(gb);
            string objectName = TableLog[a + 1].ToString() + "/" + TableLog[a].ToString() + "/";
            IsInrange = TableLog[a] >= UserPercentile && UserPercentile >= TableLog[a + 1]; 
            Button btn = gb.transform.GetChild(0).transform.GetChild(2).gameObject.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(delegate { PercentileBasedBoard(objectName, gb.transform, btn); });
            if (IsInrange)
            {
                PercentileBasedBoard(objectName, gb.transform, btn);
            }
        }



      

    }

    void PercentileBasedBoard(string name,Transform parent,Button btn)
    {
        Debug.Log(" name  " + name);
        btn.gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, -90f);
        GameObject gb = Instantiate(dataPrefeb, parent, false);
        gb.name = name;
        gb.GetComponent<PercentileRangeLb>().homepage = homepage;
        StartCoroutine(RefreshBoard());
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(delegate { CleanPercentileBoard(name,parent,btn,gb); });
    }

    IEnumerator RefreshBoard()
    {
        ViewPage.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        ViewPage.SetActive(true);
    }

    void CleanPercentileBoard(string name, Transform parent, Button btn,GameObject page)
    {
        Destroy(page);
        btn.gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, 0f);
        StartCoroutine(RefreshBoard());
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(delegate { PercentileBasedBoard(name, parent, btn); });
    }

    public void CleanBoard()
    {
        Rows.Clear();
        int count = Rowhandler.transform.childCount;
        for(int a = 0; a < count; a++)
        {
            Destroy(Rowhandler.transform.GetChild(a).gameObject);
        }
    }
}
