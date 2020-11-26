using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZonepopUp : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject startpage;
    public GameObject ZoneChild;
    public Color HoverEffect, RelasedEffect;
    public Vector3 targetposition;
    public float targetScale;
    private Vector2 initialpos;
    [SerializeField]
    private float time = 0.3f;
    public string ZoneMSg;
    public Text ZoneInfo;
    void Start()
    {
        initialpos = startpage.GetComponent<RectTransform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseEnter()
    {
        ZoneInfo.gameObject.SetActive(true);
        ZoneInfo.text = ZoneMSg;
        //StartCoroutine(ZoneEffect());
        startpage.GetComponent<Image>().color = HoverEffect;
        ZoneChild.SetActive(true);
    }

    public void OnMouseExit()
    {
        ZoneInfo.text = "";
        // StartCoroutine(CancelEffect());
        startpage.GetComponent<Image>().color = RelasedEffect;
        ZoneChild.SetActive(false);
    }

    IEnumerator ZoneEffect()
    {
        iTween.ScaleTo(startpage, new Vector3(targetScale, targetScale, targetScale), time);
        iTween.MoveTo(startpage, iTween.Hash("position", targetposition, "easeType", iTween.EaseType.linear, "time", time, "isLocal", true));
        yield return new WaitForSeconds(time);
    }
    IEnumerator CancelEffect()
    {
        iTween.MoveTo(startpage, iTween.Hash("position", Vector3.zero, "easeType", iTween.EaseType.linear, "time", 0.2f, "isLocal", true));
        iTween.ScaleTo(startpage, Vector3.one, time);
        yield return new WaitForSeconds(time);
    }
}
