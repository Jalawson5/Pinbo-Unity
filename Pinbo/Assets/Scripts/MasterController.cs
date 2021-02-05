using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterController : MonoBehaviour
{
    public static MasterController instance;
    
    ///////////////////////////////////////////////
    //Scenes:                                    //
    //0-9 - Menus, credits, etc                  //
    //1X - World 1, 11 = 1-1, 12 = 1-2, and so on//
    ///////////////////////////////////////////////
    public short scene;
    

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        scene = 0; //scene 0 is main menu//
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void StartGame()
    {
        StartCoroutine(StartRoutine());
    }
    
    private IEnumerator StartRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Stage1_1", LoadSceneMode.Single);
    }
    
    public void StageComplete()
    {
        StartCoroutine(StageRoutine());
    }
    
    private IEnumerator StageRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
