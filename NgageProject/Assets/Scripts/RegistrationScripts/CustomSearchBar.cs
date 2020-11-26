using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class CustomSearchBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Dropdown school_dropdown;
    public GameObject searchbar;
   [SerializeField] private List<string> Listdata = new List<string>();
    public InputField dataField;
   [SerializeField] private List<string> Tempdata = new List<string>();
    private Dictionary<int, string> SchoolIDs = new Dictionary<int, string>();
    private List<Dropdown.OptionData> optiondata = new List<Dropdown.OptionData>();
    [SerializeField] private List<Dropdown.OptionData> Logdata = new List<Dropdown.OptionData>();

    void Start()
    {
        StartCoroutine(Get_schoolnames());
      //  optiondata = school_dropdown.options;
       // dataField.onValueChanged.AddListener(FilterDropdown);
    }

    

    IEnumerator Get_schoolnames()
    {
        yield return new WaitForSeconds(0.1f);
        string hitting_url = "https://www.skillmuni.in/wsmapi/api/" + "getWSSchoolList";
        WWW school_names = new WWW(hitting_url);
        yield return school_names;
        if (school_names.text != null)
        {
            if (school_names.text != "[]")
            {
                List<SchoolListModel> SchoolList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SchoolListModel>>(school_names.text);
                school_dropdown.options.Clear();
                school_dropdown.value = 0;
                school_dropdown.options.Add(new Dropdown.OptionData() { text = "School Name" });
                SchoolList.ForEach(x =>
                {
                    school_dropdown.options.Add(new Dropdown.OptionData() { text = x.school_name });
                    Listdata.Add(x.school_name);
                    SchoolIDs.Add(x.id_school, x.school_name);
                });

            }
        }
    }
    private void OnEnable()
    {
        //dataField.onValueChanged.AddListener(FilterDropdown);
    }

    // Update is called once per frame
    void Update()
    {
      
       //  searchbar.SetActive(school_dropdown.transform.childCount > 4);
         //dataField.ActivateInputField();
        
     
    }


    public void FilterDropdown(string Data)
    {
        Debug.Log("data " + Data);
        Tempdata = GetResults(Data);
        //Logdata = GetResults(Data);
        school_dropdown.options.Clear();
        school_dropdown.value = 0;
        school_dropdown.options.Add(new Dropdown.OptionData() { text = "School Name" });
        Tempdata.ForEach(x =>
        {
            school_dropdown.options.Add(new Dropdown.OptionData() { text = x });
        });
        StartCoroutine(refreshpage());
       
        //school_dropdown.AddOptions(logdata);


    }
    IEnumerator refreshpage()
    {
        school_dropdown.Hide();
        yield return new WaitForSeconds(0.2f);
        school_dropdown.Show();
    }
    private List<string> GetResults(string input)
    {
        Logdata.Clear();
        if (input == null)
        {
            return Listdata;
        }
      
       // return optiondata.FindAll(x => (x.text.ToLower() ?? "").StartsWith(input.ToLower()));
        return Listdata.FindAll(x => (x ?? "").Trim().ToLower().Contains(input.Trim().ToLower())).ToList();
    }

    public void GetSchoolId()
    {
        int Schoolname = SchoolIDs.FirstOrDefault(x => x.Value == school_dropdown.options[school_dropdown.value].text).Key;
        Debug.Log("id of scahool " + Schoolname);
        if (Schoolname != 0)
        {
            //StartCoroutine(GetTeacherList(Schoolname));
        }
    }
}
