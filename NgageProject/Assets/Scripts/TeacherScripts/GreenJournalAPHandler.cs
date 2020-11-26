using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GreenJournalAPHandler : MonoBehaviour
{
    public Sprite Clicked, notClicked;
    public List<GameObject> StageTabs;
    public List<GameObject> Tabs;
    public List<TActionPlanleaderboard> TabsPages;
    public GameObject Mainpage;
    void Start()
    {
        
    }

    void OnEnable()
    {
        for (int a = 0; a < StageTabs.Count; a++)
        {
            if (a == 0)
            {
                StageTabs[a].GetComponent<Image>().sprite = Clicked;
                Tabs[a].SetActive(true);
            }
            else
            {
                StageTabs[a].GetComponent<Image>().sprite = notClicked;
                Tabs[a].SetActive(false);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SelectStages(GameObject Tab)
    {
        for (int a = 0; a < StageTabs.Count; a++)
        {
            if (StageTabs[a].name == Tab.name)
            {
                StageTabs[a].GetComponent<Image>().sprite = Clicked;
                Tabs[a].SetActive(true);
            }
            else
            {
                StageTabs[a].GetComponent<Image>().sprite = notClicked;
                Tabs[a].SetActive(false);
            }
        }
    }

    public void BackToMainPage()
    {
        TabsPages.ForEach(x =>
        {
            x.BackToMainpage();
        });
        Mainpage.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
