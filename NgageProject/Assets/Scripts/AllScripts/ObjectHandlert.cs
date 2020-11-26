using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectHandlert : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> objects;
    public List<GameObject> extraelemts;
    private List<Vector2> objectpos;
    void Start()
    {
        Initialtask();
    }

    public void OnEnable()
    {
        Initialtask();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
    

    void Initialtask()
    {
        objectpos = new List<Vector2>(new Vector2[objects.Count]);
        for (int a = 0; a < objects.Count; a++)
        {
            objects[a].gameObject.SetActive(true);
            objectpos[a] = objects[a].gameObject.GetComponent<RectTransform>().localPosition;
        }
    }

    public void OnDisable()
    {
        for (int b = 0; b < objects.Count; b++)
        {
            objects[b].gameObject.GetComponent<RectTransform>().localPosition = objectpos[b];
            objects[b].GetComponent<wasteCollection>().check = false;
        }
        for(int i =0; i < extraelemts.Count; i++)
        {
            if(extraelemts[i].GetComponent<Animator>() == null)
            {
                extraelemts[i].GetComponent<Image>().enabled = true;
                extraelemts[i].transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                extraelemts[i].GetComponent<Animator>().enabled = true;
                extraelemts[i].transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
            if(extraelemts[i].GetComponent<Animator>() && extraelemts[i].GetComponent<Image>())
            {
                extraelemts[i].GetComponent<Image>().enabled = true;
            }
        }
    }
}
