using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompleteIntroPage : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite Hero_intro, badge_intro,avatar_page,main_page,bg_image;
    public GameObject profile_setup, Stage_page,logoutbutton;
    public StartpageController startpage;
    public Button Next_btn, Back_btn;
    public GameObject Homepage_obj;
    
    void Start()
    {
       
    }

    private void OnEnable()
    {
        initialtask();
    }


    void initialtask()
    {
        Next_btn.gameObject.SetActive(true);
        //logoutbutton.SetActive(true);
        Next_btn.onClick.RemoveAllListeners();
        Next_btn.onClick.AddListener(delegate { Next_task(); });
    }
    // Update is called once per frame
    void Update()
    {
        
    }

     void Next_task()
    {
        StartCoroutine(fade_effect());
    }

    

    public void back_task()
    {
        StartCoroutine(back_1st_task());
      
    }

    IEnumerator back_1st_task()
    {
        Next_btn.gameObject.SetActive(false);
        Back_btn.gameObject.SetActive(false);
        StartCoroutine(startpage.scenechanges(Homepage_obj, main_page));
        yield return new WaitForSeconds(1f);
        startpage.homebuttonpage.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        this.gameObject.SetActive(false);
    }

    IEnumerator fade_effect()
    {
        string avatar_data = PlayerPrefs.GetString("profile_done");
        Next_btn.onClick.RemoveAllListeners();
        Back_btn.onClick.RemoveAllListeners();
        if(avatar_data == "done")
        {
            logoutbutton.SetActive(false);
            Next_btn.gameObject.SetActive(false);
            Back_btn.gameObject.SetActive(false);
            StartCoroutine(startpage.scenechanges(Homepage_obj, badge_intro));
            yield return new WaitForSeconds(1.2f);
            After_profile();
        }
        else
        {
            logoutbutton.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            Next_btn.onClick.RemoveAllListeners();
            Back_btn.onClick.RemoveAllListeners();
            Next_btn.gameObject.SetActive(false);
            Back_btn.gameObject.SetActive(false);
            //skip_btn.gameObject.SetActive(false);
            StartCoroutine(startpage.scenechanges(Homepage_obj, avatar_page));
            yield return new WaitForSeconds(1.2f);
            profile_setup.SetActive(true);
            //this.gameObject.SetActive(false);
        }
     
    }

    public void After_profile()
    {
        Debug.Log("excuted after avatar seletion");
        logoutbutton.SetActive(false);
        Next_btn.onClick.RemoveAllListeners();
        Back_btn.onClick.RemoveAllListeners();
        Next_btn.gameObject.SetActive(true);
        Back_btn.gameObject.SetActive(true);
        //skip_btn.gameObject.SetActive(true);
        Next_btn.onClick.RemoveAllListeners();
        Back_btn.onClick.RemoveAllListeners();
        Next_btn.onClick.AddListener(delegate { after_badge(); });
        Back_btn.onClick.AddListener(delegate { backfrom_badge(); });
    }

    public void after_badge()
    {
        StartCoroutine(after_badge_task());
    }

    IEnumerator after_badge_task()
    {
        logoutbutton.SetActive(false);
        Next_btn.gameObject.SetActive(false);
        Back_btn.gameObject.SetActive(false);
       // skip_btn.gameObject.SetActive(false);
        StartCoroutine(startpage.scenechanges(Homepage_obj, bg_image));
        yield return new WaitForSeconds(1.2f);
        Stage_page.SetActive(true);
        //this.gameObject.SetActive(false);
    }

    void backfrom_badge()
    {
        StartCoroutine(back_badge_task());
    }
    IEnumerator back_badge_task()
    {
       
        Next_btn.gameObject.SetActive(false);
        Back_btn.gameObject.SetActive(false);
        //skip_btn.gameObject.SetActive(false);
        StartCoroutine(startpage.scenechanges(Homepage_obj, Hero_intro));
        yield return new WaitForSeconds(1.2f);
        initialtask();
        
    }

    public void back_from_stage()
    {
        StartCoroutine(back_stage());
    }
    
    IEnumerator back_stage()
    {
        Stage_page.SetActive(false);
        Back_btn.gameObject.SetActive(false);
        StartCoroutine(startpage.scenechanges(Homepage_obj, badge_intro));
        yield return new WaitForSeconds(1.2f);
        After_profile();
    }

    public void logouttask()
    {
        StartCoroutine(logoutaction());
    }
    IEnumerator logoutaction()
    {
        StartCoroutine(startpage.scenechanges(Homepage_obj, main_page));
        Next_btn.gameObject.SetActive(false);
        logoutbutton.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        PlayerPrefs.DeleteKey("logged");
        string msg = "Logged Out Successfully!";
        StartCoroutine(startpage.Messagedisplay(msg));
        yield return new WaitForSeconds(3.5f);
        startpage.homebuttonpage.SetActive(true);
        Next_btn.gameObject.SetActive(true);
        logoutbutton.SetActive(true);
        this.gameObject.SetActive(false);
      
    }



}
