using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SimpleSQL;
using System.Linq;

public class covidGameHandler : MonoBehaviour
{
    public GameObject Gameguide,covidGame,scorepanel;
    public GameBoard gameboard;
    public GameLeaderBoardScript GameLb;

    public SimpleSQLManager dbmanager;
    private AudioSource audioSource;
    public GameObject loadingbar;
    public CustomLoader loader;
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


    public void closeGameguide()
    {
        StartCoroutine(Closergameguide());
    }


    IEnumerator Closergameguide()
    {
        iTween.ScaleTo(Gameguide, Vector3.zero, 0.4f);
        yield return new WaitForSeconds(0.5f);
        Gameguide.SetActive(false);
        covidGame.SetActive(true);
        scorepanel.SetActive(true);
        gameboard.GamePlay();
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
        Gameguide.SetActive(true);

    }


}
