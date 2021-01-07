using LitJson;
using m2ostnextservice.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RegistrationPageHandler : MonoBehaviour
{
    
    public List<GameObject> tabs;
    public List<GameObject> Forms;
    public Sprite Pressed, notPressed;
    private int UserRole;

    public InputField Username, Emailid, Password, ConfirmPassword, mobileNo, orgname;
    public GameObject PopupPage;
    public Text Msgbox;
    public Image MoodImage;
    public Sprite Happy, sad;
    private AESAlgorithm aes;
    public GameObject Registerpage, Loginpage, MainLoginPage, startPage;
    public Scrollbar Formpage;
    public Button Hidepasswordbtn, HideConfmpasswordbtn;
    public Sprite Closeeye, Openeye;
    void Start()
    {
        
    }

    

 
    void Update()
    {
      
    }

    void ResetForm()
    {
        Username.text = "";
        Emailid.text = "";
        Password.text = "";
        ConfirmPassword.text = "";
        mobileNo.text = "";
        orgname.text = "";
        Formpage.value = 1;
    }

     void OnEnable()
    {
        ResetForm();
        aes = new AESAlgorithm();
        for (int a = 0; a < tabs.Count; a++)
        {
            if (a == 0)
            {
                Forms[a].SetActive(true);
                UserRole = 1;
                tabs[a].GetComponent<Image>().sprite = Pressed;
            }
            else
            {
                Forms[a].SetActive(false);
                tabs[a].GetComponent<Image>().sprite = notPressed;
            }
        }

    }

    

    public void SelectRoleTab(GameObject tab)
    {
        
        tabs.ForEach(x =>
        {
            x.GetComponent<Image>().sprite = x.name == tab.name ? Pressed : notPressed;
            
        });
        Forms.ForEach(a =>
        {
            a.SetActive(a.name == tab.name ? true : false);
        });
        int index = tabs.FindIndex(y => y.name == tab.name);
        UserRole = index + 1;

    }

    IEnumerator ShowPopUp(string msg, Sprite mood)
    {
        PopupPage.SetActive(true);
        MoodImage.sprite = mood;
        Msgbox.text = msg;
        yield return new WaitForSeconds(2.5f);
        iTween.ScaleTo(PopupPage, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.35f);
        Msgbox.text = "";
        PopupPage.SetActive(false);
    }

    public void registerYourself()
    {
        if(Username.text != "" && Emailid.text != "" && Password.text != "" && ConfirmPassword.text != "")
        {
            if (Password.text.Equals(ConfirmPassword.text, System.StringComparison.OrdinalIgnoreCase))
            {
                StartCoroutine(RegsiterTask());
            }
            else
            {
                string msg = "Password not matched!";
                StartCoroutine(ShowPopUp(msg, sad));
            }
        }
        else
        {
            string msg = "please fill required details!!!";
            StartCoroutine(ShowPopUp(msg, sad));
        }
    }

    IEnumerator RegsiterTask()
    {
        yield return new WaitForSeconds(0.1f);
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.RegistrationApi}";
        userRegistrationModel Model = new userRegistrationModel
        {
            Name = Username.text,
            Email = Emailid.text,
            Password = aes.getEncryptedString(Password.text),
            Phone_No = mobileNo.text,
            Organization_Name = orgname.text,
            login_type = 1,
        };

        

        string NormalLog = Newtonsoft.Json.JsonConvert.SerializeObject(Model);
        string encryptedlog = aes.getEncryptedString(NormalLog);
        CommonModel postlog = new CommonModel
        {
            Data = encryptedlog
        };
        string Finallog = Newtonsoft.Json.JsonConvert.SerializeObject(postlog);
        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, Finallog))
        {
            Request.method = UnityWebRequest.kHttpVerbPOST;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                if (Request.downloadHandler.text != null)
                {
                    if (Request.downloadHandler.text != "")
                    {
                        Debug.Log("msg " + Request.downloadHandler.text);
                        string msg = "User registered Successfully !";
                        StartCoroutine(ShowPopUp(msg, Happy));
                        yield return new WaitForSeconds(3.5f);
                        ResetForm();
                        startPage.SetActive(true);
                        MainLoginPage.SetActive(false);
                        Loginpage.SetActive(true);
                        Registerpage.SetActive(false);
                    }
                }
                else
                {
                    Debug.Log("log  issue");
                    string msg = "Invaild email id or password!";
                    StartCoroutine(ShowPopUp(msg, sad));
                }
                
            }
            else
            {
                Debug.Log("net issue");
                string msg = "Invaild email id or password!";
                StartCoroutine(ShowPopUp(msg, sad));
            }
        }
    }

    public void Showpassword()
    {
        if (Hidepasswordbtn.image.sprite.name.Equals("closeeye", System.StringComparison.OrdinalIgnoreCase))
        {
            Hidepasswordbtn.image.sprite = Openeye;
            Password.inputType = InputField.InputType.Standard;
            Password.ForceLabelUpdate();
        }
        else
        {
            Hidepasswordbtn.image.sprite = Closeeye;
            Password.inputType = InputField.InputType.Password;
            Password.ForceLabelUpdate();
        }
    }
    public void ShowConfrmpassword()
    {
        if (HideConfmpasswordbtn.image.sprite.name.Equals("closeeye", System.StringComparison.OrdinalIgnoreCase))
        {
            HideConfmpasswordbtn.image.sprite = Openeye;
            ConfirmPassword.inputType = InputField.InputType.Standard;
            ConfirmPassword.ForceLabelUpdate();
        }
        else
        {
            HideConfmpasswordbtn.image.sprite = Closeeye;
            ConfirmPassword.inputType = InputField.InputType.Password;
            ConfirmPassword.ForceLabelUpdate();
        }
    }

}
