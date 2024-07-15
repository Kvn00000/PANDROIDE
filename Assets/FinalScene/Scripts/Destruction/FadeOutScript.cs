using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class FadeOut : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private new Renderer renderer;
    private MeshRenderer meshR;
    private Color color;

    public float elapsedTime; // Temps passé pendant le fade out
    public bool isFadingOut;

    public Material OpaqueMat;
    public Material TranparentMat;

    private float initialAlpha = 1f;
    private float fadeDuration = 5f; // 5 secondes pour fade out

    private boidTuning isBoid;

    void Awake(){
        //if isBoid == null then it is a cube
        isBoid = GetComponent<boidTuning>();
        elapsedTime = 0f;
        if(isBoid != null){
            Transform child = transform.Find("paper plane asset").Find("Mesh1_Model");
            renderer = child.gameObject.GetComponent<Renderer>();
            meshR = child.GetComponent<MeshRenderer>();

        }else{
            renderer = gameObject.GetComponent<Renderer>();
            meshR = GetComponent<MeshRenderer>();
        }
        grabInteractable = GetComponent<XRGrabInteractable>();


    }
   
    void Update(){
        
        if(isFadingOut == true){
            //We reduce the alpha based on the fadeDuration

            meshR.material = TranparentMat;
            elapsedTime += Time.deltaTime;
            color = renderer.material.color;
            color.a = Mathf.Lerp(initialAlpha, 0f, elapsedTime / fadeDuration);
            renderer.material.color = color;

            //Destroy the object 
            if (elapsedTime >= fadeDuration){
                isFadingOut = false;
                elapsedTime = 0f;
                Destroy(this.gameObject);
            }
        }
    }
    void OnEnable(){
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args){
        if(isFadingOut == true){
            //Put the opaque material and set the alpha to 1
            meshR.material = OpaqueMat;
            isFadingOut = false;
            elapsedTime = 0f;
            color.a = 1f;
            renderer.material.color = color;
        }
    }
}
