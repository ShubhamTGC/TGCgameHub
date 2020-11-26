using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LightEffect : MonoBehaviour
{
    [HideInInspector] public bool iscorrect;
    [SerializeField] private Color CorrectColor, WrongColor,Initialcolor;
    [SerializeField] private float time;
    [SerializeField] private int counter;


    void Start()
    {
        
    }

    private void OnEnable()
    {
       
        StartCoroutine(Coloreffect(iscorrect ? CorrectColor:WrongColor));
    }

    IEnumerator Coloreffect(Color color)
    {
        for (int a = 0; a < counter; a++)
        {
            this.gameObject.GetComponent<Image>().color = color;
            yield return new WaitForSeconds(time);
            this.gameObject.GetComponent<Image>().color = Initialcolor;
            yield return new WaitForSeconds(time);
        }
    }
}
