using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeacherMainPage : MonoBehaviour
{
    public GameObject logoutpage, loadinganim, logoutpanel, logoutmsg;
    static StartpageController HomepageInstance;
    void Start()
    {
        HomepageInstance = StartpageController.Home_instane;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void logoutaction()
    {
        logoutpage.SetActive(true);
    }


    public void YesLogout()
    {
        logoutpage.SetActive(false);
        StartCoroutine(afterlogout());
    }
    public void LogoutCancel()
    {
        logoutpage.SetActive(false);
    }

    IEnumerator afterlogout()
    {
        yield return new WaitForSeconds(0.1f);
        loadinganim.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        loadinganim.SetActive(false);
        logoutpanel.SetActive(true);
        iTween.ScaleTo(logoutmsg, Vector3.one, 0.8f);
        yield return new WaitForSeconds(1.8f);
        iTween.ScaleTo(logoutmsg, Vector3.zero, 0.8f);
        PlayerPrefs.DeleteKey("logged");
        yield return new WaitForSeconds(0.8f);
        Destroy(HomepageInstance);
        SceneManager.LoadScene(0);
    }

    public void ExitApp()
    {
        Application.Quit();
    }

}
