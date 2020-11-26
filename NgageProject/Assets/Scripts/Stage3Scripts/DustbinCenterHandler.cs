using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustbinCenterHandler : MonoBehaviour
{
    public GameBoard Gamemanager;
    public GameObject CorrectAns, WrongAns;
    private AudioSource SoundEffect;
    public AudioClip CenterSound, wrongcenter;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        SoundEffect = this.GetComponent<AudioSource>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Truck")
        {
            if (this.gameObject.name == other.gameObject.name)
            {
                other.gameObject.SetActive(false);
                string truckname = other.gameObject.name;
                CorrectAns.transform.position = this.transform.position;
                string centername = "Hospital";
                StartCoroutine(AnsStatus(CorrectAns, Gamemanager.CenterRightpoints, truckname, centername, CenterSound));
            }
            else
            {
                string truckname = other.gameObject.name;
                other.gameObject.SetActive(false);
                string centername = this.gameObject.name;
                WrongAns.transform.position = this.transform.position;
                string CenterNamed = "";
                StartCoroutine(AnsStatus(WrongAns, Gamemanager.CenterWrongpoint, truckname, CenterNamed, wrongcenter));
            }
        }
    }

    IEnumerator AnsStatus(GameObject AnsEffect,int Score,string TruckName,string CenterName,AudioClip sound)
    {
        SoundEffect.clip = sound;
        SoundEffect.Play();
        AnsEffect.SetActive(true);
        Gamemanager.VibrateDevice();
        Gamemanager.Truckcentertask(Score, TruckName,CenterName);
        yield return new WaitForSeconds(1f);
    }
}
