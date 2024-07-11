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

    private InitSceneScript initScript;

    private boidTuning isBoid;

    void Awake(){
        grabInteractable = GetComponent<XRGrabInteractable>();
        meshR = GetComponent<MeshRenderer>();
        renderer = gameObject.GetComponent<Renderer>();
        Debug.Log("le mesh renderer : " + meshR);
        Debug.Log("le renderer : "+ renderer);


        //Si isBoid == null alors c'est un cube
        isBoid = GetComponent<boidTuning>();
    }

    void Update(){

        if(isFadingOut == true){
            //On reduit le Alpha selon le temps fadeDuration
            meshR.material = TranparentMat;
            elapsedTime += Time.deltaTime;
            color = renderer.material.color;
            color.a = Mathf.Lerp(initialAlpha, 0f, elapsedTime / fadeDuration);
            renderer.material.color = color;

            //On detruit l'objet au bout de fadeDuration secondes
            if (elapsedTime >= fadeDuration){

                isFadingOut = false;
                elapsedTime = 0f;
                this.transform.position = new Vector3(0,200,0);
                if( isBoid == null ){
                    Destroy(this.gameObject);
                }else{
                    Debug.Log("before destroy boid");
                    initScript.DestroyAndRemove(this.gameObject);
                    Debug.Log("after destroy boid");

                }
            }
        }
    }
    void OnEnable(){
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    // Start is called before the first frame update
    private void OnGrab(SelectEnterEventArgs args){
        if(isFadingOut == true){
            //On remet le Material Opaque et on remet le Alpha a 1
            meshR.material = OpaqueMat;
            isFadingOut = false;
            elapsedTime = 0f;
            color.a = 1f;
            renderer.material.color = color;
        }
    }
    public void setScene(InitSceneScript scene)
    {
        initScript = scene;    
    }
}
