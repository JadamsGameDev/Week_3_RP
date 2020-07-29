using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinUIState : UIState
{

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
