using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BoxCollider box = this.GetComponent<BoxCollider>();
        Vector3 center=box.bounds.center;
        Debug.Log("EXTENTS "+box.bounds.extents);
    }
}
