using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBumperBehavior : MonoBehaviour
{
    private int dir;
    private bool killed;
    private const int Worth = 1000;
    
    public GameObject boom;
    
    // Start is called before the first frame update
    void Start()
    {
        dir = -1; //-1 = left, 1 = right//
        killed = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitGround = Physics2D.Raycast(transform.position + new Vector3(1f * dir, -0.1f, 0), Vector2.down, 1f);
        
        if(hitGround.collider != null)
        {
            transform.position += new Vector3(0.02f * dir, 0, 0);
        }
        
        else
        {
            dir *= -1;
        }
        
        RaycastHit2D hitFront = Physics2D.Raycast(transform.position + new Vector3(1f * dir, 0, 0), Vector2.up, 0.2f);
        
        if(hitFront.collider != null && hitFront.collider.tag.Equals("Solid"))
        {
            dir *= -1;
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
