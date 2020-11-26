using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wasteproductMsg : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject productinfomsg;

    public bool check = false;
    public Level1 level1;
    //private Vector3 mousepos;
    private bool ismoving;
    private RaycastHit2D hit;
    private Vector2 mousepos;
    private Vector2 initialpos;
    private bool canblast = false,isfirst=false;
    

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenpt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos = new Vector2(screenpt.x, screenpt.y);
            hit = Physics2D.Raycast(mousepos, Vector2.zero);
            if (hit.collider.tag == "waste")
            {
                initialpos = hit.transform.gameObject.GetComponent<RectTransform>().position;
                ismoving = true;
                if(hit.transform.gameObject.name == this.gameObject.name)
                {
                    if (!isfirst)
                    {
                        isfirst = true;
                        this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                        StartCoroutine(msgbox());
                    }
                }
               
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            ismoving = false;
            if (!canblast)
            {
                hit.transform.gameObject.GetComponent<RectTransform>().position = initialpos;
            }
            else
            {
                if (!check)
                {
                    check = true;
                    level1.clickcounter += 1;
                }
                this.gameObject.SetActive(false);
            }
           
        }

       

        if (ismoving)
        {
            Vector2 targetpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit.transform.gameObject.GetComponent<RectTransform>().position = new Vector3(targetpos.x, targetpos.y,0f);
        }
    }

    IEnumerator msgbox()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "dusbin")
        {
            canblast = true;
            other.gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, 50f);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "dusbin")
        {
            canblast = false;
            other.gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }
}
