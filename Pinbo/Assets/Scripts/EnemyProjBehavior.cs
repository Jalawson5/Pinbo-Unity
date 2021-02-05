using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjBehavior : MonoBehaviour
{
    public int dir;
    private float lifetime;
    
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        //dir = -1; //Facing left by default//
        lifetime = 5f;
        
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(dir == -1)
            sr.flipX = false;
        else
            sr.flipX = true;
            
        transform.position += new Vector3(0.03f * dir, 0, 0);
        
        lifetime -= Time.deltaTime;
        
        if(lifetime <= 0)
            DestroySelf();
    }
    
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag.Equals("Player"))
        {
            collider.gameObject.SendMessage("TakeDamage");
        }
    }
    
    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
