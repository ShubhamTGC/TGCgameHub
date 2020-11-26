using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mapgame : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Button> dusbintargets;
    public List<Button> managementtarget;
    private bool firsttarget = false, secondtarget = false;
    private string firstname, secondname;
    void Start()
    {
        addlistener();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void addlistener()
    {
        foreach(Button btn in dusbintargets)
        {
            btn.onClick.AddListener(() => buttonaction());
        }
        foreach (Button btn in managementtarget)
        {
            btn.onClick.AddListener(() => buttonaction());
            
        }
        foreach (Button btn in managementtarget)
        {
            btn.enabled = false;

        }

    }

    public void buttonaction()
    {
        string targetname = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        if (!firsttarget)
        {
            firsttarget = true;
            for(int i=0;i < dusbintargets.Count; i++)
            {
                if(dusbintargets[i].gameObject.name == targetname)
                {
                    dusbintargets[i].gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                }
            }
            foreach(Button btn in managementtarget)
            {
                btn.enabled = true;
            }
            firstname = targetname;

        }
        else if (!secondtarget)
        {
            secondtarget = true;
            for (int i = 0; i < managementtarget.Count; i++)
            {
                if (managementtarget[i].gameObject.name == targetname)
                {
                    managementtarget[i].gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                }
            }
            secondname = targetname;
            StartCoroutine(resetsprite());
        }
    }

    IEnumerator resetsprite()
    {
        if (firstname == secondname)
        {
            Debug.Log("matched ");
            firsttarget = false;
            secondtarget = false;
            foreach(Button btn in dusbintargets)
            {
                btn.gameObject.transform.localScale = Vector3.one;
            }
            foreach (Button btn in managementtarget)
            {
                btn.gameObject.transform.localScale = Vector3.one;
            }
            foreach (Button btn in managementtarget)
            {
                btn.enabled = false;
            }
            //correctcounter++;
            yield return new WaitForSeconds(0.5f);
            //buttonobject[firstguesscount].interactable = false;
            //buttonobject[secondguesscount].interactable = false;
            //buttonobject[firstguesscount].image.color = new Color(0f, 0f, 0f, 0f);
            //buttonobject[secondguesscount].image.color = new Color(0f, 0f, 0f, 0f);
            //if (correctcounter == buttonobject.Count / 2)
            //{
            //    //yield return new WaitForSeconds(0.5f);
            //    Debug.Log("doneeeee");
            //}
            ////correctguess();

        }
        else
        {
            Debug.Log(" not matched ");
            firsttarget = false;
            secondtarget = false;
            foreach (Button btn in dusbintargets)
            {
                btn.gameObject.transform.localScale = Vector3.one;
            }
            foreach (Button btn in managementtarget)
            {
                btn.gameObject.transform.localScale = Vector3.one;
            }
            foreach (Button btn in managementtarget)
            {
                btn.enabled = false;
            }
        }

    }

}
