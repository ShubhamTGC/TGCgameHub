using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSQL;
using System.Linq;
using UnityEngine.UI;
public class MonsterSpeedController : MonoBehaviour
{
    // Start is called before the first frame update
    public SimpleSQLManager dbmanager;
    [SerializeField]private List<float> SpeedValues = new List<float>();
    public GameBoard PlayerManager;
    private List<int> SpeedScoreLimits = new List<int>();
    public List<MonsterMovement> MonstersFollower;
    public List<RandomMonsterMove> MonsterAI;
    public GameObject LevelText;
    private int levelid =0,counter=0;
    void Start()
    {
        
    }

     void OnEnable()
    {
        var Speeddata = dbmanager.Table<MonsterSpeed>().Select(x => x.Score).ToList();
        var speedValue = dbmanager.Table<MonsterSpeed>().Select(y => y.SpeedValue).ToList();
        SpeedValues = speedValue;
        SpeedScoreLimits = Speeddata;

    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerManager.ScorePointCounter == SpeedScoreLimits[0] && levelid == counter)
        {
            MonstersFollower[0].moveSpeed = SpeedValues[0];
            StartCoroutine(LevelClearness(1));
        }
        else if(PlayerManager.ScorePointCounter == SpeedScoreLimits[1] && levelid == counter)
        {
            
            MonstersFollower[1].moveSpeed = SpeedValues[1];
            MonsterAI[0].moveSpeed = SpeedValues[1];
            StartCoroutine(LevelClearness(2));
        }
        else if(PlayerManager.ScorePointCounter == SpeedScoreLimits[2] && levelid == counter)
        {
            
            MonstersFollower[0].moveSpeed = SpeedValues[2];
            MonstersFollower[1].moveSpeed = SpeedValues[2];
            MonsterAI[0].moveSpeed = SpeedValues[1];
            MonsterAI[1].moveSpeed = SpeedValues[3];
            StartCoroutine(LevelClearness(3));
        }
        else if (PlayerManager.ScorePointCounter == SpeedScoreLimits[3] && levelid == counter)
        {

            MonstersFollower[0].moveSpeed = SpeedValues[2];
            MonstersFollower[1].moveSpeed = SpeedValues[3];
            MonsterAI[0].moveSpeed = SpeedValues[3];
            MonsterAI[1].moveSpeed = SpeedValues[3];
            StartCoroutine(LevelClearness(4));
        }
    }

    IEnumerator LevelClearness(int level)
    {
        LevelText.GetComponent<Text>().text = "Level " + level + "\nComplete";
        LevelText.SetActive(true);
        Time.timeScale = 0.4f;
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 1f;
        LevelText.SetActive(false);
        counter += 1;
        levelid = level + 1;
    }
}
