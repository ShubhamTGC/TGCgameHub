using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class AngryBirdGameDemo : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rigidbd;
    private Vector2 startPos, Endpos;
    private Vector2 forceAtplayer;
    [SerializeField] private float Forcefactor;
    public GameObject Dots;
    private GameObject[] projectionDots;
    public int dorcount;
    void Start()
    {
        rigidbd = this.GetComponent<Rigidbody2D>();
        projectionDots = new GameObject[dorcount];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = gameObject.transform.position;
            for(int a = 0; a < dorcount; a++)
            {
                projectionDots[a] = Instantiate(Dots, this.gameObject.transform);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Endpos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0f, 0f, 10f);
            for(int a = 0; a < dorcount; a++)
            {
                projectionDots[a].transform.position = ProjectCalculate(a * 0.1f);
            }
            gameObject.transform.position = Endpos;
            forceAtplayer = Endpos - startPos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            rigidbd.gravityScale = 1f;
            rigidbd.velocity = new Vector2(-forceAtplayer.x * Forcefactor, -forceAtplayer.y * Forcefactor);
            for(int a = 0; a < dorcount; a++)
            {
                Destroy(projectionDots[a]);
            }
        }

    }

    private Vector2 ProjectCalculate(float time)
    {
        return new Vector2(Endpos.x, Endpos.y) +
            new Vector2(-forceAtplayer.x * Forcefactor, -forceAtplayer.y * Forcefactor) * time + 0.5f * Physics2D.gravity * time * time;
    }
}
