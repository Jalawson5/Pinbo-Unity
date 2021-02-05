using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PinboController : MonoBehaviour
{
    public static PinboController instance;
    
    public const int MaxHealth = 10;
    private const float RunSpeed = 3.5f;
    private const float RollMult = 2f;
    ///////////////////////////////////////////////
    //Animator Variables:                        //
    //0 = Standing                               //
    //1 = Running                                //
    //2 = Jumping/Rising                         //
    //3 = Falling                                //
    //4 = Landing                                //
    //5 = Punching                               //
    //                                           //
    //mirror = Is Pinbo facing left?             //
    //damaged = Is Pinbo currently taking damage?//
    ///////////////////////////////////////////////
    private int state;
    private bool mirror;
    private bool damaged;
    
    private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private BoxCollider2D bc;
    
    private bool isJumping;
    private bool isRolling;
    private float rayLength;
    
    private int health;
    private int lives;
    private int score;
    private int scoreThreshold;
    private float damageTimer;
    
    public GameObject punchBox;
    public GameObject healthMeter;
    public GameObject scoreText;
    public GameObject livesText;
    private GameObject tempBox;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        state = 0;
        mirror = false;
        damaged = false;
        
        anim = this.GetComponent<Animator>();
        sr = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        bc = this.GetComponent<BoxCollider2D>();
        
        isJumping = false;
        health = MaxHealth;
        lives = 2;
        score = 0;
        scoreThreshold = 0;
        damageTimer = 2f;
        isRolling = false;
        rayLength = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("z") && !isJumping && state != 3)
        {
            rb.AddForce(new Vector2(0, 300f));
            
            isJumping = true;
            state = 2;
        }
        
        else if(rb.velocity.y < 0 && ((state == 0 && !isRolling) || state == 1 || state == 2))
        {
            state = 3;
        }
        
        else if(rb.velocity.y == 0 && state == 3)
        {
            state = 4;
            isJumping = false;
        }
        
        if(Input.GetKey("right"))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.6f, 0, 0), Vector2.up, rayLength);
            if(hit.collider == null || !hit.collider.tag.Equals("Solid"))
            {
                if(!isRolling)
                    transform.position += new Vector3(RunSpeed * Time.deltaTime, 0, 0);
                
                else
                    transform.position += new Vector3(RunSpeed * RollMult * Time.deltaTime, 0, 0);
                
                if(state == 0 || (state == 4 && anim.GetBool("landed")) || (state == 4 && isRolling))
                    state = 1;
                    
                mirror = false;
            }
        }
        
        else if(Input.GetKey("left"))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(-0.6f, 0, 0), Vector2.up, rayLength);
            if(hit.collider == null || !hit.collider.tag.Equals("Solid"))
            {
                if(!isRolling)
                    transform.position += new Vector3(RunSpeed * -1f * Time.deltaTime, 0, 0);
                    
                else
                    transform.position += new Vector3(RunSpeed * -1f * RollMult * Time.deltaTime, 0, 0);
                
                if(state == 0 || (state == 4 && anim.GetBool("landed")) || (state == 4 && isRolling))
                    state = 1;
                    
                mirror = true;
            }
        }
        
        else if(state == 1 || (state == 4 && anim.GetBool("landed")) || (state == 4 && isRolling))
        {
            state = 0;
        }
        
        if(Input.GetKeyDown("x") && !anim.GetBool("Attacking") && state != 4 && !isRolling) //Cannot attack while landing//
        {
            //anim.SetInteger("State", 5);
            anim.SetBool("Attacking", true);
            tempBox = Instantiate(punchBox, transform.position, Quaternion.identity);
            tempBox.transform.parent = this.transform;
        }
        
        if(Input.GetKeyDown("a") && (state == 0 || state == 1))
        {
            //isRolling = !isRolling;
            
            if(!isRolling)
            {
                isRolling = true;
                bc.size = new Vector3(0.9f, 0.9f, 0);
                rayLength = 0.5f;
            }
            
            else
            {
                RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + new Vector3(-0.6f, 0.5f, 0), Vector2.up, 0.5f);
                RaycastHit2D hitRight = Physics2D.Raycast(transform.position + new Vector3(0.6f, 0.5f, 0), Vector2.up, 0.5f);
                
                if((hitLeft.collider == null || !hitLeft.collider.tag.Equals("Solid")) && (hitRight.collider == null || !hitRight.collider.tag.Equals("Solid")))
                {
                    isRolling = false;
                    bc.size = new Vector3(1.1f, 1.1f, 0);
                    transform.position += new Vector3(0f, 0.2f, 0f);
                    rayLength = 1f;
                }
            }
            
            anim.SetBool("Rolling", isRolling);
        }
        
        if(!anim.GetBool("Attacking"))
        {
            anim.SetInteger("State", state);
            if(tempBox != null)
            {
                Destroy(tempBox.gameObject);
                tempBox = null;
            }
        }
        
        if(anim.GetBool("landed"))
            anim.SetBool("landed", false);
        
        damageTimer -= Time.deltaTime;
        if(damaged)
        {
            damaged = false;
            damageTimer = 2f;
            anim.SetBool("Damaged", true);
            
            int dir = -1;
            
            if(mirror)
                dir *= -1;
                
            float yForce = 0;
            if(isJumping)
                yForce = 100f;
                
            rb.AddForce(new Vector3(100f*dir, yForce, 0));
            health -= 1;
        }
        
        sr.flipX = mirror;
        
        if(tempBox != null)
        {
            if(mirror)
                tempBox.transform.localScale = new Vector3(-1f, 1f, 0);
                
            else
                tempBox.transform.localScale = new Vector3(1f, 1f, 0);
        }
    }
    
    void OnTriggerStay2D(Collider2D collider)
    {
        if(!collider.gameObject.tag.Equals("Exit"))
            TakeDamage();
    }
    
    private void TakeDamage()
    {
        if(damageTimer <= 0)
        {
            damaged = true;
            health--;
            healthMeter.SendMessage("DecHealth", 1);
            
            if(health <= 0)
            {
                LoseLife();
            }
        }
    }
    
    private void LoseLife()
    {
        lives--;
        
        livesText.GetComponent<Text>().text = "Lives: " + lives;
        
        if(lives < 0)
        {
            lives = 0;
            //GAME OVER//
        }
    }
    
    public void ScorePoints(int amount)
    {
        score += amount;
        scoreThreshold += amount;
        if(scoreThreshold >= 100000)
        {
            scoreThreshold -= 100000;
            lives++;
        }
        
        scoreText.GetComponent<Text>().text = "Score: " + score;
    }
    
    private void Flip()
    {
        rb.AddForce(new Vector2(0, 500f));
        isJumping = true;
        state = 2;
    }
    
    private void StageComplete()
    {
        MasterController.instance.StageComplete();
    }
}
