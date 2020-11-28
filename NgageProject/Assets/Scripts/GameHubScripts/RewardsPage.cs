using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardsPage : MonoBehaviour
{
    public List<GameObject> Tabs;
    public List<GameObject> titles;
    public List<GameObject> Pages;
    public Text UserScore;
    public HomePageCardSection Homepage;
    void Start()
    {
        
    }
    private void OnEnable()
    {
        UserScore.text = Homepage.Uservalue.ToString();
        for (int a = 0; a < Tabs.Count; a++)
        {
            if (a == 0)
            {
                Tabs[a].GetComponent<Image>().enabled = true;
                titles[a].SetActive(true);
                Pages[a].SetActive(true);
            }
            else
            {
                Tabs[a].GetComponent<Image>().enabled = false;
                titles[a].SetActive(false);
                Pages[a].SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void tabSelection(GameObject tabs)
    {
        Tabs.ForEach(x =>
        {
            x.GetComponent<Image>().enabled = x.name.Equals(tabs.name, System.StringComparison.OrdinalIgnoreCase) ? true : false;
        });
        titles.ForEach(y =>
        {
            y.SetActive(y.name.Equals(tabs.name, System.StringComparison.OrdinalIgnoreCase));
        });
        Pages.ForEach(c =>
        {
            c.SetActive(c.name.Equals(tabs.name, System.StringComparison.OrdinalIgnoreCase));
        });

    }
}
