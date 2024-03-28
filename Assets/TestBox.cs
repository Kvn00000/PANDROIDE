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
    public float speed = 3.4f;
    private bool grounded = false;
    private bool noturn = true;
    private bool goTo=false;
    private bool frein = false;
    private int cptTempoGOTO = 0;

    public bool withGoto = false;
    public bool withCohesion = false;
    public bool withAvoid = false;
    public bool withDEBUG = false;
    int timer = 0;
    //FOR V3

    private List<Collider> attractionCollider = new List<Collider>();
    private List<Collider> cohesionCollider = new List<Collider>();
    private List<Collider> closeCollider = new List<Collider>();


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //Invoke("ShowObject", 2);
    }

    // Update is called once per frame
    



    void Update()
    {

        v2();
    }

    private void LateUpdate()
    {
        IsGrounded();

    }

    // V3 --> Tentative With sphere
    private void v3()
    {
        //If not on the ground apply gravity
        if (!grounded)
        {
            rb.AddForce(Physics.gravity * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {
            if (withDEBUG)
            {
                Debug.Log("//////////////////////////////////////////////////////////////////////////////////");
            }

            float rotation = 0.0f;
            float oldrotate;
            //Define distance
            float wallRay = 0.4f;
            //////////////////////////
            if ((withGoto))
            {
                rotation = AttractionSphere(rotation);
                if ((rotation != 0.0f) && (withDEBUG)) { Debug.Log("ATTRACTION ACTIVATED : " + rotation); }
            }
            if ((withCohesion))
            {
                oldrotate = rotation;
                rotation = CohesionSphere(rotation);
                if ((rotation != oldrotate) && (withDEBUG)) { Debug.Log("COHESION ACTIVATED : " + rotation); }
            }
            if ((withAvoid))
            {
                oldrotate = rotation;
                rotation = AvoidSphere(rotation);
                if ((rotation != oldrotate) && (withDEBUG)) { Debug.Log("AVOIDANCE ACTIVATED : " + rotation); }
            }
            //Highest Priority
            oldrotate = rotation;
            rotation = AvoidWallRcast(rotation, wallRay);

            if ((rotation != oldrotate))
            {
                if ((withDEBUG))
                {
                    Debug.Log("WALL AVOID ACTIVATED : " + rotation);
                }
            }
            if ((withDEBUG)) { Debug.Log("FINAL ROTATION : " + rotation); }
            // APPLY ROTATION
            if (rotation == 0.0)
            {
                rb.angularVelocity = Vector3.zero;
                rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Force);
            }
            else
            {
                float angle = rotation * Time.deltaTime;
                //Debug.Log(" FINAL VALUE OF ANGLE AFTER TIME "+angle);
                transform.Rotate(Vector3.up, angle);
                if (frein) { rb.AddForce(transform.forward * speed * 0.01f * Time.deltaTime, ForceMode.Force); }
                else { rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Force); }
            }
            clearPerceptions();
            setFrein(false);
            if (withDEBUG)
            {
                //Debug.Log("VELOCITY IS " + rb.velocity);
                Debug.Log("//////////////////////////////////////////////////////////////////////////////////");
            }

        }
    }
    private float AttractionSphere(float rotate)
    {
        throw new NotImplementedException();
    }
    private float CohesionSphere(float rotate)
    {
        throw new NotImplementedException();
    }
    private float AvoidSphere(float rotate)
    {
        Vector3 closestPoint=Vector3.zero;
        bool unassigned = true;
        Vector3 myPos = rb.transform.position;
        if (closeCollider.Count == 0)
        {
            if (withDEBUG) { Debug.Log("NO CLOSE FOUND"); }
            return rotate;
        }
        foreach (Collider c in closeCollider)
        {
            Vector3 clp = c.ClosestPoint(myPos);
            if (unassigned)
            {
                closestPoint = clp;
                unassigned = false;
            }
            else
            {
                float distance = Vector3.Distance(myPos, clp);
                float minD = Vector3.Distance(myPos, closestPoint);
                if (distance < minD)
                {
                    closestPoint = clp;
                }
            }
        }
        
        if (withDEBUG) { Debug.DrawLine(myPos, closestPoint, Color.yellow); }
        float rotation= getAvoidSphereValue(myPos,closestPoint);
        if (rotation != 0.0) {
            setFrein(true);
            return rotation; }
        else { return rotate; }
    }

    private float getAvoidSphereValue(Vector3 myPos,Vector3 closestPoint)
    {
        Vector3 localCpos = rb.transform.InverseTransformPoint(closestPoint);
        bool isRight = false;
        // Determining case
        if (localCpos.x > 0) { isRight = true; }
        if ((isRight)){ return -20; }
        else{ return 20; }

    }

    // Toolbox for BOID SPHERES V3

    public void addCloseboid(Collider toAdd) 
    {
        if (!closeCollider.Contains(toAdd))
        {
            closeCollider.Add(toAdd);
        }
    }
    public void addCohesionBoid(Collider toAdd)
    {
        if ((!closeCollider.Contains(toAdd)) && (!cohesionCollider.Contains(toAdd)))
        {
            cohesionCollider.Add(toAdd);
        }
    }
    public void addAttractionBoid(Collider toAdd)
    {
        if (((!closeCollider.Contains(toAdd)) && (!cohesionCollider.Contains(toAdd))) && (!attractionCollider.Contains(toAdd)))
        {
            attractionCollider.Add(toAdd);
        }
    }
    private void clearPerceptions()
    {
        closeCollider = new List<Collider>();
        cohesionCollider = new List<Collider>();
        attractionCollider = new List<Collider>();
    }


    // V2 --> REFONTE AVEC RAYCAST 
    private void v2()
    {
        //If not on the ground apply gravity
        if (!grounded)
        {
            rb.AddForce(Physics.gravity * Time.deltaTime, ForceMode.Acceleration);
        }
        else
        {
            if (withDEBUG)
            {
                Debug.Log("//////////////////////////////////////////////////////////////////////////////////");
            }
            
            float rotation = 0.0f;
            float oldrotate;
            //Define distance
            float wallRay = 0.40f;
            float avoidRay = 0.41f;
            float cohesionRay = 0.8f;
            float attractionRay = 1.1f;
            //////////////////////////


            if ((withGoto) && (timer==0))
            {
                rotation = GoToBoidRcastv2(rotation,cohesionRay,attractionRay);
                if((rotation != 0.0f) && (withDEBUG)){ Debug.Log("ATTRACTION ACTIVATED : " + rotation); }
            }
            if ((withCohesion)&&(timer==0))
            {
                oldrotate = rotation;
                rotation = CohesionBoidRcast(rotation,avoidRay,cohesionRay);
                if ((rotation != oldrotate) && (withDEBUG)) { Debug.Log("COHESION ACTIVATED : " + rotation); }
            }
            if ((withAvoid)&&(timer==0))
            {
                oldrotate= rotation;
                rotation = AvoidBoidRcastv3(rotation,avoidRay);
                if ((rotation != oldrotate) && (withDEBUG)) { Debug.Log("AVOIDANCE ACTIVATED : " + rotation); }
            }
            //Highest Priority
            oldrotate = rotation;
            rotation = AvoidWallRcast(rotation,wallRay);

            if ((rotation != oldrotate)) {
                timer = 3;
                if ((withDEBUG))
                {
                    Debug.Log("WALL AVOID ACTIVATED : " + rotation); 
                }
            }
            if ((withDEBUG)) { Debug.Log("FINAL ROTATION : " + rotation); }
            // APPLY ROTATION
            if (rotation == 0.0)
            {    
                rb.angularVelocity = Vector3.zero;
                rb.AddForce(transform.forward * speed  * Time.deltaTime, ForceMode.Force);
            }
            else
            {
                float angle = rotation * Time.deltaTime;
                //Debug.Log(" FINAL VALUE OF ANGLE AFTER TIME "+angle);

               
               
                transform.Rotate(Vector3.up, angle);
                rb.angularVelocity = Vector3.zero;
                rb.inertiaTensor = Vector3.zero;
                if (frein) { rb.AddForce(transform.forward * speed*0.01f * Time.deltaTime, ForceMode.Force); }
                else { rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Force); }
            }
            //Debug.Log("Inertie " + rb.inertiaTensor);
            //Debug.Log("Inertie " + rb.inertiaTensorRotation);
            //Debug.Log("ANGULAR VELOCITY " + rb.angularVelocity);
            // APPLY TRANSLATION
            /*
            if (frein)
            {
                rb.AddForce(transform.forward * speed * 0.01f * Time.deltaTime, ForceMode.Force);
            }
            else
            {
                rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Force);
            }
            */
            setFrein(false);
            if (timer > 0) { timer--; }
            if (withDEBUG)
            {
                //Debug.Log("VELOCITY IS " + rb.velocity);
                Debug.Log("//////////////////////////////////////////////////////////////////////////////////");
            }
            
        }
    }
    

    // AVOID WALL BEHAVIOUR
    private float AvoidWallRcast(float rotate,float wallRay)
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

        //Debug.DrawRay(myPos,transform.forward);
        //Debug.DrawRay(myPos, transform.right);
        //Debug.DrawRay(myPos, -transform.right);
        //Debug.DrawRay(myPos, fright);
        //Debug.DrawRay(myPos, fleft);
        //
        float maxdistance = wallRay;
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
            setGoto(false);
            setFrein(true);
            return rotation;
        }
        else
        {
            return rotate;
        }
    }
    //COHERENCE BEHAVIOUR
    private float CohesionBoidRcast(float rotate,float minRay,float maxRay) 
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
        Ray downLeft = new Ray(myPos, -fright);
        Ray downRight = new Ray(myPos, -fleft);
        // Init Collision detection parameters
        float maxdistance = maxRay;
        float mindistance = minRay;
        //Setting LayerMask for collision detection
        int layerBoid = 6;
        LayerMask layermask = 1 << layerBoid;
        //Getting all collisions
        List<Vector3> allPosCollide = new List<Vector3>();
        List<Transform> allTransformsCollide = new List<Transform>();

        //hit left
        RaycastHit[] leftHit = Physics.RaycastAll(left, maxdistance, layermask);
        collidedCohesionRcastAll(leftHit,myPos,allPosCollide,allTransformsCollide, mindistance, maxdistance);
        //hit right
        RaycastHit[] rightHit = Physics.RaycastAll(right, maxdistance, layermask);
        collidedCohesionRcastAll(rightHit, myPos, allPosCollide, allTransformsCollide, mindistance, maxdistance);
        //hit front
        RaycastHit[] frontHit = Physics.RaycastAll(front, maxdistance, layermask);
        collidedCohesionRcastAll(frontHit, myPos, allPosCollide, allTransformsCollide, mindistance, maxdistance);
        //hit front Right
        RaycastHit[] frontRightHit = Physics.RaycastAll(FrontRight, maxdistance, layermask);
        collidedCohesionRcastAll(frontRightHit, myPos, allPosCollide, allTransformsCollide, mindistance, maxdistance);
        //hit front Left
        RaycastHit[] frontLeftHit = Physics.RaycastAll(FrontLeft, maxdistance, layermask);
        collidedCohesionRcastAll(frontLeftHit, myPos, allPosCollide, allTransformsCollide, mindistance, maxdistance);
        //hit down Right
        RaycastHit[] downRightHit = Physics.RaycastAll(downRight, maxdistance, layermask);
        collidedCohesionRcastAll(downRightHit, myPos, allPosCollide, allTransformsCollide, mindistance, maxdistance);
        //hit down Left
        RaycastHit[] downLeftHit = Physics.RaycastAll(downLeft, maxdistance, layermask);
        collidedCohesionRcastAll(downLeftHit, myPos, allPosCollide, allTransformsCollide, mindistance, maxdistance);


        //Getting rotation
        float rotation = 0.0f;
        rotation = getCohesionRotation(myPos, allPosCollide,allTransformsCollide);

        if (rotation != 0.0) { return rotation; }
        else { return rotate; }
    }
    // REPULSION BEHAVIOUR
    private float AvoidBoidRcastv3(float rotate, float avoidRay)
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
        Ray downLeft = new Ray(myPos, -fright);
        Ray downRight = new Ray(myPos, -fleft);
        // Init Collision detection parameters
        float maxdistance = 0.8f;
        float disCollide = avoidRay;
        //Setting LayerMask for collision detection
        int layerBoid = 6;
        LayerMask layermask = 1 << layerBoid;
        //Getting all collisions
        List<Vector3> allPosCollide = new List<Vector3>();
        Vector3 closestBuddy = Vector3.zero;
        //hit left
        RaycastHit[] leftHit = Physics.RaycastAll(left, maxdistance, layermask);
        closestBuddy = collidedRcastAllv3(leftHit, myPos, disCollide, closestBuddy);
        //hit right
        RaycastHit[] rightHit = Physics.RaycastAll(right, maxdistance, layermask);
        closestBuddy = collidedRcastAllv3(rightHit, myPos, disCollide, closestBuddy);
        //hit front
        RaycastHit[] frontHit = Physics.RaycastAll(front, maxdistance, layermask);
        closestBuddy = collidedRcastAllv3(frontHit, myPos, disCollide, closestBuddy);
        //hit front Right
        RaycastHit[] frontRightHit = Physics.RaycastAll(FrontRight, maxdistance, layermask);
        closestBuddy = collidedRcastAllv3(frontRightHit, myPos, disCollide, closestBuddy);

        //hit front Left
        RaycastHit[] frontLeftHit = Physics.RaycastAll(FrontLeft, maxdistance, layermask);
        closestBuddy = collidedRcastAllv3(frontLeftHit, myPos, disCollide, closestBuddy);

        //hit down Right
        RaycastHit[] downRightHit = Physics.RaycastAll(downRight, maxdistance, layermask);
        closestBuddy = collidedRcastAllv3(downRightHit, myPos, disCollide, closestBuddy);

        //hit down Left
        RaycastHit[] downLeftHit = Physics.RaycastAll(downLeft, maxdistance, layermask);
        closestBuddy = collidedRcastAllv3(downLeftHit, myPos, disCollide, closestBuddy);
        //Getting rotation
        float rotation;
        if (closestBuddy.Equals(Vector3.zero))
        {
            rotation=0.0f;
        }
        else
        {
            if (withDEBUG) { Debug.DrawLine(myPos,closestBuddy,Color.yellow); }
            rotation = getAvoidRotationv2(myPos, closestBuddy);
        }

        if (rotation != 0.0) { return rotation; }
        else { return rotate; }
    }
    private float AvoidBoidRcastv2(float rotate,float avoidRay)
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
        Ray downLeft = new Ray(myPos, -fright);
        Ray downRight = new Ray(myPos, -fleft);
        // Init Collision detection parameters
        float maxdistance = 0.8f;
        float disCollide = avoidRay;
        //Setting LayerMask for collision detection
        int layerBoid = 6;
        LayerMask layermask = 1 << layerBoid;
        //Getting all collisions
        List<Vector3> allPosCollide = new List<Vector3>();
        
        //hit left
        RaycastHit[] leftHit = Physics.RaycastAll(left, maxdistance, layermask);
        collidedRcastAllV2(leftHit, myPos, disCollide, allPosCollide);
        //hit right
        RaycastHit[] rightHit = Physics.RaycastAll(right, maxdistance, layermask);
        collidedRcastAllV2(rightHit, myPos, disCollide, allPosCollide);
        //hit front
        RaycastHit[] frontHit = Physics.RaycastAll(front, maxdistance, layermask);
        collidedRcastAllV2(frontHit, myPos, disCollide, allPosCollide);
        //hit front Right
        RaycastHit[] frontRightHit = Physics.RaycastAll(FrontRight, maxdistance, layermask);
        collidedRcastAllV2(frontRightHit, myPos, disCollide, allPosCollide);
        //hit front Left
        RaycastHit[] frontLeftHit = Physics.RaycastAll(FrontLeft, maxdistance, layermask);
        collidedRcastAllV2(frontLeftHit, myPos, disCollide, allPosCollide);
        //hit down Right
        RaycastHit[] downRightHit = Physics.RaycastAll(downRight, maxdistance, layermask);
        collidedRcastAllV2(downRightHit, myPos, disCollide, allPosCollide);
        //hit down Left
        RaycastHit[] downLeftHit = Physics.RaycastAll(downLeft, maxdistance, layermask);
        collidedRcastAllV2(downLeftHit, myPos, disCollide, allPosCollide);


        //Getting rotation
        float rotation = 0.0f;
        rotation = getAvoidRotation(myPos, allPosCollide);
        //Overwriting less important rotations if needed

        if (rotation != 0.0) { return rotation; }
        else { return rotate; }
    }
    private float AvoidBoidRcast(float rotate,float avoidRay)
    {
        //Init Ray
        Vector3 myPos = rb.transform.position;
        Ray front = new Ray(myPos, transform.forward);
        Ray right = new Ray(myPos, transform.right);
        Ray left = new Ray(myPos, -transform.right);
        // Build Other
        Vector3 f = transform.forward;
        Vector3 r = transform.right;
        Vector3 fright = new Vector3(f.x +r.x, f.y, f.z + r.z);
        Vector3 fleft = new Vector3(f.x - r.x, f.y + r.y, f.z - r.z);
        Ray FrontRight = new Ray(myPos, fright);
        Ray FrontLeft = new Ray(myPos, fleft);
        Ray downLeft = new Ray(myPos, -fright);
        Ray downRight = new Ray(myPos, -fleft);
        // Init int
        int fr = 0;
        int ri = 0;
        int le = 0;
        int fri = 0;
        int fle = 0;
        // Draw Ray
        /*
        Debug.DrawRay(myPos, transform.forward);
        Debug.DrawRay(myPos, transform.right);
        Debug.DrawRay(myPos, -transform.right);
        Debug.DrawRay(myPos, fright);
        Debug.DrawRay(myPos, fleft);

        Debug.DrawRay(myPos, fleft2,Color.cyan);
        Debug.DrawRay(myPos, fright2, Color.green);
        //*/
        //
        float maxdistance = 1.0f;
        float disCollide = avoidRay;
        int layerWall = 6;
        LayerMask layermask = 1 << layerWall;
        float rotation = 0.0f;
        //bool hit = false;
        //hit front
        RaycastHit[] frontHit = Physics.RaycastAll(front, maxdistance, layermask);
        //Debug.Log("------------------- FRONT ---------------------");
        fr = collidedRcastAllv4(frontHit, myPos, disCollide);
        if (fr == 1){ return 30; }
        //hit front Right
        RaycastHit[] frontRightHit = Physics.RaycastAll(FrontRight, maxdistance, layermask);
        //Debug.Log("------------------- FRI ---------------------");
        fri = collidedRcastAllv4(frontRightHit, myPos, disCollide);
        if (fri == 1) { return -15; }
        //hit front Left
        RaycastHit[] frontLeftHit = Physics.RaycastAll(FrontLeft, maxdistance, layermask);
        //Debug.Log("------------------- FLE ---------------------");
        fle = collidedRcastAllv4(frontLeftHit, myPos, disCollide);
        if (fle == 1) { return 15; }
        //hit left
        RaycastHit[] leftHit = Physics.RaycastAll(left,maxdistance,layermask);
        //Debug.Log("------------------- LEFT ---------------------");
        le =collidedRcastAllv4(leftHit, myPos,disCollide);
        if (le == 1) { return 5; }
        //hit right
        RaycastHit[] rightHit = Physics.RaycastAll(right,maxdistance, layermask);
        //Debug.Log("------------------- RIGHT ---------------------");
        ri = collidedRcastAllv4(rightHit, myPos,disCollide);
        if (ri == 1) { return -5; }

        rotation = 20 * fr + 15 * fle - 15 * fri + 5 * le - 5 * ri;
        rotation = 0.0f;
        if (rotation != 0.0){ return rotation; }
        else { return rotate; }
    }
    // ATTRACTION BEHAVIOUR
    private float GoToBoidRcastv2(float rotate,float minRay,float maxRay)
    {
        //Init Ray
        Vector3 myPos = rb.transform.position;

        //Debug.Log("================>"+myPos);
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
        Ray downLeft = new Ray(myPos, -fright);
        Ray downRight = new Ray(myPos, -fleft);
        // Draw Ray
        if (withDEBUG)
        {
            /*
            Debug.DrawRay(myPos, transform.forward);
            Debug.DrawRay(myPos, transform.right);
            Debug.DrawRay(myPos, -transform.right);
            Debug.DrawRay(myPos, fright);
            Debug.DrawRay(myPos, fleft);
            Debug.DrawRay(myPos, -fright);
            Debug.DrawRay(myPos, -fleft);
            */
        }
        //
        float maxdistance = maxRay;
        float mindistance = minRay;
        int layerWall = 6;
        LayerMask layermask = 1 << layerWall;
        float rotation = 0.0f;
        List<Vector3> allPosCollide = new List<Vector3>();
        //hit front
        RaycastHit[] frontHit = Physics.RaycastAll(front, maxdistance, layermask);
        goToRcastAllV2(frontHit, myPos,mindistance, allPosCollide);
        //hit front Right
        RaycastHit[] frontRightHit = Physics.RaycastAll(FrontRight, maxdistance, layermask);
        goToRcastAllV2(frontRightHit, myPos, mindistance, allPosCollide);
        //hit front Left
        RaycastHit[] frontLeftHit = Physics.RaycastAll(FrontLeft, maxdistance, layermask);
        goToRcastAllV2(frontLeftHit, myPos, mindistance, allPosCollide);
        //hit left
        RaycastHit[] leftHit = Physics.RaycastAll(left, maxdistance, layermask);
        goToRcastAllV2(leftHit, myPos, mindistance, allPosCollide);
        //hit right
        RaycastHit[] rightHit = Physics.RaycastAll(right, maxdistance, layermask);
        goToRcastAllV2(rightHit, myPos, mindistance, allPosCollide) ;
        //hit down Right
        RaycastHit[] downRightHit = Physics.RaycastAll(downRight, maxdistance, layermask);
        goToRcastAllV2(downRightHit, myPos, mindistance, allPosCollide);
        //hit down Left
        RaycastHit[] downLeftHit = Physics.RaycastAll(downLeft, maxdistance, layermask);
        goToRcastAllV2(downLeftHit, myPos,mindistance, allPosCollide);
        rotation = getGotoRotationv2(myPos, allPosCollide);
        //rotation = 0;
        if (rotation != 0.0){ return rotation; }
        else { return rotate; }
    }
    private float GoToBoidRcastv1(float rotate)
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
        Ray downLeft = new Ray(myPos, -fright);
        Ray downRight = new Ray(myPos, -fleft);
        // Init int
        int fr = 0;
        int ri = 0;
        int le = 0;
        int fri = 0;
        int fle = 0;
        int dri = 0;
        int dle = 0;
        // Draw Ray

        Debug.DrawRay(myPos, transform.forward);
        Debug.DrawRay(myPos, transform.right);
        Debug.DrawRay(myPos, -transform.right);
        Debug.DrawRay(myPos, fright);
        Debug.DrawRay(myPos, fleft);
        Debug.DrawRay(myPos, -fright, Color.red);
        Debug.DrawRay(myPos, -fleft, Color.blue);
        //
        float maxdistance = 1.1f;
        int layerWall = 6;
        LayerMask layermask = 1 << layerWall;
        float rotation = 0.0f;
        bool hit = false;

        //hit front
        RaycastHit[] frontHit = Physics.RaycastAll(front, maxdistance, layermask);
        if (!hit)
        {
            fr = goToRcastAll(frontHit, myPos);
            if (fr == 1)
            {
                hit = true;
            }
        }
        //hit front Right
        RaycastHit[] frontRightHit = Physics.RaycastAll(FrontRight, maxdistance, layermask);
        fri = goToRcastAll(frontRightHit, myPos);
        if (!hit)
        {
            fri = goToRcastAll(frontRightHit, myPos);
            if (fri == 1)
            {
                hit = true;
            }
        }
        //hit front Left
        RaycastHit[] frontLeftHit = Physics.RaycastAll(FrontLeft, maxdistance, layermask);
        fle = goToRcastAll(frontLeftHit, myPos);
        if (!hit)
        {
            fle = goToRcastAll(frontLeftHit, myPos);
            if (fle == 1)
            {
                hit = true;
            }
        }

        //hit left
        RaycastHit[] leftHit = Physics.RaycastAll(left, maxdistance, layermask);
        le = goToRcastAll(leftHit, myPos);
        if (!hit)
        {
            le = goToRcastAll(leftHit, myPos);
            if (le == 1)
            {
                hit = true;
            }
        }
        //hit right
        RaycastHit[] rightHit = Physics.RaycastAll(right, maxdistance, layermask);
        ri = goToRcastAll(rightHit, myPos);
        if (!hit)
        {
            ri = goToRcastAll(rightHit, myPos);
            if (ri == 1)
            {
                hit = true;
            }
        }

        //hit down Right
        RaycastHit[] downRightHit = Physics.RaycastAll(downRight, maxdistance, layermask);
        if (!hit)
        {
            dri = goToRcastAll(downRightHit, myPos);
            if (dri == 1)
            {
                hit = true;
            }
        }
        //hit down Left
        RaycastHit[] downLeftHit = Physics.RaycastAll(downLeft, maxdistance, layermask);
        if (!hit)
        {
            dle = goToRcastAll(downLeftHit, myPos);
            if (dle == 1)
            {
                hit = true;
            }
        }

        rotation = 0 * fr - 2 * fle + 2 * fri; //- 10 * le + 10 * ri - 20 * dle + 20 * dri;
        //rotation = 0;
        if (rotation != 0.0)
        {
            setGoto(true);
            return rotation;
        }
        else
        {
            return rotate;
        }
    }
    // GET DESTINATION
    private Vector3 getDestinationCluster(Vector3 myPos, List<Vector3> allPosCollide)
    {
        if (allPosCollide.Count < 4)
        {
            return getDestination(myPos, allPosCollide);
        }
        else
        {
            // Building different groups
            List<List<Vector3>> clusters = new List<List<Vector3>>();
            List<Vector3> cluster = new List<Vector3>();
            foreach (Vector3 v in allPosCollide)
            {
                if (cluster.Count == 4)
                {
                    clusters.Add(cluster);
                    cluster = new List<Vector3>();
                }
                cluster.Add(v);
                
            }
            if (!clusters.Contains(cluster))
            {
                clusters.Add(cluster);
            }
            // Getting subPoints
            List<Vector3> subGresults = new List<Vector3>();
            foreach (List<Vector3> subGroup in clusters)
            {
                subGresults.Add(getDestination(myPos, subGroup));
            }
            return getDestination(myPos, subGresults);
        }
    }
    private Vector3 getOppositeDestinationCluster(Vector3 myPos, List<Vector3> allPosCollide)
    {
        if (allPosCollide.Count < 4)
        {
            return getOppositeDestination(myPos, allPosCollide);
        }
        else
        {
            // Building different groups
            List<List<Vector3>> clusters = new List<List<Vector3>>();
            List<Vector3> cluster = new List<Vector3>();
            foreach (Vector3 v in allPosCollide)
            {
                if (cluster.Count == 4)
                {
                    clusters.Add(cluster);
                    cluster = new List<Vector3>();
                }
                cluster.Add(v);

            }
            if (!clusters.Contains(cluster))
            {
                clusters.Add(cluster);
            }
            // Getting subPoints
            List<Vector3> subGresults = new List<Vector3>();
            foreach (List<Vector3> subGroup in clusters)
            {
                subGresults.Add(getOppositeDestination(myPos, subGroup));
            }
            return getOppositeDestination(myPos, subGresults);
        }
    }
    private Vector3 getDestination(Vector3 myPos,List<Vector3> allPosCollide)
    {
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;
        
        foreach (Vector3 v in allPosCollide)
        {
            //if (withDEBUG) { Debug.DrawLine(myPos, v); }
            x += v.x;
            y += v.y;
            z += v.z;
        }
        float cx = x / allPosCollide.Count;
        float cy = y / allPosCollide.Count;
        float cz = z / allPosCollide.Count;
        return new Vector3(cx, cy, cz);
    }
    private Vector3 getOppositeDestination(Vector3 myPos, List<Vector3> allPosCollide)
    {
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;

        foreach (Vector3 v in allPosCollide)
        {
            if (withDEBUG) {
                //Debug.Log("COLLIDED DISTANCE " + Vector3.Distance(myPos, v));
                Debug.DrawLine(myPos, v, Color.black); }
            x -= v.x;
            z -= v.z;
        }
        float cx = x / allPosCollide.Count;
        float cy = myPos.y;
        float cz = z / allPosCollide.Count;
        if (withDEBUG)
        {
            //Debug.Log("COLLIDED DISTANCE " + Vector3.Distance(myPos, v));
            Debug.DrawLine(myPos, new Vector3(cx, cy,cz), Color.black);
        }
        return new Vector3(cx, cy, cz);
    }
    // GET ANGLES
    private float getAngleTowards(Vector3 myPos, Vector3 myDest)
    {
        Vector3 centerPos = myDest;
        Vector3 localCpos = rb.transform.InverseTransformPoint(centerPos);
        bool isFront = false;
        bool isRight = false;
        // Determining case
        if (localCpos.z > 0) { isFront = true; }
        if (localCpos.x > 0) { isRight = true; }
        // Calculating angles
        float lAngle1 = Vector3.Angle(myPos, localCpos);
        float lAngle5 = Vector3.Angle(myPos + transform.forward, localCpos);
        float slAngle1 = Vector3.SignedAngle(myPos, localCpos, Vector3.up);
        float slAngle5 = Vector3.SignedAngle(myPos + transform.forward, localCpos, Vector3.up);
        if (withDEBUG)
        {
            //Debug.DrawLine(myPos, centerPos, Color.green);
            //Debug.DrawLine(myPos, myPos + transform.forward, Color.cyan);
            //Debug.Log("ABPOS/CPOS " + lAngle1 + " FOR/CPOS " + lAngle5);
            //Debug.Log("SV : ABPOS/CPOS " + slAngle1 + " FOR/CPOS " + slAngle5);
        }

        // Finding Right Angle -- lot of twiking but seems to work
        if (isFront)
        {
            if (isRight)
            {
                //if (withDEBUG) { Debug.Log("FR :" + Math.Max(lAngle1, lAngle5)); }
                return Math.Max(lAngle1, lAngle5);
            }
            else
            {
                //if (withDEBUG) { Debug.Log("FL :" + Math.Max(lAngle1, lAngle5)); }
                return -Math.Max(lAngle1, lAngle5);
            }
        }
        /////////////////////////////////////
        else
        {
            if (isRight)
            {
                //if (withDEBUG) { Debug.Log("DR :" + lAngle1); }
                return 180 - lAngle1;
            }
            else
            {
                //if (withDEBUG) { Debug.Log("DL " + slAngle1); }
                return -(180 - slAngle1);
            }
        }
    }
    private Vector3 getOppositeVector(Vector3 vector)
    {
        Vector3 loppo = rb.transform.InverseTransformPoint(vector);

        Vector3 test2 = new Vector3(-loppo.x, vector.y, -loppo.z);
        test2 = rb.transform.TransformPoint(test2);
        test2.y = vector.y;
        return test2;
    }
        // AVOID ROTATION
    private float getAvoidRotation(Vector3 myPos, List<Vector3> allPosCollide)
    {
        if (allPosCollide.Count == 0){ return 0.0f; }
        List<Vector3> trial = new List<Vector3>();
        foreach (Vector3 v in allPosCollide)
        {
            trial.Add(rb.transform.InverseTransformPoint(v));
        }
        Vector3 oppoDest = getDestinationCluster(myPos, allPosCollide);
        Vector3 opDest = getOppositeVector(oppoDest);
        float angle= getAngleTowards(myPos, opDest);
        if (withDEBUG) {
            //Debug.Log("OPPO DEST IS " + oppoDest);
            //Debug.Log("ANGLE AVOID " + angle+" "+ angle2);
            //Debug.DrawLine(myPos, oppoDest, Color.magenta);
            Debug.DrawLine(myPos, opDest, Color.yellow);
        }
        return angle;
    }
    private float getAvoidRotationv2(Vector3 myPos, Vector3 closest)
    {
        Vector3 loc= rb.transform.InverseTransformPoint(closest);
        bool isRight = false;

        if(loc.x > 0) { isRight = true; }

        if (isRight) { return -20; }
        else { return 20; }
    }
        // ATTRACTION ROTATION
    private float getGotoRotationv2(Vector3 myPos, List<Vector3> allPosCollide)
    {

        if (allPosCollide.Count == 0)
        {
            return 0.0f;
        }
        Vector3 centerPos = getDestinationCluster(myPos, allPosCollide);
        if (withDEBUG) { Debug.DrawLine(myPos, centerPos, Color.green); }
        return getAngleTowards(myPos, centerPos);
    }
    private float getGotoRotation(Vector3 myPos, List<Vector3> allPosCollide)
    {
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;
        if (allPosCollide.Count == 0)
        {
            return 0.0f;
        }
        foreach (Vector3 v in allPosCollide)
        {
            x += v.x;
            y += v.y;
            z += v.z;
        }
        float cx = x / allPosCollide.Count;
        float cy = y / allPosCollide.Count;
        float cz = z / allPosCollide.Count;

        Vector3 centerPos = new Vector3(cx, cy, cz);
        Vector3 lpos = rb.transform.InverseTransformPoint(myPos);
        var po = rb.transform.rotation;
        Vector3 localCpos = rb.transform.InverseTransformPoint(centerPos);
        Vector3 reU = po.eulerAngles;
        float testZ = localCpos.z;
        float testX = localCpos.x;
        bool isFront = false;
        bool isRight = false;
        bool isBack = false;
        bool isLeft = false;
        if (testZ > 0){ isFront = true; }
        if (testX > 0){ isRight = true; }
        if (testZ < 0){ isBack = true; }
        if (testX < 0){ isLeft = true; }
        /*
        Debug.DrawLine(myPos, myPos + rb.transform.right, Color.red);
        Debug.DrawLine(myPos, myPos + transform.right, Color.magenta);
        Debug.DrawLine(myPos + rb.transform.right, centerPos, Color.red);
        Debug.DrawLine(myPos, centerPos, Color.green);
        Debug.DrawLine(myPos, myPos + transform.forward, Color.blue);
        Debug.DrawLine(myPos + transform.forward, centerPos, Color.blue);
        Debug.Log("MY POS IS =================== " + myPos);
        Debug.Log("MY LOCAL POS IS =================== " + lpos);
        Debug.Log("M ROTATE IS " + po);
        Debug.Log("CENTER POS IS " + centerPos);
        Debug.Log("LOCAL CENTER POS IS " + localCpos);
        */
        //////////////////   ABSOLUTE ANGLES   /////////////////////////////
        float angle = Vector3.Angle(myPos, centerPos);
        float angle2 = Vector3.Angle(centerPos, myPos + transform.right);
        float angle3 = Vector3.Angle(centerPos, myPos + transform.forward);
        float angle4 = Vector3.Angle(transform.right, centerPos);
        float angle5 = Vector3.Angle(transform.forward, centerPos);
        float sAngle1 = Vector3.SignedAngle(myPos, centerPos, Vector3.up);
        float sAngle2 = Vector3.SignedAngle(myPos + transform.right, centerPos, Vector3.up);
        float sAngle3 = Vector3.SignedAngle(myPos + transform.forward, centerPos, Vector3.up);
        float sAngle4 = Vector3.SignedAngle(transform.right, centerPos, Vector3.up);
        float sAngle5 = Vector3.SignedAngle(transform.forward, centerPos, Vector3.up);

        /*
        Debug.Log("ANGLE ARE MPOS/CENTER " + angle + " MPOS+R/CENTER = " + angle2 + " MPOS+F/CENTER = " + angle3);
        Debug.Log("ANGLE : RIGHT/CENTER " + angle4 + " FORWARD/CENTER " + angle5);
        Debug.Log("THE SIGNED ANGLE ARE MPOS/CENTER " + sAngle1 + " MPOS+R/CENTER = " + sAngle2 + " MPOS+F/CENTER = " + sAngle3);
        Debug.Log("THE SIGNED ANGLE : RIGHT/CENTER " + sAngle4 + " FORWARD/CENTER " + sAngle5);
        Debug.Log("-----------------------------------------------------------------------------");
        */
        //////////////////  LOCAL ANGLES    /////////////////////////////////
        float lAngle1 = Vector3.Angle(myPos, localCpos);
        float lAngle2 = Vector3.Angle(localCpos, myPos + transform.right);
        float lAngle3 = Vector3.Angle(localCpos, myPos + transform.forward);
        float lAngle4 = Vector3.Angle(transform.right, localCpos);
        float lAngle5 = Vector3.Angle(transform.forward, localCpos);
        float slAngle1 = Vector3.SignedAngle(myPos, localCpos, Vector3.up);
        float slAngle2 = Vector3.SignedAngle(localCpos, myPos + transform.right, Vector3.up);
        float slAngle3 = Vector3.SignedAngle(localCpos, myPos + transform.forward, Vector3.up);
        float slAngle4 = Vector3.SignedAngle(transform.right, localCpos, Vector3.up);
        float slAngle5 = Vector3.SignedAngle(transform.forward, localCpos, Vector3.up);
        float sAngle7 = Vector3.SignedAngle(myPos + transform.forward, Vector3.up, Vector3.up);
        /*
        Debug.Log("LOCAL ANGLE ARE MPOS/CENTER " + lAngle1 + " MPOS+R/CENTER = " + lAngle2 + " MPOS+F/CENTER = " + lAngle3);
        Debug.Log("LOCAL ANGLE : RIGHT/CENTER " + lAngle4 + " FORWARD/CENTER " + lAngle5);
        Debug.Log("LOCAL SIGNED ANGLE ARE MPOS/CENTER " + lAngle1 + " MPOS+R/CENTER = " + lAngle2 + " MPOS+F/CENTER = " + lAngle3);
        Debug.Log("LOCAL SIGNED ANGLE : RIGHT/CENTER " + lAngle4 + " FORWARD/CENTER " + lAngle5);
        Debug.Log("LOCAL SIGNED ANGLE : MPOS+FOR " + sAngle2);
        */
        //----------------------------------------------------------------------------------------------------------//
        //Debug.Log(" ISLeft = " + isLeft + " IS RIGHT = " + isRight + " IS FRONT =" + isFront + " IS BACK =" + isBack);
        if ((isFront) && (isLeft))
        {
            //Debug.Log("-------------------- slANGLE4");
            if ((-5 < reU.y) && (reU.y < 5)) { return sAngle4; }
            if ((-5 < reU.y) && (reU.y < 5)) { return -sAngle4; }
            //return sAngle4; 
        }
        if ((isFront) && (isRight))
        {
            Debug.Log("-------------------- slANGLE1");

            return slAngle1;
        }
        if ((isBack) && (isRight))
        {
            //Debug.Log("-------------------- sANGLE5");
            if (angle4 < 90) { return -slAngle5; }
            else { return slAngle5; }
        }
        if ((isBack) && (isLeft)) { return 180; }
        if (sAngle1 == float.NaN){ return 0.0f; }
        //float rep;
        //rep = Math.Max(sAngle1, sAngle2);
        return 0.0F;
    }
        // COHESION ROTATION
    private float getCohesionRotation(Vector3 myPos, List<Vector3> allPosCollide, List<Transform> allTransform)
    {
        if (allPosCollide.Count == 0) { return 0.0f; }
        
        List<Vector3> theirForward = new List<Vector3>();
        for (int i=0; i < allPosCollide.Count; i++)
        {
            Vector3 pos = allPosCollide[i];
            Transform hisTransform = allTransform[i];
            Vector3 forw = pos + hisTransform.forward;
            if (withDEBUG) { 
                Debug.DrawLine(pos, forw, Color.blue); 
            }
            theirForward.Add(forw);
        }
        Vector3 mine = myPos + transform.forward;
        if (withDEBUG)
        {
            Debug.DrawLine(myPos, mine, Color.blue);
        }
        //theirForward.Add(mine);
        Vector3 dest = getDestinationCluster(myPos, theirForward);
        if (withDEBUG) { 
            Debug.DrawLine(myPos, dest, Color.magenta);
        }

        //return 0.0f;
        return getAngleTowards(myPos, dest);

        
    }

    // CHECK COLLISIONS 
    private int collidedRcastAll(RaycastHit[] tab, Vector3 myPos, float maxDistance)
    {
        // Start to 1 to ignore self collision
        for (int i = 1; i < tab.Length; i++)
        {
            RaycastHit hitInfo = tab[i];
            //Debug.Log("DISTANCE HIT["+i+"] = "+hitInfo.distance);
            if ((hitInfo.transform.position != myPos) && (hitInfo.distance > tab[0].distance) && (hitInfo.distance < maxDistance))
            {
                //Debug.Log("DISTANCE COLLISION WITH+"+ i+" IS " + hitInfo.distance);
                //Debug.DrawLine(myPos, hitInfo.transform.position, Color.magenta);
                return 1;
            }
        }
        return 0;
    }
    private void collidedRcastAllV2(RaycastHit[] tab, Vector3 myPos, float maxDistance, List<Vector3> allCollide)
    {
        for (int i = 1; i < tab.Length; i++)
        {
            RaycastHit hitInfo = tab[i];
            float dist = Vector3.Distance(myPos,hitInfo.transform.position);
            if ((hitInfo.transform.position != myPos) && ( dist> tab[0].distance) && (dist < maxDistance) && (!allCollide.Contains(hitInfo.transform.position)))
            {
                allCollide.Add(hitInfo.transform.position);
            }
        }
    }
    private Vector3 collidedRcastAllv3(RaycastHit[] tab, Vector3 myPos, float maxDistance, Vector3 closest) {
        for (int i = 1; i < tab.Length; i++)
        {
            RaycastHit hitInfo = tab[i];
            float dist = Vector3.Distance(myPos, hitInfo.transform.position);
            if ((hitInfo.transform.position != myPos) && (dist > tab[0].distance) && (dist < maxDistance))
            {
                if (closest == Vector3.zero)
                {
                    closest = hitInfo.transform.position;
                }
                else
                {
                    float minD = Vector3.Distance(myPos, closest);
                    if (minD > dist)
                    {
                        return hitInfo.transform.position;
                    }
                }
            }
        }
        return closest;
    }
        // VARIANTE WITH DISTANCE FIXED
    private int collidedRcastAllv4(RaycastHit[] tab, Vector3 myPos, float maxDistance)
    {
        // Start to 1 to ignore self collision
        for (int i = 1; i < tab.Length; i++)
        {
            RaycastHit hitInfo = tab[i];
            float dist = Vector3.Distance(myPos, hitInfo.transform.position);
            //Debug.Log("DISTANCE HIT["+i+"] = "+hitInfo.distance);
            if ((hitInfo.transform.position != myPos) && (dist > tab[0].distance) && (dist < maxDistance))
            {
                //Debug.Log("DISTANCE COLLISION WITH+"+ i+" IS " + hitInfo.distance);
                Debug.DrawLine(myPos, hitInfo.transform.position, Color.yellow);
                return 1;
            }
        }
        return 0;
    }
    private void collidedCohesionRcastAll(RaycastHit[] tab, Vector3 myPos, List<Vector3> allCollide, List<Transform> allTransformsCollide,float mindistance, float maxdistance)
    {
        for (int i = 1; i < tab.Length; i++)
        {
            RaycastHit hitInfo = tab[i];
            float dist = Vector3.Distance(myPos, hitInfo.transform.position);
            if ((hitInfo.transform.position != myPos) && (dist >= mindistance) && (dist < maxdistance) && (! allCollide.Contains(hitInfo.transform.position)))
            {
                allCollide.Add(hitInfo.transform.position);
                allTransformsCollide.Add(hitInfo.transform);
            }
        }
    } 
    private int goToRcastAll(RaycastHit[] tab, Vector3 myPos)
    {
        for (int i = 0; i < tab.Length; i++)
        {
            RaycastHit hitInfo = tab[i];
            if (hitInfo.transform.position != myPos)
            {
                if (hitInfo.distance > 1)
                {
                    Debug.DrawLine(myPos, hitInfo.transform.position,Color.cyan);
                    return 1;
                } 
            }
        }
        return 0;
    }
    private void goToRcastAllV2(RaycastHit[] tab, Vector3 myPos,float minDistance, List<Vector3> allCollide)
    {
        for (int i = 0; i < tab.Length; i++)
        {
            RaycastHit hitInfo = tab[i];
            float dist = Vector3.Distance(myPos, hitInfo.transform.position);
            if ((hitInfo.transform.position != myPos) && (dist >= minDistance) && (!allCollide.Contains(hitInfo.transform.position)))
            {
                allCollide.Add(hitInfo.transform.position);
            }
        }
    }


    // SETTERS 
    private void setGoto(bool v)
    {
        goTo = v;
    }
    private void setFrein(bool value)
    {
        frein = value;
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

    //Ground
    public void IsGrounded()
    {
        //Debug.Log("GROUND SIZE= " + groundCollider.Count);
        if (groundCollider.Count != 0){
           //Debug.Log("is Grounded");
           
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
            //Debug.Log("The layer is " + test);
            if (test == LayerMask.NameToLayer("SOL"))
            {
                //Debug.Log("GROUND ADDED");
                groundCollider.Add(toAdd);
                return;
            }
            if (test == LayerMask.NameToLayer("MUR"))
            {
                //Debug.Log("WALL ADDED");
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