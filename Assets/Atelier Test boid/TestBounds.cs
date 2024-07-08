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
    public float resizeAmount=1.5f;
    private bool inverse = false;
    private Quaternion topQuat;
    private Rigidbody rb;
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
        rb=this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("EXTENTS " + box.bounds.extents);
        Debug.Log("TOP is " + top+ " bottom is "+bottom);
        resizeCube(resizeAmount, "x");
        //Debug.Log("Front is " + front + " back is " + back);
        //Debug.Log("Right is " + right + " left is " + left);
        //Debug.Log("center " + center + " other center " + this.GetComponent<BoxCollider>().bounds);
        //Debug.Log(" Quater is " + this.transform.rotation);
        Debug.DrawLine(center, top, Color.red);
        Vector3 rot = this.transform.rotation.eulerAngles;
        Vector3 newTop = RotatePointAroundPivot(top,center,rot) ;
        Debug.DrawLine(center, newTop, Color.green);
       Quaternion test = Quaternion.FromToRotation(top, newTop);
        Quaternion test2 = Quaternion.FromToRotation(center, newTop);
        Quaternion t3 = Quaternion.LookRotation(newTop, top);
        //Debug.Log(" Test 1: " + test + " test 2 : " + test2);
        //Debug.Log(" Rotate Test : " + this.transform.rotation.eulerAngles + "  Via newTop = " + test.eulerAngles + " t2 : " + test2.eulerAngles);
        //Debug.Log("T3 " + RotatePointAroundPivot(newTop,center,Quaternion.Inverse(this.transform.rotation).eulerAngles)) ;
        //Debug.Log("Box center "+this.transform.TransformPoint(box.center)+" Rigid Center "+this.transform.TransformPoint(rb.centerOfMass));
    }
    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles){
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles)* dir; // rotate it
        Vector3 nPoint = dir + pivot;
        return nPoint; // return it

    }

   private void resizeCube(float amount, string axis)
   {
        Debug.Log("Here");
        switch (axis)
        {
            case "x":
                if (!inverse)
                {
                    Debug.Log("ICI");
                    this.transform.position = new Vector3(this.transform.position.x+(amount/2), this.transform.position.y, this.transform.position.z);
                    this.transform.localScale = new Vector3(this.transform.localScale.x+amount, this.transform.localScale.y, this.transform.localScale.z);
                }
                else
                {
                    
                    this.transform.position = new Vector3(this.transform.position.x-(amount/2), this.transform.position.y, this.transform.position.z);
                    this.transform.localScale = new Vector3(this.transform.localScale.x-amount, this.transform.localScale.y, this.transform.localScale.z);
                }
                break;
            case "y":
                if (!inverse)
                {
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + (amount / 2), this.transform.position.z);
                    this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y+amount, this.transform.localScale.z);
                }
                else
                {
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y-(amount / 2), this.transform.position.z);
                    this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y-amount, this.transform.localScale.z);
                }
                break;
            case "z":
                if (!inverse)
                {
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + (amount / 2));
                    this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z + amount);
                }
                else
                {
                    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z- (amount / 2));
                    this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z- amount);
                }
                break;
            default:
                break;
        
        }
   }
}
