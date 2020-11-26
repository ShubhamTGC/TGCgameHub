using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageMovingEffect : MonoBehaviour
{
    [SerializeField]private float time = 20f;
    [SerializeField] private float destinationpos = -1820;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(cloudanim());
    }

    // Update is called once per frame
    void Update()
    {
       // transform.position = transform.position + Vector3(Mathf.PingPong(Time.time, 8), 0, 0);
    }

    IEnumerator cloudanim()
    {

        yield return new WaitForSeconds(0.1f);
        iTween.MoveTo(this.gameObject, iTween.Hash("x", destinationpos, "easeType", iTween.EaseType.linear, "LoopType", iTween.LoopType.loop, "islocal", true, "time", time));

    }
}
