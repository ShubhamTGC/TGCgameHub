using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InternetConnection : MonoBehaviour
{
    public GameObject internetpanel;
    void Start()
    {
        StartCoroutine(checkInternet());
    }

    IEnumerator checkInternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            internetpanel.SetActive(true);
        }
        else
        {
            internetpanel.SetActive(false);
            
        }
        yield return new WaitForSeconds(5f);
        StartCoroutine(checkInternet());
    }
}
