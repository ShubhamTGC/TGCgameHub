using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage1Menus : MonoBehaviour
{
    public List<GameObject> Zones;
    public List<GameObject> ZoneButtons;
    public Sprite Pressed, Relased;
    public string MainUrl, GetlevelWisedataApi;
    public int Gamelevel;
    [HideInInspector]
    public int GameAttemptNumber;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        Initialsetup();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void Initialsetup()
    {
        StartCoroutine(GetGameAttemptNoTask());
        Zones[0].SetActive(true);
        ZoneButtons[0].GetComponent<Image>().sprite = Pressed;
        for(int a=1;a< ZoneButtons.Count; a++)
        {
            ZoneButtons[a].GetComponent<Image>().sprite = Relased;
            Zones[a].SetActive(false);
        }

    }

    public void Zoneselection(GameObject currentZone)
    {
        bool enable;
        Zones.ForEach(z =>
        {
            enable = z.name == currentZone.name;
            z.gameObject.SetActive(enable);
        });

        ZoneButtons.ForEach(b =>
        {
            b.GetComponent<Image>().sprite = b.name == currentZone.name ? Pressed : Relased;
        });


    }

    IEnumerator GetGameAttemptNoTask()
    {
        string HittingUrl = $"{MainUrl}{GetlevelWisedataApi}?UID={PlayerPrefs.GetInt("UID")}&OID={PlayerPrefs.GetInt("OID")}&id_level={Gamelevel}&game_type={1}";
        WWW Attempt_res = new WWW(HittingUrl);
        yield return Attempt_res;
        if (Attempt_res.text != null)
        {
            if (Attempt_res.text != "[]")
            {
                List<LevelUserdataModel> leveldata = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LevelUserdataModel>>(Attempt_res.text);
                GameAttemptNumber = leveldata.Count;
                Debug.Log("stage 1 game Attempt " + GameAttemptNumber);
            }
            else
            {
                GameAttemptNumber = 0;
            }


        }
    }
}
