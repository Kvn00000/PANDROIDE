using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeCubeGravity : MonoBehaviour
{
    Rigidbody parent;
    List<Collider> listcollider= new List<Collider>();
    // Start is called before the first frame update
    void Start()
    {
        parent = this.GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (!listcollider.Contains(other))
        {
            listcollider.Add(other);
        }
        if (listcollider.Count > 0)
        {
            parent.mass = 1000000;
        }
    }
    private void OnTriggerStay(Collider other)
    {

    }
    private void OnTriggerExit(Collider other)
    {
        listcollider.Remove(other);
        if (listcollider.Count < 0)
        {
            parent.mass = 1000f;
        }
    }
}
