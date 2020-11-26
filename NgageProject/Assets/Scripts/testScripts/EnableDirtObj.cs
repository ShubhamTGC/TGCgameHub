using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDirtObj : MonoBehaviour
{
    // Start is called before the first frame update
    public float FadeValue;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(EnabledObject());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EnabledObject()
    {
        float value = this.GetComponent<SpriteRenderer>().color.a;
        while(FadeValue > value)
        {
            value += 0.1f;
            this.GetComponent<SpriteRenderer>().color = new Color(this.GetComponent<SpriteRenderer>().color.r, this.GetComponent<SpriteRenderer>().color.g,
                this.GetComponent<SpriteRenderer>().color.b, value);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
