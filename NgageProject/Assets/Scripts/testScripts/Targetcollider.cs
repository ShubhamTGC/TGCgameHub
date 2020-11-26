using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetcollider : MonoBehaviour
{
    public GameObject targetbody;
    public GameObject wrongbin;
    public bool correcttarget, wrongtarget;


    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == targetbody.name)
        {
            correcttarget = true;
            wrongtarget = false;
        }
        else
        {
            wrongbin = other.gameObject;
            wrongtarget = true;
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == targetbody.name)
        {
            correcttarget = false;
        }
      
    }
}
