using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHistoryController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
        Destroy(this.gameObject);
    }
}
