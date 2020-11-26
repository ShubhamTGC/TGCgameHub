using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarUpgradeInfo : MonoBehaviour
{
    public List<Sprite> BoyFaces;
    public List<Sprite> GirlFaces;
    public Image BoyFace,GirlFace;
    public GameObject BoyObejct, GirlObject;
    public GameObject stars;
    public string Mainurl, Userdata_API;
    public int BodyType;
    [SerializeField] private int stage3AvatarType;
    void Start()
    {
        stars.SetActive(true);
        if (PlayerPrefs.GetString("gender").Equals("m",System.StringComparison.OrdinalIgnoreCase))
        {
            BoyObejct.SetActive(true);
            GirlObject.SetActive(false);
            SetFaceImage(BoyFaces, BoyFace);
        }
        else
        {
            BoyObejct.SetActive(false);
            GirlObject.SetActive(true);
            SetFaceImage(GirlFaces, GirlFace);
        }
        StartCoroutine(PostUserData());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetFaceImage(List<Sprite> Faces,Image PlayerFace)
    {
        for(int a = 0; a < Faces.Count; a++)
        {
            if(a == PlayerPrefs.GetInt("characterType"))
            {
                PlayerFace.sprite = Faces[a];
            }
        }
    }


    IEnumerator PostUserData()
    {
        string gender = PlayerPrefs.GetString("gender");
        int avatar_updated = stage3AvatarType + PlayerPrefs.GetInt("characterType");
        yield return new WaitForSeconds(1f);
        string avatar_url = Mainurl + Userdata_API;

        WWWForm userdata_form = new WWWForm();
        userdata_form.AddField("UID", PlayerPrefs.GetInt("UID"));
        userdata_form.AddField("OID", PlayerPrefs.GetInt("OID"));
        userdata_form.AddField("avatar_type", avatar_updated);
        userdata_form.AddField("body_type", BodyType);
        userdata_form.AddField("Gender", gender);
        Debug.Log("avatra data " + avatar_updated);
        WWW avatar_www = new WWW(avatar_url, userdata_form);
        yield return avatar_www;
        if (avatar_www.text != null)
        {
            Debug.Log(avatar_www.text);
            PlayerPrefs.SetString("Stage2Avatar", "done");
            PlayerPrefs.SetString("Stage3Avatar", "done");
            PlayerPrefs.SetInt("characterType", avatar_updated);
            PlayerPrefs.SetInt("avatar_type", avatar_updated);
            PlayerPrefs.SetInt("PlayerBody", BodyType);
            PlayerPrefs.SetString("gender", gender);


        }

    }
}
