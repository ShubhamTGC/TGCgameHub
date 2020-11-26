using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenePreview : MonoBehaviour
{
    public GameObject MainBg;
    public Sprite defaultimage, selectedimage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseEnter()
    {
        StartCoroutine(scenechanges(MainBg, selectedimage));
    }

    //public void 
    //{
    //    StartCoroutine(scenechanges(MainBg, selectedimage));
    //}

    public void OnMouseExit()
    {
        StartCoroutine(scenechanges(MainBg, defaultimage));
    }


    public IEnumerator scenechanges(GameObject parentobejct, Sprite new_sprite)
    {
        float bgvalue = parentobejct.GetComponent<Image>().color.a;
        while (bgvalue > 0)
        {
            bgvalue -= 0.1f;
            yield return new WaitForSeconds(0.01f);
            parentobejct.GetComponent<Image>().color = new Color(1, 1, 1, bgvalue);
        }
        parentobejct.GetComponent<Image>().sprite = new_sprite;
        bgvalue = parentobejct.GetComponent<Image>().color.a;
        while (bgvalue < 1)
        {
            bgvalue += 0.1f;
            yield return new WaitForSeconds(0.01f);
            parentobejct.GetComponent<Image>().color = new Color(1, 1, 1, bgvalue);
        }

    }

}
