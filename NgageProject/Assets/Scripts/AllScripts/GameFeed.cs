using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFeed : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject CommentBox;
    private bool ActiveStatus;
    public Sprite DoneRating, NotRating;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void CommentBoxHandler()
    {
        CommentBox.SetActive(CommentBox.activeInHierarchy == true ? false : true);
    }
}
