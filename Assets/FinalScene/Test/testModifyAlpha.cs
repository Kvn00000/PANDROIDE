using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class testModifyAlpha : MonoBehaviour
{
    // Start is called before the first frame update
    private float alphaA;
    private XRGrabInteractable grabInteractable;

    public GameObject cube;
    private Coroutine fadeCoroutine;

    void Awake(){
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Start(){
        fadeCoroutine = StartCoroutine(FadeToZeroAlpha(cube, 5.0f));
    }
    // void update(){
    //     if (Input.GetMouseButtonDown(0)){
    //         if (fadeCoroutine != null)
    //         {
    //             Debug.Log("nejfgknlfd");
    //             StopCoroutine(fadeCoroutine);
    //             fadeCoroutine = null;


    //             Renderer renderer = gameObject.GetComponent<Renderer>();
    //             if (renderer != null){
    //                 Material material = renderer.material;
    //                 Color color = material.color;
    //                 color.a = 1;
    //                 material.color = color;
    //             }
    //         }
    //     }
    // }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args){
            // When object grabbed we stop the fade out and set the alpha to 1
            Debug.Log("jgfghygne");

            if(fadeCoroutine != null){
                Debug.Log("je cancel la coroutine");
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = null;

                Renderer renderer = gameObject.GetComponent<Renderer>();
                if (renderer != null){
                    Material material = renderer.material;
                    Color color = material.color;
                    color.a = 1;
                    material.color = color;
                }
            }
        
    }
    
    IEnumerator FadeToZeroAlpha(GameObject targetObject, float duration)
    {
        Renderer renderer = targetObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = renderer.material;
            Color color = material.color;
            float startAlpha = color.a;

            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                float blend = t / duration;
                color.a = Mathf.Lerp(startAlpha, 0, blend);
                material.color = color;
                yield return null;
            }

            // Assurez-vous que l'alpha est exactement 0 après la fin de la boucle
            Destroy(targetObject);
            color.a = 0;
            material.color = color;
        }
    }
}