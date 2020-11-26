using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class wasteCollection : MonoBehaviour
{

    [Header("==scoring elements===")]
    public int correctpoint;
    public int partiallycorrectpoint, wrongpoint;
    public Zonehandler zonewaste;
    public bool check = false;
    private bool ismoving;
    private RaycastHit2D hit;
    private Vector2 mousepos;
    private Vector2 initialpos;
    private bool canblast = false, isfirst = false;
    private bool reduce = false, reuse = false, recycle=false;
    public Vector2 collidersize;
    public GameObject collidedDustbin;
    public AudioClip correct_clip, wrong_clip;
    private Generationlevel startpage;
    public Sprite ContainerWithled, containerWithoutLed;
    private string containername;

    public void Start()
    {
       startpage = FindObjectOfType<Generationlevel>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenpt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos = new Vector2(screenpt.x, screenpt.y);
            hit = Physics2D.Raycast(mousepos, Vector2.zero);
          
            if(hit != null && hit.collider != null && hit.collider.tag == "reduce")
            {
                hit.transform.parent.gameObject.transform.SetAsLastSibling();
                if (hit.collider.gameObject.name == this.gameObject.name)
                {
                    collidersize = hit.collider.gameObject.GetComponent<BoxCollider2D>().size;
                    hit.collider.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(30, 30);
                }
                hit.collider.gameObject.transform.GetComponentInParent<Image>().enabled = false;
                initialpos = hit.transform.gameObject.GetComponent<RectTransform>().localPosition;

                hit.collider.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                hit.collider.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                ismoving = true;
            }

        }




        if (Input.GetMouseButtonUp(0))
        {
            ismoving = false;
            if (!canblast)
            {
                if(hit != null && hit.collider != null && hit.collider.tag == "reduce")
                {
                    if(hit.collider.gameObject.name == this.gameObject.name)
                    {
                        hit.collider.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(collidersize.x, collidersize.y);
                        hit.transform.gameObject.GetComponent<RectTransform>().localPosition = initialpos;
                        hit.collider.gameObject.transform.GetComponentInParent<Image>().enabled = true;
                        hit.collider.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                        hit.collider.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
            }
            else
            {

                if (hit != null && hit.collider != null && hit.collider.tag == "reduce")
                {
                    if (hit.collider.gameObject.name == this.gameObject.name)
                    {
                        hit.collider.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(collidersize.x, collidersize.y);
                        hit.collider.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                    }
                }
                if (!check)
                {
                    check = true;
                    startpage.VibrateDevice();
                    zonewaste.waste_count += 1;
                    if(hit.collider.tag == "reduce")
                    {
                        hit.collider.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(collidersize.x, collidersize.y);
                    }
                  
                    zonewaste.CheckcollisionResult(containername, hit.collider.gameObject.name);


                }
             
                zonewaste.collected_count += 1;
                zonewaste.score_check = true;
                this.gameObject.SetActive(false);
            }

        }

        if (ismoving)
        {
            Vector2 targetpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit.transform.gameObject.GetComponent<RectTransform>().position = new Vector3(targetpos.x, targetpos.y, 0f);
        }
    }

   

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Container")
        {
            canblast = true;
            containername = other.gameObject.name;
            other.gameObject.GetComponent<Image>().sprite = containerWithoutLed;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Container")
        {
            canblast = false;
            containername = "";
            other.gameObject.GetComponent<Image>().sprite = ContainerWithled;
        }
    }


    void scaledownObject()
    {
        Debug.Log(collidedDustbin.name);
        collidedDustbin.transform.GetChild(3).GetComponent<Text>().text = "";
        collidedDustbin.transform.GetChild(3).gameObject.SetActive(false);
        collidedDustbin.transform.GetChild(6).gameObject.GetComponent<Text>().text = "";
        collidedDustbin.transform.GetChild(6).gameObject.SetActive(false);
        collidedDustbin.GetComponent<dustbineffect>().enabled = false;
    }
}
