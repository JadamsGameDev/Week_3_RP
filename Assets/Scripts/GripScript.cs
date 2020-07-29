using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GripScript : MonoBehaviour
{
    //private BoxCollider m_collider = GetComponent<BoxCollider>();

    // Start is called before the first frame update
    void Start()
    {
        //m_collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<RigidbodyFirstPersonController>().setHang(true);
            float halfHeight = (other.GetComponent<CapsuleCollider>().height / 2f);
            other.transform.position = new Vector3(transform.position.x, (transform.position.y + halfHeight), transform.position.z);
            //other.attachedRigidbody.useGravity = false;
            other.attachedRigidbody.velocity = new Vector3(0f, 0f, 0f);
            other.attachedRigidbody.isKinematic = true;
            GetComponent<BoxCollider>().isTrigger = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GetComponent<BoxCollider>().isTrigger = true;
            //other.attachedRigidbody.useGravity = true;
            other.GetComponent<RigidbodyFirstPersonController>().setHang(false);
            other.attachedRigidbody.isKinematic = false;
        }
    }
}

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.tag == "Player")  //(collision.collider.tag == "Player")
//        {
//            if (collision.collider.gameObject.GetComponent<RigidbodyFirstPersonController>().Jumping == false)
//            {
//                collision.collider.gameObject.GetComponent<RigidbodyFirstPersonController>().setHang(true);
//            }
//            else
//            {
//                collision.collider.gameObject.GetComponent<RigidbodyFirstPersonController>().setHang(false);
//            }
//        }
//    }

//    private void OnCollisionStay(Collision collision)
//    {
//        if (collision.gameObject.tag == "Player") //(collision.collider.tag == "Player")
//        {
//            if (collision.collider.gameObject.GetComponent<RigidbodyFirstPersonController>().Jumping == false)
//            {
//                collision.collider.gameObject.GetComponent<RigidbodyFirstPersonController>().setHang(true);
//            }
//            else
//            {
//                collision.collider.gameObject.GetComponent<RigidbodyFirstPersonController>().setHang(false);
//            }
//        }
//    }
//}
