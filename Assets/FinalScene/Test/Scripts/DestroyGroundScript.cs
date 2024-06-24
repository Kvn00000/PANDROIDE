using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DestroyGroundScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Destroyer Initialized");
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
        if (withDEBUG)
        {
            Debug.Log("Collided object is Wall : " + isWall + " Boid : " + isBoid + " Arene Sol : " + isGround);
            if (other.GetComponent<ARPlane>() == null){ Debug.Log("NOT AR PLANE"); }
          
            else{ Debug.Log("IS AN AR PLANE"); }
        }
        //Check if collided is not a Plane and is on a good layer
        if ( isWall || isBoid  ||isGround)
        {
            if (isBoid)
            {
                Destroy(other.transform.parent.gameObject);
            }
            else 
            { 
                Destroy(other.gameObject); 
            }
        }
        if (withDEBUG){ Debug.Log("#######################################"); }
    }
    /*
    private void OnTriggerStay  (Collider other)
    {
        Debug.Log("Saw Something");
        int wallLayer = LayerMask.NameToLayer("MUR");
        int boidLayer = LayerMask.NameToLayer("BOID");
        int groundArenaLayer = LayerMask.NameToLayer("SOL");
        int collidedLayer = other.gameObject.layer;
        //Check if collided is not a Plane and is on a good layer
        if ((collidedLayer == wallLayer) || (collidedLayer == boidLayer) || (collidedLayer == groundArenaLayer))
        {
            foreach (var childTransform in GetComponentsInChildren<Transform>())
            {
                Destroy(childTransform.gameObject);
            }
            Destroy(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Saw Something");
        int wallLayer = LayerMask.NameToLayer("MUR");
        int boidLayer = LayerMask.NameToLayer("BOID");
        int groundArenaLayer = LayerMask.NameToLayer("SOL");
        int collidedLayer = other.gameObject.layer;
        //Check if collided is not a Plane and is on a good layer
        if ((collidedLayer == wallLayer) || (collidedLayer == boidLayer) || (collidedLayer == groundArenaLayer))
        {
            foreach (var childTransform in GetComponentsInChildren<Transform>())
            {
                Destroy(childTransform.gameObject);
            }
            Destroy(other);
        }
    }
    */
}

