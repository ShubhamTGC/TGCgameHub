using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class RegistrationScript : MonoBehaviour
{

    public AESalgorithm AesEncryptor;
    public Image Form_page;
    public GameObject FormHandler;
    public List<InputField> Allfields;
    public GameObject homePage_buttons;
    public GameObject statusPanel, StatusBox;

    [Header("====API Integration")]
    [Space(10)]
    public string BaseUrl;
    public string School_list, CheckUserID,UserRegister;
    public Dropdown school_dropdown;
    public Dictionary<string, int> school_ids = new Dictionary<string, int>();


    [Header("====User from fields")]
    [Space(10)]
    public InputField Firstname;
    public InputField Lastname, rollno, Useraddress, user_email, user_phone;
    public Toggle male, female;
    private string gender;
    public Text ExtramsgBox;

    private int school_id;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(fill_image());
        StartCoroutine(Get_schoolnames());
    }

    IEnumerator fill_image()
    {
        float fill_amount = Form_page.fillAmount;
        while(fill_amount < 1)
        {
            fill_amount += 0.1f;
            Form_page.fillAmount = fill_amount;
            yield return new WaitForSeconds(0.05f);
        }

        for (int a = 0; a < FormHandler.transform.childCount; a++)
        {
            FormHandler.transform.GetChild(a).gameObject.SetActive(true);
           
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cancel_registration()
    {
        for(int a = 0; a < Allfields.Count; a++)
        {
            Allfields[a].text = "";
        }
        for (int a = 0; a < FormHandler.transform.childCount; a++)
        {
            FormHandler.transform.GetChild(a).gameObject.SetActive(false);
        }
        StartCoroutine(canecl_register());
    }
    IEnumerator canecl_register()
    {
        float fill_amount = Form_page.fillAmount;

        while (fill_amount > 0)
        {
            fill_amount -= 0.1f;
            Form_page.fillAmount = fill_amount;
            yield return new WaitForSeconds(0.05f);
        }
        homePage_buttons.SetActive(true);
        school_dropdown.value = 0;
        this.gameObject.SetActive(false);
    }

    //==============taking school name and presenting in drop donw menu========================
    IEnumerator Get_schoolnames()
    {
        yield return new WaitForSeconds(0.1f);
        string hitting_url = BaseUrl + School_list;
        WWW school_names = new WWW(hitting_url);
        yield return school_names;
        if(school_names.text != null)
        {
            JsonData school_res = JsonMapper.ToObject(school_names.text);
            //school_dropdown.options.Add()
            //school_res[0]["school_name"].ToString()
            school_dropdown.options.Clear();
            school_dropdown.value = 0;
            school_dropdown.options.Add(new Dropdown.OptionData() { text = "School Name"});
            for (int a= 0;a< school_res.Count; a++)
            {
                school_dropdown.options.Add(new Dropdown.OptionData() { text = school_res[a]["school_name"].ToString() });
                school_ids.Add(school_res[a]["school_name"].ToString(), int.Parse(school_res[a]["id_school"].ToString()));
            }
        }

    }



    //======================  User Registration and Related to registration part ====================================

    public void RegisterUser()
    {


        if (Firstname.text == "" || Lastname.text == "" || rollno.text == "" || Useraddress.text == "" || school_dropdown.value == 0)
        {

             string msg = "Please fill the required information.";
            StartCoroutine(showtext(msg));
        }
        else
        {
            school_id = school_ids[school_dropdown.options[school_dropdown.value].text];
            string user_id = "gs" + rollno.text + "_" + school_id;
            Debug.Log(user_id);
            StartCoroutine(Check_user(user_id));

        }
    }
    IEnumerator showtext(string msg)
    {
        ExtramsgBox.text = msg;
        yield return new WaitForSeconds(2f);
        ExtramsgBox.text = "";
    }
    IEnumerator getUserdata()
    {
        string user_id = "gs" + rollno.text + "_" + school_id;
        string register_url = BaseUrl + UserRegister;
        yield return new WaitForSeconds(0.1f);
        if (male.isOn)
        {
            gender = "MALE";
        }
        else if(female.isOn)
        {
            gender = "FEMALE";
        }
        
        WWWForm Post_userData = new WWWForm();
        Post_userData.AddField("FIRSTNAME", Firstname.text);
        Post_userData.AddField("LASTNAME", Lastname.text);
        Post_userData.AddField("MOBILENO", user_phone.text);
        Post_userData.AddField("MAILID", user_email.text);
        Post_userData.AddField("ID_USER", "1");
        Post_userData.AddField("RollNo", rollno.text);
        Post_userData.AddField("address", Useraddress.text);
        Post_userData.AddField("id_school", school_id);
        Post_userData.AddField("userId", user_id);
        Post_userData.AddField("password", AesEncryptor.getEncryptedString("12345"));
        Post_userData.AddField("GENDER", gender);

        WWW Register_www = new WWW(register_url, Post_userData);
        yield return Register_www;
        if (Register_www.text != null)
        {
            Debug.Log("Register response " + Register_www.text);
            JsonData register_response = JsonMapper.ToObject(Register_www.text);
            string status = register_response["response_status"].ToString();
            if (status == "SUCCESS")
            {
                string msg = "You have sucessfully registered\n Check your email for details.";
                StartCoroutine(show_status(msg));

            }
        }
    }

    IEnumerator statusMsg(string msg)
    {
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator Check_user(string userid)
    {
        yield return new WaitForSeconds(0.1f);
        WWWForm user_check = new WWWForm();
        user_check.AddField("UserID", userid);
        string user_checkingUrl = BaseUrl + CheckUserID;
        WWW User_status = new WWW(user_checkingUrl,user_check);
        yield return User_status;
        if(User_status != null)
        {
            JsonData user_response = JsonMapper.ToObject(User_status.text);
            string user = user_response["IsPresent"].ToString();
            if(user.ToLower() == "true")
            {
                Debug.Log("already regsitered");
                string msg = "You have already Registered";
                StartCoroutine(show_status(msg));
                for (int a = 0; a < Allfields.Count; a++)
                {
                    Allfields[a].text = "";
                }
                school_dropdown.value = 0;
            }
            else
            {
                
                StartCoroutine(getUserdata());
            }
            
        }

    }

    IEnumerator show_status(string msg)
    {
        yield return new WaitForSeconds(0.1f);
        statusPanel.SetActive(true);
        StatusBox.transform.GetChild(0).gameObject.GetComponent<Text>().text = msg;
        StatusBox.SetActive(true);
        yield return new WaitForSeconds(2f);
    }

    public void close_msgBox()
    {
        StartCoroutine(closure_box());
    }
    IEnumerator closure_box()
    {
        iTween.ScaleTo(StatusBox, Vector3.zero, 0.5f);
        yield return new WaitForSeconds(0.6f);
        StatusBox.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
        StatusBox.SetActive(false);
        statusPanel.SetActive(false);
    }
}
