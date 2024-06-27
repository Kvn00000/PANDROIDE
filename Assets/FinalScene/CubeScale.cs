using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


public class CubeScale : MonoBehaviour
{
    // Start is called before the first frame update

    private XRGrabInteractable grabInteractable;
    private List<XRBaseInteractor> interactors = new List<XRBaseInteractor>();
    private MeshFilter mesh;
    private XRBaseInteractor interactor2;
    private Vector3 StartControllerPos;

    private string surfaceDetected;


    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        mesh = GetComponent<MeshFilter>();
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }


    private void OnGrab(SelectEnterEventArgs args)
    {
        if (!interactors.Contains(args.interactor)){
            interactors.Add(args.interactor);
            Debug.Log("nfjgkdfls");
            Debug.Log(args.interactor.transform.position);

            if (interactors.Count == 2){
                interactor2 = args.interactor;
                StartControllerPos = interactor2.transform.position;
                grabInteractable.trackRotation = false;

                
                Vector3 originalCoords = RotatePointAroundPivot(interactor2.transform.position,this.transform.position,Quaternion.Inverse(this.transform.rotation).eulerAngles);
            }
        }
    }


    private void OnRelease(SelectExitEventArgs args){
        //Quand on relache l'objet on le supprime
        if (interactors.Contains(args.interactor)){
            interactors.Remove(args.interactor);

            if (interactors.Count < 2){
                XRGrabInteractable gr = this.GetComponent<XRGrabInteractable>();
                gr.trackRotation = true;
            }
        }
    }



    // Update is called once per frame
    void Update(){
        if (interactors.Count == 2){
                float distance = Vector3.Distance(interactor2.transform.position ,StartControllerPos);
                switch(surfaceDetected){
                    case "Top":
                        
                        break;
                    
                    case "Front":
                        if( interactor2.transform.position.z - StartControllerPos.z > 0 ){
                            resizeCube(distance,"z", true );
                        }else{
                            resizeCube(distance,"z", false );
                        }
                        
                        break;
                    
                    case "Left":
                        break;

                    case "Right":
                        if( interactor2.transform.position.x - StartControllerPos.x > 0 ){
                            resizeCube(distance,"x", true );
                        }else{
                            resizeCube(distance,"x", false );
                        }
                        
                        break;

                    case "Back":
                        break;

                    case "Bottom":
                        break;
                }
        
    }
    }



    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 centre, Vector3 angles){
        Vector3 dir = point - centre; // get point direction relative to pivot
        dir = Quaternion.Euler(angles)* dir; // rotate it
        Vector3 nPoint = dir + centre;
        return nPoint; // return it
    }



    public void resizeCube(float amount, string axis, bool inverse){
        Debug.Log("Here");
        amount = amount * 0.05f;
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
