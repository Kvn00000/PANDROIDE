using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandGrabDetector : MonoBehaviour
{

    public XRRayInteractor xrInteractor;
    private XRGrabInteractable grabInteractable;
    private List<XRBaseInteractor> interactors = new List<XRBaseInteractor>();

    private MeshFilter mesh;

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
        if (!interactors.Contains(args.interactor))
        {
            interactors.Add(args.interactor);
        }

        if (interactors.Count == 2){
            Debug.Log("Object grabbed with both hands.");
            
            // foreach (var collider in args.interactable.colliders)
            // {
            //     string colliderName = collider.name;
            //     Debug.Log("Collider grabbé : " + colliderName);
                
            // }
            // Debug.Log("???????");

            // XRBaseInteractable interactable = args.interactable;

            // // Parcours des colliders de l'interactable
            // foreach (var collider in interactable.colliders)
            // {
            //     // Vérification si le collider est touché par le rayon du XR Ray Interactor
            //     RaycastHit hitInfo;
            //     bool isHit = xrInteractor.GetCurrentRaycastHit(out hitInfo);

            //     if (isHit && hitInfo.collider == collider)
            //     {
            //         Debug.Log("Collider en collision avec le ray : " + collider.name);
            //         // Faites quelque chose avec le collider s'il est touché par le rayon
            //     }
            // }


            if (xrInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                // Affichez le nom du collider touché
                Debug.Log("Collider touché : " + hit.collider.name);

                // Affichez le nom de l'objet parent du collider touché
                Debug.Log("Parent de l'objet touché : " + hit.collider.transform.parent.name);
            }

        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        if (interactors.Contains(args.interactor)){
            interactors.Remove(args.interactor);
        }

        if (interactors.Count < 2){
            Debug.Log(mesh.mesh.vertices.Length);

            for (int i = 0; i < mesh.mesh.vertices.Length; i++){
                Debug.Log($"Vertex {i}: {mesh.mesh.vertices[i]} et en global ::: {transform.TransformPoint(mesh.mesh.vertices[i])}");
            }
            

            Debug.Log(mesh.mesh.triangles.Length);
            for(int i = 0; i < mesh.mesh.triangles.Length; i++){
                Debug.Log($"Trianggle {i}: {mesh.mesh.triangles[i]}");
                
            }

            Debug.Log("Object is no longer grabbed with both hands.");

            // Your logic when the object is no longer grabbed with both hands
        }
    }


    private void DetectFace(XRRayInteractor rayInteractor)
    {
        if (rayInteractor.TryGetHitInfo(out Vector3 hitPosition, out Vector3 hitNormal, out int _, out bool _))
        {
            string face = GetCubeFace(transform.TransformDirection(hitNormal));
            Debug.Log("Face touchée : " + face);
        }
    }

    private string GetCubeFace(Vector3 hitNormal)
    {
        if (hitNormal == Vector3.up)
            return "Haut";
        else if (hitNormal == Vector3.down)
            return "Bas";
        else if (hitNormal == Vector3.left)
            return "Gauche";
        else if (hitNormal == Vector3.right)
            return "Droite";
        else if (hitNormal == Vector3.forward)
            return "Devant";
        else if (hitNormal == Vector3.back)
            return "Derrière";
        else
            return "Inconnue " + hitNormal;
    }
}
