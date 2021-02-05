using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperBehavior : MonoBehaviour
{
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter2D(Collision2D coll)
    {
        anim.ResetTrigger("FlipTrigger");
        if(coll.gameObject.tag.Equals("Player"))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0.5f, 1f, 0f), Vector2.up, 0.2f);
            
            if(hit.collider != null && hit.collider.tag.Equals("Player"))
            {
                coll.gameObject.SendMessage("Flip");
                anim.SetTrigger("FlipTrigger");
            }
        }
    }
}
