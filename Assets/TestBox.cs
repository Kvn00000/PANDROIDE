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

    private bool frein = false;




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
        //If not on the ground apply gravity
        if (!grounded)
        {
            rb.AddForce(Physics.gravity * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {
            float rotation = 0.0f;
            rotation = AvoidBoidRcast(rotation);
            rotation = AvoidWallRcast(rotation);
            //transform.Rotate(transform.up, rotation*Time.deltaTime);
            rb.AddTorque(transform.right * rotation*Time.deltaTime);
            //rb.AddRelativeTorque(transform.right * rotation * Time.deltaTime);
            if (rotation == 0.0)
            {
                rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Force);
                rb.angularVelocity = Vector3.zero;
            }
            if (rotation != 0.0)
            {
                rb.AddForce(transform.forward * speed*0.001f * Time.deltaTime, ForceMode.Force);
            }

        }

    }
    // VERSION AVEC RAYCAST
    private float AvoidWallRcast(float rotate)
    {
        //Init Ray
        Vector3 myPos = rb.transform.position;
        Ray front = new Ray(myPos, transform.forward);
        Ray right = new Ray(myPos, transform.right);
        Ray left = new Ray(myPos, -transform.right);
        // Build Other
        Vector3 f = transform.forward;
        Vector3 r = transform.right;
        Vector3 fright = new Vector3(f.x + r.x,f.y,f.z+r.z) ;
        Vector3 fleft= new Vector3(f.x - r.x, f.y+r.y, f.z - r.z);
        Ray FrontRight = new Ray(myPos, fright);
        Ray FrontLeft = new Ray(myPos, fleft);
        // Init int
        int fr = 0;
        int ri = 0;
        int le = 0;
        int fri = 0;
        int fle = 0;
        // Draw Ray

        Debug.DrawRay(myPos,transform.forward);
        Debug.DrawRay(myPos, transform.right);
        Debug.DrawRay(myPos, -transform.right);
        Debug.DrawRay(myPos, fright);
        Debug.DrawRay(myPos, fleft);
        //
        float maxdistance = 0.58f;
        int layerWall = 8;
        LayerMask layermask = 1 << layerWall;
        float rotation = 0.0f;
        bool hit = false;
        //hit left
        if (Physics.Raycast(left, maxdistance, layermask) && (!Physics.Raycast(right, maxdistance, layermask)))
        {
            //Debug.Log("HIT LEFT");
            rotation = 10;
            le = 1;
            hit = true;
        }
        //hit right
        if ((Physics.Raycast(right, maxdistance, layermask)) && (!Physics.Raycast(left, maxdistance, layermask)))
        {
            //Debug.Log("HIT Right");
            rotation = -20;
            ri = 1;
            hit = true;
        }
        //hit front
        if (Physics.Raycast(front, maxdistance, layermask))
        {
            //Debug.Log("HIT FRONT");
            fr = 1;
            hit = true;
        }
        if(hit==false)
        {
            //Debug.Log("HIT FRONT");
            //rb.angularVelocity = Vector3.zero;
        }
        rotation = 40 * fr + 25 * le - 25 * ri;
        if (rotation != 0.0)
        {
            return rotation;
        }
        else
        {
            return rotate;
        }
    }

    private float AvoidBoidRcast(float rotate)
    {
        //Init Ray
        Vector3 myPos = rb.transform.position;
        Ray front = new Ray(myPos, transform.forward);
        Ray right = new Ray(myPos, transform.right);
        Ray left = new Ray(myPos, -transform.right);
        // Build Other
        Vector3 f = transform.forward;
        Vector3 r = transform.right;
        Vector3 fright = new Vector3(f.x + r.x, f.y, f.z + r.z);
        Vector3 fleft = new Vector3(f.x - r.x, f.y + r.y, f.z - r.z);
        Ray FrontRight = new Ray(myPos, fright);
        Ray FrontLeft = new Ray(myPos, fleft);
        // Init int
        int fr = 0;
        int ri = 0;
        int le = 0;
        int fri = 0;
        int fle = 0;
        // Draw Ray

        Debug.DrawRay(myPos, transform.forward);
        Debug.DrawRay(myPos, transform.right);
        Debug.DrawRay(myPos, -transform.right);
        Debug.DrawRay(myPos, fright);
        Debug.DrawRay(myPos, fleft);
        //
        float maxdistance = 0.2f;
        int layerWall = 6;
        LayerMask layermask = 1 << layerWall;
        float rotation = 0.0f;
        bool hit = false;
        //hit left
        RaycastHit[] leftHit = Physics.RaycastAll(left,maxdistance,layermask);
        le=collidedRcastAll(leftHit, myPos);

        //hit right
        RaycastHit[] rightHit = Physics.RaycastAll(right,maxdistance, layermask);
        ri = collidedRcastAll(rightHit, myPos);

        //hit front
        RaycastHit[] frontHit = Physics.RaycastAll(front, maxdistance, layermask);
        fr = collidedRcastAll(frontHit, myPos);
        //hit front Right
        RaycastHit[] frontRightHit = Physics.RaycastAll(FrontRight, maxdistance, layermask);
        fri = collidedRcastAll(frontRightHit, myPos);
        //hit front Left
        RaycastHit[] frontLeftHit = Physics.RaycastAll(FrontLeft, maxdistance, layermask);
        fle = collidedRcastAll(frontLeftHit, myPos);
        rotation = 20 * fr + 15 * fle - 15 * fri + 5 * le - 5 * le;

        if (rotation != 0.0)
        {
            return rotation;
        }
        else
        {
            return rotate;
        }
    }

    private int collidedRcastAll(RaycastHit[] tab, Vector3 myPos)
    {
        for(int i = 0; i<tab.Length; i++)
        {
            RaycastHit hitInfo = tab[i];
            if (hitInfo.transform.position != myPos)
            {
                return 1;
            }
        }
        return 0;
    }
    // BOOTLEG EN BAS --> MARCHE PAS
    private void v1()
    {
        // Moves the object forward one unit every frame.
        //Rotation Check
        float rotate = 0.0f;
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
            rotate = handleWallCollision();
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
                Debug.Log("NO ROTATE");
                //Finished a rotation
                if (!noturn)
                {
                    //rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    //wallCollider = new List<Collider>();
                    Debug.Log("Finished rotation restarting");
                }
                rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Force);
                noturn = true;
            }
            // I have to turn
            else
            {
                Debug.Log("ROTATE");
                if (noturn)
                {
                    if (frein)
                    {
                        rb.velocity = Vector3.zero;
                    }
                    noturn = false;
                }
                //rb.AddRelativeTorque(Vector3.right * rotate * Time.deltaTime, ForceMode.Force);
                transform.Rotate(Vector3.up, 20 * rotate * Time.deltaTime);
                if (frein)
                {
                    Debug.Log("FREIN ON");
                    rb.AddForce(transform.forward * speed * 0.01f * Time.deltaTime, ForceMode.Force);
                }
                else
                {
                    Debug.Log("FREIN OFF");
                    rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Force);
                }


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
            Debug.Log("DISTANCE = " + dis);
            if (angle<110)
            {
                rotation = 0.8f;
                if (angle <90)
                {
                    frein = true;
                }
                else 
                {
                    frein = false;
                }

            }

            //Debug.Log("Closest point = "+clp);
            //Debug.Log("on Bounds = " + clpb);
            //Debug.Log("TOWARDS = " + tow);
            
            
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
            //Debug.Log("is  NOT Grounded");
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