using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bird : MonoBehaviour
{
    public int time;
    //private Vector3 startpos;
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
        iTween.MoveTo(this.gameObject, iTween.Hash("x", 1250f, "easeType", iTween.EaseType.linear, "LoopType", iTween.LoopType.loop, "islocal", true, "time", time));

    }
}
