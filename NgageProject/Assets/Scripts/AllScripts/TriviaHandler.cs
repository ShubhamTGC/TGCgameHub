using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YoutubePlayer
{
    public class TriviaHandler : MonoBehaviour
    {
       
        
        public Image LoadingBar;
        [HideInInspector]
        public bool Laodingstart;
        [SerializeField]
        private float totaltime;
        [SerializeField]
        private float LimitValue;
        private float currentTime;
        public bool videoplayed = false;
        public HomePageScript HomeBool;
        // Start is called before the first frame update
        void Start()
        {

        }

        void OnEnable()
        {
            Laodingstart = true;
            StartCoroutine(CustomLoader());
        }

        void OnDisable()
        {
            Laodingstart = false;
            currentTime = 0f;
            LoadingBar.fillAmount = 0f;

        }

        // Update is called once per frame
        void Update()
        {

        }

        IEnumerator CustomLoader()
        {
            if(currentTime < LimitValue)
            {
                 yield return new WaitForSeconds(0.5f);
                if (HomeBool.DownloadDone)
                {
                    LoadingBar.fillAmount = 1f;
                    yield return new WaitForSeconds(0.5f);
                    Laodingstart = false;
                    this.gameObject.SetActive(false);
                    
                }
                else
                {
                    currentTime += 2f;
                    LoadingBar.fillAmount = currentTime / totaltime;
                }
               
            }
             else
            {
                while (!HomeBool.DownloadDone)
                {
                    yield return new WaitForSeconds(0.1f);

                }
                yield return new WaitForSeconds(0.5f);
                currentTime += 2f;
                LoadingBar.fillAmount = currentTime / totaltime;
                if(LoadingBar.fillAmount == 1)
                {
                    Laodingstart = false;
                    this.gameObject.SetActive(false);
                }

            }

            if (Laodingstart)
            {
                StartCoroutine(CustomLoader());
            }
        }
       
    }



    
}

