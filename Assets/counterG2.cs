using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counterG2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Ground encoutered");
        if (other.gameObject.layer == LayerMask.NameToLayer("SOL"))
        {
            boidTuning parent = (boidTuning)transform.parent.GetComponent<boidTuning>();
            parent.AddCollider(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("SOL"))
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
