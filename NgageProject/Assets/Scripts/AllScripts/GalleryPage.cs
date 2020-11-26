using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


[System.Serializable]
public struct UserTagPhotoList_field
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
public class GalleryPage : MonoBehaviour
{
 
    public GameObject galleryChild;
    string URL;
    public Sprite defaultSprite;
    public List<UserTagPhotoList_field> ll;
    public GameObject galleryParent;
    string getUrl = "www.skillmuni.in/wsmapi/api/getTagPhotoList?UID=3649&OID=133&Level=1";
    public string Mainurl, getimage_API;
    public Text msgbox;

    // Start is called before the first frame update
    void Start()
    {
        ll = new List<UserTagPhotoList_field>();
        StartCoroutine(GetText());
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
            "&Level=" + PlayerPrefs.GetInt("game_id");
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

                // Show results as text
                Debug.Log(www.downloadHandler.text);

                JSONObject js = new JSONObject(www.downloadHandler.text);
                // ll = new List<UserTagPhotoList>(js[1].Count);
                //string s = www.downloadHandler.text;
                Debug.Log(js[1].ToString());

                if (js[1].Count == 0)
                {
                    msgbox.gameObject.SetActive(true);
                }
                else
                {
                    msgbox.gameObject.SetActive(false);

                    Debug.Log(galleryParent.transform.childCount);
                    Debug.Log(js[1].Count);
                    for (int i = galleryParent.transform.childCount; i < js[1].Count; i++)
                    {

                        ll.Add(JsonUtility.FromJson<UserTagPhotoList_field>(js[1][i].ToString()));
                        GameObject gm = Instantiate(galleryChild, new Vector3(0, 0, 0), Quaternion.identity);
                        gm.transform.SetParent(galleryParent.transform);
                        gm.transform.localPosition = new Vector3(0, 0, 0);
                        gm.transform.localScale = new Vector3(1, 1, 1);
                        //gm.transform.GetChild(0).GetComponent<Image>().sprite = ll[i].photo_filename;
                        StartCoroutine(GetTexture(gm.transform.GetChild(0).GetComponent<Image>(), ll[i].photo_filename));
                        gm.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<Text>().text = ll[i].key_info;
                        gm.transform.GetChild(2).gameObject.transform.GetChild(0).GetComponent<Text>().text = ll[i].detail_info;
                        gm.transform.GetChild(3).gameObject.transform.GetChild(0).GetComponent<Text>().text = ll[i].id_lati + "," + ll[i].id_long;
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
    }

    IEnumerator GetTexture(Image img, string Url)
    {

        Debug.Log(Url);

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
                //Debug.Log(www.downloadHandler.data.Length + path + name);
                if (www.isDone)
                    if (texture2d.LoadImage(www.downloadHandler.data))
                    {
                        sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
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
