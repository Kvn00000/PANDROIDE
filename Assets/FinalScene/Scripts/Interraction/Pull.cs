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


    //We change the color when the hand hover entered with the ground (not for the plane)
    protected override void OnHoverEntered(HoverEnterEventArgs interactor)
    {
        topScript.setNewMesh(topScript.mat);
    }


    protected override void OnHoverExited(HoverExitEventArgs interactor){
        topScript.setNewMesh(topScript.originalMat);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        XRBaseInteractor interactor = selectingInteractor; 
        //IXRSelectInteractable interactor = (IXRSelectInteractable)interactorsSelecting;



        if(updatePhase == XRInteractionUpdateOrder.UpdatePhase.Fixed){
            //when trigger is hold we update the vertices
            if(isSelected){
                topScript.updateVertices(interactor.transform.position.y);
            }
        }
    }

}