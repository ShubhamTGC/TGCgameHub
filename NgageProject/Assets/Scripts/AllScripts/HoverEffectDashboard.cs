using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverEffectDashboard : MonoBehaviour
{
    public GameObject NoticeBoard;
    [HideInInspector]
    public string CorrectAns;


    // Update is called once per frame
    void Update()
    {
       
    }


    private void OnMouseEnter()
    {
        NoticeBoard.SetActive(true);
        NoticeBoard.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Correct Option is " + CorrectAns;
        NoticeBoard.transform.SetSiblingIndex(1);

    }
    private void OnMouseExit()
    {
        NoticeBoard.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        NoticeBoard.SetActive(false);
       
    }
}
