using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class LoginHandler : MonoBehaviour
{

    public GameObject login, level1;
    [Header("-----Login elements-----")]
    public InputField username_input; 
    public InputField password_input;
    public GameObject loginpage,gameheading, profilesetup_page, statusMsgpanel,loginbutton,gameFrame;
    public AESalgorithm asealgorithm;

    [Header("-------API links and main url---------")]
    public string Mainurl;
    public string loginAPI;

    [Header("-------Json data variables---------")]
    private JsonData loginresult;

    [Header("-----profile setup elements-----")]
    public GameObject boy;
    public GameObject girl, submitbutton,characterpage,userdetailspage;
    public Image characterimage;
    public InputField profile_name;
    public Dropdown grade;
    private string charactertype;
    public Sprite boysprite, girlsprite,boyface,girlface;
    public GameObject buttonpanel, profileimage;
    public GameObject toplayer;

    private void Awake()
    {
        toplayer.SetActive(true);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(username_input.text != "" && password_input.text != "")
        {
            loginbutton.GetComponent<Button>().interactable = true;
        }
        else
        {
            loginbutton.GetComponent<Button>().interactable = false;
        }

        if(charactertype != "" && profile_name.text != "" && grade.value.ToString() != "")
        {
            submitbutton.GetComponent<Button>().interactable = true;
        }
        else
        {
            submitbutton.GetComponent<Button>().interactable = false;
        }

    }
    

    //---------------login method-------------------------//
    public void login_task()
    {
        var username = username_input.text;
        var password = password_input.text;
        var encryptedpassword = asealgorithm.getEncryptedString(password);
        if(username != "" && password != "")
        {
            //-------offline test------------------------//
            if (username == "shubham" && password == "12345")
            {
                username_input.text = "";
                password_input.text = "";
                username_input.GetComponent<InputField>().enabled = false;
                password_input.GetComponent<InputField>().enabled = false;
                loginbutton.GetComponent<Button>().enabled = false;
                string msg = "Login Successfully!";
                StartCoroutine(Messagedisplay(msg));
                StartCoroutine(Enablepage(profilesetup_page, loginpage, 4f));
            }
            else
            {
                username_input.text = "";
                password_input.text = "";
                string msg = "Login Failed";
                StartCoroutine(Messagedisplay(msg));
            }

            //----------------------api integration -------------//
            //string loginlink = Mainurl + loginAPI;
            //Debug.Log(loginlink);
            //WWWForm loginForm = new WWWForm();
            //loginForm.AddField("IMEI", "");
            //loginForm.AddField("USERID", username);
            //loginForm.AddField("PASSWORD", encryptedpassword);
            //loginForm.AddField("OS", "");
            //loginForm.AddField("Network", "");
            //loginForm.AddField("OSVersion", "");
            //loginForm.AddField("Details", "");
            //loginForm.AddField("REURL", "");

            //WWW loginurl = new WWW(loginlink,loginForm);
            //StartCoroutine(Loginactivity(loginurl));
          
        }
       
      
    }

    IEnumerator Loginactivity(WWW loginurl)
    {
        yield return loginurl;
        if(loginurl.text != null)
        {
            loginresult = JsonMapper.ToObject(loginurl.text);
            var status = loginresult["AuthStatus"].ToString();
            if(status == "SUCCESS")
            {
                string msg = "Login Successfully!";
                StartCoroutine(Messagedisplay(msg));
                StartCoroutine(Enablepage(profilesetup_page, loginpage, 3f));
            }
            else
            {
                string msg = "Login Failed!";
                StartCoroutine(Messagedisplay(msg));
            }
        }
    }

    IEnumerator Messagedisplay(string msg)
    {
        yield return new WaitForSeconds(0.1f);
        statusMsgpanel.SetActive(true);
        statusMsgpanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = msg;
        iTween.ScaleTo(statusMsgpanel, Vector3.one, 0.8f);
        yield return new WaitForSeconds(2f);
        iTween.ScaleTo(statusMsgpanel, Vector3.zero, 0.8f);
        yield return new WaitForSeconds(1f);
        statusMsgpanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        statusMsgpanel.SetActive(false);
     
        
    }

    //--------------------method for enable and disable objects--------------------------------
    IEnumerator Enablepage(GameObject enableobject,GameObject disableobject,float time)
    {
        yield return new WaitForSeconds(time);
        enableobject.SetActive(true);
        disableobject.SetActive(false);

    }

    //----------------------method for profile setup--------------------------
    public void boyselection()
    {
        charactertype = "boy";
        StartCoroutine(Enablepage(userdetailspage, characterpage, 1f));
        characterimage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 300);
        characterimage.sprite = boysprite;

    }
    public void girlselection()
    {
        charactertype = "girl";
        StartCoroutine(Enablepage(userdetailspage, characterpage, 1f));
        characterimage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 330);
        characterimage.sprite = girlsprite;
    }

    public void submit()
    {
        string profilename = profile_name.text;
        string grade_value = grade.options[grade.value].text;
       // Debug.Log("profile_name :" + profilename + "grade :" + grade_value + "character :" + charactertype);
        if(charactertype == "boy")
        {
            PlayerPrefs.SetInt("characterType", 1);
            profileimage.GetComponent<Image>().sprite = boyface;
        }
        else
        {
            PlayerPrefs.SetInt("characterType", 0);
            profileimage.GetComponent<Image>().sprite = girlface;
        }
        gameFrame.SetActive(true);
        level1.SetActive(true);
        login.SetActive(false);
        toplayer.SetActive(false);
        buttonpanel.SetActive(true);

    }

    public void backpage()
    {
        StartCoroutine(Enablepage(characterpage, userdetailspage, 1f));
    }

}
