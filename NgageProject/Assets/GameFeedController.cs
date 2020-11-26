using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameFeedController : MonoBehaviour
{
    
    // Profile
    public Image _DP;
    public Text _UserName;
    public Text _SchoolName;
    public Text _Grade;
    public Text _Stage;
    public Text _StageName;
    public Text _PhotosUploadedText;



    //Game Feed
    public GameObject _FeedPrefab;
    public ScrollRect _FeedScrollRect;
    public GameObject _ScrollContent;
    public GameObject _ImagePrefab;
    public GameObject _GameFeedGameObject;
    public GameObject _GameFeedController;
    public GameObject _GameFeedButton;
    public InputField _NoOfFields;
    public GameObject _NoOfFieldObject;
    public Transform Feedparent;

    //private int _ImageCount=10;

    // Start is called before the first frame update
    void Start()
    {
        _UserName.text = "User1";
        _SchoolName.text = "Tagore International";
        _Grade.text = "GRADE VA";
        _Stage.text = "Stage1";
        _StageName.text = "Waste Generation";
        _PhotosUploadedText.text = "Photos Uploaded";
        _FeedScrollRect.verticalNormalizedPosition = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateFeed(int UserIndex)
    {
        for(int a = 0; a < UserIndex; a++)
        {
            GameObject Feed = Instantiate(_FeedPrefab, _ScrollContent.transform, false);
            Feed.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<Text>().text = "User " + a;
            Feed.transform.GetChild(0).gameObject.transform.GetChild(2).gameObject.GetComponent<Text>().text = "GRADE V ";
            Feed.transform.GetChild(0).gameObject.transform.GetChild(3).gameObject.GetComponent<Text>().text = "ACTION PLAN";
            int Rand = UnityEngine.Random.Range(3, 10);
            if (Rand >= 1)
            {

                for (int i = 0; i < Rand; i++)
                {
                    GameObject gb = Instantiate(_ImagePrefab);
                    gb.transform.SetParent(Feed.transform.GetChild(0).gameObject.transform.GetChild(4).
                        transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform, false);
                }

            }
        }
       
    }

    public void GameFeedButton()
    {
        _GameFeedGameObject.SetActive(true);
        _GameFeedController.SetActive(true);
        _GameFeedButton.SetActive(false);
        _NoOfFieldObject.SetActive(false);
        int Count = int.Parse(_NoOfFields.text);
        GenerateFeed(Count);
    }

    //public void EnableGameFeedButton()
    //{
    //    for (int i = 1; i < 5; i++)
    //    {
    //        GenerateFeed(i);
    //    }
    //    _GameFeedButton.GetComponent<Button>().interactable = false;
        
    //}
}
