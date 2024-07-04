using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class StopCoroutine : MonoBehaviour
{
    private Coroutine fadeCoroutine;
    private XRGrabInteractable grabInteractable;

    void Awake(){
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable(){
        grabInteractable.selectEntered.AddListener(OnGrab);
        }

    // Start is called before the first frame update
    private void OnGrab(SelectEnterEventArgs args){
            // When object grabbed we stop the fade out and set the alpha to 1
            Debug.Log("j'ai grab")
            if(fadeCoroutine != null){
                StopCoroutine(fadeCoroutine);
                Debug.Log("je cancel la coroutine");
                fadeCoroutine = null;

                Renderer renderer = gameObject.GetComponent<Renderer>();
                if (renderer != null){
                    Debug
                    Material material = renderer.material;
                    Color color = material.color;
                    color.a = 1;
                    material.color = color;
                }
            }
        
    }

    public void SetCoroutine(Coroutine c){
        fadeCoroutine = c;
    }

}
