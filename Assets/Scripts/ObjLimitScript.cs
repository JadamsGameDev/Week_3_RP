using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script grabs the Object limit variable from the RigidbodyFirstPersonController.cs script

public class ObjLimitScript : MonoBehaviour
{
    public Text ObjLimitText;
    public UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController RBController;
    // Start is called before the first frame update
    void Start()
    {
        RBController = GameObject.Find("Player").GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        ObjLimitText.text = "Object values: " + RBController.ObjLimit + "/5";
    }
}
