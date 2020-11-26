using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AssessmentDashBoard : MonoBehaviour
{
    public Sprite ClickedSprite, NonClickedSprite;
    public List<GameObject> tabs, dataHolder,DataTab;
    public AssessmentGameHandler Assessmentgame;
    public Text StageScore,OverAllscore;
    public Text UserName, UserGrade;
    public List<Sprite> Boyface, GirlFace;
    public Image Boyimage, GirlImage;
    public Image FinalPageFace;
    void Start()
    {
        
    }
    private void OnEnable()
    {
        if (PlayerPrefs.GetString("gender").ToLower() == "m")
        {
            Boyimage.gameObject.SetActive(true);
            GirlImage.gameObject.SetActive(false);
            PlayerSetup(Boyface, Boyimage);
        }
        else
        {
            Boyimage.gameObject.SetActive(false);
            GirlImage.gameObject.SetActive(true);
            PlayerSetup(GirlFace, GirlImage);
        }

        for (int a = 0; a < tabs.Count; a++)
        {
            if(a == 0)
            {
                tabs[a].GetComponent<Image>().sprite = ClickedSprite;
                dataHolder[a].GetComponent<Image>().enabled = true;
            }
            else
            {
                tabs[a].GetComponent<Image>().sprite = NonClickedSprite;
                dataHolder[a].GetComponent<Image>().enabled = false;
            }
        }

        OverAllscore.text = (Assessmentgame.Stage1UserScore + Assessmentgame.Stage2UserScore + Assessmentgame.Stage3UserScore).ToString();
        StageScore.text = "Stage l Score :" + Assessmentgame.Stage1UserScore.ToString();

    }

    void PlayerSetup(List<Sprite> Face, Image profilepic)
    {
        UserName.text = PlayerPrefs.GetString("username");
        UserGrade.text = PlayerPrefs.GetString("User_grade");
        for (int a = 0; a < Face.Count; a++)
        {
            if (a == PlayerPrefs.GetInt("characterType"))
            {
                profilepic.sprite = Face[a];
                FinalPageFace.sprite = Face[a];
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void SelectedTab(GameObject SelectedButton)
    {
        tabs.ForEach(x =>
        {
            x.GetComponent<Image>().sprite = x.name == SelectedButton.name ? ClickedSprite : NonClickedSprite;
            
         
        });
        DataTab.ForEach(y =>
        {
            y.SetActive(y.name == SelectedButton.name ? true : false);
        });

        dataHolder.ForEach(y =>
        {
            y.GetComponent<Image>().enabled = y.name == SelectedButton.name;
        });

        if(SelectedButton.name == "Stage1")
        {
            StageScore.text = "Stage l Score :" + Assessmentgame.Stage1UserScore.ToString();
        }
        else if(SelectedButton.name == "Stage2")
        {
            StageScore.text = "Stage ll Score :" + Assessmentgame.Stage2UserScore.ToString();
        }else if(SelectedButton.name == "Stage3")
        {
            StageScore.text = "Stage lll Score :" + Assessmentgame.Stage3UserScore.ToString();
        }
      
       
    }


}
