using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    public bool bDead = false;
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
        if (!bDead)
        {
            if (other = PlayerObj.GetComponent<CapsuleCollider>())
            {
                bDead = true;
                GameManager.Instance.Dead();
            }
        }
    }
}
