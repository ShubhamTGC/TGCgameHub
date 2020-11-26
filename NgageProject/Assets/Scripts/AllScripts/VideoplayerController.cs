using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoplayerController : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(PlayVideo());
    }
    private void OnEnable()
    {
        StartCoroutine(PlayVideo());
    }

    void OnDisable()
    {
        rawImage.gameObject.GetComponent<RawImage>().enabled = false;
        videoPlayer.Stop();
    }
    IEnumerator PlayVideo()
    {
        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }
        rawImage.gameObject.GetComponent<RawImage>().enabled = true;
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
        audioSource.Play();
    }
}
