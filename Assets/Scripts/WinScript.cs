using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour
{
    public bool bWin = false;
    public GameObject PlayerObj;
    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Checks if the player has entered the win zone
    void OnTriggerEnter(Collider other)
    {
        if (!bWin)
        {
            if (other = PlayerObj.GetComponent<CapsuleCollider>())
            {
                bWin = true;
                GameManager.Instance.Win();
            }
        }
    }
}
