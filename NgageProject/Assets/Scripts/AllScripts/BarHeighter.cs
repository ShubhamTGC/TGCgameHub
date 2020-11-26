using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarHeighter : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite heighlitedimage, normalimage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnMouseEnter()
    {
        this.gameObject.GetComponent<Image>().sprite = heighlitedimage;
    }

    public void OnMouseExit()
    {
        this.gameObject.GetComponent<Image>().sprite = normalimage;
    }
}
