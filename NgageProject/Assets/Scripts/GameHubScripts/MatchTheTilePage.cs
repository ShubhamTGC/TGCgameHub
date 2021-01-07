using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleSQL;
using System.Linq;
using UnityEngine.UI;

public class MatchTheTilePage : MonoBehaviour
{
    public GameObject GameGuidepage;
    public GameObject Game;
    public MatchTheTile matchThetile;
    public SimpleSQLManager dbmanager;
    public GameLeaderBoardScript GameLb;
    private AudioSource audioSource;
    public GameObject Loadingbar;
    public CustomLoader loader;
    [SerializeField] private int gameid = 4;
    private Sprite gamesprite;
    public GameObject gamemainpage;
    private void Awake()
    {
        Loadingbar.SetActive(true);
        StartCoroutine(AvatarSelection());
        audioSource = Camera.main.gameObject.GetComponent<AudioSource>();
        var gameimage = dbmanager.Table<GameListDetails>().FirstOrDefault(x => x.GameId == gameid).BackgroundImgURL;
        StartCoroutine(GameIamgeSprite(gameimage));
        StartCoroutine(getSoundData());
    }
    void Start()
    {
        
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




    private void OnEnable()
    {
        //GameGuidepage.SetActive(true);
    }


    public void ExitPage()
    {
        SceneManager.LoadScene(0);
    }

    public void Gameplaytask()
    {
        StartCoroutine(Gameplay());
    }

    IEnumerator Gameplay()
    {
        iTween.ScaleTo(GameGuidepage, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        GameGuidepage.SetActive(false);
        Game.SetActive(true);
    }

    IEnumerator AvatarSelection()
    {
        var FlagName = dbmanager.Table<MatchTheTileFlag>().ToList();
        int Flagcounter = 0;
        int Locationcounter = 0;
        Debug.Log(Application.persistentDataPath);
        string flagpath = Application.persistentDataPath + "/MatchTheTileGameFlag";
        string loactionpath = Application.persistentDataPath + "/MatchTheTileGameLocation";
        System.IO.DirectoryInfo Flagdir = new System.IO.DirectoryInfo(flagpath);
        System.IO.DirectoryInfo Loactiondir = new System.IO.DirectoryInfo(flagpath);
        matchThetile.DustbinSprite = new Sprite[Flagdir.GetFiles().Length];
        matchThetile.WasteSprite = new Sprite[Loactiondir.GetFiles().Length];
        foreach (string file in System.IO.Directory.GetFiles(flagpath))
        {
            string flagtag = "";
            string spritename = Path.GetFileNameWithoutExtension(file);
            FlagName.ForEach(x =>
            { 
                if(x.IdFlag == int.Parse(spritename))
                {
                    flagtag = x.Flag;
                }
               
            });
            //string flagtag = FlagName[Flagcounter].Flag;
            Sprite avatar = GetAvatarSprite(file, flagtag);
            matchThetile.DustbinSprite[Flagcounter] = avatar;
            Flagcounter++;
            yield return new WaitForSeconds(0.1f);

        }

        foreach (string file in System.IO.Directory.GetFiles(loactionpath))
        {
            string spritename = Path.GetFileNameWithoutExtension(file);

            Sprite avatar = GetAvatarSprite(file, spritename);
            matchThetile.WasteSprite[Locationcounter] = avatar;
            Locationcounter++;
            yield return new WaitForSeconds(0.1f);

        }
        StartCoroutine(AvatarfaceSprites());
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

        loader.Isdone = true;
        GameGuidepage.SetActive(true);

    }

    IEnumerator GameIamgeSprite(string imageurl)
    {
        yield return new WaitForSeconds(0.05f);
        gamesprite = GetAvatarSprite(imageurl, "background");
        gamemainpage.GetComponent<Image>().sprite = gamesprite;


    }
}
