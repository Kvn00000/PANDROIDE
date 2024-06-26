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
        }

        
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (interactors.Contains(args.interactor)){
            interactors.Remove(args.interactor);
        }

        if (interactors.Count < 2){
            // Debug.Log(mesh.mesh.vertices.Length);

            // for (int i = 0; i < mesh.mesh.vertices.Length; i++){
            //     Debug.Log($"Vertex {i}: {mesh.mesh.vertices[i]} et en global ::: {transform.TransformPoint(mesh.mesh.vertices[i])}");
            // }
            

            // Debug.Log(mesh.mesh.triangles.Length);
            // for(int i = 0; i < mesh.mesh.triangles.Length; i++){
            //     Debug.Log($"Trianggle {i}: {mesh.mesh.triangles[i]}");
                
            // }

            // Debug.Log("Object is no longer grabbed with both hands.");

            // Your logic when the object is no longer grabbed with both hands
        }
    }

    void Update(){
        if (interactors.Count == 2){
            if (xrInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit)){
                Debug.Log("Collider touché : " + hit.collider.name);

                switch(hit.collider.name){
                    case "Top":
                        Vector3[] newTopVertice = childs.myVertices;

                        // childs.updateVertices();
                        break;
                    
                    case "Front":
                        break;
                    
                    case "Left":
                        break;

                    case "Right":
                        break;

                    case "Back":
                        break;

                    case "Bottom":
                        break;
                }
            }

        }
        

    }


    
}
