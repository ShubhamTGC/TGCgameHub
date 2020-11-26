using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.IO;

public class SaveImageToServerLatest : MonoBehaviour
{
    public Image rawImagel;
    string URL = "www.skillmuni.in/wsmapi/api/PostPhotoUpload/TagPhotoUpload";
    Byte[] imageBytes;
    Texture2D test;
    byte[] b;
    // Start is called before the first frame update
    void Start()
    {
        b = new byte[1];
        test = new Texture2D(256, 256, TextureFormat.RGB24, true);
        test.name = "test.png";
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnButtonClick()
    {
       //StartCoroutine(Upload());
        StartCoroutine(SaveToServer(URL));
    }

    public IEnumerator SaveToServer(string URL)
    {
        yield return new WaitForEndOfFrame();
        int width = Screen.width;
        int height = Screen.height;
        Texture2D tex = new Texture2D(width , height, TextureFormat.RGB24, true);
        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();
       // Debug.Log(BitConverter.ToString(tex.EncodeToPNG()));
        Texture2D newScreenshot = ScaleTexture(tex, 256, 256);
        // Encode texture into PNG
       
        var bytes = newScreenshot.EncodeToPNG();
        //Debug.Log(BitConverter.ToString(bytes));
        tex.name = "test.Png";
        imageBytes = bytes;
        //rawImagel.gameObject.SetActive(true);
        //rawImagel.sprite = SpriteFromTexture2D(tex);
        test = tex;
        
        StartCoroutine(Upload());
    }

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, true);
        Color[] rpixels = result.GetPixels(0);
        float incX = ((float)1 / source.width) * ((float)source.width / targetWidth);
        float incY = ((float)1 / source.height) * ((float)source.height / targetHeight);
        for (int px = 0; px < rpixels.Length; px++)
        {
            rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth),
                              incY * ((float)Mathf.Floor(px / targetWidth)));
        }
        result.SetPixels(rpixels, 0);
        result.Apply();
        return result;
    }

    IEnumerator Upload()
    {
       
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("UID", "3649"));
        formData.Add(new MultipartFormDataSection("OID", "133"));
        formData.Add(new MultipartFormDataSection("EXTN", "png"));
        formData.Add(new MultipartFormFileSection("Media",b , test.name, "image/png"));
        formData.Add(new MultipartFormDataSection("GCI", "1"));
        formData.Add(new MultipartFormDataSection("Level", "1"));
        formData.Add(new MultipartFormDataSection("LATI", "50"));
        formData.Add(new MultipartFormDataSection("LONGI", "30"));
        formData.Add(new MultipartFormDataSection("DETAIL", "1"));
        formData.Add(new MultipartFormDataSection("KEYINFO", "2"));

        UnityWebRequest www = UnityWebRequest.Post(URL, formData);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!" + www.downloadHandler.text);
        }
    }

    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}
