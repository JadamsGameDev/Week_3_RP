using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinUIState : UIState
{
    //public Text WinText;
    //public float Timer = 0.0f;
    void Start()
    {
    }

    private void Update()
    {
        //WinText.text = "You Win! \n Press enter to restart";// \n Time: " + Timer; ;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    //public void RecordTime()
    //{
    //    Timer = Time.deltaTime;
    //}
}
