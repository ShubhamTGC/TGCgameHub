using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileDragging : MonoBehaviour
{

    public float maxStretch = 3.0f;
    public LineRenderer catapultLineFront;
    public LineRenderer catapultLineBack;
    private SpringJoint2D spring;
    private Transform catapult;
    private Ray rayToMouse;
    private Ray leftCatapultToProjectile;
    private float maxStretchSqr;
    private float circleRadius;
    private bool clickedOn;
    private Vector2 prevVelocity;
    private Rigidbody2D rigidbody2d;
    private CircleCollider2D circle;
    private ThrowWasteHandler throwwaste;
    [HideInInspector]
    public Vector2 catapultToMouse;
    public GameObject ONstrike;
    private bool collide = false;
    public int DotsNumber;
    public GameObject dots;
    public Transform ProjectionParent;
    private GameObject[] projectionDots;
    private Vector3 Endpos, startPos;
    private Vector3 forceAtplayer;
    private Vector3 mouseWorldPoint;
    [SerializeField] private float Forcefactor;
    private Vector3 lastForceAtPlayer;
    [SerializeField] private float Timedata;
    private Vector3 temppos;
    void Awake()
    {
       
    }

    void Start()
    {
       
    }

    private void OnEnable()
    {
        this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        collide = false;
        throwwaste = FindObjectOfType<ThrowWasteHandler>();
        StartCoroutine(initailsetup());
        projectionDots = new GameObject[DotsNumber];



    }

    IEnumerator initailsetup()
    {
      
        yield return new WaitForSeconds(0.2f);
        catapultLineFront.enabled = true;
        catapultLineBack.enabled = true;
        spring = GetComponent<SpringJoint2D>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        circle = GetComponent<CircleCollider2D>();
        catapult = spring.connectedBody.transform;

        rayToMouse = new Ray(catapult.position, Vector3.zero);
        leftCatapultToProjectile = new Ray(catapultLineFront.transform.position, Vector3.zero);
        maxStretchSqr = maxStretch * maxStretch;
        circleRadius = circle.radius;
        LineRendererSetup();
        this.gameObject.GetComponent<CircleCollider2D>().enabled = true;

    }
    void Update()
    {
        if (clickedOn)
            Dragging();

        if (spring != null)
        {
            if (!clickedOn)
                prevVelocity = rigidbody2d.velocity;

            LineRendererUpdate();
        }
        else
        {
            catapultLineFront.enabled = false;
            catapultLineBack.enabled = false;
        }


    }

    void LineRendererSetup()
    {
        catapultLineFront.SetPosition(0, catapultLineFront.transform.position);
        catapultLineBack.SetPosition(0, catapultLineBack.transform.position);

        //catapultLineFront.sortingLayerName = "Foreground";
       // catapultLineBack.sortingLayerName = "Foreground";
        catapultLineFront.sortingLayerName = "Default";
       catapultLineBack.sortingLayerName = "Default";

        catapultLineFront.sortingOrder = 3;
        catapultLineBack.sortingOrder = 1;
    }

    void OnMouseDown()
    {

        for (int a = 0; a < DotsNumber; a++)
        {
            projectionDots[a] = Instantiate(dots, this.gameObject.transform);
        }
        clickedOn = true;
        throwwaste.gameSound.clip = throwwaste.Stretching;
        throwwaste.gameSound.Play();
        spring.enabled = false;
        startPos = gameObject.transform.position;
        
    }

    void OnMouseUp()
    {
        for (int a = 0; a < DotsNumber; a++)
        {
            Destroy(projectionDots[a]);
        }
        clickedOn = false;
        Destroy(spring);
        rigidbody2d.gravityScale = 1f;
        throwwaste.gameSound.clip = throwwaste.Shoot;
        throwwaste.gameSound.Play();
        rigidbody2d.velocity = new Vector2(-forceAtplayer.x * Forcefactor, -forceAtplayer.y * Forcefactor);
       
    }

    void Dragging()
    {
       
        mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        catapultToMouse = mouseWorldPoint - catapult.position;
        Endpos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0f, 0f, 10f);

        if (catapultToMouse.sqrMagnitude > maxStretchSqr)
        {
            rayToMouse.direction = catapultToMouse;
            mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
          
        }
        
        if(Mathf.Sign(forceAtplayer.x) == 1) 
        {
            for (int a = 0; a < DotsNumber; a++)
            {
                projectionDots[a].SetActive(false);
            }
        }
        else
        {
            for (int a = 0; a < DotsNumber; a++)
            {
                projectionDots[a].SetActive(true);
            }
        }

        for (int a = 0; a < DotsNumber; a++)
        {
            projectionDots[a].transform.position = ProjectCalculate(a * 0.1f);
        }
        forceAtplayer = mouseWorldPoint - startPos;
        mouseWorldPoint.z = 0f;
        transform.position = mouseWorldPoint;
    }

    void LineRendererUpdate()
    {
       
        Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
        leftCatapultToProjectile.direction = catapultToProjectile;
        Vector3 holdPoint = leftCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + circleRadius);
        catapultLineFront.SetPosition(1, holdPoint);
        catapultLineBack.SetPosition(1, holdPoint);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (!collide)
        {
            collide = true;
            string objectname = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.name;
            //Debug.Log("collision name " + collision.gameObject.name);
            if(collision.gameObject.tag != "leaderboard")
            {
                if (collision.gameObject.tag == "Groud")
                {
                    ONstrike.transform.position = this.gameObject.transform.position;
                    string collidername = collision.collider.gameObject.name;
                    throwwaste.checkCollidedAns(collidername, objectname, collision.gameObject);
                    StartCoroutine(CollidedTask(collision.gameObject));
                }
                else
                {
                    string collidername = "wall";
                    throwwaste.checkCollidedAns(collidername, objectname, collision.gameObject);
                    collide = false;
                    this.gameObject.SetActive(false);

                }
            }
           
        }
     
    }

    IEnumerator CollidedTask(GameObject dustbin)
    {
        dustbin.transform.GetChild(0).gameObject.SetActive(false);
        dustbin.transform.GetChild(1).gameObject.SetActive(true);
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        ONstrike.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        dustbin.transform.GetChild(0).gameObject.SetActive(true);
        dustbin.transform.GetChild(1).gameObject.SetActive(false);
        ONstrike.SetActive(false);
        collide = false;
        this.gameObject.SetActive(false);
    }

    private Vector2 ProjectCalculate(float time)
    {
        return new Vector2(mouseWorldPoint.x, mouseWorldPoint.y) +
            new Vector2(-forceAtplayer.x * Forcefactor, -forceAtplayer.y * Forcefactor) * time + 0.5f * Physics2D.gravity * time * time;
    }
  
}