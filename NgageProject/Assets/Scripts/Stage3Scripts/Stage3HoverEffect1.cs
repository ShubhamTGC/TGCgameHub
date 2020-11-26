using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Stage3HoverEffect1 : MonoBehaviour
{
    public GameObject startpage;
    public GameObject ZoneChild;
    public Color HoverEffect, RelasedEffect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseEnter()
    {
        startpage.GetComponent<Image>().color = HoverEffect;
        ZoneChild.SetActive(true);
    }

    public void OnMouseExit()
    {
       
        // StartCoroutine(CancelEffect());
        startpage.GetComponent<Image>().color = RelasedEffect;
        ZoneChild.SetActive(false);
    }
}
