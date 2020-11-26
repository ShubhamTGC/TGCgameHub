using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfRotation : MonoBehaviour
{
   
    public float rotationvalue;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
       if(this.gameObject.name == "ZoneAboard")
        {
            if (Mathf.Sign(this.transform.rotation.z) != -1)
            {
                this.transform.Rotate(0, 0, Time.deltaTime * -rotationvalue * 2);
            }
           
        }
       if(this.gameObject.name == "ZoneBboard")
        {
            if (Mathf.Sign(this.transform.rotation.z) != 1)
            {
                this.transform.Rotate(0, 0, Time.deltaTime * -rotationvalue * 2);
            }
         
        }
    }


}
