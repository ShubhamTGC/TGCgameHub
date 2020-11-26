using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonHandler : MonoBehaviour
{
    public List<GameObject> Buttons;
    private Vector3 initialPos;
    public GameObject MainButton;
    public GameObject SettingPage;
    void Start()
    {
        initialPos = MainButton.GetComponent<RectTransform>().localPosition;
    }

    
    void Update()
    {
        
    }

    public void ShowMenu()
    {
        if (Buttons[0].gameObject.activeInHierarchy)
        {
            StartCoroutine(MenuClose());
        }
        else
        {
            StartCoroutine(MenuOpen());
        }
       
    }

    IEnumerator MenuOpen()
    {
        this.gameObject.GetComponent<Image>().enabled = true;
        this.gameObject.GetComponent<Image>().raycastTarget = true;
        Buttons.ForEach(x =>
        {
            x.SetActive(true);
        });
        yield return new WaitForSeconds(0.1f);
    }


    IEnumerator MenuClose()
    {
        this.gameObject.GetComponent<Image>().enabled = false;
        this.gameObject.GetComponent<Image>().raycastTarget = false;
        Buttons.ForEach(x =>
        {
            iTween.MoveTo(x.gameObject, iTween.Hash("position", initialPos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.2f));
        });
        yield return new WaitForSeconds(0.4f);
        Buttons.ForEach(x =>
        {
            x.SetActive(false);
        });

    }

    public void Setting()
    {
        SettingPage.SetActive(true);
        StartCoroutine(MenuClose());
    }


}
