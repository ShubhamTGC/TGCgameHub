using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCardEffect : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector3 Targetpos;
    [SerializeField]
    private Vector3 Pausepos;
    [SerializeField]
    private float time= 0.4f;
    [SerializeField]
    public float pauseSize = 2f;
    void Start()
    {
        
    }

    void OnEnable()
    {
        
        
    }

    public void ShowEffect()
    {
        pauseSize = 2f;
        StartCoroutine(ShowCard());
    }
     IEnumerator ShowCard()
    {
        Debug.Log("pause scale " + pauseSize);
        iTween.MoveTo(this.gameObject, iTween.Hash("position", Pausepos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", time));
        iTween.ScaleTo(this.gameObject, new Vector3(pauseSize, pauseSize, 0), time);
        yield return new WaitForSeconds(time);
        yield return new WaitForSeconds(3f);
        iTween.MoveTo(this.gameObject, iTween.Hash("position", Targetpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", time));
        iTween.ScaleTo(this.gameObject, Vector3.one, time);
        yield return new WaitForSeconds(time);
    }
}
