using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class boidTuning : MonoBehaviour
{
    //Attributs private
    private Rigidbody rb;
    private List<Collider> groundCollider = new List<Collider>();
    private bool grounded = false;
    //Attributs publics
    public float speed = 200.0f;
    public bool withGoto = false;
    public bool withCohesion = false;
    public bool withAvoid = false;
    public bool withDEBUG = false;

    public float wallRay=0.6f;
    public float avoidRay = 0.6f;
    public float cohesionRay = 1.0f;
    public float attractionRay = 1.1f;
    public float filter = 5;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        /*
       Boid avec Raycast --> le plus avancé meme si je comprend pas pourquoi ça marche pas 
       */
        String modeUsed = "";
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
            float wallRay = this.wallRay;
            float avoidRay = this.avoidRay;
            float cohesionRay = this.cohesionRay;
            float attractionRay = this.attractionRay;
            //////////////////////////


            if ((withGoto))
            {
                rotation = GoToBoidRcastv3(rotation, cohesionRay, attractionRay);
                if ((rotation != 0.0f))
                {

                    modeUsed = "GoTo Green Line";
                    //if (withDEBUG) { Debug.Log("ATTRACTION ACTIVATED : " + rotation); }
                }
            }
            if ((withCohesion))
            {
                oldrotate = rotation;
                rotation = CohesionBoidRcast(rotation, avoidRay, cohesionRay);
                //permet de savoir si le comportement a remplace l'ancienne valeur
                if ((rotation != oldrotate))
                {
                    modeUsed = "Cohesion blue and pink line";
                    //if (withDEBUG) { Debug.Log("COHESION ACTIVATED : " + rotation); }
                }
            }
            if ((withAvoid))
            {
                oldrotate = rotation;
                rotation = AvoidBoidRcastv3(rotation, avoidRay);
                //permet de savoir si le comportement a remplace l'ancienne valeur
                if ((rotation != oldrotate))
                {
                    modeUsed = "Avoid yellow line";
                    //if (withDEBUG) { Debug.Log("AVOIDANCE ACTIVATED : " + rotation); }
                }
            }
            //Highest Priority
            oldrotate = rotation;
            rotation = AvoidWallRcastv3(rotation, wallRay);

            if ((rotation != oldrotate))
            {
                modeUsed = "Wall red";
                //if ((withDEBUG)){ Debug.Log("WALL AVOID ACTIVATED : " + rotation); }
            }
            if ((withDEBUG)) { Debug.Log("FINAL ROTATION : " + rotation + " " + modeUsed); }
            // APPLY ROTATION
            if (rotation == 0.0)
            {

                //tentative reduire velocite rotation et inertie pour eviter drifting boid
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
                rb.inertiaTensor = Vector3.zero;
                rb.inertiaTensorRotation = Quaternion.identity;
                rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Force);
            }
            else
            {
                //multiplication par Time.deltaTime pour fluidifier la rotation
                // valeur obtenue très faible
                float angle = rotation * Time.deltaTime;
                if (withDEBUG)
                {
                    Debug.Log(" FINAL VALUE OF ANGLE AFTER TIME " + angle);
                }
                if (angle > filter) { angle = filter; }
                if (angle < -filter) { angle = -filter; }
                if (withDEBUG)
                {
                    Debug.Log(" FINAL VALUE OF rotation after filter " + angle);
                }
                transform.Rotate(Vector3.up, angle);
                //tentative reduire velocite rotation et inertie pour eviter drifting boid
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
                rb.inertiaTensor = Vector3.zero;
                rb.inertiaTensorRotation = Quaternion.identity;
                //this.transform.Translate(this.transform.forward);
                rb.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Force);

            }

            //Debug.Log("Inertie " + rb.inertiaTensor);
            //Debug.Log("Inertie " + rb.inertiaTensorRotation);
            //Debug.Log("ANGULAR VELOCITY " + rb.angularVelocity);
            if (withDEBUG)
            {
                //Debug.Log("VELOCITY IS " + rb.velocity);
                Debug.Log("//////////////////////////////////////////////////////////////////////////////////");
            }

        }
    }

    private void LateUpdate()
    {
        // check si en contact avec le sol
        IsGrounded();

    }

    // AVOID WALL BEHAVIOUR
    private float AvoidWallRcastv4(float rotate, float wallRay)
    {
        /*
        Comportement pour eviter les murs avec une distance max de wallRay
        Rotation ==> premier détecté renvoie la valeur
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
        float[] toRotate = { 50f, 45f, -45f, 25f, -25f };
        //hit front
        if (Physics.Raycast(front, out info, maxdistance, layermask))
        {
            //Debug.Log("HIT FRONT");
            if (withDEBUG)
            {
                Debug.Log("Collision at " + info.transform.position);
                Debug.DrawLine(myPos, myPos + f, Color.red);
            }
            //return 50;
            float dis = Vector3.Distance(myPos + f, info.transform.position);
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
            if (withDEBUG)
            {
                Debug.DrawLine(myPos, myPos +fleft, Color.red);
            }
            //return 45;
            float dis = Vector3.Distance(myPos+fleft, info.transform.position);
            distanceList.Add(dis);
        }
        else
        {
            distanceList.Add(8000);
        }
        // Hit FR
        if (Physics.Raycast(frightRay,out info, maxdistance, layermask))
        {
            if (withDEBUG)
            {
                Debug.DrawLine(myPos, myPos + fright, Color.red);
            }
            //return -45;
            float dis = Vector3.Distance(myPos+fright, info.transform.position);
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
            if (withDEBUG) { Debug.DrawLine(myPos, myPos - r, Color.red); }
            //return 25;
            float dis = Vector3.Distance(myPos-r, info.transform.position);
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
            if (withDEBUG) { Debug.DrawLine(myPos, myPos + r, Color.red); }
            //return -25;
            float dis = Vector3.Distance(myPos+r, info.transform.position);
            distanceList.Add(dis);
        }
        else
        {
            distanceList.Add(8000);
        }
        if (withDEBUG)
        {
            Debug.Log("COUNT " + distanceList.Count);
            foreach(float distance in distanceList)
            {
                Debug.Log(" Value found in disList " + distance);
            }
        }
        // TO-DO : Traitement distance
        if (distanceList.Count > 0)
        {
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
        
        return rotate;
    }
    private float AvoidWallRcastv3(float rotate, float wallRay)
    {
        /*
        Comportement pour eviter les murs avec une distance max de wallRay
        Rotation ==> premier détecté renvoie la valeur
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
        //hit front
        if (Physics.Raycast(front,out info, maxdistance, layermask))
        {
            //Debug.Log("HIT FRONT");
            if (withDEBUG) {
                Debug.Log("Collision at " + info.transform.position);
                Debug.DrawLine(myPos, myPos + (Vector3.forward), Color.red); 
            }
            return 50;
        }
        // Hit FL
        if (Physics.Raycast(fleftRay, maxdistance, layermask))
        {
            //Debug.Log("HIT FRONT");
            if (withDEBUG)
            {
                Debug.DrawLine(myPos, myPos + (Vector3.forward), Color.red);
            }
            return 45;
        }
        // Hit FR
        if (Physics.Raycast(frightRay, maxdistance, layermask))
        {
            if (withDEBUG)
            { 
                Debug.DrawLine(myPos, myPos + (Vector3.forward), Color.red);
            }
            return -45;
        }
        //hit left
        if (Physics.Raycast(left, maxdistance, layermask))
        {
            //Debug.Log("HIT LEFT");
            if (withDEBUG) { Debug.DrawLine(myPos, myPos - (Vector3.right), Color.red); }
            return 25;
        }
        //hit right
        if ((Physics.Raycast(right, maxdistance, layermask)))
        {
            //Debug.Log("HIT Right");
            if (withDEBUG) { Debug.DrawLine(myPos, myPos + (Vector3.right), Color.red); }
            return -25;
        }
        return rotate;
    }
    // COHESION BEHAVIOUR
    private float CohesionBoidRcast(float rotate, float minRay, float maxRay)
    {
        /*
        Comportement s'alligner avec tous les détectés
        Rotation ==> angle entre la destination moyenne et le forward
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
        collidedCohesionRcastAll(leftHit, myPos, allPosCollide, allTransformsCollide, mindistance, maxdistance);
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
        rotation = getCohesionRotationV2(myPos, allPosCollide, allTransformsCollide);

        if (rotation != 0.0) { return rotation; }
        else { return rotate; }
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
        if (withDEBUG){ Debug.Log("Count Front " + frontHit.Length); }
        //hit front Right
        RaycastHit[] frontRightHit = Physics.RaycastAll(FrontRight, maxdistance, layermask);
        closestPoint = goToRcastAllV3(frontRightHit, myPos, mindistance, maxdistance, closestPoint);
        if (withDEBUG) { Debug.Log("Count FR " + frontRightHit.Length); }
        //hit front Left
        RaycastHit[] frontLeftHit = Physics.RaycastAll(FrontLeft, maxdistance, layermask);
        closestPoint = goToRcastAllV3(frontLeftHit, myPos, mindistance, maxdistance, closestPoint);
        if (withDEBUG) { Debug.Log("Count FL " + frontLeftHit.Length); }
        //hit left
        RaycastHit[] leftHit = Physics.RaycastAll(left, maxdistance, layermask);
        closestPoint = goToRcastAllV3(leftHit, myPos, mindistance, maxdistance, closestPoint);
            
        if (withDEBUG) { Debug.Log("Count left " + leftHit.Length); }
        //hit right
        RaycastHit[] rightHit = Physics.RaycastAll(right, maxdistance, layermask);
        closestPoint = goToRcastAllV3(rightHit, myPos, mindistance, maxdistance, closestPoint);
        if (withDEBUG) { Debug.Log("Count right " + rightHit.Length); }
        //hit down Right
        RaycastHit[] downRightHit = Physics.RaycastAll(downRight, maxdistance, layermask);
        closestPoint = goToRcastAllV3(downRightHit, myPos, mindistance, maxdistance, closestPoint);
        if (withDEBUG) { Debug.Log("Count Dright " + downRightHit.Length); }
        //hit down Left
        RaycastHit[] downLeftHit = Physics.RaycastAll(downLeft, maxdistance, layermask);
        closestPoint = goToRcastAllV3(downLeftHit, myPos, mindistance, maxdistance, closestPoint);
        if (withDEBUG) { Debug.Log("Count Dleft " + downLeftHit.Length); }
        rotation = getGotoRotationv3(myPos, closestPoint);
        //rotation = 0;
        if (rotation != 0.0) { return rotation; }
        else { return rotate; }
    }
    // GET ANGLES
    // Méthodes pour obtenir les angles de rotation
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
        //AVOID ROTATION
    private float getAvoidRotationv2(Vector3 myPos, Vector3 closest)
    {
        Vector3 loc = rb.transform.InverseTransformPoint(closest);
        bool isRight = false;
        if (loc.x > 0) { isRight = true; }
        if (withDEBUG) {
            /*
            Debug.DrawLine(myPos, closest, Color.yellow);
            Debug.Log("LOC = "+loc) ;
            Debug.Log("Closest ="+closest);
            */
        }
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
                Debug.Log("Proposed correction is " + correction);
            }
            bool isRight = false;
            if (newPos.x > 0)
            {
                isRight = true;
            }
            overAllCorrection += correction;
            //overAllCorrection = Math.Max(overAllCorrection, correction);

            if (withDEBUG)
            {
                Debug.DrawLine(pos, forw, Color.blue);
               
                Debug.Log("My forward = " + this.transform.forward);
                Debug.Log("His forward = " + hisTransform.forward);
                Debug.DrawLine(myPos, newPos, Color.magenta);
            }
            theirForward.Add(formyforward);

        }
        Vector3 mine = myPos + transform.forward;
        if (withDEBUG)
        {
            Debug.DrawLine(myPos, mine, Color.blue);
            Debug.Log("OVERALL CORECTION IS " + overAllCorrection);
        }
        return overAllCorrection;

        //return 0.0f;

    }
    private float getGotoRotationv3(Vector3 myPos, Vector3 closestPoint)
    {

        if (closestPoint == Vector3.zero)
        {
            return 0.0f;
        }
        if (withDEBUG) { Debug.DrawLine(myPos, closestPoint, Color.green); }
        return getAngleTowards(myPos, closestPoint);
    }
    // CHECK COLLISIONS 
    //Méthodes de détection des collisions pour chaque comportement
    private Vector3 collidedRcastAllv3(RaycastHit[] tab, Vector3 myPos, float maxDistance, Vector3 closest)
    {
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
    private void collidedCohesionRcastAll(RaycastHit[] tab, Vector3 myPos, List<Vector3> allCollide, List<Transform> allTransformsCollide, float mindistance, float maxdistance)
    {
        for (int i = 1; i < tab.Length; i++)
        {
            RaycastHit hitInfo = tab[i];
            float dist = Vector3.Distance(myPos, hitInfo.transform.position);
            if ((hitInfo.transform.position != myPos) && (dist >= mindistance) && (dist < maxdistance) && (!allCollide.Contains(hitInfo.transform.position)))
            {
                allCollide.Add(hitInfo.transform.position);
                allTransformsCollide.Add(hitInfo.transform);
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
                Debug.Log("My pos is " + myPos);
                Debug.Log("Hit at " + dist);
                Debug.Log("Hit position at " + hitInfo.transform.position);
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

    //Ground
    public void IsGrounded()
    {
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
