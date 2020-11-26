using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomLoader : MonoBehaviour
{
    public Image LoadingBar;
    [HideInInspector]
    public bool Laodingstart;
    [SerializeField]
    private float totaltime;
    [SerializeField]
    private float LimitValue;
    private float currentTime;
    public bool Isdone;
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        Isdone = false;
        Laodingstart = true;
        StartCoroutine(CustomLoaderbar());
    }

    void OnDisable()
    {
        Laodingstart = false;
        currentTime = 0f;
        LoadingBar.fillAmount = 0f;

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CustomLoaderbar()
    {
        if (currentTime < LimitValue)
        {
            yield return new WaitForSeconds(0.5f);
            if (Isdone)
            {
                LoadingBar.fillAmount = 1f;
                yield return new WaitForSeconds(0.5f);
                Laodingstart = false;
                this.gameObject.SetActive(false);

            }
            else
            {
                currentTime += 2f;
                LoadingBar.fillAmount = currentTime / totaltime;
            }

        }
        else
        {
            while (!Isdone)
            {
                yield return new WaitForSeconds(0.1f);

            }
            yield return new WaitForSeconds(0.5f);
            currentTime += 2f;
            LoadingBar.fillAmount = currentTime / totaltime;
            if (LoadingBar.fillAmount == 1)
            {
                Laodingstart = false;
                this.gameObject.SetActive(false);
            }

        }

        if (Laodingstart)
        {
            StartCoroutine(CustomLoaderbar());
        }
    }

}

