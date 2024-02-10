using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class AnimatedHandOnInput : MonoBehaviour{
    // Capture boutton avant de la manette
    public InputActionProperty pinchAnimationAction;
    // Capture boutton lateral manette
    public InputActionProperty gripAnimationAction;
    //Permet de linker l'animation et de parameter les animations
    public Animator handAnimator;

   

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        //get value du boutton avant presse
        float tvalue = pinchAnimationAction.action.ReadValue<float>();
        //Gere l'animation
        handAnimator.SetFloat("Trigger",tvalue);

        //get value grip boutton
        float gValue = gripAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Grip",gValue);

    }
}
