using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class BounceScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Rigidbody>().drag = 0f;
            //Vector3 plane = transform.up;
            //Vector3 playerVelocity = Vector3.ProjectOnPlane(other.GetComponent<Rigidbody>().velocity, plane);
            //playerVelocity.y = (playerVelocity.y + 1f) * 15f;
            //playerVelocity.z = playerVelocity.z * 5f;
            // (other.GetComponent<Rigidbody>().velocity.z * 1f)
            other.GetComponent<Rigidbody>().AddForce(new Vector3(0f, 13f, 0f), ForceMode.Impulse);
        }
    }
}
