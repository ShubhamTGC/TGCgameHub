using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstpageController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> allbuttons;
    public List<GameObject> movingobj;
    private List<Vector2> movingobjpos;
    public Sprite volumeon, volumeoff,musicon,musicoff;
    public Button volume_btn, music_btn;
    private int volumecount =1,musicCount=1;
    public StartpageController startpage;
    void Start()
    {
       StartCoroutine(Initialtask());
    }

    private void OnEnable()
    {
        StartCoroutine(Initialtask());
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Initialtask()
    {
        movingobjpos = new List<Vector2>(new Vector2[movingobj.Count]);
        for(int a = 0; a < movingobj.Count; a++)
        {
            movingobjpos[a] = movingobj[a].GetComponent<RectTransform>().localPosition;
        }
        yield return new WaitForSeconds(1.5f);
        for(int i =0;i < allbuttons.Count; i++)
        {
            allbuttons[i].GetComponent<Animator>().enabled = true;
        }

    }
    public void volume()
    {
        if(volumecount %2 == 0)
        {
            volume_btn.gameObject.GetComponent<Image>().sprite = volumeon;
        }
        else
        {
            volume_btn.gameObject.GetComponent<Image>().sprite = volumeoff;
        }
        volumecount += 1;
    }
    public void music()
    {
        if (musicCount % 2 == 0)
        {
            music_btn.gameObject.GetComponent<Image>().sprite = musicon;
        }
        else
        {
            music_btn.gameObject.GetComponent<Image>().sprite = musicoff;
        }
        musicCount += 1;
    }
    public void exit()
    {
        
    }

    public void play()
    {
        for(int a = 0; a < movingobj.Count; a++)
        {
            movingobj[a].GetComponent<RectTransform>().localPosition = movingobjpos[a];
        }
        this.gameObject.SetActive(false);
        startpage.Initialtask();

    }

  
}
