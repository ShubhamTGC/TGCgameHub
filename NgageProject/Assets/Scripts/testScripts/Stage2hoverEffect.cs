using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage2hoverEffect : MonoBehaviour
{

    public GameObject panel;
    public GameObject level1, level2;
    public GameObject Level1Card, Level2Card,levelcard2Stick;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseOver()
    {
        if (this.gameObject.name.StartsWith("S1"))
        {
            //panel.SetActive(true);
            panel.GetComponent<Image>().enabled = true;
            level2.transform.SetSiblingIndex(0);
            levelcard2Stick.transform.SetSiblingIndex(1);
            Level2Card.transform.SetSiblingIndex(2);
            panel.transform.SetSiblingIndex(3);
            Level1Card.transform.SetSiblingIndex(4);
            level1.transform.SetSiblingIndex(5);
        }
        if (this.gameObject.name.StartsWith("S2"))
        {
            //panel.SetActive(true);
            panel.GetComponent<Image>().enabled = true;
            levelcard2Stick.SetActive(true);
            Level2Card.SetActive(false);
            Level1Card.transform.SetSiblingIndex(0);
            level1.transform.SetSiblingIndex(1);
            panel.transform.SetSiblingIndex(2);
            levelcard2Stick.transform.SetSiblingIndex(3);
            level2.transform.SetSiblingIndex(4);
            Level2Card.transform.SetSiblingIndex(5);
          
         
        }
       
    }

    public void OnMouseExit()
    {
        //panel.SetActive(false);
        levelcard2Stick.SetActive(false);
        Level2Card.SetActive(true);
        panel.GetComponent<Image>().enabled = false;
        panel.transform.SetSiblingIndex(0);
        Level1Card.transform.SetSiblingIndex(1);
        level1.transform.SetSiblingIndex(2);
        Level2Card.transform.SetSiblingIndex(3);
        levelcard2Stick.transform.SetSiblingIndex(4);
        level2.transform.SetSiblingIndex(5);
    }
}
