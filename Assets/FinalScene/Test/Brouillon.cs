using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brouillon : MonoBehaviour
{

    public float detectionRadius = 5F;

    public bool _ray;    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }








    private RaycastHit[][] getRayFrontRight(){        
        RaycastHit[][] hitAllRight = new RaycastHit[3][];

        Vector3 FRdiag2 = (transform.right+ transform.forward)*0.5F;
        Vector3 FRdiag1 = (transform.forward+FRdiag2)*0.5F;
        Vector3 FRdiag3 = (transform.right+FRdiag2)*0.5F;
        
        hitAllRight[0] = Physics.RaycastAll(transform.position ,FRdiag1 ,detectionRadius);
        hitAllRight[1] = Physics.RaycastAll(transform.position ,FRdiag2 ,detectionRadius);
        hitAllRight[2] = Physics.RaycastAll(transform.position ,FRdiag3 ,detectionRadius);

        if(_ray){
            Debug.DrawRay(transform.position ,FRdiag1 , Color.red);
            Debug.DrawRay(transform.position ,FRdiag2 , Color.red);
            Debug.DrawRay(transform.position ,FRdiag3 , Color.red);
        }

        return hitAllRight;
    }

    private RaycastHit[][] getRayBackRight(){        
        RaycastHit[][] hitAllRight = new RaycastHit[2][];

        Vector3 FRdiag2 = (transform.right - transform.forward)*0.5F;
        Vector3 FRdiag1 = (FRdiag2 + transform.right)*0.5F;
        
        hitAllRight[0] = Physics.RaycastAll(transform.position ,FRdiag1 ,detectionRadius);
        hitAllRight[1] = Physics.RaycastAll(transform.position ,FRdiag2 ,detectionRadius);

        if(_ray){
            Debug.DrawRay(transform.position ,FRdiag1 , Color.red);
            Debug.DrawRay(transform.position ,FRdiag2 , Color.red);
        }
        return hitAllRight;
    }

    private RaycastHit[][] getRayFrontLeft(){
        RaycastHit[][] hitAllLeft = new RaycastHit[4][];

        Vector3 FLdiag2 = (-transform.right+ transform.forward)*0.5F;
        Vector3 FLdiag1 = (transform.forward+FLdiag2)*0.5F;
        Vector3 FLdiag3 = (-transform.right+FLdiag2)*0.5F;
        
        //Front Left
        hitAllLeft[0] = Physics.RaycastAll(transform.position,FLdiag1 ,detectionRadius);
        hitAllLeft[1] = Physics.RaycastAll(transform.position,FLdiag2 ,detectionRadius);
        hitAllLeft[2] = Physics.RaycastAll(transform.position,FLdiag3 ,detectionRadius);

        if(_ray){
            Debug.DrawRay(transform.position ,FLdiag1 , Color.red);
            Debug.DrawRay(transform.position ,FLdiag2 , Color.red);
            Debug.DrawRay(transform.position ,FLdiag3 , Color.red);
        }

        return hitAllLeft;
    }


    private RaycastHit[][] getRayBackLeft(){
        RaycastHit[][] hitAllLeft = new RaycastHit[4][];

        Vector3 FLdiag2 = (-transform.right - transform.forward)*0.5F;
        Vector3 FLdiag1 = (-transform.right + FLdiag2)*0.5F;

        //Front Left
        hitAllLeft[0] = Physics.RaycastAll(transform.position,FLdiag1 ,detectionRadius);
        hitAllLeft[1] = Physics.RaycastAll(transform.position,FLdiag2 ,detectionRadius);

        if(_ray){
            Debug.DrawRay(transform.position ,FLdiag1 , Color.red);
            Debug.DrawRay(transform.position ,FLdiag2 , Color.red);
        }

        return hitAllLeft;
    }



    private float alignementBoid(){
        float cote = 0;

        RaycastHit[] hitFront;
        RaycastHit[][] hitFrontRight = new RaycastHit[4][];
        RaycastHit[] hitRight;
        RaycastHit[][] hitBackRight = new RaycastHit[4][];
        RaycastHit[][] hitFrontLeft = new RaycastHit[4][];
        RaycastHit[] hitLeft;
        RaycastHit[][] hitBackLeft = new RaycastHit[4][];

        if(_ray){
            Debug.DrawRay(transform.position ,transform.forward , Color.red);
            Debug.DrawRay(transform.position ,transform.right , Color.red);
            Debug.DrawRay(transform.position ,-transform.right , Color.red);
        }

        //Front
        hitFront = Physics.RaycastAll(transform.position,transform.forward ,detectionRadius);
        
        //Droite -->>>>> 1
        //Front Right

        //Right
        hitRight = Physics.RaycastAll(transform.position, transform.right,detectionRadius);
       
        //Front Left
        //Left
        hitLeft = Physics.RaycastAll(transform.position,-transform.right,detectionRadius);

        int maxRight = 0;
        int maxLeft = 0;

        hitFrontRight = getRayFrontRight();
        hitBackRight = getRayBackRight();
        hitFrontLeft = getRayFrontLeft();
        hitBackLeft = getRayBackLeft();
        
        foreach(RaycastHit hit in hitFront){
            Debug.Log("il est en face  "+hit.transform.forward + " mon forward " + transform.forward);
            float scalaireForward = Vector3.Dot(transform.forward, hit.transform.forward);
            float scalaireRight = Vector3.Dot(transform.forward, hit.transform.right);

            if(scalaireForward > 0){
                if(scalaireRight > 0 ){
                    Debug.Log("haut gauche");
                }else{
                    Debug.Log("haut droite");
                }
            }else{
                if(scalaireRight > 0 ){
                    Debug.Log("bas gauche");
                }else{
                    Debug.Log("bas droite");
                }
            }
        
        }
        
        foreach(RaycastHit hit in hitRight){
            if(this.GetInstanceID() != hit.transform.GetInstanceID())
            Debug.Log("il est a droite  "+hit.transform.forward + " mon forward " + transform.forward);
            Debug.Log(Vector3.Angle(this.transform.forward,hit.transform.forward));
            float scalaireForward = Vector3.Dot(transform.forward, hit.transform.forward);
            float scalaireRight = Vector3.Dot(transform.forward, hit.transform.right);

            if(scalaireForward > 0){
                if(scalaireRight > 0 ){
                    Debug.Log("haut gauche");
                }else{
                    Debug.Log("haut droite");
                }
            }else{
                if(scalaireRight > 0 ){
                    Debug.Log("bas gauche");
                }else{
                    Debug.Log("bas droite");
                }
            }
        }
        foreach(RaycastHit hit in hitLeft){
            if(this.GetInstanceID() != hit.transform.GetInstanceID())
            Debug.Log("il est a gauche  "+hit.transform.forward + " mon forward " + transform.forward);
            Debug.Log(Vector3.Angle(this.transform.forward,hit.transform.forward));
            float scalaireForward = Vector3.Dot(transform.forward, hit.transform.forward);
            float scalaireRight = Vector3.Dot(transform.forward, hit.transform.right);

            if(scalaireForward > 0){
                if(scalaireRight > 0 ){
                    Debug.Log("haut gauche");
                }else{
                    Debug.Log("haut droite");
                }
            }else{
                if(scalaireRight > 0 ){
                    Debug.Log("bas gauche");
                }else{
                    Debug.Log("bas droite");
                }
            }
        }


        return 0;
    }











}
