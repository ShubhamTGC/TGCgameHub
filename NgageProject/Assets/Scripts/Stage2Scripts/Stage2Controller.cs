using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using SimpleSQL;
using System.Linq;
using System.IO;
using SimpleSQL;
public class Stage2Controller : MonoBehaviour
{

    public GameObject GameGuide, Zonegame;
    public GameLeaderBoardScript GameLb;
    public SimpleSQLManager dbmanager;

    private AudioSource audioSource;
    public GameObject loadingbar;
    public CustomLoader Laoder;
    private void Awake()
    {
        audioSource = Camera.main.gameObject.GetComponent<AudioSource>();
        StartCoroutine(getSoundData());
    }


    IEnumerator getSoundData()
    {

        var soundLog = dbmanager.Table<SettingPage>().FirstOrDefault();
        if (soundLog != null)
        {
            audioSource.volume = soundLog.Music == 1 ? soundLog.Music / 2f : soundLog.Music;
        }
        else
        {
            audioSource.volume = 0.5f;
        }
        yield return new WaitForSeconds(0.1f);
    }
    void Start()
    {

    }
       
    private void OnEnable()
    {
        loadingbar.SetActive(true);
        StartCoroutine(AvatarfaceSprites());
       
    }

   
    // Update is called once per frame
    void Update()
    {

    }




    public IEnumerator scenechanges(GameObject parentobejct, Sprite new_sprite)
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





    public void BacktoHome()
    {
        SceneManager.LoadScene(0);
    }

    public void closeGameGuide()
    {
        StartCoroutine(gamePlayTask());
    }
    IEnumerator gamePlayTask()
    {
        iTween.ScaleTo(GameGuide, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        GameGuide.SetActive(false);
        Zonegame.SetActive(true);
    }
    private Sprite GetAvatarSprite(string path, string spritename)
    {
        if (path.Length > 0)
        {
            byte[] imagedata = File.ReadAllBytes(path);
            Texture2D texture2d = new Texture2D(1, 1);
            Sprite sprite;
            texture2d.LoadImage(imagedata);
            sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), new Vector2(0.5f, 0.5f));
            sprite.name = spritename;
            return sprite;
        }
        return null;
    }

    IEnumerator AvatarfaceSprites()
    {
        yield return new WaitForSeconds(0.05f);
        string path = Application.persistentDataPath + "/AvatarFile";
        foreach (string file in System.IO.Directory.GetFiles(path))
        {
            string spritename = Path.GetFileNameWithoutExtension(file);
            Sprite avatar = GetAvatarSprite(file, spritename);
            GameLb.AvatarModel.Add(avatar);
        }
        Laoder.Isdone = true;
        GameGuide.SetActive(true);

    }

    //IEnumerator getCmsdata()
    //{
    //    string Hitting = $"{MainUrl}{GetCmsConfigApi}?id_level={2}";
    //    WWW Cmswww = new WWW(Hitting);
    //    yield return Cmswww;
    //    int Totalscore = 0;
    //    if(Cmswww.text != null)
    //    {
    //        if(Cmswww.text != "[]")
    //        {
    //            List<Stage1CMSModel> StageCmsLog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Stage1CMSModel>>(Cmswww.text);
    //            StageCmsLog.ForEach(x =>
    //            {
    //                var LocalCmsLog = dbmanager.Table<WasteSeperation>().FirstOrDefault(y => y.ItemId == x.item_Id);
    //                if (LocalCmsLog == null)
    //                {
    //                    WasteSeperation WasteLog = new WasteSeperation
    //                    {
    //                        ItemId = x.item_Id,
    //                        ItemName = x.item_Name,
    //                        PCscore = x.partialcorrect_point,
    //                        Cscore = x.correct_point,
    //                        RoomId = x.id_room
    //                    };
    //                    dbmanager.Insert(WasteLog);
    //                }
    //                else
    //                {
    //                    LocalCmsLog.ItemId = x.item_Id;
    //                    LocalCmsLog.ItemName = x.item_Name;
    //                    LocalCmsLog.PCscore = x.partialcorrect_point;
    //                    LocalCmsLog.Cscore = x.correct_point;
    //                    LocalCmsLog.RoomId = x.id_room;
    //                    dbmanager.UpdateTable(LocalCmsLog);
    //                }

    //                Totalscore += x.correct_point;
    //            });

    //            var percentlog = dbmanager.Table<LevelPercentageTable>().FirstOrDefault(c => c.LevelId == 2).LevelPercentage;
    //            int FinalLevelScore = (Totalscore / 100) * percentlog;
    //            stage2zones.ForEach(x =>
    //            {
    //                x.Stage2UnlockScore = FinalLevelScore;
    //            });
    //            Stage2Parent.Stage2UnlockScore = FinalLevelScore;

    //            var scoreconfig = dbmanager.Table<ScoreConfiguration>().FirstOrDefault(a => a.levelId == 2);

    //            if (scoreconfig == null)
    //            {
    //                ScoreConfiguration log = new ScoreConfiguration
    //                {
    //                    levelId = 2,
    //                    TotalScore = Totalscore,
    //                    PercentScore = percentlog,
    //                    UnlockScore = FinalLevelScore
    //                };
    //                dbmanager.Insert(log);
    //            }
    //            else
    //            {
    //                scoreconfig.PercentScore = percentlog;
    //                scoreconfig.levelId = 2;
    //                scoreconfig.TotalScore = Totalscore;
    //                scoreconfig.UnlockScore = FinalLevelScore;
    //                dbmanager.UpdateTable(scoreconfig);
    //            }

    //        }
    //    }
    //}
}
