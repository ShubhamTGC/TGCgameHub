using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Dashboard_image : MonoBehaviour
{
    // Start is called before the first frame update
    public Image image_taken;
    public Text msg;
    public List<GameObject> dashboards;
    private Zonehandler zone;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void show_dashboard_image(string zone_name)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine(on_load(zone_name));
        }

    }

    public void show_image_method(GameObject zone)
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
           // StartCoroutine(show_image(zone));
        }
    }


    IEnumerator on_load(string zone_name)
    {
        zone_name = zone_name + ".png";
        yield return new WaitForSeconds(0f);
        if (File.Exists(Application.persistentDataPath + "/" + zone_name))
        {
            byte[] byteArray = File.ReadAllBytes(Application.persistentDataPath + "/" + zone_name);
            Texture2D texture = new Texture2D(8, 8);
            texture.LoadImage(byteArray);
            msg.gameObject.SetActive(false);
            image_taken.gameObject.SetActive(true);
            Sprite s = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1f);
            image_taken.sprite = s;
        }
        else
        {
            msg.gameObject.SetActive(true);
            image_taken.gameObject.SetActive(false);
        }
    }

    //IEnumerator show_image(GameObject zone)
    //{
    //    yield return new WaitForSeconds(0f);
    //    if (zone.GetComponent<Zonehandler>().Zone_sprite != null)
    //    {
    //        msg.gameObject.SetActive(false);
    //        image_taken.gameObject.SetActive(true);
    //        Sprite dashboard_sprite = zone.GetComponent<Zonehandler>().Zone_sprite;
    //        image_taken.sprite = dashboard_sprite;
    //    }
    //    else
    //    {
    //        msg.gameObject.SetActive(true);
    //        image_taken.gameObject.SetActive(false);
    //    }

    //}

    public void dashboard_enable(GameObject current_dashboard)
    {
        StartCoroutine(dashbaord(current_dashboard));

    }
    IEnumerator dashbaord(GameObject current_dashboard)
    {
        for (int a = 0; a < dashboards.Count; a++)
        {

            if (dashboards[a].name == current_dashboard.name)
            {
                msg.gameObject.SetActive(false);
                dashboards[a].SetActive(true);
                dashboards[a].GetComponent<DashBoardData>().startDashboard();

            }
            else
            {
                if (dashboards[a].activeInHierarchy)
                {
                    dashboards[a].GetComponent<DashBoardData>().reset_task();
                    yield return new WaitForSeconds(0.6f);
                    dashboards[a].SetActive(false);
                }

            }
        }
    }

}
