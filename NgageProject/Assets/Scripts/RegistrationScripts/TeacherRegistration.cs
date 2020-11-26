using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TeacherRegistration : MonoBehaviour
{

    public string MainUrl, teacherRegisterApi, SchoolListApi, TeacherListApi, GetCountryNameApi;
    public InputField Username, Userid, Grade, Emailid;
    public Dropdown CountryList, TeacherList, school_dropdown;
    private Dictionary<int, string> SchoolIDs = new Dictionary<int, string>();
    private Dictionary<int, string> TeachersIDs = new Dictionary<int, string>();
    private Dictionary<int, string> CountryIDs = new Dictionary<int, string>();
    public Text ExtramsgBox;
    public GameObject PopUppage;
    public Dropdown Gender;
    private string Genderdata;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        if(SchoolIDs.Count == 0)
        {
            StartCoroutine(Get_schoolnames());
            StartCoroutine(getCountryname());
        }
    }

    void ResetForm()
    {
        Username.text = "";
        Grade.text = "";
        Userid.text = "";
        Emailid.text = "";
        school_dropdown.value = 0;
        TeacherList.value = 0;
        CountryList.value = 0;
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

    IEnumerator Get_schoolnames()
    {
        yield return new WaitForSeconds(0.1f);
        string hitting_url = MainUrl + SchoolListApi;
        WWW school_names = new WWW(hitting_url);
        yield return school_names;
        if (school_names.text != null)
        {
            if(school_names.text != "[]")
            {
                List<SchoolListModel> SchoolList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SchoolListModel>>(school_names.text);
                school_dropdown.options.Clear();
                school_dropdown.value = 0;
                school_dropdown.options.Add(new Dropdown.OptionData() { text = "School Name" });
                SchoolList.ForEach(x =>
                {
                    school_dropdown.options.Add(new Dropdown.OptionData() { text = x.school_name });
                    SchoolIDs.Add(x.id_school, x.school_name);
                });
            }
        }
    }

    IEnumerator getCountryname()
    {
        string HittingUrl = $"{MainUrl}{GetCountryNameApi}?countryid=101";
        WWW Request = new WWW(HittingUrl);
        yield return Request;
        if (Request.text != null)
        {
            if (Request.text != "[]")
            {
                List<CountryModel> countrylog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CountryModel>>(Request.text);
                CountryList.options.Clear();
                CountryList.value = 0;
                CountryList.options.Add(new Dropdown.OptionData() { text = "State name..." });
                countrylog.ForEach(x =>
                {
                    CountryList.options.Add(new Dropdown.OptionData() { text = x.name });
                    CountryIDs.Add(x.id, x.name);
                });
            }
        }
    }

    public void GetSchoolId()
    {
        int Schoolname = SchoolIDs.FirstOrDefault(x => x.Value == school_dropdown.options[school_dropdown.value].text).Key;
        if (Schoolname != 0)
        {
            StartCoroutine(GetTeacherList(Schoolname));
        }
    }

    IEnumerator GetTeacherList(int ID)
    {
        TeachersIDs.Clear();
        string HittingUrl = $"{MainUrl}{TeacherListApi}?id_school={ID}";
        WWW teacherwww = new WWW(HittingUrl);
        yield return teacherwww;
        if (teacherwww.text != null)
        {
            if (teacherwww.text != "[]")
            {
                List<TeacherListModel> TeacherLog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TeacherListModel>>(teacherwww.text);
                TeacherList.options.Clear();
                TeacherList.value = 0;
                TeacherList.options.Add(new Dropdown.OptionData() { text = "Teacher's Id" });
                TeacherLog.ForEach(x =>
                {
                    TeacherList.options.Add(new Dropdown.OptionData() { text = x.FIRSTNAME });
                    TeachersIDs.Add(x.ID_USER, x.FIRSTNAME);
                });
            }
            else
            {
                TeacherList.options.Clear();
                TeacherList.value = 0;
                TeacherList.options.Add(new Dropdown.OptionData() { text = "No Teacher's available" });
            }
        }
        else
        {
            string msg = "Check Your Internet Connection!";
            StartCoroutine(showtext(msg));
        }

    }




    public void teacherregister()
    {
        if (Username.text == "" || Grade.text == "" || Userid.text == "" || school_dropdown.value == 0 || TeacherList.value == 0 
            || CountryList.value == 0 ||Emailid.text == "")
        {
            string msg = "Please fill the required information.";
            StartCoroutine(showtext(msg));
        }
        else
        {

            StartCoroutine(getRegisterTeacher());
        }
    }

    
   
    
    IEnumerator getRegisterTeacher()
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
        yield return new WaitForSeconds(0f);
        string HittingUrl = $"{MainUrl}{teacherRegisterApi}";
        TeacherRegisterModel teacherlog = new TeacherRegisterModel
        {
            TeacherName = Username.text,
            Grade = Grade.text,
            teacherEmpId = TeachersIDs.FirstOrDefault(x => x.Value == TeacherList.options[TeacherList.value].text).Key.ToString(),
            id_school = SchoolIDs.FirstOrDefault(x => x.Value == school_dropdown.options[school_dropdown.value].text).Key.ToString(),
            id_state = CountryIDs.FirstOrDefault(x => x.Value == CountryList.options[CountryList.value].text).Key.ToString(),
            EmailId = Emailid.text,
            TeacherUserId = Userid.text,
            Gender = Genderdata
        };

        string Logdata = Newtonsoft.Json.JsonConvert.SerializeObject(teacherlog);
        Debug.Log("log data " + Logdata);
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
                        ResetForm();
                    }
                }
            }
            else
            {
                Debug.Log(" Registration msg =" + Request.downloadHandler.text);
                string msg = "Check Your Internet Connection!";
                StartCoroutine(showtext(msg));
                ResetForm();
            }
        }

    }
}
