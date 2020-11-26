using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtons : MonoBehaviour
{
   private Vector3 TargetPos;
    public GameObject TargetObj;
    private void OnEnable()
    {
        TargetPos = TargetObj.GetComponent<RectTransform>().localPosition;
        StartCoroutine(SetButton());
    }

    IEnumerator SetButton()
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("position", TargetPos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.2f));
        yield return new WaitForSeconds(0.1f);
    }
}
