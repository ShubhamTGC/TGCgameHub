using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RandomMonsterMove : MonoBehaviour
{

    private Nodes currentnode, targetNode, previoudNode;
    private Vector2 direction, NextDirection;
    public float moveSpeed;
    private float speed;
    private float ChangeSpeedTimer;
    public GameObject WrongAns;
    public GameBoard Gamemanager;
    private AudioSource Monstereffect;
    public AudioClip SOund,monsterCollision;
    private bool playsound;
    [SerializeField]
    private float SoundTime = 10f;
    void Start()
    {

        Nodes node = GetNodePosition(transform.localPosition);
        if (node != null)
        {
            currentnode = node;
            Debug.Log("current Node name " + currentnode.name);

        }
        targetNode = ChooseNextNode();
        previoudNode = currentnode;
        Monstereffect = this.gameObject.GetComponent<AudioSource>();
        playsound = true;
        Monstereffect.clip = SOund;
        StartCoroutine(MonsterSound());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    

    void Move()
    {
        if (targetNode != currentnode && targetNode != null )
        {

            if (OvershootTarget())
            {
                currentnode = targetNode;
                transform.localPosition = currentnode.transform.position;
                targetNode = ChooseNextNode();
                previoudNode = currentnode;
                currentnode = null;
            }

            else
            {
                transform.localPosition += (Vector3)direction * moveSpeed * Time.deltaTime;
            }
        }
    }

    Nodes ChooseNextNode()
    {
        Vector2 targetTile = Vector2.zero;
        Nodes moveToNode = null;
        Nodes[] FoundNodes = new Nodes[4];
        Vector2[] FoundNodedirection = new Vector2[4];
        int nodeCounter = 0;

        for (int a = 0; a < currentnode.neighbours.Length; a++)
        {

            if (currentnode.validDirections[a] != null)
            {
                FoundNodes[nodeCounter] = currentnode.neighbours[a];
                FoundNodedirection[nodeCounter] = currentnode.validDirections[a];
                nodeCounter++;
            }
        }

        if (FoundNodes.Length == 1)
        {
            moveToNode = FoundNodes[0];
            direction = FoundNodedirection[0];

        }
       // Debug.Log("coming node having nodes " + nodeCounter);
        if (FoundNodes.Length > 1)
        {
            int randomIndex = UnityEngine.Random.Range(0, nodeCounter);
           // Debug.Log( "random no " + randomIndex);
            moveToNode = FoundNodes[randomIndex];
            direction = FoundNodedirection[randomIndex];
            //Debug.Log(" git the node " + moveToNode.gameObject.name + "random no "+ randomIndex);

        }
  


        return moveToNode;
    }


    bool OvershootTarget()
    {
        float nodetoTarget = lengthFromnode(targetNode.transform.position);
        float nodeToself = lengthFromnode(transform.localPosition);

        return nodeToself > nodetoTarget;
    }

    float lengthFromnode(Vector2 targetpos)
    {
        Vector2 vec = targetpos - (Vector2)previoudNode.transform.position;
        return vec.sqrMagnitude;
    }

    float GetDistance(Vector2 PosA, Vector2 PosB)
    {
        float dx = PosA.x - PosB.x;
        float dy = PosA.y - PosB.y;
        float distnace = Mathf.Sqrt(dx * dx + dy * dy);
        return distnace;

    }

    Nodes GetNodePosition(Vector2 pos)
    {
        GameObject tile = GameObject.Find("Gamemanager").GetComponent<GameBoard>().board[(int)pos.x, (int)pos.y];
        if (tile != null)
        {
            if (tile.GetComponent<Nodes>() != null)
            {
                return tile.GetComponent<Nodes>();
            }
        }
        return null;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Truck")
        {
            string truckname = other.gameObject.name;
            other.gameObject.SetActive(false);
            WrongAns.transform.position = this.transform.position;
            string CenterName = "";
            StartCoroutine(AnsStatus(WrongAns, 0, truckname, CenterName));
        }
    }


    IEnumerator AnsStatus(GameObject AnsEffect, int Score, string TruckName, string CenterName)
    {
        Monstereffect.clip = monsterCollision;
        Monstereffect.Play();
        AnsEffect.SetActive(true);
        Gamemanager.VibrateDevice();
        Gamemanager.TruckCenterResult(Score, TruckName, CenterName);
        yield return new WaitForSeconds(1f);


    }

    IEnumerator MonsterSound()
    {
       
        if (playsound)
        {
            yield return new WaitForSeconds(SoundTime);
            Monstereffect.Play();
            StartCoroutine(MonsterSound());
        }
    }





}
