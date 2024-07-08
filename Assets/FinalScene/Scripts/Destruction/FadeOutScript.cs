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

    public Material mat;
    private float initialAlpha = 1f;
    private float fadeDuration = 5f; // 5 secondes pour fade out

    void Awake(){
        grabInteractable = GetComponent<XRGrabInteractable>();
        renderer = gameObject.GetComponent<Renderer>();
        color = renderer.material.color;
    }

    void Update(){

        if(isFadingOut == true){
           
            Debug.Log("_______________________");
            Debug.Log("BEFORE "+mat.GetFloat("_Mode"));
            mat.SetFloat("_Mode", 3);
            Debug.Log("AFTER "+ mat.GetFloat("_Mode"));
            Debug.Log("_______________________");
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(initialAlpha, 0f, elapsedTime / fadeDuration);
            renderer.material.color = color;

            //On detruit l'objet au bout de fadeDuration secondes
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
            mat.SetFloat("_Mode", 0);
            isFadingOut = false;
            elapsedTime = 0f;
            color.a = 1f;
            renderer.material.color = color;
        }
    }
}
