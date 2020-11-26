using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2ShowCards : MonoBehaviour
{
    public Vector3 TargetPos;
    [SerializeField]
    private float commonTime;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(showEffect());
    }

   IEnumerator showEffect()
    {
        iTween.ScaleTo(this.gameObject, Vector3.one, commonTime);
        iTween.MoveTo(this.gameObject, iTween.Hash("position", TargetPos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", commonTime));
        yield return new WaitForSeconds(commonTime+0.2f);
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
       
    }
}
