using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class Movement : MonoBehaviour
{
    public float speed = 0.1F;

    public float detectionRadius = 5F;

    public bool _ray;    
   private Rigidbody rb;


    // +y droite  -y gauche
    private Vector3 delta  = new  Vector3(0,30F,0);

    private Vector3 noise = new Vector3(0,10F,0);
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {   



        float side = avoidWall();
        if (side == 0){
            side = alignementSphere(detectionRadius);
        }
        this.transform.position += speed * transform.forward * Time.deltaTime ;
        // if(side == 0){
        //     noise = -noise;
        //     // transform.Rotate(noise*Time.deltaTime);
        // }
        noise = -noise;
        transform.Rotate(delta*side*Time.deltaTime + noise*Time.deltaTime);

    }



    private float avoidWall(){
        float cote = 0;
        //Sinus > 0 coté gauche

        RaycastHit hit;

        //Front
        if(Physics.Raycast(transform.position,transform.forward,out hit ,detectionRadius)){
            if(hit.transform.gameObject.layer == 8){
                cote = 1;
            }
        }
        //Front Right
        if(Physics.Raycast(transform.position,transform.right+ transform.forward,out hit ,detectionRadius)){
            if(hit.transform.gameObject.layer == 8){
                return -1;
            }
        }
        //Right
        if(Physics.Raycast(transform.position, transform.right,out hit ,detectionRadius)){
            if(hit.transform.gameObject.layer == 8){
                return -1;
            }
        }        
        //Front Left
        if(Physics.Raycast(transform.position,-transform.right + transform.forward,out hit ,detectionRadius)){
            if(hit.transform.gameObject.layer == 8){
                return 1;
            }
        }
        //Left
        if(Physics.Raycast(transform.position,-transform.right,out hit ,detectionRadius)){
            if(hit.transform.gameObject.layer == 8){
                return 1;
            }
        }
        return cote;
    }

    private float alignementSphere(float detectionRadius){
        int cptRight = 0;
        int cptLeft = 0;
        RaycastHit[] allHit = Physics.SphereCastAll(transform.position, detectionRadius, transform.forward);

        foreach(RaycastHit hit in allHit){
                if(hit.transform.GetInstanceID() != transform.GetInstanceID() && hit.transform.gameObject.layer == 6){
                Debug.DrawLine(transform.position,hit.transform.position);
                Debug.Log(hit.transform.forward + " et le right" + hit.transform.right);
                
                float scalaireForward = Vector3.Dot(transform.forward, hit.transform.forward);
                float scalaireRight = Vector3.Dot(transform.forward, hit.transform.right);
                Debug.Log(scalaireForward + " et le scalaire righhht  ::" + scalaireRight);
                if(scalaireForward > 0 && scalaireForward < 0.995){
                    if(scalaireRight > 0 ){
                        Debug.Log("haut gauche");
                        cptLeft = cptLeft +1;
                    }else if (scalaireRight == 0){
                        Debug.Log(" vers le haut scalaire droite = 0");
                    }else{
                        Debug.Log("haut droite");
                        cptRight = cptRight +1;
                    }
                }else if(scalaireForward < -0 && scalaireForward > -0.995){
                    if(scalaireRight > 0 ){
                        Debug.Log("bas gauche");
                        cptLeft = cptLeft +1;
                    }else if (scalaireRight == 0){
                        Debug.Log("vers le bas scalaire droite = 0");
                    }else{
                        Debug.Log("bas droite");
                        cptRight = cptRight +1;
                    }
                }
            }
        }
        if (cptLeft > cptRight){
            return -1;
        }else if(cptLeft == 0&& cptRight == 0){
            return 0;
        }else{
            return 1;
        }
    }

}
