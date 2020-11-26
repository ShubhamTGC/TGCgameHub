using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;
using UnityEngine.UI.Extensions;

public class Studentregistration : MonoBehaviour
{
    // Start is called before the first frame update
    public string MainUrl, SchoolListApi, CheckUserID,TeacherListApi,StudentRegisterApi, GetCountryNameApi;
    public Dropdown school_dropdown, TeacherName, Countrynames,Gender;
    public InputField Username, Grade, UserId, Emailid;
    private Dictionary<string, int> SchoolIdList = new Dictionary<string, int>();
    public GameObject PopUppage;
    public Text ExtramsgBox;
    [SerializeField] private List<InputField> Allfields;
    private  Dictionary<int, string> SchoolIDs = new Dictionary<int, string>();
    private Dictionary<int, string> TeachersIDs = new Dictionary<int, string>();
    private Dictionary<int, string> CountryIDs = new Dictionary<int, string>();
    public List<Dropdown.OptionData> schooloptions;
    public InputField schoolsearchBar;
    private string Genderdata;
    void Start()
    {
        schooloptions = school_dropdown.options;
    }

    private void OnEnable()
    {
        ResetForm();
        if (SchoolIdList.Count == 0)
        {
            StartCoroutine(Get_schoolnames());
            StartCoroutine(getCountryname());
        }
      
    }

    void ResetForm()
    {
        Username.text = "";
        Grade.text = "";
        UserId.text = "";
        Emailid.text = "";
        school_dropdown.value = 0;
        TeacherName.value = 0;
        Countrynames.value = 0;
        Gender.value = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        CountryIDs.Clear();
        TeachersIDs.Clear();
        SchoolIDs.Clear();
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
                    //SchoolBox.AvailableOptions.Add(x.school_name);
                });
                schooloptions = school_dropdown.options;
                //schooloptions.
            }
        }
    }

    IEnumerator getCountryname()
    {
        string HittingUrl = $"{MainUrl}{GetCountryNameApi}?countryid=101";
        WWW Request = new WWW(HittingUrl);
        yield return Request;
        if(Request.text != null)
        {
            if(Request.text != "[]")
            {
                List<CountryModel> countrylog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CountryModel>>(Request.text);
                Countrynames.options.Clear();
                Countrynames.value = 0;
                Countrynames.options.Add(new Dropdown.OptionData() { text = "State name..." });
                countrylog.ForEach(x =>
                {
                    Countrynames.options.Add(new Dropdown.OptionData() { text = x.name });
                    CountryIDs.Add(x.id, x.name);
                });
            }
        }
    }


    public void RegisterUser()
    {
        if (Username.text == ""  || Grade.text == "" || UserId.text == ""  || school_dropdown.value == 0 || TeacherName.value == 0  || Countrynames.value == 0 
           || Emailid.text == "")
        {
            string msg = "Please fill the required information.";
            StartCoroutine(showtext(msg));
        }
        else
        {
           
            StartCoroutine(Check_user(UserId.text));
        }
    }

    IEnumerator Check_user(string userid)
    {
        yield return new WaitForSeconds(0.1f);
        WWWForm user_check = new WWWForm();
        user_check.AddField("UserID", userid);
        string user_checkingUrl = MainUrl + CheckUserID;
        WWW User_status = new WWW(user_checkingUrl, user_check);
        yield return User_status;
        if (User_status != null)
        {
            JsonData user_response = JsonMapper.ToObject(User_status.text);
            string user = user_response["IsPresent"].ToString();
            if (user.ToLower() == "true")
            {
                Debug.Log("already regsitered");
                string msg = "You have already Registered";
                StartCoroutine(showtext(msg));
            }
            else
            {
                StartCoroutine(GetStudentdata());
            }

        }
        else
        {
            string msg = "Check Your Internet Connection!";
            StartCoroutine(showtext(msg));
        }

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
        if(teacherwww.text != null)
        {
            if(teacherwww.text != "[]")
            {
                List<TeacherListModel> TeacherLog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TeacherListModel>>(teacherwww.text);
                TeacherName.options.Clear();
                TeacherName.value = 0;
                TeacherName.options.Add(new Dropdown.OptionData() { text = "Teacher's name" });
                TeacherLog.ForEach(x =>
                {
                    TeacherName.options.Add(new Dropdown.OptionData() { text = x.FIRSTNAME});
                    TeachersIDs.Add(x.ID_USER, x.FIRSTNAME);
                });
            }
            else
            {
                TeacherName.options.Clear();
                TeacherName.value = 0;
                TeacherName.options.Add(new Dropdown.OptionData() { text = "No Teacher's available" });
            }
        }
        else
        {
            string msg = "Check Your Internet Connection!";
            StartCoroutine(showtext(msg));
        }

    }
    IEnumerator GetStudentdata()
    {
        yield return new WaitForSeconds(0f);
        string teacherid = TeachersIDs.FirstOrDefault(x => x.Value == TeacherName.options[TeacherName.value].text).Key.ToString();
        string gendervalue = Gender.options[Gender.value].text;
        if (gendervalue.Equals("male", System.StringComparison.OrdinalIgnoreCase))
        {
            Genderdata = "M";
        }
        else
        {
            Genderdata = "F";
        }
        string HittingUrl = $"{MainUrl}{StudentRegisterApi}";
        UserRegistrationModel UserLog = new UserRegistrationModel
        {
            StudentName = Username.text,
            Grade = Grade.text,
            StudentUserId = UserId.text,
            EmailId = Emailid.text,
            TeacherId = TeachersIDs.FirstOrDefault(x => x.Value == TeacherName.options[TeacherName.value].text).Key.ToString(),
            id_school = SchoolIDs.FirstOrDefault(x => x.Value == school_dropdown.options[school_dropdown.value].text).Key.ToString(),
            id_state =  CountryIDs.FirstOrDefault(x => x.Value == Countrynames.options[Countrynames.value].text).Key.ToString(),
            Gender = Genderdata
        };

        string RegistrationLog = Newtonsoft.Json.JsonConvert.SerializeObject(UserLog);
        Debug.Log("Complete log  " + RegistrationLog);
        using (UnityWebRequest Request = UnityWebRequest.Put(HittingUrl, RegistrationLog))
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


    public void FilterDropdown()
    {
        school_dropdown.options = schooloptions.FindAll(option => option.text.IndexOf("United") >= 0);
    }
}
