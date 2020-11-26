using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DashboardProfileSetup : MonoBehaviour
{
    // Start is called before the first frame update
    public Image BoyPRFFace,BoyPRFBBody;
    public Image GirlPRFFace, GirlPRFBBody;
    public List<Sprite> BoyFace,BoyBody, GirlFace,GirlBody;
    public GameObject BoyProfile, GirlProfile;
    public Text Username;
    public GameObject priorityDash, alignDash, leftButton, rightbutton;
    public Text Dashboardname;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        Username.text = PlayerPrefs.GetString("username");
        if (PlayerPrefs.GetString("gender").Equals("m",System.StringComparison.OrdinalIgnoreCase))
        {
            BoyProfile.SetActive(true);
            GirlProfile.SetActive(false);
            PlayerSetup(BoyFace,BoyBody, BoyPRFFace, BoyPRFBBody);
        }
        else
        {
            BoyProfile.SetActive(false);
            GirlProfile.SetActive(true);
            PlayerSetup(GirlFace,GirlBody, GirlPRFFace, GirlPRFBBody);
        }
        if (SceneManager.GetActiveScene().name.Equals("stage3games", System.StringComparison.OrdinalIgnoreCase))
        {
            DashboradSetup();
        }
     
    }


    void DashboradSetup()
    {
        alignDash.SetActive(false);
        leftButton.SetActive(false);
        priorityDash.SetActive(true);
        rightbutton.SetActive(true);
        Dashboardname.text = "Prioritized Table";

    }
    void PlayerSetup(List<Sprite> Face,List<Sprite> Body,Image PlayerFace,Image PlayerBody)
    {
        int CharacterNum = PlayerPrefs.GetInt("characterType");
        int BodyNum = PlayerPrefs.GetInt("PlayerBody");
        for (int a = 0; a < Face.Count; a++)
        {
            if (a == CharacterNum)
            {
                PlayerFace.sprite = Face[a];
            }
        }

        for (int b = 0; b < Body.Count; b++)
        {
            if (b == BodyNum)
            {
                PlayerBody.sprite = Body[b];
            }
        }
    }

 
}
