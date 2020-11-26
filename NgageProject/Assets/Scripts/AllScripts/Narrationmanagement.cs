using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Narrationmanagement : MonoBehaviour
{
    // Start is called before the first frame update
    
    public int msgcounter = 0;
    public List<GameObject> tillavatar;
    public List<GameObject> msglist;
    public bool btncliked = false;
    public GameObject playbtn,skipbtn,nextbtn;
    private Vector3 initialpos;
    public GameObject profilesetup;
    public GameObject intromsg,badgemsg,wastemsg,hero_banner;
    public Sprite bagde_sprite;
    public GameObject narration_page;
    public StartpageController startpage;
    public Sprite hero_sprite, badge_sprite;
    void Start()
    {
        initialpos = this.gameObject.GetComponent<RectTransform>().localPosition;
        Invoke("msgtask", 0.8f);
    }

     void OnEnable()
    {
        initialpos = this.gameObject.GetComponent<RectTransform>().localPosition;
        Invoke("msgtask", 0.8f);
    }

    private void OnDisable()
    {
        msgcounter = 0;
        btncliked = false;
        this.gameObject.GetComponent<RectTransform>().localPosition = initialpos;
        for (int a = 0; a < msglist.Count; a++)
        {
            msglist[a].gameObject.SetActive(false);
        }
     }

  
    // Update is called once per frame
    void Update()
    {
        
    }

    void msgtask()
    {
        intromsg.SetActive(true);
        
    }
    public void avatarpage()
    {
        intromsg.SetActive(false);
        this.gameObject.GetComponent<Image>().enabled = false;
        profilesetup.SetActive(true);
    }

    public void showmsg()
    {
        btncliked = true;
        msgcounter += 1;
       // StartCoroutine(afteravatar_action(msgcounter));
    }


    public void AfterAvatar_task()
    {
        this.gameObject.GetComponent<Image>().sprite = bagde_sprite;
        this.gameObject.GetComponent<Image>().enabled = true;
        badgemsg.SetActive(true);
        skipbtn.SetActive(true);
    }

    public void afterbadgetask()
    {
        this.gameObject.GetComponent<Image>().enabled = false;
        hero_banner.SetActive(true);
        wastemsg.SetActive(true);
        badgemsg.SetActive(false);
        narration_page.SetActive(false);
    }

    IEnumerator afteravatar_action(int msgno)
    {
       
        if(msgno < 1)
        {
            msglist[msgno].SetActive(true);
        }
        if(msgno > 0 && msgno < msglist.Count)
        {
            msglist[msgno].SetActive(true);
            msglist[msgno - 1].SetActive(false);
        }
        if(msgno == msglist.Count - 1)
        {
            playbtn.SetActive(true);
        }

        if(msgno < msglist.Count - 1)
        {
            if (!btncliked)
            {
                yield return new WaitForSeconds(6f);
                msgcounter += 1;
                StartCoroutine(afteravatar_action(msgcounter));
            }
            else
            {
                btncliked = false;
            }
        }
    }
}
