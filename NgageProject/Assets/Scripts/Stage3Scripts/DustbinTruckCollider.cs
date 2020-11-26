using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustbinTruckCollider : MonoBehaviour
{
    public GameBoard Gamemanager;
    private bool Collided;
    void Start()
    {
        
    }
    private void OnEnable()
    {
      
       
        Collided = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Truck" && Collided)
        {
            Collided = false;
            Gamemanager.VibrateDevice();
            Gamemanager.CheckCorrectAns(this.gameObject, other.gameObject);
           
        }

    }

    IEnumerator ResetCollision()
    {
        yield return new WaitForSeconds(0.5f);
        Collided = true;
    }
}
