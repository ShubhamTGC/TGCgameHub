using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class GetLocation : MonoBehaviour
{

    //public Text statusTxt;
    // Start is called before the first frame update
    void Start()
    {

        CheckLocationPermission();
        GetUserLocation();
    }

    private void OnEnable()
    {
        GetUserLocation();
    }
    // Update is called once per frame
    void Update()
    {

    }

    void CheckLocationPermission()
    {

        if(UnityEngine.Application.platform == RuntimePlatform.Android)
        {
            if (!Input.location.isEnabledByUser) //FIRST IM CHACKING FOR PERMISSION IF "true" IT MEANS USER GAVED PERMISSION FOR USING LOCATION INFORMATION
            {
                //statusTxt.text = "No Permission please allow to access the location";
                Permission.RequestUserPermission(Permission.FineLocation);
            }
            Permission.RequestUserPermission(Permission.Camera);
        }

    }

    public void GetUserLocation()
    {
        if (UnityEngine.Application.platform == RuntimePlatform.Android)
        {
            CheckLocationPermission();
            //statusTxt.text = "Ok Permission";
            StartCoroutine("GetLatLonUsingGPS");
        }

    }

    IEnumerator GetLatLonUsingGPS()
    {
        Input.location.Start();
        int maxWait = 5;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }
        if (maxWait < 1)
        {
            //statusTxt.text = "Failed To Iniyilize in 10 seconds";
            yield break;
        }
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            //statusTxt.text = "Failed To Initialize";
            yield break;
        }
        else
        {
           // statusTxt.text = "waiting before getting lat and lon";
            yield return new WaitForSeconds(5);
            // Access granted and location value could be retrieve
            double longitude = Input.location.lastData.longitude;
            double latitude = Input.location.lastData.latitude;
            if (Input.location.status == LocationServiceStatus.Running)
            {
                //Get the location data here
                //statusTxt.text = "" + Input.location.status + "  lat:" + latitude + "  long:" + longitude;
                PlayerPrefs.SetFloat("LAT", (float)latitude);
                PlayerPrefs.SetFloat("LONG", (float)longitude);
            }
            //AddLocation(latitude, longitude);

        }
        //Stop retrieving location
        //Input.location.Stop();
        StopCoroutine("Start");
    }

}
