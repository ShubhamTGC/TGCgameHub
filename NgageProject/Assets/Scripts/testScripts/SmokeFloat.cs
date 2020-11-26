using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeFloat : MonoBehaviour
{
    // Start is called before the first frame update
    public float time;
    public float  targetpos;
    void Start()
    {
        StartCoroutine(cloudanim());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator cloudanim()
    {
        yield return new WaitForSeconds(0.1f);
        iTween.MoveTo(this.gameObject, iTween.Hash("x", targetpos, "easeType", iTween.EaseType.linear, "LoopType", iTween.LoopType.loop, "islocal", true, "time", time));

    }
}
