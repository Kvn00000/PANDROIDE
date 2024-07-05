using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeCubeGravity : MonoBehaviour
{
    Rigidbody parent;
    private bool grav = true;
    List<Collider> listcollider= new List<Collider>();
    // Start is called before the first frame update
    void Start()
    {
        parent = this.GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (listcollider.Count < 0)
        {
            parent.mass = 1000f;
        }
        else
        {
            parent.mass = 1000000;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        grav = false;
    }
    private void OnTriggerStay(Collider other)
    {
        grav = false;
    }
    private void OnTriggerExit(Collider other)
    {
        grav = true;
    }
}
