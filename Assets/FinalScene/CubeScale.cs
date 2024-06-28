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
            // Debug.Log(args.interactor.transform.position);

            // Debug.Log(this.transform.forward);
            
            

            if (interactors.Count == 2){
                interactor2 = args.interactor;
                StartControllerPos = interactor2.transform.position;
                grabInteractable.trackRotation = false;

                Vector3 originalCoords = RotatePointAroundPivot(interactor2.transform.position,this.transform.position,Quaternion.Inverse(this.transform.rotation).eulerAngles);
            
                surfaceDetected = DetectGrabbedFace(args.interactor.transform.position);
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
                surfaceDetected = "";

            }
        }
    }



    // Update is called once per frame
    void Update(){
        if (interactors.Count == 2){
                // float distance = Vector3.Distance(interactor2.transform.position ,StartControllerPos);
                float distance;
                switch(surfaceDetected){
                    case "Top":
                        
                        break;
                    
                    case "Front":
                        distance = interactor2.transform.position.z - StartControllerPos.z;
                        if( interactor2.transform.position.z - StartControllerPos.z > 0 ){
                            resizeCube(distance,"z", true );
                        }else{
                            resizeCube(distance,"z", false );
                        }
                        
                        break;
                    
                    case "Left":
                        break;

                    case "Right":
                        distance = interactor2.transform.position.x - StartControllerPos.x;

                        if( interactor2.transform.position.x - StartControllerPos.x > 0 ){
                            resizeCube(distance,"x", true );
                        }else{
                            resizeCube(distance,"x", false );
                        }
                        
                        break;

                    case "Back":
                        distance = interactor2.transform.position.z - StartControllerPos.z;
                    if( interactor2.transform.position.z - StartControllerPos.z <0 ){
                            resizeCube(distance,"z", false );
                        }else{
                            resizeCube(distance,"z", true );
                        }
                        break;

                    case "Bottom":
                        break;
                    
                    case "":
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

    private string DetectGrabbedFace(Vector3 contactPoint)
    {
        // Assuming the object is a cube, define the face normals
        Dictionary<string, Vector3> faceNormals = new Dictionary<string, Vector3>
        {
            { "Top", Vector3.up },
            { "Bottom", Vector3.down },
            { "Front", Vector3.forward },
            { "Back", Vector3.back },
            { "Left", Vector3.left },
            { "Right", Vector3.right }
        };

        // Find the closest face
        string closestFace = "";
        float maxDot = float.MinValue;

        foreach (var face in faceNormals)
        {
            Vector3 faceNormal = this.transform.TransformDirection(face.Value);
            float dot = Vector3.Dot((contactPoint - this.transform.position).normalized, faceNormal);

            if (dot > maxDot)
            {
                maxDot = dot;
                closestFace = face.Key;
            }
        }

        Debug.Log("Grabbed face: " + closestFace);

        return closestFace;
    }


}
