using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleSQL;
using System.Linq;
public class BrightnessControl : MonoBehaviour
{
    

    public Sprite OnSprite, OffSprite;
    public SimpleSQLManager dbmanager;
    public Button Musicbtn, Soundbtn, vibrationbtn;
    private int MusicValue,SoundValue,VibrationValue;
    public Text AppversionName;
    void Start()
    {
     
    }

    private void OnEnable()
    {
        AppversionName.text = "Version : " + Application.version.ToString();
        var SettingLog = dbmanager.Table<GameSetting>().FirstOrDefault();
        if(SettingLog != null)
        {
            Musicbtn.image.sprite = SettingLog.Music == 1 ? OnSprite : OffSprite;
            Soundbtn.image.sprite = SettingLog.Sound == 1 ? OnSprite : OffSprite;
            vibrationbtn.image.sprite = SettingLog.Vibration == 1 ? OnSprite : OffSprite;
            Camera.main.gameObject.GetComponent<AudioSource>().volume = SettingLog.Music;
            MusicValue = SettingLog.Music;
            SoundValue = SettingLog.Sound;
            VibrationValue = SettingLog.Vibration;

        }
        else
        {
            Musicbtn.image.sprite = Soundbtn.image.sprite = vibrationbtn.image.sprite= OnSprite;
            MusicValue = SoundValue = VibrationValue =1;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //public void SetVolume(float val)
    //{
    //    brightnessvalue = 1- val;
    //    float value = val * 100;
    //    valuetext.text = value.ToString("F0");
    //}


   

    public void SoundControl()
    {
        if(Soundbtn.image.sprite.name == "On")
        {
            Soundbtn.image.sprite = OffSprite;
            SoundValue = 0;
        }
        else
        {
            SoundValue = 1;

            Soundbtn.image.sprite = OnSprite;
        }
    }

    public void VibrationControl()
    {
        if (vibrationbtn.image.sprite.name == "On")
        {
            VibrationValue = 0;
            vibrationbtn.image.sprite = OffSprite;
        }
        else
        {
            VibrationValue = 1;
            vibrationbtn.image.sprite = OnSprite;
        }
    }

    public void MusicControl()
    {
        if (Musicbtn.image.sprite.name == "On")
        {
            Musicbtn.image.sprite = OffSprite;
            MusicValue = 0;
            Camera.main.gameObject.GetComponent<AudioSource>().volume = MusicValue;
        }
        else
        {
            Musicbtn.image.sprite = OnSprite;
            MusicValue = 1;
            Camera.main.gameObject.GetComponent<AudioSource>().volume = MusicValue;

        }
    }

    public void saveData()
    {
        StartCoroutine(SaveSettingdata());
   
    }

    IEnumerator SaveSettingdata()
    {
        var Log = dbmanager.Table<GameSetting>().FirstOrDefault();
        if (Log == null)
        {
            GameSetting Gamelog = new GameSetting
            {
                Music = MusicValue,
                Sound = SoundValue,
                Vibration = VibrationValue
            };
            dbmanager.Insert(Gamelog);
        }
        else
        {
            Log.Music = MusicValue;
            Log.Sound = SoundValue;
            Log.Vibration = VibrationValue;
            dbmanager.UpdateTable(Log);
        }
        yield return new WaitForSeconds(0.4f);
        this.gameObject.SetActive(false);
    }



}
