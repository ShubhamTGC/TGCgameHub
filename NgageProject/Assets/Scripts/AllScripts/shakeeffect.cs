using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shakeeffect : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 shakepos;
    public float time;
    private Vector3 initialpos;
    void Start()
    {
        StartCoroutine(shake());
    }

    private void OnEnable()
    {
        StartCoroutine(shake());
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator shake()
    {
        initialpos = this.gameObject.GetComponent<RectTransform>().localPosition;
        yield return new WaitForSeconds(0.2f);
        iTween.ShakePosition(this.gameObject, iTween.Hash("x", 0.2f, "time", 1f));
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<RectTransform>().localPosition = initialpos;
    }
}
