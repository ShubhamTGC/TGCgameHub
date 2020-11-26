using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zoneselection : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject startpage;
    public Text zoneinfo;
    public string zonemsg;
    void Update()
    {
      
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)), Vector2.zero);
            if (hit.collider.transform.gameObject.name == this.gameObject.name)
            {
                zoneinfo.text = zonemsg;
                startpage.GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.6f, 1);
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }

        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)), Vector2.zero);
            if (hit.collider.transform.gameObject.name == this.gameObject.name)
            {
                zoneinfo.text = "";
                startpage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
                this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        
       
    }

    public void OnMouseEnter()
    {
        if(gameObject.name == this.gameObject.name)
        {
            zoneinfo.text = zonemsg;
        }
        startpage.GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f, 1);
        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
    
    public void OnMouseExit()
    {
        if (gameObject.name == this.gameObject.name)
        {
            zoneinfo.text = "";
        }
        startpage.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1);
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }
    
}
