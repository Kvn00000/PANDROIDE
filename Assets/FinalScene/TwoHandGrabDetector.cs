using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class TwoHandGrabDetector : MonoBehaviour
{

    public XRRayInteractor xrInteractor;

    public GameObject Top;
    public GameObject Front;
    public GameObject Left;
    public GameObject Right;
    public GameObject Back;
    public GameObject Bottom;



    private XRGrabInteractable grabInteractable;
    private List<XRBaseInteractor> interactors = new List<XRBaseInteractor>();

    private ChildComponent childs;
    private MeshFilter mesh;

    public InputActionProperty grabAction;


    private XRBaseInteractor interactor2;

    private Vector3 StartControllerPos;
    private string faceTouched;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        mesh = GetComponent<MeshFilter>();
        childs = GetComponent<ChildComponent>();
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
            // // xrInteractor.TryGetHitInfo(out XRBaseInteractable interactable);
            // Debug.Log("Collider touché : " + args.interactable.gameObject.name );


            // Collider collider = args.collider;
            // if (collider != null && collider.transform.IsChildOf(transform)){
            //     Debug.Log("Collider enfant touché : " + collider.name);
            //     // Faites quelque chose avec le collider enfant ici
            // }
            
            if (interactors.Count == 2){
                interactor2 = args.interactor;
                StartControllerPos = interactor2.transform.position;
                // faceTouched = hit.collider.name;



                


                // float rayLength = 5f;

                // // Dessinez le rayon à partir du point de contact, dans la direction de la normale
                // Debug.DrawRay(hit.point, hit.normal * rayLength, Color.red);
            }
        }
    }

    private void OnRelease(SelectExitEventArgs args){
        //Quand on relache l'objet on le supprime
        if (interactors.Contains(args.interactor)){
            interactors.Remove(args.interactor);
        }
    }

    void Update(){
        if (interactors.Count == 2){
                //Ici je me base toujours sur le controller droit donc a changer
                xrInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit);


                float distance = Vector3.Distance(interactor2.transform.position ,StartControllerPos);

                switch(hit.collider.name){
                    case "Top":
                        
                        break;
                    
                    case "Front":
                        if( interactor2.transform.position.z - StartControllerPos.z > 0 ){
                            childs.resizeCube(distance,"z", true );
                        }else{
                            childs.resizeCube(distance,"z", false );
                        }
                        
                        break;
                    
                    case "Left":
                        break;

                    case "Right":
                        if( interactor2.transform.position.x - StartControllerPos.x > 0 ){
                            childs.resizeCube(distance,"x", true );
                        }else{
                            childs.resizeCube(distance,"x", false );
                        }
                        
                        break;

                    case "Back":
                        break;

                    case "Bottom":
                        break;
                }





                Debug.Log("Collider touché : " + hit.collider.name);
                Debug.Log("Normale de la face : " + hit.normal);
                Debug.Log("Direction du controller : " + interactor2.transform.forward);
                // Vector3 controllerDelta = interactor2.transform.position - StartControllerPos;
                // float projection = Vector3.Dot(controllerDelta, hit.normal);
                // Debug.Log(projection);
                // Vector3[] newVertices = (Vector3[])childs.myVertices.Clone();




                // switch(hit.collider.name){
                //     case "Top":
                //         // Vector3[] newTopVertice = childs.myVertices;

                //         // childs.updateVertices();
                //         break;
                    
                //     case "Front":
                //         break;
                    
                //     case "Left":
                //         break;

                //     case "Right":
                //         newVertices[2] += hit.normal * projection;
                //         newVertices[3] += hit.normal * projection;
                //         newVertices[6] += hit.normal * projection;
                //         newVertices[7] += hit.normal * projection;
                //         Vector3[] newRightVertice = new Vector3[4]{
                //             newVertices[2],
                //             newVertices[3],
                //             newVertices[6],
                //             newVertices[7]
                //         };

                //         Debug.Log(newVertices);
                //         Debug.Log(hit.collider.transform.parent.GetComponent<MeshFilter>());

                //         Vector3[] newFrontVertice = new Vector3[4]{
                //             newVertices[0],
                //             newVertices[1],
                //             newVertices[2],
                //             newVertices[3]
                //         };
                //         Debug.Log("iciii");
                //         Debug.Log(newFrontVertice[0]);
                //         Debug.Log(newFrontVertice[1]);
                //         Debug.Log(newFrontVertice[2]);
                //         Debug.Log(newFrontVertice[3]);

                //         Vector3[] newTopVertice = new Vector3[4]{
                //             newVertices[1],
                //             newVertices[3],
                //             newVertices[5],
                //             newVertices[7]
                //         };
                //         Vector3[] newBackVertice = new Vector3[4]{
                //             newVertices[5],
                //             newVertices[7],
                //             newVertices[4],
                //             newVertices[6]
                //         };
                //         Vector3[] newBottomVertice = new Vector3[4]{
                //             newVertices[0],
                //             newVertices[2],
                //             newVertices[4],
                //             newVertices[6]
                //         };
                //         childs.updateBoxCollider(Front.GetComponent<BoxCollider>() as BoxCollider, newFrontVertice);
                //         childs.updateBoxCollider(Top.GetComponent<BoxCollider>() as BoxCollider, newTopVertice);
                //         childs.updateBoxCollider(Back.GetComponent<BoxCollider>() as BoxCollider , newBackVertice);
                //         childs.updateBoxCollider(Bottom.GetComponent<BoxCollider>() as BoxCollider, newBottomVertice);
                //         childs.updateVertices(newVertices,newRightVertice,hit.collider.transform.parent.GetComponent<MeshFilter>() as MeshFilter,hit.collider as BoxCollider);
                        


                //         break;

                //     case "Back":
                //         break;

                //     case "Bottom":
                //         break;
                // }
            

        }
        

    }




    
}
