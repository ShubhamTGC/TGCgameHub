using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHistoryController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject viewpage;
   
    void Start()
    {
        
    }

 

    public void CloseCard()
    {
        StartCoroutine(Closingtask());
        
    }

    IEnumerator Closingtask()
    {
        iTween.ScaleTo(this.gameObject, Vector3.zero, 0.3f);
        yield return new WaitForSeconds(0.4f);
        viewpage.SetActive(true);
        Destroy(this.gameObject);
    }
}
