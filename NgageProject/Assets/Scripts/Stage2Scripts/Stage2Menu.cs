using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Menu : MonoBehaviour
{
    public string MainUrl, GetlevelWisedataApi;
    public int Gamelevel;
    [HideInInspector]
    public int GameAttemptNumber;
    void Start()
    {
        
    }


    private void OnEnable()
    {
        StartCoroutine(GetGameAttemptNoTask());
    }

    // Update is called once per frame
    void Update()
    {
        
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
                Debug.Log("stage 2 game Attempt " + GameAttemptNumber);
            }
            else
            {
                GameAttemptNumber = 0;
            }


        }
    }
}
