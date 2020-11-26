using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ForgotPageHandler : MonoBehaviour
{
    public GameObject PageFields,PopUpPage,EmailVarificationPage,OTPPagevalidation,NewPasswordPage;
    public InputField EmailInput,UserID;
    public InputField useridinput, NewpasswordInput, ConfirmPasswordInput;
    public Button Verifybtn,Donebtn;
    public Text Showmsg;
    public AESalgorithm AesAlgo;
    private string useridtext;
    [Space(15)]
    [Header("API INTEGRATION")]
    public string MainUrl;
    public string EmailvarificationApi, PasswordUpdateApi;
    public Button NewpassShowBtn, ConfirmPassShowBtn;
    public Sprite CloseEye, OpenEye;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        EmailInput.text = "";
        UserID.text = "";   
        useridinput.text = "";
        NewpasswordInput.text = "";
        ConfirmPasswordInput.text = "";
        EmailVarificationPage.SetActive(true);
        NewPasswordPage.SetActive(false);
        PageFields.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        // Verifybtn.interactable = EmailInput.text != "";
        Donebtn.interactable = NewpasswordInput.text == ConfirmPasswordInput.text;

    }

    public void BackToLoginPage()
    {
        StartCoroutine(Backtask());
    }

    IEnumerator Backtask()
    {
        iTween.ScaleTo(PageFields, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        PageFields.SetActive(false);
        this.gameObject.SetActive(false);

    }

    public void CheckEmailID()
    {
        if(UserID.text != "" && EmailInput.text != "")
        {
            StartCoroutine(CheckEmailTask());
        }
        else
        {
            string msg = "Please fill required details!";
            StartCoroutine(ShowMsgPopup(msg));
        }
    
    }

    IEnumerator CheckEmailTask()
    {
        useridtext = UserID.text;
        string HittingUrl = $"{MainUrl}{EmailvarificationApi}";
        EmailIDValidation EmailLog = new EmailIDValidation
        {
            UserId = UserID.text,
            MailId = EmailInput.text
        };
        string logData = Newtonsoft.Json.JsonConvert.SerializeObject(EmailLog);

        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, logData))
        {
            Request.method = UnityWebRequest.kHttpVerbPOST;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                Debug.Log(Request.downloadHandler.text);
                if (Request.downloadHandler.text != null)
                {
                    if(Request.downloadHandler.text != "[]")
                    {
                        Validesponse responseLog = Newtonsoft.Json.JsonConvert.DeserializeObject<Validesponse>(Request.downloadHandler.text);
                        if (responseLog.Status.Equals("SUCCESS", System.StringComparison.OrdinalIgnoreCase))
                        {
                            StartCoroutine(Emailvarification());
                            UserID.text = "";
                            EmailInput.text = "";
                        }
                        else
                        {
                            string msg = responseLog.Message;
                            StartCoroutine(ShowMsgPopup(msg));
                        }
                    }
                }
            }
            else
            {
                string msg = "Please check your internet Connectivity and Try Again later.";
                StartCoroutine(ShowMsgPopup(msg));
            }
        }

     
    }

    IEnumerator Emailvarification()
    {
        yield return new WaitForSeconds(0.1f);
        string msg = "Found a Valid User!";
        StartCoroutine(ShowMsgPopup(msg));
        yield return new WaitForSeconds(4f);
        useridinput.text = useridtext;
        EmailVarificationPage.SetActive(false);
        NewPasswordPage.SetActive(true);
    }

    IEnumerator ShowMsgPopup(string msg)
    {
        PopUpPage.SetActive(true);
        Showmsg.text = msg;
        iTween.ScaleTo(PopUpPage, Vector3.one, 0.4f);
        yield return new WaitForSeconds(3f);
        iTween.ScaleTo(PopUpPage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        Showmsg.text = "";
        PopUpPage.SetActive(false);
    }

    public void UpdatePassword()
    {
        if(useridinput.text != "" && NewpasswordInput.text != "" && ConfirmPasswordInput.text != "")
        {
            StartCoroutine(UpdatePasswordTask());
        }
        else
        {
            string msg = "Please fill required details!";
            StartCoroutine(ShowMsgPopup(msg));
        }
    }

    IEnumerator UpdatePasswordTask()
    {
        string EncrypredPass = AesAlgo.getEncryptedString(NewpasswordInput.text);
        string HittingUrl = $"{MainUrl}{PasswordUpdateApi}";

        PasswordUpdate passLog = new PasswordUpdate
        {
            UserId = useridinput.text,
            Password = EncrypredPass
        };

        string Logdata = Newtonsoft.Json.JsonConvert.SerializeObject(passLog);

        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, Logdata))
        {
            Request.method = UnityWebRequest.kHttpVerbPOST;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                Debug.Log(Request.downloadHandler.text);
                if (Request.downloadHandler.text != null)
                {
                    if (Request.downloadHandler.text != "[]")
                    {
                        PasswordResponse responseLog = Newtonsoft.Json.JsonConvert.DeserializeObject<PasswordResponse>(Request.downloadHandler.text);
                        if (responseLog.Status.Equals("SUCCESS", System.StringComparison.OrdinalIgnoreCase))
                        {
                            useridinput.text = "";
                            NewpasswordInput.text = "";
                            ConfirmPasswordInput.text = "";
                            string msg = responseLog.Message;
                            StartCoroutine(ShowMsgPopup(msg));
                            yield return new WaitForSeconds(4);
                            BackToLoginPage();
                        }
                        else
                        {
                            string msg = responseLog.Message;
                            StartCoroutine(ShowMsgPopup(msg));
                        }
                    }
                }
            }
            else
            {
                string msg = "Please check your internet Connectivity and Try Again later.";
                StartCoroutine(ShowMsgPopup(msg));
            }
        }
    }

    public void NewShowOrHidePass(InputField PassField)
    {
        if(NewpassShowBtn.image.sprite.name.Equals("CloseEye",System.StringComparison.OrdinalIgnoreCase))
        {
            PassField.contentType = InputField.ContentType.Standard;
            PassField.ForceLabelUpdate();
            NewpassShowBtn.image.sprite = OpenEye;
        }
        else
        {
            PassField.contentType = InputField.ContentType.Password;
            PassField.ForceLabelUpdate();
            NewpassShowBtn.image.sprite = CloseEye;
        }
       
    }
    public void ConfirmShowOrHidePass(InputField PassField)
    {
        if (ConfirmPassShowBtn.image.sprite.name.Equals("CloseEye", System.StringComparison.OrdinalIgnoreCase))
        {
            PassField.contentType = InputField.ContentType.Standard;
            PassField.ForceLabelUpdate();
            ConfirmPassShowBtn.image.sprite = OpenEye;
        }
        else
        {
            PassField.contentType = InputField.ContentType.Password;
            PassField.ForceLabelUpdate();
            ConfirmPassShowBtn.image.sprite = CloseEye;
        }
   
    }
}
