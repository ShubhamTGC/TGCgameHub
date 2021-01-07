using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedeemCouponHandler : MonoBehaviour
{
    private string LogicalbaniyaUrl = "https://demo.thelogicalbanya.com/kbba/";
    private string PostCoupondataApi = "http://140.238.249.68/TGCGame/api/rewardsreedmeLog";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenWebPage()
    {
        //string Url = "https://demo.thelogicalbanya.com/kbba/?clientID=demo&key=demo&userID=25&rewards=MjUwMA==&username=aman&postURL=http://140.238.249.68/TGCGame/api/rewardsreedmeLog&returnURL=https://www.demo-bank.com/&PartnerCode=demo&model=1";
        //Application.OpenURL(Url);
        
        string Url = $"{LogicalbaniyaUrl}?clientID=demo&key=demo&userID={PlayerPrefs.GetInt("UID")}&rewards=MjUwMA==&username={PlayerPrefs.GetString("Username")}&postURL={PostCoupondataApi}&returnURL=https://www.demo-bank.com/&PartnerCode=demo&model=1";
        Application.OpenURL(Url);
    }
}
