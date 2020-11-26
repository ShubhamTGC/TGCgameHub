using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;

public class Parentregistration : MonoBehaviour
{
    public string MainUrl, ParentregistrationApi;
    public InputField Userid, parentName, Emailid, studentid;
    private Dictionary<int, string> CountryData = new Dictionary<int, string>();
    public Text ExtramsgBox;
    public GameObject PopUppage;
    public Dropdown Gender;
    private string Genderdata;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        resetForm();
    }

   void resetForm()
    {
        Userid.text = "";
        parentName.text = "";
        Emailid.text = "";
        studentid.text = "";
        Gender.value = 0;

    }


    IEnumerator showtext(string msg)
    {
        ExtramsgBox.text = msg;
        PopUppage.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        ExtramsgBox.text = "";
        iTween.ScaleTo(PopUppage, Vector3.zero, 0.2f);
        yield return new WaitForSeconds(0.3f);
        PopUppage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ParentRegister()
    {
        if (parentName.text == "" || studentid.text == "" || Userid.text == ""   || Emailid.text == "")
        {
            string msg = "Please fill the required information.";
            StartCoroutine(showtext(msg));
        }
        else
        {

            StartCoroutine(GetParentRegistered());
        }
    }

    IEnumerator GetParentRegistered()
    {
        string gendervalue = Gender.options[Gender.value].text;
        if (gendervalue.Equals("male", System.StringComparison.OrdinalIgnoreCase))
        {
            Genderdata = "M";
        }
        else
        {
            Genderdata = "F";
        }
        string HittingUrl = $"{MainUrl}{ParentregistrationApi}";

        ParentModel ParentLog = new ParentModel
        {
            ParentName = parentName.text,
            StudentsUserId = studentid.text,
            ParentUserId = Userid.text,
            EmailId = Emailid.text,
            Gender = Genderdata
        };

        string Logdata = Newtonsoft.Json.JsonConvert.SerializeObject(ParentLog);
        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, Logdata))
        {
            Request.method = UnityWebRequest.kHttpVerbPOST;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                if (Request.downloadHandler.text != null)
                {
                    Debug.Log(" Registration msg =" + Request.downloadHandler.text);
                    RegisterResponseModel log = Newtonsoft.Json.JsonConvert.DeserializeObject<RegisterResponseModel>(Request.downloadHandler.text);
                    if (log.STATUS.Equals("success", System.StringComparison.OrdinalIgnoreCase))
                    {
                        string msg = log.Message;
                        StartCoroutine(showtext(msg));
                        resetForm();
                    }
                    else
                    {
                        string msg = log.Message;
                        StartCoroutine(showtext(msg));
                    }
                }
            }
            else
            {
                Debug.Log(" Registration msg =" + Request.downloadHandler.text);
                string msg = "Check Your Internet Connection!";
                StartCoroutine(showtext(msg));
            }

        }
    }
}
