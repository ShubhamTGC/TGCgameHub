using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargets : MonoBehaviour
{
    [SerializeField] private Vector3 Targetpos;
    [SerializeField] private float time;
    private Vector3 initialpos = Vector3.zero;
    void Start()
    {
        
    }

    // Update is called once per frame
     void OnEnable()
    {
        StartCoroutine(MovetoTarget());
    }


    IEnumerator MovetoTarget()
    {
        iTween.ScaleTo(this.gameObject, Vector3.one, time);
        iTween.MoveTo(this.gameObject, iTween.Hash("position", Targetpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", time));
        yield return new WaitForSeconds(time + 0.2f);

    }


    public void Resetpos()
    {
        StartCoroutine(resettask());
      
    }
    IEnumerator resettask()
    {
        iTween.ScaleTo(this.gameObject, Vector3.zero, time);
        iTween.MoveTo(this.gameObject, iTween.Hash("position", initialpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", time));
        yield return new WaitForSeconds(time + 0.2f);
    }


}
