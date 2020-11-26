using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class seperationGame : MonoBehaviour
{
    public List<GameObject> wasteobejcts;
    private bool ismoving;
    private RaycastHit2D hit;
    private Vector2 mousepos;
    private Vector2 initialpos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenpt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos = new Vector2(screenpt.x, screenpt.y);
            hit = Physics2D.Raycast(mousepos, Vector2.zero);
            if(hit.collider.tag == "waste")
            {
                initialpos = hit.transform.gameObject.GetComponent<RectTransform>().position;
                ismoving = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            ismoving = false;
            if(hit.transform.gameObject.GetComponent<Targetcollider>().correcttarget == true)
            {
                GameObject gb = hit.transform.gameObject.GetComponent<Targetcollider>().targetbody;
                gb.transform.GetChild(0).gameObject.SetActive(true);
                gb.transform.GetChild(1).gameObject.GetComponent<Text>().text = "+5";
                gb.transform.GetChild(1).gameObject.GetComponent<zoomAnim>().enabled = true;
                StartCoroutine(scaledownObject(gb.transform.GetChild(1).gameObject, 1.5f));
                Destroy(hit.transform.gameObject);
            }
            else if(hit.transform.gameObject.GetComponent<Targetcollider>().wrongtarget == true)
            {
                GameObject gb = hit.transform.gameObject.GetComponent<Targetcollider>().wrongbin;
                gb.transform.GetChild(2).gameObject.SetActive(true);
                gb.transform.GetChild(1).gameObject.GetComponent<Text>().text = "-5";
                gb.transform.GetChild(1).gameObject.GetComponent<zoomAnim>().enabled = true;
                StartCoroutine(scaledownObject(gb.transform.GetChild(1).gameObject, 1.5f));
                hit.transform.gameObject.GetComponent<RectTransform>().position = initialpos;
                hit.transform.gameObject.GetComponent<Targetcollider>().wrongtarget = false;
            }
        }

        if (ismoving)
        {
            Vector3 targetpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit.transform.gameObject.GetComponent<RectTransform>().position = new Vector2(targetpos.x,targetpos.y);
        }
    }

    IEnumerator scaledownObject(GameObject scoreobj,float time)
    {
       
        yield return new WaitForSeconds(time);
        scoreobj.GetComponent<Text>().text = "";
        iTween.ScaleTo(scoreobj, Vector2.zero, 1);
        scoreobj.GetComponent<zoomAnim>().enabled = false;
    }
    
}
