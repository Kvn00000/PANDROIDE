using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DestroyGroundScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Destroyer Initialized");
    }

    public bool withDEBUG = false;
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (withDEBUG)
        {
            Debug.Log("#######################################");
            Debug.Log("Saw Something");
        }
        int wallLayer = LayerMask.NameToLayer("MUR");
        int boidLayer = LayerMask.NameToLayer("BOID");
        int groundArenaLayer = LayerMask.NameToLayer("SOL");
        int collidedLayer = other.gameObject.layer;
        bool isWall = (collidedLayer == wallLayer);
        bool isBoid = (collidedLayer == boidLayer);
        bool isGround = (collidedLayer == groundArenaLayer);
        if (withDEBUG){
            Debug.Log("Collided object is Wall : " + isWall + " Boid : " + isBoid + " Arene Sol : " + isGround);
            if (other.GetComponent<ARPlane>() == null){ Debug.Log("NOT AR PLANE"); }
          
            else{ Debug.Log("IS AN AR PLANE"); }
        }
        Debug.Log(" TAG " + other.gameObject.tag);
        //Check if collided is not a Plane and is on a good layer
        if ( isWall || isBoid  ||isGround || other.gameObject.CompareTag("Destructible"))
        {
            if ((isBoid)|| (other.gameObject.CompareTag("Destructible"))){
                // other.transform.parent.gameObject.GetComponent<StopFadeOut>().SetCoroutine(StartCoroutine(FadeToZeroAlpha(other.transform.parent.gameObject, 5.0f)));
                // Destroy(other.transform.parent.gameObject);
                Debug.Log("Je fade OUT");
                other.transform.parent.gameObject.GetComponent<StopFadeOut>().isFadingOut = true;
                other.transform.parent.gameObject.GetComponent<StopFadeOut>().elapsedTime = 0f;

            }else{ 
                // Destroy(other.gameObject); 
                // other.gameObject.GetComponent<StopFadeOut>().SetCoroutine(StartCoroutine(FadeToZeroAlpha(other.gameObject,5.0f)));
                other.gameObject.GetComponent<StopFadeOut>().isFadingOut = true;
                other.gameObject.GetComponent<StopFadeOut>().elapsedTime = 0f;

            }
        }
        if (withDEBUG){ Debug.Log("#######################################"); }
    }


    // public void StartFade(GameObject targetObject)
    // {
    //     StartCoroutine(FadeToZeroAlpha(targetObject));
    // }

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
                Debug.Log("dans la coroutine");
                yield return null;
            }
            color.a = 0;
            material.color = color;

            Debug.Log("le  alphaa : " + color.a);

            // Destroy(targetObject);

        }
    }
    /*
    private void OnTriggerStay  (Collider other)
    {
        if (withDEBUG)
        {
            Debug.Log("#######################################");
            Debug.Log("Saw Stay Something");
        }
        int wallLayer = LayerMask.NameToLayer("MUR");
        int boidLayer = LayerMask.NameToLayer("BOID");
        int groundArenaLayer = LayerMask.NameToLayer("SOL");
        int collidedLayer = other.gameObject.layer;
        bool isWall = (collidedLayer == wallLayer);
        bool isBoid = (collidedLayer == boidLayer);
        bool isGround = (collidedLayer == groundArenaLayer);
        if (withDEBUG)
        {
            Debug.Log("Collided object is Wall : " + isWall + " Boid : " + isBoid + " Arene Sol : " + isGround);
            if (other.GetComponent<ARPlane>() == null) { Debug.Log("NOT AR PLANE"); }

            else { Debug.Log("IS AN AR PLANE"); }
        }
        Debug.Log(" TAG " + other.gameObject.tag);
        //Check if collided is not a Plane and is on a good layer
        if (isWall || isBoid || isGround || other.gameObject.CompareTag("Destructible"))
        {
            if (isBoid)
            {
                Destroy(other.transform.parent.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }
            GameObject parentPotentiel = this.GetComponentInParent<GameObject>();
            if (parentPotentiel != null)
            {
                Destroy(parentPotentiel);
            }
        }
        if (withDEBUG) { Debug.Log("#######################################"); }
    }
    private void OnTriggerExit(Collider other)
    {
        if (withDEBUG)
        {
            Debug.Log("#######################################");
            Debug.Log("Saw EXIT Something");
        }
        int wallLayer = LayerMask.NameToLayer("MUR");
        int boidLayer = LayerMask.NameToLayer("BOID");
        int groundArenaLayer = LayerMask.NameToLayer("SOL");
        int collidedLayer = other.gameObject.layer;
        bool isWall = (collidedLayer == wallLayer);
        bool isBoid = (collidedLayer == boidLayer);
        bool isGround = (collidedLayer == groundArenaLayer);
        if (withDEBUG)
        {
            Debug.Log("Collided object is Wall : " + isWall + " Boid : " + isBoid + " Arene Sol : " + isGround);
            if (other.GetComponent<ARPlane>() == null) { Debug.Log("NOT AR PLANE"); }

            else { Debug.Log("IS AN AR PLANE"); }
        }
        Debug.Log(" TAG " + other.gameObject.tag);
        //Check if collided is not a Plane and is on a good layer
        if (isWall || isBoid || isGround || other.gameObject.CompareTag("Destructible"))
        {
            if (isBoid)
            {
                Destroy(other.transform.parent.gameObject);
            }
            else
            {
                Destroy(other.gameObject);
            }
            GameObject parentPotentiel = this.GetComponentInParent<GameObject>();
            if (parentPotentiel != null)
            {
                Destroy(parentPotentiel);
            }
        }
        if (withDEBUG) { Debug.Log("#######################################"); }
    }
    */
}

