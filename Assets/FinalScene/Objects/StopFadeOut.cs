using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class StopFadeOut : MonoBehaviour
{
    private Coroutine fadeCoroutine;
    private XRGrabInteractable grabInteractable;
    private Renderer renderer;

    void Awake(){
        grabInteractable = GetComponent<XRGrabInteractable>();
        renderer = gameObject.GetComponent<Renderer>();
    }

    void update(){
        if(renderer.material.color.a <= 0.1){
            Destroy(gameObject);
        }
    }
    void OnEnable(){
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    // Start is called before the first frame update
    private void OnGrab(SelectEnterEventArgs args){
            // When object grabbed we stop the fade out and set the alpha to 1
            Debug.Log("j'ai grab");
            if(fadeCoroutine != null){
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = null;
                Debug.Log("j'ai stop la coroutine");
                Material material = renderer.material;
                Color color = material.color;
                color.a = 1;
                material.color = color;

                Debug.Log("je reset le alpha");

            }
        
    }

    public void SetCoroutine(Coroutine c){
        fadeCoroutine = c;
    }


}
