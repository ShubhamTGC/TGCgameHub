using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage3localdashboard : MonoBehaviour
{
    public string tablename;
    public Text Tabledata;
    public GameObject Nextbtn, BackBtn, Prioritytable, drivingtable;

    private void OnEnable()
    {
        Tabledata.text = tablename;
        Nextbtn.SetActive(true);
        BackBtn.SetActive(false);
        Prioritytable.SetActive(true);
        drivingtable.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
