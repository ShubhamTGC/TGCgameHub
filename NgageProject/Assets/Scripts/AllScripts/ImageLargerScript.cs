using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageLargerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject imagePanel;
    public Transform canvas;
    public Image ActualImage;
    private bool Pointed,HelpingBool;
    private RaycastHit2D hit;
    private Vector2 mousepos;
    private GameObject gb;

    void Start()
    {
        HelpingBool = true;
    }

   
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenpt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos = new Vector2(screenpt.x, screenpt.y);
            hit = Physics2D.Raycast(mousepos, Vector2.zero);
            if (hit != null && hit.collider != null && hit.collider.gameObject.name == this.gameObject.name && HelpingBool)
            {
                HelpingBool = false;
                StartCoroutine(LargetView());
            }
              
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (hit != null && hit.collider != null && hit.collider.gameObject.name == this.gameObject.name && !HelpingBool)
            {
                HelpingBool = true;
                StartCoroutine(SmallView());
            }
              
        }
        

        //if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        //{
        //    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //    {
        //        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)), Vector2.zero);
        //        if (hit != null && hit.collider != null && hit.collider.transform.gameObject.name == this.gameObject.name && HelpingBool)
        //        {
        //            HelpingBool = false;
        //            StartCoroutine(LargetView());
        //        }

        //    }
        //    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        //    {
        //        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)), Vector2.zero);
        //        if (hit != null && hit.collider != null && hit.collider.transform.gameObject.name == this.gameObject.name)
        //        {
        //            HelpingBool = true;
        //            StartCoroutine(SmallView());
        //        }
        //    }
        //}
     


       
    }


    IEnumerator LargetView()
    {
        gb = Instantiate(imagePanel, canvas, false);
        gb.SetActive(true);
        gb.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ActualImage.sprite;
        iTween.ScaleTo(gb, Vector3.one, 0.5f);
        yield return new WaitForSeconds(0.3f);
    }

    IEnumerator SmallView()
    {
        Destroy(gb,0.05f);
        yield return new WaitForSeconds(0.05f);
        
    }

  


}
