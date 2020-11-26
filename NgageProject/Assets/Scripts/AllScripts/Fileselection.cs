using SFB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class Fileselection : MonoBehaviour
{
    // Start is called before the first frame update
    public static ClickAndGetImage instance;
    public GameObject SelectedBtn;
    //public Text imagedata;
    private string path;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    //[MenuItem("Example/Overwrite Texture")]
    public  void Apply(GameObject preview_btn)
    {
        //===================image selection working=========================================//
        if(Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            string[] image_path = StandaloneFileBrowser.OpenFilePanel("Select Image to Upload", "", "jpg", false);
            if (image_path.Length != 0)
            {
                SelectedBtn = preview_btn;
                byte[] image_data = File.ReadAllBytes(image_path[0]);
                StartCoroutine(SaveImageToServer.instance.show_image_mathod(WriteByte(image_data), SelectedBtn));
            
            }
        }
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
            {
                if (path != null)
                {
                    SelectedBtn = preview_btn;
                    Texture2D tex = NativeCamera.LoadImageAtPath(path, 1024, false);
                    byte[] image_data = tex.EncodeToPNG();
                    StartCoroutine(SaveImageToServer.instance.show_image_mathod(WriteByte(image_data), SelectedBtn));

                }
            }, 1024);
        }

    }

    public byte[] WriteByte(byte[] t)
    {

        return t;
    }


}
