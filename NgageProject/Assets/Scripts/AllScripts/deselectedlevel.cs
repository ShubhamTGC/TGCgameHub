using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deselectedlevel : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 targetpos;
    void Start()
    {
        StartCoroutine(levelanim());
    }
     void OnEnable()
    {
        StartCoroutine(levelanim());
    }

    IEnumerator levelanim()
    {
        yield return new WaitForSeconds(0.1f);
        this.gameObject.GetComponent<Animator>().enabled = false;
        iTween.MoveTo(this.gameObject, iTween.Hash("position", targetpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 0.6f));
        iTween.ScaleTo(this.gameObject, Vector3.zero, 1f);
    }


}
