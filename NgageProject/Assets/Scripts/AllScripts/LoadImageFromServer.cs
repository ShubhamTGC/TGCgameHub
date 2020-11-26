using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.IO;

[System.Serializable]
public struct UserTagPhotoList
{
    public int id_org;
    public int id_user;
    public int id_game_content;
    public int id_level;
    public string photo_filename;
    public string id_lati;
    public string id_long;
    public string detail_info;
    public string key_info;
    public string status;

}

public class LoadImageFromServer : MonoBehaviour
{
    public GameObject galleryChild;
    public GameObject gellery_win;
    string URL;
    public Sprite defaultSprite;
    public List<UserTagPhotoList> ll;
    public GameObject galleryParent;
    string getUrl = "www.skillmuni.in/wsmapi/api/getTagPhotoList?UID=3649&OID=133&Level=1";
    public string Mainurl, getimage_API;
    public Text msgbox;
    public string tempUID="", tempOID="", tempLvl="";
    private GameObject gallery_prefeb;
    // Start is called before the first frame update
    void Start()
    {
        ll = new List<UserTagPhotoList>();
        //StartCoroutine(GetText());
        if(Application.platform == RuntimePlatform.Android)
        {
            gallery_prefeb = galleryChild;
        }
        else
        {
            gallery_prefeb = gellery_win;
        }
    }

    void OnEnable()
    {
        StartCoroutine(GetText());
    }
    // "usertagphotolist":[{"id_org":130,"id_user":3457,"id_game_content":1,"id_level":1,"photo_filename":"http://www.skillmuni.in/wsmapi/Content/TagedImage/3457130420201441.jpg","status":"A"},{"id_org":130,"id_user":3457,"id_game_content":1,"id_level":1,"photo_filename":"http://www.skillmuni.in/wsmapi/Content/TagedImage/3457130420201459.jpg","status":"A"},{"id_org":130,"id_user":3457,"id_game_content":1,"id_level":1,"photo_filename":"http://www.skillmuni.in/wsmapi/Content/TagedImage/3457130420201506.jpg","status":"A"},{"id_org":130,"id_user":3457,"id_game_content":1,"id_level":1,"photo_filename":"http://www.skillmuni.in/wsmapi/Content/TagedImage/3457130420202236.jpg","status":"A"}]

    // Update is called once per frame
    void Update()
    {

    }



    IEnumerator GetText()
    {
        //Debug.Log(Mainurl + getimage_API);

        string posting_url = "www.skillmuni.in/wsmapi/api/getTagPhotoList?UID=" + PlayerPrefs.GetInt("UID") + "&OID=" + PlayerPrefs.GetInt("OID") +
            "&Level=" + PlayerPrefs.GetInt("level_value");
        Debug.Log("main url " + posting_url);

        using (UnityWebRequest www = UnityWebRequest.Get(posting_url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
               
                if (tempUID != "" && tempOID != "" && tempLvl != "")
                {
                    if (tempUID != PlayerPrefs.GetInt("UID").ToString() || tempOID != PlayerPrefs.GetInt("OID").ToString() || tempLvl != PlayerPrefs.GetInt("level_value").ToString())
                    {
                        DestroyChild();
                        ll.Clear();
                    }

                    yield return new WaitForSeconds(0.2f);
                    Debug.Log(galleryParent.transform.childCount + " - " + ll.Count);
                }
                tempUID = PlayerPrefs.GetInt("UID").ToString();
                tempOID = PlayerPrefs.GetInt("OID").ToString();
                tempLvl = PlayerPrefs.GetInt("level_value").ToString();
                // Show results as text
                Debug.Log(www.downloadHandler.text);

                JSONObject js = new JSONObject(www.downloadHandler.text);
                // ll = new List<UserTagPhotoList>(js[1].Count);
                //string s = www.downloadHandler.text;
                Debug.Log(js[1].ToString());
                Debug.Log(galleryParent.transform.childCount);
                Debug.Log(js[1].Count);



                for (int i = galleryParent.transform.childCount; i < js[1].Count; i++)
                {

                    ll.Add(JsonUtility.FromJson<UserTagPhotoList>(js[1][i].ToString()));
                    GameObject gm = Instantiate(gallery_prefeb, new Vector3(0, 0, 0), Quaternion.identity);
                    gm.transform.SetParent(galleryParent.transform);
                    gm.transform.localPosition = new Vector3(0, 0, 0);
                    gm.transform.localScale = new Vector3(1, 1, 1);
                    //gm.transform.GetChild(0).GetComponent<Image>().sprite = ll[i].photo_filename;
                    StartCoroutine(GetTexture(gm.transform.GetChild(0).GetComponent<Image>(), ll[i].photo_filename));
                    gm.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>().text = ll[i].key_info;
                    gm.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>().text = ll[i].detail_info;
                    if(Application.platform == RuntimePlatform.Android)
                    {
                        gm.transform.GetChild(3).gameObject.transform.GetChild(0).GetComponent<Text>().text = ll[i].id_lati + "," + ll[i].id_long;
                    }

                    if (ll[i].id_level == 1)
                    {
                        gm.transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<Text>().text = "Residential";
                    }
                    else if (ll[i].id_level == 2)
                    {
                        gm.transform.GetChild(4).gameObject.transform.GetChild(0).GetComponent<Text>().text = "School";
                    }
                }

            }
        }
    }

    void DestroyChild()
    {
        for (int i = 0; i < galleryParent.transform.childCount; i++)
        {
            Destroy(galleryParent.transform.GetChild(i).gameObject);
        }
    }

    IEnumerator GetTexture(Image img, string Url)
    {

        //Debug.Log(Url);

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(Url, true);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //Debug.Log(path + name);
            Debug.Log(www.error);
            //m_LoadImg = StartCoroutine(GetTexture(path, name, img));
        }
        else
        {
            try
            {
                Texture2D texture2d = new Texture2D(1, 1);
                Sprite sprite = null;

                if (www.isDone)
                    if (texture2d.LoadImage(www.downloadHandler.data))
                    {

                        //sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
                        Debug.Log(" Image size " + texture2d.height + " " + texture2d.width);
                        if (texture2d.height <= 12 && texture2d.width <= 12)
                        {
                            sprite = defaultSprite;
                        }
                        else
                        {
                            sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
                        }

                    }

                if (sprite != null)
                {
                    img.sprite = sprite;
                    
                }
                else
                {
                    img.sprite = defaultSprite;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        // UIManager11.instance.loadingCanvas.SetActive(false);
    }

    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}
