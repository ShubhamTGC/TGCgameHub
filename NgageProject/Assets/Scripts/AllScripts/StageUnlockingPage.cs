using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSQL;
using System.Linq;

public class StageUnlockingPage : MonoBehaviour
{
    // Start is called before the first frame update
    public string MainUrl, StageUnlockApi;
    public int Stage1Score, Stage2Score;
    public Button Stage2Btn, Stage3Btn;
    public SimpleSQLManager dbmanager;
    void Start()
    {
        StartCoroutine(GetUnlockStage());
    }

    private void OnEnable()
    {
        
        StartCoroutine(GetUnlockStage());

    }

  

    IEnumerator GetUnlockStage()
    {
        yield return new WaitForSeconds(0.1f);
        var Log = dbmanager.Table<StageClearness>().ToList();
        Log.ForEach(x =>
        {
            if (x.LevelId == 1)
            {
                Stage2Btn.interactable = x.IsClear == 1 ? true : false;
            }
            if (x.LevelId == 2)
            {
                Stage3Btn.interactable = x.IsClear == 1 ? true : false;
            }
        });
    }
}
