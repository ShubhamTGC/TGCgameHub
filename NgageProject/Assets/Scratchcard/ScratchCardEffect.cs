﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using m2ostnextservice.Models;
using UnityEngine.Networking;

public class ScratchCardEffect : MonoBehaviour
{
    public GameObject maskPrefeb;
    [SerializeField]private bool isPressed;
    private Vector3 Mousepos;
    [SerializeField] private float MaskDistance = 5.0f;
    private RaycastHit2D hit;
    private Vector3 mousepos;
    [HideInInspector] public int CardID;
    private bool scratched;
    public GameObject leaderboard, cardPanelpage;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        isPressed = false;
        scratched = false;
    }

    // Update is called once per frame
    void Update()
    {
        var pos = Input.mousePosition;
        pos.z = MaskDistance;
        Mousepos = Camera.main.ScreenToWorldPoint(pos);

        if (isPressed)
        {
            GameObject mask = Instantiate(maskPrefeb, Mousepos, Quaternion.identity);
            mask.transform.parent = this.gameObject.transform;
            Invoke("Revelcard", 3f);

        }
    

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenpt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousepos = new Vector2(screenpt.x, screenpt.y);
            hit = Physics2D.Raycast(mousepos, Vector2.zero);
            if (hit != null && hit.collider != null && hit.collider.tag == "Card")
            {
                isPressed = true;
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            isPressed = false;
        }
    }

    void Revelcard()
    {
        if (!scratched)
        {
            StartCoroutine(PostCardData());
        }
    }

    IEnumerator PostCardData()
    {
        scratched = true;
        Debug.Log("card id " + CardID);
        string HittingUrl = $"{MainUrls.BaseUrl}{MainUrls.PostCardID}?Id={CardID}";
   

        using (UnityWebRequest Request = UnityWebRequest.Get(HittingUrl))
        {
            Request.method = UnityWebRequest.kHttpVerbPOST;
            Request.SetRequestHeader("Content-Type", "application/json");
            Request.SetRequestHeader("Accept", "application/json");
            yield return Request.SendWebRequest();
            if (!Request.isNetworkError && !Request.isHttpError)
            {
                if (Request.responseCode.Equals(200))
                {
                    if (Request.downloadHandler.text != null)
                    {
                        Debug.Log("response" + Request.downloadHandler.text);
                        yield return new WaitForSeconds(0.4f);
                        StartCoroutine(CloseCardpageTask());
                       
                    }
                }
                else
                {
                    Debug.Log("Problem " + Request.downloadHandler.text);
                }
            }
            else
            {
                Debug.Log("Problem " + Request.downloadHandler.text);
            }
        }
    }


    IEnumerator CloseCardpageTask()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.3f);
        leaderboard.SetActive(true);
        cardPanelpage.SetActive(false);
        Destroy(this.gameObject);
    }

}
