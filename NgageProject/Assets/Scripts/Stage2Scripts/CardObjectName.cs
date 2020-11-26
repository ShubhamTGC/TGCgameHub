using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardObjectName : MonoBehaviour
{
    // Start is called before the first frame update
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
                string ObjectName = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite.name;
                hit.collider.transform.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = ObjectName;
                hit.collider.transform.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }

        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)), Vector2.zero);
            if (hit.collider.transform.gameObject.name == this.gameObject.name)
            {
                hit.collider.transform.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
                hit.collider.transform.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void OnMouseEnter()
    {
        if(Application.platform == RuntimePlatform.WindowsPlayer)
        {
            string ObjectName = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite.name;
            this.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = ObjectName;
            this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
    

    }
    public void OnMouseExit()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            this.gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
            this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
