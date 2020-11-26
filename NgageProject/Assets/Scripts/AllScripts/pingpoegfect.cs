using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pingpoegfect : MonoBehaviour
{
    // Start is called before the first frame update
    public float time,delaytime;

    void Start()
    {
        StartCoroutine(pingpop());
    }


    private void OnEnable()
    {
        StartCoroutine(pingpop());
    }
  

    IEnumerator pingpop()
    {
        yield return new WaitForSeconds(delaytime);
        iTween.ScaleTo(this.gameObject, iTween.Hash("scale", Vector3.one, "easeType", iTween.EaseType.linear, "looptype", iTween.LoopType.pingPong,
            "time", time));
    }
}
