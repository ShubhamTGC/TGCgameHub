using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownFilter : MonoBehaviour
{
    [SerializeField]
    private InputField inputField;

    [SerializeField]
    private Dropdown dropdown;

    private List<Dropdown.OptionData> dropdownOptions;

    private void Start()
    {
       
        dropdownOptions = dropdown.options;
    }

  
  
    private void OnEnable()
    {
        
        inputField.ActivateInputField();
    }
    public void FilterDropdown()
    {
        //dropdown.options[0].text = "";
        dropdown.options = dropdownOptions.FindAll(option => option.text.IndexOf(inputField.text) >= 0);
    }
}
