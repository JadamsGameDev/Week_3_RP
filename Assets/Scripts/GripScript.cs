using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GripScript : MonoBehaviour
{
    public GameObject m_JumpPadPrefab;
    private BoxCollider m_collider;

    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.tag == "Player")
        {
            other.GetComponent<RigidbodyFirstPersonController>().setHang(true);
            float halfHeight = (other.GetComponent<CapsuleCollider>().height / 2f);
            other.transform.position = new Vector3(transform.position.x, (transform.position.y + halfHeight + 0.1f), transform.position.z);
            //other.attachedRigidbody.useGravity = false;
            //other.attachedRigidbody.velocity = new Vector3(0f, 0f, 0f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<RigidbodyFirstPersonController>().Jumping == false)
            {
                other.GetComponent<RigidbodyFirstPersonController>().setHang(true);
                float halfHeight = (other.GetComponent<CapsuleCollider>().height / 2f);
                other.transform.position = new Vector3(transform.position.x, (transform.position.y + halfHeight + 0.1f), transform.position.z);
            }
            else
            {
                other.GetComponent<RigidbodyFirstPersonController>().setHang(false);
                //other.GetComponent<RigidbodyFirstPersonController>().
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //other.attachedRigidbody.useGravity = true;
            other.GetComponent<RigidbodyFirstPersonController>().setHang(false);
        }
    }
}
