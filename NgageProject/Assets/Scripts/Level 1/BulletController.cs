using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    private float speed = 20f;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(destroyBullet());
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position += transform.right * Time.deltaTime * speed;
    }

    private IEnumerator destroyBullet()
    {
        yield return new WaitForSeconds(1.5f);
        this.gameObject.SetActive(false);
    }

}
