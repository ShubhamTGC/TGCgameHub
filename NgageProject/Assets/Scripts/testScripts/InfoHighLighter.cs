using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoHighLighter : MonoBehaviour
{
    // Start is called before the first frame update
    public Text zoneinfo;
    public string zonemsg;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)), Vector2.zero);
            if (hit.collider.transform.gameObject.name == this.gameObject.name)
            {
                zoneinfo.gameObject.SetActive(true);
                zoneinfo.text = zonemsg;
            }

        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)), Vector2.zero);
            if (hit.collider.transform.gameObject.name == this.gameObject.name)
            {
                zoneinfo.gameObject.SetActive(false);
                zoneinfo.text = "";
            }
        }
    }


    public void OnMouseEnter()
    {
        if (gameObject.name == this.gameObject.name)
        {
            
            zoneinfo.gameObject.SetActive(true);
            zoneinfo.text = zonemsg;
        }
    }

    public void OnMouseExit()
    {
        if (gameObject.name == this.gameObject.name)
        {
            zoneinfo.gameObject.SetActive(false);
            zoneinfo.text = "";
        }
        
    }
}
