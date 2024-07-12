using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

public class DestroyGroundScript : MonoBehaviour
{
    // Start is called before the first frame update
    /*
    0 --> direct destruction
    1 --> Fade out destruction
    */
    private int _FadeOut;

    private int _wallLayer;
    private int _boidLayer;
    private int _groundArenaLayer;
    public bool withDEBUG = false;

    private void Awake()
    {
        _wallLayer = LayerMask.NameToLayer("MUR");
        _boidLayer = LayerMask.NameToLayer("BOID");
        _groundArenaLayer = LayerMask.NameToLayer("SOL");
        //Debug.Log("Destroyer Initialized " + _FadeOut);

        //Subscribing to change mod event

        if (PlayerPrefs.HasKey("FadeOut"))
        {
            _FadeOut = PlayerPrefs.GetInt("FadeOut");
        }
    }



    private void OnTriggerEnter (Collider other)
    {

        if (withDEBUG)
        {
            Debug.Log("#######################################");
            Debug.Log("Saw Something "+ _FadeOut);

        }
        //Debug.Log(" TAG " + other.gameObject.tag);
        //Check if collided is not a Plane and is on a good layer
        if (isToDestroy(other))
        {
            if ((other.gameObject.layer==_boidLayer)||(other.gameObject.CompareTag("Destructible"))){
                if (_FadeOut == 1) 
                { 
                    other.transform.parent.gameObject.GetComponent<FadeOut>().isFadingOut = true;
                    //other.transform.parent.gameObject.GetComponent<FadeOut>().elapsedTime = 0f;
                    //other.GetComponent<Rigidbody>().isKinematic = true;

                }
                

            }else{
                if (_FadeOut == 1)
                {
                    other.gameObject.GetComponent<FadeOut>().isFadingOut = true;
                    //other.gameObject.GetComponent<FadeOut>().elapsedTime = 0f;
                    //other.GetComponent<Rigidbody>().isKinematic = true;
                }
                

            }
        }
        if (withDEBUG){ Debug.Log("#######################################"); }
    }
    private void OnTriggerStay(Collider other)
    {
        if (withDEBUG)
        {
            Debug.Log("#######################################");
            Debug.Log("Saw Something " + _FadeOut);

        }
        //Debug.Log(" TAG " + other.gameObject.tag);
        //Check if collided is not a Plane and is on a good layer
        if (isToDestroy(other))
        {
            if ((other.gameObject.layer == _boidLayer) || (other.gameObject.CompareTag("Destructible")))
            {
                if (_FadeOut == 1)
                {
                    other.transform.parent.gameObject.GetComponent<FadeOut>().isFadingOut = true;
                    //other.transform.parent.gameObject.GetComponent<FadeOut>().elapsedTime = 0f;
                    //other.GetComponent<Rigidbody>().isKinematic = true;

                }


            }
            else
            {
                if (_FadeOut == 1)
                {
                    other.gameObject.GetComponent<FadeOut>().isFadingOut = true;
                    //other.gameObject.GetComponent<FadeOut>().elapsedTime = 0f;
                    //other.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
        if (withDEBUG) { Debug.Log("#######################################"); }
    }

    private bool isToDestroy(Collider collide)
    {
        int collidedLayer = collide.gameObject.layer;
        bool isWall = collidedLayer==_wallLayer;
        bool isBoid = collidedLayer == _boidLayer;
        bool isGround = collidedLayer == _groundArenaLayer;
        if (withDEBUG)
        {
            Debug.Log("Collided object is Wall : " + isWall + " Boid : " + isBoid + " Arene Sol : " + isGround);
            if (collide.GetComponent<ARPlane>() == null) { Debug.Log("NOT AR PLANE"); }

            else { Debug.Log("IS AN AR PLANE"); }
        }
        return (isWall || isBoid || isGround);
    }



    public void setMod(int value)
    {
        this._FadeOut = value;
    }

}

