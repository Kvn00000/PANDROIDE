using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class test : XRGrabInteractable
{
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if(updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic){
            if(isSelected){
                translation();
            }
        }
    }

    public void translation(){
        //Up the height of the box but how
    }
}