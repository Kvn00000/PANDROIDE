using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System;


public class CubeScale : MonoBehaviour
{
    // Start is called before the first frame update

    private XRGrabInteractable grabInteractable;
    private List<XRBaseInteractor> interactors = new List<XRBaseInteractor>();
    private MeshFilter mesh;
    private XRBaseInteractor interactor2;
    private Vector3 StartControllerPos;
    private Vector3 previousPos;


    public Material GrabbedMat;
    public Material OldMat;
            
    private Component component;

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
        
            
            if (interactors.Count == 2){

                interactor2 = args.interactor;
                StartControllerPos = interactor2.transform.position;

                previousPos = interactor2.transform.position;
                //grabInteractable.trackRotation = false;

                // Vector3 originalCoords = RotatePointAroundPivot(interactor2.transform.position,this.transform.position,Quaternion.Inverse(this.transform.rotation).eulerAngles);
                surfaceDetected = DetectGrabbedFace(args.interactor.transform.position);
                Debug.Log("get type");

                Type type = Type.GetType(surfaceDetected+"Color");
                Debug.Log("component");

                component = transform.Find(surfaceDetected).GetComponent(Type.GetType(surfaceDetected+"Color"));
                Debug.Log("set new mesh");

                type.GetMethod("setNewMesh").Invoke(component, new object[]{GrabbedMat});
        
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

                Type type = Type.GetType(surfaceDetected+"Color");
                component = transform.Find(surfaceDetected).GetComponent(Type.GetType(surfaceDetected+"Color"));
                type.GetMethod("setNewMesh").Invoke(component, new object[]{OldMat});
                surfaceDetected = "";

            }
        }
    }



    // Update is called once per frame
    void Update(){
        
        if (interactors.Count == 2 && Vector3.Distance(interactor2.transform.position ,previousPos) > 0.001  ){
            float distance = Vector3.Distance(interactor2.transform.position ,previousPos);
            Vector3 current = interactor2.transform.position;

            Vector3 test_prev=this.transform.InverseTransformPoint(previousPos);
            Vector3 test_cur= this.transform.InverseTransformPoint(current);

                // Vector3 distance = interactor2.transform.position - previousPos;
                // Debug.Log(distance);
            switch (surfaceDetected){
                    case "Top":
                        if(test_cur.y - test_prev.y < 0 ){

                        // if( Vector3.Distance(interactor2.transform.position ,StartControllerPos) > Vector3.Distance(previousPos ,StartControllerPos) ){
                            // Debug.Log(" Diff positive");
                            resizeCube(distance,"y", true );
                        }else{
                            // Debug.Log(" Diff neg");
                            resizeCube(distance,"y", false );
                        }
                        previousPos = current;
                        break;
                    
                    case "Front":
                        if(test_cur.z - test_prev.z < 0 ){

                        // if( Vector3.Distance(interactor2.transform.position ,StartControllerPos) > Vector3.Distance(previousPos ,StartControllerPos) ){
                            // Debug.Log(" Diff positive");
                            resizeCube(distance,"z", true );
                        }else{
                            // Debug.Log(" Diff neg");
                            resizeCube(distance,"z", false );
                        }
                        previousPos = interactor2.transform.position;
                        
                        break;
                    
                    case "Left":

                            if(test_cur.x - test_prev.x > 0 ){

                        // if( Vector3.Distance(interactor2.transform.position ,StartControllerPos) > Vector3.Distance(previousPos ,StartControllerPos) ){
                            // Debug.Log(" Diff positive");
                            resizeCube(distance,"x", true );
                        }else{
                            // Debug.Log(" Diff neg");
                            resizeCube(distance,"x", false );
                        }
                        previousPos = interactor2.transform.position;
                        break;

                    case "Right":
                        if(test_cur.x - test_prev.x < 0 ){

                        // if( Vector3.Distance(interactor2.transform.position ,StartControllerPos) > Vector3.Distance(previousPos ,StartControllerPos) ){
                            // Debug.Log(" Diff positive");
                            resizeCube(distance,"x", true );
                        }else{
                            // Debug.Log(" Diff neg");
                            resizeCube(distance,"x", false );
                        }
                        previousPos = interactor2.transform.position;
                        
                        break;

                    case "Back":
                    // Debug.Log(interactor2.transform.position.z);
                    // Debug.Log(previousPos.z);

                    // Debug.Log("fjgklf");
                    // Debug.Log(Vector3.Distance(interactor2.transform.position ,StartControllerPos) == Vector3.Distance(previousPos ,StartControllerPos));

                    // Debug.Log(Vector3.Distance(interactor2.transform.position ,StartControllerPos));
                    // Debug.Log(Vector3.Distance(previousPos ,StartControllerPos));

                    // distance = interactor2.transform.position.z - StartControllerPos.z;
                    if(test_cur.z - test_prev.z > 0 ){

                    // if( Vector3.Distance(interactor2.transform.position ,StartControllerPos) > Vector3.Distance(previousPos ,StartControllerPos) ){
                            // Debug.Log(" Diff positive");
                            resizeCube(distance,"z", true );
                        }else{
                            // Debug.Log(" Diff neg");
                            resizeCube(distance,"z", false );
                        }
                        previousPos = current;
                        break;

                    case "Bottom":
                        if(test_cur.y - test_prev.y > 0 ){

                        // if( Vector3.Distance(interactor2.transform.position ,StartControllerPos) > Vector3.Distance(previousPos ,StartControllerPos) ){
                            // Debug.Log(" Diff positive");
                            resizeCube(distance,"y", true );
                        }else{
                            // Debug.Log(" Diff neg");
                            resizeCube(distance,"y", false );
                        }
                    previousPos = current;
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
        // amount = amount * 0.05f;
        switch (axis)
        {
            case "x":
                if (!inverse)
                {
                    //this.transform.position = new Vector3(this.transform.position.x+(amount/2), this.transform.position.y, this.transform.position.z);
                    this.transform.localScale = new Vector3(this.transform.localScale.x+amount, this.transform.localScale.y, this.transform.localScale.z);
                }
                else
                {
                    
                    //this.transform.position = new Vector3(this.transform.position.x-(amount/2), this.transform.position.y, this.transform.position.z);
                    this.transform.localScale = new Vector3(this.transform.localScale.x-amount, this.transform.localScale.y, this.transform.localScale.z);
                }
                break;
            case "y":
                if (!inverse)
                {
                    //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + (amount / 2), this.transform.position.z);
                    this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y+amount, this.transform.localScale.z);
                }
                else
                {
                    //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y-(amount / 2), this.transform.position.z);
                    this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y-amount, this.transform.localScale.z);
                }
                break;
            case "z":
                if (!inverse)
                {
                    //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + (amount / 2));
                    this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z + amount);
                }
                else
                {
                    //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z- (amount / 2));
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
