using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenComeIN : MonoBehaviour
{
    // Start is called before the first frame update
    public float time;
    public float delaytime;
    public GameObject targetobj;
    private Vector3 targetobjpos;
    
    void Start()
    {
        StartCoroutine(objectanim());
    }
    private void OnEnable()
    {
        StartCoroutine(objectanim());
    }

    IEnumerator objectanim()
    {
        targetobjpos = targetobj.GetComponent<RectTransform>().localPosition;
        yield return new WaitForSeconds(delaytime);
        iTween.MoveTo(this.gameObject, iTween.Hash("position",targetobjpos, "easeType", iTween.EaseType.linear, "isLocal", true,
            "time", time));
    }


}
