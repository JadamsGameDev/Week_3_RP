using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathUIScript : UIState
{
    public Text LoseText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        LoseText.text = "You Lose! \n Press enter to try again!";
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
