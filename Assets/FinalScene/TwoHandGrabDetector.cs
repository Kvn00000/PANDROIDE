using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class TwoHandGrabDetector : MonoBehaviour
{

    public XRRayInteractor xrInteractor;
    private XRGrabInteractable grabInteractable;
    private List<XRBaseInteractor> interactors = new List<XRBaseInteractor>();

    private ChildComponent childs;
    private MeshFilter mesh;

    public InputActionProperty grabAction;


    private XRBaseInteractor interactor2;

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
        if (!interactors.Contains(args.interactor))
        {

            



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
                Debug.Log("Deuxième interactor grabbé : " + interactor2.transform.parent.name);

                xrInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit);
                Debug.Log("Collider touché : " + hit.collider.name);
                Debug.Log("Collider touché : " + hit.normal);

                float rayLength = 5f;

                // Dessinez le rayon à partir du point de contact, dans la direction de la normale
                Debug.DrawRay(hit.point, hit.normal * rayLength, Color.red);
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
   
                
                // switch(hit.collider.name){
                //     case "Top":
                //         Vector3[] newTopVertice = childs.myVertices;

                //         // childs.updateVertices();
                //         break;
                    
                //     case "Front":
                //         break;
                    
                //     case "Left":
                //         break;

                //     case "Right":
                //         break;

                //     case "Back":
                //         break;

                //     case "Bottom":
                //         break;
                // }
            

        }
        

    }




    
}
