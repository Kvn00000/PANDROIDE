using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counterG2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Ground encoutered");
        //Debug.Log("FOUND A " + other.gameObject.layer);
        //Debug.Log("LOOKING FOR A " + LayerMask.NameToLayer("SOL") + " OR A " + LayerMask.NameToLayer("MUR"));
        if ((other.gameObject.layer == LayerMask.NameToLayer("SOL"))||(other.gameObject.layer == LayerMask.NameToLayer("MUR")))
        {
            //Debug.Log("FOUND A SOL");
            boidTuning parent = (boidTuning)transform.parent.GetComponent<boidTuning>();
            parent.AddCollider(other);
            //Debug.Log(other.gameObject.layer + " ADDED");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.layer == LayerMask.NameToLayer("SOL")) || (other.gameObject.layer == LayerMask.NameToLayer("MUR")))
        {
            boidTuning parent = (boidTuning)transform.parent.GetComponent<boidTuning>();
            parent.AddCollider(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        boidTuning parent = (boidTuning)transform.parent.GetComponent<boidTuning>();
        parent.RemoveCollider(other, other.gameObject.layer);
    }

}
