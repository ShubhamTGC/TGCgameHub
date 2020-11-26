using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightOnEffect : MonoBehaviour
{
    [SerializeField] private float delaytime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(sceneappear());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator sceneappear()
    {
        yield return new WaitForSeconds(delaytime);
        float shadevalue = this.gameObject.GetComponent<Image>().color.a;
        while (shadevalue < 1)
        {
            this.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, shadevalue);
            shadevalue += 0.1f;
            yield return new WaitForSeconds(0.05f);
        }
       
    }
}
