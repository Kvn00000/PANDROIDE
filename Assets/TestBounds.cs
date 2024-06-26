using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBounds : MonoBehaviour
{

    public Vector3 center;
    public Vector3 top;
    public Vector3 bottom;
    public Vector3 front;
    public Vector3 back;
    public Vector3 left;
    public Vector3 right;
    public BoxCollider box;

    // Start is called before the first frame update
    void Start()
    {
        box = this.GetComponent<BoxCollider>();
        center = box.bounds.center;
        Vector3 extents = box.bounds.extents;
        top = new Vector3(center.x, center.y + extents.y, center.z);
        bottom = new Vector3(center.x, center.y - extents.y, center.z);
        front = new Vector3(center.x, center.y, center.z + extents.z);
        back = new Vector3(center.x, center.y, center.z - extents.z);
        left = new Vector3(center.x - extents.x, center.y, center.z);
        right = new Vector3(center.x + extents.x, center.y, center.z);
    }

    // Update is called once per frame
    void Update()
    {
   
        Debug.Log("EXTENTS "+box.bounds.extents);
        Debug.Log("TOP is " + top+ " bottom is "+bottom);
        Debug.Log("Front is " + front + " back is " + back);
        Debug.Log("Right is " + right + " left is " + left);

    }
}
