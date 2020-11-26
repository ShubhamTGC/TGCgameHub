using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShakeEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(shakeeffect());
    }

    IEnumerator shakeeffect()
    {
        yield return new WaitForSeconds(0.5f);
        //this.gameObject.GetComponent<SpriteRenderer>().sprite = dustbinsprite;
        iTween.ShakePosition(this.gameObject, iTween.Hash("x", 0.2f, "time", 1f));
        //startpage.VibrateDevice();
        yield return new WaitForSeconds(1f);
        //this.gameObject.GetComponent<Image>().sprite = initialbinsprite;
        //this.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = initial_cap;

    }
}
