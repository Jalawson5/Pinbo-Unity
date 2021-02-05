using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartListener : MonoBehaviour
{
    private bool hasStarted;

    // Start is called before the first frame update
    void Start()
    {
        hasStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if((Input.GetKeyDown("z") || Input.GetKeyDown("x")) && !hasStarted)
        {
            hasStarted = true;
            MasterController.instance.StartGame();
        }
    }
}
