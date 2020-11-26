using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotFilledIndication : MonoBehaviour
{
    public GameObject Star;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (this.gameObject.GetComponent<InputField>())
        {
            Star.SetActive(this.gameObject.GetComponent<InputField>().text == "");
        }
        else if (this.gameObject.GetComponent<Dropdown>())
        {
            Star.SetActive(this.gameObject.GetComponent<Dropdown>().value == 0);
        }
    }
}
