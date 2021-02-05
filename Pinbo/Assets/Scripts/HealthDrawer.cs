using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrawer : MonoBehaviour
{
    public GameObject healthImg;
    
    private int maxHealth;
    private GameObject[] meter;
    
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = PinboController.MaxHealth;
        meter = new GameObject[maxHealth];
        InitHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void InitHealth()
    {
        for(int i = 0; i < maxHealth; i++)
        {
            meter[i] = Instantiate(healthImg, new Vector3(0f + (i * 30), 0f, 0f), Quaternion.identity);
            meter[i].transform.SetParent(transform, false);
        }
    }
    
    private void DecHealth(int amount)
    {
        for(int i = maxHealth - 1; i > maxHealth - 1 - amount; i--)
        {
            if(i < 0)
                break;
                
            if(meter[i] == null)
            {
                amount++;
                continue;
            }
            
            Destroy(meter[i]);
        }
    }
    
    private void IncHealth(int amount)
    {
        for(int i = 0; i < maxHealth && amount > 0; i++)
        {
            if(meter[i] == null)
            {
                amount--;
                meter[i] = Instantiate(healthImg, new Vector3(0f + (i + 30), 0f, 0f), Quaternion.identity);
            }
        }
    }
}
