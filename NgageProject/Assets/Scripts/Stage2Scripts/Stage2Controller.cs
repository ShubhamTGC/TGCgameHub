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
    [SerializeField] private int Gameid;
    public List<ThrowWasteHandler> GameLevelhandlers;
    private List<int> RoomIds = new List<int>();
    public List<KeyValuePair<string, List<Sprite>>> Imagedata = new List<KeyValuePair<string, List<Sprite>>>();
    private void Awake()
    {
        audioSource = Camera.main.gameObject.GetComponent<AudioSource>();
        StartCoroutine(getSoundData());
        var GameLog = dbmanager.Table<ObjectGameList>().FirstOrDefault(x => x.GameId == Gameid).RoomId;
        var unique = dbmanager.Table<ObjectGameList>().Where(y => y.GameId == Gameid).Select(x => x.RoomId).Distinct().ToList();
        RoomIds = unique;
        for (int a = 0; a < GameLevelhandlers.Count; a++)
        {
            GameLevelhandlers[a].LevelRoomid = unique[a];
        }

        StartCoroutine(GetGameSprites());
    }

    IEnumerator GetGameSprites()
    {
        var dbSet = dbmanager.Table<ObjectGameList>().ToList();
        for (int a = 0; a < RoomIds.Count; a++)
        {
            var ObjectUrl = dbSet.Where(x => x.RoomId == RoomIds[a]).Select(b => b.ObjItemImgURL).ToList();
            StartCoroutine(GetItemImage(ObjectUrl, RoomIds[a].ToString()));
            yield return new WaitForSeconds(0.2f);
        }
    }
    IEnumerator GetItemImage(List<string> sprite,string roomid)
    {
        yield return new WaitForSeconds(0.05f);
        List<Sprite> imagesprite = new List<Sprite>();
        foreach (string file in sprite)
        {
            string spritename = Path.GetFileNameWithoutExtension(file);
            Sprite avatar = getGamesSpritesMethod(file, spritename);
            imagesprite.Add(avatar);
            Debug.Log("created sprite " + spritename);
            GameLb.AvatarModel.Add(avatar);
        }
        Imagedata.Add(new KeyValuePair<string, List<Sprite>>(roomid, imagesprite));
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


    private Sprite getGamesSpritesMethod(string path, string spritename)
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
}
