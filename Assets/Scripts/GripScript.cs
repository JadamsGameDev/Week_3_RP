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
            //float halfHeight = (other.GetComponent<CapsuleCollider>().height / 2f);
            other.transform.position = transform.GetChild(4).transform.position; //new Vector3(transform.position.x, transform.position.y, transform.position.z);
            RigidbodyFirstPersonController.boxCollider = GetComponent<BoxCollider>();

        }

    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        print("TriggerExit");

    //        GetComponent<BoxCollider>().enabled = false;
    //        other.GetComponent<RigidbodyFirstPersonController>().setHang(false);

    //        StartCoroutine(waitToTurnOnCollider());

            
    //    }
    //}

    IEnumerator waitToTurnOnCollider()
    {
        yield return new WaitForSeconds(1.0f);
        GetComponent<BoxCollider>().enabled = true;
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
