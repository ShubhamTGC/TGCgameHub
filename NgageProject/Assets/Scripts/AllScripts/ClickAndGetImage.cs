using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class ClickAndGetImage : MonoBehaviour
{
    public static ClickAndGetImage instance;
    public byte[] pngData;
    public GameObject SelectedBtn;
    public byte[] imagebyte;

    void Awake()
    {
        instance = this;
    }

    public void OnGetImage(GameObject previewObj)
    {
       
        SelectedBtn = previewObj;
        // NOTE: gameObject.name MUST BE UNIQUE!!!!
        GetImage.GetImageFromUserAsync(gameObject.name, "ReceiveImage");
     


    }

    static string s_dataUrlPrefix = "data:image/png;base64,";
    public void ReceiveImage(string dataUrl)
    {
#if UNITY_WEBGL

        if (dataUrl.StartsWith(s_dataUrlPrefix))
        {
            pngData = System.Convert.FromBase64String(dataUrl.Substring(s_dataUrlPrefix.Length));
            StartCoroutine(SaveImageToServer.instance.show_image_mathod(WriteByte(pngData), SelectedBtn));
           // imagebyte = WriteByte(pngData);

            // Create a new Texture (or use some old one?)
            // return pngData;
        }
        else
        {
            Debug.LogError("Error getting image:" + dataUrl);
            // return null;
        }
#endif
    }

    public byte[] WriteByte(byte[] t)
    {
      
        return t;
    }
}