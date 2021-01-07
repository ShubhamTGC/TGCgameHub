using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSQL;
using System.IO;
using System.Linq;
using UnityEngine.UI;

public class AnagramGame : MonoBehaviour
{
    public GameObject GameGuide, Quizgame;
    public AnagrameController Anagramcontroller;
    public SimpleSQLManager dbmanager;
    [HideInInspector]public List<Sprite> PlayerAvatar = new List<Sprite>();
    public GameLeaderBoardScript GameLb;
    public GameObject loadingbar;
    public CustomLoader loader;
    [SerializeField]private int gameid =2;
    public GameObject Gamepage;
    private Sprite Gamebgsprite;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = Camera.main.gameObject.GetComponent<AudioSource>();
        StartCoroutine(getSoundData());
    }


    IEnumerator getSoundData()
    {
        var GameImage = dbmanager.Table<GameListDetails>().FirstOrDefault(a => a.GameId == 2).BackgroundImgURL;
        StartCoroutine(GameBackgroundImageTask(GameImage));
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
        StartCoroutine(GameImageSelection());
        
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void Closegameguide()
    {
        StartCoroutine(OpeningGame());
    }

    IEnumerator OpeningGame()
    {

        iTween.ScaleTo(GameGuide, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        GameGuide.SetActive(false);
        Quizgame.SetActive(true);
    }

 

    IEnumerator GameImageSelection()
    {
        int counter = 0;
        Debug.Log(Application.persistentDataPath);
        string path = Application.persistentDataPath + "/AnagramFiles";
        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
        Anagramcontroller.AnswerSprite = new Sprite[dir.GetFiles().Length];
        foreach (string file in System.IO.Directory.GetFiles(path))
        {
            Debug.Log("image name " + file);
            string spritename = Path.GetFileNameWithoutExtension(file);
            Sprite avatar = GetAvatarSprite(file, spritename);
            Anagramcontroller.AnswerSprite[counter] = avatar;
            counter++;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(AvatarSelection());
      
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

    IEnumerator AvatarSelection()
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
        GameGuide.SetActive(true);

    }

    IEnumerator GameBackgroundImageTask(string Imageurl)
    {
        yield return new WaitForSeconds(0.05f);
        Gamebgsprite = GetAvatarSprite(Imageurl, "Background");
        Gamepage.GetComponent<Image>().sprite = Gamebgsprite;
    }

}
