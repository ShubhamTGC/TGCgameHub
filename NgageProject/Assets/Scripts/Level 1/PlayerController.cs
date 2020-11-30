using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[SerializeField]
public class PlayerController : MonoBehaviour
{
    public static PlayerController instant;
    private SpawnManager spawnManager;
    //private Spawn2Manager spawn2Manager;

    public Joystick joystick;

    public int speed = 5;
    bool isGrounded = true; //to check if the player is grounded

    float hMovement = 0f;

    public Animator animator;

    private bool canMove;

    public int currentScore = 0; 
    public Text scoreText;

    public int lives;
    public List<Image> heartList;

    public GameObject popupText;
    public GameObject minusText;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public List<GameObject> bulletPoolList;

    private AudioSource audioSource;

    public bool isGamePaused;

    public int completeScore;
    public int correct_Point;
    public int wrong_Point;
    public int enemiesKilled;

    private int sceneNo;

    public TextMeshProUGUI[] gameOverCount;

    private void OnEnable()
    {
        Time.timeScale = 1;
        spawnManager = FindObjectOfType<SpawnManager>();
        currentScore = 0;
        audioSource = this.GetComponent<AudioSource>();
        canMove = true;
        lives = 3;
        hMovement = 0;
        popupText.gameObject.SetActive(false);
        isGamePaused = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //spawnManager = FindObjectOfType<SpawnManager>();
        audioSource = this.GetComponent<AudioSource>();
        canMove = true;
        lives = 3;
        instant = this;
        hMovement = 0;
        popupText.gameObject.SetActive(false);
        isGamePaused = false;

        for (int i = 0; i < 10; i++)
        {
            GameObject laser = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            //laser.transform.SetParent(this.gameObject.transform);
            laser.SetActive(false);
            bulletPoolList.Add(laser);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //hMovement = Input.GetAxis("Horizontal") * speed; //getting the horizontal input

        if(joystick.Horizontal >= 0.2f)
        {
            hMovement = speed;
        }
        else if(joystick.Horizontal <= -0.2f)
        {
            hMovement = -speed;
        }
        else
        {
            hMovement = 0;
        }

        if (isGrounded && !isGamePaused) 
        {
            animator.SetFloat("move", Mathf.Abs(hMovement));
            animator.SetBool("isJumping", false);
        }

        if(joystick.Vertical > 0.5f)
        {
            jump();
        }

        /*if (Input.GetKey(KeyCode.UpArrow) && isGrounded == true)
        {
            isGrounded = false;
            animator.SetBool("isJumping", true);
            jump();
        }*/

        scoreText.text = currentScore.ToString();
    }

    private void FixedUpdate()
    {
        if (!isGamePaused)
        {
            movePlayer();
        }
    }

    public void jump()
    {
        if (isGrounded)
        {
            isGrounded = false;
            this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 20), ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
            //jump();
        }
    }

    public void fireBullet()
    {
        if (!isGamePaused)
        {
            GameObject bulletP = getPooledBullet();
            bulletP.transform.position = bulletSpawn.transform.position;
            bulletP.transform.rotation = bulletSpawn.transform.rotation;
            bulletP.SetActive(true);
            audioSource.PlayOneShot(audioSource.clip, 1);
        } 
    }

    public GameObject getPooledBullet()
    {
        for (int i = 0; i < bulletPoolList.Count; i++)
        {
            if (!bulletPoolList[i].activeInHierarchy)
            {
                return bulletPoolList[i];
            }
        }
        return null;
    }


    private void OnCollisionEnter2D(Collision2D collision) //to check if player is colliding with anything
    {
        if (collision.gameObject.CompareTag("ground")) //if player touches the ground, he is grounded
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }

        else if (collision.gameObject.CompareTag("platform")) 
        {
            animator.SetFloat("move", 0);
        }

        else if (collision.gameObject.CompareTag("enemy"))
        {
            if (currentScore >= 10)
            {
                currentScore -= 10;
            } 
            if (lives == 1)
            {
                lives = 0;
                //gameOverCount[1].text = currentScore.ToString();
                spawnManager.gameOver(false);
                //spawnManager.gameOver(false); //game will over if player run out of lives
                return;
            }
            
            lives--;
            heartList[lives].gameObject.SetActive(false);
            StartCoroutine(colEnemy());


            StopCoroutine("popUp");
            StopCoroutine("enemyHit");
            minusText.GetComponent<Text>().text = "+" + wrong_Point;
            popupText.gameObject.SetActive(false);
            minusText.gameObject.SetActive(false);
            StartCoroutine("enemyHit");


            //if player touches the enemy, he will bounce back

            if (this.transform.position.x < collision.transform.position.x && isGrounded)
            {
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-10, 10), ForceMode2D.Impulse);
            }
            else if (this.transform.position.x > collision.transform.position.x && isGrounded)
            {
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(10, 10), ForceMode2D.Impulse);
            }
            else 
            {
                this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(10, 10), ForceMode2D.Impulse);
            }
        }
        else
        {
            isGrounded = false;
        }
    }

    private IEnumerator enemyHit()
    {
        minusText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        minusText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("coin")) //if player collects coins, the score will increase
        {
            currentScore += correct_Point;
            collision.gameObject.SetActive(false);
            if(currentScore >= completeScore)
            {
                //gameOverCount[0].text = currentScore.ToString();
                spawnManager.gameOver(true);
            }


            StopCoroutine("popUp");
            StopCoroutine("enemyHit");
            popupText.GetComponent<Text>().text = "+" + correct_Point;
            popupText.gameObject.SetActive(false);
            minusText.gameObject.SetActive(false);
            StartCoroutine("popUp");
        }
    }

    private IEnumerator popUp()
    {
        popupText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        popupText.gameObject.SetActive(false);
    }

    private void movePlayer()
    {
        if (canMove)
        {
            if (hMovement < 0) //if the player is moving in left direction, we will rotate the player
            {
                if (this.transform.localEulerAngles.y != 180)
                {
                    this.transform.localEulerAngles = new Vector3(0, 180, 0);
                }
            }
            else
            {
                this.transform.localEulerAngles = new Vector3(0, 0, 0);
            }

            this.transform.position += new Vector3(hMovement * Time.deltaTime * speed, 0, 0); //moving the player
        }
    }

    private IEnumerator colEnemy()
    {
        canMove = false;
        yield return new WaitForSeconds(1f);
        canMove = true;
    }

}
