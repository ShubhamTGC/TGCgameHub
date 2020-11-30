using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ImageCollection : MonoBehaviour
{
    // Start is called before the first frame update
    public GameLeaderBoardScript GameLb;
    void Start()
    {
        
    }
    private void OnEnable()
    {
        StartCoroutine(AvatarfaceSprites());
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
