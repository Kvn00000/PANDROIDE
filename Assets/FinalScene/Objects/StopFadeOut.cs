using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class StopFadeOut : MonoBehaviour
{
    private Coroutine fadeCoroutine;
    private XRGrabInteractable grabInteractable;
    private Renderer renderer;
    private Color color;

    public float elapsedTime;
    public bool isFadingOut;


    public float fadeDuration = 5f;

    void Awake(){
        grabInteractable = GetComponent<XRGrabInteractable>();
        renderer = gameObject.GetComponent<Renderer>();
        color = renderer.material.color;
    }

    void update(){

        if(isFadingOut){
            Debug.Log("diminue le alpha");    
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(color.a, 0f, elapsedTime / fadeDuration);

            if (elapsedTime >= fadeDuration){
                Debug.Log("destroyed normalement");

                isFadingOut = false;
                elapsedTime = 0f;
                Destroy(gameObject);
            }
        }

            

    }
    void OnEnable(){
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    // Start is called before the first frame update
    private void OnGrab(SelectEnterEventArgs args){
        Debug.Log("grab");

        if(isFadingOut == true){
                Debug.Log("cancel le fade out");

            isFadingOut = false;
            elapsedTime = 0f;
            color.a = 1;
        }
        // When object grabbed we stop the fade out and set the alpha to 1
        // Debug.Log("j'ai grab");
        // if(fadeCoroutine != null){
        //     StopCoroutine(fadeCoroutine);
        //     fadeCoroutine = null;
        //     Debug.Log("j'ai stop la coroutine");
        //     Material material = renderer.material;
        //     Color color = material.color;
        //     color.a = 1;
        //     material.color = color;

        //     Debug.Log("je reset le alpha");

        // }


        
    }

    private void decreaseAlpha(){
        
    }

    public void SetCoroutine(Coroutine c){
        fadeCoroutine = c;
    }


}
