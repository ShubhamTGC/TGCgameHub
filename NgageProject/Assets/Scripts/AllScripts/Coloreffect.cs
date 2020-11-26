using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coloreffect : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isdone = false;
    void Start()
    {
        
    }

     void OnEnable()
    {
        
    }

     void OnDisable()
    {
        isdone = false;
        this.gameObject.GetComponent<Image>().color = Color.white;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void timertask()
    {
        StartCoroutine(effect());
        isdone = false;
    }

    public IEnumerator effect()
    {
        yield return new WaitForSeconds(0);
        this.gameObject.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<Image>().color = Color.white;
        yield return new WaitForSeconds(1f);
        if (!isdone)
        {
            StartCoroutine(effect());
        }
        
    }
}
