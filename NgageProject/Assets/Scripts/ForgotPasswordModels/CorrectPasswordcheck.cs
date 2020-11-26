using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorrectPasswordcheck : MonoBehaviour
{
    public InputField passwordfield,Confirmpassword;

    public Color NotEqual;
    public Color Equal;
    public GameObject Star;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Star.SetActive(Confirmpassword.text.Length > 0);
        Star.GetComponent<Image>().color = Confirmpassword.text == passwordfield.text ? Equal : NotEqual;
    }
}
