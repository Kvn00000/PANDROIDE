using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Pull : XRSimpleInteractable
{


    private float init_height;

    private Top topScript;
    // protected override void Awake()
    // {
    //     base.Awake();

    // }


    [System.Obsolete]
    protected override void Awake(){
        base.Awake();
        topScript = GetComponent<Top>();
    }

    protected override void OnHoverEntered(HoverEnterEventArgs interactor)
    {
        // Debug.Log($"{interactor.interactorObject} hovered over {interactor.interactableObject}", this);
        topScript.setNewMesh(topScript.mat);
    }

    protected override void OnHoverExited(HoverExitEventArgs interactor){
        // Debug.Log($"{interactor.interactorObject} exit over {interactor.interactableObject}", this);
        topScript.setNewMesh(topScript.originalMat);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        XRBaseInteractor interactor = selectingInteractor;
        if(updatePhase == XRInteractionUpdateOrder.UpdatePhase.Fixed){
            if(isSelected){
                topScript.updateVertices(interactor.transform.position.y);

            }
        }
    }

}