using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class FadeOut : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Renderer renderer;
    private Color color;

    public float elapsedTime; // Temps passé pendant le fade out
    public bool isFadingOut;

    public float initialAlpha = 1f;
    public float fadeDuration = 5f; // 5 secondes pour fade out

    void Awake(){
        grabInteractable = GetComponent<XRGrabInteractable>();
        renderer = gameObject.GetComponent<Renderer>();
        color = renderer.material.color;
    }

    void Update(){

        if(isFadingOut == true){
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(initialAlpha, 0f, elapsedTime / fadeDuration);
            renderer.material.color = color;

            if (elapsedTime >= fadeDuration){
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
        if(isFadingOut == true){
            isFadingOut = false;
            elapsedTime = 0f;
            color.a = 1f;
            renderer.material.color = color;
        }
    }
}
