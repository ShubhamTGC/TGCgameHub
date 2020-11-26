using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dustbineffect : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite dustbinsprite,capsprite;
    public Vector3 shakepos;
    public float time;
    private bool isdone = false;
    public Sprite initialbinsprite, initial_cap;
    private Generationlevel startpage;
    void Start()
    {
        startpage = FindObjectOfType<Generationlevel>();
        initialbinsprite = this.gameObject.GetComponent<Image>().sprite;
        initial_cap = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite;
        StartCoroutine(shakeeffect());
    }
     void OnEnable()
    {
        initialbinsprite = this.gameObject.GetComponent<Image>().sprite;
        initial_cap = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite;
        StartCoroutine(shakeeffect());
    }
    // Update is called once per frame
    void Update()
    { 

    }

    IEnumerator shakeeffect()
    {
        yield return new WaitForSeconds(0.5f);
        this.gameObject.GetComponent<Image>().sprite = dustbinsprite;
        this.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = capsprite;
        iTween.ShakePosition(this.gameObject, iTween.Hash("x", 0.2f, "time", 1f));
        startpage.VibrateDevice();
        yield return new WaitForSeconds(1f);
        this.gameObject.GetComponent<Image>().sprite = initialbinsprite;
        this.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = initial_cap;
       
    }
}
