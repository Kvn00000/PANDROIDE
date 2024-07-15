using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class boidTuning : MonoBehaviour
{
    //Attributs private
    private Rigidbody rb;
    private List<Collider> groundCollider = new List<Collider>();
    private bool grounded = false;
    private Transform _myTransform;
    
    
    //Attributs public

    //Booleans that indicates which behaviour is used --> set in the prefab of the boid
    public bool withGoto = false;
    public bool withCohesion = false;
    public bool withAvoid = false;
    public bool withDEBUG = false;
    //
    public float speed;
    public float wallRay;
    public float avoidRay;
    public float cohesionRay;
    public float attractionRay;
    public float filter;

    /*
    Init and Start function --> Initialize boids parameters 
    */
    public void Init(float _speed,float _wallRay, float _avoidRay, float _cohesionRay, float _attractionRay, float _filter){
        speed =_speed;
        wallRay = _wallRay;
        avoidRay = _avoidRay;
        cohesionRay = _cohesionRay;
        attractionRay = _attractionRay;
        filter = _filter;
        //Debug.Log("Wall R : " + wallRay + "  Cohesion R : " + cohesionRay + "  Attraction R : " + attractionRay);
        rb = GetComponent<Rigidbody>();
        //step = 0;
        
    }
    
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        _myTransform = this.transform;
    }
    //Update is called once per frame
    void Update()
    {
        /*
       Boid avec Raycast --> le plus avanc� meme si je comprend pas pourquoi �a marche pas 
       */
        
        //If not on the ground apply gravity
        
        // Allow Boids to fall on the ground with the groundGestion on the bottom face
        Quaternion newRota= new Quaternion(0, _myTransform.rotation.y, 0, _myTransform.rotation.w);
        this.transform.rotation = newRota;

        //if Boid is not on the ground add custom gravity
        if (!grounded)
        {
            rb.AddForce(Physics.gravity*0.5f, ForceMode.Acceleration);
        }
        else
        {
            HandleBoidsBehaviours();
        }
    }

    private void LateUpdate()
    {
        // Check if boid is on the ground
        IsGrounded();

    }

    private void HandleBoidsBehaviours()
    {
        String modeUsed = "";
        if (withDEBUG) { Debug.Log("//////////////////////////////////////////////////////////////////////////////////"); }

        float rotation = 0.0f;
        float oldrotate;
        /*
        Go through each behaviour in the following order :
        Attraction Behaviour --> Cohesion Behaviour --> Avoid Boids Behaviour --> Avoid wall Behaviour
        */
        if ((withGoto))
        {
            rotation = GoToBoidRcastv3(rotation, cohesionRay, attractionRay);
            if ((rotation != 0.0f))
            {
                if (withDEBUG) 
                {
                    modeUsed = "GoTo Green Line";
                    Debug.Log("ATTRACTION ACTIVATED : " + rotation); 
                }
            }
        }
        if ((withCohesion))
        {
            oldrotate = rotation;
            rotation = CohesionBoidRcast(rotation, avoidRay, cohesionRay);
            //Check if old value need to be overwritten 
            if ((rotation != oldrotate))
            {
                if (withDEBUG) 
                { 
                    modeUsed = "Cohesion blue and pink line";
                    Debug.Log("COHESION ACTIVATED : " + rotation); 
                }
            }
        }
        if ((withAvoid))
        {
            oldrotate = rotation;
            rotation = AvoidBoidRcastv3(rotation, avoidRay);
            //Check if old value need to be overwritten 
            if ((rotation != oldrotate))
            {
                if (withDEBUG) 
                {
                    modeUsed = "Avoid yellow line";
                    Debug.Log("AVOIDANCE ACTIVATED : " + rotation); 
                }
            }
        }
        //Highest Priority
        oldrotate = rotation;
        rotation = AvoidWallRcastv4(rotation, wallRay);

        if ((rotation != oldrotate))
        {
            modeUsed = "Wall red";
            if ((withDEBUG)){ Debug.Log("WALL AVOID ACTIVATED : " + rotation); }
        }
        if ((withDEBUG)) { Debug.Log("FINAL ROTATION : " + rotation + " " + modeUsed); }
        // APPLY ROTATION
        if (rotation == 0.0)
        {
            StopBoidDrift();
            rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Force);
        }
        else
        {
            //multiply by Time.deltaTime to make the rotation more smooth
            // 
            float angle = rotation * Time.deltaTime;
            if (withDEBUG)
            {
                Debug.Log(" FINAL VALUE OF ANGLE AFTER TIME " + angle);
            }
            angle = filterAngle(angle);
            if (withDEBUG)
            {
                Debug.Log(" FINAL VALUE OF rotation after filter " + angle);
            }
            _myTransform.Rotate(Vector3.up, angle);
            StopBoidDrift();
            rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Force);

        }
        if (withDEBUG)
        {
            //Debug.Log("VELOCITY IS " + rb.velocity);
            Debug.Log("//////////////////////////////////////////////////////////////////////////////////");
        }
    }
    private void StopBoidDrift()
    {
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
        rb.inertiaTensor = Vector3.zero;
        rb.inertiaTensorRotation = Quaternion.identity;
    }
    private float filterAngle( float angle)
    {
        float newAngle = angle;
        if (newAngle > filter) { newAngle = filter; }
        if (newAngle < -filter) { newAngle = -filter; }
        return newAngle;
    } 
    
    /*
    ------------------- BEHAVIOURS --------------------------------
    will usually follow these steps :
     Intialize the rays --> collect collision detections infos --> get the angle --> check if the returned value needs to be overwritten
    */
      // AVOID WALL BEHAVIOUR
    private float AvoidWallRcastv4(float rotate, float wallRay)
    {
        /*
        Comportement pour eviter les murs avec une distance max de wallRay
        Rotation ==> premier d�tect� renvoie la valeur
        */
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
        // Draw Ray
        Ray fleftRay = new Ray(myPos, fleft);
        Ray frightRay = new Ray(myPos, fright);
        //Debug.DrawRay(myPos,transform.forward);
        //Debug.DrawRay(myPos, transform.right);
        //Debug.DrawRay(myPos, -transform.right);
        //Debug.DrawRay(myPos, fright);
        //Debug.DrawRay(myPos, fleft);
        //
        float maxdistance = wallRay;
        int layerWall = 8;
        LayerMask layermask = 1 << layerWall;
        RaycastHit info;
        // Init distance floats
        List<float> distanceList = new List<float>();
        float[] toRotate = { 60f, 59f, -55f, 35f, -35f };
        //hit front
        if (Physics.Raycast(front, out info, maxdistance, layermask))
        {
            //Debug.Log("HIT FRONT");
            //float dis = Vector3.Distance(myPos + f, info.transform.position);
            float dis = Vector3.Distance(myPos, info.point);
            if (withDEBUG)
            {
                //Debug.Log(" F Collision at " + info.point + " with distance of " +dis);
                //Debug.DrawLine(myPos, myPos + f, Color.red);
            }
            //return 50;
            distanceList.Add(dis);
        }
        else
        {
            distanceList.Add(8000);
        }
        // Hit FL
        if (Physics.Raycast(fleftRay,out info, maxdistance, layermask))
        {
            //Debug.Log("HIT FRONT");
            //float dis = Vector3.Distance(myPos+fleft, info.transform.position);
            float dis = Vector3.Distance(myPos, info.point);
            if (withDEBUG)
            {
                //Debug.Log(" FL Collision at " + info.point + " with distance of " + dis);
                //Debug.DrawLine(myPos, myPos +fleft, Color.red);
            }
            //return 45;
            distanceList.Add(dis);
        }
        else
        {
            distanceList.Add(8000);
        }
        // Hit FR
        if (Physics.Raycast(frightRay,out info, maxdistance, layermask))
        {
            //float dis = Vector3.Distance(myPos+fright, info.transform.position);
            float dis = Vector3.Distance(myPos, info.point);
            if (withDEBUG)
            {
                //Debug.Log("FR Collision at " + info.point + " with distance of " + dis);
                //Debug.DrawLine(myPos, myPos + fright, Color.red);
            }
            //return -45;
            distanceList.Add(dis);
        }
        else
        {
            distanceList.Add(8000);
        }
        //hit left
        if (Physics.Raycast(left,out info, maxdistance, layermask))
        {
            //Debug.Log("HIT LEFT");
            //float dis = Vector3.Distance(myPos-r, info.transform.position);
            float dis = Vector3.Distance(myPos, info.point);
            if (withDEBUG) {
                //Debug.Log("L Collision at " + info.point + " with distance of " + dis);
                //Debug.DrawLine(myPos, myPos - r, Color.red); 
            }
            //return 25;
            distanceList.Add(dis);
        }
        else
        {
            distanceList.Add(8000);
        }
        //hit right
        if ((Physics.Raycast(right,out info, maxdistance, layermask)))
        {
            //Debug.Log("HIT Right");
            //float dis = Vector3.Distance(myPos+r, info.transform.position);
            float dis = Vector3.Distance(myPos, info.point);
            if (withDEBUG) {
                //Debug.Log(" R Collision at " + info.point + " with distance of " + dis);
                //Debug.DrawLine(myPos, myPos + r, Color.red); 
            }
            //return -25;
            distanceList.Add(dis);
        }
        else
        {
            distanceList.Add(8000);
        }
        if (withDEBUG)
        {
            //Debug.Log("COUNT " + distanceList.Count);
            foreach(float distance in distanceList)
            {
                //Debug.Log(" Value found in disList " + distance);
            }
        }
        // TO-DO : Traitement distance
        float min=300;
        int minidx = -1;
        for (int i =0;i<distanceList.Count;i++)
        {
            float distance = distanceList[i];
            if (distance < min) {
                min = distance;
                minidx = i;
            }
        }
        if (min == 300) { return rotate; }
        return toRotate[minidx];
    }
      // REPULSION BEHAVIOUR
    private float AvoidBoidRcastv3(float rotate, float avoidRay)
    {
        /*
       Comportement pour eviter le boid le plus proche
       Rotation ==> valeur fixe donne par la fonction
       */
        //Init Ray
        Vector3 myPos = rb.transform.position;

        Ray front = new Ray(myPos, transform.forward);
        Ray right = new Ray(myPos, transform.right);
        Ray left = new Ray(myPos, -transform.right);
        // Build Other Ray
        Vector3 f = transform.forward;
        Vector3 r = transform.right;
        Vector3 fright = new Vector3(f.x + r.x, f.y, f.z + r.z);
        Vector3 fleft = new Vector3(f.x - r.x, f.y + r.y, f.z - r.z);
        Ray FrontRight = new Ray(myPos, fright);
        Ray FrontLeft = new Ray(myPos, fleft);
        Ray downLeft = new Ray(myPos, -fright);
        Ray downRight = new Ray(myPos, -fleft);
        // Init Collision detection parameters
        float maxdistance = 0.8f; // To be sure that it will collide with something
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
            rotation = 0.0f;
        }
        else
        {
            if (withDEBUG) { Debug.DrawLine(myPos, closestBuddy, Color.yellow); }
            rotation = getAvoidRotationv2(myPos, closestBuddy);
        }

        if (rotation != 0.0) { return rotation; }
        else { return rotate; }
    }
      // COHESION BEHAVIOUR
    private float CohesionBoidRcast(float rotate, float minRay, float maxRay)
    {
        /*
        Comportement s'alligner avec tous les d�tect�s
        Rotation ==> angle entre la destination moyenne et le forward
        */
        //Init Ray
        Vector3 myPos = rb.transform.position;
        Vector3 mPos = this.GetComponent<BoxCollider>().center;
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
        //
        /*
        if (withDEBUG)
        {
            Debug.DrawRay(myPos, transform.forward,Color.cyan);
            Debug.DrawRay(myPos, transform.right, Color.cyan);
            Debug.DrawRay(myPos, -transform.right, Color.cyan);
            Debug.DrawRay(myPos, fright, Color.cyan);
            Debug.DrawRay(myPos, fleft, Color.cyan);
            Debug.DrawRay(myPos, -fright, Color.cyan);
            Debug.DrawRay(myPos, - fleft, Color.cyan);

        }
        */
        // Init Collision detection parameters
        float maxdistance = maxRay;
        float mindistance = minRay;
        //Setting LayerMask for collision detection
        int layerBoid = LayerMask.NameToLayer("BOID");
        LayerMask layermask = 1 << layerBoid;
        //Getting all collisions
        List<Vector3> allPosCollide = new List<Vector3>();
        List<Transform> allTransformsCollide = new List<Transform>();

        //hit left
        RaycastHit[] leftHit = Physics.RaycastAll(left, maxdistance, layermask);
        //if (withDEBUG){ Debug.Log("Left tab : " + leftHit.Length); }
        collidedCohesionRcastAll(leftHit, myPos, allPosCollide, allTransformsCollide, mindistance, maxdistance);
        //hit right
        RaycastHit[] rightHit = Physics.RaycastAll(right, maxdistance, layermask);
        //if (withDEBUG){ Debug.Log("Right tab : " + rightHit.Length); }
        collidedCohesionRcastAll(rightHit, myPos, allPosCollide, allTransformsCollide, mindistance, maxdistance);
        //hit front
        RaycastHit[] frontHit = Physics.RaycastAll(front, maxdistance, layermask);
        //if (withDEBUG){ Debug.Log("Front tab : " + frontHit.Length); }
        collidedCohesionRcastAll(frontHit, myPos, allPosCollide, allTransformsCollide, mindistance, maxdistance);
        //hit front Right
        RaycastHit[] frontRightHit = Physics.RaycastAll(FrontRight, maxdistance, layermask);
        //if (withDEBUG){ Debug.Log("Front Right tab : " + frontRightHit.Length); }
        collidedCohesionRcastAll(frontRightHit, myPos, allPosCollide, allTransformsCollide, mindistance, maxdistance);
        //hit front Left
        RaycastHit[] frontLeftHit = Physics.RaycastAll(FrontLeft, maxdistance, layermask);
        collidedCohesionRcastAll(frontLeftHit, myPos, allPosCollide, allTransformsCollide, mindistance, maxdistance);
        //if (withDEBUG){ Debug.Log("Front Left tab : " + frontLeftHit.Length); }
        //hit down Right
        RaycastHit[] downRightHit = Physics.RaycastAll(downRight, maxdistance, layermask);
        //if (withDEBUG){ Debug.Log("Down Right tab : " + downRightHit.Length); }
        collidedCohesionRcastAll(downRightHit, myPos, allPosCollide, allTransformsCollide, mindistance, maxdistance);
        //hit down Left
        RaycastHit[] downLeftHit = Physics.RaycastAll(downLeft, maxdistance, layermask);
        //if (withDEBUG){ Debug.Log("Down Left tab : " + downLeftHit.Length); }
        collidedCohesionRcastAll(downLeftHit, myPos, allPosCollide, allTransformsCollide, mindistance, maxdistance);


        //Getting rotation
        float rotation = 0.0f;
        //if (withDEBUG) { Debug.Log("Cohesions cibles ="+allPosCollide.Count); }
        rotation = getCohesionRotationV2(myPos, allPosCollide, allTransformsCollide);

        if (rotation != 0.0) { return rotation; }
        else { return rotate; }
    }
      // ATTRACTION BEHAVIOUR
    private float GoToBoidRcastv3(float rotate, float minRay, float maxRay)
    {
        /*
       Comportement pour approcher le plus proche
       Rotation ==> valeur fixe donne par la fonction
       */
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
            Debug.DrawRay(myPos, -fright,Color.cyan);
            Debug.DrawRay(myPos, -fleft,Color.red);
            */
        }
        //
        float maxdistance = maxRay;
        float mindistance = minRay;
        int layerWall = 6;
        LayerMask layermask = 1 << layerWall;
        float rotation=0.0f;
        Vector3 closestPoint = Vector3.zero;
        //hit front
        RaycastHit[] frontHit = Physics.RaycastAll(front, maxdistance, layermask);
        closestPoint = goToRcastAllV3(frontHit, myPos, mindistance,maxdistance, closestPoint);
        //if (withDEBUG){ Debug.Log("Count Front " + frontHit.Length); }
        //hit front Right
        RaycastHit[] frontRightHit = Physics.RaycastAll(FrontRight, maxdistance, layermask);
        closestPoint = goToRcastAllV3(frontRightHit, myPos, mindistance, maxdistance, closestPoint);
        //if (withDEBUG) { Debug.Log("Count FR " + frontRightHit.Length); }
        //hit front Left
        RaycastHit[] frontLeftHit = Physics.RaycastAll(FrontLeft, maxdistance, layermask);
        closestPoint = goToRcastAllV3(frontLeftHit, myPos, mindistance, maxdistance, closestPoint);
        //if (withDEBUG) { Debug.Log("Count FL " + frontLeftHit.Length); }
        //hit left
        RaycastHit[] leftHit = Physics.RaycastAll(left, maxdistance, layermask);
        closestPoint = goToRcastAllV3(leftHit, myPos, mindistance, maxdistance, closestPoint);
            
        //if (withDEBUG) { Debug.Log("Count left " + leftHit.Length); }
        //hit right
        RaycastHit[] rightHit = Physics.RaycastAll(right, maxdistance, layermask);
        closestPoint = goToRcastAllV3(rightHit, myPos, mindistance, maxdistance, closestPoint);
        //if (withDEBUG) { Debug.Log("Count right " + rightHit.Length); }
        //hit down Right
        RaycastHit[] downRightHit = Physics.RaycastAll(downRight, maxdistance, layermask);
        closestPoint = goToRcastAllV3(downRightHit, myPos, mindistance, maxdistance, closestPoint);
        //if (withDEBUG) { Debug.Log("Count Dright " + downRightHit.Length); }
        //hit down Left
        RaycastHit[] downLeftHit = Physics.RaycastAll(downLeft, maxdistance, layermask);
        closestPoint = goToRcastAllV3(downLeftHit, myPos, mindistance, maxdistance, closestPoint);
        //if (withDEBUG) { Debug.Log("Count Dleft " + downLeftHit.Length); }
        rotation = getGotoRotationv3(myPos, closestPoint);
        //rotation = 0;
        if (rotation != 0.0) { return rotation; }
        else { return rotate; }
    }

    /*
    ------------------- Rotations Functions --------------------------------
    */

    // functions to get rotation angles
    private float getAngleTowards(Vector3 myPos, Vector3 myDest)
    {
        // Function to get the angle from myPos towards myDest
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

        // Finding Right Angle -- lot of twiking but seems to work
        if (isFront)
        {
            if (isRight)
            {
                return Math.Max(lAngle1, lAngle5);
            }
            else
            {
                return -Math.Max(lAngle1, lAngle5);
            }
        }
        else
        {
            if (isRight)
            {
                return 180 - lAngle1;
            }
            else
            {
                return -(180 - slAngle1);
            }
        }
    }
        //AVOID ROTATION
    private float getAvoidRotationv2(Vector3 myPos, Vector3 closest)
    {
        // Get the rotation to avoid an other boid
        Vector3 loc = rb.transform.InverseTransformPoint(closest);
        bool isRight = false;
        if (loc.x > 0) { isRight = true; }
        if (isRight) {
            if (withDEBUG) { Debug.Log("RIGHT"); }
            if (loc.x < 0.3) { return -40; }
            return -20; 
        }
        else {
            if (withDEBUG) { Debug.Log("LEFT"); }
            if (loc.x > -0.3) { return 40; }
            return 20; 
        }
    }
        //COHESION ROTATION
    private float getCohesionRotationV2(Vector3 myPos, List<Vector3> allPosCollide, List<Transform> allTransform)
    {
        // get the angle needed to correct the trajectory of the boid
        if (allPosCollide.Count == 0) { return 0.0f; }
        float overAllCorrection = 0;
        List<Vector3> theirForward = new List<Vector3>();
        for (int i = 0; i < allPosCollide.Count; i++)
        {
            Vector3 pos = allPosCollide[i];
            Transform hisTransform = allTransform[i];
            //
            Vector3 hisForward = hisTransform.forward;
            Vector3 forw = pos + hisTransform.forward;
            Vector3 formyforward =myPos+hisTransform.forward;

            Vector3 newPos = new Vector3(myPos.x + hisForward.x, myPos.y, myPos.z + hisForward.z);
            float correction = getAngleTowards(myPos, newPos);
            if (withDEBUG)
            {
                //Debug.Log("Proposed correction is " + correction);
            }
            overAllCorrection += correction;
            if (withDEBUG)
            {
                Debug.DrawLine(pos, forw, Color.blue);
               
                //Debug.Log("My forward = " + this.transform.forward);
                //Debug.Log("His forward = " + hisTransform.forward);
                Debug.DrawLine(myPos, newPos, Color.magenta);
            }
            theirForward.Add(formyforward);

        }
        Vector3 mine = myPos + transform.forward;
        if (withDEBUG)
        {
            Debug.DrawLine(myPos, mine, Color.blue);
            //Debug.Log("OVERALL CORECTION IS " + overAllCorrection);
        }
        return overAllCorrection;

        //return 0.0f;

    }
        // ATTRACTION ROTATION
    private float getGotoRotationv3(Vector3 myPos, Vector3 closestPoint)
    {
        //get the angle toward the closest individual in attraction range
        if (closestPoint == Vector3.zero)
        {
            return 0.0f;
        }
        if (withDEBUG) { Debug.DrawLine(myPos, closestPoint, Color.green); }
        return getAngleTowards(myPos, closestPoint);
    }
    /*
    ------------------- Collision detection Functions --------------------------------
    */
    private Vector3 collidedRcastAllv3(RaycastHit[] tab, Vector3 myPos, float maxDistance, Vector3 closest)
    {
        for (int i = 1; i < tab.Length; i++)
        {
            RaycastHit hitInfo = tab[i];
            float dist = Vector3.Distance(myPos, hitInfo.transform.position);
            // Check if not colliding with self and if it's in range of avoid behaviour 
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
    private void collidedCohesionRcastAll(RaycastHit[] tab, Vector3 myPos, List<Vector3> allCollide, List<Transform> allTransformsCollide, float mindistance, float maxdistance)
    {
        for (int i = 1; i < tab.Length; i++)
        {
            //limit the number of individuals to consider to 10 --> try to save some performances
            if (allCollide.Count < 10)
            {
                RaycastHit hitInfo = tab[i];
                float dist = Vector3.Distance(myPos, hitInfo.transform.position);
                // Check if not colliding with self and if it's in range of cohesion behaviour and if the collided is not already detected
                if ((hitInfo.transform.position != myPos) && (dist >= mindistance) && (dist < maxdistance) && (!allCollide.Contains(hitInfo.transform.position)))
                {
                    allCollide.Add(hitInfo.transform.position);
                    allTransformsCollide.Add(hitInfo.transform);
                }
            }
            else
            {
                break;
            }
        }
    }
    private Vector3 goToRcastAllV3(RaycastHit[] tab, Vector3 myPos, float minDistance,float maxDistance, Vector3 closest)
    {
        for (int i = 0; i < tab.Length; i++)
        {
            RaycastHit hitInfo = tab[i];

            float dist = Vector3.Distance(myPos, hitInfo.transform.position);
            if (withDEBUG) {
                //Debug.Log("My pos is " + myPos);
                //Debug.Log("Hit at " + dist);
                //Debug.Log("Hit position at " + hitInfo.transform.position);
            }
            if ((hitInfo.transform.position != myPos) && (dist >= minDistance) && (dist<maxDistance))
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

    /*
    ------------------- Ground gestion Functions --------------------------------
    */
    public void IsGrounded()
    {
        //Check if the boid is on the ground or not

        //Debug.Log("GROUND SIZE= " + groundCollider.Count);
        if (groundCollider.Count != 0)
        {
            //Debug.Log("is Grounded");

            grounded = true;
        }
        else
        {
            //Debug.Log("is  NOT Grounded");
            grounded = false;
        }
    }
    //Toolbox to handle collison layer --> used to handle gravity
    public void AddCollider(Collider toAdd)
    {
        if (!CheckPresence(toAdd))
        {
            int test = toAdd.gameObject.layer;
            //Debug.Log("The layer is " + test);
            if ((test == LayerMask.NameToLayer("SOL"))||(test==LayerMask.NameToLayer("MUR")))
            {
                //Debug.Log("GROUND ADDED");
                groundCollider.Add(toAdd);
                return;
            }
        }
    }
    public void RemoveCollider(Collider toRemove, int layer)
    {
        if (CheckPresence(toRemove))
        {
            if ((layer == LayerMask.NameToLayer("SOL"))||(layer==LayerMask.NameToLayer("MUR")))
            {
                groundCollider.Remove(toRemove);
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
        return false;
    }
}
