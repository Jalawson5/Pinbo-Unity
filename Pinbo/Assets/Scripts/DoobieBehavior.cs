using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoobieBehavior : MonoBehaviour
{
    private int dir;
    private bool killed;
    private float bulletTime;
    private float imGonnaShoot;
    private const int Worth = 1500;
    
    public GameObject boom;
    public GameObject bullet;
    
    private SpriteRenderer sr;
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        dir = -1; //-1 = left, 1 = right//
        killed = false;
        bulletTime = 5f;
        imGonnaShoot = 0.05f;
        
        sr = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.7f * dir, -0.1f, 0), Vector2.down, 1f);
        
        if(bulletTime > 0 && !anim.GetBool("attacking"))
        {
            bulletTime -= Time.deltaTime;
            if(hit.collider != null)
            {
                transform.position += new Vector3(0.01f * dir, 0, 0);
            }

            else
            {
                dir *= -1;
                
                if(dir == 1)
                    sr.flipX = true;
                
                else
                    sr.flipX = false;
            }
        }
        
        else
        {
            if(!anim.GetBool("shotReady"))
                anim.SetBool("shotReady", true);
                
            imGonnaShoot -= Time.deltaTime;
            
            if(imGonnaShoot <= 0)
            {
                GameObject proj = Instantiate(bullet, transform.position, Quaternion.identity);
                proj.GetComponent<EnemyProjBehavior>().dir = this.dir;
                
                bulletTime = 5f;
                imGonnaShoot = 0.035f;
            }
        }
    }
    
    void Hit()
    {
        killed = true;
        Destroy(this.gameObject);
    }
    
    void OnDestroy()
    {
        if(killed)
        {
            Instantiate(boom, transform.position, Quaternion.identity);
            PinboController.instance.ScorePoints(Worth);
        }
    }
}
