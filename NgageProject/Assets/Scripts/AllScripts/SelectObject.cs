using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class SelectObject : MonoBehaviour
{
    // Start is called before the first frame update
    public KeyCode Key,HitKey;
    public List<GameObject> AllObjects;
    private int ObjectIndex =0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(Key))
        {
            if(ObjectIndex == AllObjects.Count)
            {
                ObjectIndex = 0;
            }
            EventSystem.current.SetSelectedGameObject(AllObjects[ObjectIndex]);
            string Buttonname = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log(Buttonname);
            ObjectIndex++;
        }


        if (Input.GetKeyDown(HitKey))
        {
            GameObject selectedobj = EventSystem.current.currentSelectedGameObject.gameObject;
            if (selectedobj.GetComponent<Button>())
            {
                selectedobj.GetComponent<Button>().onClick.Invoke();
            }
          
        }
    }



    public void openAppInPlaystore()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            Application.OpenURL("market://details?q=pname:com.Beeah.uwm/");
        }
        else
        {
            Application.OpenURL("itms-apps://itunes.apple.com/app/com.Beeah.uwm");
        }
      
    }
}
