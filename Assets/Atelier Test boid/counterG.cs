using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counterG : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Ground encoutered");
        if (other.gameObject.layer == LayerMask.NameToLayer("SOL"))
        {
            Testbox parent = (Testbox)transform.parent.GetComponent<Testbox>();
            parent.AddCollider(other);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SOL"))
        {
            Testbox parent = (Testbox)transform.parent.GetComponent<Testbox>();
            parent.AddCollider(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Testbox parent = (Testbox)transform.parent.GetComponent<Testbox>();
        parent.RemoveCollider(other, other.gameObject.layer);
    }
    
}
