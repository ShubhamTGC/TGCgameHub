using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraTest : MonoBehaviour
{
    // Start is called before the first frame update
    private NativeCamera.CameraCallback callback;
    public Text Image_path;
    private string path;
    public Image CapturedImage;
    public Button clickbtn;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TakePic()
    {
        NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
        {
            Image_path.text = path;
            if(path != null)
            {
                Texture2D tex = NativeCamera.LoadImageAtPath(path,512);
                if(tex == null)
                {
                    return;
                }
                CapturedImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                CapturedImage.gameObject.SetActive(true);
                clickbtn.gameObject.SetActive(false);
            }


        });
     
   


    }
}
