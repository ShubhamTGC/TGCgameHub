using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject boyplayer,boyplayer2,girlplayer,girlplayer2;
    private Animator playeranimator;
    public Vector3 playerpos,kitchenpos;
    public float bedtime,kitchentime;
    private GameObject player, player2;
    private Sprite playersprite;
    public Sprite sideboy, sidegirl;
    public GameObject kitchenscene,bedroom,skipbutton;
    private bool ismove, cityskipped = false, forestskipped = false;
    private bool housecheck = false, industrycheck =false,  hospitalcheck = false, forestcheck =false,  schoolcheck = false;
    public int clickcounter = 0;// industrycounter = 0,hospitalcounter =0,forestcounter =0,schoolcounter =0;
    public GameObject dusbin;
    [Header("in the kitchen msg")]
    public GameObject cloudmsg;
    public GameObject arrowindicator,nextbutton;
    public List<GameObject> wastematerials;
    public List<GameObject> industrymaterial;
    public List<GameObject> hospitalmaterial;
    public List<GameObject> forestmaterial;
    public List<GameObject> schoolmaterial;


    [Header("out side of the home")]
    public GameObject Outsidehome;
    public GameObject playeroutside,bus,fornt_part;
    public Vector3 busstoppos, busexitpos,cityentrypos,cityexitpos,industrystoppos,industryexitpos;
    public Vector3 hospitalentrypos, hospitalstoppos, hospitalexitpos, forest_enter_pos, forest_exitpos;
    public Vector3 jungle_entrypos, junglestop_pos, jungle_exitpos;

    [Header("=======scene changing obejcts=========")]
    public Sprite city_back;
    public Sprite fornt_city,kitchen_sprite,house_sprite,industry_sprite, hospital_sprite, forest_sprite,jungle_sprite,
        jungle_fornt;

    [Header("----------industrial area------------")]
    public GameObject industrialobjects;
    public GameObject hospitalobjects;
    public GameObject industry_nextbtn,windowview,forestobjects;
    public Button AllinOneskip;
    void Start()
    {
        // StartCoroutine(scenechanges(city_back));
       // playeranimator = player.GetComponent<Animator>();
        int playerid = PlayerPrefs.GetInt("characterType");
        if (playerid == 1)
        {
            player = boyplayer;
            player2 = boyplayer2;
            playersprite = sideboy;
            playeroutside.GetComponent<Image>().sprite = sideboy;
            StartCoroutine(playeranim());
        }
        else
        {
            player = girlplayer;
            player2 = girlplayer2;
            playersprite = sidegirl;
            playeroutside.GetComponent<Image>().sprite = sidegirl;
            StartCoroutine(playeranim());
        }


    }

   
    void Update()
    {
        //-------------house material------------------
      if(clickcounter == wastematerials.Count && housecheck)
        {
            //ischeck = false;
            nextbutton.SetActive(true);
        }
        else
        {
            nextbutton.SetActive(false);
        }

      //-----------industry counter----------------------
        if (clickcounter == industrymaterial.Count && industrycheck)
        {
            bus.SetActive(true);
            iTween.MoveTo(windowview, bus.transform.localPosition, 2f);
            iTween.ScaleTo(windowview, Vector3.zero, 2f);
            AllinOneskip.gameObject.SetActive(true);
        }
        else
        {
           // AllinOneskip.gameObject.SetActive(false);
        }

        //------------hospital counter------------
        if (clickcounter == hospitalmaterial.Count && hospitalcheck)
        {

            bus.SetActive(true);
            iTween.MoveTo(windowview, bus.transform.localPosition, 2f);
            iTween.ScaleTo(windowview, Vector3.zero, 2f);
            AllinOneskip.gameObject.SetActive(true);
        }
        else
        {
            //nextbutton.SetActive(false);
        }

        if (clickcounter == forestmaterial.Count && forestcheck)
        {

            //nextbutton.SetActive(true);
        }
        else
        {
            //nextbutton.SetActive(false);
        }



    }

    IEnumerator playeranim()
    {
        
        player.SetActive(true);
        yield return new WaitForSeconds(1f);
        player.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length-0.5f);
        skipbutton.SetActive(true);
        player.SetActive(false);
        player2.SetActive(true);
        iTween.MoveTo(player2, iTween.Hash("position", playerpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", bedtime));
        yield return new WaitForSeconds(bedtime);
        //player2.GetComponent<Animator>().enabled = false;
        if (ismove)
        {

        }
        else
        {
            StartCoroutine(scenechanges(bedroom,kitchen_sprite));
            skipbutton.SetActive(false);
            StartCoroutine(kitchenactivity());

        }
        


    }

    public void kitchentask()
    {
        ismove = true;
        iTween.Stop(player2);
        StopCoroutine("playeranim");
        player2.transform.localPosition = playerpos;
        player2.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        skipbutton.SetActive(false);
        StartCoroutine(scenechanges(bedroom, kitchen_sprite));
        StartCoroutine(kitchenactivity());
    }

    IEnumerator kitchenactivity()
    {
        housecheck = true;
        player2.transform.localPosition = playerpos;
        player2.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        yield return new WaitForSeconds(1);
        kitchenscene.SetActive(true);
        player2.GetComponent<Animator>().enabled = true;
        iTween.MoveTo(player2, iTween.Hash("position", kitchenpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", kitchentime));
        yield return new WaitForSeconds(kitchentime);
        //player2.GetComponent<Animator>().SetBool("ideal", true);
        player2.GetComponent<Animator>().enabled = false;
        player2.GetComponent<SpriteRenderer>().sprite = playersprite;
        yield return new WaitForSeconds(0.5f);
        Vector3 cloudpos = player2.transform.localPosition;
        cloudmsg.transform.localPosition = new Vector3(cloudpos.x + 160, cloudpos.y + 450f, cloudpos.z);
        cloudmsg.transform.GetChild(0).gameObject.GetComponent<Text>().text = "This is some waste from my own house.";
        cloudmsg.SetActive(true);
        yield return new WaitForSeconds(5f);
        iTween.ScaleTo(cloudmsg, Vector3.zero, 1f);
        arrowindicator.SetActive(true);
        iTween.MoveTo(arrowindicator, iTween.Hash("y", 16f, "easeType", iTween.EaseType.linear, "LoopType", iTween.LoopType.loop, "islocal", true, "time", 2));
        yield return new WaitForSeconds(3f);
        arrowindicator.SetActive(false);
        dusbin.SetActive(true);
        for (int a= 0; a < wastematerials.Count; a++)
        {
            wastematerials[a].gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        }
    }

     public void Gatescene()
    {
        dusbin.GetComponent<RectTransform>().localPosition = new Vector2(-1400f, -368f);
        dusbin.SetActive(false);
        kitchenscene.SetActive(false);
        player2.SetActive(false);
        StartCoroutine(scenechanges(bedroom, house_sprite));
        clickcounter = 0;
        housecheck = false;
        StartCoroutine(busanim());
    }

    //-----------method for outside the home-------------------------
    IEnumerator busanim()
    {
        yield return new WaitForSeconds(1f);
        playeroutside.SetActive(true);
        bus.gameObject.SetActive(true);
        bus.GetComponent<RectTransform>().sizeDelta = new Vector2(800 , 500);
        iTween.MoveTo(bus, iTween.Hash("position", busstoppos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 4f));
        yield return new WaitForSeconds(5f);
        playeroutside.SetActive(false);
        AllinOneskip.gameObject.SetActive(true);
        AllinOneskip.onClick.AddListener(delegate { skipcitypart(); });
        iTween.MoveTo(bus, iTween.Hash("position", busexitpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 4f));
        yield return new WaitForSeconds(4f);
        if (!cityskipped)
        {
            StartCoroutine(scenechanges(bedroom, city_back));
            StartCoroutine(citypart());
        }
       
    }

    void skipcitypart()
    {
        cityskipped = true;
        iTween.Stop(bus);
        bus.SetActive(false);
        //StartCoroutine(scenechanges(bedroom, industry_sprite));
        StartCoroutine(Towardsindustry());
    }

    IEnumerator scenechanges(GameObject parentobejct ,Sprite new_sprite)
    {
        yield return new WaitForSeconds(0.1f);
        float bgvalue = parentobejct.GetComponent<Image>().color.a;
        while (bgvalue > 0)
        {
            bgvalue -= 0.1f;
            yield return new WaitForSeconds(0.05f);
            parentobejct.GetComponent<Image>().color = new Color(1, 1, 1, bgvalue);
        }
        parentobejct.GetComponent<Image>().sprite = new_sprite;
        bgvalue = parentobejct.GetComponent<Image>().color.a;
       
        while (bgvalue < 1)
        {
            bgvalue += 0.1f;
            yield return new WaitForSeconds(0.05f);
            parentobejct.GetComponent<Image>().color = new Color(1, 1, 1, bgvalue);
        }
        
    }

    IEnumerator citypart()
    {
        AllinOneskip.onClick.RemoveAllListeners();
        yield return new WaitForSeconds(1f);
        fornt_part.SetActive(true);
        fornt_part.GetComponent<Image>().sprite = fornt_city;
        bus.transform.localPosition = cityentrypos;
        bus.GetComponent<RectTransform>().sizeDelta = new Vector2(600, 400);
        iTween.MoveTo(bus, iTween.Hash("position", cityexitpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 4f));
        yield return new WaitForSeconds(4f);
        fornt_part.SetActive(false);
        StartCoroutine(Towardsindustry());
    }
    IEnumerator Towardsindustry()
    {
        AllinOneskip.onClick.RemoveAllListeners();
        AllinOneskip.gameObject.SetActive(false);
        StartCoroutine(scenechanges(bedroom, industry_sprite));
        yield return new WaitForSeconds(1f);
        bus.transform.localPosition = new Vector2(-1400f,-320f);
        bus.SetActive(true);
        industrialobjects.SetActive(true);
        industrycheck = true;
        iTween.MoveTo(bus, iTween.Hash("position", industrystoppos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 2f));
        yield return new WaitForSeconds(4f);
        windowview.SetActive(true);
        windowview.transform.localPosition = bus.transform.localPosition;
        windowview.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = playersprite;
        bus.SetActive(false);
        iTween.MoveTo(windowview, Vector3.zero, 2f);
        iTween.ScaleTo(windowview, Vector3.one, 2f);
        yield return new WaitForSeconds(1f);
        bus.SetActive(false);
        windowview.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(4f);
        windowview.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        //windowview.SetActive(false);
       // dusbin.GetComponent<ScreenComeIN>().targetpos = new Vector3(-95f, -158f, 0f);
        dusbin.transform.localScale = new Vector3(0.6f, 0.6f, 0f);
        dusbin.SetActive(true);
        AllinOneskip.onClick.AddListener(delegate { IndusryTOhospital(); });

    }

    void IndusryTOhospital()
    {
        dusbin.GetComponent<RectTransform>().localPosition = new Vector3(-1400f, -368f, 0f);
        dusbin.transform.localScale = Vector3.one;
        dusbin.SetActive(false);
        AllinOneskip.gameObject.SetActive(false);
        clickcounter = 0;
        StartCoroutine(towardshospital());
       
    }

    IEnumerator towardshospital()
    {
        AllinOneskip.onClick.RemoveAllListeners();
        yield return new WaitForSeconds(0.1f);
        iTween.MoveTo(bus, iTween.Hash("position", industryexitpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 2f));
        yield return new WaitForSeconds(2f);
        industrialobjects.SetActive(false);
        StartCoroutine(scenechanges(bedroom, hospital_sprite));
        yield return new WaitForSeconds(1f);
        hospitalobjects.SetActive(true);
        bus.transform.localPosition = hospitalentrypos;
        iTween.MoveTo(bus, iTween.Hash("position", hospitalstoppos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 2f));
        yield return new WaitForSeconds(4f);
        //windowview.transform.localPosition = bus.transform.localPosition;
        windowview.SetActive(true);
        windowview.transform.localPosition = bus.transform.localPosition;
        windowview.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = playersprite;
        bus.SetActive(false);
        iTween.MoveTo(windowview, Vector3.zero, 2f);
        iTween.ScaleTo(windowview, Vector3.one, 2f);
        yield return new WaitForSeconds(1f);
        bus.SetActive(false);
        windowview.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(4f);
        windowview.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        hospitalcheck = true;
       // dusbin.GetComponent<ScreenComeIN>().targetpos = new Vector3(227f, -286f, 0f);
        dusbin.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
        dusbin.SetActive(true);
        AllinOneskip.onClick.AddListener(delegate { hospitalexitmethod(); });
        for (int i =0;i < hospitalmaterial.Count; i++)
        {
            hospitalmaterial[i].gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        }
    }

    void hospitalexitmethod()
    {
        dusbin.GetComponent<RectTransform>().localPosition = new Vector3(-1400f, -368f, 0f);
        dusbin.transform.localScale = Vector3.one;
        dusbin.SetActive(false);
        AllinOneskip.onClick.RemoveAllListeners();
        AllinOneskip.gameObject.SetActive(false);
        clickcounter = 0;
        hospitalobjects.SetActive(false);
        StartCoroutine(towardsforest());
       
        
    }

    IEnumerator towardsforest()
    {
        yield return new WaitForSeconds(1f);
        iTween.MoveTo(bus, iTween.Hash("position", hospitalexitpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 2f));
        yield return new WaitForSeconds(2);
        StartCoroutine(scenechanges(bedroom, forest_sprite));
        yield return new WaitForSeconds(1);
        bus.transform.localPosition = forest_enter_pos;
        AllinOneskip.gameObject.SetActive(true);
        AllinOneskip.onClick.AddListener(delegate { forestskip(); });
        iTween.MoveTo(bus, iTween.Hash("position", forest_exitpos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 5f));
        yield return new WaitForSeconds(5f);
        if (!forestskipped)
        {
            StartCoroutine(towardsjungle());
        }
    }

    void forestskip()
    {
        iTween.Stop(bus);
        bus.SetActive(false);
        forestskipped = true;
        StartCoroutine(towardsjungle());
    }

    IEnumerator towardsjungle()
    {
        clickcounter = 0;
        AllinOneskip.onClick.RemoveAllListeners();
        AllinOneskip.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(scenechanges(bedroom, jungle_sprite));
        fornt_part.GetComponent<Image>().sprite = jungle_fornt;
        yield return new WaitForSeconds(0.8f);
        forestobjects.SetActive(true);
        fornt_part.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        bus.SetActive(true);
        bus.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(800f, 500f);
        bus.transform.localPosition = jungle_entrypos;
        iTween.MoveTo(bus, iTween.Hash("position", junglestop_pos, "isLocal", true, "easeType", iTween.EaseType.linear, "time", 2f));
        yield return new WaitForSeconds(4);
        windowview.SetActive(true);
        windowview.transform.localPosition = bus.transform.localPosition;
        windowview.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = playersprite;
        bus.SetActive(false);
        fornt_part.SetActive(false);
        iTween.MoveTo(windowview, Vector3.zero, 2f);
        iTween.ScaleTo(windowview, Vector3.one, 2f);
        yield return new WaitForSeconds(1f);
        windowview.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(4f);
        windowview.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
        forestcheck = true;
       // dusbin.GetComponent<ScreenComeIN>().targetpos = new Vector3(277f, -238f, 0f);
        dusbin.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
        dusbin.SetActive(true);
        for (int a =0;a < forestmaterial.Count; a++)
        {
            forestmaterial[a].gameObject.GetComponent<PolygonCollider2D>().enabled = true;
        }
    }



}
