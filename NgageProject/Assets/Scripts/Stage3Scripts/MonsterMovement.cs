using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public float moveSpeed = 3.9f;
    public Nodes StartingPosition;
    public int ScatterModeTimer1 = 7;
    public int ChaseModeTimer1 = 20;
    public int ScatterModeTimer2 = 7;
    public int ChaseModeTimer2 = 20;
    public int ScatterModeTimer3 = 5;
    public int ChaseModeTimer3 = 20;
    public int ScatterModeTimer4 = 5;
    public int Ghost2relaseTime = 5;
    private float GhostReleaseTimer;
    public bool isGhost2Inside = false;
    private int modechangeinteration = 1;
    private float ModeChangetimer = 0;
    public bool StartMoving;
    public GameObject WrongAns;
    public GameBoard Gamemanager;
    private AudioSource MosterSound;
    public enum Mode
    {
        Chase,
        Scatter,
        Frightened
    }

    public enum GhostType
    {
        Red,
        Pink,
        Blue,
        Orange
    }

    public GhostType ghostType = GhostType.Red;

    Mode currentmode = Mode.Scatter;
    Mode previousmode;
    private GameObject truck;

    public Nodes currentnode, targetNode, previoudNode;
    private Vector2 direction, NextDirection;
    [HideInInspector]
    public Vector2 MonsterInitialPos;
    private bool GameStarted = false;
    private AudioSource Soundeffect;
    public AudioClip Monstercollision;


    void Start()
    {
        if (!GameStarted)
        {
            GameStarted = true;
            MosterSound = this.gameObject.GetComponent<AudioSource>();
            MonsterInitialPos = this.gameObject.transform.position;
            InitialSetup();
            Debug.Log(" started ");
        }
  
    }


    public void InitialSetup()
    {

        GameObject[] GameTrucks = GameObject.FindGameObjectsWithTag("Truck");
        for(int a = 0; a < GameTrucks.Length; a++)
        {
            if (GameTrucks[a].activeInHierarchy)
            {
                truck = GameTrucks[a];
            }
        }
        Nodes node = GetNodePosition(transform.localPosition);
        if (node != null)
        {
            currentnode = node;
        }

        if (isGhost2Inside)
        {
            direction = Vector2.down;
            targetNode = currentnode.neighbours[0];
        }

        else
        {
            direction = Vector2.left;
            targetNode = ChooseNextNode();
        }

        previoudNode = currentnode;
        StartMoving = true;
        Soundeffect = this.gameObject.GetComponent<AudioSource>();
    }

     void OnEnable()
    {
        //if (GameStarted)
        //{
        //    MonsterInitialPos = this.gameObject.transform.position;
        //    InitialSetup();
        //    Debug.Log(" started onenable");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (StartMoving)
        {
            ReleaseGhost();
            ModeUpdate();
            Move();
        }
     
    }

    void Move()
    {
       
        if (targetNode != currentnode && targetNode != null && !isGhost2Inside)
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
        else
        {

        }
       
    }

    void ModeUpdate()
    {   
        if(currentmode != Mode.Frightened)
        {
            ModeChangetimer += Time.deltaTime;
            if(modechangeinteration == 1)
            {
                if(currentmode == Mode.Scatter && ModeChangetimer > ScatterModeTimer1)
                {
                    ChangeMode(Mode.Chase);
                    ModeChangetimer = 0;
                }
                if(currentmode == Mode.Chase && ModeChangetimer > ChaseModeTimer1)
                {
                    modechangeinteration = 2;
                    ChangeMode(Mode.Scatter);
                    ModeChangetimer = 0;
                }
            }
            else if (modechangeinteration == 2)
            {
                if (currentmode == Mode.Scatter && ModeChangetimer > ScatterModeTimer2)
                {
                    ChangeMode(Mode.Chase);
                    ModeChangetimer = 0;
                }
                if (currentmode == Mode.Chase && ModeChangetimer > ChaseModeTimer2)
                {
                    modechangeinteration = 3;
                    ChangeMode(Mode.Scatter);
                    ModeChangetimer = 0;
                }
            }
            else if (modechangeinteration == 3)
            {
                if (currentmode == Mode.Scatter && ModeChangetimer > ScatterModeTimer3)
                {
                    ChangeMode(Mode.Chase);
                    ModeChangetimer = 0;
                }
                if (currentmode == Mode.Chase && ModeChangetimer > ChaseModeTimer3)
                {
                    modechangeinteration = 4;
                    ChangeMode(Mode.Scatter);
                    ModeChangetimer = 0;
                }
            }
            else if (modechangeinteration == 4)
            {
                if (currentmode == Mode.Scatter && ModeChangetimer > ScatterModeTimer4)
                {
                    ChangeMode(Mode.Chase);
                    ModeChangetimer = 0;
                }
               
            }

        }else if(currentmode == Mode.Frightened)
        {

        }
    }

    void ChangeMode(Mode m)
    {
        currentmode = m;
    }

    void ReleasePinkGhost()
    {
        if(ghostType == GhostType.Pink && isGhost2Inside)
        {
            isGhost2Inside = false;
        }
    }

    void ReleaseGhost()
    {
        GhostReleaseTimer += Time.deltaTime;
        if(GhostReleaseTimer > Ghost2relaseTime)
        {
            ReleasePinkGhost();
        }
    }

    Vector2 GetRedGhostTargetTile()
    {
        Vector2 Truckpos = truck.transform.localPosition;
        Vector2 targettile = new Vector2(Mathf.RoundToInt(Truckpos.x), Mathf.RoundToInt(Truckpos.y));
        return targettile;

    }
    Vector2 GetPinkGhostTargetTile()    
    {
        Vector2 Truckpos = truck.transform.localPosition;
        Vector2 TruckOrientation = truck.GetComponent<TruckPlayer>().orientation;
        int TruckPosX = Mathf.RoundToInt(TruckOrientation.x);
        int TruckPosY = Mathf.RoundToInt(TruckOrientation.y);
        Vector2 TruckTile = new Vector2(TruckPosX, TruckPosY);
        Vector2 targetTile = TruckTile + (4 * TruckOrientation);
        return targetTile;
    }

    Vector2 getTargetTile()
    {
        Vector2 targetTile = Vector2.zero;
        if(ghostType == GhostType.Red)
        {
            targetTile = GetRedGhostTargetTile();
        }
        else if(ghostType == GhostType.Pink)
        {
            targetTile = GetPinkGhostTargetTile();
        }
        return targetTile;
    }



    //GET NODE METHOD
    Nodes ChooseNextNode()
    {
        Vector2 targetTile = Vector2.zero;
        targetTile = getTargetTile();
        Nodes moveToNode = null;
        Nodes[] FoundNodes = new Nodes[4];
        Vector2[] FoundNodedirection = new Vector2[4];
        int nodeCounter = 0;
        if(currentnode != null)
        {

        }
        else
        {
            return moveToNode;
        }
       
        for(int a = 0; a < currentnode.neighbours.Length; a++)
        {

            if (currentnode.validDirections[a] != null)
            {
                FoundNodes[nodeCounter] = currentnode.neighbours[a];
                FoundNodedirection[nodeCounter] = currentnode.validDirections[a];
                nodeCounter++;
            }
        }

        if(FoundNodes.Length == 1)
        {
            moveToNode = FoundNodes[0];
            direction = FoundNodedirection[0];    

        }

        if(FoundNodes.Length > 1)
        {
            float LeastDistance = 10000f;
            for(int a = 0; a < FoundNodes.Length; a++)
            {
                if(FoundNodedirection[a] != Vector2.zero)
                {
                    float distance = GetDistance(FoundNodes[a].transform.position, targetTile);
                    if(distance < LeastDistance)
                    {
                        LeastDistance = distance;
                        moveToNode = FoundNodes[a];
                        direction = FoundNodedirection[a];
                    }
                }
            }
        }

      
        return moveToNode;
     }

    // GET NODE POSITION 
    Nodes GetNodePosition(Vector2 pos)
    {
        GameObject tile = GameObject.Find("Gamemanager").GetComponent<GameBoard>().board[(int)pos.x, (int)pos.y];
        if(tile != null)
        {
            if(tile.GetComponent<Nodes>() != null)
            {
                return tile.GetComponent<Nodes>();
            }
        }
        return null;
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

    float GetDistance(Vector2 PosA,Vector2 PosB)
    {
        float dx = PosA.x - PosB.x;
        float dy = PosA.y - PosB.y;
        float distnace = Mathf.Sqrt(dx * dx + dy * dy);
        return distnace;

    }


    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Truck")
        {
            string CenterName = "";
            string truckname = other.gameObject.name;
            other.gameObject.SetActive(false);
            Gamemanager.VibrateDevice();
            int attackscore = Gamemanager.monsterAttackScore;
            WrongAns.transform.position = this.transform.position;
            StartCoroutine(AnsStatus(WrongAns, attackscore, truckname, CenterName));
        }
     
    }


    IEnumerator AnsStatus(GameObject AnsEffect, int Score, string TruckName,string CenterName)
    {
        AnsEffect.SetActive(true);
        Soundeffect.clip = Monstercollision;
        Soundeffect.Play();
        Gamemanager.TruckCenterResult(Score, TruckName, CenterName);
        yield return new WaitForSeconds(1f);
    }
}
