using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isfirst = true;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (isfirst)
        {
            isfirst = false;
            this.gameObject.GetComponent<Animator>().SetBool("zoom", true);
        }
    }

    private void OnMouseExit()
    {
        if (!isfirst)
        {
            isfirst = true;
            this.gameObject.GetComponent<Animator>().SetBool("zoom", false);
        }
    }

}
