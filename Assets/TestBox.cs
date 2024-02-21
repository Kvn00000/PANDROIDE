using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testbox : MonoBehaviour
{
    //public GameObject boid;
    private Rigidbody rb;
    private List<Collider> groundCollider = new List<Collider>();
    private List<Collider> wallCollider = new List<Collider>();
    private List<Vector3> refpos = new List<Vector3>();
    private List<Collider> boidCollider = new List<Collider>();
    private bool grounded = false;
    private bool noturn = true;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Invoke("ShowObject", 2);
    }

    // Update is called once per frame
    public float speed = 2.5f;



    void Update()
    {
        // Moves the object forward one unit every frame.
        //Rotation Check
        float rotate=0.0f;
        /*
        // boid detected in far sphere
        if (FarBoid())
        {
            rotate = handleApproach();
        }
        // 
        if (MediumBoid())
        {
            rotate = handleAlign();
        }
        if (CloseBoid())
        {
            rotate = handleAvoid();
        }
        */
        if (IsWallNear())
        {
             rotate= handleWallCollision();
        }     
        // Add Gravity if not grounded
        if (!grounded)
        {
            rb.AddForce(Physics.gravity * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {
            Debug.Log("Rotate Value = " + rotate);
            // I don't have to turn
            if (rotate < 0.1f)
            {
                //Finished a rotation
                if (!noturn)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    wallCollider = new List<Collider>();
                    Debug.Log("Finished rotation restarting");
                }
                rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Force);
                noturn = true;
            }
            // I have to turn
            else
            {
                if(noturn) 
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    noturn = false;
                }
                rb.AddRelativeTorque(Vector3.right * rotate * Time.deltaTime, ForceMode.Force);
                rb.AddForce(transform.forward * speed *0.01f* Time.deltaTime, ForceMode.Impulse);

            }
        }


    }

    private float handleWallCollision()
    {
        float rotation = 0.0f;
        Vector3 mypos = this.transform.position;
        //Debug.Log("\t\tMyPOS = " + mypos);
        foreach (Collider wall in wallCollider)
        {

            Vector3 pos=wall.gameObject.transform.position;
            Vector3 clp = wall.ClosestPoint(mypos);
            Vector3 clpb = wall.ClosestPointOnBounds(mypos);
            Vector3 tow = Vector3.MoveTowards(this.transform.forward, clp,1);
            float dis = Vector3.Distance(this.transform.forward, tow);
            float angle = Vector3.Angle(this.transform.forward, clp);
            Debug.Log("ANGLE = " + angle);
            if (angle<100)
            {
                rotation = 0.9f;
            }

            //Debug.Log("Closest point = "+clp);
            //Debug.Log("on Bounds = " + clpb);
            //Debug.Log("TOWARDS = " + tow);
            //Debug.Log("DISTANCE = " + dis);
            
        }
        return rotation;
    }

    private void LateUpdate()
    {
        IsGrounded();
        
    }

    //Checking Collisions 
    //Ground
    public void IsGrounded()
    {
        //Debug.Log("GROUND SIZE= " + groundCollider.Count);
        if (groundCollider.Count != 0){
           Debug.Log("is Grounded");
           
           grounded = true;
        }
        else
        {
            Debug.Log("is  NOT Grounded");
            grounded = false;
        }
    }
    //Wall
    private bool IsWallNear() 
    {
        if (wallCollider.Count != 0)
        {
            return true;
        }
        return false;
    }
    //Toolbox to handle collison layer
    public void AddCollider(Collider toAdd)
    {
        if (!CheckPresence(toAdd))
        {
            int test = toAdd.gameObject.layer;
            Debug.Log("The layer is " + test);
            if (test == LayerMask.NameToLayer("SOL"))
            {
                //Debug.Log("GROUND ADDED");
                groundCollider.Add(toAdd);
                return;
            }
            if (test == LayerMask.NameToLayer("MUR"))
            {
                Debug.Log("WALL ADDED");
                wallCollider.Add(toAdd);
                return;
            }
            if (test == LayerMask.NameToLayer("BOID"))
            {
                //Debug.Log("BOID ADDED");
                boidCollider.Add(toAdd);
                return;
            }
        }
    }
    public void RemoveCollider(Collider toRemove, int layer)
    {
        if (CheckPresence(toRemove))
        {
            if (layer == LayerMask.NameToLayer("SOL"))
            {
                groundCollider.Remove(toRemove);
                return;
            }
            if (layer == LayerMask.NameToLayer("MUR"))
            {
                wallCollider.Remove(toRemove);

                return;
            }
            if (layer == LayerMask.NameToLayer("BOID"))
            {
                boidCollider.Remove(toRemove);
                return;
            }
        }
    }

    public bool CheckPresence(Collider tocheck)
    {
        if (groundCollider.Contains(tocheck))
        {
            return true;
        }
        if (wallCollider.Contains(tocheck))
        {
            return true;
        }
        if (boidCollider.Contains(tocheck))
        {
            return true;
        }
        return false;
    }


}

