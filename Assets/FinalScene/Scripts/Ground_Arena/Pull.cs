using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Pull : XRSimpleInteractable
{
    private Top topScript;

    protected override void Awake(){
        base.Awake();
        topScript = GetComponent<Top>();
    }

    //On change la couleur quand la main est en contact avec le sol
    protected override void OnHoverEntered(HoverEnterEventArgs interactor)
    {
        // Debug.Log($"{interactor.interactorObject} hovered over {interactor.interactableObject}", this);
        topScript.setNewMesh(topScript.mat);
    }

    //On remet la couleur initiale quand la main n'est plus en contact
    protected override void OnHoverExited(HoverExitEventArgs interactor){
        // Debug.Log($"{interactor.interactorObject} exit over {interactor.interactableObject}", this);
        topScript.setNewMesh(topScript.originalMat);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        XRBaseInteractor interactor = selectingInteractor;



        if(updatePhase == XRInteractionUpdateOrder.UpdatePhase.Fixed){
            //Quand la gachette est appuyé on grab + update le vertice du sol
            if(isSelected){
                topScript.updateVertices(interactor.transform.position.y);
            }
        }
    }

}