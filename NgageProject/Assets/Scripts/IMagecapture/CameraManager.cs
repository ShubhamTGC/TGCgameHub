using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    bool isCameraAvailale;
    public WebCamTexture backCamera;
    Texture defaultBg;
    public SaveImageToServer after_capture;
    public RawImage background;
    public GameObject click_button;
    private GameObject SelectedBtn;
    public GameObject captureBtn, CloseBtn;
    // Start is called before the first frame update
    void Start()
    {
        OpenBackCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCameraAvailale == false)
            return;


        float scaleY = backCamera.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1, scaleY, 1);

        int orient = -backCamera.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

    public void CloseCamera()
    {
        background.gameObject.SetActive(false);
        captureBtn.SetActive(false);
        CloseBtn.SetActive(false);
        after_capture.reset_objects();
        backCamera.Stop();
    }

    public void PlayCamera(GameObject thisobject)
    {
        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            captureBtn.SetActive(true);
            CloseBtn.SetActive(true);
            click_button = thisobject;
            Debug.Log("hitting");
            background.gameObject.SetActive(true);
            backCamera.Play();
            background.texture = backCamera;
            isCameraAvailale = true;
        }
      
    }

    public void OpenBackCamera()
    {
        defaultBg = background.texture;
        WebCamDevice[] deveices = WebCamTexture.devices;

        if (deveices.Length == 0)
        {
            Debug.Log("No Camera Detected");
            isCameraAvailale = false;
            return;
        }

        for (int i = 0; i < deveices.Length; i++)
        {
            if (!deveices[i].isFrontFacing)
            {
                backCamera = new WebCamTexture(deveices[i].name, Screen.width, Screen.height);
            }

            if (backCamera == null)
            {
                Debug.Log("No Back Camera Found");
                return;
            }


        }
    }


    public void TakePic(GameObject PIcObject)
    {
        SelectedBtn = PIcObject;
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
            {

                if (path != null)
                {
                    Texture2D tex = NativeCamera.LoadImageAtPath(path, 1024);
                    if (tex == null)
                    {
                        return;
                    }
                    byte[] image_data = File.ReadAllBytes(path);
                    if (image_data != null)
                    {
                        //imagedata.text = " image found ";
                        StartCoroutine(SaveImageToServer.instance.show_image_mathod(WriteByte(image_data), SelectedBtn));
                    }
                    else
                    {
                        Debug.Log("image data null ");
                        //imagedata.text = " image data null";
                    }

                    //preview_btn.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                    //CapturedImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
                }


            });
        }
    }

    public byte[] WriteByte(byte[] t)
    {

        return t;
    }

}
