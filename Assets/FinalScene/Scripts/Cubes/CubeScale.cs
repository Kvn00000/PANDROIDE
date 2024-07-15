using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System;


public class CubeScale : MonoBehaviour{
    private XRGrabInteractable grabInteractable;
    private List<XRBaseInteractor> interactors = new List<XRBaseInteractor>();
    private XRBaseInteractor interactor2;
    private Vector3 previousPos;
    private Transform childGrabbed;
    private string surfaceDetected;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
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

            //If we grab with two hands
            if (interactors.Count == 2){
                interactor2 = args.interactor;
                previousPos = interactor2.transform.position;
                surfaceDetected = DetectGrabbedFace(args.interactor.transform.position);

                //To have a different color when grabbed
                childGrabbed = transform.Find(surfaceDetected);
                childGrabbed.gameObject.SetActive(true);
            }
        }
    }


    private void OnRelease(SelectExitEventArgs args){
        
        //When release we remove the interactor
        if (interactors.Contains(args.interactor)){
            interactors.Remove(args.interactor);

            if (interactors.Count < 2){
                grabInteractable.trackRotation = true;
                if(childGrabbed != null)
                {
                    childGrabbed.gameObject.SetActive(false);
                    childGrabbed = null;
                }
               
            }
        }
    }



    void Update(){

        //If grabbed with two hands and the second hand moves we change the scale
        if (interactors.Count == 2 && Vector3.Distance(interactor2.transform.position ,previousPos) > 0.001  ){
            float distance = Vector3.Distance(interactor2.transform.position ,previousPos);
            Vector3 current = interactor2.transform.position;

            Vector3 test_prev=this.transform.InverseTransformPoint(previousPos);
            Vector3 test_cur= this.transform.InverseTransformPoint(current);

            switch (surfaceDetected){
                    case "Top":
                        if(test_cur.y - test_prev.y < 0 ){
                            resizeCube(distance,"y", true );
                        }else{
                            resizeCube(distance,"y", false );
                        }
                        previousPos = current;
                        break;

                    case "Front":
                        if(test_cur.z - test_prev.z < 0 ){
                            resizeCube(distance,"z", true );
                        }else{
                            resizeCube(distance,"z", false );
                        }
                        previousPos = interactor2.transform.position;

                        break;

                    case "Left":

                            if(test_cur.x - test_prev.x > 0 ){
                            resizeCube(distance,"x", true );
                        }else{
                            resizeCube(distance,"x", false );
                        }
                        previousPos = interactor2.transform.position;
                        break;

                    case "Right":
                        if(test_cur.x - test_prev.x < 0 ){

                            resizeCube(distance,"x", true );
                        }else{
                            resizeCube(distance,"x", false );
                        }
                        previousPos = interactor2.transform.position;

                        break;

                    case "Back":
                        if(test_cur.z - test_prev.z > 0 ){
                            resizeCube(distance,"z", true );
                        }else{
                            resizeCube(distance,"z", false );
                        }
                        previousPos = current;
                        break;

                    case "Bottom":
                        if(test_cur.y - test_prev.y > 0 ){
                            resizeCube(distance,"y", true );
                        }else{
                            resizeCube(distance,"y", false );
                        }
                        previousPos = current;
                        break;

                    case "":
                        break;


                }

        }
    }




    public void resizeCube(float amount, string axis, bool inverse){
        // bool inverse to see if we add or substract the amount
        switch (axis){
            case "x":
                if (!inverse){
                    this.transform.localScale = new Vector3(this.transform.localScale.x+amount, this.transform.localScale.y, this.transform.localScale.z);
                }else{
                    this.transform.localScale = new Vector3(this.transform.localScale.x-amount, this.transform.localScale.y, this.transform.localScale.z);
                }
                break;
            case "y":
                if (!inverse){
                    this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y+amount, this.transform.localScale.z);
                }else{
                    this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y-amount, this.transform.localScale.z);
                }
                break;
            case "z":
                if (!inverse){
                    this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z + amount);
                }else{
                    this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z- amount);
                }
                break;
            default:
                break;

        }
   }

    private string DetectGrabbedFace(Vector3 contactPoint)
    {
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
