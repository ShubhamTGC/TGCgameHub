using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumevalueChange : MonoBehaviour
{
    // Reference to Audio Source component
    private AudioSource audioSrc;
    public Text volumevalue;
    // Music volume variable that will be modified
    // by dragging slider knob
    public float musicVolume = 1f;

    // Use this for initialization
    void Start()
    {

       
       audioSrc = GetComponent<AudioSource>();
        
        
    }

    private void OnEnable()
    {
        if (!PlayerPrefs.HasKey("volume"))
        {
            musicVolume = 1f;
        }
        else
        {
            musicVolume = PlayerPrefs.GetFloat("volume");
        }
    }

    // Update is called once per frame
    void Update()
    {

        // Setting volume option of Audio Source to be equal to musicVolume
        
        audioSrc.volume = musicVolume;
        
      
    }

    // Method that is called by slider game object
    // This method takes vol value passed by slider
    // and sets it as musicValue
    public void SetVolume(float vol)
    {
        musicVolume = vol;
        float volume = vol * 100;
        volumevalue.text = volume.ToString("F0");
        PlayerPrefs.SetFloat("volume", musicVolume);
    }
}
