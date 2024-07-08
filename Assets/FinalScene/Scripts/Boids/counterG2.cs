using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class counterG2 : MonoBehaviour
{

    private int _layerSol;
    private int _layerMur;
    private boidTuning parent;
    private void Start()
    {
        _layerSol = LayerMask.NameToLayer("SOL");
        _layerMur = LayerMask.NameToLayer("MUR");
       parent = transform.parent.GetComponent<boidTuning>();
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Ground encoutered");
        //Debug.Log("FOUND A " + other.gameObject.layer);
        //Debug.Log("LOOKING FOR A " + LayerMask.NameToLayer("SOL") + " OR A " + LayerMask.NameToLayer("MUR"));
        if ((other.gameObject.layer ==_layerSol )||(other.gameObject.layer == _layerMur))
        {
            //Debug.Log("FOUND A SOL");
            parent.AddCollider(other);
            //Debug.Log(other.gameObject.layer + " ADDED");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.layer == _layerSol) || (other.gameObject.layer == _layerMur))
        {
            parent.AddCollider(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        parent.RemoveCollider(other, other.gameObject.layer);
    }
    private void OnDestroy()
    {
        //Debug.Log("POOOOOOOOOOOOOOOOOKKKKKKKKKIIIIIIIIIIIIIIIIIIIIIIIIIMMMMMMMMMMMMOOOOOOOOOOOOOOOOONNNNNNNNN");
    }
}
