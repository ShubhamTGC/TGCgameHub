using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TruckPlayer : MonoBehaviour
{
    private Vector3 UpDirection = new Vector3(0,0,90);
    private Vector3 RightDirection = new Vector3(0,0,0);
    private Vector3 LeftDirection = new Vector3(0,0,180);
    private Vector3 DownDirection = new Vector3(0,0,-90);
    private Vector2 direction = Vector2.zero;
    public float speed =40f;
    public Vector2 orientation = Vector2.zero;
    public Nodes currentNode,previousNode,targetNode;
    public Vector2 NextDirection;

    private Vector2 startTouchPos, endtouchPosition;
    public float BufferValue = 100;
    public bool StartMoving;
    void Start()
    {

    }

    IEnumerator ShowPresence()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(0.2f);
        this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }

    private void OnEnable()
    {

        Nodes node = getNodeposition(transform.localPosition);
        if (node != null)
        {
            currentNode = node;
        }
        direction = Vector2.right;
        orientation = Vector2.right;
        ChangePosition(direction);
        StartCoroutine(ShowPresence());
        StartMoving = false;
    }



    public void ResetNodes()
    {
        currentNode = null;
        targetNode = null;
        previousNode = null;
        StartMoving = true;
    }


    void Update()
    {
        if (!StartMoving)
        {
            Move();
            updateOrientation();
            ReadInput();
           
           
        }
    
    }

    
    void ReadInput()
    {
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangePosition(Vector2.right);

        }else if (Input.GetKeyDown(KeyCode.LeftArrow)) 
        {
            ChangePosition(Vector2.left);
        }else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangePosition(Vector2.up);
        }else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangePosition(Vector2.down);
        }

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPos = Input.GetTouch(0).position;
        }
        if(Input.touchCount > 0 && Input.GetTouch(0).phase== TouchPhase.Ended)
        {
            endtouchPosition = Input.GetTouch(0).position;
            if (endtouchPosition.x < startTouchPos.x  && endtouchPosition.y < startTouchPos.y + BufferValue && endtouchPosition.y > startTouchPos.y - BufferValue)
            {
                ChangePosition(Vector2.left);
            }
            if(endtouchPosition.x > startTouchPos.x && endtouchPosition.y < startTouchPos.y + BufferValue && endtouchPosition.y > startTouchPos.y - BufferValue)
            {
                ChangePosition(Vector2.right);
            }
            if(endtouchPosition.y < startTouchPos.y && endtouchPosition.x < startTouchPos.x + BufferValue && endtouchPosition.x > startTouchPos.x - BufferValue)
            {
                ChangePosition(Vector2.down);
            }
            if(endtouchPosition.y > startTouchPos.y && endtouchPosition.x < startTouchPos.x + BufferValue && endtouchPosition.x > startTouchPos.x - BufferValue)
            {
                ChangePosition(Vector2.up);
            }
        }


    }

    void updateOrientation()
    {
        if(direction == Vector2.up)
        {
            orientation = Vector2.up;
            this.transform.eulerAngles = UpDirection;
        }else if(direction == Vector2.down)
        {
            orientation = Vector2.down;
            this.transform.eulerAngles = DownDirection;
        }
        else if (direction == Vector2.left)
        {
            orientation = Vector2.left;
            this.transform.eulerAngles = LeftDirection;
        }
        else if (direction == Vector2.right)
        {
            orientation = Vector2.right;
            this.transform.eulerAngles = RightDirection;
        }
    }

    void Move()
    {
        if(targetNode != currentNode && targetNode != null)
        {

            if(NextDirection == direction * -1)
            {
                direction *= -1;
                Nodes tempNode = targetNode;
                targetNode = previousNode;
                previousNode = tempNode;
            }
            if (OvershootTarget())
            {
                currentNode = targetNode;
                transform.localPosition = currentNode.transform.position;
                Nodes movetoNode = canMove(NextDirection);
                if(movetoNode != null)
                    direction = NextDirection;
                
                if(movetoNode == null)
                    movetoNode = canMove(direction);
                
                if(movetoNode != null)
                {
                    targetNode = movetoNode;
                    previousNode = currentNode;
                    currentNode = null;
                }
                else
                {
                    direction = Vector2.zero;
                }
            }
            else
            {
                transform.position += new Vector3(direction.x, direction.y, 0) * Time.deltaTime * speed;
            }
        }
       
    }

    Nodes getNodeposition(Vector2 pos)
    {
        GameObject tile = GameObject.Find("Gamemanager").GetComponent<GameBoard>().board[(int)pos.x, (int)pos.y];
        if(tile != null)
        {
            return tile.GetComponent<Nodes>();
        }
        return null;
    }

    Nodes canMove(Vector2 d)
    {
        Nodes moveToNodes = null;
        for(int a = 0; a < currentNode.neighbours.Length; a++)
        {
            if(currentNode.validDirections[a] == d)
            {
                moveToNodes = currentNode.neighbours[a];
                break;
            }
        }
        return moveToNodes;
    }

    void MoveToNode(Vector2 d)
    {
        Nodes Movetonode = canMove(d);
        if(Movetonode != null)
        {
            transform.localPosition = Movetonode.transform.position;
            currentNode = Movetonode;
        }
    }


    void ChangePosition(Vector2 dir)
    {
        if(dir != direction)
            NextDirection = dir;

        if(currentNode != null)
        {
            Nodes movetonode = canMove(dir);
            if(movetonode != null)
            {
                direction = dir;
                targetNode = movetonode;
                previousNode = currentNode;
                currentNode = null;

            }
        }
        
    }


    bool OvershootTarget()
    {
        float nodetoTarget = lengthFromnode(targetNode.transform.position);
        float nodeToself = lengthFromnode(transform.localPosition);
        return nodeToself > nodetoTarget;
    }

    float lengthFromnode(Vector2 targetpos)
    {
        Vector2 vec = targetpos - (Vector2)previousNode.transform.position;
        return vec.sqrMagnitude;
    }

}
