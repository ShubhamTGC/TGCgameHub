using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OTPbarhandler : MonoBehaviour
{
    public List<InputField> OtpBar;
    private int BarCounter;
    private bool ForwardMove, BackwordMove;
    void Start()
    {
        
    }
    private void OnEnable()
    {
        ForwardMove = true;
        BackwordMove = false;
        BarCounter = 0;
        OtpBar[0].ActivateInputField();
    }

    // Update is called once per frame
    void Update()
    {
        //if(OtpBar[BarCounter].text != "" && ForwardMove)
        //{
        //    OtpBar[BarCounter].DeactivateInputField();
        //    BarCounter++;
        //    OtpBar[BarCounter].ActivateInputField();
        //}
        //if(OtpBar[BarCounter].text == "" && BackwordMove)
        //{

        //}
    }


    public void FilledBar()
    {
        if(BarCounter < OtpBar.Count - 1)
        {
            if (BarCounter < OtpBar.Count && OtpBar[BarCounter].text.Length > 0)
            {
                OtpBar[BarCounter].DeactivateInputField();
                BarCounter++;
                OtpBar[BarCounter].ActivateInputField();
            }
        }
    
    }


}
