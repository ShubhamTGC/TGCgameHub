using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3HoverEffect : MonoBehaviour
{
    public GameObject panel, PriorityPage, GamePage;
    void Start()
    {
        
    }

    public void OnMouseOver()
    {
        if (this.gameObject.name.StartsWith("PlayGame"))
        {
            panel.SetActive(true);
            GamePage.transform.SetSiblingIndex(2);
            panel.transform.SetSiblingIndex(1);
            PriorityPage.transform.SetSiblingIndex(0);
        }
        if (this.gameObject.name.StartsWith("ArrangeTruck"))
        {
            panel.SetActive(true);
            GamePage.transform.SetSiblingIndex(0);
            panel.transform.SetSiblingIndex(1);
            PriorityPage.transform.SetSiblingIndex(2);
        }

    }

    public void OnMouseExit()
    {
        panel.transform.SetSiblingIndex(0);
        GamePage.transform.SetSiblingIndex(1);
        PriorityPage.transform.SetSiblingIndex(2);
        panel.SetActive(false);

    }
}
